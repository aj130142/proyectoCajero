-- =================================================================================
-- Script de Creación de Base de Datos para el Proyecto de Cajero Automático
-- Diseñado por: Gemini y el Usuario
-- Fecha: 2025-11-01
-- Motor de BD: SQL Server
-- =================================================================================

--Crear la base de datos y usarla
CREATE DATABASE CajeroDB;
GO

USE CajeroDB;
GO

-- *********************************************************************************
-- SECCIÓN 1: TABLAS DE CATÁLOGO
-- Estas tablas contienen datos de soporte que raramente cambian.
-- Se crean primero porque otras tablas dependen de ellas.
-- *********************************************************************************

-- Define los roles de los empleados (Administrador, Soporte, etc.)
CREATE TABLE Rol (
    RolID INT PRIMARY KEY IDENTITY(1,1),
    NombreRol NVARCHAR(50) NOT NULL UNIQUE,
    DescripcionRol NVARCHAR(255) NULL,
    Activo BIT NOT NULL DEFAULT (1)
);

-- Define los tipos de cuenta (Ahorros, Corriente, etc.)
CREATE TABLE TipoCuenta (
    TipoCuentaID TINYINT PRIMARY KEY,
    Nombre NVARCHAR(50) NOT NULL UNIQUE
);

-- Define los estados de una cuenta (Activa, Bloqueada, etc.)
CREATE TABLE EstadoCuenta (
    EstadoCuentaID TINYINT PRIMARY KEY,
    Nombre NVARCHAR(50) NOT NULL UNIQUE,
    Descripcion NVARCHAR(255) NULL
);

-- Define los estados de una tarjeta (Activa, Vencida, Robada, etc.)
CREATE TABLE EstadoTarjeta (
    EstadoTarjetaID TINYINT PRIMARY KEY,
    Nombre NVARCHAR(50) NOT NULL UNIQUE,
    Descripcion NVARCHAR(255) NULL
);

-- Define los estados operativos de un cajero (En Servicio, Mantenimiento, etc.)
CREATE TABLE EstadoCajero (
    EstadoCajeroID TINYINT PRIMARY KEY,
    Nombre NVARCHAR(50) NOT NULL UNIQUE
);

-- Define los billetes y monedas que maneja el sistema
CREATE TABLE Denominacion (
    DenominacionID INT PRIMARY KEY IDENTITY(1,1),
    Valor DECIMAL(18, 2) NOT NULL,
    Moneda VARCHAR(3) NOT NULL,
    Tipo VARCHAR(10) NOT NULL -- 'Billete' o 'Moneda'
);

-- Define los tipos de transacciones (Retiro, Depósito, etc.)
CREATE TABLE TipoTransaccion (
    TipoTransaccionID TINYINT PRIMARY KEY,
    Nombre NVARCHAR(50) NOT NULL UNIQUE,
    Codigo VARCHAR(10) NOT NULL UNIQUE,
    AfectaSaldo BIT NOT NULL
);

-- Define los motivos por los que un inicio de sesión puede fallar
CREATE TABLE MotivoFalloLogin (
    MotivoFalloID TINYINT PRIMARY KEY,
    Codigo VARCHAR(20) NOT NULL UNIQUE,
    Descripcion NVARCHAR(255) NOT NULL
);

-- Define los tipos de movimientos de efectivo (Inicialización, Recarga)
CREATE TABLE TipoMovimientoEfectivo (
    TipoMovimientoID TINYINT PRIMARY KEY,
    Nombre NVARCHAR(50) NOT NULL UNIQUE
);


-- *********************************************************************************
-- SECCIÓN 2: TABLAS DE ENTIDADES PRINCIPALES
-- Estas son las tablas centrales del sistema.
-- *********************************************************************************

-- Almacena los datos personales de los clientes finales
CREATE TABLE Usuario (
    UsuarioID INT PRIMARY KEY IDENTITY(1,1),
    Nombres NVARCHAR(100) NOT NULL,
    Apellidos NVARCHAR(100) NOT NULL,
    DPI VARCHAR(13) NOT NULL UNIQUE CHECK (LEN(DPI) = 13),
    CorreoElectronico NVARCHAR(254) NOT NULL UNIQUE,
    TelefonoCelular VARCHAR(20) NOT NULL UNIQUE,
    FechaCreacion DATETIME2(7) NOT NULL DEFAULT GETDATE(),
    Activo BIT NOT NULL DEFAULT (1),
    FechaModificacion DATETIME2(7) NULL
);

