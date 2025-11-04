# Proyecto Cajero Automático - Documentación de Arquitectura

## 1. Descripción General del Proyecto

Este documento detalla la arquitectura de la base de datos para un sistema de Cajero Automático diseñado para una empresa que desea proveer servicios de efectivo a sus empleados. El sistema se divide en dos áreas funcionales principales:

*   **Área Administrativa:** Para el personal interno (empleados del banco/institución) con permisos para gestionar usuarios, cuentas, tarjetas y el efectivo del cajero.
*   **Área de Usuarios:** Para los clientes finales (empleados de la empresa cliente) que utilizarán el cajero para realizar transacciones como retiros, depósitos y consultas.

Aunque el requisito inicial del proyecto mencionaba el "almacenamiento de información en archivos", se ha optado por una arquitectura de base de datos relacional (SQL Server) para garantizar la integridad de los datos, la seguridad, el rendimiento y la escalabilidad, superando con creces las capacidades de un sistema basado en archivos de texto.

## 2. Principios de Diseño y Arquitectura

La base de datos se ha diseñado siguiendo principios estándar de la industria para crear un sistema robusto, seguro y mantenible.

*   **Separación de Entidades:** Se distingue claramente entre un **`Usuario`** (cliente final) y un **`Empleado`** (personal interno), cada uno con su propia tabla y atributos, evitando la mezcla de dominios de negocio.
*   **Control de Acceso Basado en Roles (RBAC):** Los permisos para los empleados se gestionan a través de una tabla `Rol`, permitiendo una administración flexible y escalable de los niveles de acceso.
*   **Normalización (3NF):** Se utilizan **Tablas de Catálogo** (`EstadoCuenta`, `TipoTransaccion`, `Denominacion`, etc.) para minimizar la redudancia de datos, garantizar la consistencia y mejorar la eficiencia del almacenamiento.
*   **Seguridad Primero:**
    *   **Hashing de Credenciales:** Las contraseñas de los empleados y los PINs de las tarjetas de los usuarios **nunca** se almacenan en texto plano. Se utiliza un hash criptográfico (ej. SHA-256) para su almacenamiento y verificación.
    *   **Autenticación de Dos Factores (2FA):** El inicio de sesión de los usuarios requiere un token de un solo uso (OTP) gestionado por la tabla `TokenOTP` y enviado vía SMS, añadiendo una capa crítica de seguridad.
*   **Auditoría Completa e Inmutable:** Se utiliza un sistema exhaustivo de **Tablas de Log** para registrar cada evento importante (creación de usuarios, cambios de estado, modificaciones de límites, inicios de sesión, etc.).
*   **Automatización con Triggers:** La mayoría de los registros de auditoría se generan automáticamente a través de **Triggers** de la base de datos, garantizando que ninguna acción importante quede sin registrar y separando la lógica de auditoría de la lógica de negocio de la aplicación.
*   **Rendimiento Optimizado:** Se distingue entre datos de estado actual (almacenados directamente en las entidades principales para acceso rápido) y datos históricos (almacenados en tablas de log para consultas de auditoría).

## 3. Esquema de la Base de Datos (26 Tablas)

El esquema se divide en tres categorías funcionales: Entidades Principales, Tablas de Catálogo y Tablas de Auditoría.

### A. Entidades Principales (8 tablas)

| Tabla | Descripción |
| :--- | :--- |
| `Usuario` | Almacena los datos personales de los clientes finales (dueños de las cuentas). |
| `Empleado` | Almacena los datos del personal interno que administra el sistema. Incluye un `RolID`. |
| `Cuenta` | Contiene la información de las cuentas bancarias (saldo, límite de retiro diario, etc.). |
| `Tarjeta` | Almacena los datos de las tarjetas de débito, incluyendo el `PinHash` y su vínculo a una `Cuenta`. |
| `Cajero` | Representa la entidad física del cajero, su ubicación y estado operativo. |
| `Transaccion`| Registra cada operación financiera (retiro, depósito) vinculada a una `Cuenta`. |
| `InventarioEfectivo`| Tabla de unión que detalla la cantidad de cada `Denominacion` en cada `Cajero`. |
| `TokenOTP` | Gestiona los tokens de un solo uso para la autenticación de dos factores. |

### B. Tablas de Catálogo (9 tablas)

| Tabla | Descripción |
| :--- | :--- |
| `Rol` | Define los roles de los `Empleados` (ej. "Administrador de Cajeros"). |
| `TipoCuenta` | Define los tipos de `Cuenta` (ej. "Ahorros", "Monetaria"). |
| `EstadoCuenta` | Define los estados de una `Cuenta` (ej. "Activa", "Bloqueada"). |
| `EstadoTarjeta` | Define los estados de una `Tarjeta` (ej. "Activa", "Vencida", "Reportada como robada"). |
| `EstadoCajero` | Define los estados operativos de un `Cajero` (ej. "En Servicio", "Bajo Mantenimiento"). |
| `Denominacion` | Define los billetes y monedas que el sistema maneja (ej. Q.200, Q.100). |
| `TipoTransaccion`| Define si una `Transaccion` es un "Retiro", "Depósito", etc. |
| `MotivoFalloLogin`| Define las razones por las que un inicio de sesión puede fallar. |
| `TipoMovimientoEfectivo`| Define si una carga de efectivo es una "Inicialización" o una "Recarga". |

