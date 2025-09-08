using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using static proyectoCajero.archivosTxt;

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



        private void ActualizarTotal()
        {
            // Obtenemos el valor de cada NumericUpDown
            decimal total = (num200.Value * 200) + (num100.Value * 100) +
                            (num50.Value * 50) + (num20.Value * 20) +
                            (num10.Value * 10) + (num5.Value * 5) +
                            (num1.Value * 1);

            // Actualizamos el texto del Label y le damos formato de moneda
            lblTotal.Text = $"Total: {total:C}"; // "C" es para formato de moneda (Q)

            // Opcional: Cambia el color del texto si se excede del límite
            if (total > 10000)
            {
                lblTotal.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                lblTotal.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void num200_ValueChanged(object sender, EventArgs e)
        {
            ActualizarTotal();
        }

        private void num100_ValueChanged(object sender, EventArgs e)
        {
            ActualizarTotal();

        }

        private void num50_ValueChanged(object sender, EventArgs e)
        {
            ActualizarTotal();
        }

        private void num20_ValueChanged(object sender, EventArgs e)
        {
            ActualizarTotal();
        }

        private void num10_ValueChanged(object sender, EventArgs e)
        {
            ActualizarTotal();
        }

        private void num5_ValueChanged(object sender, EventArgs e)
        {
            ActualizarTotal();
        }

        private void num1_ValueChanged(object sender, EventArgs e)
        {
            ActualizarTotal();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // Creamos una nueva instancia del objeto Cajero.
            var cajero = new Cajero();

            // Llenamos el diccionario de billetes con los valores de los NumericUpDown.
            // Usamos (int) para convertir el valor decimal del control a un entero.
            cajero.Billetes.Add(200, (int)num200.Value);
            cajero.Billetes.Add(100, (int)num100.Value);
            cajero.Billetes.Add(50, (int)num50.Value);
            cajero.Billetes.Add(20, (int)num20.Value);
            cajero.Billetes.Add(10, (int)num10.Value);
            cajero.Billetes.Add(5, (int)num5.Value);
            cajero.Billetes.Add(1, (int)num1.Value);

            // Validamos que el total no exceda el límite de inicialización.
            if (cajero.TotalEfectivo > 10000)
            {
                MessageBox.Show("El monto total para inicializar el cajero no puede exceder Q. 10,000.00", "Límite Excedido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Detenemos la operación.
            }

            try
            {
                // Guardamos el estado del cajero en un archivo JSON.
                string nombreArchivo = "cajero.json";
                // Usamos la misma utilidad de direcciones que ya tienes.
                string pathFinal = direccione.obtenerRutasTxt(nombreArchivo);

                // Opciones para que el JSON se vea bonito (indentado).
                var opcionesJson = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(cajero, opcionesJson);

                File.WriteAllText(pathFinal, jsonString);

                MessageBox.Show($"Cajero inicializado exitosamente con {cajero.TotalEfectivo:C}", "Operación Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close(); // Cierra la ventana después de guardar.
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al guardar el estado del cajero: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
