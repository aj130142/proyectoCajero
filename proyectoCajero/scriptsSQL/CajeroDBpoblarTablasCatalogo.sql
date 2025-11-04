-- =================================================================================
-- Script de Inserción de Datos Iniciales (Población de Catálogos)
-- Base de Datos: CajeroDB
-- =================================================================================

-- Asegurarse de que se está usando la base de datos correcta
USE CajeroDB;
GO

-- *********************************************************************************
-- NOTA IMPORTANTE:
-- Para las tablas con claves primarias que NO son IDENTITY (ej. TINYINT),
-- debemos insertar los IDs manualmente.
-- Para las tablas con claves primarias que SÍ SON IDENTITY (ej. Rol, Denominacion),
-- NO debemos especificar el ID, la base de datos lo hará automáticamente.
-- *********************************************************************************


-- 1. Poblando la tabla Rol
PRINT 'Poblando la tabla Rol...';
INSERT INTO Rol (NombreRol, DescripcionRol) VALUES
(N'Administrador de Cajeros', N'Permisos totales sobre la gestión de usuarios, cuentas, tarjetas y efectivo del cajero.'),
(N'Soporte Técnico', N'Permisos para consultar logs, ver estados de cuentas y usuarios, pero no para modificar datos financieros.'),
(N'Auditor', N'Permisos de solo lectura sobre todas las tablas de logs y transacciones.');
GO


-- 2. Poblando la tabla TipoCuenta
PRINT 'Poblando la tabla TipoCuenta...';
INSERT INTO TipoCuenta (TipoCuentaID, Nombre) VALUES
(1, N'Cuenta de Ahorros'),
(2, N'Cuenta Monetaria');
GO


-- 3. Poblando la tabla EstadoCuenta
PRINT 'Poblando la tabla EstadoCuenta...';
INSERT INTO EstadoCuenta (EstadoCuentaID, Nombre, Descripcion) VALUES
(1, N'Activa', N'La cuenta permite todas las operaciones de débito y crédito.'),
(2, N'Bloqueada', N'No se permiten transacciones. Requiere intervención manual de un administrador.'),
(3, N'Inactiva', N'La cuenta no ha tenido movimiento en un período prolongado.'),
(4, N'Cerrada', N'La cuenta ha sido cerrada permanentemente y no puede ser reactivada.');
GO


-- 4. Poblando la tabla EstadoTarjeta
PRINT 'Poblando la tabla EstadoTarjeta...';
INSERT INTO EstadoTarjeta (EstadoTarjetaID, Nombre, Descripcion) VALUES
(1, N'Activa', N'La tarjeta está activa y puede ser utilizada para transacciones.'),
(2, N'Bloqueada - PIN incorrecto', N'Bloqueo temporal por múltiples intentos de PIN fallidos.'),
(3, N'Vencida', N'La fecha de expiración de la tarjeta ha pasado.'),
(4, N'Reportada como robada/extraviada', N'Bloqueo permanente solicitado por el cliente.'),
(5, N'Inactiva', N'La tarjeta ha sido emitida pero aún no ha sido activada por el cliente.');
GO


-- 5. Poblando la tabla EstadoCajero
PRINT 'Poblando la tabla EstadoCajero...';
INSERT INTO EstadoCajero (EstadoCajeroID, Nombre) VALUES
(1, N'En Servicio'),
(2, N'Fuera de Servicio'),
(3, N'Bajo Mantenimiento'),
(4, N'Bajo en Efectivo');
GO


-- 6. Poblando la tabla Denominacion (Según los requisitos del PDF)
PRINT 'Poblando la tabla Denominacion...';
INSERT INTO Denominacion (Valor, Moneda, Tipo) VALUES
(200.00, 'GTQ', 'Billete'),
(100.00, 'GTQ', 'Billete'),
(50.00, 'GTQ', 'Billete'),
(20.00, 'GTQ', 'Billete'),
(10.00, 'GTQ', 'Billete'),
(5.00, 'GTQ', 'Billete'),
(1.00, 'GTQ', 'Moneda'); -- Aunque el PDF dice "billetes", 1 es usualmente moneda.
GO


-- 7. Poblando la tabla TipoTransaccion
PRINT 'Poblando la tabla TipoTransaccion...';
INSERT INTO TipoTransaccion (TipoTransaccionID, Nombre, Codigo, AfectaSaldo) VALUES
(1, N'Retiro de Efectivo', 'RET', 1),
(2, N'Depósito en Efectivo', 'DEP', 1),
(3, N'Consulta de Saldo', 'CON', 0);
GO


-- 8. Poblando la tabla MotivoFalloLogin
PRINT 'Poblando la tabla MotivoFalloLogin...';
INSERT INTO MotivoFalloLogin (MotivoFalloID, Codigo, Descripcion) VALUES
(1, 'PIN_INCORRECTO', N'El número de PIN ingresado no coincide con el registrado para la tarjeta.'),
(2, 'TARJETA_NO_EXISTE', N'El número de tarjeta ingresado no se encuentra en el sistema.'),
(3, 'TARJETA_BLOQUEADA', N'La tarjeta se encuentra en un estado bloqueado y no puede ser utilizada.'),
(4, 'TARJETA_VENCIDA', N'La fecha de expiración de la tarjeta ha pasado.'),
(5, 'TARJETA_INACTIVA', N'La tarjeta aún no ha sido activada por el cliente.');
GO


-- 9. Poblando la tabla TipoMovimientoEfectivo (Según los requisitos del PDF)
PRINT 'Poblando la tabla TipoMovimientoEfectivo...';
INSERT INTO TipoMovimientoEfectivo (TipoMovimientoID, Nombre) VALUES
(1, N'Inicialización de Cajero'),
(2, N'Recarga de Efectivo');
GO


PRINT '¡Todas las tablas de catálogo han sido pobladas exitosamente!';