-- Almacena los datos del personal interno de la institución
CREATE TABLE Empleado (
    EmpleadoID INT PRIMARY KEY IDENTITY(1,1),
    NombreUsuario NVARCHAR(50) NOT NULL UNIQUE,
    HashContraseña NVARCHAR(256) NOT NULL,
    Nombres NVARCHAR(100) NOT NULL,
    Apellidos NVARCHAR(100) NOT NULL,
    CorreoInstitucional NVARCHAR(254) NOT NULL UNIQUE,
    RolID INT NOT NULL,
    Activo BIT NOT NULL DEFAULT (1),
    FechaCreacion DATETIME2(7) NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Empleado_Rol FOREIGN KEY (RolID) REFERENCES Rol(RolID)
);

-- Contiene la información de las cuentas bancarias de los usuarios
CREATE TABLE Cuenta (
    CuentaID INT PRIMARY KEY IDENTITY(1,1),
    NumeroCuenta VARCHAR(20) NOT NULL UNIQUE,
    UsuarioID INT NOT NULL,
    TipoCuentaID TINYINT NOT NULL,
    SaldoActual DECIMAL(18, 2) NOT NULL CHECK (SaldoActual >= 0),
    MontoMaximoRetiroDiario DECIMAL(18, 2) NOT NULL DEFAULT(5000.00),
    EstadoCuentaID TINYINT NOT NULL,
    FechaCreacion DATETIME2(7) NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Cuenta_Usuario FOREIGN KEY (UsuarioID) REFERENCES Usuario(UsuarioID),
    CONSTRAINT FK_Cuenta_TipoCuenta FOREIGN KEY (TipoCuentaID) REFERENCES TipoCuenta(TipoCuentaID),
    CONSTRAINT FK_Cuenta_EstadoCuenta FOREIGN KEY (EstadoCuentaID) REFERENCES EstadoCuenta(EstadoCuentaID)
);

-- Almacena los datos de las tarjetas de débito
CREATE TABLE Tarjeta (
    TarjetaID INT PRIMARY KEY IDENTITY(1,1),
    CuentaID INT NOT NULL,
    NumeroTarjeta VARCHAR(19) NOT NULL UNIQUE,
    PinHash NVARCHAR(256) NOT NULL,
    CVVHash NVARCHAR(256) NOT NULL,
    FechaAsignacion DATETIME2(7) NOT NULL DEFAULT GETDATE(),
    FechaExpiracion DATE NOT NULL,
    EstadoTarjetaID TINYINT NOT NULL,
    CONSTRAINT FK_Tarjeta_Cuenta FOREIGN KEY (CuentaID) REFERENCES Cuenta(CuentaID),
    CONSTRAINT FK_Tarjeta_EstadoTarjeta FOREIGN KEY (EstadoTarjetaID) REFERENCES EstadoTarjeta(EstadoTarjetaID)
);

-- Almacena la información de identificación y ubicación de cada cajero
CREATE TABLE Cajero (
    CajeroID INT PRIMARY KEY IDENTITY(1,1),
    Ubicacion NVARCHAR(255) NOT NULL UNIQUE,
    NumeroSerie VARCHAR(50) NOT NULL UNIQUE,
    DireccionIP VARCHAR(45) NULL UNIQUE,
    EstadoCajeroID TINYINT NOT NULL,
    FechaInstalacion DATE NULL,
    CONSTRAINT FK_Cajero_EstadoCajero FOREIGN KEY (EstadoCajeroID) REFERENCES EstadoCajero(EstadoCajeroID)
);

-- El desglose de billetes y cantidades dentro de cada cajero
CREATE TABLE InventarioEfectivo (
    CajeroID INT NOT NULL,
    DenominacionID INT NOT NULL,
    Cantidad INT NOT NULL CHECK (Cantidad >= 0),
    PRIMARY KEY (CajeroID, DenominacionID), -- Clave Primaria Compuesta
    CONSTRAINT FK_InventarioEfectivo_Cajero FOREIGN KEY (CajeroID) REFERENCES Cajero(CajeroID),
    CONSTRAINT FK_InventarioEfectivo_Denominacion FOREIGN KEY (DenominacionID) REFERENCES Denominacion(DenominacionID)
);

-- Registra cada retiro o depósito realizado
CREATE TABLE Transaccion (
    TransaccionID BIGINT PRIMARY KEY IDENTITY(1,1),
    CuentaID INT NOT NULL,
    TarjetaID INT NOT NULL,
    TipoTransaccionID TINYINT NOT NULL,
    Monto DECIMAL(18, 2) NOT NULL,
    FechaHora DATETIME2(7) NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Transaccion_Cuenta FOREIGN KEY (CuentaID) REFERENCES Cuenta(CuentaID),
    CONSTRAINT FK_Transaccion_Tarjeta FOREIGN KEY (TarjetaID) REFERENCES Tarjeta(TarjetaID),
    CONSTRAINT FK_Transaccion_TipoTransaccion FOREIGN KEY (TipoTransaccionID) REFERENCES TipoTransaccion(TipoTransaccionID)
);

