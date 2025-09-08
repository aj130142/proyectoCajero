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
    public enum CajeroFormMode
    {
        Inicializar,
        Agregar
    }
    public partial class cajeroInicializar : Form
    {
        private readonly CajeroFormMode _mode;
        private Cajero _cajeroActual;
        public int contador = 0;
        public cajeroInicializar(CajeroFormMode mode)
        {
            InitializeComponent();    
            _mode = mode; // Guardamos el modo
            SetupForm();  // Llamamos a un método para configurar la apariencia
        }
        public class DataModel
        {
            public string Nombre { get; set; }
            public string Pin { get; set; }
        }



        private void ActualizarTotal()
        {
            decimal totalNuevo = (num200.Value * 200) + (num100.Value * 100) +
                        (num50.Value * 50) + (num20.Value * 20) +
                        (num10.Value * 10) + (num5.Value * 5) +
                        (num1.Value * 1);

            decimal totalFinal;
            decimal limite;

            if (_mode == CajeroFormMode.Agregar && _cajeroActual != null)
            {
                totalFinal = _cajeroActual.TotalEfectivo + totalNuevo;
                limite = 30000;
                lblTotal.Text = $"Total Actual: {_cajeroActual.TotalEfectivo:C}\n" +
                              $"Monto a Agregar: {totalNuevo:C}\n" +
                              $"Nuevo Total: {totalFinal:C}";
            }
            else // Modo Inicializar
            {
                totalFinal = totalNuevo;
                limite = 10000;
                lblTotal.Text = $"Total: {totalFinal:C}";
            }

            lblTotal.ForeColor = totalFinal > limite ? System.Drawing.Color.Red : System.Drawing.Color.Black;
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
            Cajero cajeroParaGuardar;
            decimal limite;
            string mensajeErrorLimite;

            if (_mode == CajeroFormMode.Inicializar)
            {
                cajeroParaGuardar = new Cajero();
                cajeroParaGuardar.Billetes.Add(200, (int)num200.Value);
                cajeroParaGuardar.Billetes.Add(100, (int)num100.Value);
                cajeroParaGuardar.Billetes.Add(50, (int)num50.Value);
                cajeroParaGuardar.Billetes.Add(20, (int)num20.Value);
                cajeroParaGuardar.Billetes.Add(10, (int)num10.Value);
                cajeroParaGuardar.Billetes.Add(5, (int)num5.Value);
                cajeroParaGuardar.Billetes.Add(1, (int)num1.Value);
                limite = 10000;
                mensajeErrorLimite = "El monto total para inicializar no puede exceder Q. 10,000.00";
            }
            else // Modo Agregar
            {
                if (_cajeroActual == null) return; // No debería pasar si el form cargó bien
                cajeroParaGuardar = _cajeroActual; // Empezamos con el estado actual

                // Sumamos los nuevos billetes a los existentes
                cajeroParaGuardar.Billetes[200] += (int)num200.Value;
                cajeroParaGuardar.Billetes[100] += (int)num100.Value;
                cajeroParaGuardar.Billetes[50] += (int)num50.Value;
                cajeroParaGuardar.Billetes[20] += (int)num20.Value;
                cajeroParaGuardar.Billetes[10] += (int)num10.Value;
                cajeroParaGuardar.Billetes[5] += (int)num5.Value;
                cajeroParaGuardar.Billetes[1] += (int)num1.Value;
                limite = 30000;
                mensajeErrorLimite = "El monto total en el cajero no puede exceder Q. 30,000.00";
            }

            if (cajeroParaGuardar.TotalEfectivo > limite)
            {
                MessageBox.Show(mensajeErrorLimite, "Límite Excedido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Importante: Si hay error, revertimos los cambios en modo Agregar para no corromper el estado
                if (_mode == CajeroFormMode.Agregar) CargarEstadoActualCajero();
                return;
            }

            try
            {
                string nombreArchivo = "cajero.json";
                string pathFinal = direccione.obtenerRutasTxt(nombreArchivo);
                var opcionesJson = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(cajeroParaGuardar, opcionesJson);
                File.WriteAllText(pathFinal, jsonString);

                MessageBox.Show($"Operación exitosa. Nuevo total en cajero: {cajeroParaGuardar.TotalEfectivo:C}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al guardar el estado del cajero: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupForm()
        {
            if (_mode == CajeroFormMode.Inicializar)
            {
                this.Text = "Inicializar Cajero";
                btnGuardar.Text = "Inicializar";
            }
            else // Modo Agregar
            {
                this.Text = "Agregar Efectivo";
                btnGuardar.Text = "Agregar";
                CargarEstadoActualCajero();
            }
        }

        private void CargarEstadoActualCajero()
        {
            string nombreArchivo = "cajero.json";
            string pathFinal = direccione.obtenerRutasTxt(nombreArchivo);

            if (!File.Exists(pathFinal))
            {
                MessageBox.Show("El cajero aún no ha sido inicializado. Debe inicializarlo antes de poder agregar efectivo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Cerramos el formulario si no hay nada que agregar.
                // Usamos BeginInvoke para cerrar de forma segura después de que el formulario termine de cargarse.
                this.BeginInvoke(new Action(() => this.Close()));
                return;
            }

            try
            {
                string jsonString = File.ReadAllText(pathFinal);
                _cajeroActual = JsonSerializer.Deserialize<Cajero>(jsonString);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al leer el estado actual del cajero: " + ex.Message);
                this.BeginInvoke(new Action(() => this.Close()));
            }
        }
    }
}
