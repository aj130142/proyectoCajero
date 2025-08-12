namespace proyectoCajero
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void insertarUsuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            insertarUsuario ventanaInser = new insertarUsuario();

            ventanaInser.Show();

        }
    }
}
