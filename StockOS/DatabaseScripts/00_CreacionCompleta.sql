USE StockOS;
GO

-- ==========================================
-- 1. LIMPIEZA PREVIA (Orden Inverso)
-- ==========================================

DROP TABLE IF EXISTS movimiento_caja;
DROP TABLE IF EXISTS pago;
DROP TABLE IF EXISTS metodo_pago;
DROP TABLE IF EXISTS factura;
DROP TABLE IF EXISTS detalle_venta;
DROP TABLE IF EXISTS venta;
DROP TABLE IF EXISTS caja_sesion;
DROP TABLE IF EXISTS caja;
DROP TABLE IF EXISTS detalle_compra;
DROP TABLE IF EXISTS compra;
DROP TABLE IF EXISTS stock_sucursal;
DROP TABLE IF EXISTS producto;
DROP TABLE IF EXISTS proveedor;
DROP TABLE IF EXISTS categoria;
DROP TABLE IF EXISTS cliente;
DROP TABLE IF EXISTS empleado;
DROP TABLE IF EXISTS sucursal;
DROP TABLE IF EXISTS rol;
GO

-- ==========================================
-- 2. CREACIÓN DE TABLAS BASE
-- ==========================================

CREATE TABLE rol (
  id_rol INT IDENTITY(1,1) NOT NULL,
  nombre VARCHAR(50) NOT NULL UNIQUE,
  descripcion VARCHAR(255) NOT NULL,
  PRIMARY KEY (id_rol)
);
GO

CREATE TABLE sucursal (
  id_sucursal INT IDENTITY(1,1) NOT NULL,
  nombre VARCHAR(100) NOT NULL UNIQUE,
  direccion VARCHAR(200) NOT NULL,
  activo BIT NOT NULL DEFAULT 1,
  PRIMARY KEY (id_sucursal)
);
GO

CREATE TABLE empleado (
  id_empleado INT IDENTITY(1,1) NOT NULL,
  nombre VARCHAR(50) NOT NULL,
  apellido VARCHAR(50) NOT NULL,
  dni VARCHAR(20) NOT NULL UNIQUE,
  email VARCHAR(100) NOT NULL UNIQUE,
  telefono VARCHAR(30) NOT NULL,
  direccion VARCHAR(200) NOT NULL,
  password_hash VARCHAR(255) NOT NULL,
  estado BIT NOT NULL DEFAULT 1, 
  id_rol INT NOT NULL,
  id_sucursal INT NOT NULL,
  PRIMARY KEY (id_empleado),
  FOREIGN KEY (id_rol) REFERENCES rol(id_rol),
  FOREIGN KEY (id_sucursal) REFERENCES sucursal(id_sucursal)
);
GO

CREATE TABLE cliente (
  id_cliente INT IDENTITY(1,1) NOT NULL,
  razon_social VARCHAR(100) NOT NULL,
  cuit_dni VARCHAR(30),
  condicion_iva VARCHAR(50), 
  telefono VARCHAR(30),
  email VARCHAR(100),
  direccion VARCHAR(200),
  activo BIT NOT NULL DEFAULT 1,
  PRIMARY KEY (id_cliente)
);
GO

CREATE TABLE categoria (
  id_categoria INT IDENTITY(1,1) NOT NULL,
  nombre VARCHAR(100) NOT NULL UNIQUE,
  descripcion VARCHAR(255) NOT NULL,
  activo BIT NOT NULL DEFAULT 1,
  PRIMARY KEY (id_categoria)
);
GO

CREATE TABLE proveedor (
  id_proveedor INT IDENTITY(1,1) NOT NULL,
  razon_social VARCHAR(100) NOT NULL,
  cuit VARCHAR(30) UNIQUE,
  telefono VARCHAR(30) NOT NULL,
  email VARCHAR(100) NOT NULL,
  direccion VARCHAR(200) NOT NULL,
  activo BIT NOT NULL DEFAULT 1,
  PRIMARY KEY (id_proveedor)
);
GO

