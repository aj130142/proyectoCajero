using static proyectoCajero.archivosTxt;

namespace proyectoCajero
{
    public partial class Form1 : Form
    {
        List<string> listaLog = new List<string>();
        public Form1()
        {
            InitializeComponent();
            administarToolStripMenuItem.Visible = false;

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void insertarUsuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            insertarUsuario ventanaInser = new insertarUsuario();

            ventanaInser.Show();

        }

        private void buscarUsuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            controlUsuario ventanaControl = new controlUsuario();
            ventanaControl.Show();
        }

        private void modificarUsuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            modificarUsuarios modificarUser = new modificarUsuarios();

            modificarUser.Show();
        }

        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void sesionBtn_Click(object sender, EventArgs e)
        {
            string userLog = admName.Text;
            string contraLog = contAdm.Text;
            string nombreTxt = "adminUserTxt.txt";
            string rutatxt = direccione.obtenerRutasTxt(nombreTxt);
            listaLog = leerTxt.obtenerDatosTxt(rutatxt);
            int nombreEncontrado = listaLog.FindIndex(x => x == userLog);
            string contraSearch = listaLog[nombreEncontrado + 1];

            if (nombreEncontrado >= 0 && contraSearch == contraLog)
            {
                administarToolStripMenuItem.Visible = true;
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
            agregaAd.ShowDialog();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
