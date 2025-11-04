using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using proyectoCajero.DataAccess.Models;

namespace proyectoCajero
{
    public class ConexionBd
    {
        private readonly string _connectionString;

        public ConexionBd()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["CajeroDB"]?.ConnectionString
                ?? throw new InvalidOperationException("Connection string 'CajeroDB' no encontrada en App.config");
        }

        public async Task<SqlConnection> OpenConnectionAsync()
        {
            var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            return conn;
        }

        public async Task<int> ExecuteNonQueryAsync(string sql, IEnumerable<SqlParameter>? parameters = null, SqlTransaction? tx = null)
        {
            if (tx != null)
            {
                using var cmd = tx.Connection!.CreateCommand();
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                cmd.Transaction = tx;
                if (parameters != null)
                {
                    foreach (var p in parameters) cmd.Parameters.Add(p);
                }
                return await cmd.ExecuteNonQueryAsync();
            }

            using var conn = await OpenConnectionAsync();
            using var cmd2 = conn.CreateCommand();
            cmd2.CommandText = sql;
            cmd2.CommandType = CommandType.Text;
            if (parameters != null)
            {
                foreach (var p in parameters) cmd2.Parameters.Add(p);
            }
            return await cmd2.ExecuteNonQueryAsync();
        }

        public async Task<T?> ExecuteScalarAsync<T>(string sql, IEnumerable<SqlParameter>? parameters = null, SqlTransaction? tx = null)
        {
            object? obj;
            if (tx != null)
            {
                using var cmd = tx.Connection!.CreateCommand();
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                cmd.Transaction = tx;
                if (parameters != null) foreach (var p in parameters) cmd.Parameters.Add(p);
                obj = await cmd.ExecuteScalarAsync();
            }
            else
            {
                using var conn = await OpenConnectionAsync();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                if (parameters != null) foreach (var p in parameters) cmd.Parameters.Add(p);
                obj = await cmd.ExecuteScalarAsync();
            }

            if (obj == null || obj == DBNull.Value) return default;
            return (T)Convert.ChangeType(obj, typeof(T));
        }

        public async Task<List<T>> QueryAsync<T>(string sql, Func<SqlDataReader, T> map, IEnumerable<SqlParameter>? parameters = null)
        {
            using var conn = await OpenConnectionAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            if (parameters != null) foreach (var p in parameters) cmd.Parameters.Add(p);

            using var reader = await cmd.ExecuteReaderAsync();
            var list = new List<T>();
            while (await reader.ReadAsync())
            {
                list.Add(map(reader));
            }
            return list;
        }

        // Ajustado a esquema: Usuario.UsuarioID, Usuario.Nombres/Apellidos, Cuenta.MontoMaximoRetiroDiario, Tarjeta.PinHash
        public async Task<Usuario?> GetUsuarioByNumeroTarjetaAsync(string numeroTarjeta)
        {
            const string sql = @"
SELECT TOP(1)
    u.UsuarioID as UsuarioId,
    (u.Nombres + ' ' + u.Apellidos) as UsuarioNombre,
    t.NumeroTarjeta as NumeroTarjeta,
    c.SaldoActual as SaldoActual,
    c.MontoMaximoRetiroDiario as MontoMaximoRetiroDiario
FROM Tarjeta t
INNER JOIN Cuenta c ON t.CuentaID = c.CuentaID
INNER JOIN Usuario u ON c.UsuarioID = u.UsuarioID
WHERE t.NumeroTarjeta = @num";

            var parametros = new List<SqlParameter>
            {
                new SqlParameter("@num", SqlDbType.NVarChar) { Value = numeroTarjeta }
            };

            var lista = await QueryAsync(sql, reader =>
            {
                var u = new Usuario();
                u.Id = reader.GetInt32(reader.GetOrdinal("UsuarioId"));
                u.Nombre = reader.GetString(reader.GetOrdinal("UsuarioNombre"));
                u.NumeroTarjeta = reader.GetString(reader.GetOrdinal("NumeroTarjeta"));
                u.SaldoActual = reader.GetDecimal(reader.GetOrdinal("SaldoActual"));
                u.MontoMaximoDiario = reader.GetDecimal(reader.GetOrdinal("MontoMaximoRetiroDiario"));
                return u;
            }, parametros);

            return lista.Count > 0 ? lista[0] : null;
        }

