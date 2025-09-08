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

        public class rutasJSOn
        {
            public string ruta()
            {
                string directorioEjecucion = AppDomain.CurrentDomain.BaseDirectory;

                string palabra = "proyectoCajero";
                string parteAntes = "";
                int indice = directorioEjecucion.IndexOf(palabra);

                if (indice != -1)
                {
                    parteAntes = directorioEjecucion.Substring(0, indice) + palabra; // Parte anterior
                    string parteDespues = directorioEjecucion.Substring(indice + palabra.Length); // Parte posterior

                    
                }

                return parteAntes;
            }
        }

    }

    // Esta clase se especializará en leer y escribir la lista de usuarios.
    public static class ManejadorArchivosUsuario
    {
        /// <summary>
        /// Lee el archivo de usuarios y lo convierte en una lista de objetos Usuario.
        /// </summary>
        /// <param name="path">La ruta completa del archivo usuario.txt</param>
        /// <returns>Una lista de objetos Usuario.</returns>
        public static List<Usuario> LeerUsuarios(string path)
        {
            var listaUsuarios = new List<Usuario>();

            if (!File.Exists(path))
            {
                // Si el archivo no existe, simplemente devolvemos una lista vacía.
                return listaUsuarios;
            }

            var lineas = File.ReadAllLines(path).ToList();

            // Cada usuario ocupa 6 líneas en el archivo.
            // Recorremos la lista de líneas de 6 en 6.
            for (int i = 0; i < lineas.Count; i += 6)
            {
                // Nos aseguramos de que haya suficientes líneas para un usuario completo.
                if (i + 5 < lineas.Count)
                {
                    var usuario = new Usuario();

                    // Intentamos convertir cada línea al tipo de dato correspondiente.
                    // Usar TryParse es más seguro que Parse, ya que no lanza un error si el formato es incorrecto.
                    int.TryParse(lineas[i], out int id);
                    usuario.Id = id;

                    usuario.Nombre = lineas[i + 1];
                    usuario.NumeroTarjeta = lineas[i + 2];
                    usuario.PIN = lineas[i + 3];

                    decimal.TryParse(lineas[i + 4], out decimal saldo);
                    usuario.SaldoActual = saldo;

                    decimal.TryParse(lineas[i + 5], out decimal maxDiario);
                    usuario.MontoMaximoDiario = maxDiario;

                    // Añadimos el objeto usuario completamente formado a nuestra lista.
                    listaUsuarios.Add(usuario);
                }
            }

            return listaUsuarios;
        }

        /// <summary>
        /// Escribe una lista de objetos Usuario en el archivo de texto, sobrescribiendo el contenido anterior.
        /// </summary>
        /// <param name="path">La ruta completa del archivo usuario.txt</param>
        /// <param name="listaUsuarios">La lista de usuarios a guardar.</param>
        public static void EscribirUsuarios(string path, List<Usuario> listaUsuarios)
        {
            var lineasParaEscribir = new List<string>();

            foreach (var usuario in listaUsuarios)
            {
                lineasParaEscribir.Add(usuario.Id.ToString());
                lineasParaEscribir.Add(usuario.Nombre);
                lineasParaEscribir.Add(usuario.NumeroTarjeta);
                lineasParaEscribir.Add(usuario.PIN);
                lineasParaEscribir.Add(usuario.SaldoActual.ToString()); // Convertimos el decimal a string
                lineasParaEscribir.Add(usuario.MontoMaximoDiario.ToString()); // Convertimos el decimal a string
            }

            // File.WriteAllLines se encarga de abrir el archivo, escribir todas las líneas
            // y cerrarlo de forma segura. Sobrescribe el archivo si ya existe.
            File.WriteAllLines(path, lineasParaEscribir);
        }
    }

    public static class ManejadorArchivosTransaccion
    {
        // Usaremos un formato simple separado por comas (CSV)
        // Ejemplo de línea: 2025-09-08T10:30:00,1111222233334444,Retiro,500.00
        private const string SEPARADOR = ",";

        public static List<Transaccion> LeerTransacciones(string path)
        {
            var listaTransacciones = new List<Transaccion>();
            if (!File.Exists(path))
            {
                return listaTransacciones;
            }

            var lineas = File.ReadAllLines(path);
            foreach (var linea in lineas)
            {
                var partes = linea.Split(SEPARADOR);
                if (partes.Length == 4)
                {
                    var transaccion = new Transaccion();
                    DateTime.TryParse(partes[0], out DateTime fecha);
                    transaccion.FechaHora = fecha;
                    transaccion.NumeroTarjeta = partes[1];
                    Enum.TryParse(partes[2], out TipoTransaccion tipo);
                    transaccion.Tipo = tipo;
                    decimal.TryParse(partes[3], out decimal monto);
                    transaccion.Monto = monto;
                    listaTransacciones.Add(transaccion);
                }
            }
            return listaTransacciones;
        }

        // Este método lo usaremos más adelante en el Área de Usuario para GUARDAR transacciones
        public static void AgregarTransaccion(string path, Transaccion transaccion)
        {
            string linea = string.Join(SEPARADOR,
                // Formato estándar ISO 8601 para fechas, fácil de leer por máquinas
                transaccion.FechaHora.ToString("o"),
                transaccion.NumeroTarjeta,
                transaccion.Tipo,
                transaccion.Monto
            );

            // Usamos File.AppendAllText para añadir la nueva transacción al final sin borrar las anteriores
            File.AppendAllText(path, linea + Environment.NewLine);
        }
    }
}
