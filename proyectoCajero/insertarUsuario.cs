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
            string auxnoTarjeta = tarjetaTB.Text;
            string auxPin = pinTB.Text;

            

            string nombreUsuario = nameTB.Text;
            long noTarjeta = long.Parse(auxnoTarjeta);
            int noPin = Int16.Parse(auxPin);
            string saldo = saldoTB.Text;
            string maxSaldo = maxsaldoTB.Text;



            nameTB.Text = "";
            tarjetaTB.Text = "";
            pinTB.Text = "";
            saldoTB.Text = "";
            maxsaldoTB.Text = "";
            escribirArchi.Main(path:pathFinal,
                nameUs:nombreUsuario,
                noTarjeta:auxnoTarjeta,
                noPin:auxPin,saldoUs:saldo,
                maxsaldoUsu:maxSaldo);




        }

        private void okeyBtn_MouseClick(object sender, MouseEventArgs e)
        {


        }

        private void insertarUsuario_Load(object sender, EventArgs e)
        {
            
        }
    }
}
