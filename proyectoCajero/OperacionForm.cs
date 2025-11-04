using System;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Json;
using static proyectoCajero.archivosTxt;
using Microsoft.Data.SqlClient;

namespace proyectoCajero
{
    public partial class OperacionForm : Form
    {
        private readonly Usuario _usuario;
        private readonly TipoOperacion _tipoOperacion;
        private readonly decimal _montoRetiradoHoy;
        public Usuario UsuarioActualizado { get; private set; }

        private const byte TipoMovimientoInicializacion = 1;
        private const byte TipoMovimientoRecarga = 2;

        public OperacionForm(Usuario usuario, TipoOperacion tipo, decimal montoRetiradoHoy = 0)
        {
            InitializeComponent();
            _usuario = usuario;
            _tipoOperacion = tipo;
            _montoRetiradoHoy = montoRetiradoHoy; // Guardamos el nuevo dato
            SetupForm();
        }

        private void SetupForm()
        {
            if (_tipoOperacion == TipoOperacion.Deposito)
            {
                this.Text = "Realizar Depósito";
                btnAceptar.Text = "Depositar";
            }
            else // Futuro modo Retiro
            {
                this.Text = "Realizar Retiro";
                btnAceptar.Text = "Retirar";
            }
        }
        private void ActualizarTotal()
        {
            decimal total = (num200.Value * 200) + (num100.Value * 100) +
                            (num50.Value * 50) + (num20.Value * 20) +
                            (num10.Value * 10) + (num5.Value * 5) +
                            (num1.Value * 1);
            lblTotalOperacion.Text = $"Total: {total:C}";
        }

        private void num200_ValueChanged(object sender, EventArgs e)
        {
            ActualizarTotal();
        }

        private void num100_ValueChanged(object sender, EventArgs e)
        {
            ActualizarTotal();
        }

        private void num50_ValueChanged(object sender, EventArgs e)
        {
            ActualizarTotal();
        }

        private void num20_ValueChanged(object sender, EventArgs e)
        {
            ActualizarTotal();
        }

        private void num10_ValueChanged(object sender, EventArgs e)
        {
            ActualizarTotal();
        }

        private void num5_ValueChanged(object sender, EventArgs e)
        {
            ActualizarTotal();
        }

        private void num1_ValueChanged(object sender, EventArgs e)
        {
            ActualizarTotal();
        }

        private async Task<int?> ResolveEmpleadoIdAsync(SqlConnection conn, SqlTransaction tx)
        {
            if (AppState.CurrentEmpleadoId.HasValue) return AppState.CurrentEmpleadoId.Value;
            using var cmd = conn.CreateCommand();
            cmd.Transaction = tx;
            cmd.CommandText = "SELECT TOP(1) EmpleadoID FROM Empleado WHERE Activo = 1";
            var res = await cmd.ExecuteScalarAsync();
            if (res != null && res != DBNull.Value) return Convert.ToInt32(res);
            return null;
        }

