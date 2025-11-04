using System;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace proyectoCajero
{
    public partial class CambiarPinForm : Form
    {
        private Usuario _usuario;
        public CambiarPinForm(Usuario usuario)
        {
            InitializeComponent();
            _usuario = usuario;
        }

        private async void btnAceptar_Click(object sender, EventArgs e)
        {
            string pinActualIngresado = txtPinActual.Text?.Trim() ?? string.Empty;
            string pinNuevo = txtPinNuevo.Text?.Trim() ?? string.Empty;
            string pinConfirmar = txtPinConfirmar.Text?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(pinActualIngresado) || string.IsNullOrWhiteSpace(pinNuevo) || string.IsNullOrWhiteSpace(pinConfirmar))
            {
                MessageBox.Show("Por favor complete todos los campos de PIN.", "Datos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Validar formato del nuevo PIN
            if (pinNuevo.Length != 4 || !int.TryParse(pinNuevo, out _))
            {
                MessageBox.Show("El nuevo PIN debe contener exactamente 4 dígitos numéricos.", "Formato Inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 3. Validar que el nuevo PIN no sea igual al actual (we'll check after fetching current hash)
            if (pinNuevo == pinActualIngresado)
            {
                MessageBox.Show("El nuevo PIN no puede ser igual al actual.", "PIN Inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 4. Validar que los nuevos PINs coincidan
            if (pinNuevo != pinConfirmar)
            {
                MessageBox.Show("El nuevo PIN y su confirmación no coinciden.", "Error de Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Proceed with DB update
            try
            {
                var conexion = new ConexionBd();
                using var conn = await conexion.OpenConnectionAsync();
                using var tx = conn.BeginTransaction();
                try
                {
                    // Get current pin hash for tarjeta
                    using var cmdGet = conn.CreateCommand();
                    cmdGet.Transaction = tx;
                    cmdGet.CommandText = "SELECT TOP(1) TarjetaID, PinHash FROM Tarjeta WHERE NumeroTarjeta = @num";
                    cmdGet.Parameters.Add(new SqlParameter("@num", System.Data.SqlDbType.VarChar) { Value = _usuario.NumeroTarjeta });

                    using var reader = await cmdGet.ExecuteReaderAsync();
                    if (!await reader.ReadAsync())
                    {
                        reader.Close();
                        tx.Rollback();
                        MessageBox.Show("No se encontró la tarjeta asociada al usuario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    int tarjetaId = reader.GetInt32(reader.GetOrdinal("TarjetaID"));
                    string pinHashActual = reader.IsDBNull(reader.GetOrdinal("PinHash")) ? string.Empty : reader.GetString(reader.GetOrdinal("PinHash"));
                    reader.Close();

                    bool pinIsHashed = !string.IsNullOrEmpty(pinHashActual) && pinHashActual.Length == 64 && System.Text.RegularExpressions.Regex.IsMatch(pinHashActual, "^[0-9a-fA-F]{64}$");
                    bool valido = false;
                    if (pinIsHashed)
                    {
                        valido = HashHelper.VerifySha256Hash(pinActualIngresado, pinHashActual);
                    }
                    else
                    {
                        // fallback to plaintext compare if DB stored plain PIN (legacy)
                        valido = pinActualIngresado == pinHashActual;
                    }

                    if (!valido)
                    {
                        tx.Rollback();
                        MessageBox.Show("El PIN actual es incorrecto.", "Error de Verificación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Compute new hash and update Tarjeta
                    string nuevoHash = HashHelper.ComputeSha256Hash(pinNuevo);
                    using var cmdUpdate = conn.CreateCommand();
                    cmdUpdate.Transaction = tx;
                    cmdUpdate.CommandText = "UPDATE Tarjeta SET PinHash = @new WHERE TarjetaID = @id";
                    cmdUpdate.Parameters.Add(new SqlParameter("@new", System.Data.SqlDbType.NVarChar) { Value = nuevoHash });
                    cmdUpdate.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.Int) { Value = tarjetaId });
                    await cmdUpdate.ExecuteNonQueryAsync();

                    // Insert audit log into LogCambioPin with correct columns (FuenteCambio, UsuarioID or EmpleadoID)
                    try
                    {
                        using var cmdLog = conn.CreateCommand();
                        cmdLog.Transaction = tx;
                        cmdLog.CommandText = @"INSERT INTO LogCambioPin (TarjetaID, FechaHoraCambio, FuenteCambio, UsuarioID, EmpleadoID)
VALUES (@tarjetaId, SYSUTCDATETIME(), @fuente, @usuarioId, @empleadoId)";
                        cmdLog.Parameters.Add(new SqlParameter("@tarjetaId", System.Data.SqlDbType.Int) { Value = tarjetaId });
                        // Determine source and associated user/employee
                        string fuente = AppState.CurrentEmpleadoId.HasValue ? "Empleado" : "Usuario";
                        object usuarioIdObj = AppState.CurrentEmpleadoId.HasValue ? DBNull.Value : (object)_usuario.Id;
                        object empleadoIdObj = AppState.CurrentEmpleadoId.HasValue ? (object)AppState.CurrentEmpleadoId.Value : DBNull.Value;
                        cmdLog.Parameters.Add(new SqlParameter("@fuente", System.Data.SqlDbType.NVarChar) { Value = fuente });
                        cmdLog.Parameters.Add(new SqlParameter("@usuarioId", System.Data.SqlDbType.Int) { Value = usuarioIdObj });
                        cmdLog.Parameters.Add(new SqlParameter("@empleadoId", System.Data.SqlDbType.Int) { Value = empleadoIdObj });
                        await cmdLog.ExecuteNonQueryAsync();
                    }
                    catch
                    {
                        // ignore logging errors to not block PIN change
                    }

                    tx.Commit();
                    MessageBox.Show("Su PIN ha sido cambiado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                catch (Exception exTx)
                {
                    try { tx.Rollback(); } catch { }
                    MessageBox.Show("Error al actualizar PIN: " + exTx.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al actualizar el PIN: " + ex.Message, "Error Crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
