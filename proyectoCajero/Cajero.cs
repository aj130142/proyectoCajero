using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyectoCajero
{
    public class Cajero
    {
        // Usamos un Diccionario para guardar la cantidad de billetes por cada denominación.
        // La clave (int) es la denominación (ej: 200, 100, 50).
        // El valor (int) es la cantidad de billetes de esa denominación.
        public Dictionary<int, int> Billetes { get; set; }

        public Cajero()
        {
            // Constructor: inicializa el diccionario para que no esté vacío.
            Billetes = new Dictionary<int, int>();
        }

        // Una propiedad "calculada" que nos da el total de dinero en el cajero.
        public decimal TotalEfectivo
        {
            get
            {
                // Multiplica cada denominación por su cantidad de billetes y suma todo.
                return Billetes.Sum(billete => (decimal)billete.Key * billete.Value);
            }
        }
    }
}
