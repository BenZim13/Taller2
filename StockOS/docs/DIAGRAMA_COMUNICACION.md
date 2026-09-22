# 🔄 Diagrama de Comunicación - StockOS

## Flujo de Comunicación entre Capas

```
┌────────────────────────────────────────────────────────────────────────┐
│                        CAPA DE PRESENTACIÓN                            │
│                      (StockOS.UI.WinForms)                             │
│                                                                        │
│  ┌──────────────┐   ┌──────────────┐   ┌──────────────┐                │
│  │  FormLogin   │   │ FormInicio   │   │  UcVentas    │                │
│  │              │   │              │   │              │                │
│  │ - txtUsuario │   │ - MenuStrip  │   │ - DataGrid   │                │
│  │ - txtPassword│   │ - Panel      │   │ - btnCobrar  │                │
│  │ - btnIngresar│   │ -UserControls│   │ - txtCodigo  │                │ 
│  └──────┬───────┘   └──────┬───────┘   └──────┬───────┘                │
│         │                  │                  │                        │
│         │btnIngresar_Click │MenuClick         │btnCobrar_Click         │ 
│         │                  │                  │                        │
└─────────┼──────────────────┼──────────────────┼───────────────────────-┘
		  │                  │                  │
		  │ Inyección de Dependencias           │
		  │ (Microsoft.Extensions.DependencyInjection)
		  │                  │                  │
		  ▼                  ▼                  ▼
┌────────────────────────────────────────────────────────────────────────┐
│                      CAPA DE APLICACIÓN                                │
│                    (StockOS.Application)                               │
│                                                                        │
│  ┌─────────────────────────────────────────────────────────────────┐  │
│  │                    SERVICIOS DE NEGOCIO                         │  │
│  │                                                                 │  │
│  │  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐        │  │
│  │  │ AuthService  │  │VentaService  │  │ProductoService│       │  │
│  │  │              │  │              │  │              │        │  │
│  │  │ LoginAsync() │  │RegistrarVenta│  │BuscarPorCodigo│      │  │
│  │  │ • Validar    │  │• ValidarPerm │  │• ValidarPerm  │      │  │
│  │  │   Empleado   │  │• ValidarDatos│  │• Buscar       │      │  │
│  │  │ • BCrypt     │  │• Delegar     │  │              │        │  │
│  │  └──────┬───────┘  └──────┬───────┘  └──────┬───────┘        │  │
│  │         │                 │                 │                 │  │
│  └─────────┼─────────────────┼─────────────────┼─────────────────┘  │
│            │                 │                 │                     │
│  ┌─────────┼─────────────────┼─────────────────┼─────────────────┐  │
│  │         ▼                 ▼                 ▼                 │  │
│  │  ┌────────────────────────────────────────────────┐          │  │
│  │  │       AuthorizationService (RBAC)              │          │  │
│  │  │                                                │          │  │
│  │  │  ValidarPermiso(string permiso)                │          │  │
│  │  │  ├─ Obtener usuario de SesionActual           │          │  │
│  │  │  ├─ Verificar rol del usuario                 │          │  │
│  │  │  ├─ Consultar matriz de permisos              │          │  │
│  │  │  └─ Lanzar excepción si no autorizado         │          │  │
│  │  │                                                │          │  │
│  │  │  Matriz de Permisos:                          │          │  │
│  │  │  • Rol 1 (Admin): TODOS                       │          │  │
│  │  │  • Rol 2 (Cajero): VENTAS, CAJA              │          │  │
│  │  │  • Rol 3 (Encargado): STOCK, COMPRAS         │          │  │
│  │  │  • Rol 4 (Repositor): Solo Lectura           │          │  │
│  │  └────────────────────────────────────────────────┘          │  │
│  │                                                               │  │
│  │  ┌────────────────────────────────────────────────┐          │  │
│  │  │           SesionActual (Singleton)             │          │  │
│  │  │                                                │          │  │
│  │  │  static Usuario: Empleado?                     │          │  │
│  │  │  static IdCajaSesionAbierta: int?              │          │  │
│  │  │  static EstablecerUsuario(Empleado)            │          │  │
│  │  │  static Limpiar()                              │          │  │
│  │  └────────────────────────────────────────────────┘          │  │
│  └───────────────────────────────────────────────────────────────┘  │
│                                                                      │
│            │ Llama a Interfaces (Dependency Inversion)              │
│            ▼                                                         │
└────────────────────────────────────────────────────────────────────────┘
			│
			│ Contratos (IProductoRepository, IVentaRepository, etc.)
			▼
┌────────────────────────────────────────────────────────────────────────┐
│                       CAPA DE DOMINIO                                  │
│                      (StockOS.Domain)                                  │
│                                                                        │
│  ┌─────────────────────────────────────────────────────────────────┐  │
│  │                      INTERFACES                                 │  │
│  │                                                                 │  │
│  │  interface IProductoRepository {                                │  │
│  │      IEnumerable<Producto> ObtenerTodos();                      │  │
│  │      Producto? BuscarPorCodigoBarra(string codigo);             │  │
│  │      void Agregar(Producto producto);                           │  │
│  │  }                                                              │  │
│  │                                                                 │  │
│  │  interface IVentaRepository {                                   │  │
│  │      int RegistrarVenta(Venta, List<DetalleVenta>,             │  │
│  │                         int idSucursal, int idMetodoPago);      │  │
│  │  }                                                              │  │
│  └─────────────────────────────────────────────────────────────────┘  │
│                                                                        │
│  ┌─────────────────────────────────────────────────────────────────┐  │
│  │                       ENTIDADES                                 │  │
│  │                                                                 │  │
│  │  Producto {                      Venta {                        │  │
│  │    IdProducto: int                 IdVenta: int                 │  │
│  │    CodigoBarra: string             FechaHora: DateTime          │  │
│  │    Nombre: string                  TotalVenta: decimal          │  │
│  │    PrecioVenta: decimal            IdCajaSesion: int            │  │
│  │    PorcentajeIva: decimal          DetalleVenta: ICollection    │  │
│  │  }                               }                              │  │
│  │                                                                 │  │
│  │  Empleado {                      CajaSesion {                   │  │
│  │    IdEmpleado: int                 IdCajaSesion: int            │  │
│  │    Dni: string                     FechaApertura: DateTime      │  │
│  │    PasswordHash: string            MontoApertura: decimal       │  │
│  │    IdRol: int                      Estado: byte                 │  │
│  │  }                                 IdEmpleado: int              │  │
│  │                                 }                               │  │
│  └─────────────────────────────────────────────────────────────────┘  │
│                                                                        │
│  ┌─────────────────────────────────────────────────────────────────┐  │
│  │                      ENUMERACIONES                              │  │
│  │                                                                 │  │
│  │  static class Permisos {                                        │  │
│  │      const string VENTAS_REALIZAR = "VENTAS_REALIZAR";         │  │
│  │      const string PRODUCTOS_CREAR = "PRODUCTOS_CREAR";         │  │
│  │      const string CAJA_ABRIR = "CAJA_ABRIR";                   │  │
│  │      // ... 15 permisos más                                    │  │
│  │  }                                                              │  │
│  └─────────────────────────────────────────────────────────────────┘  │
│                                                                        │
└────────────────────────────────────────────────────────────────────────┘
			│
			│ Implementaciones (Concrete Classes)
			▼
┌────────────────────────────────────────────────────────────────────────┐
│                   CAPA DE INFRAESTRUCTURA                              │
│                  (StockOS.Infrastructure)                              │
│                                                                        │
│  ┌─────────────────────────────────────────────────────────────────┐  │
│  │                       REPOSITORIOS                              │  │
│  │                                                                 │  │
│  │  class ProductoRepository : IProductoRepository {               │  │
│  │      private StockOsContext _context;                           │  │
│  │                                                                 │  │
│  │      Producto? BuscarPorCodigoBarra(string codigo) {           │  │
│  │          return _context.Productos                              │  │
│  │              .Include(p => p.IdCategoriaNavigation)             │  │
│  │              .Include(p => p.StockSucursals)                    │  │
│  │              .FirstOrDefault(p => p.CodigoBarra == codigo);     │  │
│  │      }                                                          │  │
│  │  }                                                              │  │
│  │                                                                 │  │
│  │  class VentaRepository : IVentaRepository {                     │  │
│  │      int RegistrarVenta(...) {                                  │  │
│  │          using var transaction = _context.Database              │  │
│  │              .BeginTransaction();                               │  │
│  │          try {                                                  │  │
│  │              // 1. sp_Ventas_Insertar                          │  │
│  │              _context.Database.ExecuteSqlRaw(                   │  │
│  │                  "EXEC sp_Ventas_Insertar ...");                │  │
│  │                                                                 │  │
│  │              // 2. sp_DetalleVenta_Insertar (x N)              │  │
│  │              foreach(item in detalles) {                        │  │
│  │                  _context.Database.ExecuteSqlRaw(               │  │
│  │                      "EXEC sp_DetalleVenta_Insertar ...");      │  │
│  │                                                                 │  │
│  │                  // 3. sp_Stock_Descontar                      │  │
│  │                  _context.Database.ExecuteSqlRaw(               │  │
│  │                      "EXEC sp_Stock_Descontar ...");            │  │
│  │              }                                                  │  │
│  │                                                                 │  │
│  │              // 4. sp_Pagos_Insertar                           │  │
│  │              _context.Database.ExecuteSqlRaw(                   │  │
│  │                  "EXEC sp_Pagos_Insertar ...");                 │  │
│  │                                                                 │  │
│  │              transaction.Commit();                              │  │
│  │              return idVenta;                                    │  │
│  │          } catch {                                              │  │
│  │              transaction.Rollback();                            │  │
│  │              throw;                                             │  │
│  │          }                                                      │  │
│  │      }                                                          │  │
│  │  }                                                              │  │
│  └─────────────────────────────────────────────────────────────────┘  │
│                                                                        │
│  ┌─────────────────────────────────────────────────────────────────┐  │
│  │                    PERSISTENCIA                                 │  │
│  │                                                                 │  │
│  │  class StockOsContext : DbContext {                             │  │
│  │      public DbSet<Producto> Productos { get; set; }            │  │
│  │      public DbSet<Venta> Ventas { get; set; }                  │  │
│  │      public DbSet<Empleado> Empleados { get; set; }            │  │
│  │      // ... 15 DbSets más                                      │  │
│  │                                                                 │  │
│  │      protected override OnModelCreating(ModelBuilder mb) {      │  │
│  │          // Configuración de 18 entidades                      │  │
│  │          mb.Entity<Producto>(entity => {                        │  │
│  │              entity.HasKey(e => e.IdProducto);                  │  │
│  │              entity.HasOne(p => p.IdCategoriaNavigation)        │  │
│  │                  .WithMany(c => c.Productos)                    │  │
│  │                  .HasForeignKey(p => p.IdCategoria);            │  │
│  │          });                                                    │  │
│  │          // ... 627 líneas de configuración                    │  │
│  │      }                                                          │  │
│  │  }                                                              │  │
│  └─────────────────────────────────────────────────────────────────┘  │
│                                                                        │
│            │ ADO.NET / Entity Framework Core                           │
│            ▼                                                           │
└────────────────────────────────────────────────────────────────────────┘
			│
			│ SQL Queries / Stored Procedures
			▼
┌────────────────────────────────────────────────────────────────────────┐
│                         BASE DE DATOS                                  │
│                        SQL Server 2022                                 │
│                                                                        │
│  ┌─────────────────────────────────────────────────────────────────┐  │
│  │                         TABLAS                                  │  │
│  │                                                                 │  │
│  │  producto (18 columnas)                                         │  │
│  │  ├─ id_producto PK                                              │  │
│  │  ├─ codigo_barra UNIQUE                                         │  │
│  │  ├─ precio_venta_actual                                         │  │
│  │  ├─ porcentaje_iva DEFAULT 21.00                                │  │
│  │  └─ FK: id_categoria, id_proveedor                              │  │
│  │                                                                 │  │
│  │  venta (9 columnas)                                             │  │
│  │  ├─ id_venta PK                                                 │  │
│  │  ├─ fecha_hora DEFAULT SYSDATETIME()                            │  │
│  │  ├─ total_venta                                                 │  │
│  │  └─ FK: id_caja_sesion, id_cliente                              │  │
│  │                                                                 │  │
│  │  caja_sesion (10 columnas)                                      │  │
│  │  ├─ id_caja_sesion PK                                           │  │
│  │  ├─ fecha_apertura                                              │  │
│  │  ├─ fecha_cierre (nullable)                                     │  │
│  │  ├─ monto_apertura                                              │  │
│  │  ├─ monto_cierre (nullable)                                     │  │
│  │  ├─ diferencia_cierre (nullable)                                │  │
│  │  ├─ estado (1=Abierta, 2=Cerrada)                               │  │
│  │  └─ FK: id_caja, id_empleado                                    │  │
│  │                                                                 │  │
│  │  ... (15 tablas más)                                            │  │
│  └─────────────────────────────────────────────────────────────────┘  │
│                                                                        │
│  ┌─────────────────────────────────────────────────────────────────┐  │
│  │                   STORED PROCEDURES                             │  │
│  │                                                                 │  │
│  │  sp_Ventas_Insertar (                                           │  │
│  │      @Subtotal DECIMAL(18,2),                                   │  │
│  │      @TotalVenta DECIMAL(18,2),                                 │  │
│  │      @IdCajaSesion INT,                                         │  │
│  │      @IdVenta INT OUTPUT                                        │  │
│  │  )                                                              │  │
│  │  BEGIN                                                          │  │
│  │      INSERT INTO venta (...) VALUES (...);                      │  │
│  │      SET @IdVenta = SCOPE_IDENTITY();                           │  │
│  │  END                                                            │  │
│  │                                                                 │  │
│  │  sp_Stock_Descontar (                                           │  │
│  │      @IdProducto INT,                                           │  │
│  │      @IdSucursal INT,                                           │  │
│  │      @CantidadAVender INT                                       │  │
│  │  )                                                              │  │
│  │  BEGIN                                                          │  │
│  │      -- Validar stock suficiente                               │  │
│  │      IF @StockActual < @CantidadAVender                         │  │
│  │          THROW 50002, 'Stock insuficiente', 1;                  │  │
│  │                                                                 │  │
│  │      -- Descontar                                              │  │
│  │      UPDATE stock_sucursal                                      │  │
│  │      SET cantidad_actual -= @CantidadAVender                    │  │
│  │      WHERE id_producto = @IdProducto;                           │  │
│  │  END                                                            │  │
│  │                                                                 │  │
│  │  ... (23 stored procedures más)                                 │  │
│  └─────────────────────────────────────────────────────────────────┘  │
│                                                                        │
│  ┌─────────────────────────────────────────────────────────────────┐  │
│  │                    TRIGGERS & AUDIT                             │  │
│  │                                                                 │  │
│  │  TRIGGER tr_Auditoria_Venta                                     │  │
│  │  ON venta AFTER INSERT                                          │  │
│  │  AS BEGIN                                                       │  │
│  │      INSERT INTO auditoria (tabla, operacion, fecha, usuario)   │  │
│  │      SELECT 'venta', 'INSERT', SYSDATETIME(), SYSTEM_USER       │  │
│  │      FROM inserted;                                             │  │
│  │  END                                                            │  │
│  └─────────────────────────────────────────────────────────────────┘  │
│                                                                        │
└────────────────────────────────────────────────────────────────────────┘
```

