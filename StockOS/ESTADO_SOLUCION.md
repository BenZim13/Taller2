# Estado actual de la solución StockOS

> Documento generado a partir de la inspección estática de la solución. Describe lo que está presente en el código, no sustituye una validación funcional contra una base de datos configurada.

## 1. Resumen ejecutivo

StockOS es una aplicación de escritorio para gestión de stock y ventas, construida con .NET 8 y Windows Forms. La solución está separada en cuatro proyectos:

- **Domain**: entidades, enum de roles e interfaces de repositorio.
- **Application**: servicios de negocio y sesión actual.
- **Infrastructure/DataAccess**: Entity Framework Core, contexto SQL Server y repositorios.
- **UI.WinForms**: formularios, controles de usuario, navegación y composición mediante inyección de dependencias.

El núcleo implementado cubre:

- Inicio de sesión por DNI y contraseña.
- Registro, edición, listado y baja lógica de empleados.
- Gestión básica de productos y categorías.
- Consulta de inventario por sucursal.
- Ingreso de stock.
- Apertura de caja.
- Armado de tickets y registro transaccional de ventas.
- Navegación y permisos básicos por rol.

**Estado de compilación durante la inspección:** se detectó inicialmente un `CS0117` en `StockOS/src/StockOS.UI.WinForms/Forms/UcVentas.cs`, línea 146, relacionado con el uso de `CajaSesion.SesionActual.Id`. La validación final del workspace devolvió compilación correcta y no reportó errores en ese archivo; conviene revisar este punto si el error reaparece en Visual Studio.

---

## 2. Estructura técnica

### 2.1 Proyectos

| Proyecto | Framework | Responsabilidad | Estado |
|---|---|---|---|
| `StockOS.Domain` | `net8.0` | Entidades, interfaces y roles | Implementado como base del dominio |
| `StockOS.Application` | `net8.0` | Servicios de aplicación | Implementación básica, con lógica delegada a repositorios |
| `StockOS.Infrastructure` / `StockOS.DataAccess` | `net8.0` | EF Core, SQL Server y repositorios | Implementado, depende de SQL externo |
| `StockOS.UI.WinForms` | `net8.0-windows` | Interfaz de escritorio | Implementada parcialmente |

### 2.2 Dependencias principales

- .NET 8.
- Windows Forms.
- Entity Framework Core 8.0.11.
- SQL Server mediante `Microsoft.EntityFrameworkCore.SqlServer`.
- BCrypt.Net-Next 4.2.0 para contraseñas.
- Microsoft.Extensions.Hosting y Dependency Injection.

### 2.3 Arranque de la aplicación

`Program.cs` configura:

- `StockOsContext` con SQL Server.
- Repositorios y servicios mediante DI.
- Formularios y controles de usuario.
- Flujo login → formulario principal → cierre de sesión o salida.

La cadena de conexión está actualmente escrita directamente en `Program.cs`:

```text
Server=localhost;Database=StockOS;Trusted_Connection=True;TrustServerCertificate=True;
```

No se encontró un archivo `appsettings.json`, configuración externa ni mecanismo de selección de entorno.

---

## 3. Modelo de dominio y persistencia

El contexto `StockOsContext` contiene mapeos para 18 conjuntos de entidades, entre ellos:

- `Empleado`, `Rol` y `Sucursal`.
- `Producto`, `Categoria` y `StockSucursal`.
- `Caja`, `CajaSesion` y `MovimientoCaja`.
- `Venta`, `DetalleVenta`, `Pago` y `MetodoPago`.
- `Cliente`, `Factura`, `Compra`, `DetalleCompra` y `Proveedor`.

Esto indica que el modelo de datos previsto es más amplio que la funcionalidad actualmente expuesta por la UI.

### Persistencia actual

Los repositorios combinan:

- Consultas LINQ/EF Core para algunas lecturas.
- Procedimientos almacenados para autenticación, usuarios, productos, stock, cajas y ventas.
- Transacciones EF Core para el registro de ventas.

Procedimientos almacenados referenciados desde el código:

- `sp_Usuarios_Autenticar`
- `sp_Usuarios_Insertar`
- `sp_Usuarios_Actualizar`
- `sp_Usuarios_CambiarEstado`
- `sp_Categorias_ObtenerTodas`
- `sp_Productos_ObtenerTodos`
- `sp_Productos_Insertar`
- `sp_Productos_Actualizar`
- `sp_Stock_ObtenerActual`
- `sp_Stock_IngresarMercaderia`
- `sp_Stock_Descontar`
- `sp_Cajas_ObtenerPorSucursal`
- `sp_CajaSesion_Abrir`
- `sp_CajaSesion_VerificarAbierta`
- `sp_Ventas_Insertar`
- `sp_DetalleVenta_Insertar`

