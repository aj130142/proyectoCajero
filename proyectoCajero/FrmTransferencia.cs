using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace proyectoCajero
{
    public partial class FrmTransferencia : Form
    {
        private bool bancosCargados;
        private readonly int idCajero;
        private readonly string tarjetaOrigen; // usuario logueado
        public FrmTransferencia(int idCajero, string tarjetaOrigen)
        {
            InitializeComponent();
            this.idCajero = idCajero;
            this.tarjetaOrigen = tarjetaOrigen?.Trim() ?? "";

            var u = SqlDb.BuscarUsuario(this.tarjetaOrigen);
            if (u == null)
            {
                MessageBox.Show("La tarjeta de origen no pertenece a un usuario. Operación cancelada.");
                Close();
                return;
            }
        }

        private void FrmTransferencia_Load(object sender, EventArgs e)
        {
            mtxtTarjetaExt.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
            lblNomDest.Text = "-";

            try
            {
                var bancosOtros = MySqlCentral.ListarBancosExcluyendo(AppState.IdBancoPropio);
                cboBancoExt.DisplayMember = "Nombre";
                cboBancoExt.ValueMember = "Id";
                cboBancoExt.DataSource = bancosOtros;
                cboBancoExt.SelectedIndex = bancosOtros.Count > 0 ? 0 : -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudieron cargar bancos: " + ex.Message);
                cboBancoExt.DataSource = null;
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            mtxtTarjetaExt.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
            var tarjeta = (mtxtTarjetaExt.Text ?? "").Trim();
            var bancoId = Convert.ToString(cboBancoExt.SelectedValue);

            if (tarjeta.Length != 16) { MessageBox.Show("Tarjeta inválida."); return; }

            try
            {
                var nombre = MySqlCentral.GetNombreUsuarioGlobal(tarjeta, bancoId);
                lblNombreExt.Text = string.IsNullOrEmpty(nombre) ? "(no encontrado)" : nombre;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Búsqueda falló: " + ex.Message);
            }
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            mtxtTarjetaExt.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;

            var tarjetaDestino = (mtxtTarjetaExt.Text ?? "").Trim();
            var bancoId = Convert.ToString(cboBancoExt.SelectedValue);
            var concepto = (txtConceptoExt.Text ?? "").Trim();

            decimal monto;
            if (!decimal.TryParse(txtMontoExt.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out monto) || monto <= 0)
            { MessageBox.Show("Monto inválido."); return; }

            //if (!SeguridadAcciones.SolicitarTokenAccion(tarjetaOrigen, $"Transf. externo a {tarjetaDestino} por Q{monto}")) return;

            try
            {
                var uOrigen = SqlDb.BuscarUsuario(tarjetaOrigen);
                if (uOrigen == null || uOrigen.SaldoActual < monto) { MessageBox.Show("Saldo insuficiente."); return; }

                using (var cn = new SqlConnection(SqlDb.CS))
                {
                    cn.Open();
                    using (var tx = cn.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        // Actualizar saldo de la cuenta origen
                        using (var cmd = new SqlCommand(
                            @"UPDATE c
                              SET c.SaldoActual = c.SaldoActual - @monto
                              FROM Cuenta c
                              INNER JOIN Tarjeta t ON t.CuentaID = c.CuentaID
                              WHERE t.NumeroTarjeta = @tarjeta", cn, tx))
                        {
                            cmd.Parameters.AddWithValue("@monto", monto);
                            cmd.Parameters.AddWithValue("@tarjeta", tarjetaOrigen);
                            cmd.ExecuteNonQuery();
                        }

                        decimal saldoPost;
                        using (var c2 = new SqlCommand(
                            @"SELECT c.SaldoActual
                              FROM Cuenta c
                              INNER JOIN Tarjeta t ON t.CuentaID = c.CuentaID
                              WHERE t.NumeroTarjeta = @tarjeta", cn, tx))
                        {
                            c2.Parameters.AddWithValue("@tarjeta", tarjetaOrigen);
                            saldoPost = Convert.ToDecimal(c2.ExecuteScalar());
                        }

                        var detalle = System.Text.Json.JsonSerializer.Serialize(new
                        {
                            a = "out-ext",
                            banco = bancoId,
                            para = tarjetaDestino,
                            concepto
                        });

                        SqlDb.InsertTransaccion(cn, tx, idCajero, tarjetaOrigen, "TransferOutExt", monto, saldoPost, detalle, null);
                        tx.Commit();
                    }
                }

                // NUEVO: guarda en la nube con banco_destino + descripcion
                MySqlCentral.InsertarTransferenciaGlobal(tarjetaOrigen, bancoId, tarjetaDestino, monto, concepto);

                MessageBox.Show("Transferencia externa registrada.");
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al transferir: " + ex.Message);
            }
        }

        private void CargarBancos()
        {
            try
            {
                var bancos = MySqlCentral.ListarBancos();
                cboBancoExt.DisplayMember = "Nombre";
                cboBancoExt.ValueMember = "Id";
                cboBancoExt.DataSource = bancos;
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudieron cargar bancos: " + ex.Message);
            }
        }
    }
}