CREATE TABLE producto (
  id_producto INT IDENTITY(1,1) NOT NULL,
  codigo_barra VARCHAR(50) NOT NULL UNIQUE,
  nombre VARCHAR(100) NOT NULL,
  descripcion VARCHAR(255),
  precio_venta_actual DECIMAL(12, 2) NOT NULL CHECK (precio_venta_actual >= 0),
  porcentaje_iva DECIMAL(5, 2) NOT NULL DEFAULT 21.00,
  id_categoria INT NOT NULL,
  id_proveedor INT NULL, 
  activo BIT NOT NULL DEFAULT 1,
  PRIMARY KEY (id_producto),
  FOREIGN KEY (id_categoria) REFERENCES categoria(id_categoria),
  FOREIGN KEY (id_proveedor) REFERENCES proveedor(id_proveedor)
);
GO

CREATE TABLE stock_sucursal (
  id_producto INT NOT NULL,
  id_sucursal INT NOT NULL,
  stock_actual INT NOT NULL DEFAULT 0,
  stock_minimo INT NOT NULL DEFAULT 0 CHECK (stock_minimo >= 0),
  PRIMARY KEY (id_producto, id_sucursal),
  FOREIGN KEY (id_producto) REFERENCES producto(id_producto),
  FOREIGN KEY (id_sucursal) REFERENCES sucursal(id_sucursal)
);
GO

CREATE TABLE compra (
  id_compra INT IDENTITY(1,1) NOT NULL,
  fecha_hora DATETIME2(0) NOT NULL DEFAULT SYSDATETIME(),
  total DECIMAL(12, 2) NOT NULL CHECK (total >= 0),
  numero_comprobante VARCHAR(50),
  id_proveedor INT NOT NULL,
  id_empleado INT NOT NULL,
  id_sucursal INT NOT NULL,
  PRIMARY KEY (id_compra),
  FOREIGN KEY (id_proveedor) REFERENCES proveedor(id_proveedor),
  FOREIGN KEY (id_empleado) REFERENCES empleado(id_empleado),
  FOREIGN KEY (id_sucursal) REFERENCES sucursal(id_sucursal)
);
GO

CREATE TABLE detalle_compra (
  id_detalle_compra INT IDENTITY(1,1) NOT NULL,
  cantidad INT NOT NULL CHECK (cantidad > 0),
  precio_unitario_compra DECIMAL(12, 2) NOT NULL CHECK (precio_unitario_compra >= 0),
  id_compra INT NOT NULL,
  id_producto INT NOT NULL,
  PRIMARY KEY (id_detalle_compra),
  FOREIGN KEY (id_compra) REFERENCES compra(id_compra),
  FOREIGN KEY (id_producto) REFERENCES producto(id_producto),
  UNIQUE (id_compra, id_producto)
);
GO

CREATE TABLE caja (
  id_caja INT IDENTITY(1,1) NOT NULL,
  nombre_numero VARCHAR(50) NOT NULL,
  id_sucursal INT NOT NULL,
  activo BIT NOT NULL DEFAULT 1,
  PRIMARY KEY (id_caja),
  FOREIGN KEY (id_sucursal) REFERENCES sucursal(id_sucursal),
  UNIQUE (id_sucursal, nombre_numero)
);
GO

CREATE TABLE caja_sesion (
  id_caja_sesion INT IDENTITY(1,1) NOT NULL,
  fecha_apertura DATETIME2(0) NOT NULL DEFAULT SYSDATETIME(),
  fecha_cierre DATETIME2(0),
  monto_apertura DECIMAL(12, 2) NOT NULL DEFAULT 0 CHECK (monto_apertura >= 0),
  monto_cierre_real DECIMAL(12, 2) NULL CHECK (monto_cierre_real >= 0),
  diferencia_cierre DECIMAL(12, 2) NULL, 
  estado TINYINT NOT NULL DEFAULT 1, 
  id_caja INT NOT NULL,
  id_empleado INT NOT NULL,
  PRIMARY KEY (id_caja_sesion),
  FOREIGN KEY (id_caja) REFERENCES caja(id_caja),
  FOREIGN KEY (id_empleado) REFERENCES empleado(id_empleado)
);
GO

