using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace proyectoCajero
{
    public static class SqlDb
    {
        public static string CS => System.Configuration.ConfigurationManager.ConnectionStrings["CajeroDB"].ConnectionString;

        // Buscar usuario por número de tarjeta
        public static Usuario BuscarUsuario(string numeroTarjeta)
        {
            using (var cn = new SqlConnection(CS))
            using (var cmd = new SqlCommand(@"SELECT TOP 1 u.UsuarioID, u.Nombres, u.Apellidos, c.SaldoActual
FROM Tarjeta t
INNER JOIN Cuenta c ON t.CuentaID = c.CuentaID
INNER JOIN Usuario u ON c.UsuarioID = u.UsuarioID
WHERE t.NumeroTarjeta = @num", cn))
            {
                cmd.Parameters.AddWithValue("@num", numeroTarjeta);
                cn.Open();
                using (var r = cmd.ExecuteReader())
                {
                    if (r.Read())
                    {
                        return new Usuario
                        {
                            Id = r.GetInt32(0),
                            Nombre = r.GetString(1) + " " + r.GetString(2),
                            SaldoActual = r.GetDecimal(3),
                            NumeroTarjeta = numeroTarjeta
                        };
                    }
                }
            }
            return null;
        }

        // Insertar transacción (para transferencias externas, etc)
        public static void InsertTransaccion(SqlConnection cn, SqlTransaction tx, int idCajero, string numeroTarjeta, string tipo, decimal monto, decimal saldoPost, string detalle, int? cuentaDestinoId)
        {
            // Busca TarjetaID y CuentaID
            int tarjetaId = 0, cuentaId = 0;
            using (var cmd = new SqlCommand("SELECT TarjetaID, CuentaID FROM Tarjeta WHERE NumeroTarjeta = @num", cn, tx))
            {
                cmd.Parameters.AddWithValue("@num", numeroTarjeta);
                using (var r = cmd.ExecuteReader())
                {
                    if (r.Read())
                    {
                        tarjetaId = r.GetInt32(0);
                        cuentaId = r.GetInt32(1);
                    }
                }
            }
            // TipoTransaccionID: 4 para TransferOutExt (ajusta según tu catálogo)
            byte tipoTransId = tipo == "TransferOutExt" ? (byte)4 : (byte)0;
            using (var cmd = new SqlCommand(@"INSERT INTO Transaccion (CuentaID, TarjetaID, TipoTransaccionID, Monto, FechaHora)
VALUES (@cuenta, @tarjeta, @tipo, @monto, GETDATE())", cn, tx))
            {
                cmd.Parameters.AddWithValue("@cuenta", cuentaId);
                cmd.Parameters.AddWithValue("@tarjeta", tarjetaId);
                cmd.Parameters.AddWithValue("@tipo", tipoTransId);
                cmd.Parameters.AddWithValue("@monto", monto);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
