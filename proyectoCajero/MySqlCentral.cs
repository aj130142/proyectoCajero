using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyectoCajero
{
    public static class MySqlCentral
    {
        private const string CS =
            "Server=sql10.freesqldatabase.com;Port=3306;Database=sql10806388;" +
            "Uid=sql10806388;Pwd=Wuv4HLENgQ;SslMode=Preferred;Connection Timeout=10;" +
            "AllowPublicKeyRetrieval=True;Charset=utf8mb4;";

        public sealed class BancoItem
        {
            public string Id { get; set; }      // CHAR(8)
            public string Nombre { get; set; }
            public override string ToString() { return Nombre; }
        }

        // ---------- Conexión ----------
        public static bool ProbarConexion(out string error)
        {
            try
            {
                using (var cn = new MySqlConnection(CS))
                {
                    cn.Open();
                    using (var cmd = new MySqlCommand("SELECT 1", cn)) cmd.ExecuteScalar();
                }
                error = ""; return true;
            }
            catch (Exception ex) { error = ex.Message; return false; }
        }

        // ---------- Bancos ----------
        public static List<BancoItem> ListarBancos() { return ListarBancosExcluyendo(null); }

        public static List<BancoItem> ListarBancosExcluyendo(string excluirId)
        {
            var list = new List<BancoItem>();
            using (var cn = new MySqlConnection(CS))
            {
                cn.Open();
                string sql = "SELECT id,nombre FROM Bancos "
                           + (string.IsNullOrEmpty(excluirId) ? "" : "WHERE id<>@ex ")
                           + "ORDER BY nombre";
                using (var cmd = new MySqlCommand(sql, cn))
                {
                    if (!string.IsNullOrEmpty(excluirId)) cmd.Parameters.AddWithValue("@ex", excluirId);
                    using (var rd = cmd.ExecuteReader())
                        while (rd.Read())
                            list.Add(new BancoItem { Id = rd.GetString(0), Nombre = rd.GetString(1) });
                }
            }
            return list;
        }

        // ---------- UsuariosGlobal ----------
        // Modo seguro: no permite cambiar banco_id de una tarjeta existente
        // return: 1 insertado, 2 actualizado nombre, -2 conflicto (otro banco), -1 error
        public static int InsertarUsuarioGlobalSeguro(string tarjeta, string nombre, string bancoId, out string error)
        {
            try
            {
                using (var cn = new MySqlConnection(CS))
                {
                    cn.Open();

                    // ¿Existe la tarjeta?
                    string bancoActual = null;
                    using (var sel = new MySqlCommand("SELECT banco_id FROM UsuariosGlobal WHERE tarjeta=@t LIMIT 1", cn))
                    {
                        sel.Parameters.AddWithValue("@t", tarjeta);
                        var o = sel.ExecuteScalar();
                        if (o != null && o != DBNull.Value) bancoActual = Convert.ToString(o);
                    }

                    if (bancoActual == null)
                    {
                        // Insert
                        using (var ins = new MySqlCommand(
                            "INSERT INTO UsuariosGlobal(tarjeta,nombre,banco_id) VALUES(@t,@n,@b)", cn))
                        {
                            ins.Parameters.AddWithValue("@t", tarjeta);
                            ins.Parameters.AddWithValue("@n", nombre);
                            ins.Parameters.AddWithValue("@b", bancoId);
                            ins.ExecuteNonQuery();
                            error = ""; return 1;
                        }
                    }

                    // Ya existe
                    if (!string.Equals(bancoActual, bancoId, StringComparison.Ordinal))
                    {
                        // Conflicto: pertenece a otro banco
                        error = "Tarjeta pertenece al banco " + bancoActual + ".";
                        return -2;
                    }

                    // Mismo banco: solo actualizar nombre
                    using (var up = new MySqlCommand(
                        "UPDATE UsuariosGlobal SET nombre=@n WHERE tarjeta=@t AND banco_id=@b", cn))
                    {
                        up.Parameters.AddWithValue("@n", nombre);
                        up.Parameters.AddWithValue("@t", tarjeta);
                        up.Parameters.AddWithValue("@b", bancoId);
                        up.ExecuteNonQuery();
                        error = ""; return 2;
                    }
                }
            }
            catch (Exception ex) { error = ex.Message; return -1; }
        }

        public static string GetNombreUsuarioGlobal(string tarjeta, string bancoId)
        {
            using (var cn = new MySqlConnection(CS))
            {
                cn.Open();
                const string sql = "SELECT nombre FROM UsuariosGlobal WHERE tarjeta=@t AND banco_id=@b";
                using (var cmd = new MySqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@t", tarjeta);
                    cmd.Parameters.AddWithValue("@b", bancoId);
                    var o = cmd.ExecuteScalar();
                    return o == null ? null : Convert.ToString(o);
                }
            }
        }

        // ---------- TransferenciasGlobal ----------
        // Inserta usando columnas banco_destino y descripcion
        public static long InsertarTransferenciaGlobal(
            string tarjetaOrigen, string bancoDestino, string tarjetaDestino, decimal monto, string descripcion)
        {
            using (var cn = new MySqlConnection(CS))
            {
                cn.Open();
                const string sql = @"
INSERT INTO TransferenciasGlobal
(tarjeta_origen, banco_destino, tarjeta_destino, monto, descripcion, fecha)
VALUES (@o,@b,@d,@m,@desc,NOW())";
                using (var cmd = new MySqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@o", tarjetaOrigen);
                    cmd.Parameters.AddWithValue("@b", bancoDestino);
                    cmd.Parameters.AddWithValue("@d", tarjetaDestino);
                    cmd.Parameters.AddWithValue("@m", monto);
                    cmd.Parameters.AddWithValue("@desc",
                        string.IsNullOrWhiteSpace(descripcion) ? (object)DBNull.Value : descripcion);
                    cmd.ExecuteNonQuery();
                    using (var idCmd = new MySqlCommand("SELECT LAST_INSERT_ID()", cn))
                        return Convert.ToInt64(idCmd.ExecuteScalar());
                }
            }
        }

        public sealed class TransferPendiente
        {
            public long Id;
            public string Origen;
            public string DestBanco;
            public string DestTarjeta;
            public decimal Monto;
            public string Descripcion;
        }

        public static List<TransferPendiente> ListarPendientesParaBanco(string bancoId)
        {
            var list = new List<TransferPendiente>();
            using (var cn = new MySqlConnection(CS))
            {
                cn.Open();
                const string sql = @"
SELECT id, tarjeta_origen, tarjeta_destino, monto, descripcion
FROM TransferenciasGlobal
WHERE estado=0 AND banco_destino=@b
ORDER BY id";
                using (var cmd = new MySqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@b", bancoId);
                    using (var rd = cmd.ExecuteReader())
                        while (rd.Read())
                            list.Add(new TransferPendiente
                            {
                                Id = rd.GetInt64(0),
                                Origen = rd.GetString(1),
                                DestBanco = bancoId,
                                DestTarjeta = rd.GetString(2),
                                Monto = rd.GetDecimal(3),
                                Descripcion = rd.IsDBNull(4) ? "" : rd.GetString(4)
                            });
                }
            }
            return list;
        }

        public static int MarcarAplicada(long id)
        {
            using (var cn = new MySqlConnection(CS))
            {
                cn.Open();
                const string sql =
                    "UPDATE TransferenciasGlobal SET estado=1, aplicado_en=NOW() WHERE id=@id AND estado=0";
                using (var cmd = new MySqlCommand(sql, cn))
                { cmd.Parameters.AddWithValue("@id", id); return cmd.ExecuteNonQuery(); }
            }
        }
    }
}
