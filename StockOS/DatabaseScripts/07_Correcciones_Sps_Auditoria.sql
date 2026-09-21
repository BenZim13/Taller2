-- =========================================================================================
-- SCRIPT DE CORRECCIÓN Y AUDITORÍA: PROCEDIMIENTOS ALMACENADOS - StockOS
-- Fecha: 20 de Septiembre de 2026
-- Descripción: Script con correcciones críticas de trazabilidad, control de stock negativo,
--              inmutabilidad de comprobantes históricos y normalización de mayúsculas en caja.
-- =========================================================================================

USE StockOS;
GO

-- =========================================================================================
-- 1. CONTROL DE STOCK EN VENTAS: PREVENCIÓN DE STOCK NEGATIVO
-- =========================================================================================
-- Se añade validación de existencia previa antes de descontar. Si el stock es insuficiente,
-- se interrumpe la transacción y se arroja un error controlado (código 50002).
-- =========================================================================================
CREATE OR ALTER PROCEDURE sp_Stock_Descontar
    @IdProducto INT, 
    @IdSucursal INT, 
    @CantidadAVender INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @StockActual INT;

    SELECT @StockActual = stock_actual 
    FROM stock_sucursal 
    WHERE id_producto = @IdProducto AND id_sucursal = @IdSucursal;

    IF @StockActual IS NULL
    BEGIN
        THROW 50001, 'El producto no cuenta con registro de inventario en la sucursal seleccionada.', 1;
    END

    IF @StockActual < @CantidadAVender
    BEGIN
        THROW 50002, 'Stock insuficiente para descontar la venta. La cantidad solicitada supera las existencias.', 1;
    END

    UPDATE stock_sucursal 
    SET stock_actual = stock_actual - @CantidadAVender
    WHERE id_producto = @IdProducto AND id_sucursal = @IdSucursal;
END
GO

-- =========================================================================================
-- 2. ARQUEO DE CAJA: NORMALIZACIÓN DE CASE-SENSITIVITY EN MOVIMIENTOS
-- =========================================================================================
-- La tabla movimiento_caja requiere CHECK ('INGRESO', 'EGRESO'). Se normaliza la búsqueda
-- utilizando UPPER(tipo_movimiento) y mayúsculas estrictas para compatibilidad total
-- con colaciones Case-Sensitive (CS) en SQL Server.
-- =========================================================================================
CREATE OR ALTER PROCEDURE sp_Caja_CalcularMontoEsperado
    @IdCajaSesion INT,
    @MontoEsperado DECIMAL(12, 2) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Apertura DECIMAL(12, 2) = 0;
    DECLARE @Ventas DECIMAL(12, 2) = 0;
    DECLARE @Ingresos DECIMAL(12, 2) = 0;
    DECLARE @Egresos DECIMAL(12, 2) = 0;

    -- Saldo inicial con el que abrió la caja
    SELECT @Apertura = ISNULL(monto_apertura, 0) 
    FROM caja_sesion 
    WHERE id_caja_sesion = @IdCajaSesion;

    -- Ventas cobradas en efectivo (id_metodo_pago = 1) en esta sesión
    SELECT @Ventas = ISNULL(SUM(p.monto), 0)
    FROM pago p
    INNER JOIN venta v ON p.id_venta = v.id_venta
    WHERE v.id_caja_sesion = @IdCajaSesion AND p.id_metodo_pago = 1;

    -- Movimientos manuales de caja (normalizado a mayúsculas estrictas)
    SELECT @Ingresos = ISNULL(SUM(monto), 0) 
    FROM movimiento_caja 
    WHERE id_caja_sesion = @IdCajaSesion AND UPPER(tipo_movimiento) = 'INGRESO';

    SELECT @Egresos = ISNULL(SUM(monto), 0) 
    FROM movimiento_caja 
    WHERE id_caja_sesion = @IdCajaSesion AND UPPER(tipo_movimiento) = 'EGRESO';

    SET @MontoEsperado = @Apertura + @Ventas + @Ingresos - @Egresos;
END
GO

-- =========================================================================================
-- 3. INMUTABILIDAD DE COMPROBANTES HISTÓRICOS DE COMPRA
-- =========================================================================================
-- Las facturas y comprobantes históricos de compras deben ser inmutables por trazabilidad
-- fiscal y de auditoría contable. La modificación del catálogo no debe alterar comprobantes pasados.
-- =========================================================================================
CREATE OR ALTER PROCEDURE sp_Compras_ActualizarPrecio
    @IdProducto INT, 
    @NuevoPrecioCompra DECIMAL(12,2)
AS
BEGIN
    SET NOCOUNT ON;
    -- Por principio de trazabilidad contable, los registros históricos de compras emitidos
    -- en el pasado permanecen inmutables. El costo de reposición se registra en cada compra nueva.
    RETURN;
END
GO