CREATE TABLE movimiento_caja (
  id_movimiento INT IDENTITY(1,1) NOT NULL,
  fecha_hora DATETIME2(0) NOT NULL DEFAULT SYSDATETIME(),
  tipo_movimiento VARCHAR(10) NOT NULL CHECK (tipo_movimiento IN ('INGRESO', 'EGRESO')),
  monto DECIMAL(12, 2) NOT NULL CHECK (monto > 0),
  descripcion VARCHAR(255) NOT NULL,
  id_caja_sesion INT NOT NULL,
  PRIMARY KEY (id_movimiento),
  FOREIGN KEY (id_caja_sesion) REFERENCES caja_sesion(id_caja_sesion)
);
GO

CREATE TABLE venta (
  id_venta INT IDENTITY(1,1) NOT NULL,
  fecha_hora DATETIME2(0) NOT NULL DEFAULT SYSDATETIME(),
  subtotal DECIMAL(12, 2) NOT NULL CHECK (subtotal >= 0),
  descuento_total DECIMAL(12, 2) NOT NULL DEFAULT 0 CHECK (descuento_total >= 0),
  total_venta DECIMAL(12, 2) NOT NULL CHECK (total_venta >= 0),
  estado TINYINT NOT NULL DEFAULT 1, 
  id_caja_sesion INT NOT NULL,
  id_cliente INT NULL, 
  PRIMARY KEY (id_venta),
  FOREIGN KEY (id_caja_sesion) REFERENCES caja_sesion(id_caja_sesion),
  FOREIGN KEY (id_cliente) REFERENCES cliente(id_cliente)
);
GO

CREATE TABLE detalle_venta (
  id_detalle_venta INT IDENTITY(1,1) NOT NULL,
  cantidad INT NOT NULL CHECK (cantidad > 0),
  precio_unitario_historico DECIMAL(12, 2) NOT NULL CHECK (precio_unitario_historico >= 0),
  descuento DECIMAL(12, 2) NOT NULL DEFAULT 0,
  id_venta INT NOT NULL,
  id_producto INT NOT NULL,
  PRIMARY KEY (id_detalle_venta),
  FOREIGN KEY (id_venta) REFERENCES venta(id_venta),
  FOREIGN KEY (id_producto) REFERENCES producto(id_producto),
  UNIQUE (id_venta, id_producto)
);
GO

CREATE TABLE factura (
  id_factura INT IDENTITY(1,1) NOT NULL,
  tipo_comprobante VARCHAR(10) NOT NULL, 
  numero_factura VARCHAR(30) NOT NULL UNIQUE,
  cae_autorizacion VARCHAR(30), 
  fecha_emision DATE NOT NULL,
  monto_neto DECIMAL(12, 2) NOT NULL CHECK (monto_neto >= 0),
  iva_total DECIMAL(12, 2) NOT NULL CHECK (iva_total >= 0),
  total DECIMAL(12, 2) NOT NULL CHECK (total >= 0),
  id_venta INT NOT NULL UNIQUE,
  PRIMARY KEY (id_factura),
  FOREIGN KEY (id_venta) REFERENCES venta(id_venta)
);
GO

CREATE TABLE metodo_pago (
  id_metodo_pago INT IDENTITY(1,1) NOT NULL,
  nombre VARCHAR(50) NOT NULL UNIQUE,
  activo BIT NOT NULL DEFAULT 1,
  PRIMARY KEY (id_metodo_pago)
);
GO

CREATE TABLE pago (
  id_pago INT IDENTITY(1,1) NOT NULL,
  monto DECIMAL(12, 2) NOT NULL CHECK (monto > 0),
  referencia_transaccion VARCHAR(100),
  id_metodo_pago INT NOT NULL,
  id_venta INT NOT NULL,
  PRIMARY KEY (id_pago),
  FOREIGN KEY (id_metodo_pago) REFERENCES metodo_pago(id_metodo_pago),
  FOREIGN KEY (id_venta) REFERENCES venta(id_venta)
);
GO

-- ==========================================
-- 3. DATOS SEMILLA (Inserts Iniciales)
-- ==========================================

-- Roles
INSERT INTO rol (nombre, descripcion) VALUES 
('Administrador', 'Acceso total al sistema'),
('Cajero', 'Atención al punto de venta, cobro a clientes y manejo de caja.'),
('Encargado de Depósito', 'Control total del inventario, ingreso de mercadería y gestión de stock.'),
('Repositor', 'Consulta de stock y control de ubicación de productos en el salón.');
GO

