USE StockOS;
GO

-- 1. Insertar Producto
CREATE OR ALTER PROCEDURE sp_Productos_Insertar
    @CodigoBarra VARCHAR(50),
    @Nombre VARCHAR(100),
    @Descripcion VARCHAR(255),
    @PrecioVentaActual DECIMAL(12,2),
    @PorcentajeIva DECIMAL(5,2),
    @IdCategoria INT,
    @IdProducto INT OUTPUT
AS
BEGIN
    INSERT INTO producto (codigo_barra, nombre, descripcion, precio_venta_actual, porcentaje_iva, id_categoria, activo)
    VALUES (@CodigoBarra, @Nombre, @Descripcion, @PrecioVentaActual, @PorcentajeIva, @IdCategoria, 1);
    
    SET @IdProducto = SCOPE_IDENTITY();
END
GO

-- 2. Obtener Todos los Productos
CREATE OR ALTER PROCEDURE sp_Productos_ObtenerTodos
AS
BEGIN
    SELECT id_producto, codigo_barra, nombre, descripcion, 
           precio_venta_actual, porcentaje_iva, id_categoria, activo
    FROM producto;
END
GO

-- 3. Actualizar Producto
CREATE OR ALTER PROCEDURE sp_Productos_Actualizar
    @IdProducto INT,
    @CodigoBarra VARCHAR(50),
    @Nombre VARCHAR(100),
    @Descripcion VARCHAR(255),
    @PrecioVentaActual DECIMAL(12,2),
    @PorcentajeIva DECIMAL(5,2),
    @IdCategoria INT,
    @Activo BIT
AS
BEGIN
    UPDATE producto
    SET codigo_barra = @CodigoBarra,
        nombre = @Nombre,
        descripcion = @Descripcion,
        precio_venta_actual = @PrecioVentaActual,
        porcentaje_iva = @PorcentajeIva,
        id_categoria = @IdCategoria,
        activo = @Activo
    WHERE id_producto = @IdProducto;
END
GO