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
    public partial class controlUsuario : Form
    {
        List<string> listaUsa = new List<string>();
        public controlUsuario()
        {
            InitializeComponent();
            
            

        }

        public void controlUsuario_Load(object sender, EventArgs e)
        {
           
            string cargarUsuario = "usuario.txt";
            string pathF = direccione.obtenerRutasTxt(cargarUsuario);
            listaUsa = leerTxt.obtenerDatosTxt(pathF);


        }
    }
}
