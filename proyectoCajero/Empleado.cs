using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using proyectoCajero; // for ConexionBd, HashHelper, AppState

namespace CapaPresentacion
{
    public partial class GestionarEmpleado : Form
    {
        private int? _editingEmpleadoId = null;
        private bool _editing = false;

        public GestionarEmpleado()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await LoadRolesAsync();
            await LoadEmpleadosAsync();
        }

        private async Task LoadRolesAsync()
        {
            try
            {
                var conexion = new ConexionBd();
                string sql = "SELECT RolID, NombreRol, DescripcionRol FROM Rol WHERE Activo = 1 ORDER BY NombreRol";
                var lista = await conexion.QueryAsync(sql, reader => new RoleItem
                {
                    RolID = reader.GetInt32(reader.GetOrdinal("RolID")),
                    NombreRol = reader.GetString(reader.GetOrdinal("NombreRol")),
                    DescripcionRol = reader.IsDBNull(reader.GetOrdinal("DescripcionRol")) ? string.Empty : reader.GetString(reader.GetOrdinal("DescripcionRol"))
                });

                rolCBox.DisplayMember = "NombreRol";
                rolCBox.ValueMember = "RolID";
                rolCBox.DataSource = lista;
                rolCBox.SelectedIndexChanged += RolCBox_SelectedIndexChanged;
                // Set initial description
                if (lista.Count > 0)
                {
                    descripcionRolLabel.Text = lista[0].DescripcionRol;
                }
            }
            catch (Exception)
            {
                // ignore load errors
            }
        }

