USE StockOS;
GO

-- ==========================================
-- 1. Insertar Cabecera de la Venta
-- ==========================================
CREATE OR ALTER PROCEDURE sp_Ventas_Insertar
    @Subtotal DECIMAL(12,2),
    @DescuentoTotal DECIMAL(12,2),
    @TotalVenta DECIMAL(12,2),
    @IdCajaSesion INT,
    @IdCliente INT = NULL, -- Puede ser NULL si es consumidor final sin registrar
    @IdVenta INT OUTPUT
AS
BEGIN
    INSERT INTO venta (subtotal, descuento_total, total_venta, estado, id_caja_sesion, id_cliente)
    VALUES (@Subtotal, @DescuentoTotal, @TotalVenta, 1, @IdCajaSesion, @IdCliente);
    
    SET @IdVenta = SCOPE_IDENTITY();
END
GO

-- ==========================================
-- 2. Insertar Detalle de Venta
-- ==========================================
CREATE OR ALTER PROCEDURE sp_DetalleVenta_Insertar
    @Cantidad INT,
    @PrecioUnitario DECIMAL(12,2),
    @Descuento DECIMAL(12,2),
    @IdVenta INT,
    @IdProducto INT
AS
BEGIN
    INSERT INTO detalle_venta (cantidad, precio_unitario_historico, descuento, id_venta, id_producto)
    VALUES (@Cantidad, @PrecioUnitario, @Descuento, @IdVenta, @IdProducto);
END
GO

-- ==========================================
-- 3. Descontar Stock (Vital para la venta)
-- ==========================================
CREATE OR ALTER PROCEDURE sp_Stock_Descontar
    @IdProducto INT,
    @IdSucursal INT,
    @CantidadAVender INT
AS
BEGIN
    -- Restamos la cantidad vendida del stock actual
    UPDATE stock_sucursal
    SET stock_actual = stock_actual - @CantidadAVender
    WHERE id_producto = @IdProducto AND id_sucursal = @IdSucursal;
END
GO