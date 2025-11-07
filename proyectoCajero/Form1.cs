using System.Text.Json;
using static proyectoCajero.archivosTxt;
using static proyectoCajero.carpetas.IVercrearArchivo;
using static proyectoCajero.cajeroInicializar;

namespace proyectoCajero
{
    public partial class Form1 : Form
    {
        List<string> listaLog = new List<string>(); //lista que guarda los admin
        public Form1()
        {
            InitializeComponent();
            administarToolStripMenuItem.Visible = true;//oculta el resto del menu hasta que inicies sesion, cambialo para tener acceso

        }
        public class DataModel
        {
            public string Nombre { get; set; }
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            // Comprobación rápida de conexión a la base de datos al iniciar la aplicación.
            try
            {
                var conexion = new ConexionBd();
                using var conn = await conexion.OpenConnectionAsync();
                // Si llegamos aquí, la conexión fue exitosa.
                MessageBox.Show("Conexión a la base de datos exitosa.", "Conexión OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Cerrar la conexión (using se encarga de ello)
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al conectar con la base de datos: {ex.Message}", "Error de Conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Opcional: puedes deshabilitar la UI administrativa aquí si la conexión es crítica.
            }

            verificarCarpeta();

            var ruta = new archivosTxt.rutasJSOn();
            string ruta1 = ruta.ruta();
            string rutaf = ruta1 + @"\" + "archivo.json";
            var data = new DataModel { Nombre = "Ejemplo" };
            string json = JsonSerializer.Serialize(data);
            File.WriteAllText(rutaf, json);

        }

        private async void sesionBtn_Click(object sender, EventArgs e)
        {
            string userLog = admName.Text; //obtiene el usuario
            string contraLog = contAdm.Text;//obtiene la contrasena

            try
            {
                var conexion = new ConexionBd();
                string sql = "SELECT TOP(1) EmpleadoID, NombreUsuario, HashContraseña, Activo FROM Empleado WHERE NombreUsuario = @user";
                var parametros = new List<Microsoft.Data.SqlClient.SqlParameter>
                {
                    new Microsoft.Data.SqlClient.SqlParameter("@user", System.Data.SqlDbType.NVarChar) { Value = userLog }
                };

                var lista = await conexion.QueryAsync(sql, reader =>
                {
                    var emp = new Empleado();
                    emp.EmpleadoID = reader.GetInt32(reader.GetOrdinal("EmpleadoID"));
                    emp.NombreUsuario = reader.GetString(reader.GetOrdinal("NombreUsuario"));
                    emp.HashContraseña = reader.GetString(reader.GetOrdinal("HashContraseña"));
                    emp.Activo = reader.GetBoolean(reader.GetOrdinal("Activo"));
                    return emp;
                }, parametros);

                if (lista.Count == 0)
                {
                    MessageBox.Show("Usuario no encontrado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var empleado = lista[0];
                if (!empleado.Activo)
                {
                    MessageBox.Show("Empleado inactivo.", "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Verificar hash
                bool valido = HashHelper.VerifySha256Hash(contraLog, empleado.HashContraseña);
                if (!valido)
                {
                    MessageBox.Show("Contraseña incorrecta.", "Error de autenticación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Acceso concedido: establecer empleado en AppState y mostrar menus administrativos
                AppState.CurrentEmpleadoId = empleado.EmpleadoID;
                administarToolStripMenuItem.Visible = true;
                admName.Visible = false;
                contAdm.Visible = false;
                label1.Visible = false;
                label2.Visible = false;
                sesionBtn.Visible = false;
                newAdmBtn.Visible = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al intentar iniciar sesión: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void newAdmBtn_Click(object sender, EventArgs e)
        {
            agregarAdmin agregaAd = new agregarAdmin();
            agregaAd.ShowDialog(); //llama a la ventana agregarAdmin
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void insertarUsuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            insertarUsuario ventanaInser = new insertarUsuario();
            ventanaInser.Show(); // abrimos la ventana insertarUsuario
        }

        private void buscarUsuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            controlUsuario ventanaControl = new controlUsuario();
            ventanaControl.Show(); // abrimos la ventana controlUsuario
        }

        private void modificarUsuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            modificarUsuarios modificarUser = new modificarUsuarios();
            modificarUser.Show();// abrimos modificarUsuarios
        }

        private void activarCajerosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cajeroInicializar inicia = new cajeroInicializar(CajeroFormMode.Inicializar);
            inicia.ShowDialog(); // Usar ShowDialog es mejor para ventanas que deben completarse
        }

        private void agregarEfectivoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Abrimos el mismo formulario, pero en modo Agregar
            cajeroInicializar agrega = new cajeroInicializar(CajeroFormMode.Agregar);
            agrega.ShowDialog();
        }

        private void btnModoUsuario_Click(object sender, EventArgs e)
        {
            // Creamos una instancia del nuevo formulario de login
            LoginUsuario ventanaLoginUsuario = new LoginUsuario();

            // Ocultamos la ventana principal de administración
            this.Hide();

            // Mostramos el formulario de login. Usamos ShowDialog para que la ejecución
            // se detenga aquí hasta que el usuario cierre la ventana de login.
            ventanaLoginUsuario.ShowDialog();

            // Una vez que la ventana de login se cierra (ya sea por éxito o cancelación),
            // volvemos a mostrar la ventana principal de administración.
            this.Show();
        }

        private void gestionarEmpleadosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ventanaEmpleado = new CapaPresentacion.GestionarEmpleado();
            this.Hide();
            ventanaEmpleado.ShowDialog();
            this.Show();
        }
    }
}
