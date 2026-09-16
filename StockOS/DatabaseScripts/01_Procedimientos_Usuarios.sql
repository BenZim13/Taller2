USE StockOS;
GO

-- 1. Insertar Usuario
CREATE OR ALTER PROCEDURE sp_Usuarios_Insertar
    @Nombre VARCHAR(50),
    @Apellido VARCHAR(50),
    @Dni VARCHAR(20),
    @Email VARCHAR(100),
    @Telefono VARCHAR(30),
    @PasswordHash VARCHAR(255),
    @IdRol INT,
    @IdSucursal INT,
    @IdEmpleado INT OUTPUT
AS
BEGIN
    INSERT INTO empleado (nombre, apellido, dni, email, telefono, password_hash, estado, id_rol, id_sucursal)
    VALUES (@Nombre, @Apellido, @Dni, @Email, @Telefono, @PasswordHash, 1, @IdRol, @IdSucursal);
    
    SET @IdEmpleado = SCOPE_IDENTITY();
END
GO

-- 2. Autenticar Usuario (Login)
CREATE OR ALTER PROCEDURE sp_Usuarios_Autenticar
    @Dni VARCHAR(20)
AS
BEGIN
    SELECT id_empleado, nombre, apellido, dni, email, telefono, password_hash, estado, id_rol, id_sucursal
    FROM empleado 
    WHERE dni = @Dni;
END
GO

-- 3. Actualizar Usuario
CREATE OR ALTER PROCEDURE sp_Usuarios_Actualizar
    @IdEmpleado INT,
    @Nombre VARCHAR(50),
    @Apellido VARCHAR(50),
    @Dni VARCHAR(20),
    @Email VARCHAR(100),
    @Telefono VARCHAR(30),
    @IdRol INT,
    @IdSucursal INT,
    @Estado BIT,
    @PasswordHash VARCHAR(255)
AS
BEGIN
    UPDATE empleado
    SET nombre = @Nombre, 
        apellido = @Apellido, 
        dni = @Dni, 
        email = @Email, 
        telefono = @Telefono, 
        id_rol = @IdRol, 
        id_sucursal = @IdSucursal,
        estado = @Estado,
        password_hash = COALESCE(@PasswordHash, password_hash)
    WHERE id_empleado = @IdEmpleado;
END
GO

-- 4. Cambiar Estado (Baja Lógica / Suspensión)
CREATE OR ALTER PROCEDURE sp_Usuarios_CambiarEstado
    @IdEmpleado INT,
    @Estado BIT
AS
BEGIN
    UPDATE empleado
    SET estado = @Estado
    WHERE id_empleado = @IdEmpleado;
END
GO