-- Sucursal y Caja
INSERT INTO sucursal (nombre, direccion, activo) VALUES ('Casa Central', 'Calle Falsa 123', 1);
INSERT INTO caja (nombre_numero, id_sucursal, activo) VALUES ('Caja 1', 1, 1);
GO

-- Empleados (Con campo dirección completado)
INSERT INTO empleado (nombre, apellido, dni, email, direccion, telefono, password_hash, estado, id_rol, id_sucursal) 
VALUES ('Benja', 'Zimerman', '43064294', 'admin@stockos.com', 'Sin especificar', '3794562092', 'admin123', 1, 1, 1);

INSERT INTO empleado (nombre, apellido, dni, email, direccion, telefono, password_hash, estado, id_rol, id_sucursal) 
VALUES ('Saul', 'Arnica', '43205368', 'administrador@stockos.com', 'Sin especificar', '3794834167', 'admin123', 1, 1, 1);
GO

-- Métodos de Pago
INSERT INTO metodo_pago (nombre, activo) VALUES 
('Efectivo', 1),
('Tarjeta de Débito', 1),
('Tarjeta de Crédito', 1),
('Mercado Pago', 1);
GO

-- Categorías
INSERT INTO categoria (nombre, descripcion, activo) VALUES 
('Panaderia', 'Pan, facturas, medialunas y productos de panadería', 1),
('Bebidas', 'Gaseosas, aguas, jugos, cervezas y bebidas en general', 1),
('Carniceria', 'Carnes vacunas, porcinas, aves y derivados', 1),
('Verduleria', 'Frutas y verduras frescas', 1),
('Rotiseria', 'Comidas preparadas, empanadas y platos listos', 1),
('Fiambreria', 'Fiambres, embutidos y quesos', 1),
('Comestibles', 'Alimentos secos, conservas, condimentos y enlatados', 1),
('Lacteos', 'Leches, yogures, cremas y manteca', 1),
('Perfumeria', 'Perfumes, desodorantes y cosmeticos', 1),
('Limpieza', 'Productos de limpieza del hogar y superficies', 1),
('Higiene', 'Articulos de higiene personal: shampoo, jabon, cepillos', 1);
GO

-- Proveedor Predeterminado
SET IDENTITY_INSERT proveedor ON;
INSERT INTO proveedor (id_proveedor, razon_social, cuit, telefono, email, direccion, activo)
VALUES (1, 'Proveedor Predeterminado', '00-00000000-0', '0000000000', 'contacto@stockos.com', 'S/D', 1);
SET IDENTITY_INSERT proveedor OFF;
GO

-- ==========================================
-- 4. PROCEDIMIENTOS ALMACENADOS: USUARIOS
-- ==========================================

CREATE OR ALTER PROCEDURE sp_Usuarios_Insertar
    @Nombre VARCHAR(50), @Apellido VARCHAR(50), @Dni VARCHAR(20), @Email VARCHAR(100),
    @Direccion VARCHAR(255), @Telefono VARCHAR(30), @PasswordHash VARCHAR(255),
    @IdRol INT, @IdSucursal INT, @IdEmpleado INT OUTPUT
AS
BEGIN
    INSERT INTO empleado (nombre, apellido, dni, email, direccion, telefono, password_hash, estado, id_rol, id_sucursal)
    VALUES (@Nombre, @Apellido, @Dni, @Email, @Direccion, @Telefono, @PasswordHash, 1, @IdRol, @IdSucursal);
    SET @IdEmpleado = SCOPE_IDENTITY();
END
GO

CREATE OR ALTER PROCEDURE sp_Usuarios_Autenticar
    @Dni VARCHAR(20)
AS
BEGIN
    SELECT id_empleado, nombre, apellido, dni, email, direccion, telefono, password_hash, estado, id_rol, id_sucursal
    FROM empleado WHERE dni = @Dni;
END
GO

CREATE OR ALTER PROCEDURE sp_Usuarios_Actualizar
    @IdEmpleado INT, @Nombre VARCHAR(50), @Apellido VARCHAR(50), @Dni VARCHAR(20),
    @Email VARCHAR(100), @Direccion VARCHAR(255), @Telefono VARCHAR(30),
    @IdRol INT, @IdSucursal INT, @Estado BIT, @PasswordHash VARCHAR(255)
