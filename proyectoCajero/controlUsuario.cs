using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static proyectoCajero.archivosTxt;
using Microsoft.Data.SqlClient;

namespace proyectoCajero
{
    public partial class controlUsuario : Form
    {
        private List<Usuario> listaUsuarios = new List<Usuario>();
        private List<Transaccion> listaTransacciones = new List<Transaccion>();

        // Binding for user info label
        private BindingSource _bsUsuario;
        private UserInfoView _userInfoView;

        public controlUsuario()
        {
            InitializeComponent();

            // Initialize binding objects
            _bsUsuario = new BindingSource();
            _userInfoView = new UserInfoView { Info = string.Empty };
            _bsUsuario.DataSource = _userInfoView;

            // Bind the label's Text to the view model's Info property
            lblInfoUsuario.DataBindings.Clear();
            lblInfoUsuario.DataBindings.Add("Text", _bsUsuario, "Info", false, DataSourceUpdateMode.OnPropertyChanged);
        }

        private class UserInfoView
        {
            public string Info { get; set; }
        }

        public async void controlUsuario_Load(object sender, EventArgs e)
        {
            // Cargar usuarios desde archivos como antes (seguimos usando la lista para nombres locales)
            try
            {
                string pathUsuarios = direccione.obtenerRutasTxt("usuario.txt");
                var desdeArchivo = ManejadorArchivosUsuario.LeerUsuarios(pathUsuarios);
                if (desdeArchivo != null) listaUsuarios = desdeArchivo;
            }
            catch
            {
                listaUsuarios = new List<Usuario>();
            }

            // Intentar cargar tipos desde BD para cmbTipoOperacion
            try
            {
                var conexion = new ConexionBd();
                var tipos = await conexion.GetTiposTransaccionAsync(); // returns (Codigo, Nombre)
                cmbTipoOperacion.Items.Clear();
                cmbTipoOperacion.Items.Add("Ambos");
                foreach (var t in tipos)
                {
                    cmbTipoOperacion.Items.Add(t.Nombre);
                }
                cmbTipoOperacion.SelectedIndex = 0;
            }
            catch
            {
                // fallback: already defined in Designer
            }

            try
            {
                var conexion = new ConexionBd();

                // Obtener MAX(FechaHora) como objeto y convertir a DateTime? para evitar InvalidCast
                const string sqlMax = "SELECT MAX(FechaHora) FROM Transaccion";
                object? maxObj = await conexion.ExecuteScalarAsync<object>(sqlMax);
                DateTime? maxFecha = null;
                if (maxObj != null && maxObj != DBNull.Value)
                {
                    maxFecha = Convert.ToDateTime(maxObj);
                }

                DateTime diaAConsultar = maxFecha?.Date ?? DateTime.Today;

                const string sqlTx = @"SELECT tr.FechaHora, tt.Nombre AS Tipo, t.NumeroTarjeta, tr.Monto
FROM Transaccion tr
INNER JOIN Tarjeta t ON tr.TarjetaID = t.TarjetaID
INNER JOIN TipoTransaccion tt ON tr.TipoTransaccionID = tt.TipoTransaccionID
WHERE CAST(tr.FechaHora AS DATE) = @fecha
ORDER BY tr.FechaHora DESC";

                var parametros = new List<SqlParameter>
                {
                    new SqlParameter("@fecha", SqlDbType.Date) { Value = diaAConsultar }
                };

                var transDb = await conexion.QueryAsync(sqlTx, reader =>
                {
                    var tx = new Transaccion();
                    if (!reader.IsDBNull(reader.GetOrdinal("FechaHora"))) tx.FechaHora = reader.GetDateTime(reader.GetOrdinal("FechaHora"));
                    var tipoStr = !reader.IsDBNull(reader.GetOrdinal("Tipo")) ? reader.GetString(reader.GetOrdinal("Tipo")) : string.Empty;
                    if (!string.IsNullOrEmpty(tipoStr) && Enum.TryParse<TipoTransaccion>(tipoStr, true, out var parsed)) tx.Tipo = parsed;
                    tx.NumeroTarjeta = !reader.IsDBNull(reader.GetOrdinal("NumeroTarjeta")) ? reader.GetString(reader.GetOrdinal("NumeroTarjeta")) : string.Empty;
                    tx.Monto = !reader.IsDBNull(reader.GetOrdinal("Monto")) ? reader.GetDecimal(reader.GetOrdinal("Monto")) : 0m;
                    return tx;
                }, parametros);

                listaTransacciones = (transDb != null && transDb.Any()) ? transDb : ManejadorArchivosTransaccion.LeerTransacciones(direccione.obtenerRutasTxt("transacciones.txt")) ?? new List<Transaccion>();

                if (listaTransacciones == null || !listaTransacciones.Any())
                {
                    lblControlDiario.Text = "No hay transacciones registradas para mostrar estadísticas.";
                    return;
                }

                // Mostrar estadísticas resumidas en el panel derecho
                var fechaMax = listaTransacciones.Max(t => t.FechaHora).Date;
                var transDbToday = listaTransacciones.Where(t => t.FechaHora.Date == fechaMax).ToList();

                decimal totalRetiradoHoy = transDbToday.Where(t => t.Tipo == TipoTransaccion.Retiro).Sum(t => t.Monto);
                decimal totalDepositadoHoy = transDbToday.Where(t => t.Tipo == TipoTransaccion.Deposito).Sum(t => t.Monto);
                decimal neto = totalDepositadoHoy - totalRetiradoHoy;
                var depositosHoy = transDbToday.Where(t => t.Tipo == TipoTransaccion.Deposito);
                decimal promedioDepositado = depositosHoy.Any() ? depositosHoy.Average(t => t.Monto) : 0m;

                var ultimaTransaccion = transDbToday.OrderByDescending(t => t.FechaHora).First();
                var ultimoUsuario = listaUsuarios.FirstOrDefault(u => u.NumeroTarjeta == ultimaTransaccion.NumeroTarjeta);
                string nombreUltimoUsuario = ultimoUsuario?.Nombre ?? "Desconocido";

                var topRetiradores = transDbToday
                    .Where(t => t.Tipo == TipoTransaccion.Retiro)
                    .GroupBy(t => t.NumeroTarjeta)
                    .Select(g => new { NumeroTarjeta = g.Key, MontoTotal = g.Sum(x => x.Monto), Count = g.Count() })
                    .OrderByDescending(x => x.MontoTotal)
                    .Take(10)
                    .ToList();

                var topDepositantes = transDbToday
                    .Where(t => t.Tipo == TipoTransaccion.Deposito)
                    .GroupBy(t => t.NumeroTarjeta)
                    .Select(g => new { NumeroTarjeta = g.Key, MontoTotal = g.Sum(x => x.Monto), Count = g.Count() })
                    .OrderByDescending(x => x.MontoTotal)
                    .Take(10)
                    .ToList();

                var sb = new StringBuilder();
                sb.AppendLine($"Estadísticas para: {fechaMax:d}");
                sb.AppendLine($"Total retirado (salió): Q{totalRetiradoHoy:N2}");
                sb.AppendLine($"Total depositado (entró): Q{totalDepositadoHoy:N2}");
                sb.AppendLine($"Capital neto (entradas - salidas): Q{neto:N2}");
                sb.AppendLine($"Promedio depósito (hoy): Q{promedioDepositado:N2}");
                sb.AppendLine($"Último usuario: {nombreUltimoUsuario} ({ultimaTransaccion.NumeroTarjeta}) a las {ultimaTransaccion.FechaHora:T}");
                sb.AppendLine();
                sb.AppendLine("Top 10 retiradores (por monto):");
                int pos = 1;
                foreach (var r in topRetiradores)
                {
                    var u = listaUsuarios.FirstOrDefault(x => x.NumeroTarjeta == r.NumeroTarjeta);
                    string nombre = u?.Nombre ?? r.NumeroTarjeta;
                    sb.AppendLine($"{pos}. {nombre} - Q{r.MontoTotal:N2} ({r.Count} operaciones)");
                    pos++;
                }
                sb.AppendLine();
                sb.AppendLine("Top 10 depositantes (por monto):");
                pos = 1;
                foreach (var d in topDepositantes)
                {
                    var u = listaUsuarios.FirstOrDefault(x => x.NumeroTarjeta == d.NumeroTarjeta);
                    string nombre = u?.Nombre ?? d.NumeroTarjeta;
                    sb.AppendLine($"{pos}. {nombre} - Q{d.MontoTotal:N2} ({d.Count} operaciones)");
                    pos++;
                }

                lblControlDiario.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                lblControlDiario.Text = "Error al cargar datos: " + ex.Message;
            }
        }

        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            string nombreBusqueda = txtBuscarUsuario.Text?.Trim();

