using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public int UsuarioId { get; set; }
        public DateTime FechaHora { get; set; }
        public string NumeroTarjeta { get; set; }
        public TipoTransaccion Tipo { get; set; }
        public decimal Monto { get; set; }
    }
}
