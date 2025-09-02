using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace proyectoCajero
{
    internal class conexion
    {
     


        public class ServidorTCP
        {
            private TcpListener listener;
            private bool enEjecucion;

            // Diccionario: ID del cliente -> NetworkStream
            private ConcurrentDictionary<string, NetworkStream> clientes =
                new ConcurrentDictionary<string, NetworkStream>();

            // Evento cuando llega un mensaje de algún cliente
            public event Action<string, string> OnMensajeRecibido;
            // parámetros: (idCliente, mensaje)

            public ServidorTCP(string ip, int puerto)
            {
                listener = new TcpListener(IPAddress.Parse(ip), puerto);
            }

            public void Iniciar()
            {
                enEjecucion = true;
                listener.Start();
                Console.WriteLine("Servidor iniciado...");
                _ = AceptarClientes();
            }

            private async Task AceptarClientes()
            {
                while (enEjecucion)
                {
                    var tcpCliente = await listener.AcceptTcpClientAsync();
                    _ = Task.Run(() => ManejarCliente(tcpCliente));
                }
            }

            private async Task ManejarCliente(TcpClient tcpCliente)
            {
                var stream = tcpCliente.GetStream();
                byte[] buffer = new byte[1024];

                // Primer mensaje debe ser el ID del cliente
                int bytesLeidos = await stream.ReadAsync(buffer, 0, buffer.Length);
                string idCliente = Encoding.UTF8.GetString(buffer, 0, bytesLeidos);

                if (!clientes.TryAdd(idCliente, stream))
                {
                    MessageBox.Show($"⚠ El cliente {idCliente} ya está conectado.");
                    tcpCliente.Close();
                    return;
                }

                MessageBox.Show($"Cliente conectado: {idCliente}");

                try
                {
                    while (enEjecucion)
                    {
                        bytesLeidos = await stream.ReadAsync(buffer, 0, buffer.Length);
                        if (bytesLeidos == 0) break; // Cliente se desconectó

                        string mensaje = Encoding.UTF8.GetString(buffer, 0, bytesLeidos);

                        OnMensajeRecibido?.Invoke(idCliente, mensaje);
                    }
                }
                catch { }
                finally
                {
                    clientes.TryRemove(idCliente, out _);
                    stream.Close();
                    tcpCliente.Close();
                    Console.WriteLine($"Cliente desconectado: {idCliente}");
                }
            }

            // Método para enviar mensaje a un cliente específico
            public async Task Enviar(string idCliente, string mensaje)
            {
                if (clientes.TryGetValue(idCliente, out var stream))
                {
                    byte[] datos = Encoding.UTF8.GetBytes(mensaje);
                    await stream.WriteAsync(datos, 0, datos.Length);
                }
            }

            // Método para enviar a todos los clientes
            public async Task EnviarATodos(string mensaje)
            {
                byte[] datos = Encoding.UTF8.GetBytes(mensaje);

                foreach (var kvp in clientes)
                {
                    try
                    {
                        await kvp.Value.WriteAsync(datos, 0, datos.Length);
                    }
                    catch { }
                }
            }

            public void Detener()
            {
                enEjecucion = false;
                listener.Stop();
                Console.WriteLine("Servidor detenido.");
            }
        }
    }
}
