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
            // 2. Calcular el total retirado por el usuario en el día de hoy
            string pathTransacciones = direccione.obtenerRutasTxt("transacciones.txt");
            List<Transaccion> todasLasTransacciones = ManejadorArchivosTransaccion.LeerTransacciones(pathTransacciones);

            decimal retiradoHoy = todasLasTransacciones
                .Where(t =>
                    t.UsuarioId == _usuarioActual.Id && // Transacciones de este usuario
                    t.Tipo == TipoTransaccion.Retiro &&                 // que sean retiros
                    t.FechaHora.Date == DateTime.Today)                // y que sean de hoy
                .Sum(t => t.Monto);                                    // Sumar los montos

            // Guardamos el cálculo en la propiedad de nuestro objeto usuario
            _usuarioActual.MontoRetiradoHoy = retiradoHoy;
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
                $"Saldo total en su cuenta: {saldoTotal:C}\n\n" +
                $"--- Límites de Retiro para Hoy ---\n" +
                $"Límite diario: {limiteDiario:C}\n" +
                $"Retirado hoy: {retiradoHoy:C}\n" +
                $"Disponible para retirar hoy: {disponibleParaRetirarHoy:C}";

            // Mostramos la información en un MessageBox
            MessageBox.Show(mensaje, "Consulta de Saldo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnUltimasTransacciones_Click(object sender, EventArgs e)
        {
            // 1. Leer todas las transacciones del archivo
            string pathTransacciones = direccione.obtenerRutasTxt("transacciones.txt");
            List<Transaccion> todasLasTransacciones = ManejadorArchivosTransaccion.LeerTransacciones(pathTransacciones);

            // 2. Filtrar, Ordenar y Seleccionar las transacciones del usuario
            var ultimasTransacciones = todasLasTransacciones
                .Where(t => t.UsuarioId == _usuarioActual.Id) // Filtrar por el usuario actual t.NumeroTarjeta == _usuarioActual.NumeroTarjeta
                .OrderByDescending(t => t.FechaHora)                         // Ordenar de la más reciente a la más antigua
                .Take(5)                                                     // Tomar solo las primeras 5
                .ToList();                                                   // Convertir el resultado a una lista

            // 3. Construir el mensaje para mostrar
            string mensaje;
            if (ultimasTransacciones.Any()) // Verificamos si se encontró alguna transacción
            {
                // Usamos StringBuilder para construir eficientemente el string
                var sb = new System.Text.StringBuilder();
                sb.AppendLine("Sus últimas 5 transacciones:");
                sb.AppendLine("------------------------------------");

                foreach (var transaccion in ultimasTransacciones)
                {
                    // Damos formato a cada línea para que sea clara
                    sb.AppendLine($"{transaccion.FechaHora:g} - {transaccion.Tipo} - {transaccion.Monto:C}");
                }

                mensaje = sb.ToString();
            }
            else
            {
                mensaje = "No se encontraron transacciones en su historial.";
            }

            // 4. Mostrar el mensaje
            MessageBox.Show(mensaje, "Últimas Transacciones", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