        // sp_set_session_context('EmpleadoId', @id)
        public async Task SetSessionContextAsync(SqlConnection conn, int empleadoId, SqlTransaction? tx = null)
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "EXEC sp_set_session_context @key, @value";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@key", SqlDbType.NVarChar) { Value = "EmpleadoId" });
            cmd.Parameters.Add(new SqlParameter("@value", SqlDbType.Int) { Value = empleadoId });
            if (tx != null) cmd.Transaction = tx;
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<List<(string Codigo, string Nombre)>> GetTiposTransaccionAsync()
        {
            const string sql = "SELECT Codigo, Nombre FROM TipoTransaccion ORDER BY Nombre";
            var list = new List<(string Codigo, string Nombre)>();
            using var conn = await OpenConnectionAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                string codigo = reader.IsDBNull(0) ? string.Empty : reader.GetString(0);
                string nombre = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                list.Add((codigo, nombre));
            }
            return list;
        }

        // Helper to remove diacritics for accent-insensitive comparisons
        private static string RemoveDiacritics(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            var normalized = text.Normalize(System.Text.NormalizationForm.FormD);
            var sb = new System.Text.StringBuilder();
            foreach (var ch in normalized)
            {
                var uc = CharUnicodeInfo.GetUnicodeCategory(ch);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(ch);
                }
            }
            return sb.ToString().Normalize(System.Text.NormalizationForm.FormC);
        }

        // Query transactions by date range and optional typeCode (type filtering done in SQL if code provided)
        public async Task<List<TransactionDto>> QueryTransactionsAsync(string numeroTarjeta, DateTime? desde, DateTime? hasta, string? tipoCodigo)
        {
            var sql = new System.Text.StringBuilder();
            sql.AppendLine("SELECT tr.FechaHora, tt.Nombre AS Tipo, t.NumeroTarjeta, tr.Monto");
            sql.AppendLine("FROM Transaccion tr");
            sql.AppendLine("INNER JOIN Tarjeta t ON tr.TarjetaID = t.TarjetaID");
            sql.AppendLine("INNER JOIN TipoTransaccion tt ON tr.TipoTransaccionID = tt.TipoTransaccionID");
            sql.AppendLine("WHERE t.NumeroTarjeta = @num");

            var parametros = new List<SqlParameter>
            {
                new SqlParameter("@num", SqlDbType.NVarChar) { Value = numeroTarjeta }
            };

            if (desde.HasValue && hasta.HasValue)
            {
                sql.AppendLine("AND CAST(tr.FechaHora AS DATE) BETWEEN @desde AND @hasta");
                parametros.Add(new SqlParameter("@desde", SqlDbType.Date) { Value = desde.Value.Date });
                parametros.Add(new SqlParameter("@hasta", SqlDbType.Date) { Value = hasta.Value.Date });
            }

            if (!string.IsNullOrEmpty(tipoCodigo) && tipoCodigo != "Todos")
            {
                sql.AppendLine("AND tt.Codigo = @codigo");
                parametros.Add(new SqlParameter("@codigo", SqlDbType.NVarChar) { Value = tipoCodigo });
            }

            sql.AppendLine("ORDER BY tr.FechaHora DESC");

            var results = await QueryAsync(sql.ToString(), reader => new TransactionDto
            {
                FechaHora = reader.GetDateTime(reader.GetOrdinal("FechaHora")),
                Tipo = reader.GetString(reader.GetOrdinal("Tipo")),
                NumeroTarjeta = reader.GetString(reader.GetOrdinal("NumeroTarjeta")),
                Monto = reader.GetDecimal(reader.GetOrdinal("Monto"))
            }, parametros);

            return results;
        }

        // Query recent/top transactions for a given card
        public async Task<List<TransactionDto>> QueryRecentTransactionsAsync(string numeroTarjeta, int top = 10)
        {
            const string sql = @"SELECT TOP(@top) tr.FechaHora, tt.Nombre AS Tipo, t.NumeroTarjeta, tr.Monto
FROM Transaccion tr
INNER JOIN Tarjeta t ON tr.TarjetaID = t.TarjetaID
INNER JOIN TipoTransaccion tt ON tr.TipoTransaccionID = tt.TipoTransaccionID
WHERE t.NumeroTarjeta = @num
ORDER BY tr.FechaHora DESC";

            var parametros = new List<SqlParameter>
            {
                new SqlParameter("@top", SqlDbType.Int) { Value = top },
                new SqlParameter("@num", SqlDbType.NVarChar) { Value = numeroTarjeta }
            };

            return await QueryAsync(sql, reader => new TransactionDto
            {
                FechaHora = reader.GetDateTime(reader.GetOrdinal("FechaHora")),
                Tipo = reader.GetString(reader.GetOrdinal("Tipo")),
                NumeroTarjeta = reader.GetString(reader.GetOrdinal("NumeroTarjeta")),
                Monto = reader.GetDecimal(reader.GetOrdinal("Monto"))
            }, parametros);
        }
    }
}
