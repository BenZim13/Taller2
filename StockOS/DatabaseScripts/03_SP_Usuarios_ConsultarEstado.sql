USE StockOS;
GO

-- =============================================================
-- Migración: Stored Procedure para consultar estado de usuario
-- Objetivo: Permitir verificar si un usuario se encuentra activo
--           o deshabilitado antes de permitir el inicio de sesión.
-- =============================================================

CREATE OR ALTER PROCEDURE sp_Usuarios_ConsultarEstado
    @Dni VARCHAR(20),
    @Estado BIT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @Estado = NULL;

    SELECT @Estado = estado
    FROM empleado
    WHERE dni = @Dni;
END
GO

