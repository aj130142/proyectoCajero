using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using System.Data;

namespace proyectoCajero
{
    public partial class TransferenciasInternas : Form
    {
        // ... (Tus variables miembro _usuario, _conexion, etc. están perfectas) ...
        private readonly Usuario _usuario;
        private readonly ConexionBd _conexion;
        private string _tokenValido = string.Empty;
        private List<CuentaInfo> _cuentasUsuario;

        public TransferenciasInternas(Usuario usuario)
        {
            InitializeComponent();
            _usuario = usuario;
            _conexion = new ConexionBd();
            _cuentasUsuario = new List<CuentaInfo>();
        }

        // --- MÉTODOS DE INICIALIZACIÓN Y UI (Sin cambios, ya estaban bien) ---

        private async void TransferenciasInternas_Load(object sender, EventArgs e)
        {
            await CargarCuentasUsuario();
            // CORRECCIÓN MENOR: El botón debe estar habilitado al inicio
            // y las validaciones lo deshabilitarán si es necesario.
            btnTransferir.Enabled = true;
        }

        private async System.Threading.Tasks.Task CargarCuentasUsuario()
        {
            // ... (Este método está perfecto, no necesita cambios) ...
            try
            {
                // Consulta para obtener las cuentas del usuario actual
                string sql = @"
                    SELECT c.CuentaID, c.NumeroCuenta, c.SaldoActual, tc.Nombre as TipoCuenta
                    FROM Cuenta c
                    INNER JOIN Usuario u ON c.UsuarioID = u.UsuarioID
                    INNER JOIN TipoCuenta tc ON c.TipoCuentaID = tc.TipoCuentaID
                    WHERE u.UsuarioID = @UsuarioID AND c.EstadoCuentaID = 1
                    ORDER BY c.NumeroCuenta";

                var parametros = new List<SqlParameter>
                {
                    new SqlParameter("@UsuarioID", SqlDbType.Int) { Value = _usuario.Id }
                };

                _cuentasUsuario = await _conexion.QueryAsync(sql, reader => new CuentaInfo
                {
                    CuentaID = reader.GetInt32(reader.GetOrdinal("CuentaID")),
                    NumeroCuenta = reader.GetString(reader.GetOrdinal("NumeroCuenta")),
                    SaldoActual = reader.GetDecimal(reader.GetOrdinal("SaldoActual")),
                    TipoCuenta = reader.GetString(reader.GetOrdinal("TipoCuenta"))
                }, parametros);

                // Cargar el ComboBox
                cuentaDebitarCB.Items.Clear();
                foreach (var cuenta in _cuentasUsuario)
                {
                    cuentaDebitarCB.Items.Add($"{cuenta.NumeroCuenta} - {cuenta.TipoCuenta}");
                }

                if (cuentaDebitarCB.Items.Count > 0)
                {
                    cuentaDebitarCB.SelectedIndex = 0;
                }
                else
                {
                    MessageBox.Show("No tienes cuentas activas disponibles para transferir.",
                        "Sin Cuentas", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las cuentas: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cuentaDebitarCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            // ... (Este método está perfecto, no necesita cambios) ...
            if (cuentaDebitarCB.SelectedIndex >= 0 && cuentaDebitarCB.SelectedIndex < _cuentasUsuario.Count)
            {
                var cuentaSeleccionada = _cuentasUsuario[cuentaDebitarCB.SelectedIndex];
                SaldoCuentaDebitarLabel.Text = $"Saldo: Q{cuentaSeleccionada.SaldoActual:N2}";
                ActualizarMontoRestante();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // ... (Este método está perfecto, no necesita cambios) ...
            ActualizarMontoRestante();
        }

        private void ActualizarMontoRestante()
        {
            // ... (Este método está perfecto, no necesita cambios) ...
            if (cuentaDebitarCB.SelectedIndex >= 0 &&
                cuentaDebitarCB.SelectedIndex < _cuentasUsuario.Count &&
                decimal.TryParse(textBox1.Text, out decimal montoTransferir))
            {
                var cuentaSeleccionada = _cuentasUsuario[cuentaDebitarCB.SelectedIndex];
                decimal saldoRestante = cuentaSeleccionada.SaldoActual - montoTransferir;
                MontoretirarRestanteLabel.Text = $"Saldo restante: Q{saldoRestante:N2}";

                if (saldoRestante < 0)
                {
                    MontoretirarRestanteLabel.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    MontoretirarRestanteLabel.ForeColor = System.Drawing.Color.Green;
                }
            }
            else
            {
                MontoretirarRestanteLabel.Text = "Saldo restante: Q0.00";
                MontoretirarRestanteLabel.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void btnGenerarToken_Click(object sender, EventArgs e)
        {
            // !!! ADVERTENCIA DE SEGURIDAD !!!
            // Este método DEBE ser refactorizado para generar, guardar y enviar
            // el token desde el servidor, como discutimos.
            // Por ahora, lo mantenemos como está para que el flujo funcione.
            using (var tokenForm = new TokenGeneratorForm())
            {
                if (tokenForm.ShowDialog() == DialogResult.OK)
                {
                    _tokenValido = tokenForm.TokenGenerado;
                    tokenTextbox.Text = _tokenValido;
                    MessageBox.Show($"Token generado correctamente. Por favor, ingrésalo en el campo de Token.",
                        "Token Generado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }


        // --- ***** INICIO DE LAS MODIFICACIONES IMPORTANTES ***** ---

        private async void btnTransferir_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos())
            {
                return;
            }

            btnTransferir.Enabled = false;
            Cursor = Cursors.WaitCursor;

            try
            {
                // Obtener datos del formulario
                var cuentaOrigen = _cuentasUsuario[cuentaDebitarCB.SelectedIndex];
                string numeroCuentaDestino = cuentaCreditarLabel.Text.Trim();
                decimal monto = decimal.Parse(textBox1.Text);
                string descripcion = Descripcion.Text.Trim();

                // --- MODIFICACIÓN 1: Se elimina la llamada a 'ValidarCuentaDestino' ---
                // El nuevo procedimiento almacenado se encarga de esta validación internamente.

                var confirmResult = MessageBox.Show(
                    $"¿Está seguro de que desea realizar la siguiente transferencia?\n\n" +
                    $"De la cuenta: {cuentaOrigen.NumeroCuenta}\n" +
                    $"A la cuenta: {numeroCuentaDestino}\n" +
                    $"Monto: Q{monto:N2}",
                    "Confirmar Transferencia",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmResult != DialogResult.Yes)
                {
                    // Se añade un return dentro del 'finally' para que no se quede colgado
                    // si el usuario dice que no.
                    return;
                }

                // --- MODIFICACIÓN 2: Se llama al nuevo método EjecutarTransferencia ---
                // que ahora acepta los números de cuenta (string) directamente.
                await EjecutarTransferencia(cuentaOrigen.NumeroCuenta, numeroCuentaDestino, monto, descripcion);

                MessageBox.Show("¡Transferencia realizada exitosamente!",
                    "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimpiarFormulario();
                await CargarCuentasUsuario();
            }
            catch (SqlException sqlEx)
            {
                string mensaje = sqlEx.Number switch
                {
                    50001 => "Error: La cuenta de origen y destino no pueden ser la misma.",
                    50002 => "Error: El monto de la transferencia debe ser mayor a cero.",
                    50003 => "Error: El número de cuenta de origen no existe.",
                    50004 => "Error: La cuenta de origen no tiene fondos suficientes.",
                    50005 => "Error: El número de cuenta de destino no existe o no está activa.",
                    _ => $"Ha ocurrido un error inesperado en la base de datos. Por favor, contacte a soporte.\n\nCódigo de error: {sqlEx.Number}\nMensaje: {sqlEx.Message}"
                };
                MessageBox.Show(mensaje, "Error de Transferencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ha ocurrido un error inesperado en la aplicación: {ex.Message}",
                    "Error General", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
                btnTransferir.Enabled = true;
            }
        }

        private bool ValidarCampos()
        {
            // ... (Este método está perfecto, no necesita cambios) ...
            // Validar cuenta origen
            if (cuentaDebitarCB.SelectedIndex < 0)
            {
                MessageBox.Show("Debe seleccionar una cuenta de origen.",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cuentaDebitarCB.Focus();
                return false;
            }

            // Validar cuenta destino
            if (string.IsNullOrWhiteSpace(cuentaCreditarLabel.Text))
            {
                MessageBox.Show("Debe ingresar el número de cuenta destino.",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cuentaCreditarLabel.Focus();
                return false;
            }

            // Validar que no sean la misma cuenta
            var cuentaOrigen = _cuentasUsuario[cuentaDebitarCB.SelectedIndex];
            if (cuentaOrigen.NumeroCuenta == cuentaCreditarLabel.Text.Trim())
            {
                MessageBox.Show("La cuenta de origen y destino no pueden ser la misma.",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Validar monto
            if (!decimal.TryParse(textBox1.Text, out decimal monto) || monto <= 0)
            {
                MessageBox.Show("Debe ingresar un monto válido mayor a cero.",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox1.Focus();
                return false;
            }

            // Validar saldo suficiente
            if (monto > cuentaOrigen.SaldoActual)
            {
                MessageBox.Show("El monto excede el saldo disponible en la cuenta.",
                    "Saldo Insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox1.Focus();
                return false;
            }

            // Validar descripción
            if (string.IsNullOrWhiteSpace(Descripcion.Text))
            {
                MessageBox.Show("Debe ingresar una descripción para la transferencia.",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Descripcion.Focus();
                return false;
            }

            // Validar token
            if (string.IsNullOrWhiteSpace(tokenTextbox.Text))
            {
                MessageBox.Show("Debe generar e ingresar el token de seguridad.",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btnGenerarToken.Focus();
                return false;
            }

            if (tokenTextbox.Text.Trim() != _tokenValido)
            {
                MessageBox.Show("El token ingresado no es válido. Por favor, genere uno nuevo.",
                    "Token Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tokenTextbox.Clear();
                _tokenValido = string.Empty;
                btnGenerarToken.Focus();
                return false;
            }

            return true;
        }

        // --- MODIFICACIÓN 3: El método 'ValidarCuentaDestino' ya no es necesario. ---
        // Puedes eliminarlo para limpiar el código.

        // private async System.Threading.Tasks.Task<int> ValidarCuentaDestino(string numeroCuenta) { ... }

        // --- MODIFICACIÓN 4: El método 'EjecutarTransferencia' ahora acepta strings. ---
        private async System.Threading.Tasks.Task EjecutarTransferencia(string numeroCuentaOrigen, string numeroCuentaDestino, decimal monto, string descripcion)
        {
            using (var conn = await _conexion.OpenConnectionAsync())
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "dbo.sp_RealizarTransferencia";
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Los parámetros ahora son los números de cuenta (VARCHAR)
                    cmd.Parameters.Add(new SqlParameter("@NumeroCuentaOrigen", SqlDbType.VarChar, 20) { Value = numeroCuentaOrigen });
                    cmd.Parameters.Add(new SqlParameter("@NumeroCuentaDestino", SqlDbType.VarChar, 20) { Value = numeroCuentaDestino });
                    cmd.Parameters.Add(new SqlParameter("@Monto", SqlDbType.Decimal) { Value = monto, Precision = 18, Scale = 2 });
                    cmd.Parameters.Add(new SqlParameter("@Descripcion", SqlDbType.NVarChar, 100) { Value = descripcion });

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        private void LimpiarFormulario()
        {
            // ... (Este método está perfecto, no necesita cambios) ...
            cuentaCreditarLabel.Clear();
            textBox1.Clear();
            Descripcion.Clear();
            tokenTextbox.Clear();
            _tokenValido = string.Empty;
            MontoretirarRestanteLabel.Text = "Saldo restante: Q0.00";
            MontoretirarRestanteLabel.ForeColor = System.Drawing.Color.Black;
        }

        private void btnAgregarCuenta_Click(object sender, EventArgs e)
        {
            // ...
        }

        private class CuentaInfo
        {
            // ... (Esta clase está perfecta, no necesita cambios) ...
            public int CuentaID { get; set; }
            public string NumeroCuenta { get; set; } = string.Empty;
            public decimal SaldoActual { get; set; }
            public string TipoCuenta { get; set; } = string.Empty;
        }

        private void historialBtn_Click(object sender, EventArgs e)
        {
            HistorialtransaccionCuenta historial = new HistorialtransaccionCuenta(_usuario);
            historial.ShowDialog();
        }
    }
}