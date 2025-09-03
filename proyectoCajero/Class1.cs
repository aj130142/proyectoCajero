using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace proyectoCajero
{
    internal class Class1
    {
        public class Program
        {
            // Evento que se dispara cuando el archivo cambia

            public bool cambios { get; set; }
            public Program(){


             

             }
        public static event Action<bool> ArchivoCambiado;


        public  void Main1()
            {
                var rutaf= new archivosTxt.rutasJSOn();
                string ruta1=rutaf.ruta();

                ruta1 = ruta1 + @"\" + "archivo.json";

                // Iniciar watcher
                IniciarWatcher(ruta1);

                // Suscribirse al evento
                ArchivoCambiado += (cambio) =>
                {
                    MessageBox.Show("El JSON fue modificado -> " + cambio);
                    cambios=cambio;
                };

                Console.WriteLine("Escuchando cambios... Presiona ENTER para salir.");
                Console.ReadLine();
            }

            public static void IniciarWatcher(string ruta)
            {
                string carpeta = Path.GetDirectoryName(ruta);
                string archivo = Path.GetFileName(ruta);

                FileSystemWatcher watcher = new FileSystemWatcher(carpeta, archivo);
                watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size;

                watcher.Changed += (s, e) =>
                {
                    ArchivoCambiado?.Invoke(true); // Dispara el evento con true
                };

                watcher.EnableRaisingEvents = true;
            }
        }
    }
}