            // If textbox empty, clear bindings
            if (string.IsNullOrWhiteSpace(nombreBusqueda))
            {
                _userInfoView.Info = string.Empty;
                _bsUsuario.ResetBindings(false);
                lblStatus.Text = string.Empty;
                dgvResultados.DataSource = null;
                return;
            }

            // Evitar búsquedas por número de tarjeta en el cuadro de búsqueda de nombre
            if (nombreBusqueda.Any(char.IsDigit))
            {
                _userInfoView.Info = "No use números en esta búsqueda. Para buscar por tarjeta use el filtro de tarjeta.";
                _bsUsuario.ResetBindings(false);
                return;
            }

            // Buscar por nombre parcial (case-insensitive) en lista local
            var usuario = listaUsuarios.FirstOrDefault(u => !string.IsNullOrEmpty(u.Nombre) && u.Nombre.IndexOf(nombreBusqueda, StringComparison.OrdinalIgnoreCase) >= 0);

            // Si no está en la lista local, intentar buscar en la BD por nombre parcial
            if (usuario == null)
            {
                try
                {
                    var conexion = new ConexionBd();
                    const string sqlUser = @"SELECT TOP(1) u.UsuarioID, (u.Nombres + ' ' + u.Apellidos) AS Nombre, t.NumeroTarjeta, c.SaldoActual, c.MontoMaximoRetiroDiario
FROM Usuario u
INNER JOIN Cuenta c ON u.UsuarioID = c.UsuarioID
INNER JOIN Tarjeta t ON c.CuentaID = t.CuentaID
WHERE (u.Nombres + ' ' + u.Apellidos) LIKE @name";

                    var parametrosUser = new List<SqlParameter>
                    {
                        new SqlParameter("@name", SqlDbType.NVarChar) { Value = "%" + nombreBusqueda + "%" }
                    };

                    var lista = await conexion.QueryAsync(sqlUser, reader =>
                    {
                        var u = new Usuario();
                        u.Id = reader.IsDBNull(reader.GetOrdinal("UsuarioID")) ? 0 : reader.GetInt32(reader.GetOrdinal("UsuarioID"));
                        u.Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre"));
                        u.NumeroTarjeta = reader.IsDBNull(reader.GetOrdinal("NumeroTarjeta")) ? string.Empty : reader.GetString(reader.GetOrdinal("NumeroTarjeta"));
                        u.SaldoActual = reader.IsDBNull(reader.GetOrdinal("SaldoActual")) ? 0m : reader.GetDecimal(reader.GetOrdinal("SaldoActual"));
                        u.MontoMaximoDiario = reader.IsDBNull(reader.GetOrdinal("MontoMaximoRetiroDiario")) ? 0m : reader.GetDecimal(reader.GetOrdinal("MontoMaximoRetiroDiario"));
                        return u;
                    }, parametrosUser);

                    if (lista != null && lista.Any())
                    {
                        usuario = lista.First();
                        // avoid duplicates by NumeroTarjeta
                        if (!string.IsNullOrEmpty(usuario.NumeroTarjeta) && !listaUsuarios.Any(x => x.NumeroTarjeta == usuario.NumeroTarjeta))
                        {
                            listaUsuarios.Add(usuario);
                        }
                    }
                }
                catch
                {
                    // ignore DB lookup errors here; we'll show not found below
                }
            }

