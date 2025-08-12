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
    public partial class insertarUsuario : Form
    {
        public insertarUsuario()
        {
            InitializeComponent();
        }

        private void okeyBtn_Click(object sender, EventArgs e)
        {
            
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

            noTarjeta = 0;

            

        }

        private void okeyBtn_MouseClick(object sender, MouseEventArgs e)
        {
           

        }
    }
}
