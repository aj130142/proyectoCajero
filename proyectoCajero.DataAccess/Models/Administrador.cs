using System;

namespace proyectoCajero
{
    public class Administrador
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        // No incluimos la propiedad Password por seguridad.
    }
}