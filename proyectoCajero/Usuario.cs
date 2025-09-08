using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace proyectoCajero
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string NumeroTarjeta { get; set; }
        public string PIN { get; set; }
        public decimal SaldoActual { get; set; }
        public decimal MontoMaximoDiario { get; set; }

        // Podríamos añadir más propiedades en el futuro si es necesario,
        // como por ejemplo, el total retirado en el día.
        public decimal MontoRetiradoHoy { get; set; }
    }
}

/**Explicación:**
*Cada propiedad(`Id`, `Nombre`, etc.) corresponde directamente a los datos que guardas en `usuario.txt`.
*   Usamos `decimal` para el dinero (`SaldoActual`, `MontoMaximoDiario`). Este es el tipo de dato recomendado en C# para cálculos financieros porque evita los pequeños errores de redondeo que pueden ocurrir con `double` o `float`.
*   He añadido `MontoRetiradoHoy`. Nos será muy útil para verificar el límite diario de retiro.

---*/