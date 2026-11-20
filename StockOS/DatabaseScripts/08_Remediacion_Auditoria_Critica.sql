-- =========================================================================================
-- SCRIPT 08: REMEDIACIÓN DE AUDITORÍA CRÍTICA (INTEGRIDAD Y CONCURRENCIA)
-- Proyecto: StockOS (.NET 8 - SQL Server 2022)
-- Base de Datos: StockOS
-- Descripción: 
--   1. Añade restricción CHECK (stock_actual >= 0) en stock_sucursal contra stock negativo.
--   2. Corrige condición de carrera en sp_Stock_Descontar mediante actualización condicional atómica.
--   3. Corrige distorsión en sp_Caja_CalcularMontoEsperado filtrando solo ventas activas (v.estado = 1).
-- =========================================================================================

USE StockOS;
GO

-- =========================================================================================
-- 1. RESTRICCIÓN DE INTEGRIDAD: PREVENCIÓN DE STOCK NEGATIVO
-- =========================================================================================
IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CK_stock_actual_positivo')
BEGIN
    PRINT 'Agregando restricción CHECK en stock_sucursal (stock_actual >= 0)...';
    ALTER TABLE stock_sucursal 
    ADD CONSTRAINT CK_stock_actual_positivo CHECK (stock_actual >= 0);
END
ELSE
BEGIN
    PRINT 'La restricción CK_stock_actual_positivo ya existe.';
END
GO

-- =========================================================================================
-- 2. PROCEDIMIENTO sp_Stock_Descontar (ATÓMICO Y LIBRE DE RACE CONDITIONS)
-- =========================================================================================
CREATE OR ALTER PROCEDURE sp_Stock_Descontar
    @IdProducto INT, 
    @IdSucursal INT, 
    @CantidadAVender INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Actualización condicional atómica: previene sobreventa concurrente entre múltiples terminales
    UPDATE stock_sucursal 
    SET stock_actual = stock_actual - @CantidadAVender
    WHERE id_producto = @IdProducto 
      AND id_sucursal = @IdSucursal 
      AND stock_actual >= @CantidadAVender;

    IF @@ROWCOUNT = 0
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM stock_sucursal WHERE id_producto = @IdProducto AND id_sucursal = @IdSucursal)
            THROW 50001, 'El producto no cuenta con registro de inventario en la sucursal seleccionada.', 1;
        ELSE
            THROW 50002, 'Stock insuficiente para descontar la venta. La cantidad solicitada supera las existencias.', 1;
    END
END
GO
PRINT 'Procedimiento sp_Stock_Descontar actualizado exitosamente con control de concurrencia atómico.';
GO

-- =========================================================================================
-- 3. PROCEDIMIENTO sp_Caja_CalcularMontoEsperado (FILTRO DE VENTAS ACTIVAS ESTADO = 1)
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
    -- Regla mandatoria: v.estado = 1 (Ignorar pagos de ventas canceladas/anuladas)
    SELECT @Ventas = ISNULL(SUM(p.monto), 0)
    FROM pago p
    INNER JOIN venta v ON p.id_venta = v.id_venta
    WHERE v.id_caja_sesion = @IdCajaSesion 
      AND p.id_metodo_pago = 1
      AND v.estado = 1;

    -- Movimientos manuales de caja (normalizado a mayúsculas estrictas para Case-Sensitive)
    SELECT @Ingresos = ISNULL(SUM(monto), 0) 
    FROM movimiento_caja 
    WHERE id_caja_sesion = @IdCajaSesion AND UPPER(tipo_movimiento) = 'INGRESO';

    SELECT @Egresos = ISNULL(SUM(monto), 0) 
    FROM movimiento_caja 
    WHERE id_caja_sesion = @IdCajaSesion AND UPPER(tipo_movimiento) = 'EGRESO';

    SET @MontoEsperado = @Apertura + @Ventas + @Ingresos - @Egresos;
END
GO
PRINT 'Procedimiento sp_Caja_CalcularMontoEsperado actualizado exitosamente con filtro de ventas activas.';
GO