AS
BEGIN
    UPDATE empleado
    SET nombre = @Nombre, apellido = @Apellido, dni = @Dni, email = @Email, 
        direccion = @Direccion, telefono = @Telefono, id_rol = @IdRol, 
        id_sucursal = @IdSucursal, estado = @Estado,
        password_hash = COALESCE(@PasswordHash, password_hash)
    WHERE id_empleado = @IdEmpleado;
END
GO

CREATE OR ALTER PROCEDURE sp_Usuarios_CambiarEstado
    @IdEmpleado INT, @Estado BIT
AS
BEGIN
    UPDATE empleado SET estado = @Estado WHERE id_empleado = @IdEmpleado;
END
GO

CREATE OR ALTER PROCEDURE sp_Usuarios_ConsultarEstado
    @Dni VARCHAR(20), @Estado BIT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @Estado = NULL;
    SELECT @Estado = estado FROM empleado WHERE dni = @Dni;
END
GO

-- ==========================================
-- 5. PROCEDIMIENTOS ALMACENADOS: CATEGORIAS
-- ==========================================

CREATE OR ALTER PROCEDURE sp_Categorias_ObtenerTodas
AS
BEGIN
    SELECT id_categoria, nombre, descripcion, activo FROM categoria WHERE activo = 1;
END
GO

CREATE OR ALTER PROCEDURE sp_Categorias_Insertar
    @Nombre NVARCHAR(100), @Descripcion NVARCHAR(255), @IdCategoria INT OUTPUT
AS
BEGIN
    INSERT INTO categoria (nombre, descripcion, activo) VALUES (@Nombre, @Descripcion, 1);
    SET @IdCategoria = SCOPE_IDENTITY();
END
GO

CREATE OR ALTER PROCEDURE sp_Categorias_Actualizar
    @IdCategoria INT, @Nombre NVARCHAR(100), @Descripcion NVARCHAR(255), @Activo BIT
AS
BEGIN
    UPDATE categoria SET nombre = @Nombre, descripcion = @Descripcion, activo = @Activo WHERE id_categoria = @IdCategoria;
END
GO

-- ==========================================
-- 6. PROCEDIMIENTOS ALMACENADOS: PRODUCTOS
-- ==========================================

CREATE OR ALTER PROCEDURE sp_Productos_Insertar
    @CodigoBarra VARCHAR(50), @Nombre VARCHAR(100), @Descripcion VARCHAR(255),
    @PrecioVentaActual DECIMAL(12,2), @PorcentajeIva DECIMAL(5,2),
    @IdCategoria INT, @IdProveedor INT = NULL, @IdProducto INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO producto (codigo_barra, nombre, descripcion, precio_venta_actual, porcentaje_iva, id_categoria, id_proveedor, activo)
    VALUES (@CodigoBarra, @Nombre, @Descripcion, @PrecioVentaActual, @PorcentajeIva, @IdCategoria, @IdProveedor, 1);
    SET @IdProducto = SCOPE_IDENTITY();
END
GO

CREATE OR ALTER PROCEDURE sp_Productos_ObtenerTodos
AS
BEGIN
    SET NOCOUNT ON;
    SELECT id_producto, codigo_barra, nombre, descripcion, precio_venta_actual, porcentaje_iva, id_categoria, id_proveedor, activo
    FROM producto;
END
GO

CREATE OR ALTER PROCEDURE sp_Productos_Actualizar
    @IdProducto INT, @CodigoBarra VARCHAR(50), @Nombre VARCHAR(100), @Descripcion VARCHAR(255),
    @PrecioVentaActual DECIMAL(12,2), @PorcentajeIva DECIMAL(5,2), @IdCategoria INT,
    @Activo BIT, @IdProveedor INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE producto
    SET codigo_barra = @CodigoBarra, nombre = @Nombre, descripcion = @Descripcion,
        precio_venta_actual = @PrecioVentaActual, porcentaje_iva = @PorcentajeIva,
        id_categoria = @IdCategoria, id_proveedor = @IdProveedor, activo = @Activo
    WHERE id_producto = @IdProducto;
END
GO

