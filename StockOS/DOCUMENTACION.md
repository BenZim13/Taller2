# StockOS — arquitectura y funcionamiento

> Documento generado a partir del código actual de la solución `StockOS` (estado revisado en la rama `main`). Describe la implementación existente, no una arquitectura propuesta.

## 1. Propósito

StockOS es una aplicación de escritorio para la gestión de un comercio: autenticación de empleados, roles y permisos, productos y categorías, proveedores, compras, stock por sucursal, ventas, caja, reportes PDF/gráficos y copias de seguridad de SQL Server.

La aplicación se ejecuta como WinForms sobre .NET 8. La persistencia principal usa Entity Framework Core y SQL Server; algunas operaciones críticas y consultas de reportes llaman procedimientos almacenados o SQL directamente.

## 2. Solución y proyectos

```text
StockOS.sln
├── src/StockOS.Domain              (.NET 8)
├── src/StockOS.Application         (.NET 8)
├── src/StockOS.Infrastructure      (.NET 8)
├── src/StockOS.UI.WinForms         (.NET 8 Windows Forms)
└── tests/StockOS.Application.Tests (.NET 8 Windows Forms / xUnit)
```

### Dependencias entre proyectos

```text
UI.WinForms ─────► Application ─────► Domain
	 │                  │
	 └──────────────► Infrastructure ─► Domain

Application.Tests ───► Application, Domain, UI.WinForms
```

- **Domain** contiene entidades de negocio generadas/modeladas para EF Core, DTOs, enumeraciones y contratos.
- **Application** contiene servicios de aplicación, validaciones de negocio, autorización, generación de tickets y reportes.
- **Infrastructure** contiene `StockOsContext`, repositorios EF/SQL y el servicio de backup.
- **UI.WinForms** contiene el punto de entrada, formularios, controles de usuario, validación de interfaz y composición del contenedor DI.
- **Tests** verifica principalmente los servicios de Application, los validadores, el backup y ciertos flujos de UI.

## 3. Arranque y composición de dependencias

El punto de entrada es `src/StockOS.UI.WinForms/Program.cs`, clase `Program`, método `Main`:

1. Inicializa WinForms mediante `ApplicationConfiguration.Initialize()`.
2. Configura QuestPDF con licencia Community y fuentes del sistema.
3. Configura Serilog para escribir en `logs/stockos-<fecha>.txt`.
4. Registra manejadores globales para `Application.ThreadException` y `AppDomain.UnhandledException`.
5. Lee `appsettings.json` desde el directorio de salida.
6. Construye un `Host` de Microsoft.Extensions.Hosting.
7. Registra `StockOsContext` con `UseSqlServer` usando la cadena `ConnectionStrings:StockOS`.
8. Registra repositorios y servicios como **scoped**; los formularios y controles como **transient**.
9. Crea un scope por ciclo de autenticación. Muestra `FormLogin`; si autentica, muestra `FormInicio` y ejecuta el bucle de mensajes de WinForms.
10. Al salir, intenta cerrar automáticamente la caja, limpia `SesionActual` y decide si vuelve al login o termina.

La cadena actual es:

```json
Server=localhost;Database=StockOS;Trusted_Connection=True;TrustServerCertificate=True;
```

`appsettings.json` también suministra `DatosComercio`, utilizados para tickets y reportes: nombre, CUIT, domicilio, condición frente al IVA, punto de venta y registro fiscal.

## 4. Comunicación entre capas

El flujo normal es:

```text
Form/UserControl
	│ llama a una interfaz de servicio (Application)
	▼
Servicio de aplicación
	│ valida datos, permiso y reglas de negocio
	│ llama a una interfaz de repositorio (Domain)
	▼
Repositorio (Infrastructure)
	│ usa StockOsContext, LINQ/EF Core, SQL o procedimientos almacenados
	▼
SQL Server
```

La UI no debería acceder directamente a `DbContext`; recibe servicios por constructor mediante DI. Application depende de interfaces de Domain, por lo que Infrastructure puede proporcionar las implementaciones concretas. En `Program` se conectan ambos lados, por ejemplo `IProductoService -> ProductoService` y `IProductoRepository -> ProductoRepository`.

En operaciones como una venta, `VentaService` valida autorización y datos; `VentaRepository` abre una transacción, inserta la cabecera, inserta cada detalle, descuenta stock y registra el pago. Si falla una parte, hace rollback.

La excepción más visible al patrón es `SesionActual`: es un estado estático global usado por servicios y UI para saber el empleado, sucursal y sesión de caja actuales.

