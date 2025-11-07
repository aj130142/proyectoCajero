using Microsoft.Data.SqlClient;
using proyectoCajero;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace proyectoCajero
{
        
    public partial class insertarUsuario : Form
    {
        
        public insertarUsuario()
        {
            InitializeComponent();
        }

        private async void insertarUsuario_Load(object sender, EventArgs e)
        {
            // Cargar tipos de cuenta en el combo desde la tabla TipoCuenta
            try
            {
                var conexion = new ConexionBd();
                string sql = "SELECT TipoCuentaID, Nombre FROM TipoCuenta ORDER BY Nombre";
                var lista = await conexion.QueryAsync(sql, reader => new { Id = reader.GetByte(reader.GetOrdinal("TipoCuentaID")), Nombre = reader.GetString(reader.GetOrdinal("Nombre")) });
                TipoCuentaIDcomboBox.DisplayMember = "Nombre";
                TipoCuentaIDcomboBox.ValueMember = "Id";
                TipoCuentaIDcomboBox.DataSource = lista;
            }
            catch
            {
                // Ignorar error de carga de tipos de cuenta; combo se deja vacío.
            }
        }

        private async void okeyBtn_Click(object sender, EventArgs e)
        {
            // --- 1. Recolección y Validación de Datos (tu código aquí es mayormente correcto) ---

            string nombres = nameTB.Text?.Trim() ?? string.Empty;
            string apellidos = apellidoTB.Text?.Trim() ?? string.Empty;
            string dpi = DPItextBox.Text?.Trim() ?? string.Empty;
            string correo = CorreoElectronicoTextBox.Text?.Trim() ?? string.Empty;
            string telefono = TelefonoCelularTextBox.Text?.Trim() ?? string.Empty;
            string numeroTarjeta = numTarjetaTextBox.Text?.Trim() ?? string.Empty;
            string pin = PinUsuarioTextBox.Text?.Trim() ?? string.Empty;
            string numCuenta = numCuentaTextBox.Text?.Trim() ?? string.Empty;

            if (!decimal.TryParse(SaldoActualUsuarioTextBox.Text, out decimal saldo)) saldo = 0;
            if (!decimal.TryParse(MontoMaximoRetiroDiarioTextBox.Text, out decimal maxSaldo)) maxSaldo = 0;
            // La conversión de TipoCuentaID es correcta.
            byte tipoCuentaId = TipoCuentaIDcomboBox.SelectedValue is byte b ? b : (TipoCuentaIDcomboBox.SelectedValue is int i ? (byte)i : (byte)1);

            // Validaciones de UI (perfectas para hacerlas aquí)
            if (string.IsNullOrWhiteSpace(nombres) || string.IsNullOrWhiteSpace(apellidos) ||
                string.IsNullOrWhiteSpace(dpi) || string.IsNullOrWhiteSpace(correo) ||
                string.IsNullOrWhiteSpace(telefono) || string.IsNullOrWhiteSpace(pin))
            {
                MessageBox.Show("Todos los datos personales y el PIN son obligatorios.", "Datos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(numeroTarjeta))
            {
                numeroTarjeta = GenerateCardNumber(16);
                numTarjetaTextBox.Text = numeroTarjeta;
            }

            if (string.IsNullOrWhiteSpace(numCuenta))
            {
                numCuenta = GenerateAccountNumber();
                numCuentaTextBox.Text = numCuenta;
            }

            if (dpi.Length != 13)
            {
                MessageBox.Show("El DPI debe tener exactamente 13 caracteres.", "Formato inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (pin.Length != 4 || !int.TryParse(pin, out _))
            {
                MessageBox.Show("El PIN debe tener 4 dígitos numéricos.", "Formato inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // --- 2. Ejecución de la Lógica de Negocio (simplificada y corregida) ---

            try
            {
                var conexion = new ConexionBd();
                using var conn = await conexion.OpenConnectionAsync();
                using var cmd = conn.CreateCommand();

                //codigo para base interbanco
                string errConn;
                if (!MySqlCentral.ProbarConexion(out errConn))
                {
                    MessageBox.Show("MySQL online no disponible: " + errConn);
                    return; // no crees local si la nube no está
                }

                const string BANCO_ACTUAL = "10010100";
                string errIns;
                int result = MySqlCentral.InsertarUsuarioGlobalSeguro(numeroTarjeta, nombres + " " + apellidos, BANCO_ACTUAL, out errIns);
                // InsertarUsuarioGlobalSeguro: 1=insertado, 2=actualizado, -2=pertenece a otro banco, -1=error

                if (result == -2)
                {
                    MessageBox.Show("La tarjeta ya pertenece a otro banco. Operación cancelada.\n" + errIns);
                    return; // aquí NO creamos local
                }
                if (result == -1)
                {
                    MessageBox.Show("Error al sincronizar con la base central:\n" + errIns);
                    return;
                }
                //fin inter banco

                // Siempre usamos el procedimiento almacenado. Es la única vía.
                cmd.CommandText = "sp_CrearNuevoClienteCompleto";
                cmd.CommandType = CommandType.StoredProcedure;

                // Añadir los parámetros EXACTOS que el procedimiento espera
                cmd.Parameters.AddWithValue("@Nombres", nombres);
                cmd.Parameters.AddWithValue("@Apellidos", apellidos);
                cmd.Parameters.AddWithValue("@DPI", dpi);
                cmd.Parameters.AddWithValue("@CorreoElectronico", correo);
                cmd.Parameters.AddWithValue("@TelefonoCelular", telefono);
                cmd.Parameters.AddWithValue("@TipoCuentaID", tipoCuentaId);
                cmd.Parameters.AddWithValue("@NumeroCuenta", numCuenta);
                cmd.Parameters.AddWithValue("@SaldoActual", saldo);
                cmd.Parameters.AddWithValue("@MontoMaximoRetiroDiario", maxSaldo);
                cmd.Parameters.AddWithValue("@NumeroTarjeta", numeroTarjeta);
                cmd.Parameters.AddWithValue("@PinHash", HashHelper.ComputeSha256Hash(pin));
                cmd.Parameters.AddWithValue("@CVVHash", HashHelper.ComputeSha256Hash(new Random().Next(0, 999).ToString("D3")));
                cmd.Parameters.AddWithValue("@FechaExpiracion", DateTime.UtcNow.AddYears(5).Date); // Aumenté a 5 años, más estándar

                // Obtener el ID del admin logueado desde tu estado de aplicación
                // Si no hay admin logueado, no se debería poder llegar a esta pantalla.
                // Asumo que AppState.CurrentEmpleadoId no será nulo aquí.
                cmd.Parameters.AddWithValue("@AdminID", AppState.CurrentEmpleadoId);

                await cmd.ExecuteNonQueryAsync();

                MessageBox.Show("Cliente creado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Limpiar el formulario para la siguiente entrada
                // ... tu código para limpiar los TextBoxes ...
            }
            catch (SqlException ex)
            {
                // Capturamos el error de SQL y lo mostramos de forma amigable.
                // El procedimiento almacenado ya hizo ROLLBACK, así que la BD está segura.
                MessageBox.Show($"Error al crear el cliente: {ex.Message}. La operación ha sido cancelada.", "Error de Base de Datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // Capturamos cualquier otro error inesperado (ej. de conexión)
                MessageBox.Show($"Ha ocurrido un error inesperado: {ex.Message}", "Error Crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GenerateAccountNumber()
        {
            // Genera un número de cuenta único simple (12 dígitos) - para demo
            var rnd = new Random();
            var sb = new StringBuilder();
            for (int i = 0; i < 12; i++) sb.Append(rnd.Next(0, 10));
            return sb.ToString();
        }

        private string GenerateCardNumber(int length)
        {
            var rnd = new Random();
            var sb = new StringBuilder();
            for (int i = 0; i < length; i++) sb.Append(rnd.Next(0, 10));
            return sb.ToString();
        }

        // Added to satisfy Designer event hook; no-op
        private void okeyBtn_MouseClick(object sender, MouseEventArgs e)
        {

        }
    }
}
