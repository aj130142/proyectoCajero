using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            string pinActualIngresado = txtPinActual.Text;
            string pinNuevo = txtPinNuevo.Text;
            string pinConfirmar = txtPinConfirmar.Text;

            // 1. Validar PIN Actual
            if (pinActualIngresado != _usuario.PIN)
            {
                MessageBox.Show("El PIN actual es incorrecto.", "Error de Verificación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 2. Validar formato del nuevo PIN
            if (pinNuevo.Length != 4 || !int.TryParse(pinNuevo, out _))
            {
                MessageBox.Show("El nuevo PIN debe contener exactamente 4 dígitos numéricos.", "Formato Inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 3. Validar que el nuevo PIN no sea igual al actual
            if (pinNuevo == _usuario.PIN)
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

            // --- SI TODAS LAS VALIDACIONES PASAN, PROCEDEMOS A ACTUALIZAR ---

            try
            {
                // Cargar la lista completa de usuarios
                string pathUsuarios = archivosTxt.direccione.obtenerRutasTxt("usuario.txt");
                List<Usuario> todosLosUsuarios = ManejadorArchivosUsuario.LeerUsuarios(pathUsuarios);

                // Encontrar nuestro usuario en esa lista
                var usuarioParaActualizar = todosLosUsuarios.FirstOrDefault(u => u.NumeroTarjeta == _usuario.NumeroTarjeta);
                if (usuarioParaActualizar != null)
                {
                    // Actualizar el PIN
                    usuarioParaActualizar.PIN = pinNuevo;

                    // Guardar la lista completa de vuelta al archivo
                    ManejadorArchivosUsuario.EscribirUsuarios(pathUsuarios, todosLosUsuarios);

                    // Registrar el cambio en el log
                    string pathLogPin = archivosTxt.direccione.obtenerRutasTxt("log_cambio_pin.txt");
                    ManejadorLogs.RegistrarCambioPin(pathLogPin, _usuario.NumeroTarjeta);

                    MessageBox.Show("Su PIN ha sido cambiado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
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