## 5. Capa Domain

### Entidades

Las entidades son clases `partial` con propiedades escalares y propiedades de navegación EF Core. No contienen, salvo el cálculo indicado, casos de uso completos.

| Clase | Función y miembros relevantes |
|---|---|
| `Caja` | Caja física. `IdCaja`, `NombreNumero`, `IdSucursal`, `Activo`, navegación a `Sucursal` y sesiones. |
| `CajaSesion` | Turno de caja abierto/cerrado. Fechas, montos, estado, caja, empleado, movimientos y ventas. |
| `Categoria` | Categoría de productos: nombre, descripción, estado y colección de productos. |
| `Cliente` | Cliente de una venta: razón social, CUIT/DNI, IVA, contacto, domicilio y estado. |
| `Compra` | Cabecera de compra: fecha, total, comprobante, proveedor, empleado, sucursal y detalles. |
| `DetalleCompra` | Producto comprado, cantidad, precio unitario y relación con la compra. |
| `DetalleVenta` | Producto vendido, cantidad, precio histórico y descuento. |
| `Empleado` | Usuario del sistema: identidad, DNI, correo, `PasswordHash`, estado, rol y sucursal. |
| `Factura` | Comprobante de una venta: tipo/número, CAE, fecha, neto, IVA y total. |
| `MetodoPago` | Catálogo de medios de pago y pagos relacionados. |
| `MovimientoCaja` | Entrada/salida de dinero asociada a una sesión, con tipo, importe y descripción. |
| `Pago` | Importe y referencia de un pago, asociado a venta y método. |
| `Producto` | Producto comercial: código de barras, nombre, precios, IVA, categoría, proveedor y estado. `CalcularPrecioVentaFinal(precioCompra, margenGanancia, porcentajeIva)` calcula el precio aplicando margen e IVA. |
| `Proveedor` | Datos del proveedor y sus compras/productos. |
| `Rol` | Nombre/descripción del rol y empleados asociados. |
| `StockSucursal` | Existencia y stock mínimo de un producto en una sucursal. |
| `Sucursal` | Sucursal, dirección, estado y cajas/compras/empleados/stock relacionados. |
| `Venta` | Cabecera de venta: fecha, subtotal, descuento, total, estado, sesión de caja, cliente, detalles, factura y pagos. |

### DTOs

- `ReporteItemDTO`: código de barras, nombre, cantidad total y subtotal de un producto en reportes.
- `ResumenFinancieroDTO`: ingresos brutos, descuentos, ingresos netos y egresos brutos.
- `VentasPorFechaDTO`: fecha y total vendido para gráficos de línea.

### Interfaces y contratos

Las interfaces de `Domain/Interfaces` separan contratos de implementación:

- Repositorios: `IEmpleadoRepository`, `ISucursalRepository`, `IRolRepository`, `ICategoriaRepository`, `IProductoRepository`, `IStockSucursalRepository`, `ICajaSesionRepository`, `ICajaRepository`, `IVentaRepository`, `ICompraRepository`, `IProveedorRepository` e `IReporteRepository`.
- Servicios transversales: `IAuthorizationService`, `IBackupService` e `ITicketService`.
- `IBackupService` usa `ConfiguracionBackupPeriodico` (habilitado, periodo, hora, destino, última y próxima ejecución).
- `ITicketService` usa `DatosComercio` y `DatosPago`, DTOs de presentación para generar el comprobante.

Las interfaces de repositorio exponen, en general, consultas/listados y operaciones CRUD o transaccionales. Las interfaces de servicio de Application (`IAuthService`, `ICajaService`, `ICategoriaService`, `ICompraService`, `IConfiguracionService`, `IEmpleadoService`, `IProductoService`, `IProveedorService`, `IReporteService`, `IRolService`, `IStockService`, `ISucursalService`, `IVentaService`) representan los casos de uso consumidos por la UI.

### Enumeraciones y autorización

- `RolUsuario`: Gerente, Cajero, Encargado de Depósito y Repositor.
- `PeriodoBackup`: periodicidad del backup.
- `Permisos`: constantes como `PRODUCTOS_VER`, `STOCK_INGRESAR`, `VENTAS_REALIZAR`, `CAJA_ABRIR`, `CAJA_CERRAR`, `REPORTES_VER`, `COMPRAS_GESTIONAR`, `PROVEEDORES_GESTIONAR` y `CONFIGURACION_GESTIONAR`.

## 6. Capa Application

