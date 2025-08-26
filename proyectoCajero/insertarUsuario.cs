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
            string nombreArchivo = "usuario.txt"; // nombre del archivo a buscar en la caperta archivos txt
            string pathFinal = direccione.obtenerRutasTxt(nombreArchivo);//ingresamos el nombre del archivo y obtenemos la ruta completa del usuario.txt
            string auxnoTarjeta = tarjetaTB.Text;// obtiene el texto del textbox
            string auxPin = pinTB.Text;// obtiene el texto del textbox



            string nombreUsuario = nameTB.Text;// obtiene el texto del textbox
            long noTarjeta = long.Parse(auxnoTarjeta);// obtiene el texto del textbox y cast numerico(long por su capacidad)
            int noPin = Int16.Parse(auxPin);// obtiene el texto del textbox y cast numerico(int por su capacidad)
            string saldo = saldoTB.Text;// obtiene el texto del textbox
            string maxSaldo = maxsaldoTB.Text;// obtiene el texto del textbox

            List<string> lista = new List<string>();
            lista=leerTxt.obtenerDatosTxt(pathFinal);
            
            bool aceptar=verificar.verificarRepetido(nombreUsuario, lista);
            bool pinAceptar= verificar.verificarRepetido(auxnoTarjeta, lista);
            if (aceptar!=true && pinAceptar!=true) {
                //Aca se limpia la informacion de los textbox 
                nameTB.Text = "";
                tarjetaTB.Text = "";
                pinTB.Text = "";
                saldoTB.Text = "";
                maxsaldoTB.Text = "";
                //aca termina

                //metodo especial para guardar usuario, debe contar con todos los argumentos
                escribirArchi.Main(path: pathFinal,
                    nameUs: nombreUsuario,
                    noTarjeta: auxnoTarjeta,
                    noPin: auxPin, saldoUs: saldo,
                    maxsaldoUsu: maxSaldo);

            }
            else
            {
                MessageBox.Show("usuario repetido");
            }
            

            


        }

        private void okeyBtn_MouseClick(object sender, MouseEventArgs e)
        {


        }

        private void insertarUsuario_Load(object sender, EventArgs e)
        {
            
        }
    }
}