CREATE OR ALTER PROCEDURE sp_Productos_CambiarEstado
    @IdProducto INT, @Activo BIT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE producto SET activo = @Activo WHERE id_producto = @IdProducto;
END
GO

-- ==========================================
-- 7. PROCEDIMIENTOS ALMACENADOS: PROVEEDORES
-- ==========================================

CREATE OR ALTER PROCEDURE sp_Proveedores_ObtenerTodos
AS
BEGIN
    SET NOCOUNT ON;
    SELECT id_proveedor, razon_social, cuit, telefono, email, direccion, activo FROM proveedor ORDER BY razon_social ASC;
END;
GO

CREATE OR ALTER PROCEDURE sp_Proveedores_Insertar
    @RazonSocial VARCHAR(100), @Cuit VARCHAR(30) = NULL, @Telefono VARCHAR(30) = '',
    @Email VARCHAR(100) = '', @Direccion VARCHAR(200) = '', @Activo BIT = 1, @IdProveedor INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO proveedor (razon_social, cuit, telefono, email, direccion, activo)
    VALUES (@RazonSocial, NULLIF(LTRIM(RTRIM(@Cuit)), ''), ISNULL(@Telefono, ''), ISNULL(@Email, ''), ISNULL(@Direccion, ''), @Activo);
    SET @IdProveedor = SCOPE_IDENTITY();
END;
GO

CREATE OR ALTER PROCEDURE sp_Proveedores_Actualizar
    @IdProveedor INT, @RazonSocial VARCHAR(100), @Cuit VARCHAR(30) = NULL, @Telefono VARCHAR(30) = '',
    @Email VARCHAR(100) = '', @Direccion VARCHAR(200) = '', @Activo BIT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE proveedor SET razon_social = @RazonSocial, cuit = NULLIF(LTRIM(RTRIM(@Cuit)), ''),
        telefono = ISNULL(@Telefono, ''), email = ISNULL(@Email, ''), direccion = ISNULL(@Direccion, ''), activo = @Activo
    WHERE id_proveedor = @IdProveedor;
END;
GO

CREATE OR ALTER PROCEDURE sp_Proveedores_CambiarEstado
    @IdProveedor INT, @Activo BIT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE proveedor SET activo = @Activo WHERE id_proveedor = @IdProveedor;
END;
GO

-- ==========================================
-- 8. PROCEDIMIENTOS ALMACENADOS: COMPRAS
-- ==========================================

CREATE OR ALTER PROCEDURE sp_Compras_Insertar
    @FechaHora DATETIME2, @Total DECIMAL(12,2), @NumeroComprobante VARCHAR(50) = NULL,
    @IdProveedor INT, @IdEmpleado INT, @IdSucursal INT, @IdCompra INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO compra (fecha_hora, total, numero_comprobante, id_proveedor, id_empleado, id_sucursal)
    VALUES (@FechaHora, @Total, @NumeroComprobante, @IdProveedor, @IdEmpleado, @IdSucursal);
    SET @IdCompra = SCOPE_IDENTITY();
END
GO

CREATE OR ALTER PROCEDURE sp_DetalleCompra_Insertar
    @IdCompra INT, @IdProducto INT, @Cantidad INT, @PrecioUnitarioCompra DECIMAL(12,2), @IdDetalleCompra INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO detalle_compra (id_compra, id_producto, cantidad, precio_unitario_compra)
    VALUES (@IdCompra, @IdProducto, @Cantidad, @PrecioUnitarioCompra);
    SET @IdDetalleCompra = SCOPE_IDENTITY();
END
GO

CREATE OR ALTER PROCEDURE sp_Compras_ObtenerUltimoPrecio
    @IdProducto INT, @PrecioCompra DECIMAL(12,2) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @PrecioCompra = NULL;
    SELECT TOP 1 @PrecioCompra = dc.precio_unitario_compra FROM detalle_compra dc
    INNER JOIN compra c ON dc.id_compra = c.id_compra WHERE dc.id_producto = @IdProducto
    ORDER BY c.fecha_hora DESC, dc.id_detalle_compra DESC;
END
GO

