using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Cliente
{
    internal class comunicar
    {
        public class rutasJSOn
        {
            public string ruta()
            {
                string directorioEjecucion = AppDomain.CurrentDomain.BaseDirectory;

                string palabra = "proyectoCajero";
                string parteAntes="";
                int indice = directorioEjecucion.IndexOf(palabra);

                if (indice != -1)
                {
                    parteAntes = directorioEjecucion.Substring(0, indice) + palabra; // Parte anterior
                    string parteDespues = directorioEjecucion.Substring(indice + palabra.Length); // Parte posterior

                    Console.WriteLine("Antes: " + parteAntes); // "Este es un "
                    Console.WriteLine("Después: " + parteDespues); // " de texto"
                }

                return parteAntes;
            }
        }

        public class rutaArchivoTxt
        {
            public string rutaTxt(string archivo)
            {
                string directorioEjecucion = AppDomain.CurrentDomain.BaseDirectory;

                string palabra = @"proyectoCajero\bin\Debug\net8.0-windows\archivos txt";
                string parteAntes = "";
                int indice = directorioEjecucion.IndexOf(palabra);

                if (indice != -1)
                {
                    parteAntes = directorioEjecucion.Substring(0, indice) + palabra+@"\"+archivo; // Parte anterior
                    

                }

                return parteAntes;
                
            }
        }
        

    }
}
