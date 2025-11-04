-- =================================================================================
-- Script para Insertar el Primer Empleado (Administrador)
-- Base de Datos: CajeroDB
-- =================================================================================

-- Asegurarse de que se está usando la base de datos correcta
USE CajeroDB;
GO

-- Insertar el empleado administrador.
-- Asumimos que el RolID = 1 corresponde a 'Administrador de Cajeros'
-- según el script de población de catálogos que ejecutamos anteriormente.

INSERT INTO Empleado (
    NombreUsuario,
    HashContraseña, -- ¡¡¡LEER EXPLICACIÓN IMPORTANTE ABAJO!!!
    Nombres,
    Apellidos,
    CorreoInstitucional,
    RolID,
    Activo
)
VALUES (
    'admin',
    '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918', -- HASH de ejemplo para la contraseña 'admin123'. NO USAR EN PRODUCCIÓN.
    N'Administrador',
    N'Principal',
    'admin@tubanco.com',
    1, -- Este es el ID del Rol 'Administrador de Cajeros'
    1  -- 1 = Activo
);
GO

PRINT '¡Empleado Administrador creado exitosamente!';
GO