        private async void btnAceptar_Click(object sender, EventArgs e)
        {
            decimal montoTotal = (num200.Value * 200) + (num100.Value * 100) +
                       (num50.Value * 50) + (num20.Value * 20) +
                       (num10.Value * 10) + (num5.Value * 5) +
                       (num1.Value * 1);

            if (montoTotal <= 0)
            {
                MessageBox.Show("Debe especificar una cantidad de billetes.", "Monto Inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_tipoOperacion == TipoOperacion.Deposito)
            {
                // Lógica de depósito con base de datos
                try
                {
                    var conexion = new ConexionBd();
                    using var conn = await conexion.OpenConnectionAsync();
                    using var tx = conn.BeginTransaction();

                    // Resolver cajero
                    int? cajeroId = AppState.CurrentCajeroId;
                    if (!cajeroId.HasValue)
                    {
                        using var cmdFind = conn.CreateCommand();
                        cmdFind.Transaction = tx;
                        cmdFind.CommandText = "SELECT TOP(1) CajeroID FROM Cajero WHERE Activo = 1";
                        var res = await cmdFind.ExecuteScalarAsync();
                        if (res != null && res != DBNull.Value) cajeroId = Convert.ToInt32(res);
                    }

                    if (!cajeroId.HasValue)
                    {
                        tx.Rollback();
                        MessageBox.Show("No se encontró un cajero configurado. Contacte a un administrador.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Obtener info del cajero
                    using var cmdCaj = conn.CreateCommand();
                    cmdCaj.Transaction = tx;
                    // DB schema uses EstadoCajeroID (catalog table) instead of an 'Inicializado' boolean or CapacidadMaxima column.
                    cmdCaj.CommandText = @"SELECT EstadoCajeroID FROM Cajero WHERE CajeroID = @id";
                    cmdCaj.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = cajeroId.Value });
                    using var readerCaj = await cmdCaj.ExecuteReaderAsync();
                    bool inicializado = false;
                    decimal capacidad = 40000m; // default fallback if DB doesn't store capacity
                    if (await readerCaj.ReadAsync())
                    {
                        int estadoCajero = !readerCaj.IsDBNull(readerCaj.GetOrdinal("EstadoCajeroID")) ? readerCaj.GetInt32(readerCaj.GetOrdinal("EstadoCajeroID")) : 0;
                        // Consideramos 'inicializado' cuando el estado es 'En Servicio' (1) o 'Bajo en Efectivo' (4)
                        inicializado = (estadoCajero == 1 || estadoCajero == 4);
                    }
                    else
                    {
                        readerCaj.Close();
                        tx.Rollback();
                        MessageBox.Show("Cajero no encontrado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    readerCaj.Close();

                    if (!inicializado)
                    {
                        tx.Rollback();
                        MessageBox.Show("El cajero no ha sido inicializado. No se puede aceptar depósitos.", "Cajero no inicializado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Calcular total actual en cajero desde InventarioEfectivo + Denominacion
                    using var cmdTotal = conn.CreateCommand();
                    cmdTotal.Transaction = tx;
                    cmdTotal.CommandText = @"SELECT ISNULL(SUM(i.Cantidad * d.Valor),0) FROM InventarioEfectivo i JOIN Denominacion d ON i.DenominacionID = d.DenominacionID WHERE i.CajeroID = @cajero";
                    cmdTotal.Parameters.Add(new SqlParameter("@cajero", SqlDbType.Int) { Value = cajeroId.Value });
                    var totalObj = await cmdTotal.ExecuteScalarAsync();
                    decimal totalActual = (totalObj == null || totalObj == DBNull.Value) ? 0m : Convert.ToDecimal(totalObj);

                    decimal nuevoTotal = totalActual + montoTotal;
                    if (nuevoTotal > capacidad)
                    {
                        tx.Rollback();
                        MessageBox.Show($"No se puede depositar {montoTotal:C}. El depósito excedería la capacidad máxima del cajero ({capacidad:C}).", "Depósito Rechazado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MessageBox.Show($"ALERTA ADMIN: Intento de depósito que excede la capacidad. Monto intentado: {montoTotal:C}. Nuevo total: {nuevoTotal:C}.", "Alerta de Depósito", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Mapear denominaciones a DenominacionID
                    var denomValues = new Dictionary<decimal, int>
                    {
                        {200m, (int)num200.Value},
                        {100m, (int)num100.Value},
                        {50m, (int)num50.Value},
                        {20m, (int)num20.Value},
                        {10m, (int)num10.Value},
                        {5m, (int)num5.Value},
                        {1m, (int)num1.Value}
                    };

                    // Obtener IDs de denominaciones
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

                    // Actualizar InventarioEfectivo por denominación (upsert)
                    foreach (var kv in denomValues)
                    {
                        if (kv.Value <= 0) continue;
                        var valor = kv.Key;
                        var cantidad = kv.Value;
                        if (!denomIds.ContainsKey(valor))
                        {
                            // Si la denominación no existe en catálogo, rollback
                            tx.Rollback();
                            MessageBox.Show($"Denominación {valor} no está registrada en la base de datos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        int denomId = denomIds[valor];

                        using var cmdUp = conn.CreateCommand();
                        cmdUp.Transaction = tx;
                        cmdUp.CommandText = @"
IF EXISTS (SELECT 1 FROM InventarioEfectivo WHERE CajeroID = @cajero AND DenominacionID = @den)
    UPDATE InventarioEfectivo SET Cantidad = Cantidad + @cant WHERE CajeroID = @cajero AND DenominacionID = @den;
ELSE
    INSERT INTO InventarioEfectivo (CajeroID, DenominacionID, Cantidad) VALUES (@cajero, @den, @cant);
";
                        cmdUp.Parameters.Add(new SqlParameter("@cajero", SqlDbType.Int) { Value = cajeroId.Value });
                        cmdUp.Parameters.Add(new SqlParameter("@den", SqlDbType.Int) { Value = denomId });
                        cmdUp.Parameters.Add(new SqlParameter("@cant", SqlDbType.Int) { Value = cantidad });
                        await cmdUp.ExecuteNonQueryAsync();
                    }

                    // Registrar LogMovimientoEfectivo y detalle si hay un empleado resolvible
                    int? empleadoId = await ResolveEmpleadoIdAsync(conn, tx);
                    if (empleadoId.HasValue)
                    {
                        long logId;
                        using (var cmdLog = conn.CreateCommand())
                        {
                            cmdLog.Transaction = tx;
                            cmdLog.CommandText = @"INSERT INTO LogMovimientoEfectivo (CajeroID, EmpleadoID, TipoMovimientoID, Justificacion) OUTPUT INSERTED.LogID VALUES (@cajero, @empleado, @tipo, @just)";
                            cmdLog.Parameters.Add(new SqlParameter("@cajero", SqlDbType.Int) { Value = cajeroId.Value });
                            cmdLog.Parameters.Add(new SqlParameter("@empleado", SqlDbType.Int) { Value = empleadoId.Value });
                            cmdLog.Parameters.Add(new SqlParameter("@tipo", SqlDbType.TinyInt) { Value = TipoMovimientoRecarga });
                            cmdLog.Parameters.Add(new SqlParameter("@just", SqlDbType.NVarChar) { Value = (object)DBNull.Value ?? DBNull.Value });
                            var obj = await cmdLog.ExecuteScalarAsync();
                            logId = Convert.ToInt64(obj);
                        }

                        // Insertar detalles
                        foreach (var kv in denomValues)
                        {
                            if (kv.Value <= 0) continue;
                            var valor = kv.Key;
                            int denomId = denomIds[valor];
                            using var cmdDet = conn.CreateCommand();
                            cmdDet.Transaction = tx;
                            cmdDet.CommandText = @"INSERT INTO LogMovimientoEfectivoDetalle (LogID, DenominacionID, Cantidad) VALUES (@log, @den, @cant)";
                            cmdDet.Parameters.Add(new SqlParameter("@log", SqlDbType.BigInt) { Value = logId });
                            cmdDet.Parameters.Add(new SqlParameter("@den", SqlDbType.Int) { Value = denomId });
                            cmdDet.Parameters.Add(new SqlParameter("@cant", SqlDbType.Int) { Value = kv.Value });
                            await cmdDet.ExecuteNonQueryAsync();
                        }
                    }

                    // Actualizar saldo de la cuenta del usuario
                    // Obtener CuentaID desde Tarjeta
                    int cuentaId;
                    using (var cmdCuenta = conn.CreateCommand())
                    {
                        cmdCuenta.Transaction = tx;
                        cmdCuenta.CommandText = @"SELECT TOP(1) c.CuentaID FROM Tarjeta t INNER JOIN Cuenta c ON t.CuentaID = c.CuentaID WHERE t.NumeroTarjeta = @num";
                        cmdCuenta.Parameters.Add(new SqlParameter("@num", SqlDbType.NVarChar) { Value = _usuario.NumeroTarjeta });
                        var obj = await cmdCuenta.ExecuteScalarAsync();
                        if (obj == null || obj == DBNull.Value)
                        {
                            tx.Rollback();
                            MessageBox.Show("Cuenta asociada a la tarjeta no encontrada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        cuentaId = Convert.ToInt32(obj);
                    }

                    using (var cmdUpdSaldo = conn.CreateCommand())
                    {
                        cmdUpdSaldo.Transaction = tx;
                        cmdUpdSaldo.CommandText = "UPDATE Cuenta SET SaldoActual = SaldoActual + @monto WHERE CuentaID = @cuenta";
                        cmdUpdSaldo.Parameters.Add(new SqlParameter("@monto", SqlDbType.Decimal) { Value = montoTotal });
                        cmdUpdSaldo.Parameters.Add(new SqlParameter("@cuenta", SqlDbType.Int) { Value = cuentaId });
                        await cmdUpdSaldo.ExecuteNonQueryAsync();
                    }

                    // Insertar transacción (si existe la tabla Transaccion con estos campos)
                    using (var cmdTrans = conn.CreateCommand())
                    {
                        cmdTrans.Transaction = tx;
                        cmdTrans.CommandText = @"INSERT INTO Transaccion (CuentaID, FechaHora, TipoTransaccionID, Monto, CajeroID, NumeroTarjeta) VALUES (@cuenta, @fecha, @tipo, @monto, @cajero, @num)";
                        cmdTrans.Parameters.Add(new SqlParameter("@cuenta", SqlDbType.Int) { Value = cuentaId });
                        cmdTrans.Parameters.Add(new SqlParameter("@fecha", SqlDbType.DateTime2) { Value = DateTime.Now });
                        cmdTrans.Parameters.Add(new SqlParameter("@tipo", SqlDbType.TinyInt) { Value = (int)TipoTransaccion.Deposito });
                        cmdTrans.Parameters.Add(new SqlParameter("@monto", SqlDbType.Decimal) { Value = montoTotal });
                        cmdTrans.Parameters.Add(new SqlParameter("@cajero", SqlDbType.Int) { Value = cajeroId.Value });
                        cmdTrans.Parameters.Add(new SqlParameter("@num", SqlDbType.NVarChar) { Value = _usuario.NumeroTarjeta });
                        await cmdTrans.ExecuteNonQueryAsync();
                    }

                    tx.Commit();

                    // Actualizar objeto usuario en memoria: saldo
                    _usuario.SaldoActual += montoTotal;
                    this.UsuarioActualizado = _usuario;

                    MessageBox.Show($"Depósito de {montoTotal:C} realizado con éxito.\n\nNuevo saldo: {_usuario.SaldoActual:C}", "Depósito Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (SqlException sqlEx)
                {
                    MessageBox.Show("Error de base de datos durante el depósito: " + sqlEx.Message, "Error de Operación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ocurrió un error crítico durante el depósito: " + ex.Message, "Error de Operación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else // Retiro
            {
                try
                {
                    var conexion = new ConexionBd();
                    using var conn = await conexion.OpenConnectionAsync();
                    using var tx = conn.BeginTransaction();

                    // Obtener estado del cajero
                    int? cajeroId = AppState.CurrentCajeroId;
                    if (!cajeroId.HasValue)
                    {
                        using var cmdFind = conn.CreateCommand();
                        cmdFind.Transaction = tx;
                        cmdFind.CommandText = "SELECT TOP(1) CajeroID FROM Cajero WHERE Activo = 1";
                        var res = await cmdFind.ExecuteScalarAsync();
                        if (res != null && res != DBNull.Value) cajeroId = Convert.ToInt32(res);
                    }
                    if (!cajeroId.HasValue)
                    {
                        tx.Rollback();
                        MessageBox.Show("No se encontró un cajero configurado. Contacte a un administrador.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Calcular monto total y validar saldo y límites
                    if (montoTotal > _usuario.SaldoActual)
                    {
                        MessageBox.Show($"No tiene saldo suficiente para retirar {montoTotal:C}. Su saldo actual es {_usuario.SaldoActual:C}.", "Saldo Insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    decimal disponibleHoy = _usuario.MontoMaximoDiario - _montoRetiradoHoy;
                    if (montoTotal > disponibleHoy)
                    {
                        MessageBox.Show($"Esta operación excede su límite de retiro diario. Solo puede retirar hasta {disponibleHoy:C} más por hoy.", "Límite Diario Excedido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Obtener cantidades actuales por denominación
                    using var cmdInv = conn.CreateCommand();
                    cmdInv.Transaction = tx;
                    cmdInv.CommandText = @"SELECT d.Valor, ISNULL(i.Cantidad,0) AS Cantidad FROM Denominacion d LEFT JOIN InventarioEfectivo i ON d.DenominacionID = i.DenominacionID AND i.CajeroID = @cajero WHERE d.Valor IN (200,100,50,20,10,5,1)";
                    cmdInv.Parameters.Add(new SqlParameter("@cajero", SqlDbType.Int) { Value = cajeroId.Value });
                    var denomCurrent = new Dictionary<int,int>();
                    using (var rdr = await cmdInv.ExecuteReaderAsync())
                    {
                        while (await rdr.ReadAsync())
                        {
                            var val = Convert.ToInt32(rdr.GetDecimal(rdr.GetOrdinal("Valor")));
                            var cant = rdr.GetInt32(rdr.GetOrdinal("Cantidad"));
                            denomCurrent[val] = cant;
                        }
                        rdr.Close();
                    }

                    // Verificar disponibilidad de billetes
                    if (denomCurrent.GetValueOrDefault(200,0) < (int)num200.Value || denomCurrent.GetValueOrDefault(100,0) < (int)num100.Value ||
                        denomCurrent.GetValueOrDefault(50,0) < (int)num50.Value || denomCurrent.GetValueOrDefault(20,0) < (int)num20.Value ||
                        denomCurrent.GetValueOrDefault(10,0) < (int)num10.Value || denomCurrent.GetValueOrDefault(5,0) < (int)num5.Value ||
                        denomCurrent.GetValueOrDefault(1,0) < (int)num1.Value)
                    {
                        tx.Rollback();
                        MessageBox.Show("El cajero no dispone de la cantidad de billetes solicitada para una o más denominaciones. Por favor, intente un desglose diferente.", "Billetes no Disponibles", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Restar billetes en InventarioEfectivo
                    var deductions = new Dictionary<decimal,int>
                    {
                        {200m, (int)num200.Value},
                        {100m, (int)num100.Value},
                        {50m, (int)num50.Value},
                        {20m, (int)num20.Value},
                        {10m, (int)num10.Value},
                        {5m, (int)num5.Value},
                        {1m, (int)num1.Value}
                    };

                    // Obtener denominacion ids
                    var denomIdsRet = new Dictionary<decimal, int>();
                    using (var cmdDen = conn.CreateCommand())
                    {
                        cmdDen.Transaction = tx;
                        cmdDen.CommandText = "SELECT DenominacionID, Valor FROM Denominacion WHERE Valor IN (200,100,50,20,10,5,1)";
                        using var rdr = await cmdDen.ExecuteReaderAsync();
                        while (await rdr.ReadAsync())
                        {
                            var id = rdr.GetInt32(rdr.GetOrdinal("DenominacionID"));
                            var val = rdr.GetDecimal(rdr.GetOrdinal("Valor"));
                            denomIdsRet[val] = id;
                        }
                        rdr.Close();
                    }

                    foreach (var kv in deductions)
                    {
                        if (kv.Value <= 0) continue;
                        int denomId = denomIdsRet[kv.Key];
                        using var cmdUp = conn.CreateCommand();
                        cmdUp.Transaction = tx;
                        cmdUp.CommandText = @"UPDATE InventarioEfectivo SET Cantidad = Cantidad - @cant WHERE CajeroID = @cajero AND DenominacionID = @den";
                        cmdUp.Parameters.Add(new SqlParameter("@cant", SqlDbType.Int) { Value = kv.Value });
                        cmdUp.Parameters.Add(new SqlParameter("@cajero", SqlDbType.Int) { Value = cajeroId.Value });
                        cmdUp.Parameters.Add(new SqlParameter("@den", SqlDbType.Int) { Value = denomId });
                        await cmdUp.ExecuteNonQueryAsync();
                    }

                    // Actualizar saldo de cuenta
                    int cuentaId;
                    using (var cmdCuenta = conn.CreateCommand())
                    {
                        cmdCuenta.Transaction = tx;
                        cmdCuenta.CommandText = @"SELECT TOP(1) c.CuentaID FROM Tarjeta t INNER JOIN Cuenta c ON t.CuentaID = c.CuentaID WHERE t.NumeroTarjeta = @num";
                        cmdCuenta.Parameters.Add(new SqlParameter("@num", SqlDbType.NVarChar) { Value = _usuario.NumeroTarjeta });
                        var obj = await cmdCuenta.ExecuteScalarAsync();
                        if (obj == null || obj == DBNull.Value)
                        {
                            tx.Rollback();
                            MessageBox.Show("Cuenta asociada a la tarjeta no encontrada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        cuentaId = Convert.ToInt32(obj);
                    }

                    using (var cmdUpdSaldo = conn.CreateCommand())
                    {
                        cmdUpdSaldo.Transaction = tx;
                        cmdUpdSaldo.CommandText = "UPDATE Cuenta SET SaldoActual = SaldoActual - @monto WHERE CuentaID = @cuenta";
                        cmdUpdSaldo.Parameters.Add(new SqlParameter("@monto", SqlDbType.Decimal) { Value = montoTotal });
                        cmdUpdSaldo.Parameters.Add(new SqlParameter("@cuenta", SqlDbType.Int) { Value = cuentaId });
                        await cmdUpdSaldo.ExecuteNonQueryAsync();
                    }

                    // Insertar registro de transaccion
                    using (var cmdTrans = conn.CreateCommand())
                    {
                        cmdTrans.Transaction = tx;
                        cmdTrans.CommandText = @"INSERT INTO Transaccion (CuentaID, FechaHora, TipoTransaccionID, Monto, CajeroID, NumeroTarjeta) VALUES (@cuenta, @fecha, @tipo, @monto, @cajero, @num)";
                        cmdTrans.Parameters.Add(new SqlParameter("@cuenta", SqlDbType.Int) { Value = cuentaId });
                        cmdTrans.Parameters.Add(new SqlParameter("@fecha", SqlDbType.DateTime2) { Value = DateTime.Now });
                        cmdTrans.Parameters.Add(new SqlParameter("@tipo", SqlDbType.TinyInt) { Value = (int)TipoTransaccion.Retiro });
                        cmdTrans.Parameters.Add(new SqlParameter("@monto", SqlDbType.Decimal) { Value = montoTotal });
                        cmdTrans.Parameters.Add(new SqlParameter("@cajero", SqlDbType.Int) { Value = cajeroId.Value });
                        cmdTrans.Parameters.Add(new SqlParameter("@num", SqlDbType.NVarChar) { Value = _usuario.NumeroTarjeta });
                        await cmdTrans.ExecuteNonQueryAsync();
                    }

                    tx.Commit();

                    // Actualizar objeto usuario en memoria: saldo
                    _usuario.SaldoActual -= montoTotal;
                    this.UsuarioActualizado = _usuario;

                    MessageBox.Show($"Retiro de {montoTotal:C} realizado con éxito.\n\nNuevo saldo: {_usuario.SaldoActual:C}", "Retiro Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (SqlException sqlEx)
                {
                    MessageBox.Show("Error de base de datos durante el retiro: " + sqlEx.Message, "Error de Operación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ocurrió un error crítico durante el retiro: " + ex.Message, "Error de Operación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
