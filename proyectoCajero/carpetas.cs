using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyectoCajero
{
    internal class carpetas
    {

        public interface IVercrearArchivo
        {
            public static void verificarCarpeta()
            {
                string directorioEjecucion = AppDomain.CurrentDomain.BaseDirectory;
                string subdirec = "archivos txt/";
                string pathFinal = Path.Combine(directorioEjecucion, subdirec);
                // Verificar si la carpeta existe
                if (!Directory.Exists(pathFinal))
                {
                    // Crear la carpeta si no existe
                    Directory.CreateDirectory(pathFinal);
                    MessageBox.Show("carpeta creada");
                }
                else
                {
                    
                }
            }
        }

       
        
    }
}
