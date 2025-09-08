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
using static proyectoCajero.archivosTxt;





namespace proyectoCajero
{
    public partial class insertarUsuario : Form
    {
        
       

        public insertarUsuario()
        {
            InitializeComponent();
            
            
        }

        private void okeyBtn_Click(object sender, EventArgs e)
        {
            string nombreArchivo = "usuario.txt";
            string pathFinal = direccione.obtenerRutasTxt(nombreArchivo);

            // --- PASO 1: Leer la lista de usuarios usando el nuevo método ---
            // Ahora obtenemos una lista de objetos, mucho más fácil de manejar.
            List<Usuario> listaUsuarios = ManejadorArchivosUsuario.LeerUsuarios(pathFinal);

            // --- PASO 2: Validar la entrada ---
            // Verificamos si ya existe un usuario con el mismo nombre o número de tarjeta.
            // Usamos LINQ para hacer la búsqueda más legible.
            bool nombreRepetido = listaUsuarios.Any(u => u.Nombre == nameTB.Text);
            bool tarjetaRepetida = listaUsuarios.Any(u => u.NumeroTarjeta == tarjetaTB.Text);

            if (nombreRepetido)
            {
                MessageBox.Show("El nombre de usuario ya existe. Por favor, elija otro.");
                return; // Detenemos la ejecución del método aquí.
            }

            if (tarjetaRepetida)
            {
                MessageBox.Show("El número de tarjeta ya está registrado. Por favor, verifíquelo.");
                return; // Detenemos la ejecución.
            }

            // Validaciones adicionales (¡muy importante!)
            if (tarjetaTB.Text.Length != 16 || !long.TryParse(tarjetaTB.Text, out _))
            {
                MessageBox.Show("El número de tarjeta debe ser de 16 dígitos numéricos.");
                return;
            }

            if (pinTB.Text.Length != 4 || !int.TryParse(pinTB.Text, out _))
            {
                MessageBox.Show("El PIN debe ser de 4 dígitos numéricos.");
                return;
            }

            if (!decimal.TryParse(saldoTB.Text, out decimal saldo) || !decimal.TryParse(maxsaldoTB.Text, out decimal maxSaldo))
            {
                MessageBox.Show("El saldo y el monto máximo deben ser valores numéricos válidos.");
                return;
            }


            // --- PASO 3: Crear el nuevo objeto Usuario ---
            // Si todas las validaciones pasan, creamos el nuevo usuario.
            int nuevoId = 0;
            if (listaUsuarios.Any()) // Verificamos si hay usuarios para obtener el último Id
            {
                nuevoId = listaUsuarios.Max(u => u.Id) + 1; // Calculamos el siguiente ID disponible
            }

            Usuario nuevoUsuario = new Usuario
            {
                Id = nuevoId,
                Nombre = nameTB.Text,
                NumeroTarjeta = tarjetaTB.Text,
                PIN = pinTB.Text,
                SaldoActual = saldo,
                MontoMaximoDiario = maxSaldo
            };

            // --- PASO 4: Añadir el nuevo usuario a la lista y guardar ---
            listaUsuarios.Add(nuevoUsuario);
            ManejadorArchivosUsuario.EscribirUsuarios(pathFinal, listaUsuarios);

            MessageBox.Show("¡Usuario creado exitosamente!");

            // --- PASO 5: Limpiar los campos ---
            nameTB.Text = "";
            tarjetaTB.Text = "";
            pinTB.Text = "";
            saldoTB.Text = "";
            maxsaldoTB.Text = "";
        }

        private void okeyBtn_MouseClick(object sender, MouseEventArgs e)
        {


        }

        private void insertarUsuario_Load(object sender, EventArgs e)
        {
            
        }
    }
}
