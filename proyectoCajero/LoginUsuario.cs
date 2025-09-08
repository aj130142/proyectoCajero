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

namespace proyectoCajero
{
    public partial class LoginUsuario : Form
    {
        public Usuario UsuarioAutenticado { get; private set; }
        public LoginUsuario()
        {
            InitializeComponent();
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            string numeroTarjeta = txtNumeroTarjeta.Text;
            string pin = txtPin.Text;

            // Validaciones básicas de entrada
            if (string.IsNullOrWhiteSpace(numeroTarjeta) || string.IsNullOrWhiteSpace(pin))
            {
                MessageBox.Show("Por favor, ingrese el número de tarjeta y el PIN.", "Datos Incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Cargar la lista de usuarios para verificar
            string pathUsuarios = direccione.obtenerRutasTxt("usuario.txt");
            List<Usuario> listaUsuarios = ManejadorArchivosUsuario.LeerUsuarios(pathUsuarios);

            // Buscar al usuario por número de tarjeta Y PIN
            Usuario usuarioEncontrado = listaUsuarios.FirstOrDefault(u => u.NumeroTarjeta == numeroTarjeta && u.PIN == pin);

            if (usuarioEncontrado != null)
            {
                // ¡Éxito! El usuario es válido.
                this.UsuarioAutenticado = usuarioEncontrado;

                // Establecemos el DialogResult en OK. Esto es útil para que el formulario
                // que lo abrió sepa que la autenticación fue exitosa.
                this.DialogResult = DialogResult.OK;

                // ¡Éxito! El usuario es válido.
                this.UsuarioAutenticado = usuarioEncontrado;

                // Ocultamos el formulario de login en lugar de cerrarlo inmediatamente
                this.Hide();

                // Creamos una instancia del menú principal y le pasamos el usuario autenticado
                MenuUsuario menu = new MenuUsuario(this.UsuarioAutenticado);

                // Mostramos el menú. Usar ShowDialog asegura que el control vuelva aquí cuando se cierre.
                menu.ShowDialog();

                // Cuando el usuario haga clic en "Salir" en el menú, el control volverá aquí.
                // En ese punto, cerramos el formulario de login, lo que a su vez devolverá
                // el control al Form1, que volverá a mostrarse.
                this.Close();
            }
            else
            {
                // Error: Los datos no coinciden.
                MessageBox.Show("Número de tarjeta o PIN incorrecto. Por favor, intente de nuevo.", "Error de Autenticación", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Limpiamos los campos para un nuevo intento
                txtPin.Clear();
                txtNumeroTarjeta.Focus(); // Ponemos el foco de nuevo en el número de tarjeta
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
