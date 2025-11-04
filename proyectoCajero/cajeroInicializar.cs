using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using static proyectoCajero.archivosTxt;

namespace proyectoCajero
{
    public enum CajeroFormMode
    {
        Inicializar,
        Agregar
    }
    public partial class cajeroInicializar : Form
    {
        private readonly CajeroFormMode _mode;
        private Cajero _cajeroActual;
        private int? _cajeroIdSeleccionado;
        private List<(int CajeroID, string Ubicacion)> _cajerosLista = new();
        public int contador = 0;
        public cajeroInicializar(CajeroFormMode mode)
        {
            InitializeComponent();    
            _mode = mode; // Guardamos el modo
            SetupForm();  // Llamamos a un método para configurar la apariencia
            this.Load += cajeroInicializar_Load;
        }
        public class DataModel
        {
            public string Nombre { get; set; }
            public string Pin { get; set; }
        }

        private async void cajeroInicializar_Load(object sender, EventArgs e)
        {
            // Cargar cajeros
            try
            {
                var conexion = new ConexionBd();
                using var conn = await conexion.OpenConnectionAsync();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT CajeroID, Ubicacion FROM Cajero ORDER BY Ubicacion";
                using var reader = await cmd.ExecuteReaderAsync();
                _cajerosLista = new List<(int CajeroID, string Ubicacion)>();
                while (await reader.ReadAsync())
                {
                    _cajerosLista.Add((reader.GetInt32(0), reader.GetString(1)));
                }
                if (_cajerosLista.Count > 0)
                {
                    _cajeroIdSeleccionado = _cajerosLista[0].CajeroID;
                    cajeroIDTB.Text = _cajeroIdSeleccionado.ToString();
                    cajeroUbicacionTB.Text = _cajerosLista[0].Ubicacion;
                }
            }
            catch { }

            // Sincronizar cajeroIDTB -> cajeroUbicacionTB
            cajeroIDTB.Leave += CajeroIDTB_Leave;
            cajeroIDTB.KeyDown += (s, ev) => {
                if (ev.KeyCode == Keys.Enter)
                {
                    CajeroIDTB_Leave(cajeroIDTB, EventArgs.Empty);
                }
            };

            // Mostrar nombre de empleado actual
            try
            {
                int? empleadoId = AppState.CurrentEmpleadoId;
                if (empleadoId.HasValue)
                {
                    var conexion = new ConexionBd();
                    using var conn = await conexion.OpenConnectionAsync();
                    using var cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT Nombres FROM Empleado WHERE EmpleadoID = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = empleadoId.Value });
                    var nombre = await cmd.ExecuteScalarAsync();
                    empleadoTB.Text = nombre?.ToString() ?? "";
                }
            }
            catch { empleadoTB.Text = ""; }
        }

        private void CajeroIDTB_Leave(object sender, EventArgs e)
        {
            if (int.TryParse(cajeroIDTB.Text, out int id))
            {
                var idx = _cajerosLista.FindIndex(x => x.CajeroID == id);
                if (idx >= 0)
                {
                    _cajeroIdSeleccionado = id;
                    cajeroUbicacionTB.Text = _cajerosLista[idx].Ubicacion;
                }
                else
                {
                    cajeroUbicacionTB.Text = "";
                }
            }
        }

        private void ActualizarTotal()
        {
            decimal totalNuevo = (num200.Value * 200) + (num100.Value * 100) +
                        (num50.Value * 50) + (num20.Value * 20) +
                        (num10.Value * 10) + (num5.Value * 5) +
                        (num1.Value * 1);

            decimal totalFinal;
            decimal limite;

            if (_mode == CajeroFormMode.Agregar && _cajeroActual != null)
            {
                totalFinal = _cajeroActual.TotalEfectivo + totalNuevo;
                limite = _cajeroActual.CapacidadMaxima - _cajeroActual.MontoInicial; // 40000 - 10000 = 30000
                lblTotal.Text = $"Total Actual: {_cajeroActual.TotalEfectivo:C}\n" +
                              $"Monto a Agregar: {totalNuevo:C}\n" +
                              $"Nuevo Total: {totalFinal:C}";
            }
            else // Modo Inicializar
            {
                totalFinal = totalNuevo;
                limite = _cajeroActual != null ? _cajeroActual.MontoInicial : 10000m;
                lblTotal.Text = $"Total: {totalFinal:C}";
            }

            lblTotal.ForeColor = totalFinal > limite ? System.Drawing.Color.Red : System.Drawing.Color.Black;
        }

        private void num200_ValueChanged(object sender, EventArgs e) => ActualizarTotal();
        private void num100_ValueChanged(object sender, EventArgs e) => ActualizarTotal();
        private void num50_ValueChanged(object sender, EventArgs e) => ActualizarTotal();
        private void num20_ValueChanged(object sender, EventArgs e) => ActualizarTotal();
        private void num10_ValueChanged(object sender, EventArgs e) => ActualizarTotal();
        private void num5_ValueChanged(object sender, EventArgs e) => ActualizarTotal();
        private void num1_ValueChanged(object sender, EventArgs e) => ActualizarTotal();

        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            if (_mode != CajeroFormMode.Inicializar)
            {
                MessageBox.Show("Solo se implementa la inicialización en BD en este flujo.", "No implementado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (!_cajeroIdSeleccionado.HasValue)
            {
                MessageBox.Show("Debe seleccionar una ubicación de cajero.", "Cajero requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int cajeroId = _cajeroIdSeleccionado.Value;
            int? empleadoId = AppState.CurrentEmpleadoId;
            if (!empleadoId.HasValue)
            {
                MessageBox.Show("No se detectó empleado en sesión.", "Empleado requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string justificacion = justificacionRTB.Text?.Trim() ?? "";
            var billetes = new Dictionary<decimal, int>
            {
                {200m, (int)num200.Value},
                {100m, (int)num100.Value},
                {50m, (int)num50.Value},
                {20m, (int)num20.Value},
                {10m, (int)num10.Value},
                {5m, (int)num5.Value},
                {1m, (int)num1.Value}
            };
            decimal totalEfectivo = billetes.Sum(x => x.Key * x.Value);
            if (totalEfectivo < 10000m)
            {
                MessageBox.Show("El monto para inicializar debe ser al menos Q10,000.", "Inicialización Insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (totalEfectivo > 40000m)
            {
                MessageBox.Show("El monto para inicializar excede la capacidad máxima del cajero (Q40,000).", "Capacidad Excedida", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                var conexion = new ConexionBd();
                using var conn = await conexion.OpenConnectionAsync();
                using var tx = conn.BeginTransaction();
                // 1. Actualizar InventarioEfectivo por denominación
                var denomIds = new Dictionary<decimal, int>();
                using (var cmdDen = conn.CreateCommand())
                {
                    cmdDen.Transaction = tx;
                    cmdDen.CommandText = "SELECT DenominacionID, Valor FROM Denominacion WHERE Valor IN (200,100,50,20,10,5,1)";
                    using var rdr = await cmdDen.ExecuteReaderAsync();
                    while (await rdr.ReadAsync())
                    {
                        var id = rdr.GetInt32(rdr.GetOrdinal("DenominacionID"));
                        var val = rdr.GetDecimal(rdr.GetOrdinal("Valor"));
                        denomIds[val] = id;
                    }
                    rdr.Close();
                }
                foreach (var kv in billetes)
                {
                    if (kv.Value <= 0) continue;
                    if (!denomIds.ContainsKey(kv.Key))
                    {
                        tx.Rollback();
                        MessageBox.Show($"Denominación {kv.Key} no está registrada en la base de datos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    int denomId = denomIds[kv.Key];
                    using var cmdUp = conn.CreateCommand();
                    cmdUp.Transaction = tx;
                    cmdUp.CommandText = @"
IF EXISTS (SELECT 1 FROM InventarioEfectivo WHERE CajeroID = @cajero AND DenominacionID = @den)
    UPDATE InventarioEfectivo SET Cantidad = @cant WHERE CajeroID = @cajero AND DenominacionID = @den;
ELSE
    INSERT INTO InventarioEfectivo (CajeroID, DenominacionID, Cantidad) VALUES (@cajero, @den, @cant);
";
                    cmdUp.Parameters.Add(new SqlParameter("@cajero", SqlDbType.Int) { Value = cajeroId });
                    cmdUp.Parameters.Add(new SqlParameter("@den", SqlDbType.Int) { Value = denomId });
                    cmdUp.Parameters.Add(new SqlParameter("@cant", SqlDbType.Int) { Value = kv.Value });
                    await cmdUp.ExecuteNonQueryAsync();
                }
                // 2. Insertar LogMovimientoEfectivo (TipoMovimientoID = 1)
                long logId;
                using (var cmdLog = conn.CreateCommand())
                {
                    cmdLog.Transaction = tx;
                    cmdLog.CommandText = @"INSERT INTO LogMovimientoEfectivo (CajeroID, EmpleadoID, TipoMovimientoID, Justificacion) OUTPUT INSERTED.LogID VALUES (@cajero, @empleado, @tipo, @just)";
                    cmdLog.Parameters.Add(new SqlParameter("@cajero", SqlDbType.Int) { Value = cajeroId });
                    cmdLog.Parameters.Add(new SqlParameter("@empleado", SqlDbType.Int) { Value = empleadoId.Value });
                    cmdLog.Parameters.Add(new SqlParameter("@tipo", SqlDbType.TinyInt) { Value = 1 });
                    cmdLog.Parameters.Add(new SqlParameter("@just", SqlDbType.NVarChar) { Value = justificacion });
                    var obj = await cmdLog.ExecuteScalarAsync();
                    logId = Convert.ToInt64(obj);
                }
                // 3. Insertar detalles
                foreach (var kv in billetes)
                {
                    if (kv.Value <= 0) continue;
                    int denomId = denomIds[kv.Key];
                    using var cmdDet = conn.CreateCommand();
                    cmdDet.Transaction = tx;
                    cmdDet.CommandText = @"INSERT INTO LogMovimientoEfectivoDetalle (LogID, DenominacionID, Cantidad) VALUES (@log, @den, @cant)";
                    cmdDet.Parameters.Add(new SqlParameter("@log", SqlDbType.BigInt) { Value = logId });
                    cmdDet.Parameters.Add(new SqlParameter("@den", SqlDbType.Int) { Value = denomId });
                    cmdDet.Parameters.Add(new SqlParameter("@cant", SqlDbType.Int) { Value = kv.Value });
                    await cmdDet.ExecuteNonQueryAsync();
                }
                // 4. Actualizar estado del cajero a En Servicio (1)
                using (var cmdEstado = conn.CreateCommand())
                {
                    cmdEstado.Transaction = tx;
                    cmdEstado.CommandText = "UPDATE Cajero SET EstadoCajeroID = 1 WHERE CajeroID = @id";
                    cmdEstado.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = cajeroId });
                    await cmdEstado.ExecuteNonQueryAsync();
                }
                tx.Commit();
                MessageBox.Show($"Inicialización exitosa. Total en cajero: {totalEfectivo:C}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al inicializar el cajero: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupForm()
        {
            if (_mode == CajeroFormMode.Inicializar)
            {
                this.Text = "Inicializar Cajero";
                btnGuardar.Text = "Inicializar";
            }
            else // Modo Agregar
            {
                this.Text = "Agregar Efectivo";
                btnGuardar.Text = "Agregar";
                CargarEstadoActualCajero();
            }
        }

        private void CargarEstadoActualCajero()
        {
            string nombreArchivo = "cajero.json";
            string pathFinal = direccione.obtenerRutasTxt(nombreArchivo);

            if (!File.Exists(pathFinal))
            {
                MessageBox.Show("El cajero aún no ha sido inicializado. Debe inicializarlo antes de poder agregar efectivo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.BeginInvoke(new Action(() => this.Close()));
                return;
            }

            try
            {
                string jsonString = File.ReadAllText(pathFinal);
                _cajeroActual = JsonSerializer.Deserialize<Cajero>(jsonString);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al leer el estado actual del cajero: " + ex.Message);
                this.BeginInvoke(new Action(() => this.Close()));
            }
        }
    }
}
