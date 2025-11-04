using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace proyectoCajero
{
    public partial class LoginUsuario : Form
    {
        public Usuario UsuarioAutenticado { get; private set; }
        public LoginUsuario()
        {
            InitializeComponent();
        }

        private async Task<int?> ResolveCajeroIdAsync(SqlConnection conn, SqlTransaction? tx)
        {
            // Prefer AppState if set
            if (AppState.CurrentCajeroId.HasValue) return AppState.CurrentCajeroId.Value;

            // Try to find an active cajero in DB
            using var cmd = conn.CreateCommand();
            if (tx != null) cmd.Transaction = tx;
            cmd.CommandText = "SELECT TOP(1) CajeroID FROM Cajero WHERE Activo = 1";
            try
            {
                var result = await cmd.ExecuteScalarAsync();
                if (result != null && result != DBNull.Value)
                    return Convert.ToInt32(result);
            }
            catch
            {
                // ignore errors while trying to resolve cajero id
            }

            // Try any cajero
            using var cmd2 = conn.CreateCommand();
            if (tx != null) cmd2.Transaction = tx;
            cmd2.CommandText = "SELECT TOP(1) CajeroID FROM Cajero";
            try
            {
                var result = await cmd2.ExecuteScalarAsync();
                if (result != null && result != DBNull.Value)
                    return Convert.ToInt32(result);
            }
            catch
            {
                // ignore
            }

            // Unable to resolve a CajeroID
            return null;
        }

        private async Task InsertLogInicioSesionAsync(SqlConnection conn, SqlTransaction? tx, string numeroTarjetaIngresado, bool exitoso, int? tarjetaId, byte? motivoFalloId = null)
        {
            try
            {
                int? cajeroId = await ResolveCajeroIdAsync(conn, tx);
                if (!cajeroId.HasValue)
                {
                    // Cannot log because CajeroID is required by the table's NOT NULL FK
                    return;
                }

                using var cmd = conn.CreateCommand();
                if (tx != null) cmd.Transaction = tx;
                cmd.CommandText = @"INSERT INTO LogInicioSesion (NumeroTarjetaIngresado, Exitoso, CajeroID, TarjetaID, MotivoFalloID) VALUES (@num, @exito, @cajero, @tarjeta, @motivo)";
                cmd.Parameters.Add(new SqlParameter("@num", SqlDbType.VarChar) { Value = numeroTarjetaIngresado });
                cmd.Parameters.Add(new SqlParameter("@exito", SqlDbType.Bit) { Value = exitoso });
                cmd.Parameters.Add(new SqlParameter("@cajero", SqlDbType.Int) { Value = cajeroId.Value });
                cmd.Parameters.Add(new SqlParameter("@tarjeta", SqlDbType.Int) { Value = (object)tarjetaId ?? DBNull.Value });
                cmd.Parameters.Add(new SqlParameter("@motivo", SqlDbType.TinyInt) { Value = (object)motivoFalloId ?? DBNull.Value });
                await cmd.ExecuteNonQueryAsync();
            }
            catch
            {
                // No bloquear autenticación por fallos de logging
            }
        }

        private async void btnIngresar_Click(object sender, EventArgs e)
        {
            string numeroTarjeta = txtNumeroTarjeta.Text.Trim();
            string pin = txtPin.Text.Trim();

            // Validaciones básicas de entrada
            if (string.IsNullOrWhiteSpace(numeroTarjeta) || string.IsNullOrWhiteSpace(pin))
            {
                MessageBox.Show("Por favor, ingrese el número de tarjeta y el PIN.", "Datos Incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var conexion = new ConexionBd();
                using var conn = await conexion.OpenConnectionAsync();

                // --- NUEVO: verificar que el cajero esté inicializado ---
                int? cajeroId = await ResolveCajeroIdAsync(conn, null);
                if (!cajeroId.HasValue)
                {
                    MessageBox.Show("El cajero no ha sido inicializado. Contacte a un administrador.", "Cajero no inicializado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (var cmdCaj = conn.CreateCommand())
                {
                    // Obtener EstadoCajeroID y calcular total desde InventarioEfectivo
                    cmdCaj.CommandText = @"
SELECT
    c.EstadoCajeroID,
    ISNULL((SELECT SUM(i.Cantidad * d.Valor)
            FROM InventarioEfectivo i
            JOIN Denominacion d ON i.DenominacionID = d.DenominacionID
            WHERE i.CajeroID = c.CajeroID), 0) AS TotalEfectivo
FROM Cajero c
WHERE c.CajeroID = @id";
                    cmdCaj.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = cajeroId.Value });
                    using var readerCaj = await cmdCaj.ExecuteReaderAsync();
                    int estadoCajero = 0;
                    if (await readerCaj.ReadAsync())
                    {
                        // EstadoCajeroID is TINYINT (byte) in DB, so read as byte
                        estadoCajero = readerCaj.IsDBNull(readerCaj.GetOrdinal("EstadoCajeroID")) ? 0 : readerCaj.GetByte(readerCaj.GetOrdinal("EstadoCajeroID"));
                        // decimal totalEfectivo = readerCaj.IsDBNull(readerCaj.GetOrdinal("TotalEfectivo")) ? 0m : readerCaj.GetDecimal(readerCaj.GetOrdinal("TotalEfectivo"));
                    }
                    else
                    {
                        MessageBox.Show("El cajero no ha sido inicializado. Contacte a un administrador.", "Cajero no inicializado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Consideramos 'inicializado' cuando el estado es 'En Servicio' (1) o 'Bajo en Efectivo' (4)
                    bool inicializado = (estadoCajero == 1 || estadoCajero == 4);

                    if (!inicializado)
                    {
                        // Intentamos insertar log de intento fallido (no obligatorio)
                        _ = InsertLogInicioSesionAsync(conn, null, numeroTarjeta, false, null, null);
                        MessageBox.Show("El cajero no ha sido inicializado. Contacte a un administrador.", "Cajero no inicializado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                using var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    SELECT TOP(1)
                        u.UsuarioID as UsuarioId,
                        (u.Nombres + ' ' + u.Apellidos) as UsuarioNombre,
                        t.TarjetaID as TarjetaID,
                        t.NumeroTarjeta as NumeroTarjeta,
                        ISNULL(t.PinHash, '') as PinCandidate,
                        t.FechaExpiracion as FechaExpiracion,
                        t.EstadoTarjetaID as EstadoTarjetaID,
                        c.SaldoActual as SaldoActual,
                        c.MontoMaximoRetiroDiario as MontoMaximoRetiroDiario
                    FROM Tarjeta t
                    INNER JOIN Cuenta c ON t.CuentaID = c.CuentaID
                    INNER JOIN Usuario u ON c.UsuarioID = u.UsuarioID
                    WHERE t.NumeroTarjeta = @num AND u.Activo = 1";
                cmd.Parameters.Add(new SqlParameter("@num", SqlDbType.NVarChar) { Value = numeroTarjeta });

                int? tarjetaId = null;
                using var reader = await cmd.ExecuteReaderAsync();
                if (!await reader.ReadAsync())
                {
                    // Tarjeta no existe => motivo 2
                    await InsertLogInicioSesionAsync(conn, null, numeroTarjeta, false, null, 2);
                    MessageBox.Show("Número de tarjeta o PIN incorrecto. Por favor, intente de nuevo.", "Error de Autenticación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPin.Clear();
                    return;
                }

                string pinCandidate = reader.IsDBNull(reader.GetOrdinal("PinCandidate")) ? string.Empty : reader.GetString(reader.GetOrdinal("PinCandidate"));
                tarjetaId = reader.GetInt32(reader.GetOrdinal("TarjetaID"));

                // Check expiration
                DateTime? fechaExp = reader.IsDBNull(reader.GetOrdinal("FechaExpiracion")) ? null : reader.GetDateTime(reader.GetOrdinal("FechaExpiracion"));
                if (fechaExp.HasValue && fechaExp.Value.Date < DateTime.UtcNow.Date)
                {
                    // Tarjeta vencida => motivo 4
                    await InsertLogInicioSesionAsync(conn, null, numeroTarjeta, false, tarjetaId, 4);
                    MessageBox.Show("La tarjeta se encuentra vencida.", "Tarjeta Vencida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Check estado tarjeta if available (assume 1 = activa)
                byte? estadoTarjeta = reader.IsDBNull(reader.GetOrdinal("EstadoTarjetaID")) ? null : (byte?)reader.GetByte(reader.GetOrdinal("EstadoTarjetaID"));
                if (estadoTarjeta.HasValue && estadoTarjeta.Value != 1)
                {
                    // Tarjeta no en estado activo => motivo 3 (bloqueada) or 5 (inactiva)
                    // We map any non-1 to 3 (bloqueada) for now.
                    await InsertLogInicioSesionAsync(conn, null, numeroTarjeta, false, tarjetaId, 3);
                    MessageBox.Show("La tarjeta no está activa para su uso.", "Tarjeta No Activa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                bool pinIsHashed = !string.IsNullOrEmpty(pinCandidate) && pinCandidate.Length == 64 && System.Text.RegularExpressions.Regex.IsMatch(pinCandidate, "^[0-9a-fA-F]{64}$");

                bool valido;
                if (pinIsHashed)
                {
                    valido = HashHelper.VerifySha256Hash(pin, pinCandidate);
                }
                else
                {
                    // Fallback: compare plaintext (for migrated/demo data)
                    valido = pin == pinCandidate;
                }

                if (!valido)
                {
                    // PIN incorrecto => motivo 1
                    await InsertLogInicioSesionAsync(conn, null, numeroTarjeta, false, tarjetaId, 1);
                    MessageBox.Show("Número de tarjeta o PIN incorrecto. Por favor, intente de nuevo.", "Error de Autenticación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPin.Clear();
                    return;
                }

                // Construir objeto Usuario con los datos obtenidos
                var usuario = new Usuario();
                usuario.Id = reader.GetInt32(reader.GetOrdinal("UsuarioId"));
                usuario.Nombre = reader.GetString(reader.GetOrdinal("UsuarioNombre"));
                usuario.NumeroTarjeta = reader.GetString(reader.GetOrdinal("NumeroTarjeta"));
                usuario.SaldoActual = reader.GetDecimal(reader.GetOrdinal("SaldoActual"));
                // Map DB MontoMaximoRetiroDiario into our model property MontoMaximoDiario
                usuario.MontoMaximoDiario = reader.GetDecimal(reader.GetOrdinal("MontoMaximoRetiroDiario"));

                // Log successful login (no motivo)
                await InsertLogInicioSesionAsync(conn, null, numeroTarjeta, true, tarjetaId, null);

                // Autenticación exitosa
                this.UsuarioAutenticado = usuario;
                this.DialogResult = DialogResult.OK;
                this.Hide();

                MenuUsuario menu = new MenuUsuario(this.UsuarioAutenticado);
                menu.ShowDialog();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al intentar autenticar: " + ex.Message, "Error Crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
