USE StockOS;
GO

-- ==========================================
-- MIGRACIÓN: Categorías iniciales
-- Ejecutar UNA SOLA VEZ sobre la BD vacía.
-- ==========================================

IF NOT EXISTS (SELECT 1 FROM categoria WHERE nombre = 'Panaderia')
    INSERT INTO categoria (nombre, descripcion, activo) VALUES ('Panaderia', 'Pan, facturas, medialunas y productos de panadería', 1);

IF NOT EXISTS (SELECT 1 FROM categoria WHERE nombre = 'Bebidas')
    INSERT INTO categoria (nombre, descripcion, activo) VALUES ('Bebidas', 'Gaseosas, aguas, jugos, cervezas y bebidas en general', 1);

IF NOT EXISTS (SELECT 1 FROM categoria WHERE nombre = 'Carniceria')
    INSERT INTO categoria (nombre, descripcion, activo) VALUES ('Carniceria', 'Carnes vacunas, porcinas, aves y derivados', 1);

IF NOT EXISTS (SELECT 1 FROM categoria WHERE nombre = 'Verduleria')
    INSERT INTO categoria (nombre, descripcion, activo) VALUES ('Verduleria', 'Frutas y verduras frescas', 1);

IF NOT EXISTS (SELECT 1 FROM categoria WHERE nombre = 'Rotiseria')
    INSERT INTO categoria (nombre, descripcion, activo) VALUES ('Rotiseria', 'Comidas preparadas, empanadas y platos listos', 1);

IF NOT EXISTS (SELECT 1 FROM categoria WHERE nombre = 'Fiambreria')
    INSERT INTO categoria (nombre, descripcion, activo) VALUES ('Fiambreria', 'Fiambres, embutidos y quesos', 1);

IF NOT EXISTS (SELECT 1 FROM categoria WHERE nombre = 'Comestibles')
    INSERT INTO categoria (nombre, descripcion, activo) VALUES ('Comestibles', 'Alimentos secos, conservas, condimentos y enlatados', 1);

IF NOT EXISTS (SELECT 1 FROM categoria WHERE nombre = 'Lacteos')
    INSERT INTO categoria (nombre, descripcion, activo) VALUES ('Lacteos', 'Leches, yogures, cremas y manteca', 1);

IF NOT EXISTS (SELECT 1 FROM categoria WHERE nombre = 'Perfumeria')
    INSERT INTO categoria (nombre, descripcion, activo) VALUES ('Perfumeria', 'Perfumes, desodorantes y cosmeticos', 1);

IF NOT EXISTS (SELECT 1 FROM categoria WHERE nombre = 'Limpieza')
    INSERT INTO categoria (nombre, descripcion, activo) VALUES ('Limpieza', 'Productos de limpieza del hogar y superficies', 1);

IF NOT EXISTS (SELECT 1 FROM categoria WHERE nombre = 'Higiene')
    INSERT INTO categoria (nombre, descripcion, activo) VALUES ('Higiene', 'Articulos de higiene personal: shampoo, jabon, cepillos', 1);
GO

-- ==========================================
-- SP: Insertar nueva categoría desde la app
-- ==========================================
CREATE OR ALTER PROCEDURE sp_Categorias_Insertar
    @Nombre      NVARCHAR(100),
    @Descripcion NVARCHAR(255),
    @IdCategoria INT OUTPUT
AS
BEGIN
    INSERT INTO categoria (nombre, descripcion, activo)
    VALUES (@Nombre, @Descripcion, 1);

    SET @IdCategoria = SCOPE_IDENTITY();
END
GO

-- ==========================================
-- SP: Actualizar / dar de baja una categoría
-- ==========================================
CREATE OR ALTER PROCEDURE sp_Categorias_Actualizar
    @IdCategoria INT,
    @Nombre      NVARCHAR(100),
    @Descripcion NVARCHAR(255),
    @Activo      BIT
AS
BEGIN
    UPDATE categoria
    SET nombre      = @Nombre,
        descripcion = @Descripcion,
        activo      = @Activo
    WHERE id_categoria = @IdCategoria;
END
GO

