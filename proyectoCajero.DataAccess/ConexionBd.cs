using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

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
    }
}