-- Gestiona los tokens de un solo uso para la autenticación de 2 factores
CREATE TABLE TokenOTP (
    TokenID BIGINT PRIMARY KEY IDENTITY(1,1),
    UsuarioID INT NOT NULL,
    CodigoTokenHash NVARCHAR(256) NOT NULL,
    FechaCreacion DATETIME2(7) NOT NULL DEFAULT GETDATE(),
    FechaExpiracion DATETIME2(7) NOT NULL,
    Utilizado BIT NOT NULL DEFAULT (0),
    CONSTRAINT FK_TokenOTP_Usuario FOREIGN KEY (UsuarioID) REFERENCES Usuario(UsuarioID)
);


-- *********************************************************************************
-- SECCIÓN 3: TABLAS DE AUDITORÍA Y LOGS
-- Estas tablas registran todos los eventos importantes del sistema.
-- *********************************************************************************

-- Registra la creación de nuevos clientes
CREATE TABLE LogCreacionUsuario (
    LogID INT PRIMARY KEY IDENTITY(1,1),
    UsuarioCreadoID INT NOT NULL UNIQUE,
    AdminID INT NOT NULL,
    FechaCreacion DATETIME2(7) NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_LogCreacionUsuario_Usuario FOREIGN KEY (UsuarioCreadoID) REFERENCES Usuario(UsuarioID),
    CONSTRAINT FK_LogCreacionUsuario_Empleado FOREIGN KEY (AdminID) REFERENCES Empleado(EmpleadoID)
);

-- Registra cambios en los datos de los clientes
CREATE TABLE LogActualizacionUsuario (
    LogID BIGINT PRIMARY KEY IDENTITY(1,1),
    UsuarioModificadoID INT NOT NULL,
    AdminID INT NOT NULL,
    FechaHoraCambio DATETIME2(7) NOT NULL DEFAULT GETDATE(),
    CampoModificado NVARCHAR(100) NOT NULL,
    ValorAntiguo NVARCHAR(MAX) NULL,
    ValorNuevo NVARCHAR(MAX) NULL,
    CONSTRAINT FK_LogActualizacionUsuario_Usuario FOREIGN KEY (UsuarioModificadoID) REFERENCES Usuario(UsuarioID),
    CONSTRAINT FK_LogActualizacionUsuario_Empleado FOREIGN KEY (AdminID) REFERENCES Empleado(EmpleadoID)
);

-- Registra cambios en el límite de retiro de una cuenta
CREATE TABLE LogCambioLimiteRetiro (
    LogID BIGINT PRIMARY KEY IDENTITY(1,1),
    CuentaID INT NOT NULL,
    EmpleadoID INT NOT NULL,
    FechaHoraCambio DATETIME2(7) NOT NULL DEFAULT GETDATE(),
    LimiteAnterior DECIMAL(18, 2) NOT NULL,
    LimiteNuevo DECIMAL(18, 2) NOT NULL,
    Justificacion NVARCHAR(500) NULL,
    CONSTRAINT FK_LogCambioLimiteRetiro_Cuenta FOREIGN KEY (CuentaID) REFERENCES Cuenta(CuentaID),
    CONSTRAINT FK_LogCambioLimiteRetiro_Empleado FOREIGN KEY (EmpleadoID) REFERENCES Empleado(EmpleadoID)
);

-- Registra el evento de cambio de PIN
CREATE TABLE LogCambioPin (
    LogID BIGINT PRIMARY KEY IDENTITY(1,1),
    TarjetaID INT NOT NULL,
    FechaHoraCambio DATETIME2(7) NOT NULL DEFAULT GETDATE(),
    FuenteCambio NVARCHAR(50) NOT NULL,
    UsuarioID INT NULL,
    EmpleadoID INT NULL,
    CONSTRAINT FK_LogCambioPin_Tarjeta FOREIGN KEY (TarjetaID) REFERENCES Tarjeta(TarjetaID),
    CONSTRAINT FK_LogCambioPin_Usuario FOREIGN KEY (UsuarioID) REFERENCES Usuario(UsuarioID),
    CONSTRAINT FK_LogCambioPin_Empleado FOREIGN KEY (EmpleadoID) REFERENCES Empleado(EmpleadoID)
);

