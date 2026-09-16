USE StockOS;
GO

CREATE OR ALTER PROCEDURE sp_CajaSesion_Abrir
    @IdCaja INT,
    @IdEmpleado INT,
    @MontoApertura DECIMAL(12,2),
    @IdCajaSesion INT OUTPUT
AS
BEGIN
    -- Insertamos la nueva sesión (fecha_apertura se pone sola por el DEFAULT SYSDATETIME)
    INSERT INTO caja_sesion (monto_apertura, estado, id_caja, id_empleado)
    VALUES (@MontoApertura, 1, @IdCaja, @IdEmpleado);
    
    -- Devolvemos el ID generado para guardarlo en la sesión de C#
    SET @IdCajaSesion = SCOPE_IDENTITY();
END
GO

CREATE OR ALTER PROCEDURE sp_Cajas_ObtenerPorSucursal
    @IdSucursal INT
AS
BEGIN
    SELECT id_caja, nombre_numero, id_sucursal, activo 
    FROM caja 
    WHERE id_sucursal = @IdSucursal AND activo = 1;
END
GO