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

    public partial class agregarAdmin : Form
    {
        List<string> listaLog = new List<string>();
        public agregarAdmin()
        {
            InitializeComponent();
        }

        private void guardarBtn_Click(object sender, EventArgs e)
        {
            string newUser=nameTxt.Text;
            string newpass =passTxt.Text;
            listaLog.Add(newUser);
            listaLog.Add(newpass);
            string nombreTxt = "adminUserTxt.txt";
            string rutatxt = direccione.obtenerRutasTxt(nombreTxt);
            escrGenericoTxt.escriTxt(rutatxt, listaLog);


        }
    }
}