No se encontraron scripts `.sql`, migraciones de EF Core, instrucciones de creación de base de datos ni datos semilla dentro del repositorio inspeccionado. Por tanto, la aplicación depende de que la base de datos y todos esos procedimientos existan previamente.

---

## 4. Funcionalidades terminadas o esencialmente operativas

> "Terminada" aquí significa que existe un flujo de código completo y conectado. No implica que esté validado con pruebas automatizadas ni que esté libre de problemas de entorno.

### 4.1 Autenticación

Archivos principales:

- `src/StockOS.Application/Services/AuthService.cs`
- `src/StockOS.Infrastructure/Repositories/EmpleadoRepository.cs`
- `src/StockOS.UI.WinForms/Forms/FormLogin.cs`

Incluye:

- Validación de DNI y contraseña no vacíos.
- Consulta del empleado por DNI.
- Verificación de contraseña BCrypt.
- Compatibilidad temporal con contraseñas almacenadas en texto plano y migración al hash al iniciar sesión.
- Registro del usuario en `SesionActual`.
- Cierre y reapertura del login al cerrar sesión.

**Estado:** funcional en código, pero depende de la base de datos y del procedimiento `sp_Usuarios_Autenticar`.

### 4.2 Gestión de usuarios

Archivos principales:

- `UcUsuarios.cs`
- `UcListarUsuarios.cs`
- `FormRegistroUsuario.cs`
- `EmpleadoService.cs`
- `EmpleadoRepository.cs`

Incluye:

- Listado de empleados.
- Búsqueda por DNI, nombre, apellido o correo.
- Filtro por estado activo/inactivo.
- Alta de empleados.
- Edición de empleados.
- Validación de DNI y email duplicados.
- Cambio de contraseña opcional durante la edición.
- Baja lógica cuando existen referencias históricas.
- Asignación de rol y sucursal.

El acceso desde el menú está restringido al rol gerente.

**Estado:** implementado de forma bastante completa, pendiente de pruebas y validación de reglas de negocio adicionales.

### 4.3 Gestión de productos e inventario

Archivos principales:

- `UcInventario.cs`
- `FormRegistroProducto.cs`
- `ProductoService.cs`
- `ProductoRepository.cs`

Incluye:

- Listado de productos.
- Búsqueda por código de barras o nombre.
- Filtro por estado.
- Alta de productos.
- Edición de productos.
- Selección de categoría desde la base de datos.
- Activación/inactivación mediante actualización.
- Visualización de stock para la sucursal de la sesión.

El cajero ve el inventario en modo solo lectura: se ocultan alta, edición, eliminación e ingreso de stock.

**Estado:** implementado para operaciones básicas.

### 4.4 Ingreso y consulta de stock

Archivos principales:

- `FormIngresoStock.cs`
- `StockService.cs`
- `StockSucursalRepository.cs`

Incluye:

- Búsqueda de producto por código de barras.
- Validación de cantidad positiva.
- Ingreso de mercadería para la sucursal actual.
- Consulta del stock actual por producto y sucursal.

**Estado:** implementado, pero la búsqueda carga todos los productos y filtra en memoria. Requiere que existan los procedimientos almacenados correspondientes.

### 4.5 Apertura de caja

Archivos principales:

- `FormAperturaCaja.cs`
- `CajaService.cs`
- `CajaRepository.cs`
- `CajaSesionRepository.cs`

Incluye:

- Carga de cajas por sucursal.
- Validación de caja seleccionada.
- Validación de monto inicial.
- Apertura de sesión mediante procedimiento almacenado.
- Verificación de caja abierta por empleado en el servicio.

**Estado:** apertura implementada. No se observa un flujo equivalente para cierre de caja ni liquidación.

### 4.6 Registro transaccional de ventas

Archivos principales:

- `UcVentas.cs`
- `FormCobro.cs`
- `VentaService.cs`
- `VentaRepository.cs`

Incluye:

- Lectura de código de barras.
- Incorporación de productos al ticket.
- Acumulación de cantidades.
- Eliminación de líneas.
- Cálculo de subtotal, descuento y total.
- Selección visual de efectivo, tarjeta o Mercado Pago.
- Inserción de cabecera y detalles.
- Descuento de stock.
- Transacción con commit/rollback.

**Estado:** flujo implementado a medias: durante la inspección apareció un error de compilación transitorio en `CajaSesion.SesionActual` y el método de pago seleccionado no se persiste en la venta.

### 4.7 Navegación y permisos básicos

`FormInicio.cs` construye el menú según el rol:

- **Gerente:** Inicio, Inventario, Ventas, Reportes, Usuarios y Configuración.
- **Cajero:** Ventas e Inventario.
- **Encargado de depósito:** Inicio e Inventario.
- **Repositor:** Inicio e Inventario.

