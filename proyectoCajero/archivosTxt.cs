using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.LinkLabel;

namespace proyectoCajero
{
    internal class archivosTxt
    {
        public interface escribirArchi
        {
            public static void Main(string path,string nameUs,string noTarjeta,string noPin,string saldoUs,string maxsaldoUsu )
            {
                var numero = new Random();
                var value= numero.Next(0,99999999);
                string id=value.ToString();
                try
                {
                    // Usar StreamWriter en modo append (true)
                    using (StreamWriter sw = new StreamWriter(path, append: true))
                    {
                        sw.WriteLine(id);
                        sw.WriteLine(nameUs);
                        sw.WriteLine(noTarjeta);
                        sw.WriteLine(noPin);
                        sw.WriteLine(saldoUs);
                        sw.WriteLine(maxsaldoUsu);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al escribir en el archivo: {ex.Message}");
                }
            }
        }
       

        public class leerTxt
        {
            

            public List<string> obtenerDatosTxt(string path)
            {
                List<string> obtenerTxt = new List<string>();

                if (!File.Exists(path))
                {
                    Console.WriteLine("Archivo no encontrado");
                    return obtenerTxt;
                }

                try
                {
                    using (StreamReader reader = new StreamReader(path))
                    {
                        string linea;
                        while ((linea = reader.ReadLine()) != null)
                        {
                            obtenerTxt.Add(linea);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                return obtenerTxt;
            }


        }

    }
}
