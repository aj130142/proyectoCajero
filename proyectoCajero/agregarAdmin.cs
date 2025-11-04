using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static proyectoCajero.archivosTxt;
using Microsoft.Data.SqlClient;

namespace proyectoCajero
{

    public partial class agregarAdmin : Form
    {
        List<string> listaLog = new List<string>();//lista que guarda los textos de los textbox
        public agregarAdmin()
        {
            InitializeComponent();
        }

        private async void guardarBtn_Click(object sender, EventArgs e)
        {
            string newUser = nameTxt.Text?.Trim() ?? string.Empty; //obtiene el usuario
            string newpass = passTxt.Text ?? string.Empty; //obtine la contraseña

            if (string.IsNullOrWhiteSpace(newUser) || string.IsNullOrWhiteSpace(newpass))
            {
                MessageBox.Show("Debe especificar usuario y contraseña.", "Datos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var conexion = new ConexionBd();

                // Verificar si ya existe el usuario
                string sqlCheck = "SELECT COUNT(1) FROM Empleado WHERE NombreUsuario = @user";
                var parametrosCheck = new List<SqlParameter>
                {
                    new SqlParameter("@user", System.Data.SqlDbType.NVarChar) { Value = newUser }
                };

                int existe = await conexion.ExecuteScalarAsync<int>(sqlCheck, parametrosCheck);
                if (existe > 0)
                {
                    MessageBox.Show("El nombre de usuario ya existe en la base de datos.", "Usuario duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Hashear la contraseña antes de guardar
                string hash = HashHelper.ComputeSha256Hash(newpass);

                // Insertar nuevo empleado (por defecto RolID = 1, Activo = 1)
                string sqlInsert = @"INSERT INTO Empleado (NombreUsuario, HashContraseña, Nombres, Apellidos, CorreoInstitucional, RolID, Activo)
VALUES (@user, @hash, @nombres, @apellidos, @correo, @rol, @activo)";

                var parametrosInsert = new List<SqlParameter>
                {
                    new SqlParameter("@user", System.Data.SqlDbType.NVarChar) { Value = newUser },
                    new SqlParameter("@hash", System.Data.SqlDbType.NVarChar) { Value = hash },
                    new SqlParameter("@nombres", System.Data.SqlDbType.NVarChar) { Value = "" },
                    new SqlParameter("@apellidos", System.Data.SqlDbType.NVarChar) { Value = "" },
                    new SqlParameter("@correo", System.Data.SqlDbType.NVarChar) { Value = "" },
                    new SqlParameter("@rol", System.Data.SqlDbType.Int) { Value = 1 },
                    new SqlParameter("@activo", System.Data.SqlDbType.Bit) { Value = 1 }
                };

                int filas = await conexion.ExecuteNonQueryAsync(sqlInsert, parametrosInsert);
                if (filas > 0)
                {
                    MessageBox.Show("Administrador creado exitosamente en la base de datos.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("No se pudo crear el administrador.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al crear el administrador: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