**Estado:** implementado a nivel de navegación y visibilidad; faltan validaciones centralizadas de autorización en servicios/repositorios.

---

## 5. Funcionalidades implementadas a medias

### 5.1 Ventas

Pendientes o problemáticas:

1. Corregir el error de compilación en `UcVentas.cs:146`.
2. Usar la sesión de caja real y validar que exista una caja abierta antes de vender.
3. Guardar el método de pago elegido. Actualmente solo se muestra en el mensaje final; `metodoPago` no se utiliza para crear un `Pago`.
4. Verificar que el descuento se distribuya correctamente. Actualmente se reparte de forma uniforme entre el número de líneas, no entre cantidades o importes.
5. Implementar búsqueda de producto específica por código de barras. Actualmente se llama a `ObtenerTodos()` y se filtra en memoria.
6. Generar el comprobante o PDF. La propia interfaz muestra `[Siguiente Paso: Generar PDF]`.
7. Validar stock disponible antes de confirmar la operación y presentar un mensaje de negocio claro.

### 5.2 Caja

Existe apertura y verificación, pero faltan:

- Cierre de caja.
- Cálculo de monto esperado.
- Registro de diferencias.
- Movimientos de caja manuales.
- Historial y consulta de sesiones.
- Bloqueo de ventas si no existe una sesión abierta.

### 5.3 Control de acceso

El menú oculta opciones según el rol, pero el control está principalmente en la UI. Falta una política de autorización centralizada en la capa de aplicación para evitar depender exclusivamente de botones ocultos.

### 5.4 Servicios de aplicación

Varios métodos están declarados como `async`, pero ejecutan operaciones síncronas y no contienen `await`. Esto produce una API asíncrona aparente, sin aprovechar operaciones asíncronas de base de datos.

### 5.5 Configuración y despliegue

La conexión a SQL Server está fija en código y no hay configuración por ambiente. Falta separar configuración de desarrollo, pruebas y producción.

---

## 6. Funcionalidades presentes como pantalla, pero sin implementación real

### 6.1 Reportes

Archivos:

- `UcReportes.cs`
- `UcReportes.Designer.cs`

El control solo inicializa la interfaz. No contiene consultas, filtros, indicadores, exportación ni generación de reportes.

**Estado:** placeholder visual.

### 6.2 Configuración

Archivos:

- `UcConfig.cs`
- `UcConfig.Designer.cs`

El control solo inicializa la interfaz. No hay opciones funcionales de configuración.

**Estado:** placeholder visual.

### 6.3 Compras, proveedores y clientes

Existen entidades y tablas mapeadas para compras, proveedores y clientes, pero no se observan servicios, repositorios completos ni pantallas para operar estos módulos.

**Estado:** modelado parcialmente en dominio/persistencia, no disponible como funcionalidad de usuario.

### 6.4 Facturación y pagos

Existen entidades `Factura`, `Pago` y `MetodoPago`, pero el flujo de venta no crea un pago ni una factura. La selección de método de pago es únicamente visual.

**Estado:** modelo previsto, flujo no terminado.

---

## 7. Elementos ausentes

No se encontraron en la solución:

- Proyecto de pruebas unitarias o de integración.
- Scripts de creación de base de datos.
- Scripts de procedimientos almacenados.
- Migraciones de Entity Framework Core.
- Archivo de configuración externo para la conexión.
- Documentación de instalación y puesta en marcha.
- Datos semilla para roles, sucursales, categorías, métodos de pago o usuario inicial.
- Cierre de caja.
- Módulo de reportes.
- Módulo de configuración.
- Gestión de clientes desde la UI.
- Gestión de proveedores y compras desde la UI.
- Generación de PDF o comprobantes.
- Persistencia del método de pago.
- Registro de factura asociado a una venta.

También permanecen archivos de plantilla como `Class1.cs` en varios proyectos y una carpeta `DTOs` sin DTOs visibles; no bloquean por sí mismos, pero indican estructura pendiente de limpieza o evolución.

---

## 8. Bloqueos y riesgos técnicos

### Bloqueo inmediato

- **CS0117 en `UcVentas.cs:146`**: impide compilar la solución.

### Dependencia crítica de SQL Server

La aplicación invoca procedimientos almacenados que no están incluidos en el repositorio. Una instalación nueva no tiene forma documentada de crear la base de datos completa.

### Estado de la caja no integrado con la sesión global

La sesión global solo conserva el empleado (`SesionActual.Usuario`). No se observa una integración equivalente y clara para almacenar la sesión de caja abierta, aunque ventas intenta acceder a `CajaSesion.SesionActual`.

### Posibles problemas de rendimiento

