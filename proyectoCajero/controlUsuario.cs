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
            var ultimoUsuario = listaUsuarios.FirstOrDefault(u => u.Id == ultimaTransaccion.UsuarioId);
            string nombreUltimoUsuario = ultimoUsuario?.Nombre ?? "Desconocido";

            // Usuarios que han hecho cambio de PIN
            string pathLogPin = direccione.obtenerRutasTxt("log_cambio_pin.txt");
            List<string> tarjetasConCambioPin = ManejadorLogs.LeerLogCambioPin(pathLogPin);

            string usuariosCambioPin = "Ninguno.";
            if (tarjetasConCambioPin.Any())
            {
                // Buscamos los nombres de los usuarios basándonos en los números de tarjeta
                // Nota: Este método no es perfecto si una tarjeta fue reasignada, pero es lo mejor
                // que podemos hacer con la estructura de datos actual del log.
                var nombres = listaUsuarios
                    .Where(u => tarjetasConCambioPin.Contains(u.NumeroTarjeta))
                    .Select(u => u.Nombre)
                    .ToList();

                usuariosCambioPin = string.Join(", ", nombres);
            }


            // Mostramos la información
            lblControlDiario.Text =
                $"--- Estadísticas del {diaAConsultar:d} ---\n\n" +
                $"Total Retirado: {totalRetiradoHoy:C}\n" +
                $"Promedio Depositado: {promedioDepositado:C}\n" +
                $"Usuarios con cambio de PIN: {usuariosCambioPin}\n" +
                $"Último Acceso: {nombreUltimoUsuario} a las {ultimaTransaccion.FechaHora:T}";
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string busqueda = txtBuscarUsuario.Text;
            if (string.IsNullOrWhiteSpace(busqueda))
            {
                lblInfoUsuario.Text = "Por favor, ingrese un Nombre o ID para buscar.";
                return;
            }

            Usuario usuarioEncontrado = null;

            // --- LÓGICA DE BÚSQUEDA FLEXIBLE ---
            // Intentamos convertir la entrada a un número (ID)
            if (int.TryParse(busqueda, out int idBuscado))
            {
                // Si es un número, buscamos por ID
                usuarioEncontrado = listaUsuarios.FirstOrDefault(u => u.Id == idBuscado);
            }
            else
            {
                // Si no es un número, asumimos que es un nombre y buscamos por nombre
                usuarioEncontrado = listaUsuarios.FirstOrDefault(u => u.Nombre.Equals(busqueda, StringComparison.OrdinalIgnoreCase));
            }

            if (usuarioEncontrado == null)
            {
                lblInfoUsuario.Text = "Usuario no encontrado.";
                return;
            }

            // --- LÓGICA DE CÁLCULO CORREGIDA ---
            DateTime diaAConsultar = DateTime.Today;

            // Transacciones del usuario para el día de hoy
            // CAMBIO CLAVE: Buscamos por Id, no por NumeroTarjeta
            var transaccionesUsuarioHoy = listaTransacciones
                .Where(t => t.UsuarioId == usuarioEncontrado.Id && t.FechaHora.Date == diaAConsultar);

            decimal retiradoHoy = transaccionesUsuarioHoy
                .Where(t => t.Tipo == TipoTransaccion.Retiro)
                .Sum(t => t.Monto);

            // CAMBIO CLAVE: Buscamos por Id, no por NumeroTarjeta
            var ultimaTransaccionUsuario = listaTransacciones
                .Where(t => t.UsuarioId == usuarioEncontrado.Id)
                .OrderByDescending(t => t.FechaHora)
                .FirstOrDefault();

            string ultimoAccesoStr = ultimaTransaccionUsuario == null
                ? "Sin accesos registrados."
                : ultimaTransaccionUsuario.FechaHora.ToString(); // "F" es el formato de fecha y hora completa

            // Mostramos la información
            lblInfoUsuario.Text =
                $"Información de: {usuarioEncontrado.Nombre} (ID: {usuarioEncontrado.Id})\n\n" +
                $"Saldo Actual: {usuarioEncontrado.SaldoActual:C}\n" +
                $"Monto Máximo de Retiro Diario: {usuarioEncontrado.MontoMaximoDiario:C}\n" +
                $"Cantidad Retirada Hoy ({diaAConsultar:d}): {retiradoHoy:C}\n" +
                $"Último Acceso: {ultimoAccesoStr}";
        }
    }
}
