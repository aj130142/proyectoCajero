using System;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Json;
using static proyectoCajero.archivosTxt;

namespace proyectoCajero
{
    public partial class OperacionForm : Form
    {
        private readonly Usuario _usuario;
        private readonly TipoOperacion _tipoOperacion;
        private readonly decimal _montoRetiradoHoy;
        public Usuario UsuarioActualizado { get; private set; }

        public OperacionForm(Usuario usuario, TipoOperacion tipo, decimal montoRetiradoHoy = 0)
        {
            InitializeComponent();
            _usuario = usuario;
            _tipoOperacion = tipo;
            _montoRetiradoHoy = montoRetiradoHoy; // Guardamos el nuevo dato
            SetupForm();
        }

        private void SetupForm()
        {
            if (_tipoOperacion == TipoOperacion.Deposito)
            {
                this.Text = "Realizar Depósito";
                btnAceptar.Text = "Depositar";
            }
            else // Futuro modo Retiro
            {
                this.Text = "Realizar Retiro";
                btnAceptar.Text = "Retirar";
            }
        }
        private void ActualizarTotal()
        {
            decimal total = (num200.Value * 200) + (num100.Value * 100) +
                            (num50.Value * 50) + (num20.Value * 20) +
                            (num10.Value * 10) + (num5.Value * 5) +
                            (num1.Value * 1);
            lblTotalOperacion.Text = $"Total: {total:C}";
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

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            decimal montoTotal = (num200.Value * 200) + (num100.Value * 100) +
                       (num50.Value * 50) + (num20.Value * 20) +
                       (num10.Value * 10) + (num5.Value * 5) +
                       (num1.Value * 1);

            if (montoTotal <= 0)
            {
                MessageBox.Show("Debe especificar una cantidad de billetes.", "Monto Inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_tipoOperacion == TipoOperacion.Deposito)
            {
                // --- LÓGICA DE DEPÓSITO (la que ya tenías) ---
                // (La he movido aquí adentro para mayor claridad)
                try
                {
                    string pathCajero = direccione.obtenerRutasTxt("cajero.json");
                    var cajero = JsonSerializer.Deserialize<Cajero>(File.ReadAllText(pathCajero));
                    cajero.Billetes[200] += (int)num200.Value;
                    // ... (repite para todas las denominaciones)
                    cajero.Billetes[100] += (int)num100.Value;
                    cajero.Billetes[50] += (int)num50.Value;
                    cajero.Billetes[20] += (int)num20.Value;
                    cajero.Billetes[10] += (int)num10.Value;
                    cajero.Billetes[5] += (int)num5.Value;
                    cajero.Billetes[1] += (int)num1.Value;

                    string pathUsuarios = direccione.obtenerRutasTxt("usuario.txt");
                    var todosLosUsuarios = ManejadorArchivosUsuario.LeerUsuarios(pathUsuarios);
                    var usuarioParaActualizar = todosLosUsuarios.First(u => u.NumeroTarjeta == _usuario.NumeroTarjeta);
                    usuarioParaActualizar.SaldoActual += montoTotal;
                    this.UsuarioActualizado = usuarioParaActualizar;

                    var nuevaTransaccion = new Transaccion { UsuarioId = _usuario.Id, FechaHora = DateTime.Now, NumeroTarjeta = _usuario.NumeroTarjeta, Tipo = TipoTransaccion.Deposito, Monto = montoTotal };

                    File.WriteAllText(pathCajero, JsonSerializer.Serialize(cajero, new JsonSerializerOptions { WriteIndented = true }));
                    ManejadorArchivosUsuario.EscribirUsuarios(pathUsuarios, todosLosUsuarios);
                    ManejadorArchivosTransaccion.AgregarTransaccion(direccione.obtenerRutasTxt("transacciones.txt"), nuevaTransaccion);

                    MessageBox.Show($"Depósito de {montoTotal:C} realizado con éxito.\n\nNuevo saldo: {usuarioParaActualizar.SaldoActual:C}", "Depósito Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ocurrió un error crítico durante el depósito: " + ex.Message, "Error de Operación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else // --- LÓGICA DE RETIRO (la parte nueva) ---
            {
                try
                {
                    // --- VALIDACIONES PRIMERO ---
                    string pathCajero = direccione.obtenerRutasTxt("cajero.json");
                    var cajero = JsonSerializer.Deserialize<Cajero>(File.ReadAllText(pathCajero));

                    // 1. ¿Hay saldo suficiente en la cuenta?
                    if (montoTotal > _usuario.SaldoActual)
                    {
                        MessageBox.Show($"No tiene saldo suficiente para retirar {montoTotal:C}. Su saldo actual es {_usuario.SaldoActual:C}.", "Saldo Insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // 2. ¿Se excede el límite diario?
                    decimal disponibleHoy = _usuario.MontoMaximoDiario - _montoRetiradoHoy;
                    if (montoTotal > disponibleHoy)
                    {
                        MessageBox.Show($"Esta operación excede su límite de retiro diario. Solo puede retirar hasta {disponibleHoy:C} más por hoy.", "Límite Diario Excedido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // 3. ¿Hay suficientes billetes en el cajero?
                    if (cajero.Billetes[200] < num200.Value || cajero.Billetes[100] < num100.Value ||
                        cajero.Billetes[50] < num50.Value || cajero.Billetes[20] < num20.Value ||
                        cajero.Billetes[10] < num10.Value || cajero.Billetes[5] < num5.Value ||
                        cajero.Billetes[1] < num1.Value)
                    {
                        MessageBox.Show("El cajero no dispone de la cantidad de billetes solicitada para una o más denominaciones. Por favor, intente un desglose diferente.", "Billetes no Disponibles", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // --- SI TODAS LAS VALIDACIONES PASAN, EJECUTAMOS LA OPERACIÓN ---
                    cajero.Billetes[200] -= (int)num200.Value;
                    // ... (repite para todas las denominaciones)
                    cajero.Billetes[100] -= (int)num100.Value;
                    cajero.Billetes[50] -= (int)num50.Value;
                    cajero.Billetes[20] -= (int)num20.Value;
                    cajero.Billetes[10] -= (int)num10.Value;
                    cajero.Billetes[5] -= (int)num5.Value;
                    cajero.Billetes[1] -= (int)num1.Value;


                    string pathUsuarios = direccione.obtenerRutasTxt("usuario.txt");
                    var todosLosUsuarios = ManejadorArchivosUsuario.LeerUsuarios(pathUsuarios);
                    var usuarioParaActualizar = todosLosUsuarios.First(u => u.NumeroTarjeta == _usuario.NumeroTarjeta);
                    usuarioParaActualizar.SaldoActual -= montoTotal;
                    this.UsuarioActualizado = usuarioParaActualizar;

                    var nuevaTransaccion = new Transaccion { UsuarioId = _usuario.Id, FechaHora = DateTime.Now, NumeroTarjeta = _usuario.NumeroTarjeta, Tipo = TipoTransaccion.Retiro, Monto = montoTotal };

                    File.WriteAllText(pathCajero, JsonSerializer.Serialize(cajero, new JsonSerializerOptions { WriteIndented = true }));
                    ManejadorArchivosUsuario.EscribirUsuarios(pathUsuarios, todosLosUsuarios);
                    ManejadorArchivosTransaccion.AgregarTransaccion(direccione.obtenerRutasTxt("transacciones.txt"), nuevaTransaccion);

                    MessageBox.Show($"Retiro de {montoTotal:C} realizado con éxito.\n\nNuevo saldo: {usuarioParaActualizar.SaldoActual:C}", "Retiro Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ocurrió un error crítico durante el retiro: " + ex.Message, "Error de Operación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