Los servicios reciben sus dependencias por constructor. Normalmente validan `Permisos`, verifican reglas y delegan la persistencia al repositorio.

| Clase | Métodos y responsabilidad |
|---|---|
| `AuthService` | `LoginAsync` busca por DNI, comprueba estado y BCrypt, y devuelve éxito/usuario/error; `RegistrarAsync` registra un empleado con contraseña protegida. |
| `AuthorizationService` | `TienePermiso` consulta la matriz fija de permisos del rol actual; `ValidarPermiso` lanza `UnauthorizedAccessException` si no existe permiso. |
| `SesionActual` | Propiedades estáticas `Usuario`, `IdCajaSesionAbierta` e `IdSucursal`; `Limpiar` restablece la sesión. |
| `EmpleadoService` | `ObtenerTodos`, `ObtenerPorId`, `CrearAsync`, `ActualizarAsync`, `EliminarAsync` y `CambiarEstadoAsync`; `ValidarReglasDeNegocio` centraliza validaciones de empleados. |
| `SucursalService` | `ObtenerSucursales` devuelve las sucursales del repositorio. |
| `RolService` | `ObtenerRoles` devuelve el catálogo de roles. |
| `CategoriaService` | `ObtenerTodos`, `Agregar`, `Actualizar`; `ValidarReglasDeNegocio` comprueba categoría. |
| `ProductoService` | `ObtenerTodos`, `BuscarPorCodigoBarra`, `Agregar`, `Actualizar`, `CambiarEstado`; `ValidarReglasDeNegocio` valida datos y relaciones del producto. |
| `ProveedorService` | `ObtenerTodos`, `Agregar`, `Actualizar` y `CambiarEstado` gestionan proveedores. |
| `StockService` | `AgregarStock` ingresa mercadería; `ObtenerCantidadActual` consulta existencias por producto/sucursal. |
| `CompraService` | `RegistrarCompra` registra cabecera/detalle; `ObtenerUltimoPrecioCompra` consulta historial; `ActualizarPrecioCompra` actualiza el precio de referencia. |
| `CajaService` | `AbrirCaja`, `VerificarCajaAbierta`, `ObtenerIdSesionAbierta`, `ObtenerMontoEsperado`, `CerrarCaja`, `RegistrarMovimiento`, `ObtenerCajasPorSucursal` y `CerrarCajaPorCierreSesion` coordinan la caja y sus turnos. |
| `VentaService` | `RegistrarVenta` exige permiso, valida cabecera, detalle, sucursal, medio de pago, cantidades y precios; luego delega al repositorio transaccional. |
| `ValidadorPago` | `Validar` comprueba medio, total y efectivo recibido; `Valido`/`Invalido` construyen resultados; `Redondear` normaliza importes. |
| `ReporteService` | `ObtenerResumenFinancieroAsync`, `ObtenerTopProductosVendidosAsync` y `ObtenerVentasPorLineaTiempoAsync` consultan reportes; `GenerarReporteVentasPdfAsync` y `GenerarReporteComprasPdfAsync` preparan PDFs usando `GenerarPdf`, `ComposeHeader`, `ComposeContent`, `ComposeTable` y `ComposeFooter`. |
| `ConfiguracionService` | `ObtenerDatosComercio` convierte la configuración a `DatosComercio`. |
| `TicketService` | `GenerarTicketPdf` crea el ticket con QuestPDF; `ComposeHeader` y `ComposeContent` construyen cabecera y líneas/pago. |

## 7. Capa Infrastructure

### `StockOsContext`

`StockOsContext : DbContext` contiene `DbSet` para cajas, sesiones, categorías, clientes, compras, detalles, empleados, facturas, métodos de pago, movimientos, pagos, productos, proveedores, roles, stock, sucursales y ventas. El constructor con `DbContextOptions` es el utilizado por DI. `OnModelCreating` configura tablas, nombres de columnas, claves, índices únicos, tipos decimales, valores por defecto, relaciones y reglas de borrado.

### Repositorios

