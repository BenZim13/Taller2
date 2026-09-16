USE StockOS;
GO

-- ==========================================
-- MÓDULO: CATEGORÍAS
-- ==========================================

-- Obtener todas las categorías activas (Para el ComboBox)
CREATE OR ALTER PROCEDURE sp_Categorias_ObtenerTodas
AS
BEGIN
    SELECT id_categoria, nombre, descripcion, activo
    FROM categoria
    WHERE activo = 1;
END
GO

-- ==========================================
-- MÓDULO: STOCK
-- ==========================================

-- Consultar stock actual de un producto en una sucursal
CREATE OR ALTER PROCEDURE sp_Stock_ObtenerActual
    @IdProducto INT,
    @IdSucursal INT,
    @StockActual INT OUTPUT
AS
BEGIN
    -- Buscamos el stock y lo guardamos en la variable de salida
    SELECT @StockActual = stock_actual
    FROM stock_sucursal
    WHERE id_producto = @IdProducto AND id_sucursal = @IdSucursal;

    -- Si no existe el registro, devolvemos 0
    IF @StockActual IS NULL
        SET @StockActual = 0;
END
GO

-- Ingresar mercadería (Hace el IF/ELSE automáticamente)
CREATE OR ALTER PROCEDURE sp_Stock_IngresarMercaderia
    @IdProducto INT,
    @IdSucursal INT,
    @CantidadAIngresar INT
AS
BEGIN
    -- Verificamos si el producto ya tiene un registro en esa sucursal
    IF EXISTS (SELECT 1 FROM stock_sucursal WHERE id_producto = @IdProducto AND id_sucursal = @IdSucursal)
    BEGIN
        -- Si existe, le sumamos la cantidad nueva a lo que ya había
        UPDATE stock_sucursal
        SET stock_actual = stock_actual + @CantidadAIngresar
        WHERE id_producto = @IdProducto AND id_sucursal = @IdSucursal;
    END
    ELSE
    BEGIN
        -- Si es la primera vez que entra a esta sucursal, creamos el registro (stock mínimo en 5 por defecto)
        INSERT INTO stock_sucursal (id_producto, id_sucursal, stock_actual, stock_minimo)
        VALUES (@IdProducto, @IdSucursal, @CantidadAIngresar, 5);
    END
END
GO