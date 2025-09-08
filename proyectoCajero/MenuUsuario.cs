using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace proyectoCajero
{
    public partial class MenuUsuario : Form
    {
        private Usuario _usuarioActual;
        public MenuUsuario(Usuario usuario)
        {
            InitializeComponent();
            _usuarioActual = usuario;
        }

        private void MenuUsuario_Load(object sender, EventArgs e)
        {
            lblBienvenida.Text = $"Hola, {_usuarioActual.Nombre}";
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close(); // Simplemente cierra este formulario
        }
    }
}
