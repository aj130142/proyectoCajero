using System;

namespace proyectoCajero
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string NumeroTarjeta { get; set; } = string.Empty;
        public string PIN { get; set; } = string.Empty;
        public decimal SaldoActual { get; set; }
        public decimal MontoMaximoDiario { get; set; }

        // Podríamos añadir más propiedades en el futuro si es necesario,
        // como por ejemplo, el total retirado en el día.
        public decimal MontoRetiradoHoy { get; set; }
    }
}