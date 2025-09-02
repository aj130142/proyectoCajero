using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.IO;
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
                int lineas = 0;
                if (File.Exists(path))// metodo para poder saber cuantas lineas tiene
                {
                    lineas = File.ReadAllLines(path).Length;
                    lineas = lineas / 6;
                    
                }
                else
                {
                    Console.WriteLine("El archivo no existe.");
                    lineas = 0;
                }
                string id = lineas.ToString();// casta de string a int del id
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

                int lineas = 0;
                if (File.Exists(pathm))
                {
                    lineas = File.ReadAllLines(pathm).Length;
                    lineas = lineas / 3;

                }
                else
                {
                    Console.WriteLine("El archivo no existe.");
                    lineas = 0;
                }
                string id = lineas.ToString();
                lista1= lista1.Prepend(id).ToList();// agregamos el id al principio
                
                try
                {
                    //se escribe en la ruta las lista al final del archivo
                    try
                    {
                        //declaramos la creacion para escribir archivos
                        using (StreamWriter sw = new StreamWriter(pathm, append: true))
                        {
                           foreach(string s in lista1)
                            {
                                sw.WriteLine(s);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al escribir en el archivo: {ex.Message}");
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

                bool exiteArchivo = ListaBuscar.Exists(n => n == datoBuscar);
                MessageBox.Show(""+ exiteArchivo);
                
                return exiteArchivo;
            }

        }

        public interface registros
        {
            public static void historialPin(string path,string pinNew,string pinOld,List<string> listas)
            {
                string ruta=direccione.obtenerRutasTxt(path);
                escrGenericoTxt.escriTxt(ruta,listas);


            }
        }

    }
}