- `EmpleadoRepository`: buscar por email, DNI o ID, consultar estado, listar, agregar, actualizar y eliminar empleados.
- `SucursalRepository`: `ObtenerTodas` lista sucursales.
- `RolRepository`: `ObtenerTodos` lista roles.
- `CategoriaRepository`: listar, agregar y actualizar categorías.
- `ProductoRepository`: buscar por código de barras, listar, agregar, actualizar y cambiar estado.
- `StockSucursalRepository`: consultar cantidad e ingresar mercadería.
- `ProveedorRepository`: listar, agregar, actualizar y cambiar estado.
- `CajaSesionRepository`: abrir caja, comprobar sesión por empleado, obtener sesión abierta y comprobar si una caja física está abierta.
- `CajaRepository`: obtener cajas por sucursal, calcular monto esperado, cerrar caja y registrar movimientos.
- `CompraRepository`: registrar compra, consultar último precio y actualizar precio de compra.
- `VentaRepository`: `RegistrarVenta` ejecuta la venta completa dentro de una transacción SQL/EF, incluidos detalles, descuento de stock y pago.
- `ReporteRepository`: `ObtenerTopProductosVendidosAsync`, `ObtenerVentasPorLineaTiempoAsync` y `ObtenerResumenFinancieroAsync`; `CreateConnection` crea la conexión SQL para las consultas de reportes.

### `BackupService`

Implementa `IBackupService` y aplica autorización administrativa. `ObtenerDirectorioBackupsPorDefecto` determina la carpeta; `CalcularProximaEjecucion` calcula la próxima fecha; `CrearBackupAsync` genera el `.bak`; `AplicarPoliticaRetencion` elimina backups antiguos; `GuardarProgramacionPeriodicaAsync`, `ObtenerProgramacionPeriodica`, `ActualizarRegistroUltimaEjecucion` y `VerificarYEjecutarBackupPendienteAsync` gestionan la programación; `RestaurarBackupAsync` restaura desde un archivo. Los métodos privados validan escritura, crean carpetas y pueden registrar una tarea en Windows Task Scheduler.

## 8. Capa UI WinForms

Los archivos `*.Designer.cs` y `*.resx` son código/recursos generados por el diseñador y no contienen lógica de negocio. Los formularios y controles reciben servicios por DI y reaccionan a eventos de botones, grids y campos.

### Formularios

- `FormLogin`: captura credenciales, llama a `IAuthService.LoginAsync` y expone el usuario autenticado al programa.
- `FormInicio`: ventana principal; `EstablecerUsuario` fija el usuario y navega entre controles; `LogoutRequested` comunica cierre de sesión. `RoundedPanel` es un panel con esquinas redondeadas.
- `FormAperturaCaja`: carga cajas, valida monto y llama a apertura de caja; también permite cancelar/salir.
- `FormCierreCaja`: confirma el importe real y cierra la sesión.
- `FormMovimientoCaja`: registra ingresos/egresos de la sesión.
- `FormCobro`: selecciona medio de pago, importe recibido y vuelto; `ConfirmarPago` valida y devuelve los datos del cobro.
- `FormIngresoStock`: valida producto, sucursal y cantidad e ingresa mercadería.
- `FormRegistroProducto`: alta/edición de productos; `PrepararParaEdicion` carga un producto y los eventos guardan o cancelan.
- `FormCategoria`: lista, crea, edita y da de baja categorías.
- `FormProveedor`: alta, edición y estado de proveedores.
- `FormRegistroUsuario`: alta/edición de empleados; `PrepararParaEdicion` y `LimpiarFormulario` preparan el formulario y expone eventos `OperacionTerminada`/`AlVolver`.
- `FormBackupDatabase`: configura periodicidad, genera y restaura backups; valida acceso, habilita/deshabilita controles y muestra estado ocupado.

### Controles de usuario

- `UcInicio`: panel inicial y `SetUsuario` muestra el usuario actual.
- `UcInventario`: configura grid y permisos, carga/filtra productos, abre edición, cambia estados y navega a categorías, proveedores e ingreso de stock.
- `UcVentas`: abre/mueve/cierra caja, busca por código de barras, agrega productos al ticket, recalcula total/descuento, cobra y llama a `IVentaService`.
- `UcReportes`: selecciona reporte, filtros y gráficos; renderiza torta/línea y solicita PDFs.
- `UcConfig`: muestra configuración y abre operaciones de backup.
- `UcUsuarios`: `MostrarListarUsuarios` y `MostrarCargarUsuario` alternan listado y formulario.
- `UcListarUsuarios`: carga/filtra la grilla, selecciona empleado, edita y da de baja; expone solicitudes de edición y vuelta.
- `ValidadorUI`: validadores estáticos para letras, números, decimales, teléfono, alfanumérico, nombre de producto, CUIT y email; los métodos `Configurar...` conectan validación a `TextBox` y `MostrarAviso` informa errores.

## 9. Flujos funcionales principales

### Login y sesión

