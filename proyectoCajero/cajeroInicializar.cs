using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace proyectoCajero
{
    public partial class cajeroInicializar : Form
    {
        public int contador = 0;
        public cajeroInicializar()
        {
            InitializeComponent();

        }
        public class DataModel
        {
            public string Nombre { get; set; }
            public string Pin { get; set; }
        }

        public void button1_Click(object sender, EventArgs e)
        {
            var ruta = new archivosTxt.rutasJSOn();
            string ruta1 = ruta.ruta();
            string rutaf = ruta1 + @"\" + "archivo.json";

            var data = new DataModel { Nombre = "Desactivado" };
            string json = JsonSerializer.Serialize(data);
            File.WriteAllText(rutaf, json);
            MessageBox.Show("" + contador);





        }

        private void cajero1Off_Click(object sender, EventArgs e)
        {
            var ruta = new archivosTxt.rutasJSOn();
            string ruta1 = ruta.ruta();
            string rutaf = ruta1 + @"\" + "archivo.json";
            var data = new DataModel { Nombre = "Ejemplo" };
            string json = JsonSerializer.Serialize(data);
            File.WriteAllText(rutaf, json);
            MessageBox.Show("" + contador);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var ruta = new archivosTxt.rutasJSOn();
            string ruta1 = ruta.ruta();
            string rutaf = ruta1 + @"\" + "archivo.json";
            var data = new DataModel { Pin = "Ejemplo" };
            string json = JsonSerializer.Serialize(data);
            File.WriteAllText(rutaf, json);
            MessageBox.Show("" + contador);
        }
    }
}
