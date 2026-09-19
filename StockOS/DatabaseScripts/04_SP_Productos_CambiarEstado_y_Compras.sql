USE StockOS;
GO

-- =============================================================
-- Migración 04: Estado de Productos y Persistencia de Compras
-- =============================================================

-- 1. Proveedor Predeterminado (para satisfacer la FK obligatoria sin requerir gestión de proveedor)
IF NOT EXISTS (SELECT 1 FROM proveedor WHERE id_proveedor = 1)
BEGIN
    SET IDENTITY_INSERT proveedor ON;
    INSERT INTO proveedor (id_proveedor, razon_social, cuit, telefono, email, direccion, activo)
    VALUES (1, 'Proveedor Predeterminado', '00-00000000-0', '0000000000', 'contacto@stockos.com', 'S/D', 1);
    SET IDENTITY_INSERT proveedor OFF;
END
GO

-- 2. Procedimiento: Cambiar Estado de Producto (Activo / Inactivo)
CREATE OR ALTER PROCEDURE sp_Productos_CambiarEstado
    @IdProducto INT,
    @Activo BIT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE producto
    SET activo = @Activo
    WHERE id_producto = @IdProducto;
END
GO

-- 3. Procedimiento: Insertar Cabecera de Compra
CREATE OR ALTER PROCEDURE sp_Compras_Insertar
    @FechaHora DATETIME2,
    @Total DECIMAL(12,2),
    @NumeroComprobante VARCHAR(50) = NULL,
    @IdProveedor INT,
    @IdEmpleado INT,
    @IdSucursal INT,
    @IdCompra INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO compra (fecha_hora, total, numero_comprobante, id_proveedor, id_empleado, id_sucursal)
    VALUES (@FechaHora, @Total, @NumeroComprobante, @IdProveedor, @IdEmpleado, @IdSucursal);

    SET @IdCompra = SCOPE_IDENTITY();
END
GO

-- 4. Procedimiento: Insertar Detalle de Compra
CREATE OR ALTER PROCEDURE sp_DetalleCompra_Insertar
    @IdCompra INT,
    @IdProducto INT,
    @Cantidad INT,
    @PrecioUnitarioCompra DECIMAL(12,2),
    @IdDetalleCompra INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO detalle_compra (id_compra, id_producto, cantidad, precio_unitario_compra)
    VALUES (@IdCompra, @IdProducto, @Cantidad, @PrecioUnitarioCompra);

    SET @IdDetalleCompra = SCOPE_IDENTITY();
END
GO

-- 5. Procedimiento: Obtener Último Precio de Compra por Producto
CREATE OR ALTER PROCEDURE sp_Compras_ObtenerUltimoPrecio
    @IdProducto INT,
    @PrecioCompra DECIMAL(12,2) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @PrecioCompra = NULL;

    SELECT TOP 1 @PrecioCompra = dc.precio_unitario_compra
    FROM detalle_compra dc
    INNER JOIN compra c ON dc.id_compra = c.id_compra
    WHERE dc.id_producto = @IdProducto
    ORDER BY c.fecha_hora DESC, dc.id_detalle_compra DESC;
END
GO

-- 6. Procedimiento: Actualizar Precio de Compra de un Producto
CREATE OR ALTER PROCEDURE sp_Compras_ActualizarPrecio
    @IdProducto INT,
    @NuevoPrecioCompra DECIMAL(12,2)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @IdDetalle INT;
    DECLARE @IdCompra INT;

    SELECT TOP 1 
        @IdDetalle = dc.id_detalle_compra,
        @IdCompra = dc.id_compra
    FROM detalle_compra dc
    INNER JOIN compra c ON dc.id_compra = c.id_compra
    WHERE dc.id_producto = @IdProducto
    ORDER BY c.fecha_hora DESC, dc.id_detalle_compra DESC;

    IF @IdDetalle IS NOT NULL
    BEGIN
        UPDATE detalle_compra
        SET precio_unitario_compra = @NuevoPrecioCompra
        WHERE id_detalle_compra = @IdDetalle;

        -- Mantener congruencia recalculando el total en la cabecera de la compra
        UPDATE compra
        SET total = (SELECT ISNULL(SUM(cantidad * precio_unitario_compra), 0) FROM detalle_compra WHERE id_compra = @IdCompra)
        WHERE id_compra = @IdCompra;
    END
END
GO

