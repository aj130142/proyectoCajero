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
    public partial class modificarUsuarios : Form
    {
        public static List<string> listaUsa = new List<string>();//lista global para guardar datos de los usuarios
        public modificarUsuarios()
        {
            InitializeComponent();

            tarjetaNewTB.ReadOnly = true;
            maxSaldoTB.ReadOnly = true;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

            maxSaldoTB.ReadOnly = false;

        }

        private void modTarjCheckB_CheckedChanged(object sender, EventArgs e)
        {
            tarjetaNewTB.ReadOnly = false;
        }

        private void modificarUsuarios_Load(object sender, EventArgs e)
        {
            string cargarUsuario = "usuario.txt"; //nombre del archivo de la carpeta a buscar
            string pathF = direccione.obtenerRutasTxt(cargarUsuario);//ingresamos el nombre del archivo y obtenemos la ruta completa del usuario.txt
            listaUsa = leerTxt.obtenerDatosTxt(pathF);//con la ruta obtenemos una lista global para guardar todos los datos de los usuarios
        }

        private void okeyBtn_Click(object sender, EventArgs e)
        {
            string nombreBuscar=nameTB.Text;


        }
    }
}
