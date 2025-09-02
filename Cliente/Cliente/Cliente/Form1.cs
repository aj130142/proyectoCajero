using static Cliente.Servidor;

namespace Cliente
{
    public partial class Form1 : Form
    {
        private CancellationTokenSource cts;
        public Form1()
        {
            InitializeComponent();
            

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Main();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            
        }
        static async Task Main()
        {
            var cliente = new ClienteTCP("123");

            await cliente.Conectar("127.0.0.1", 5000);

            await cliente.Enviar("Hola servidor");

            Console.WriteLine("Mensaje enviado.");
        }
    }
}