            if (usuario == null)
            {
                _userInfoView.Info = "Usuario no encontrado.";
                _bsUsuario.ResetBindings(false);
                dgvResultados.DataSource = null;
                return;
            }

            DateTime fecha = dtpFecha.Value.Date;

            // Apply same type filter as the tarjeta path
            string tipoSeleccionado = cmbTipoOperacion.SelectedItem?.ToString();

            // Determine which transaction types to include
            Func<Transaccion, bool> tipoFilter = t => true;
            if (!string.IsNullOrEmpty(tipoSeleccionado) && tipoSeleccionado != "Ambos")
            {
                if (tipoSeleccionado.Equals("Retiro", StringComparison.OrdinalIgnoreCase))
                    tipoFilter = t => t.Tipo == TipoTransaccion.Retiro;
                else if (tipoSeleccionado.Equals("Deposito", StringComparison.OrdinalIgnoreCase))
                    tipoFilter = t => t.Tipo == TipoTransaccion.Deposito;
            }

            // Obtener totales por tipo para ese usuario/fecha; primero intentar desde listaTransacciones
            decimal retiradoHoy = 0m;
            decimal depositadoHoy = 0m;

            var txsLocal = listaTransacciones?.Where(t => t.NumeroTarjeta == usuario.NumeroTarjeta && t.FechaHora.Date == fecha).ToList();
            if (txsLocal != null && txsLocal.Any())
            {
                retiradoHoy = txsLocal.Where(t => t.Tipo == TipoTransaccion.Retiro).Sum(t => t.Monto);
                depositadoHoy = txsLocal.Where(t => t.Tipo == TipoTransaccion.Deposito).Sum(t => t.Monto);
            }
            else
            {
                // Consultar DB para totales si no están en cache
                try
                {
                    var conexion = new ConexionBd();
                    const string sqlTotals = @"SELECT tt.Nombre AS Tipo, SUM(tr.Monto) AS MontoTotal
FROM Transaccion tr
INNER JOIN Tarjeta t ON tr.TarjetaID = t.TarjetaID
INNER JOIN TipoTransaccion tt ON tr.TipoTransaccionID = tt.TipoTransaccionID
WHERE t.NumeroTarjeta = @num AND CAST(tr.FechaHora AS DATE) = @fecha
GROUP BY tt.Nombre";

                    var parametrosTotals = new List<SqlParameter>
                    {
                        new SqlParameter("@num", SqlDbType.NVarChar) { Value = usuario.NumeroTarjeta },
                        new SqlParameter("@fecha", SqlDbType.Date) { Value = fecha }
                    };

                    var totals = await conexion.QueryAsync(sqlTotals, reader => new
                    {
                        Tipo = reader.IsDBNull(reader.GetOrdinal("Tipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("Tipo")),
                        MontoTotal = reader.IsDBNull(reader.GetOrdinal("MontoTotal")) ? 0m : reader.GetDecimal(reader.GetOrdinal("MontoTotal"))
                    }, parametrosTotals);

                    foreach (var t in totals)
                    {
                        if (t.Tipo.Equals("Retiro", StringComparison.OrdinalIgnoreCase)) retiradoHoy = t.MontoTotal;
                        if (t.Tipo.Equals("Deposito", StringComparison.OrdinalIgnoreCase)) depositadoHoy = t.MontoTotal;
                    }
                }
                catch
                {
                    // ignore errors, totals remain 0
                }
            }

            var ultimaTransaccionUsuario = listaTransacciones != null && listaTransacciones.Any()
                ? listaTransacciones.Where(t => t.NumeroTarjeta == usuario.NumeroTarjeta).OrderByDescending(t => t.FechaHora).FirstOrDefault()
                : null;

            string ultimoAccesoStr = ultimaTransaccionUsuario == null ? "Nunca" : ultimaTransaccionUsuario.FechaHora.ToString();

            // Build info according to selected type: if specific type selected, show only that total, otherwise both
            var infoSb = new StringBuilder();
            infoSb.AppendLine($"Información de: {usuario.Nombre}\n");
            infoSb.AppendLine($"Saldo Actual: Q{usuario.SaldoActual:N2}");
            infoSb.AppendLine($"Monto Máximo de Retiro Diario: Q{usuario.MontoMaximoDiario:N2}");

            if (tipoSeleccionado == null || tipoSeleccionado == "Ambos")
            {
                infoSb.AppendLine($"Retirado en {fecha:d}: Q{retiradoHoy:N2}");
                infoSb.AppendLine($"Depositado en {fecha:d}: Q{depositadoHoy:N2}");
            }
            else if (tipoSeleccionado.Equals("Retiro", StringComparison.OrdinalIgnoreCase))
            {
                infoSb.AppendLine($"Retirado en {fecha:d}: Q{retiradoHoy:N2}");
            }
            else if (tipoSeleccionado.Equals("Deposito", StringComparison.OrdinalIgnoreCase))
            {
                infoSb.AppendLine($"Depositado en {fecha:d}: Q{depositadoHoy:N2}");
            }

            infoSb.AppendLine($"Último Acceso Registrado: {ultimoAccesoStr}");

            _userInfoView.Info = infoSb.ToString();
            _bsUsuario.ResetBindings(false);

            // Also populate dgvResultados with user's transactions for that date filtered by tipoFilter
            var detalles = (listaTransacciones ?? new List<Transaccion>())
                .Where(t => t.NumeroTarjeta == usuario.NumeroTarjeta && t.FechaHora.Date == fecha)
                .Where(tipoFilter)
                .OrderByDescending(t => t.FechaHora)
                .Select(t => new { Fecha = t.FechaHora, Tipo = t.Tipo.ToString(), Monto = $"Q{t.Monto:N2}" })
                .ToList();

            // Apply top mode selection to the list (Ninguno, Top 10 posición, Top 10 monto, Top 10 cantidad)
            string topMode = cmbTopMode.SelectedItem?.ToString();
            if (topMode == null || topMode == "Ninguno")
            {
                dgvResultados.DataSource = detalles;
            }
            else if (topMode == "Top 10 posición" || topMode == "Top 10 monto")
            {
                // Top by monto
                var agrupado = detalles
                    .GroupBy(d => d.Monto) // monto string; better to regroup from listaTransacciones
                    .Select(g => g.First())
                    .Take(10)
                    .ToList();
                dgvResultados.DataSource = agrupado;
            }
            else if (topMode == "Top 10 cantidad")
            {
                // Top by count of transactions (only one user so count equals detalles.Count)
                dgvResultados.DataSource = detalles.Take(10).ToList();
            }
        }

        private async void btnAplicarFiltro_Click(object sender, EventArgs e)
        {
            try
            {
                var conexion = new ConexionBd();
                DateTime fecha = dtpFecha.Value.Date;
                string tipo = cmbTipoOperacion.SelectedItem?.ToString();
                string tarjeta = string.IsNullOrWhiteSpace(txtFiltroTarjeta.Text) ? null : txtFiltroTarjeta.Text.Trim();

                // Obtener tipoCodigo si no 'Ambos'
                string tipoCodigo = null;
                if (!string.IsNullOrEmpty(tipo) && tipo != "Ambos")
                {
                    var tipos = await conexion.GetTiposTransaccionAsync();
                    var match = tipos.FirstOrDefault(t => t.Nombre.Equals(tipo, StringComparison.OrdinalIgnoreCase));
                    tipoCodigo = string.IsNullOrEmpty(match.Codigo) ? null : match.Codigo;
                }

                string sql = @"SELECT c.CuentaID, u.UsuarioID, (u.Nombres + ' ' + u.Apellidos) AS Nombre, t.NumeroTarjeta,
COUNT(*) AS Cantidad, SUM(tr.Monto) AS MontoTotal
FROM Transaccion tr
INNER JOIN Tarjeta t ON tr.TarjetaID = t.TarjetaID
INNER JOIN Cuenta c ON tr.CuentaID = c.CuentaID
INNER JOIN Usuario u ON c.UsuarioID = u.UsuarioID
INNER JOIN TipoTransaccion tt ON tr.TipoTransaccionID = tt.TipoTransaccionID
WHERE CAST(tr.FechaHora AS DATE) = @fecha";

                var parametros = new List<SqlParameter>
                {
                    new SqlParameter("@fecha", SqlDbType.Date) { Value = fecha }
                };

                if (!string.IsNullOrEmpty(tipoCodigo))
                {
                    sql += " AND tt.Codigo = @codigo";
                    parametros.Add(new SqlParameter("@codigo", SqlDbType.NVarChar) { Value = tipoCodigo });
                }

                if (!string.IsNullOrEmpty(tarjeta))
                {
                    if (!tarjeta.All(char.IsDigit))
                    {
                        lblStatus.Text = "El filtro de tarjeta solo acepta dígitos.";
                        return;
                    }

                    sql += " AND t.NumeroTarjeta LIKE @tarj";
                    parametros.Add(new SqlParameter("@tarj", SqlDbType.NVarChar) { Value = "%" + tarjeta + "%" });
                }

                sql += " GROUP BY c.CuentaID, u.UsuarioID, u.Nombres, u.Apellidos, t.NumeroTarjeta";

                if (cmbTopMode.SelectedItem?.ToString() == "Top 10 cantidad")
                {
                    sql += " ORDER BY Cantidad DESC";
                }
                else // default order by monto
                {
                    sql += " ORDER BY MontoTotal DESC";
                }

                var lista = await conexion.QueryAsync(sql, reader => new
                {
                    CuentaID = reader.GetInt32(reader.GetOrdinal("CuentaID")),
                    UsuarioID = reader.GetInt32(reader.GetOrdinal("UsuarioID")),
                    Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                    NumeroTarjeta = reader.GetString(reader.GetOrdinal("NumeroTarjeta")),
                    Cantidad = reader.GetInt32(reader.GetOrdinal("Cantidad")),
                    MontoTotal = reader.GetDecimal(reader.GetOrdinal("MontoTotal"))
                }, parametros);

                dgvResultados.DataSource = lista.Select((x, idx) => new
                {
                    Posicion = idx + 1,
                    x.Nombre,
                    NumeroTarjeta = MaskCard(x.NumeroTarjeta),
                    x.CuentaID,
                    x.Cantidad,
                    MontoTotal = $"Q{x.MontoTotal:N2}"
                }).Take(10).ToList();

                lblStatus.Text = lista.Any() ? "" : "No hay resultados para los filtros aplicados.";
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error al aplicar filtros: " + ex.Message;
            }
        }

        private string MaskCard(string card)
        {
            if (string.IsNullOrEmpty(card)) return string.Empty;
            if (card.Length <= 4) return card;
            return new string('*', Math.Max(0, card.Length - 4)) + card.Substring(card.Length - 4);
        }

        private void dgvResultados_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvResultados.CurrentRow == null)
            {
                _userInfoView.Info = string.Empty;
                _bsUsuario.ResetBindings(false);
                return;
            }

