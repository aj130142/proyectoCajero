using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace proyectoCajero
{
    public partial class TransaccionesForm : Form
    {
        private readonly string _numeroTarjetaSesion;

        public TransaccionesForm(string numeroTarjetaSesion)
        {
            InitializeComponent();
            _numeroTarjetaSesion = numeroTarjetaSesion;
        }

        private async void TransaccionesForm_Load(object sender, EventArgs e)
        {
            // Inicializar controles
            dtpDesde.Value = DateTime.Today.AddDays(-7);
            dtpHasta.Value = DateTime.Today;
            txtTarjeta.Text = _numeroTarjetaSesion;

            cmbTipo.Items.Clear();
            cmbTipo.Items.Add("Todos");
            cmbTipo.Items.Add("Retiro");
            cmbTipo.Items.Add("Deposito");
            cmbTipo.SelectedIndex = 0;

            await LoadTop10Async();
        }

        private async Task LoadTop10Async()
        {
            try
            {
                var conexion = new ConexionBd();
                var lista = await conexion.QueryRecentTransactionsAsync(txtTarjeta.Text.Trim(), 10);
                dgvTransacciones.DataSource = lista.Select(t => new {
                    Fecha = t.FechaHora,
                    Tipo = t.Tipo,
                    Numero = t.NumeroTarjetaEnmascarado(),
                    Monto = $"Q{t.Monto:N2}"
                }).ToList();

                lblStatus.Text = lista.Count > 0 ? "" : "No se encontraron transacciones.";
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error al cargar transacciones: " + ex.Message;
            }
        }

        private async void btnTop10_Click(object sender, EventArgs e)
        {
            await LoadTop10Async();
        }

        private async void btnFiltrar_Click(object sender, EventArgs e)
        {
            DateTime desde = dtpDesde.Value.Date;
            DateTime hasta = dtpHasta.Value.Date;
            if (desde > hasta)
            {
                MessageBox.Show("El rango de fechas no es válido. Asegúrese que Desde <= Hasta.", "Rango inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var conexion = new ConexionBd();
                string tipo = cmbTipo.SelectedItem?.ToString() ?? "Todos";
                var lista = await conexion.QueryTransactionsAsync(txtTarjeta.Text.Trim(), desde, hasta, tipo);
                dgvTransacciones.DataSource = lista.Select(t => new {
                    Fecha = t.FechaHora,
                    Tipo = t.Tipo,
                    Numero = t.NumeroTarjetaEnmascarado(),
                    Monto = $"Q{t.Monto:N2}"
                }).ToList();

                lblStatus.Text = lista.Count > 0 ? "" : "No se encontraron transacciones en ese rango.";
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error al filtrar transacciones: " + ex.Message;
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
