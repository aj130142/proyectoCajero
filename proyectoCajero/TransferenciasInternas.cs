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
    public partial class TransferenciasInternas : Form
    {
        private readonly Usuario _usuario;
        public TransferenciasInternas(Usuario usuario)
        {
            InitializeComponent();
            _usuario = usuario;
        }

        private void TransferenciasInternas_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            AgregarcuentaInterna agregarcuentaInterna = new AgregarcuentaInterna();
            agregarcuentaInterna.ShowDialog();
        }
    }
}
