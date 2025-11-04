-- =================================================================================
-- Script de Creación de Triggers para Auditoría Automática
-- Base de Datos: CajeroDB
-- =================================================================================

USE CajeroDB;
GO

-- --- Trigger 1: Auditoría de Cambios en la tabla Usuario ---
-- Se dispara después de un UPDATE en la tabla Usuario.
-- Registra cada cambio de columna individualmente en LogActualizacionUsuario.
PRINT 'Creando Trigger para auditar la tabla Usuario...';
GO
CREATE TRIGGER TRG_Usuario_AfterUpdate
ON Usuario
AFTER UPDATE
AS
BEGIN
    -- No hacer nada si no hay filas afectadas
    IF NOT EXISTS (SELECT * FROM inserted) AND NOT EXISTS (SELECT * FROM deleted)
        RETURN;

    -- Declarar variables para almacenar los datos del log
    DECLARE @UsuarioModificadoID INT;
    DECLARE @AdminID INT; -- Lo obtendremos del contexto de la sesión

    -- Obtener el EmpleadoID que está realizando la acción desde el contexto de la sesión
    SET @AdminID = CAST(SESSION_CONTEXT(N'EmpleadoID') AS INT);

    -- Si no se ha establecido un EmpleadoID en el contexto, lanzar un error para prevenir cambios no auditados
    IF @AdminID IS NULL
    BEGIN
        THROW 50001, 'No se puede modificar un usuario sin un EmpleadoID en el contexto de la sesión.', 1;
        ROLLBACK TRANSACTION;
        RETURN;
    END

    -- Obtener el ID del usuario que fue modificado
    SELECT @UsuarioModificadoID = UsuarioID FROM inserted;

    -- Revisar cada columna de interés y registrar el cambio si es diferente
    IF UPDATE(Nombres)
    BEGIN
        INSERT INTO LogActualizacionUsuario (UsuarioModificadoID, AdminID, CampoModificado, ValorAntiguo, ValorNuevo)
        SELECT i.UsuarioID, @AdminID, 'Nombres', d.Nombres, i.Nombres
        FROM inserted i JOIN deleted d ON i.UsuarioID = d.UsuarioID
        WHERE ISNULL(d.Nombres, '') <> ISNULL(i.Nombres, '');
    END

    IF UPDATE(Apellidos)
    BEGIN
        INSERT INTO LogActualizacionUsuario (UsuarioModificadoID, AdminID, CampoModificado, ValorAntiguo, ValorNuevo)
        SELECT i.UsuarioID, @AdminID, 'Apellidos', d.Apellidos, i.Apellidos
        FROM inserted i JOIN deleted d ON i.UsuarioID = d.UsuarioID
        WHERE ISNULL(d.Apellidos, '') <> ISNULL(i.Apellidos, '');
    END

    IF UPDATE(CorreoElectronico)
    BEGIN
        INSERT INTO LogActualizacionUsuario (UsuarioModificadoID, AdminID, CampoModificado, ValorAntiguo, ValorNuevo)
        SELECT i.UsuarioID, @AdminID, 'CorreoElectronico', d.CorreoElectronico, i.CorreoElectronico
        FROM inserted i JOIN deleted d ON i.UsuarioID = d.UsuarioID
        WHERE ISNULL(d.CorreoElectronico, '') <> ISNULL(i.CorreoElectronico, '');
    END
    
    -- Se pueden añadir más bloques IF para otras columnas como TelefonoCelular, Activo, etc.

END
GO


-- --- Trigger 2: Auditoría de la tabla Tarjeta (Creación, Cambio de Estado y Cambio de Número) ---
-- Se dispara después de un INSERT o UPDATE en la tabla Tarjeta.
PRINT 'Creando Trigger para auditar la tabla Tarjeta...';
GO
CREATE TRIGGER TRG_Tarjeta_AuditLifecycle
ON Tarjeta
AFTER INSERT, UPDATE
AS
BEGIN
    IF NOT EXISTS (SELECT * FROM inserted)
        RETURN;

    DECLARE @EmpleadoID INT = CAST(SESSION_CONTEXT(N'EmpleadoID') AS INT);

    -- Auditoría para la creación de la tarjeta (evento INSERT)
    IF EXISTS (SELECT * FROM inserted) AND NOT EXISTS (SELECT * FROM deleted)
    BEGIN
        INSERT INTO LogEstadoTarjeta (TarjetaID, EstadoAnteriorID, EstadoNuevoID, EmpleadoID, Justificacion)
        SELECT i.TarjetaID, NULL, i.EstadoTarjetaID, @EmpleadoID, 'Creación de nueva tarjeta'
        FROM inserted i;
    END

    -- Auditoría para cambios en la tarjeta (evento UPDATE)
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted)
    BEGIN
        -- Log para cambio de estado
        IF UPDATE(EstadoTarjetaID)
        BEGIN
            INSERT INTO LogEstadoTarjeta (TarjetaID, EstadoAnteriorID, EstadoNuevoID, EmpleadoID, Justificacion)
            SELECT i.TarjetaID, d.EstadoTarjetaID, i.EstadoTarjetaID, @EmpleadoID, 'Cambio de estado manual'
            FROM inserted i JOIN deleted d ON i.TarjetaID = d.TarjetaID
            WHERE d.EstadoTarjetaID <> i.EstadoTarjetaID;
        END

        -- Log para cambio de número de tarjeta
        IF UPDATE(NumeroTarjeta)
        BEGIN
            INSERT INTO LogCambioNumeroTarjeta (TarjetaID, EmpleadoID, NumeroAnterior, NumeroNuevo, Justificacion)
            SELECT i.TarjetaID, @EmpleadoID, d.NumeroTarjeta, i.NumeroTarjeta, 'Modificación de número de tarjeta'
            FROM inserted i JOIN deleted d ON i.TarjetaID = d.TarjetaID
            WHERE d.NumeroTarjeta <> i.NumeroTarjeta;
        END
    END
END
GO


-- --- Trigger 3: Auditoría de Cambios en Límite de Retiro en la tabla Cuenta ---
-- Se dispara después de un UPDATE en la tabla Cuenta.
PRINT 'Creando Trigger para auditar la tabla Cuenta...';
GO
CREATE TRIGGER TRG_Cuenta_AuditLimitChange
ON Cuenta
AFTER UPDATE
AS
BEGIN
    IF NOT EXISTS (SELECT * FROM inserted) AND NOT EXISTS (SELECT * FROM deleted)
        RETURN;

    -- Solo nos interesa si la columna del límite cambió
    IF UPDATE(MontoMaximoRetiroDiario)
    BEGIN
        DECLARE @EmpleadoID INT = CAST(SESSION_CONTEXT(N'EmpleadoID') AS INT);

        IF @EmpleadoID IS NULL
        BEGIN
            THROW 50002, 'No se puede modificar un límite sin un EmpleadoID en el contexto de la sesión.', 1;
            ROLLBACK TRANSACTION;
            RETURN;
        END

        INSERT INTO LogCambioLimiteRetiro (CuentaID, EmpleadoID, LimiteAnterior, LimiteNuevo, Justificacion)
        SELECT i.CuentaID, @EmpleadoID, d.MontoMaximoRetiroDiario, i.MontoMaximoRetiroDiario, 'Cambio de límite manual'
        FROM inserted i JOIN deleted d ON i.CuentaID = d.CuentaID
        WHERE d.MontoMaximoRetiroDiario <> i.MontoMaximoRetiroDiario;
    END
END
GO

PRINT '¡Todos los triggers han sido creados exitosamente!';