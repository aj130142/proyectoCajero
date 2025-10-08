using System.Text.Json;
using static proyectoCajero.archivosTxt;
using static proyectoCajero.carpetas.IVercrearArchivo;
using static proyectoCajero.conexion;
using static proyectoCajero.cajeroInicializar;

namespace proyectoCajero
{
    public partial class Form1 : Form
    {
        List<string> listaLog = new List<string>(); //lista que guarda los admin
        public Form1()
        {
            InitializeComponent();
            administarToolStripMenuItem.Visible = true;//oculta el resto del menu hasta que inicies sesion, cambialo para tener acceso

        }
        public class DataModel
        {
            public string Nombre { get; set; }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            verificarCarpeta();


            var ruta = new archivosTxt.rutasJSOn();
            string ruta1 = ruta.ruta();
            string rutaf = ruta1 + @"\" + "archivo.json";
            var data = new DataModel { Nombre = "Ejemplo" };
            string json = JsonSerializer.Serialize(data);
            File.WriteAllText(rutaf, json);

        }

        private void insertarUsuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            insertarUsuario ventanaInser = new insertarUsuario();

            ventanaInser.Show(); // abrimos la ventana insertarUsuario

        }

        private void buscarUsuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            controlUsuario ventanaControl = new controlUsuario();
            ventanaControl.Show(); // abrimos la ventana controlUsuario
        }

        private void modificarUsuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            modificarUsuarios modificarUser = new modificarUsuarios();

            modificarUser.Show();// abrimos modificarUsuarios
        }

        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void sesionBtn_Click(object sender, EventArgs e)
        {
            string userLog = admName.Text; //obtiene el usuario
            string contraLog = contAdm.Text;//obtiene la contrasena
            string nombreTxt = "adminUserTxt.txt";// declaramos la carpeta 
            string rutatxt = direccione.obtenerRutasTxt(nombreTxt);// metodo para obtener la ruta de adminUserTxt.txt
            listaLog = leerTxt.obtenerDatosTxt(rutatxt); //obtenemos la lista con todos los admins  
            int nombreEncontrado = listaLog.FindIndex(x => x == userLog);//devuelve la primera coincidencia
            string contraSearch = listaLog[nombreEncontrado + 1];//busca en la lista uno mas del index admin nombreEncontrado

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

        }

        private void newAdmBtn_Click(object sender, EventArgs e)
        {
            agregarAdmin agregaAd = new agregarAdmin();
            agregaAd.ShowDialog(); //llama a la ventana agregarAdmin
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void activarCajerosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cajeroInicializar inicia = new cajeroInicializar(CajeroFormMode.Inicializar);
            inicia.ShowDialog(); // Usar ShowDialog es mejor para ventanas que deben completarse
        }

        private void agregarEfectivoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Abrimos el mismo formulario, pero en modo Agregar
            cajeroInicializar agrega = new cajeroInicializar(CajeroFormMode.Agregar);
            agrega.ShowDialog();
        }

        private void btnModoUsuario_Click(object sender, EventArgs e)
        {
            // Creamos una instancia del nuevo formulario de login
            LoginUsuario ventanaLoginUsuario = new LoginUsuario();

            // Ocultamos la ventana principal de administración
            this.Hide();

            // Mostramos el formulario de login. Usamos ShowDialog para que la ejecución
            // se detenga aquí hasta que el usuario cierre la ventana de login.
            ventanaLoginUsuario.ShowDialog();

            // Una vez que la ventana de login se cierra (ya sea por éxito o cancelación),
            // volvemos a mostrar la ventana principal de administración.
            this.Show();
        }
    }
}
