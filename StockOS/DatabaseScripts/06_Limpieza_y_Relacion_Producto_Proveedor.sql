USE StockOS;
GO

-- ============================================================================
-- 1. LIMPIEZA DE PRODUCTOS Y PROVEEDORES INSTANCIADOS
-- ============================================================================

-- Tablas dependientes de producto y compra/venta
DELETE FROM detalle_venta;
DELETE FROM pago;
DELETE FROM venta;
DELETE FROM detalle_compra;
DELETE FROM compra;
DELETE FROM stock_sucursal;

-- Eliminar productos existentes
DELETE FROM producto;

-- Eliminar proveedores existentes
DELETE FROM proveedor;

-- Reiniciar identificadores IDENTITY si es necesario
DBCC CHECKIDENT ('producto', RESEED, 0);
DBCC CHECKIDENT ('proveedor', RESEED, 0);
DBCC CHECKIDENT ('compra', RESEED, 0);
DBCC CHECKIDENT ('detalle_compra', RESEED, 0);
GO

-- ============================================================================
-- 2. AGREGAR COLUMNA id_proveedor A LA TABLA producto (SI NO EXISTE)
-- ============================================================================
IF NOT EXISTS (
    SELECT 1 
    FROM sys.columns 
    WHERE object_id = OBJECT_ID('producto') 
      AND name = 'id_proveedor'
)
BEGIN
    ALTER TABLE producto 
    ADD id_proveedor INT NULL 
    CONSTRAINT FK_producto_proveedor FOREIGN KEY REFERENCES proveedor(id_proveedor);
END
GO

-- ============================================================================
-- 3. ACTUALIZAR PROCEDIMIENTOS ALMACENADOS DE PRODUCTOS
-- ============================================================================

-- A. Insertar Producto con Proveedor
CREATE OR ALTER PROCEDURE sp_Productos_Insertar
    @CodigoBarra VARCHAR(50),
    @Nombre VARCHAR(100),
    @Descripcion VARCHAR(255),
    @PrecioVentaActual DECIMAL(12,2),
    @PorcentajeIva DECIMAL(5,2),
    @IdCategoria INT,
    @IdProveedor INT = NULL,
    @IdProducto INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO producto (
        codigo_barra, 
        nombre, 
        descripcion, 
        precio_venta_actual, 
        porcentaje_iva, 
        id_categoria, 
        id_proveedor, 
        activo
    )
    VALUES (
        @CodigoBarra, 
        @Nombre, 
        @Descripcion, 
        @PrecioVentaActual, 
        @PorcentajeIva, 
        @IdCategoria, 
        @IdProveedor, 
        1
    );
    
    SET @IdProducto = SCOPE_IDENTITY();
END
GO

-- B. Obtener Todos los Productos (incluye id_proveedor)
CREATE OR ALTER PROCEDURE sp_Productos_ObtenerTodos
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        id_producto, 
        codigo_barra, 
        nombre, 
        descripcion, 
        precio_venta_actual, 
        porcentaje_iva, 
        id_categoria, 
        id_proveedor, 
        activo
    FROM producto;
END
GO

-- C. Actualizar Producto con Proveedor
CREATE OR ALTER PROCEDURE sp_Productos_Actualizar
    @IdProducto INT,
    @CodigoBarra VARCHAR(50),
    @Nombre VARCHAR(100),
    @Descripcion VARCHAR(255),
    @PrecioVentaActual DECIMAL(12,2),
    @PorcentajeIva DECIMAL(5,2),
    @IdCategoria INT,
    @Activo BIT,
    @IdProveedor INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE producto
    SET codigo_barra        = @CodigoBarra,
        nombre              = @Nombre,
        descripcion         = @Descripcion,
        precio_venta_actual = @PrecioVentaActual,
        porcentaje_iva      = @PorcentajeIva,
        id_categoria        = @IdCategoria,
        id_proveedor        = @IdProveedor,
        activo              = @Activo
    WHERE id_producto = @IdProducto;
END
GO

