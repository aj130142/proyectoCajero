using System;

namespace proyectoCajero
{
    public enum TipoOperacion
    {
        Deposito,
        Retiro
    }

    public enum TipoTransaccion
    {
        Retiro,
        Deposito
    }

    public class Transaccion
    {
        public DateTime FechaHora { get; set; }
        public string NumeroTarjeta { get; set; } = string.Empty;
        public TipoTransaccion Tipo { get; set; }
        public decimal Monto { get; set; }
    }
}