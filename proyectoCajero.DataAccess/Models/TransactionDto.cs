using System;

namespace proyectoCajero.DataAccess.Models
{
    public class TransactionDto
    {
        public DateTime FechaHora { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string NumeroTarjeta { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public int? TarjetaId { get; set; }
        public int? CuentaId { get; set; }

        public string NumeroTarjetaEnmascarado()
        {
            if (string.IsNullOrEmpty(NumeroTarjeta)) return string.Empty;
            if (NumeroTarjeta.Length <= 4) return NumeroTarjeta;
            return new string('*', Math.Max(0, NumeroTarjeta.Length - 4)) + NumeroTarjeta.Substring(NumeroTarjeta.Length - 4);
        }
    }
}