CREATE OR ALTER PROCEDURE sp_Compras_ActualizarPrecio
    @IdProducto INT, @NuevoPrecioCompra DECIMAL(12,2)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @IdDetalle INT; DECLARE @IdCompra INT;
    SELECT TOP 1 @IdDetalle = dc.id_detalle_compra, @IdCompra = dc.id_compra FROM detalle_compra dc
    INNER JOIN compra c ON dc.id_compra = c.id_compra WHERE dc.id_producto = @IdProducto
    ORDER BY c.fecha_hora DESC, dc.id_detalle_compra DESC;
    IF @IdDetalle IS NOT NULL
    BEGIN
        UPDATE detalle_compra SET precio_unitario_compra = @NuevoPrecioCompra WHERE id_detalle_compra = @IdDetalle;
        UPDATE compra SET total = (SELECT ISNULL(SUM(cantidad * precio_unitario_compra), 0) FROM detalle_compra WHERE id_compra = @IdCompra)
        WHERE id_compra = @IdCompra;
    END
END
GO

-- ==========================================
-- 9. PROCEDIMIENTOS ALMACENADOS: STOCK
-- ==========================================

CREATE OR ALTER PROCEDURE sp_Stock_ObtenerActual
    @IdProducto INT, @IdSucursal INT, @StockActual INT OUTPUT
AS
BEGIN
    SELECT @StockActual = stock_actual FROM stock_sucursal WHERE id_producto = @IdProducto AND id_sucursal = @IdSucursal;
    IF @StockActual IS NULL SET @StockActual = 0;
END
GO

CREATE OR ALTER PROCEDURE sp_Stock_IngresarMercaderia
    @IdProducto INT, @IdSucursal INT, @CantidadAIngresar INT
AS
BEGIN
    IF EXISTS (SELECT 1 FROM stock_sucursal WHERE id_producto = @IdProducto AND id_sucursal = @IdSucursal)
    BEGIN
        UPDATE stock_sucursal SET stock_actual = stock_actual + @CantidadAIngresar
        WHERE id_producto = @IdProducto AND id_sucursal = @IdSucursal;
    END
    ELSE
    BEGIN
        INSERT INTO stock_sucursal (id_producto, id_sucursal, stock_actual, stock_minimo)
        VALUES (@IdProducto, @IdSucursal, @CantidadAIngresar, 5);
    END
END
GO

CREATE OR ALTER PROCEDURE sp_Stock_Descontar
    @IdProducto INT, @IdSucursal INT, @CantidadAVender INT
AS
BEGIN
    UPDATE stock_sucursal SET stock_actual = stock_actual - @CantidadAVender
    WHERE id_producto = @IdProducto AND id_sucursal = @IdSucursal;
END
GO

-- ==========================================
-- 10. PROCEDIMIENTOS ALMACENADOS: CAJA
-- ==========================================

CREATE OR ALTER PROCEDURE sp_Cajas_ObtenerPorSucursal
    @IdSucursal INT
AS
BEGIN
    SELECT id_caja, nombre_numero, id_sucursal, activo FROM caja WHERE id_sucursal = @IdSucursal AND activo = 1;
END
GO

CREATE OR ALTER PROCEDURE sp_CajaSesion_Abrir
    @IdCaja INT, @IdEmpleado INT, @MontoApertura DECIMAL(12,2), @IdCajaSesion INT OUTPUT
AS
BEGIN
    INSERT INTO caja_sesion (monto_apertura, estado, id_caja, id_empleado)
    VALUES (@MontoApertura, 1, @IdCaja, @IdEmpleado);
    SET @IdCajaSesion = SCOPE_IDENTITY();
END
GO

CREATE OR ALTER PROCEDURE sp_CajaSesion_VerificarAbierta
    @IdEmpleado INT, @EstaAbierta BIT OUTPUT
AS
BEGIN
    IF EXISTS (SELECT 1 FROM caja_sesion WHERE id_empleado = @IdEmpleado AND estado = 1) SET @EstaAbierta = 1;
    ELSE SET @EstaAbierta = 0;
END
GO

CREATE OR ALTER PROCEDURE sp_MovimientoCaja_Insertar
    @IdCajaSesion INT, @Tipo NVARCHAR(50), @Monto DECIMAL(12,2), @Descripcion NVARCHAR(255)
