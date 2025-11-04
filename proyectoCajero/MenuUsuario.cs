using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static proyectoCajero.archivosTxt;
using Microsoft.Data.SqlClient;

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

        private async void MenuUsuario_Load(object sender, EventArgs e)
        {
            lblBienvenida.Text = $"Hola, {_usuarioActual.Nombre}";

            try
            {
                var conexion = new ConexionBd();

                // Obtener datos actualizados del usuario (saldo y límite) desde la BD
                var usuarioBd = await conexion.GetUsuarioByNumeroTarjetaAsync(_usuarioActual.NumeroTarjeta);
                if (usuarioBd != null)
                {
                    _usuarioActual.SaldoActual = usuarioBd.SaldoActual;
                    _usuarioActual.MontoMaximoDiario = usuarioBd.MontoMaximoDiario;
                }

                // Calcular el total retirado por el usuario en el día de hoy consultando la tabla Transaccion
                const string sqlSum = @"SELECT ISNULL(SUM(tr.Monto), 0)
FROM Transaccion tr
INNER JOIN Tarjeta t ON tr.TarjetaID = t.TarjetaID
INNER JOIN TipoTransaccion tt ON tr.TipoTransaccionID = tt.TipoTransaccionID
WHERE t.NumeroTarjeta = @num AND tt.Nombre = 'Retiro' AND CAST(tr.FechaHora AS DATE) = CAST(GETDATE() AS DATE)";

                var parametros = new List<SqlParameter>
                {
                    new SqlParameter("@num", SqlDbType.NVarChar) { Value = _usuarioActual.NumeroTarjeta }
                };

                decimal retiradoHoy = await conexion.ExecuteScalarAsync<decimal>(sqlSum, parametros);

                // Guardamos el cálculo en la propiedad de nuestro objeto usuario
                _usuarioActual.MontoRetiradoHoy = retiradoHoy;
            }
            catch (Exception ex)
            {
                // En caso de error con la BD, conservamos comportamiento anterior (leer desde archivos) como fallback
                try
                {
                    string pathTransacciones = direccione.obtenerRutasTxt("transacciones.txt");
                    List<Transaccion> todasLasTransacciones = ManejadorArchivosTransaccion.LeerTransacciones(pathTransacciones);

                    decimal retiradoHoy = todasLasTransacciones
                        .Where(t =>
                            t.NumeroTarjeta == _usuarioActual.NumeroTarjeta &&
                            t.Tipo == TipoTransaccion.Retiro &&
                            t.FechaHora.Date == DateTime.Today)
                        .Sum(t => t.Monto);

                    _usuarioActual.MontoRetiradoHoy = retiradoHoy;
                }
                catch
                {
                    // ignorar si el fallback también falla
                    _usuarioActual.MontoRetiradoHoy = 0m;
                }

                // Mostrar error opcional al usuario (silencioso si prefieres)
                // MessageBox.Show("No fue posible consultar la base de datos: " + ex.Message, "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close(); // Simplemente cierra este formulario
        }

        private void btnVerSaldo_Click(object sender, EventArgs e)
        {
            // El saldo total está directamente en el objeto usuario
            decimal saldoTotal = _usuarioActual.SaldoActual;

            // El límite diario de retiro también está en el objeto
            decimal limiteDiario = _usuarioActual.MontoMaximoDiario;

            // El monto retirado hoy lo calculamos al cargar el formulario
            decimal retiradoHoy = _usuarioActual.MontoRetiradoHoy;

            // Calculamos el disponible para el resto del día
            decimal disponibleParaRetirarHoy = limiteDiario - retiradoHoy;

            // Nos aseguramos de no mostrar un número negativo si por alguna razón se excedió el límite
            if (disponibleParaRetirarHoy < 0)
            {
                disponibleParaRetirarHoy = 0;
            }

            // Construimos el mensaje para mostrar al usuario
            string mensaje =
                $"Saldo total en su cuenta: Q{saldoTotal:N2}\n\n" +
                $"--- Límites de Retiro para Hoy ---\n" +
                $"Límite diario: Q{limiteDiario:N2}\n" +
                $"Retirado hoy: Q{retiradoHoy:N2}\n" +
                $"Disponible para retirar hoy: Q{disponibleParaRetirarHoy:N2}";

            // Mostramos la información en un MessageBox
            MessageBox.Show(mensaje, "Consulta de Saldo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnUltimasTransacciones_Click(object sender, EventArgs e)
        {
            // Abrir formulario de transacciones pasando la tarjeta de la sesión
            var f = new TransaccionesForm(_usuarioActual.NumeroTarjeta);
            f.ShowDialog();
        }

        private void btnCambiarPin_Click(object sender, EventArgs e)
        {
            // Creamos una instancia del formulario y le pasamos el usuario actual
            CambiarPinForm cambiarPinForm = new CambiarPinForm(_usuarioActual);
            cambiarPinForm.ShowDialog();
        }

        private void btnDeposito_Click(object sender, EventArgs e)
        {
            // Creamos una instancia del formulario en modo Depósito
            OperacionForm formDeposito = new OperacionForm(_usuarioActual, TipoOperacion.Deposito);
            var resultado = formDeposito.ShowDialog();

            // IMPORTANTE: Después de un depósito, el objeto _usuarioActual que tenemos en memoria
            // está desactualizado. Debemos refrescarlo con la nueva información.
            if (resultado == DialogResult.OK)
            {
                _usuarioActual = formDeposito.UsuarioActualizado;
                // Opcional: Recalcular retiros del día si fuera necesario
                MenuUsuario_Load(sender, e);
            }
        }

        private void btnRetiro_Click(object sender, EventArgs e)
        {
            // Creamos una instancia del formulario en modo Retiro
            // y le pasamos el monto que el usuario ya ha retirado hoy.
            OperacionForm formRetiro = new OperacionForm(_usuarioActual, TipoOperacion.Retiro, _usuarioActual.MontoRetiradoHoy);
            var resultado = formRetiro.ShowDialog();

            // Al igual que con el depósito, actualizamos el estado del usuario si la operación fue exitosa.
            if (resultado == DialogResult.OK)
            {
                _usuarioActual = formRetiro.UsuarioActualizado;
                // Volvemos a ejecutar la lógica de Load para recalcular el monto retirado hoy
                MenuUsuario_Load(sender, e);
            }
        }
    }
}
