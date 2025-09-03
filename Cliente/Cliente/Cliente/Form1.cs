using System.Text.Json;
using static Cliente.Servidor;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace Cliente
{
    public partial class Form1 : Form
    {
        private System.Windows.Forms.Timer timer;
        public string directorioEjecucion;
        public int contador = 0;
        List<string> listaLog = new List<string>();
        public Form1()
        {
            InitializeComponent();


        }
        public class DataModel
        {
            public string Nombre { get; set; }

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000; // 1 segundo en milisegundos
            timer.Tick += Timer_Tick;
            timer.Start();

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            var rutaJson = new comunicar.rutasJSOn();
            string ruta = rutaJson.ruta();
            string rutaf = ruta + @"\" + "archivo.json";



            string json = File.ReadAllText(rutaf);
            DataModel data = JsonSerializer.Deserialize<DataModel>(json);
            if (data.Nombre == "Desactivado" && contador == 0)
            {
                this.Enabled = false;

            }
            if (data.Nombre == "Ejemplo")
            {
                this.Enabled = true;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var rutaJson = new comunicar.rutaArchivoTxt();
            string userLog = notarjetaTxt.Text; //obtiene el usuario
            string contraLog = noPinTxt.Text;//obtiene la contrasena
            string nombreTxt = "adminUserTxt.txt";// declaramos la carpeta 
            string rutatxt = (nombreTxt);// metodo para obtener la ruta de adminUserTxt.txt
            //listaLog = leerTxt.obtenerDatosTxt(rutatxt); //obtenemos la lista con todos los admins  
            int nombreEncontrado = listaLog.FindIndex(x => x == userLog);//devuelve la primera coincidencia
            string contraSearch = listaLog[nombreEncontrado + 1];//busca en la lista uno mas del index admin nombreEncontrado
            /*
            if (nombreEncontrado >= 0 && contraSearch == contraLog)//verifica si existe, la contrasena es igual
            {
                //muestra el resto del menu
                administarToolStripMenuItem.Visible = true;
                //al validar los campos la interfaz se limpia con todo lo que tenga que ver con inicio de sesion
                admName.Visible = false;
                contAdm.Visible = false;
                label1.Visible = false;
                label2.Visible = false;
                sesionBtn.Visible = false;
                newAdmBtn.Visible = false;
            }
            */

        }
    }
}
