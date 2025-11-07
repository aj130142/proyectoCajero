using System;
using System.Windows.Forms;

namespace proyectoCajero
{
    public partial class TokenGeneratorForm : Form
    {
        public string TokenGenerado { get; private set; } = string.Empty;

        public TokenGeneratorForm()
        {
            InitializeComponent();
            GenerarNuevoToken();
        }

        private void GenerarNuevoToken()
        {
            // Generar token aleatorio de 5 dígitos
            Random random = new Random();
            TokenGenerado = random.Next(10000, 99999).ToString();
            lblToken.Text = TokenGenerado;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            TokenGenerado = string.Empty;
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnRegenerarToken_Click(object sender, EventArgs e)
        {
            GenerarNuevoToken();
        }
    }
}
