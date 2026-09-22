-- =========================================================================================
-- SCRIPT DE MIGRACIÓN Y SINCRONIZACIÓN DE USUARIOS: StockOS
-- Fecha: 21 de Septiembre de 2026
-- Descripción: Actualiza o inserta los 5 usuarios del sistema con sus roles, contraseñas
--              encriptadas con BCrypt y datos normalizados para sincronizar entornos de desarrollo.
-- =========================================================================================

USE StockOS;
GO

-- 1. Normalización de nombre de Rol 'Encargado de Depósito' (con tilde)
UPDATE rol 
SET nombre = N'Encargado de Depósito' 
WHERE id_rol = 3;
GO

-- 2. Sincronización de Usuarios / Empleados

-- -----------------------------------------------------------------------------------------
-- 1. Administrador Principal (DNI: 43000111 | Clave: admin123)
-- -----------------------------------------------------------------------------------------
IF EXISTS (SELECT 1 FROM empleado WHERE dni = '43000111')
BEGIN
    UPDATE empleado 
    SET nombre = 'Admin', 
        apellido = '1', 
        email = 'admin@stockos.com', 
        password_hash = '$2a$11$sOYBDzMcMNbkep7brvXLNec8OyiG84BDlNbPWyAev8qt/oZ5ZXkyi', 
        id_rol = 1, 
        estado = 1
    WHERE dni = '43000111';
END
ELSE
BEGIN
    INSERT INTO empleado (nombre, apellido, dni, email, direccion, telefono, password_hash, estado, id_rol, id_sucursal)
    VALUES ('Admin', '1', '43000111', 'admin@stockos.com', 'Sin especificar', '3794562092', '$2a$11$sOYBDzMcMNbkep7brvXLNec8OyiG84BDlNbPWyAev8qt/oZ5ZXkyi', 1, 1, 1);
END;
GO

-- -----------------------------------------------------------------------------------------
-- 2. Administrador Secundario (DNI: 43000222 | Clave: admin123)
-- -----------------------------------------------------------------------------------------
IF EXISTS (SELECT 1 FROM empleado WHERE dni = '43000222')
BEGIN
    UPDATE empleado 
    SET nombre = 'Admin', 
        apellido = '2', 
        email = 'administrador@stockos.com', 
        password_hash = '$2a$11$sOYBDzMcMNbkep7brvXLNec8OyiG84BDlNbPWyAev8qt/oZ5ZXkyi', 
        id_rol = 1, 
        estado = 1
    WHERE dni = '43000222';
END
ELSE IF EXISTS (SELECT 1 FROM empleado WHERE dni = '43205368')
BEGIN
    UPDATE empleado 
    SET nombre = 'Admin', 
        apellido = '2', 
        dni = '43000222', 
        email = 'administrador@stockos.com', 
        password_hash = '$2a$11$sOYBDzMcMNbkep7brvXLNec8OyiG84BDlNbPWyAev8qt/oZ5ZXkyi', 
        id_rol = 1, 
        estado = 1
    WHERE dni = '43205368';
END
ELSE
BEGIN
    INSERT INTO empleado (nombre, apellido, dni, email, direccion, telefono, password_hash, estado, id_rol, id_sucursal)
    VALUES ('Admin', '2', '43000222', 'administrador@stockos.com', 'Sin especificar', '3794834167', '$2a$11$sOYBDzMcMNbkep7brvXLNec8OyiG84BDlNbPWyAev8qt/oZ5ZXkyi', 1, 1, 1);
END;
GO

-- -----------------------------------------------------------------------------------------
-- 3. Repositor (DNI: 11222333 | Clave: repo123)
-- -----------------------------------------------------------------------------------------
IF EXISTS (SELECT 1 FROM empleado WHERE dni = '11222333')
BEGIN
    UPDATE empleado 
    SET nombre = 'Repositor', 
        apellido = '1', 
        email = 'repo1@gmail.com', 
        direccion = 'calle 123', 
        telefono = '3794112233',
        password_hash = '$2a$11$ZARprCPtuVdrZGc7.tP/FuHyXSMIhZw2P3WVfWe34kH6NKZyvC0H.', 
        id_rol = 4, 
        estado = 1
    WHERE dni = '11222333';
END
ELSE
BEGIN
    INSERT INTO empleado (nombre, apellido, dni, email, direccion, telefono, password_hash, estado, id_rol, id_sucursal)
    VALUES ('Repositor', '1', '11222333', 'repo1@gmail.com', 'calle 123', '3794112233', '$2a$11$ZARprCPtuVdrZGc7.tP/FuHyXSMIhZw2P3WVfWe34kH6NKZyvC0H.', 1, 4, 1);
END;
GO

-- -----------------------------------------------------------------------------------------
-- 4. Encargado de Depósito (DNI: 44555666 | Clave: depo123)
-- -----------------------------------------------------------------------------------------
IF EXISTS (SELECT 1 FROM empleado WHERE dni = '44555666')
BEGIN
    UPDATE empleado 
    SET nombre = 'Deposito', 
        apellido = '1', 
        email = 'depo@gmail.com', 
        direccion = 'calle 789', 
        telefono = '3794990088',
        password_hash = '$2a$11$1/OIXLIEqH2q2rd5Om5U2uRLaND1x7BDCd/U2RrpAuWiaCG/npBpC', 
        id_rol = 3, 
        estado = 1
    WHERE dni = '44555666';
END
ELSE
BEGIN
    INSERT INTO empleado (nombre, apellido, dni, email, direccion, telefono, password_hash, estado, id_rol, id_sucursal)
    VALUES ('Deposito', '1', '44555666', 'depo@gmail.com', 'calle 789', '3794990088', '$2a$11$1/OIXLIEqH2q2rd5Om5U2uRLaND1x7BDCd/U2RrpAuWiaCG/npBpC', 1, 3, 1);
END;
GO

-- -----------------------------------------------------------------------------------------
-- 5. Cajero (DNI: 66333999 | Clave: cajero12)
-- -----------------------------------------------------------------------------------------
IF EXISTS (SELECT 1 FROM empleado WHERE dni = '66333999')
BEGIN
    UPDATE empleado 
    SET nombre = 'Cajero', 
        apellido = '1', 
        email = 'cajero@gmail.com', 
        direccion = 'calle 777', 
        telefono = '3794887711',
        password_hash = '$2a$11$xMrrzhWA8FO3azb1QPHuKeyX0FqEvq/LHtet198OhcGNCgXlptHBy', 
        id_rol = 2, 
        estado = 1
    WHERE dni = '66333999';
END
ELSE
BEGIN
    INSERT INTO empleado (nombre, apellido, dni, email, direccion, telefono, password_hash, estado, id_rol, id_sucursal)
    VALUES ('Cajero', '1', '66333999', 'cajero@gmail.com', 'calle 777', '3794887711', '$2a$11$xMrrzhWA8FO3azb1QPHuKeyX0FqEvq/LHtet198OhcGNCgXlptHBy', 1, 2, 1);
END;
GO

