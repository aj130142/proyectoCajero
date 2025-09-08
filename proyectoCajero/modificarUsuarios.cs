using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static proyectoCajero.archivosTxt;


namespace proyectoCajero
{
    public partial class modificarUsuarios : Form
    {
        public static List<Usuario> listaUsu = new List<Usuario>();
        string pathF;
        public modificarUsuarios()
        {
            InitializeComponent();


            tarjetaNewTB.Enabled = false;
            maxSaldoTB.Enabled = false;
        }


        private void modTarjCheckB_CheckedChanged_1(object sender, EventArgs e)
        {
            if (modTarjCheckB.Checked)
            {
                tarjetaNewTB.Enabled = true;
            }
            if (modTarjCheckB.Checked == !true)
            {
                tarjetaNewTB.Enabled = false;
                modTarjCheckB.Checked = false;

            }

        }

        private void modMaxSaldoCheckB_CheckedChanged(object sender, EventArgs e)
        {
            if (modMaxSaldoCheckB.Checked)
            {
                maxSaldoTB.Enabled = true;
            }
            if (modMaxSaldoCheckB.Checked == !true)
            {
                maxSaldoTB.Enabled = false;
                modTarjCheckB.Checked = false;

            }

        }


        private void modificarUsuarios_Load(object sender, EventArgs e)
        {
            string cargarUsuario = "usuario.txt";
            pathF = direccione.obtenerRutasTxt(cargarUsuario);
            // Ahora cargamos directamente una lista de objetos Usuario
            listaUsu = ManejadorArchivosUsuario.LeerUsuarios(pathF);
        }

        private void okeyBtn_Click(object sender, EventArgs e)
        {
            string nombreBuscar = nameTB.Text;

            if (string.IsNullOrWhiteSpace(nombreBuscar))
            {
                MessageBox.Show("Por favor, ingrese el nombre del usuario a modificar.");
                return;
            }

            // --- PASO 1: Buscar el usuario en la lista ---
            // Usamos LINQ para encontrar el objeto Usuario completo que coincida con el nombre.
            Usuario usuarioAModificar = listaUsu.FirstOrDefault(u => u.Nombre.Equals(nombreBuscar, StringComparison.OrdinalIgnoreCase));

            if (usuarioAModificar == null)
            {
                MessageBox.Show("No se encontró ningún usuario con ese nombre.");
                return;
            }

            // --- PASO 2: Realizar las modificaciones ---
            // Verificamos qué checkboxes están activos y actualizamos las propiedades del objeto.
            bool cambiosRealizados = false;

            if (modTarjCheckB.Checked)
            {
                string nuevaTarjeta = tarjetaNewTB.Text;
                if (nuevaTarjeta.Length != 16 || !long.TryParse(nuevaTarjeta, out _))
                {
                    MessageBox.Show("El nuevo número de tarjeta debe ser de 16 dígitos numéricos.");
                    return;
                }
                // Verificamos que la nueva tarjeta no pertenezca ya a otro usuario.
                if (listaUsu.Any(u => u.NumeroTarjeta == nuevaTarjeta && u.Nombre != nombreBuscar))
                {
                    MessageBox.Show("Ese número de tarjeta ya está asignado a otro usuario.");
                    return;
                }
                usuarioAModificar.NumeroTarjeta = nuevaTarjeta;
                cambiosRealizados = true;
            }

            if (modMaxSaldoCheckB.Checked)
            {
                if (!decimal.TryParse(maxSaldoTB.Text, out decimal nuevoMaxSaldo))
                {
                    MessageBox.Show("El nuevo límite de saldo debe ser un valor numérico válido.");
                    return;
                }
                usuarioAModificar.MontoMaximoDiario = nuevoMaxSaldo;
                cambiosRealizados = true;
            }

            // --- PASO 3: Guardar los cambios (solo si se hizo alguno) ---
            if (cambiosRealizados)
            {
                // El pathF fue definido cuando se cargó el formulario.
                ManejadorArchivosUsuario.EscribirUsuarios(pathF, listaUsu);
                MessageBox.Show("¡Usuario modificado exitosamente!");
            }
            else
            {
                MessageBox.Show("No se seleccionó ninguna opción para modificar.");
                return;
            }

            // --- PASO 4: Limpiar la interfaz ---
            nameTB.Clear();
            tarjetaNewTB.Clear();
            maxSaldoTB.Clear();

            // Desactivar checkboxes para la siguiente operación
            allSelect.Checked = false;
            modTarjCheckB.Checked = false;
            modMaxSaldoCheckB.Checked = false;
        }

        private void allSelect_CheckedChanged(object sender, EventArgs e)
        {
            if (allSelect.Checked) { 
                modTarjCheckB.Checked = true;
                modMaxSaldoCheckB.Checked=true;
            }
            if (!allSelect.Checked )
            {
                modTarjCheckB.Checked = false;
                modMaxSaldoCheckB.Checked = false;
            }
            
        }
    }
}
