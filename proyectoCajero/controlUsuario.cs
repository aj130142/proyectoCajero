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
using static proyectoCajero.conexion;

namespace proyectoCajero
{
    public partial class controlUsuario : Form
    {
        private List<Usuario> listaUsuarios;
        private List<Transaccion> listaTransacciones;
        public controlUsuario()
        {
            InitializeComponent();



        }

        public void controlUsuario_Load(object sender, EventArgs e)
        {
            // Cargar usuarios
            string pathUsuarios = direccione.obtenerRutasTxt("usuario.txt");
            listaUsuarios = ManejadorArchivosUsuario.LeerUsuarios(pathUsuarios);

            // Cargar transacciones
            string pathTransacciones = direccione.obtenerRutasTxt("transacciones.txt");
            listaTransacciones = ManejadorArchivosTransaccion.LeerTransacciones(pathTransacciones);

            // Si no hay transacciones, mostramos un mensaje y salimos
            if (!listaTransacciones.Any())
            {
                lblControlDiario.Text = "No hay transacciones registradas para mostrar estadísticas.";
                return;
            }

            // Calcular y mostrar las estadísticas generales
            CalcularEstadisticasGenerales();
        }
        private void CalcularEstadisticasGenerales()
        {
            // Consideramos "hoy" como el día de la transacción más reciente.
            DateTime diaAConsultar = listaTransacciones.Max(t => t.FechaHora).Date;

            // Total retirado por todos los usuarios hoy
            decimal totalRetiradoHoy = listaTransacciones
                .Where(t => t.Tipo == TipoTransaccion.Retiro && t.FechaHora.Date == diaAConsultar)
                .Sum(t => t.Monto);

            // Promedio del monto depositado por todos los usuarios hoy
            var depositosHoy = listaTransacciones.Where(t => t.Tipo == TipoTransaccion.Deposito && t.FechaHora.Date == diaAConsultar);
            decimal promedioDepositado = depositosHoy.Any() ? depositosHoy.Average(t => t.Monto) : 0;

            // Último usuario que accedió al cajero
            var ultimaTransaccion = listaTransacciones.OrderByDescending(t => t.FechaHora).First();
            var ultimoUsuario = listaUsuarios.FirstOrDefault(u => u.NumeroTarjeta == ultimaTransaccion.NumeroTarjeta);
            string nombreUltimoUsuario = ultimoUsuario?.Nombre ?? "Desconocido";

            // Usuarios que han hecho cambio de PIN (funcionalidad futura)
            string usuariosCambioPin = "Funcionalidad no implementada.";

            // Mostramos la información
            lblControlDiario.Text =
                $"--- Estadísticas del {diaAConsultar:d} ---\n\n" +
                $"Total Retirado: {totalRetiradoHoy:C}\n" +
                $"Promedio Depositado: {promedioDepositado:C}\n" +
                $"Último Acceso: {nombreUltimoUsuario} a las {ultimaTransaccion.FechaHora:T}";
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string nombreBusqueda = txtBuscarUsuario.Text;
            if (string.IsNullOrWhiteSpace(nombreBusqueda))
            {
                lblInfoUsuario.Text = "Por favor, ingrese un nombre para buscar.";
                return;
            }

            var usuario = listaUsuarios.FirstOrDefault(u => u.Nombre.Equals(nombreBusqueda, StringComparison.OrdinalIgnoreCase));

            if (usuario == null)
            {
                lblInfoUsuario.Text = "Usuario no encontrado.";
                return;
            }

            DateTime diaAConsultar = DateTime.Today; // O usar la fecha de la última transacción si se prefiere

            // Transacciones del usuario para el día de hoy
            var transaccionesUsuarioHoy = listaTransacciones
                .Where(t => t.NumeroTarjeta == usuario.NumeroTarjeta && t.FechaHora.Date == diaAConsultar);

            decimal retiradoHoy = transaccionesUsuarioHoy
                .Where(t => t.Tipo == TipoTransaccion.Retiro)
                .Sum(t => t.Monto);

            var ultimaTransaccionUsuario = listaTransacciones
                .Where(t => t.NumeroTarjeta == usuario.NumeroTarjeta)
                .OrderByDescending(t => t.FechaHora)
                .FirstOrDefault();

            string ultimoAccesoStr = ultimaTransaccionUsuario == null ? "Nunca" : ultimaTransaccionUsuario.FechaHora.ToString();

            // Mostramos la información
            lblInfoUsuario.Text =
                $"Información de: {usuario.Nombre}\n\n" +
                $"Saldo Actual: {usuario.SaldoActual:C}\n" +
                $"Monto Máximo de Retiro Diario: {usuario.MontoMaximoDiario:C}\n" +
                $"Cantidad Retirada Hoy ({diaAConsultar:d}): {retiradoHoy:C}\n" +
                $"Último Acceso Registrado: {ultimoAccesoStr}"; 
        }
    }
}
