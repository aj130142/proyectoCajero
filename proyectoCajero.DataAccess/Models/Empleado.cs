using System;

namespace proyectoCajero
{
    public class Empleado
    {
        public int EmpleadoID { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public string HashContraseña { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string CorreoInstitucional { get; set; } = string.Empty;
        public int RolID { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}