        private void RolCBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (rolCBox.SelectedItem is RoleItem r)
            {
                descripcionRolLabel.Text = r.DescripcionRol ?? string.Empty;
            }
            else
            {
                descripcionRolLabel.Text = string.Empty;
            }
        }

        private async Task LoadEmpleadosAsync()
        {
            try
            {
                var conexion = new ConexionBd();
                string sql = @"SELECT e.EmpleadoID, e.NombreUsuario, e.Nombres, e.Apellidos, e.CorreoInstitucional, e.RolID, r.NombreRol, e.Activo
                               FROM Empleado e
                               LEFT JOIN Rol r ON e.RolID = r.RolID
                               ORDER BY e.EmpleadoID";
                var lista = await conexion.QueryAsync(sql, reader => new EmployeeGridItem
                {
                    EmpleadoID = reader.GetInt32(reader.GetOrdinal("EmpleadoID")),
                    NombreUsuario = reader.GetString(reader.GetOrdinal("NombreUsuario")),
                    Nombres = reader.IsDBNull(reader.GetOrdinal("Nombres")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombres")),
                    Apellidos = reader.IsDBNull(reader.GetOrdinal("Apellidos")) ? string.Empty : reader.GetString(reader.GetOrdinal("Apellidos")),
                    Correo = reader.IsDBNull(reader.GetOrdinal("CorreoInstitucional")) ? string.Empty : reader.GetString(reader.GetOrdinal("CorreoInstitucional")),
                    RolID = reader.GetInt32(reader.GetOrdinal("RolID")),
                    RolNombre = reader.IsDBNull(reader.GetOrdinal("NombreRol")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreRol")),
                    Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
                });

                dataGridView1.DataSource = lista;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar empleados: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            string nombreUsuario = nombreUsuariotxt.Text?.Trim() ?? string.Empty;
            string contrasena = contrasenaTxt.Text ?? string.Empty;
            string nombres = nombresTxt.Text?.Trim() ?? string.Empty;
            string apellidos = apellidosTxt.Text?.Trim() ?? string.Empty;
            string correo = correoTxt.Text?.Trim() ?? string.Empty;
            int rolId = rolCBox.SelectedValue is int i ? i : (rolCBox.SelectedValue is byte b ? b : 1);
            bool activo = checkBox1.Checked;

            if (string.IsNullOrWhiteSpace(nombreUsuario) || string.IsNullOrWhiteSpace(nombres) || string.IsNullOrWhiteSpace(apellidos) || string.IsNullOrWhiteSpace(correo))
            {
                MessageBox.Show("Por favor complete los campos obligatorios (usuario, nombres, apellidos, correo).", "Datos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var conexion = new ConexionBd();
                // Check unique username and email for inserts or when changed
                string sqlCheckUser = "SELECT COUNT(1) FROM Empleado WHERE NombreUsuario = @user" + (_editing && _editingEmpleadoId.HasValue ? " AND EmpleadoID <> @id" : "");
                var parametros = new List<SqlParameter> { new SqlParameter("@user", System.Data.SqlDbType.NVarChar) { Value = nombreUsuario } };
                if (_editing && _editingEmpleadoId.HasValue) parametros.Add(new SqlParameter("@id", System.Data.SqlDbType.Int) { Value = _editingEmpleadoId.Value });
                int existeUser = await conexion.ExecuteScalarAsync<int>(sqlCheckUser, parametros);
                if (existeUser > 0)
                {
                    MessageBox.Show("El nombre de usuario ya existe.", "Duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string sqlCheckEmail = "SELECT COUNT(1) FROM Empleado WHERE CorreoInstitucional = @correo" + (_editing && _editingEmpleadoId.HasValue ? " AND EmpleadoID <> @id" : "");
                var parametrosEmail = new List<SqlParameter> { new SqlParameter("@correo", System.Data.SqlDbType.NVarChar) { Value = correo } };
                if (_editing && _editingEmpleadoId.HasValue) parametrosEmail.Add(new SqlParameter("@id", System.Data.SqlDbType.Int) { Value = _editingEmpleadoId.Value });
                int existeEmail = await conexion.ExecuteScalarAsync<int>(sqlCheckEmail, parametrosEmail);
                if (existeEmail > 0)
                {
                    MessageBox.Show("El correo institucional ya está registrado.", "Duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!_editing)
                {
                    if (string.IsNullOrWhiteSpace(contrasena))
                    {
                        MessageBox.Show("Debe especificar una contraseña para el nuevo empleado.", "Datos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    string hash = HashHelper.ComputeSha256Hash(contrasena);
                    string sqlInsert = @"INSERT INTO Empleado (NombreUsuario, HashContraseña, Nombres, Apellidos, CorreoInstitucional, RolID, Activo) VALUES (@user, @hash, @nombres, @apellidos, @correo, @rol, @activo)";
                    var parametrosInsert = new List<SqlParameter>
                    {
                        new SqlParameter("@user", System.Data.SqlDbType.NVarChar) { Value = nombreUsuario },
                        new SqlParameter("@hash", System.Data.SqlDbType.NVarChar) { Value = hash },
                        new SqlParameter("@nombres", System.Data.SqlDbType.NVarChar) { Value = nombres },
                        new SqlParameter("@apellidos", System.Data.SqlDbType.NVarChar) { Value = apellidos },
                        new SqlParameter("@correo", System.Data.SqlDbType.NVarChar) { Value = correo },
                        new SqlParameter("@rol", System.Data.SqlDbType.Int) { Value = rolId },
                        new SqlParameter("@activo", System.Data.SqlDbType.Bit) { Value = activo }
                    };
                    int filas = await conexion.ExecuteNonQueryAsync(sqlInsert, parametrosInsert);
                    if (filas > 0)
                    {
                        MessageBox.Show("Empleado creado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await LoadEmpleadosAsync();
                        ClearForm();
                    }
                    else
                    {
                        MessageBox.Show("No se pudo crear el empleado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // Update
                    var parametrosUpdate = new List<SqlParameter>
                    {
                        new SqlParameter("@nombres", System.Data.SqlDbType.NVarChar) { Value = nombres },
                        new SqlParameter("@apellidos", System.Data.SqlDbType.NVarChar) { Value = apellidos },
                        new SqlParameter("@correo", System.Data.SqlDbType.NVarChar) { Value = correo },
                        new SqlParameter("@rol", System.Data.SqlDbType.Int) { Value = rolId },
                        new SqlParameter("@activo", System.Data.SqlDbType.Bit) { Value = activo },
                        new SqlParameter("@id", System.Data.SqlDbType.Int) { Value = _editingEmpleadoId.Value }
                    };

                    string sqlUpdate = @"UPDATE Empleado SET Nombres = @nombres, Apellidos = @apellidos, CorreoInstitucional = @correo, RolID = @rol, Activo = @activo";
                    if (!string.IsNullOrWhiteSpace(contrasena))
                    {
                        string hash = HashHelper.ComputeSha256Hash(contrasena);
                        sqlUpdate += ", HashContraseña = @hash";
                        parametrosUpdate.Insert(0, new SqlParameter("@hash", System.Data.SqlDbType.NVarChar) { Value = hash });
                    }
                    sqlUpdate += " WHERE EmpleadoID = @id";

                    int filas = await conexion.ExecuteNonQueryAsync(sqlUpdate, parametrosUpdate);
                    if (filas > 0)
                    {
                        MessageBox.Show("Empleado actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await LoadEmpleadosAsync();
                        ClearForm();
                        _editing = false;
                        _editingEmpleadoId = null;
                    }
                    else
                    {
                        MessageBox.Show("No se pudo actualizar el empleado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar empleado: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnEditar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var row = (EmployeeGridItem)dataGridView1.CurrentRow.DataBoundItem;
                _editing = true;
                _editingEmpleadoId = row.EmpleadoID;
                nombreUsuariotxt.Text = row.NombreUsuario;
                nombresTxt.Text = row.Nombres;
                apellidosTxt.Text = row.Apellidos;
                correoTxt.Text = row.Correo;
                // select role in combo
                for (int i = 0; i < rolCBox.Items.Count; i++)
                {
                    if (rolCBox.Items[i] is RoleItem ri && ri.RolID == row.RolID)
                    {
                        rolCBox.SelectedIndex = i;
                        break;
                    }
                }
                checkBox1.Checked = row.Activo;
            }
            else
            {
                MessageBox.Show("Seleccione una fila por favor", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var row = (EmployeeGridItem)dataGridView1.CurrentRow.DataBoundItem;
                var confirm = MessageBox.Show($"¿Eliminar al empleado '{row.NombreUsuario}'?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.Yes)
                {
                    try
                    {
                        var conexion = new ConexionBd();
                        string sql = "DELETE FROM Empleado WHERE EmpleadoID = @id";
                        var parametros = new List<SqlParameter> { new SqlParameter("@id", System.Data.SqlDbType.Int) { Value = row.EmpleadoID } };
                        await conexion.ExecuteNonQueryAsync(sql, parametros);
                        MessageBox.Show("Empleado eliminado.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await LoadEmpleadosAsync();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al eliminar empleado: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Seleccione una fila por favor", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ClearForm()
        {
            nombreUsuariotxt.Clear();
            contrasenaTxt.Clear();
            nombresTxt.Clear();
            apellidosTxt.Clear();
            correoTxt.Clear();
            if (rolCBox.Items.Count > 0) rolCBox.SelectedIndex = 0;
            checkBox1.Checked = true;
            descripcionRolLabel.Text = string.Empty;
        }

        private class RoleItem
        {
            public int RolID { get; set; }
            public string NombreRol { get; set; } = string.Empty;
            public string DescripcionRol { get; set; } = string.Empty;
        }

        private class EmployeeGridItem
        {
            public int EmpleadoID { get; set; }
            public string NombreUsuario { get; set; } = string.Empty;
            public string Nombres { get; set; } = string.Empty;
            public string Apellidos { get; set; } = string.Empty;
            public string Correo { get; set; } = string.Empty;
            public int RolID { get; set; }
            public string RolNombre { get; set; } = string.Empty;
            public bool Activo { get; set; }
        }
    }
}