## Flujo de Ejemplo: Realizar una Venta

```
┌─────────────────────────────────────────────────────────────────────┐
│                         USUARIO                                     │
│  1. Escanea código de barras con lector HID                        │
│  2. Revisa productos en pantalla                                    │
│  3. Presiona "Finalizar Venta"                                     │
│  4. Selecciona método de pago                                      │
│  5. Confirma cobro                                                 │
└────────────────────────┬────────────────────────────────────────────┘
						 │
						 ▼
┌─────────────────────────────────────────────────────────────────────┐
│                    UI LAYER (WinForms)                              │
│                                                                     │
│  UcVentas.txtCodigoBarra_KeyDown(KeyEventArgs e)                   │
│  │                                                                  │
│  ├─ if (e.Key == Enter)                                            │
│  │   └─ var producto = _productoService                            │
│  │           .BuscarPorCodigoBarra(txtCodigoBarra.Text);           │
│  │                                                                  │
│  └─ AgregarProductoALista(producto);                               │
│                                                                     │
│  FormCobro.btnConfirmar_Click()                                    │
│  │                                                                  │
│  ├─ Construir Venta y List<DetalleVenta>                           │
│  │                                                                  │
│  └─ int idVenta = _ventaService.RegistrarVenta(                    │
│          cabecera: venta,                                          │
│          detalles: itemsList,                                      │
│          idSucursal: SesionActual.Usuario.IdSucursal,             │
│          idMetodoPago: cmbMetodoPago.SelectedValue                 │
│      );                                                            │
└────────────────────────┬────────────────────────────────────────────┘
						 │
						 ▼
┌─────────────────────────────────────────────────────────────────────┐
│                 APPLICATION LAYER (Services)                        │
│                                                                     │
│  ProductoService.BuscarPorCodigoBarra(string codigo)               │
│  │                                                                  │
│  ├─ _authService.ValidarPermiso(Permisos.PRODUCTOS_VER);           │
│  │   ├─ Obtiene usuario de SesionActual                            │
│  │   ├─ Verifica rol (ej: Cajero = Rol 2)                          │
│  │   ├─ Consulta matriz de permisos                                │
│  │   └─ ✅ Cajero tiene PRODUCTOS_VER                              │
│  │                                                                  │
│  └─ return _productoRepo.BuscarPorCodigoBarra(codigo);             │
│                                                                     │
│  VentaService.RegistrarVenta(...)                                  │
│  │                                                                  │
│  ├─ _authService.ValidarPermiso(Permisos.VENTAS_REALIZAR);         │
│  │   └─ ✅ Cajero tiene VENTAS_REALIZAR                            │
│  │                                                                  │
│  ├─ Validaciones de negocio:                                       │
│  │   ├─ if (cabecera == null) throw ArgumentNullException;         │
│  │   ├─ if (detalles.Count == 0) throw ArgumentException;          │
│  │   └─ foreach(item) validar cantidad > 0                         │
│  │                                                                  │
│  └─ return _ventaRepo.RegistrarVenta(...);                         │
└────────────────────────┬────────────────────────────────────────────┘
						 │
						 ▼
┌─────────────────────────────────────────────────────────────────────┐
│              INFRASTRUCTURE LAYER (Repositories)                    │
│                                                                     │
│  ProductoRepository.BuscarPorCodigoBarra(string codigo)            │
│  │                                                                  │
│  └─ return _context.Productos                                      │
│         .Include(p => p.IdCategoriaNavigation)                     │
│         .Include(p => p.StockSucursals)                            │
│         .FirstOrDefault(p => p.CodigoBarra == codigo);             │
│                                                                     │
│  VentaRepository.RegistrarVenta(...)                               │
│  │                                                                  │
│  ├─ using var transaction = _context.Database.BeginTransaction();  │
│  │                                                                  │
│  ├─ try {                                                          │
│  │   ├─ // Paso 1: Insertar cabecera                              │
│  │   │   _context.Database.ExecuteSqlRaw(                          │
│  │   │       "EXEC sp_Ventas_Insertar @Subtotal={0}, ...");        │
│  │   │                                                             │
│  │   ├─ int idVenta = (int)outputParam.Value;                     │
│  │   │                                                             │
│  │   ├─ // Paso 2: Insertar detalles y descontar stock            │
│  │   │   foreach(item in detalles) {                              │
│  │   │       _context.Database.ExecuteSqlRaw(                      │
│  │   │           "EXEC sp_DetalleVenta_Insertar ...");             │
│  │   │                                                             │
│  │   │       _context.Database.ExecuteSqlRaw(                      │
│  │   │           "EXEC sp_Stock_Descontar ...");                   │
│  │   │   }                                                         │
│  │   │                                                             │
│  │   ├─ // Paso 3: Registrar pago                                 │
│  │   │   _context.Database.ExecuteSqlRaw(                          │
│  │   │       "EXEC sp_Pagos_Insertar ...");                        │
│  │   │                                                             │
│  │   ├─ transaction.Commit();                                      │
│  │   └─ return idVenta;                                            │
│  │                                                                  │
│  └─ } catch {                                                      │
│        transaction.Rollback();                                     │
│        throw;                                                      │
│    }                                                               │
└────────────────────────┬────────────────────────────────────────────┘
						 │
						 ▼
┌─────────────────────────────────────────────────────────────────────┐
│                       BASE DE DATOS                                 │
│                                                                     │
│  sp_Ventas_Insertar                                                │
│  ├─ INSERT INTO venta (fecha_hora, subtotal, total_venta, ...)    │
│  │   VALUES (SYSDATETIME(), @Subtotal, @TotalVenta, ...);         │
│  └─ SET @IdVenta = SCOPE_IDENTITY();                               │
│                                                                     │
│  sp_DetalleVenta_Insertar                                          │
│  └─ INSERT INTO detalle_venta (cantidad, precio_unitario, ...)    │
│      VALUES (@Cantidad, @PrecioUnitario, ...);                     │
│                                                                     │
│  sp_Stock_Descontar                                                │
│  ├─ DECLARE @StockActual INT;                                      │
│  ├─ SELECT @StockActual = cantidad_actual                          │
│  │   FROM stock_sucursal                                           │
│  │   WHERE id_producto = @IdProducto                               │
│  │     AND id_sucursal = @IdSucursal;                              │
│  │                                                                  │
│  ├─ IF @StockActual < @CantidadAVender                             │
│  │     THROW 50002, 'Stock insuficiente', 1;                       │
│  │                                                                  │
│  └─ UPDATE stock_sucursal                                          │
│      SET cantidad_actual = cantidad_actual - @CantidadAVender,     │
│          ultima_actualizacion = SYSDATETIME()                      │
│      WHERE id_producto = @IdProducto;                              │
│                                                                     │
│  sp_Pagos_Insertar                                                 │
│  └─ INSERT INTO pago (id_venta, id_metodo_pago, monto, ...)       │
│      VALUES (@IdVenta, @IdMetodoPago, @Monto, ...);                │
│                                                                     │
│  TRIGGER tr_Auditoria_Venta (se ejecuta automáticamente)           │
│  └─ INSERT INTO auditoria (tabla, operacion, fecha)                │
│      VALUES ('venta', 'INSERT', SYSDATETIME());                    │
└────────────────────────┬────────────────────────────────────────────┘
						 │
						 │ Return idVenta (flujo inverso)
						 ▼
┌─────────────────────────────────────────────────────────────────────┐
│                    UI LAYER (Respuesta)                             │
│                                                                     │
│  FormCobro (continúa después de RegistrarVenta)                    │
│  │                                                                  │
│  ├─ // Generar ticket PDF                                          │
│  │   byte[] pdfBytes = _ticketService.GenerarTicketPdf(            │
│  │       venta: venta,                                             │
│  │       cajeroNombre: SesionActual.Usuario.NombreCompleto,        │
│  │       comercio: ConfiguracionService.ObtenerDatosComercio(),    │
│  │       pago: new DatosPago { Metodo = "Efectivo", ... }          │
│  │   );                                                            │
│  │                                                                  │
│  ├─ // Guardar o imprimir ticket                                   │
│  │   File.WriteAllBytes($"tickets/ticket_{idVenta}.pdf", pdfBytes);│
│  │                                                                  │
│  ├─ DialogResult = DialogResult.OK;                                │
│  └─ Close();                                                       │
│                                                                     │
│  UcVentas (al cerrar FormCobro)                                    │
│  │                                                                  │
│  ├─ LimpiarListaDeVenta();                                         │
│  ├─ MessageBox.Show("✅ Venta registrada exitosamente");           │
│  └─ txtCodigoBarra.Focus(); // Listo para nueva venta              │
└─────────────────────────────────────────────────────────────────────┘
```