### C. Tablas de Auditoría y Logs (9 tablas)

| Tabla | Descripción |
| :--- | :--- |
| `LogCreacionUsuario` | Registra la creación de nuevos clientes. |
| `LogActualizacionUsuario`| Registra cambios en los datos de los clientes. |
| `LogCambioLimiteRetiro`| Registra cambios en el límite de retiro de una cuenta. |
| `LogCambioPin` | Registra el evento de cambio de PIN de una tarjeta. |
| `LogEstadoTarjeta` | Registra el ciclo de vida de una tarjeta (creación, bloqueo, etc.). |
| `LogCambioNumeroTarjeta`| Registra el cambio del número de una tarjeta. |
| `LogInicioSesion` | Registra todos los intentos de inicio de sesión, tanto exitosos como fallidos. |
| `LogMovimientoEfectivo`| Registra el "encabezado" de una carga de efectivo (quién, cuándo, dónde). |
| `LogMovimientoEfectivoDetalle`| Registra el desglose de billetes de una carga de efectivo. |

## 4. Procesos de Negocio Clave

### A. Autenticación de Usuario (2FA)

El inicio de sesión de un cliente es un proceso de dos etapas:
1.  **Etapa 1 (Credenciales):** El usuario ingresa su número de tarjeta y PIN. El sistema verifica el `PinHash` en la tabla `Tarjeta`.
2.  **Etapa 2 (Token):** Si la Etapa 1 es exitosa, el sistema:
    a. Genera un código OTP de 6 dígitos.
    b. Guarda el hash de este código en la tabla `TokenOTP` con una fecha de expiración corta (ej. 5 minutos).
    c. Envía el código al `TelefonoCelular` del usuario a través de un servicio de SMS Gateway (ej. Twilio).
    d. El usuario ingresa el código, y el sistema lo verifica contra la tabla `TokenOTP`, asegurándose de que no haya expirado ni sido utilizado.
    e. El token se marca como `Utilizado` para prevenir su re-utilización.

### B. Gestión del Límite de Retiro Diario

El requisito de que el límite diario se mantenga aunque el número de tarjeta cambie se resuelve de la siguiente manera:
*   El límite de retiro (`MontoMaximoRetiroDiario`) es una propiedad de la **`Cuenta`**, no de la `Tarjeta`.
*   Todas las transacciones de retiro se registran en la tabla `Transaccion` con una referencia a la **`CuentaID`**.
*   Para verificar el límite, el sistema suma todos los retiros del día para una `CuentaID` específica, sin importar qué `TarjetaID` o `NumeroTarjeta` se utilizó.

### C. Auditoría Automática con Triggers

Para garantizar que los logs sean completos y fiables, se utilizan **Triggers** de base de datos para las operaciones más críticas:
*   Un trigger en la tabla `Usuario` registra automáticamente cualquier cambio en `LogActualizacionUsuario`.
*   Un trigger en la tabla `Tarjeta` registra la creación y los cambios de estado en `LogEstadoTarjeta`, y los cambios de número en `LogCambioNumeroTarjeta`.
*   Un trigger en la tabla `Cuenta` registra los cambios de límite en `LogCambioLimiteRetiro`.

Para que los triggers sepan qué empleado está realizando el cambio, la aplicación C# debe establecer el `EmpleadoID` en el contexto de la sesión (`sp_set_session_context`) antes de ejecutar cualquier comando de modificación de datos.

## 5. Configuración Inicial del Entorno

Para desplegar y empezar a usar la base de datos, se deben seguir los siguientes pasos:

1.  **Crear la Base de Datos:** En una instancia de SQL Server, crear una nueva base de datos (ej. `CajeroDB`).
2.  **Ejecutar el Script de Creación de Tablas:** Correr el script `CREATE_TABLES.sql` que genera las 26 tablas con todas sus relaciones y restricciones.
3.  **Poblar las Tablas de Catálogo:** Correr el script `INSERT_CATALOGS.sql` para insertar los datos iniciales en las 9 tablas de catálogo.
4.  **Crear el Primer Empleado Administrador:** Correr el script `INSERT_ADMIN.sql` para registrar al primer empleado con rol de administrador, necesario para operar el sistema.
5.  **Crear los Triggers:** Correr el script `CREATE_TRIGGERS.sql` para establecer la lógica de auditoría automática en la base de datos.

Una vez completados estos pasos, la base de datos estará lista para que la aplicación en C# se conecte a ella y comience a operar.