`FormLogin` → `AuthService.LoginAsync` → `EmpleadoRepository.ObtenerPorDni` → BCrypt/estado → `Program` muestra `FormInicio` → `SesionActual.Usuario` se utiliza para permisos y sucursal. Al cerrar, `CajaService.CerrarCajaPorCierreSesion` intenta cerrar la caja y `SesionActual.Limpiar` borra el contexto.

### Alta de compra y stock

La UI llama a `CompraService` o `StockService`. Application valida permiso y datos; Infrastructure usa los repositorios correspondientes. El stock está identificado por `(IdProducto, IdSucursal)`, no solamente por producto.

### Venta

`UcVentas` verifica previamente stock y cobra mediante `FormCobro`; convierte el medio seleccionado a un ID, construye `Venta` y `DetalleVenta`, y llama a `VentaService.RegistrarVenta`. El repositorio ejecuta procedimientos almacenados dentro de una transacción y registra la venta, los detalles, el descuento de stock y el pago. Después se genera el ticket con `TicketService`.

### Reportes

`UcReportes` solicita resumen, ranking o serie temporal a `ReporteService`; el servicio delega a `ReporteRepository` y QuestPDF/Chart de WinForms se encargan de PDF y gráficos.

### Backup

`FormBackupDatabase` llama a `IBackupService` para generar/restaurar o guardar programación. Durante el inicio de sesión, `Program` dispara una comprobación de backup pendiente.

## 10. Pruebas

`StockOS.Application.Tests` usa xUnit, Moq y `Microsoft.NET.Test.Sdk`. La suite cubre `AuthService`, autorización, caja y aislamiento de sesiones, categorías, compras, configuración, empleados, productos, proveedores, reportes, roles, stock, sucursales, tickets, pagos, validadores UI y ventas. Hay además pruebas de regresión de consistencia, robustez de IVA y seguridad del backup.

Los escenarios de venta prueban permisos, venta nula, detalles vacíos, cantidades inválidas y precios negativos. Las pruebas de caja prueban aislamiento y operaciones de apertura/cierre. `ValidadorUITests` cubre entradas válidas e inválidas para los formatos soportados.

## 11. Hallazgos y errores detectados

La solución compila correctamente en la revisión realizada. No se detectaron errores de compilación. Sí se observan los siguientes riesgos o defectos potenciales en el código actual:

1. **Backup en segundo plano no esperado:** `Program.Main` ejecuta `Task.Run(() => backupService.VerificarYEjecutarBackupPendienteAsync())` sin esperar ni observar la tarea. Una excepción puede quedar sin mostrar y el servicio depende del scope/`DbContext` creado para la sesión.
2. **Estado global de sesión:** `SesionActual` usa propiedades estáticas mutables. Esto dificulta pruebas paralelas, impide varias sesiones dentro del mismo proceso y puede provocar estado residual si se añade concurrencia.
3. **Sucursal por defecto silenciosa:** `SesionActual.IdSucursal` devuelve `1` si no hay usuario. Una llamada accidental sin sesión podría operar sobre la sucursal 1 en vez de fallar explícitamente.
4. **Advertencias desactivadas:** Application, Infrastructure y UI agregan `CS1998` y `CS8618` a `NoWarn`. Esto puede ocultar métodos async que no esperan y problemas reales de inicialización/nullability.
5. **Configuración de infraestructura en archivo de UI:** la cadena de SQL Server y datos comerciales están en `appsettings.json`. Para producción convendría usar User Secrets, variables de entorno o un almacén seguro y validar la configuración al iniciar.
6. **Dependencia de base de datos externa:** venta, reportes y backup dependen de procedimientos almacenados, permisos de SQL Server, `sqlcmd`/herramientas del sistema y la base `StockOS`; no se puede validar su funcionamiento completo únicamente con la compilación.
7. **Acoplamiento de referencias:** el proyecto de tests referencia directamente UI WinForms y el proyecto Domain referencia QuestPDF aunque la generación de documentos pertenece a Application. No rompe la compilación, pero aumenta el acoplamiento y el coste de mantenimiento.
8. **Riesgo de reglas duplicadas en UI:** `UcVentas` convierte nombres de medios de pago a IDs (`1`–`4`) mediante texto. Si el catálogo de `MetodoPago` cambia, la UI puede quedar desincronizada con la base de datos.

Estos puntos son hallazgos estáticos; deben confirmarse en un entorno con la base de datos y la configuración de producción antes de clasificarlos como fallos operativos.
