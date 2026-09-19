USE StockOS;
GO

-- ============================================================================
-- PROCEDIMIENTOS ALMACENADOS PARA LA GESTIÓN DE PROVEEDORES
-- ============================================================================

-- 1. Obtener todos los proveedores
CREATE OR ALTER PROCEDURE sp_Proveedores_ObtenerTodos
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        id_proveedor,
        razon_social,
        cuit,
        telefono,
        email,
        direccion,
        activo
    FROM proveedor
    ORDER BY razon_social ASC;
END;
GO

-- 2. Insertar nuevo proveedor
CREATE OR ALTER PROCEDURE sp_Proveedores_Insertar
    @RazonSocial VARCHAR(100),
    @Cuit VARCHAR(30) = NULL,
    @Telefono VARCHAR(30) = '',
    @Email VARCHAR(100) = '',
    @Direccion VARCHAR(200) = '',
    @Activo BIT = 1,
    @IdProveedor INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO proveedor (razon_social, cuit, telefono, email, direccion, activo)
    VALUES (
        @RazonSocial, 
        NULLIF(LTRIM(RTRIM(@Cuit)), ''), 
        ISNULL(@Telefono, ''), 
        ISNULL(@Email, ''), 
        ISNULL(@Direccion, ''), 
        @Activo
    );

    SET @IdProveedor = SCOPE_IDENTITY();
END;
GO

-- 3. Actualizar proveedor existente
CREATE OR ALTER PROCEDURE sp_Proveedores_Actualizar
    @IdProveedor INT,
    @RazonSocial VARCHAR(100),
    @Cuit VARCHAR(30) = NULL,
    @Telefono VARCHAR(30) = '',
    @Email VARCHAR(100) = '',
    @Direccion VARCHAR(200) = '',
    @Activo BIT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE proveedor
    SET razon_social = @RazonSocial,
        cuit = NULLIF(LTRIM(RTRIM(@Cuit)), ''),
        telefono = ISNULL(@Telefono, ''),
        email = ISNULL(@Email, ''),
        direccion = ISNULL(@Direccion, ''),
        activo = @Activo
    WHERE id_proveedor = @IdProveedor;
END;
GO

-- 4. Cambiar estado activo/inactivo de un proveedor
CREATE OR ALTER PROCEDURE sp_Proveedores_CambiarEstado
    @IdProveedor INT,
    @Activo BIT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE proveedor
    SET activo = @Activo
    WHERE id_proveedor = @IdProveedor;
END;
GO

