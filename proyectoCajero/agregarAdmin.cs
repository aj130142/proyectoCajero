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
        List<string> listaLog = new List<string>();//lista que guarda los textos de los textbox
        public agregarAdmin()
        {
            InitializeComponent();
        }

        private void guardarBtn_Click(object sender, EventArgs e)
        {
            string newUser=nameTxt.Text;//obtiene el usuario
            string newpass =passTxt.Text;//obtine la contraseña
            string nombreTxt = "adminUserTxt.txt";//declaramos la carpeta 
            string rutatxt = direccione.obtenerRutasTxt(nombreTxt);// obtenemos la ruta completa del archivo en archivos txt
            // guarda en la lista
            List<String> lista1 = new List<String>();
            lista1 = leerTxt.obtenerDatosTxt(rutatxt);

            bool userRepetido =verificar.verificarRepetido(newUser, lista1);

            if (userRepetido!=true) {

                listaLog.Add(newUser);
                listaLog.Add(newpass);
                
                escrGenericoTxt.escriTxt(rutatxt, listaLog);//obtiene la ruta y la lista para escribirla

            }
            else
            {
                MessageBox.Show("Admin repetido");

            }
            


        }
    }
}
