using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.LinkLabel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace proyectoCajero
{
    internal class archivosTxt
    {
        public interface escribirArchi// clase para escribir archivos txt
        {
            public static void Main(string path, string nameUs, string noTarjeta, string noPin, string saldoUs, string maxsaldoUsu)
            {// este metodo es para guardar datos mediante uso de argumentos 
                var numero = new Random(); //creamos un random numero
                var value = numero.Next(0, 99999999);// crea un numero aleatorio
                string id = value.ToString();// casta de string a int
                try
                {
                    //declaramos la creacion para escribir archivos
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
                    MessageBox.Show($"Error al escribir en el archivo: {ex.Message}");
                }
            }
        }

        public interface escrGenericoTxt// escribir archivos forma generica
        {
            public static void escriTxt(string pathm, List<string> lista1)// metodo general para escribir archivos, la ruta y la lista
            {// la lista debe de estar en orden segun la necesidades

                try
                {
                    //se escribe en la ruta las lista al final del archivo
                    using (StreamWriter sw = File.AppendText(pathm))
                    {
                        bool fileExists = File.Exists(pathm) && new FileInfo(pathm).Length > 0;
                        foreach (string s in lista1)
                        {
                            if (!string.IsNullOrWhiteSpace(s)) // Evita escribir líneas vacías
                            {
                                if (fileExists)
                                {
                                    sw.WriteLine(s);
                                }
                                else
                                {
                                    sw.Write(s); // Primera línea sin salto de línea inicial
                                    fileExists = true;
                                }
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al escribir en el archivo: {ex.Message}");
                }

            }
        }


        public interface leerTxt // lee los archivos de todo tipo
        {


            public static List<string> obtenerDatosTxt(string path)// el argumento es la ruta del archivo
            {
                List<string> obtenerTxt = new List<string>();// lista para guardar informacion

                if (!File.Exists(path))// verificamos si existe o no el archivo
                {
                    MessageBox.Show("Error 505: no existe el archivo");
                    return obtenerTxt;
                }

                try
                {
                    using (StreamReader reader = new StreamReader(path)) // creamos la lectura del txt con la ruta
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
                obtenerTxt=obtenerTxt.FindAll(n => n != "");
                return obtenerTxt;//devolvemos la lista de todos los datos
            }


        }

        public interface direccione //sirve para obtener direcciones de la carpte archivos txt
        {
            public static string obtenerRutasTxt(string path1)// el argumento es el nombre del archivo
            {

                string directorioEjecucion = AppDomain.CurrentDomain.BaseDirectory;// ruta original de nuestro proyecto
                string subdirec = "archivos txt/" + path1;//unimos la carpeta "archivos txt" con el nombre del archivo txt
                string pathFinal = Path.Combine(directorioEjecucion, subdirec);//la ruta total de nuestro archivo txt en el sistema


                return pathFinal;// retorna un string para su direccion
            }


        }
        public interface verificar
        {
            public static Boolean verificarRepetido(string datoBuscar, List<string> ListaBuscar)
            {

                bool existeLuis = ListaBuscar.Exists(n => n == datoBuscar);
                MessageBox.Show(""+existeLuis);
                
                return existeLuis;
            }

        }
    }
}