            var data = dgvResultados.CurrentRow.DataBoundItem;
            if (data == null)
            {
                _userInfoView.Info = string.Empty;
                _bsUsuario.ResetBindings(false);
                return;
            }

            // data may be anonymous type from ApplyFilter or user transactions; handle common fields
            string nombre = string.Empty;
            string tarjeta = string.Empty;

            var type = data.GetType();
            var propNombre = type.GetProperty("Nombre");
            if (propNombre != null) nombre = propNombre.GetValue(data)?.ToString() ?? string.Empty;
            var propTarj = type.GetProperty("NumeroTarjeta");
            if (propTarj != null) tarjeta = propTarj.GetValue(data)?.ToString() ?? string.Empty;

            if (string.IsNullOrEmpty(tarjeta) && !string.IsNullOrEmpty(nombre))
            {
                // try to find tarjeta from listaUsuarios by name
                var user = listaUsuarios.FirstOrDefault(u => !string.IsNullOrEmpty(u.Nombre) && u.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase));
                if (user != null) tarjeta = user.NumeroTarjeta;
            }

            if (string.IsNullOrEmpty(tarjeta))
            {
                _userInfoView.Info = string.Empty;
                _bsUsuario.ResetBindings(false);
                return;
            }

            // Fill info for tarjeta (totals for selected date)
            DateTime fecha = dtpFecha.Value.Date;
            decimal retirado = listaTransacciones.Where(t => t.NumeroTarjeta == tarjeta && t.FechaHora.Date == fecha && t.Tipo == TipoTransaccion.Retiro).Sum(t => t.Monto);
            decimal depositado = listaTransacciones.Where(t => t.NumeroTarjeta == tarjeta && t.FechaHora.Date == fecha && t.Tipo == TipoTransaccion.Deposito).Sum(t => t.Monto);