- Inventario y alta de stock buscan productos cargando todos los productos y filtrando en memoria.
- La pantalla de inventario consulta el stock individualmente para cada producto, lo que puede generar muchas llamadas a la base de datos.

### Manejo de errores

La UI muestra `ex.Message` directamente. Esto es útil durante desarrollo, pero puede exponer detalles internos de SQL o infraestructura al usuario final.

### Consistencia de datos

La lógica de negocio depende en parte de procedimientos almacenados y en parte de EF Core directo. Deben documentarse claramente las reglas que viven en SQL para evitar divergencias entre aplicación y base de datos.

### Seguridad

- La compatibilidad con contraseñas en texto plano es útil para migración, pero debería eliminarse cuando todos los registros estén migrados.
- `TrustServerCertificate=True` es apropiado solo para ciertos entornos de desarrollo y debe revisarse antes de producción.
- La conexión y las credenciales de acceso no deben quedar embebidas en código en un entorno real.

---

## 9. Estado resumido por módulo

| Módulo | Estado | Observaciones |
|---|---|---|
| Login | Implementado | Depende de BD/SP; usa BCrypt y sesión global |
| Roles | Implementado parcialmente | Menú por rol; autorización no centralizada |
| Usuarios | Implementado | Alta, edición, listado, filtros y baja lógica |
| Productos | Implementado | Alta, edición, listado y estado |
| Categorías | Consulta implementada | No hay gestión de categorías en UI |
| Inventario | Implementado parcialmente | Stock por sucursal; consultas en memoria/por producto |
| Ingreso de stock | Implementado | Depende de SP y sesión de sucursal |
| Caja | Implementado parcialmente | Apertura sí; cierre y movimientos no |
| Ventas | Implementado parcialmente | Revisar el error transitorio de compilación; pago no persistido |
| Facturación | No implementado | Entidad presente, flujo ausente |
| Clientes | No implementado en UI | Entidad presente |
| Compras | No implementado en UI | Entidades presentes |
| Proveedores | No implementado en UI | Entidad presente |
| Pagos | No implementado en flujo | Método de pago solo visual |
| Reportes | No implementado | Control vacío |
| Configuración | No implementado | Control vacío |
| PDF/comprobantes | No implementado | Mencionado como siguiente paso |
| Pruebas automatizadas | Ausentes | No hay proyecto de tests |
| Instalación de BD | Incompleta | Faltan scripts/migraciones/documentación |

---

## 10. Orden recomendado de trabajo

### Prioridad 1: recuperar una compilación y ejecución mínima

1. Corregir la referencia de sesión de caja en `UcVentas.cs`.
2. Definir explícitamente cómo se representa y conserva la caja abierta.
3. Compilar nuevamente la solución.
4. Documentar o incorporar la base de datos y los procedimientos almacenados requeridos.

### Prioridad 2: cerrar el circuito de ventas

1. Validar caja abierta antes de vender.
2. Persistir el método de pago en `Pago`.
3. Crear la factura o comprobante correspondiente si forma parte del alcance.
4. Validar stock y reglas de descuento.
5. Añadir generación de comprobante.

### Prioridad 3: completar caja y operación diaria

1. Implementar cierre de caja.
2. Registrar movimientos y diferencias.
3. Mostrar historial de sesiones.
4. Añadir mensajes de negocio y auditoría.

### Prioridad 4: completar módulos pendientes

1. Reportes.
2. Configuración.
3. Clientes.
4. Proveedores y compras.
5. Categorías y métodos de pago administrables.

### Prioridad 5: calidad y despliegue

1. Añadir pruebas unitarias para servicios.
2. Añadir pruebas de integración para repositorios y procedimientos almacenados.
3. Externalizar la configuración.
4. Añadir scripts/migraciones y datos semilla.
5. Sustituir errores técnicos expuestos por mensajes controlados y logging.

---

## 11. Conclusión

StockOS tiene una base arquitectónica definida y un primer flujo operativo real para autenticación, usuarios, productos, stock, apertura de caja y ventas. La mayor parte de la estructura necesaria para un sistema de gestión comercial está modelada.

Sin embargo, el estado actual no es todavía una versión terminada: la base de datos no está reproduciblemente incluida, el ciclo de caja está incompleto, el método de pago no se guarda y los módulos de reportes/configuración son solo pantallas. Aunque la validación final de compilación fue correcta, debe verificarse el error transitorio detectado durante la inspección. La siguiente meta técnica debe ser cerrar el flujo completo **apertura de caja → venta → pago → comprobante → cierre de caja**, acompañado de pruebas y documentación de instalación.

## 12. Validación realizada

- Compilación final de la solución desde el workspace: **correcta**.
- Diagnóstico final de `UcVentas.cs`: **sin errores reportados**.
- Pruebas automatizadas: **no disponibles**, porque la solución no incluye proyecto de tests.