-- Registra el ciclo de vida de una tarjeta (creación, bloqueo, etc.)
CREATE TABLE LogEstadoTarjeta (
    LogID BIGINT PRIMARY KEY IDENTITY(1,1),
    TarjetaID INT NOT NULL,
    FechaHoraCambio DATETIME2(7) NOT NULL DEFAULT GETDATE(),
    EstadoAnteriorID TINYINT NULL,
    EstadoNuevoID TINYINT NOT NULL,
    EmpleadoID INT NULL,
    Justificacion NVARCHAR(500) NULL,
    CONSTRAINT FK_LogEstadoTarjeta_Tarjeta FOREIGN KEY (TarjetaID) REFERENCES Tarjeta(TarjetaID),
    CONSTRAINT FK_LogEstadoTarjeta_EstadoAnterior FOREIGN KEY (EstadoAnteriorID) REFERENCES EstadoTarjeta(EstadoTarjetaID),
    CONSTRAINT FK_LogEstadoTarjeta_EstadoNuevo FOREIGN KEY (EstadoNuevoID) REFERENCES EstadoTarjeta(EstadoTarjetaID),
    CONSTRAINT FK_LogEstadoTarjeta_Empleado FOREIGN KEY (EmpleadoID) REFERENCES Empleado(EmpleadoID)
);

-- Registra el cambio del número de una tarjeta
CREATE TABLE LogCambioNumeroTarjeta (
    LogID BIGINT PRIMARY KEY IDENTITY(1,1),
    TarjetaID INT NOT NULL,
    EmpleadoID INT NOT NULL,
    FechaHoraCambio DATETIME2(7) NOT NULL DEFAULT GETDATE(),
    NumeroAnterior VARCHAR(19) NOT NULL,
    NumeroNuevo VARCHAR(19) NOT NULL,
    Justificacion NVARCHAR(500) NULL,
    CONSTRAINT FK_LogCambioNumeroTarjeta_Tarjeta FOREIGN KEY (TarjetaID) REFERENCES Tarjeta(TarjetaID),
    CONSTRAINT FK_LogCambioNumeroTarjeta_Empleado FOREIGN KEY (EmpleadoID) REFERENCES Empleado(EmpleadoID)
);

-- Registra todos los intentos de inicio de sesión (exitosos y fallidos)
CREATE TABLE LogInicioSesion (
    LogID BIGINT PRIMARY KEY IDENTITY(1,1),
    NumeroTarjetaIngresado VARCHAR(19) NOT NULL,
    FechaHoraIntento DATETIME2(7) NOT NULL DEFAULT GETDATE(),
    Exitoso BIT NOT NULL,
    CajeroID INT NOT NULL,
    TarjetaID INT NULL,
    MotivoFalloID TINYINT NULL,
    CONSTRAINT FK_LogInicioSesion_Cajero FOREIGN KEY (CajeroID) REFERENCES Cajero(CajeroID),
    CONSTRAINT FK_LogInicioSesion_Tarjeta FOREIGN KEY (TarjetaID) REFERENCES Tarjeta(TarjetaID),
    CONSTRAINT FK_LogInicioSesion_MotivoFallo FOREIGN KEY (MotivoFalloID) REFERENCES MotivoFalloLogin(MotivoFalloID)
);

-- Registra el "encabezado" de una carga de efectivo (quién, cuándo, dónde)
CREATE TABLE LogMovimientoEfectivo (
    LogID BIGINT PRIMARY KEY IDENTITY(1,1),
    CajeroID INT NOT NULL,
    EmpleadoID INT NOT NULL,
    TipoMovimientoID TINYINT NOT NULL,
    FechaHora DATETIME2(7) NOT NULL DEFAULT GETDATE(),
    Justificacion NVARCHAR(500) NULL,
    CONSTRAINT FK_LogMovimientoEfectivo_Cajero FOREIGN KEY (CajeroID) REFERENCES Cajero(CajeroID),
    CONSTRAINT FK_LogMovimientoEfectivo_Empleado FOREIGN KEY (EmpleadoID) REFERENCES Empleado(EmpleadoID),
    CONSTRAINT FK_LogMovimientoEfectivo_Tipo FOREIGN KEY (TipoMovimientoID) REFERENCES TipoMovimientoEfectivo(TipoMovimientoID)
);

-- Registra el "detalle" de billetes en una carga de efectivo
CREATE TABLE LogMovimientoEfectivoDetalle (
    DetalleID BIGINT PRIMARY KEY IDENTITY(1,1),
    LogID BIGINT NOT NULL,
    DenominacionID INT NOT NULL,
    Cantidad INT NOT NULL CHECK (Cantidad > 0),
    CONSTRAINT FK_LogMovimientoDetalle_Log FOREIGN KEY (LogID) REFERENCES LogMovimientoEfectivo(LogID),
    CONSTRAINT FK_LogMovimientoDetalle_Denominacion FOREIGN KEY (DenominacionID) REFERENCES Denominacion(DenominacionID)
);

PRINT '¡La base de datos y todas las tablas han sido creadas exitosamente!';