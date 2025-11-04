using System;
using System.Collections.Generic;
using System.Linq;

namespace proyectoCajero
{
    public class Cajero
    {
        public int CajeroId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Ubicacion { get; set; } = string.Empty;
        public bool Activo { get; set; }

        // Nuevo: indica si el cajero fue inicializado con el monto inicial
        public bool Inicializado { get; set; }

        // Capacidad máxima del cajero (Q40000 por defecto)
        public decimal CapacidadMaxima { get; set; } = 40000m;

        // Monto inicial obligatorio al inicializar (Q10000 por defecto)
        public decimal MontoInicial { get; set; } = 10000m;

        // Usamos un Diccionario para guardar la cantidad de billetes por cada denominación.
        // La clave (int) es la denominación (ej: 200, 100, 50).
        // El valor (int) es la cantidad de billetes de esa denominación.
        public Dictionary<int, int> Billetes { get; set; }

        public Cajero()
        {
            // Constructor: inicializa el diccionario para que no esté vacío
            // y asegura que existan las denominaciones que usamos en la UI.
            Billetes = new Dictionary<int, int>
            {
                { 200, 0 },
                { 100, 0 },
                { 50, 0 },
                { 20, 0 },
                { 10, 0 },
                { 5, 0 },
                { 1, 0 }
            };
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