AS
BEGIN
    INSERT INTO movimiento_caja (id_caja_sesion, fecha_hora, tipo_movimiento, monto, descripcion)
    VALUES (@IdCajaSesion, SYSDATETIME(), @Tipo, @Monto, @Descripcion);
END
GO

CREATE OR ALTER PROCEDURE sp_Caja_CalcularMontoEsperado
    @IdCajaSesion INT,
    @MontoEsperado DECIMAL(12,2) OUTPUT
AS
BEGIN
    DECLARE @Apertura DECIMAL(12,2) = 0;
    DECLARE @Ventas DECIMAL(12,2) = 0;
    DECLARE @Ingresos DECIMAL(12,2) = 0;
    DECLARE @Egresos DECIMAL(12,2) = 0;

    SELECT @Apertura = ISNULL(monto_apertura, 0) FROM caja_sesion WHERE id_caja_sesion = @IdCajaSesion;

    SELECT @Ventas = ISNULL(SUM(p.monto), 0)
    FROM pago p
    INNER JOIN venta v ON p.id_venta = v.id_venta
    WHERE v.id_caja_sesion = @IdCajaSesion AND p.id_metodo_pago = 1;

    SELECT @Ingresos = ISNULL(SUM(monto), 0) FROM movimiento_caja WHERE id_caja_sesion = @IdCajaSesion AND tipo_movimiento = 'Ingreso';
    SELECT @Egresos = ISNULL(SUM(monto), 0) FROM movimiento_caja WHERE id_caja_sesion = @IdCajaSesion AND tipo_movimiento = 'Egreso';

    SET @MontoEsperado = @Apertura + @Ventas + @Ingresos - @Egresos;
END
GO

CREATE OR ALTER PROCEDURE sp_CajaSesion_Cerrar
    @IdCajaSesion INT,
    @MontoCierreReal DECIMAL(12,2)
AS
BEGIN
    DECLARE @MontoEsperado DECIMAL(12,2);

    EXEC sp_Caja_CalcularMontoEsperado @IdCajaSesion, @MontoEsperado OUTPUT;

    DECLARE @Diferencia DECIMAL(12,2) = @MontoCierreReal - @MontoEsperado;

    UPDATE caja_sesion
    SET 
        fecha_cierre = SYSDATETIME(),
        monto_cierre_real = @MontoCierreReal,
        diferencia_cierre = @Diferencia,
        estado = 0 
    WHERE id_caja_sesion = @IdCajaSesion;
END
GO

-- ==========================================
-- 11. PROCEDIMIENTOS ALMACENADOS: VENTAS
-- ==========================================

CREATE OR ALTER PROCEDURE sp_Ventas_Insertar
    @Subtotal DECIMAL(12,2), @DescuentoTotal DECIMAL(12,2), @TotalVenta DECIMAL(12,2),
    @IdCajaSesion INT, @IdCliente INT = NULL, @IdVenta INT OUTPUT
AS
BEGIN
    INSERT INTO venta (subtotal, descuento_total, total_venta, estado, id_caja_sesion, id_cliente)
    VALUES (@Subtotal, @DescuentoTotal, @TotalVenta, 1, @IdCajaSesion, @IdCliente);
    SET @IdVenta = SCOPE_IDENTITY();
END
GO

CREATE OR ALTER PROCEDURE sp_DetalleVenta_Insertar
    @Cantidad INT, @PrecioUnitario DECIMAL(12,2), @Descuento DECIMAL(12,2),
    @IdVenta INT, @IdProducto INT
AS
BEGIN
    INSERT INTO detalle_venta (cantidad, precio_unitario_historico, descuento, id_venta, id_producto)
    VALUES (@Cantidad, @PrecioUnitario, @Descuento, @IdVenta, @IdProducto);
END
GO

CREATE OR ALTER PROCEDURE sp_Pago_Insertar
    @Monto DECIMAL(12,2),
    @IdMetodoPago INT,
    @IdVenta INT,
    @ReferenciaTransaccion VARCHAR(100) = NULL,
    @IdPago INT OUTPUT
AS
BEGIN
    INSERT INTO pago (monto, referencia_transaccion, id_metodo_pago, id_venta)
    VALUES (@Monto, @ReferenciaTransaccion, @IdMetodoPago, @IdVenta);
    
    SET @IdPago = SCOPE_IDENTITY();
END
GO