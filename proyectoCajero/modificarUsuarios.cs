using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace proyectoCajero
{
    public partial class modificarUsuarios : Form
    {
        public modificarUsuarios()
        {
            InitializeComponent();
            tarjetaNewTB.Enabled = false;
            maxSaldoTB.Enabled = false;
        }

        private async void modificarUsuarios_Load(object sender, EventArgs e)
        {
            // No cargamos archivos. Podemos listar usuarios desde BD si se necesita.
        }

        private void modTarjCheckB_CheckedChanged_1(object sender, EventArgs e)
        {
            tarjetaNewTB.Enabled = modTarjCheckB.Checked;
        }

        private void modMaxSaldoCheckB_CheckedChanged(object sender, EventArgs e)
        {
            maxSaldoTB.Enabled = modMaxSaldoCheckB.Checked;
        }

        private async void okeyBtn_Click(object sender, EventArgs e)
        {
            string nombreBuscar = nameTB.Text?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(nombreBuscar))
            {
                MessageBox.Show("Por favor, ingrese el nombre del usuario a modificar.", "Datos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var conexion = new ConexionBd();
                using var conn = await conexion.OpenConnectionAsync();
                using var tx = conn.BeginTransaction();
                try
                {
                    // Buscar Usuario por nombre (asumimos nombres únicos o tomamos el primero)
                    int usuarioId = -1;
                    int cuentaId = -1;
                    using (var cmdBuscar = conn.CreateCommand())
                    {
                        cmdBuscar.Transaction = tx;
                        cmdBuscar.CommandText = @"SELECT TOP(1) u.UsuarioID as UsuarioId, c.CuentaID as CuentaId FROM Usuario u INNER JOIN Cuenta c ON u.UsuarioID = c.UsuarioID WHERE (u.Nombres + ' ' + u.Apellidos) = @nombre";
                        cmdBuscar.Parameters.Add(new SqlParameter("@nombre", SqlDbType.NVarChar) { Value = nombreBuscar });
                        using var reader = await cmdBuscar.ExecuteReaderAsync();
                        if (!await reader.ReadAsync())
                        {
                            MessageBox.Show("No se encontró ningún usuario con ese nombre en la base de datos.", "No encontrado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            tx.Rollback();
                            return;
                        }
                        usuarioId = reader.GetInt32(reader.GetOrdinal("UsuarioId"));
                        cuentaId = reader.GetInt32(reader.GetOrdinal("CuentaId"));
                    }

                    bool cambiosRealizados = false;

                    if (modTarjCheckB.Checked)
                    {
                        string nuevaTarjeta = tarjetaNewTB.Text?.Trim() ?? string.Empty;
                        if (nuevaTarjeta.Length < 12 || !long.TryParse(nuevaTarjeta, out _))
                        {
                            MessageBox.Show("El nuevo número de tarjeta debe ser de solo dígitos (mínimo 12).", "Formato inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            tx.Rollback();
                            return;
                        }

                        // Verificar que la tarjeta no exista en otra cuenta
                        using (var cmdCheck = conn.CreateCommand())
                        {
                            cmdCheck.Transaction = tx;
                            cmdCheck.CommandText = "SELECT COUNT(1) FROM Tarjeta WHERE NumeroTarjeta = @num AND CuentaID <> @cid";
                            cmdCheck.Parameters.Add(new SqlParameter("@num", SqlDbType.VarChar) { Value = nuevaTarjeta });
                            cmdCheck.Parameters.Add(new SqlParameter("@cid", SqlDbType.Int) { Value = cuentaId });
                            int existe = Convert.ToInt32(await cmdCheck.ExecuteScalarAsync());
                            if (existe > 0)
                            {
                                MessageBox.Show("Ese número de tarjeta ya está asignado a otra cuenta.", "Duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                tx.Rollback();
                                return;
                            }
                        }

                        using (var cmdUpdateTarj = conn.CreateCommand())
                        {
                            cmdUpdateTarj.Transaction = tx;
                            cmdUpdateTarj.CommandText = "UPDATE Tarjeta SET NumeroTarjeta = @num WHERE CuentaID = @cid";
                            cmdUpdateTarj.Parameters.Add(new SqlParameter("@num", SqlDbType.VarChar) { Value = nuevaTarjeta });
                            cmdUpdateTarj.Parameters.Add(new SqlParameter("@cid", SqlDbType.Int) { Value = cuentaId });
                            await cmdUpdateTarj.ExecuteNonQueryAsync();
                        }

                        cambiosRealizados = true;
                    }

                    if (modMaxSaldoCheckB.Checked)
                    {
                        if (!decimal.TryParse(maxSaldoTB.Text, out decimal nuevoMax))
                        {
                            MessageBox.Show("El nuevo límite de saldo debe ser un valor numérico válido.", "Formato inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            tx.Rollback();
                            return;
                        }

                        using (var cmdUpdateCuenta = conn.CreateCommand())
                        {
                            cmdUpdateCuenta.Transaction = tx;
                            cmdUpdateCuenta.CommandText = "UPDATE Cuenta SET MontoMaximoRetiroDiario = @max WHERE CuentaID = @cid";
                            cmdUpdateCuenta.Parameters.Add(new SqlParameter("@max", SqlDbType.Decimal) { Value = nuevoMax });
                            cmdUpdateCuenta.Parameters.Add(new SqlParameter("@cid", SqlDbType.Int) { Value = cuentaId });
                            await cmdUpdateCuenta.ExecuteNonQueryAsync();
                        }

                        cambiosRealizados = true;
                    }

                    if (cambiosRealizados)
                    {
                        tx.Commit();
                        MessageBox.Show("Usuario actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        tx.Rollback();
                        MessageBox.Show("No se seleccionó ninguna opción para modificar.", "Sin cambios", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception exTx)
                {
                    try { tx.Rollback(); } catch { }
                    MessageBox.Show("Error al actualizar usuario: " + exTx.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error de conexión o base de datos: " + ex.Message, "Error Crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void allSelect_CheckedChanged(object sender, EventArgs e)
        {
            if (allSelect.Checked)
            {
                modTarjCheckB.Checked = true;
                modMaxSaldoCheckB.Checked = true;
            }
            if (!allSelect.Checked)
            {
                modTarjCheckB.Checked = false;
                modMaxSaldoCheckB.Checked = false;
            }
        }
    }
}