            var usuarioEncontrado = listaUsuarios.FirstOrDefault(u => u.NumeroTarjeta == tarjeta);
            string nombreMostrar = usuarioEncontrado?.Nombre ?? nombre;

            _userInfoView.Info = $"Información de: {nombreMostrar}\n\nSaldo Actual: Q{usuarioEncontrado?.SaldoActual:N2 ?? 0:N2}\nRetirado en {fecha:d}: Q{retirado:N2}\nDepositado en {fecha:d}: Q{depositado:N2}";
            _bsUsuario.ResetBindings(false);
        }

        private void txtFiltroTarjeta_TextChanged(object sender, EventArgs e)
        {
            string tarjeta = txtFiltroTarjeta.Text?.Trim();
            if (string.IsNullOrEmpty(tarjeta))
            {
                _userInfoView.Info = string.Empty;
                _bsUsuario.ResetBindings(false);
                return;
            }

            // Only digits allowed; otherwise ignore
            if (!tarjeta.All(char.IsDigit))
            {
                lblStatus.Text = "El filtro de tarjeta solo acepta dígitos.";
                return;
            }

            // Find user by tarjeta and show info
            var usuario = listaUsuarios.FirstOrDefault(u => u.NumeroTarjeta != null && u.NumeroTarjeta.Contains(tarjeta));
            if (usuario == null)
            {
                _userInfoView.Info = "Usuario no encontrado por tarjeta.";
                _bsUsuario.ResetBindings(false);
                return;
            }

            DateTime fecha = dtpFecha.Value.Date;
            decimal retirado = listaTransacciones.Where(t => t.NumeroTarjeta == usuario.NumeroTarjeta && t.FechaHora.Date == fecha && t.Tipo == TipoTransaccion.Retiro).Sum(t => t.Monto);
            decimal depositado = listaTransacciones.Where(t => t.NumeroTarjeta == usuario.NumeroTarjeta && t.FechaHora.Date == fecha && t.Tipo == TipoTransaccion.Deposito).Sum(t => t.Monto);

            _userInfoView.Info = $"Información de: {usuario.Nombre}\n\nSaldo Actual: Q{usuario.SaldoActual:N2}\nRetirado en {fecha:d}: Q{retirado:N2}\nDepositado en {fecha:d}: Q{depositado:N2}";
            _bsUsuario.ResetBindings(false);
        }

        private void txtBuscarUsuario_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
