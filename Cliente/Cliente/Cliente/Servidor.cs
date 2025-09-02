using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Cliente
{
    internal class Servidor
    {


        public class ClienteTCP
        {
            private TcpClient cliente;
            private NetworkStream stream;
            private bool conectado;
            private string idCliente;

            // Evento para cuando se recibe un mensaje del servidor
            public event Action<string> OnMensajeRecibido;

            public ClienteTCP(string id)
            {
                idCliente = id; // ID fijo del cliente
            }

            // Conectar al servidor
            public async Task Conectar(string ip, int puerto)
            {
                cliente = new TcpClient();
                await cliente.ConnectAsync(ip, puerto);
                stream = cliente.GetStream();
                conectado = true;

                // Mandar el ID al servidor inmediatamente
                await Enviar($"ID:{idCliente}");

                // Empieza a escuchar mensajes del servidor
                _ = Task.Run(Escuchar);
            }

            // Método para enviar mensajes al servidor
            public async Task Enviar(string mensaje)
            {
                if (conectado && stream != null)
                {
                    byte[] datos = Encoding.UTF8.GetBytes(mensaje);
                    await stream.WriteAsync(datos, 0, datos.Length);
                }
            }

            // Escuchar mensajes del servidor
            private async Task Escuchar()
            {
                byte[] buffer = new byte[1024];

                try
                {
                    while (conectado)
                    {
                        int bytesLeidos = await stream.ReadAsync(buffer, 0, buffer.Length);
                        if (bytesLeidos == 0) break; // conexión cerrada

                        string mensaje = Encoding.UTF8.GetString(buffer, 0, bytesLeidos);
                        OnMensajeRecibido?.Invoke(mensaje); // notifica a quien esté suscrito
                    }
                }
                catch
                {
                    // Ignorar errores de desconexión
                }
                finally
                {
                    conectado = false;
                    cliente.Close();
                }
            }

            // Cerrar conexión
            public void Desconectar()
            {
                conectado = false;
                stream?.Close();
                cliente?.Close();
            }
        }

        public class escuchando
        {
            
        }


    }
}