## Principios de Clean Architecture Aplicados

### 1. **Dependency Inversion Principle (DIP)**
```
UI Layer depende de ───────┐
Application Layer          │
depende de ───────────────┐├─→ INTERFACES (Domain Layer)
Infrastructure Layer      ││                    ▲
implementa ───────────────┘│                    │
						   └────────────────────┘
						 (No hay ciclo, flujo unidireccional)
```

### 2. **Single Responsibility Principle (SRP)**
- **UI Layer**: Solo maneja presentación y eventos de usuario
- **Application Layer**: Solo contiene lógica de negocio y validaciones
- **Domain Layer**: Solo define contratos y entidades
- **Infrastructure Layer**: Solo implementa acceso a datos

### 3. **Open/Closed Principle (OCP)**
- Se puede agregar un nuevo repositorio implementando `IXxxRepository`
- Se puede agregar un nuevo servicio sin modificar los existentes
- Se puede cambiar de SQL Server a otro motor implementando las interfaces

### 4. **Interface Segregation Principle (ISP)**
- Cada repositorio tiene su propia interface específica
- No hay interfaces "gordas" con métodos que no se usan

### 5. **Liskov Substitution Principle (LSP)**
- Cualquier implementación de `IProductoRepository` puede reemplazar a otra
- Los tests usan mocks que implementan las interfaces

---

## Ventajas de Esta Arquitectura

✅ **Testeable**: 156 tests unitarios (100% éxito)  
✅ **Mantenible**: Cada capa tiene responsabilidad única  
✅ **Escalable**: Fácil agregar nuevos módulos  
✅ **Independiente**: Domain no depende de frameworks  
✅ **Segura**: Autorización en cada operación  
✅ **Transaccional**: ACID en operaciones críticas  
