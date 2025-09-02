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
        public static List<string> listaUsu = new List<string>();//lista global para guardar datos de los usuarios
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
            string cargarUsuario = "usuario.txt"; //nombre del archivo de la carpeta a buscar
            pathF = direccione.obtenerRutasTxt(cargarUsuario);//ingresamos el nombre del archivo y obtenemos la ruta completa del usuario.txt
            listaUsu = leerTxt.obtenerDatosTxt(pathF);//con la ruta obtenemos una lista global para guardar todos los datos de los usuarios
        }

        private void okeyBtn_Click(object sender, EventArgs e)
        {
            List<string> historialPin=new List<string>();
            string nombreBuscar = nameTB.Text;
            string VhistorialPIn = "historialPin";
            string maXSaldo = "";
            string newTarjeta = "";
            int indexUser = listaUsu.FindIndex(n => n == nombreBuscar);
            
            

            if (maxSaldoTB.Enabled && tarjetaNewTB.Enabled)
            {

                maXSaldo = maxSaldoTB.Text;
                listaUsu[indexUser + 4] = maXSaldo;
                newTarjeta = tarjetaNewTB.Text;
                listaUsu[indexUser + 1] = newTarjeta;
                
            }

            if (maxSaldoTB.Enabled)
            {
                maXSaldo = maxSaldoTB.Text;
                listaUsu[indexUser + 4] = maXSaldo;
            }

            if (tarjetaNewTB.Enabled)
            {
                newTarjeta = tarjetaNewTB.Text;
                listaUsu[indexUser + 1] = newTarjeta;
            }
            File.WriteAllLines(pathF, listaUsu);

            tarjetaNewTB.Clear();
            maxSaldoTB.Clear();

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
