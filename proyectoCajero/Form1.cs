using static proyectoCajero.archivosTxt;

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

        private void Form1_Load(object sender, EventArgs e)
        {

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
    }
}
