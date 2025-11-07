using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace proyectoCajero
{
    public partial class HistorialtransaccionCuenta : Form
    {
        private readonly Usuario _usuario;
        private readonly ConexionBd _conexion;

        public HistorialtransaccionCuenta(Usuario usuario)
        {
            InitializeComponent();
            _usuario = usuario;
            _conexion = new ConexionBd();
            ConfigurarDataGridView();
        }

        private async void HistorialtransaccionCuenta_Load(object sender, EventArgs e)
        {
            // Establecer valores por defecto para el rango de fechas
            IniciodateTimePicker.Value = DateTime.Now.AddMonths(-1);
            FindateTimePicker.Value = DateTime.Now;

            // Validar que el objeto de usuario se haya pasado correctamente
            if (_usuario == null)
            {
                MessageBox.Show("Error crítico: No se ha proporcionado información del usuario.", "Error de Carga", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnBuscar.Enabled = false;
                return;
            }

            // Cargar automáticamente el número de la primera tarjeta activa del usuario
            string numeroTarjetaActiva = await ObtenerTarjetaActivaDelUsuario(_usuario.Id);

            if (!string.IsNullOrEmpty(numeroTarjetaActiva))
            {
                NumeroCuentaTB.Text = numeroTarjetaActiva;
            }
            else
            {
                MessageBox.Show("No se encontró una tarjeta activa para este usuario. No se podrán realizar búsquedas.", "Sin Tarjeta Activa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btnBuscar.Enabled = false; // Deshabilitar la búsqueda si no hay tarjeta
            }
        }

        private void ConfigurarDataGridView()
        {
            // Configuración visual para que la grilla se vea profesional
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 71, 160);
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue;
        }

        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            // 1. Validaciones (Sin cambios)
            if (string.IsNullOrWhiteSpace(NumeroCuentaTB.Text))
            {
                MessageBox.Show("Por favor, ingrese un número de tarjeta.", "Dato Requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                NumeroCuentaTB.Focus();
                return;
            }
            if (FindateTimePicker.Value < IniciodateTimePicker.Value)
            {
                MessageBox.Show("La fecha final no puede ser anterior a la fecha de inicio.", "Rango de Fechas Inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Preparación (Sin cambios)
            string numeroTarjeta = NumeroCuentaTB.Text.Trim();
            DateTime fechaInicio = IniciodateTimePicker.Value.Date;
            DateTime fechaFin = FindateTimePicker.Value.Date;

            dataGridView1.DataSource = null;
            Cursor = Cursors.WaitCursor;

            try
            {
                // 3. Paso de "Traducción" (Sin cambios)
                string numeroCuenta = await ObtenerNumeroCuentaDesdeTarjeta(numeroTarjeta);
                if (string.IsNullOrEmpty(numeroCuenta))
                {
                    MessageBox.Show("El número de tarjeta ingresado no se encontró o no está asociado a una cuenta activa.", "Tarjeta no encontrada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // --- 4. Definición de la Consulta SQL (MODIFICADA PARA FILTRAR SOLO TRANSFERENCIAS) ---
                string sql = @"
            SELECT
                T.FechaHora AS 'Fecha y Hora',
                TT.Nombre AS 'Tipo de Transacción',
                CASE WHEN T.TipoTransaccionID = 4 THEN T.Monto END AS 'Débito',   -- Solo Tipo 4 (Saliente)
                CASE WHEN T.TipoTransaccionID = 5 THEN T.Monto END AS 'Crédito',   -- Solo Tipo 5 (Entrante)
                CASE 
                    WHEN T.TipoTransaccionID = 4 THEN C_Destino.NumeroCuenta
                    WHEN T.TipoTransaccionID = 5 THEN C_Origen.NumeroCuenta
                END AS 'Cuenta Contraparte',
                Trf.Descripcion AS 'Descripción'
            FROM Cuenta AS C_Busqueda
            LEFT JOIN Transferencia AS Trf_Origen ON C_Busqueda.CuentaID = Trf_Origen.CuentaOrigenID
            LEFT JOIN Transferencia AS Trf_Destino ON C_Busqueda.CuentaID = Trf_Destino.CuentaDestinoID
            JOIN Transaccion AS T ON 
                T.TransferenciaID = Trf_Origen.TransferenciaID OR 
                T.TransferenciaID = Trf_Destino.TransferenciaID
                -- Se eliminó la condición para retiros y depósitos
            JOIN TipoTransaccion AS TT ON T.TipoTransaccionID = TT.TipoTransaccionID
            LEFT JOIN Transferencia AS Trf ON T.TransferenciaID = Trf.TransferenciaID
            LEFT JOIN Cuenta AS C_Origen ON Trf.CuentaOrigenID = C_Origen.CuentaID
            LEFT JOIN Cuenta AS C_Destino ON Trf.CuentaDestinoID = C_Destino.CuentaID
            WHERE
                C_Busqueda.NumeroCuenta = @NumeroCuenta
                AND CAST(T.FechaHora AS DATE) BETWEEN @FechaInicio AND @FechaFin
                -- ***** LA NUEVA CONDICIÓN DE FILTRADO *****
                AND T.TipoTransaccionID IN (4, 5) -- Filtra para incluir solo IDs de transferencias
            ORDER BY
                T.FechaHora DESC;";

                // 5. Ejecución y 6. Mostrar Resultados (Sin cambios)
                using (var conn = await _conexion.OpenConnectionAsync())
                {
                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.Add(new SqlParameter("@NumeroCuenta", SqlDbType.VarChar, 20) { Value = numeroCuenta });
                        cmd.Parameters.Add(new SqlParameter("@FechaInicio", SqlDbType.Date) { Value = fechaInicio });
                        cmd.Parameters.Add(new SqlParameter("@FechaFin", SqlDbType.Date) { Value = fechaFin });

                        var adapter = new SqlDataAdapter(cmd);
                        var dataTable = new DataTable();
                        await Task.Run(() => adapter.Fill(dataTable));

                        if (dataTable.Rows.Count > 0)
                        {
                            dataGridView1.DataSource = dataTable;

                            var culturaGT = new System.Globalization.CultureInfo("es-GT");
                            if (dataGridView1.Columns.Contains("Débito"))
                            {
                                dataGridView1.Columns["Débito"].DefaultCellStyle.Format = "c";
                                dataGridView1.Columns["Débito"].DefaultCellStyle.FormatProvider = culturaGT;
                            }
                            if (dataGridView1.Columns.Contains("Crédito"))
                            {
                                dataGridView1.Columns["Crédito"].DefaultCellStyle.Format = "c";
                                dataGridView1.Columns["Crédito"].DefaultCellStyle.FormatProvider = culturaGT;
                            }
                        }
                        else
                        {
                            MessageBox.Show("No se encontraron TRANSFERENCIAS para la cuenta y el rango de fechas especificado.", "Sin Resultados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al buscar el historial: {ex.Message}", "Error de Búsqueda", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private async Task<string> ObtenerNumeroCuentaDesdeTarjeta(string numeroTarjeta)
        {
            try
            {
                string sql = @"
                    SELECT C.NumeroCuenta
                    FROM Cuenta AS C
                    JOIN Tarjeta AS T ON C.CuentaID = T.CuentaID
                    WHERE T.NumeroTarjeta = @NumeroTarjeta;";

                using (var conn = await _conexion.OpenConnectionAsync())
                {
                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.Add(new SqlParameter("@NumeroTarjeta", SqlDbType.VarChar, 19) { Value = numeroTarjeta });
                        object result = await cmd.ExecuteScalarAsync();
                        return result?.ToString();
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        private async Task<string> ObtenerTarjetaActivaDelUsuario(int usuarioId)
        {
            try
            {
                string sql = @"
                    SELECT TOP 1 T.NumeroTarjeta
                    FROM Tarjeta AS T
                    JOIN Cuenta AS C ON T.CuentaID = C.CuentaID
                    WHERE C.UsuarioID = @UsuarioID 
                      AND C.EstadoCuentaID = 1
                      AND T.EstadoTarjetaID = 1
                    ORDER BY T.FechaAsignacion DESC;"; // Obtener la tarjeta activa más reciente

                using (var conn = await _conexion.OpenConnectionAsync())
                {
                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.Add(new SqlParameter("@UsuarioID", SqlDbType.Int) { Value = usuarioId });
                        object result = await cmd.ExecuteScalarAsync();
                        return result?.ToString();
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        private void NumeroCuentaTB_TextChanged(object sender, EventArgs e)
        {
            // Este evento puede quedar vacío o usarse para validaciones en tiempo real si se desea.
        }
    }
}