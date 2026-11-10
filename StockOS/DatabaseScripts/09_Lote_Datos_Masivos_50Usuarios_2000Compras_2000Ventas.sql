-- =========================================================================================
-- SCRIPT INTEGRAL DE DATOS MASIVOS (SEED DATA COMPLETO) - StockOS
-- =========================================================================================
-- Este script es 100% autónomo y genera todo el ecosistema de datos sin borrar lo existente:
--   1. Estructura Base: Sucursal, 5 Cajas Operativas, 11 Categorías, 10 Proveedores y Medios de Pago.
--   2. 50 Usuarios (tabla [empleado]):
--      - 15 Usuarios DADOS DE BAJA (estado = 0) que trabajaron en la plataforma (con sesiones
--        de caja asignadas, compras y ventas históricas registradas durante su etapa activa).
--      - 35 Usuarios ACTIVOS (estado = 1) trabajando actualmente con sus cajas y sesiones.
--   3. 1.000 Productos (tabla [producto] y [stock_sucursal]) CLASIFICADOS ESTRICTAMENTE
--      según su categoría real (Panadería, Bebidas, Carnicería, Verdulería, Rotisería,
--      Fiambrería, Comestibles, Lácteos, Perfumería, Limpieza e Higiene), con marcas y
--      proveedores coherentes para cada rubro.
--   4. 2.000 Compras (tablas [compra] y [detalle_compra]) de los 1.000 productos.
--   5. 2.000 Ventas (tablas [caja_sesion], [venta], [detalle_venta] y [pago]) de los 1.000
--      productos, distribuidas entre Diciembre del año pasado y el mes actual.
-- =========================================================================================

USE StockOS;
GO

SET NOCOUNT ON;
SET XACT_ABORT ON;

BEGIN TRANSACTION;

BEGIN TRY
    PRINT '=========================================================================';
    PRINT 'INICIANDO CARGA MASIVA INTEGRAL EN STOCKOS';
    PRINT '  - 1.000 Productos clasificados exactamente por Categoría, Marca y Rubro';
    PRINT '  - 50 Usuarios (15 Dados de Baja con historial + 35 Activos) y Cajas';
    PRINT '  - 2.000 Compras y 2.000 Ventas (Diciembre pasado a este mes)';
    PRINT '=========================================================================';

    -- =========================================================================
    -- PASO 0: ASEGURAR ROLES, SUCURSAL, CAJAS, CATEGORÍAS, PROVEEDORES Y MEDIOS DE PAGO
    -- =========================================================================
    PRINT 'Paso 0/4 -> Verificando y asegurando Roles, Sucursal, Cajas, Categorías y Proveedores...';

    -- 0.1 Roles base
    IF NOT EXISTS (SELECT 1 FROM rol WHERE id_rol = 1)
    BEGIN
        INSERT INTO rol (nombre, descripcion) VALUES 
        (N'Administrador', N'Acceso total al sistema'),
        (N'Cajero', N'Atención al punto de venta, cobro a clientes y manejo de caja.'),
        (N'Encargado de Depósito', N'Control total del inventario, ingreso de mercadería y gestión de stock.'),
        (N'Repositor', N'Consulta de stock y control de ubicación de productos en el salón.');
    END

    -- 0.2 Sucursal Principal
    DECLARE @IdSucursal INT = (SELECT TOP 1 id_sucursal FROM sucursal WHERE activo = 1 ORDER BY id_sucursal);
    IF @IdSucursal IS NULL
    BEGIN
        INSERT INTO sucursal (nombre, direccion, activo) VALUES ('Casa Central', 'Av. Libertad 1250, Corrientes', 1);
        SET @IdSucursal = SCOPE_IDENTITY();
    END

    -- 0.3 Asegurar 5 Cajas físicas en la sucursal para asignar las sesiones de los usuarios
    DECLARE @NumCaja INT = 1;
    WHILE @NumCaja <= 5
    BEGIN
        DECLARE @NombreCaja VARCHAR(50) = 'Caja ' + CAST(@NumCaja AS VARCHAR(10));
        IF NOT EXISTS (SELECT 1 FROM caja WHERE id_sucursal = @IdSucursal AND nombre_numero = @NombreCaja)
        BEGIN
            INSERT INTO caja (nombre_numero, id_sucursal, activo) VALUES (@NombreCaja, @IdSucursal, 1);
        END
        SET @NumCaja = @NumCaja + 1;
    END

    DECLARE @CajasDisponibles TABLE (idx INT IDENTITY(1,1), id_caja INT);
    INSERT INTO @CajasDisponibles (id_caja)
    SELECT id_caja FROM caja WHERE id_sucursal = @IdSucursal AND activo = 1 ORDER BY id_caja;
    DECLARE @CantCajas INT = (SELECT COUNT(*) FROM @CajasDisponibles);

    -- 0.4 Métodos de Pago
    IF NOT EXISTS (SELECT 1 FROM metodo_pago)
    BEGIN
        INSERT INTO metodo_pago (nombre, activo) VALUES 
        ('Efectivo', 1),
        ('Tarjeta de Débito', 1),
        ('Tarjeta de Crédito', 1),
        ('Mercado Pago', 1);
    END

    DECLARE @MetodosPagoDisponibles TABLE (idx INT IDENTITY(1,1), id_metodo_pago INT);
    INSERT INTO @MetodosPagoDisponibles (id_metodo_pago)
    SELECT id_metodo_pago FROM metodo_pago WHERE activo = 1 ORDER BY id_metodo_pago;
    DECLARE @CantMetodosPago INT = (SELECT COUNT(*) FROM @MetodosPagoDisponibles);

    -- 0.5 Categorías (Las 11 categorías comerciales del sistema)
    DECLARE @CatNombres TABLE (id INT IDENTITY(1,1), nombre VARCHAR(100), desc_cat VARCHAR(255));
    INSERT INTO @CatNombres (nombre, desc_cat) VALUES
    ('Panaderia', 'Pan, facturas, medialunas y productos de panadería'),
    ('Bebidas', 'Gaseosas, aguas, jugos, cervezas y bebidas en general'),
    ('Carniceria', 'Carnes vacunas, porcinas, aves y derivados'),
    ('Verduleria', 'Frutas y verduras frescas de estación'),
    ('Rotiseria', 'Comidas preparadas, empanadas y platos listos'),
    ('Fiambreria', 'Fiambres, embutidos, salames y quesos'),
    ('Comestibles', 'Alimentos secos, conservas, condimentos y enlatados'),
    ('Lacteos', 'Leches, yogures, cremas, postres y manteca'),
    ('Perfumeria', 'Perfumes, desodorantes, cuidado capilar y cosméticos'),
    ('Limpieza', 'Productos de limpieza del hogar, ropa y superficies'),
    ('Higiene', 'Artículos de higiene personal: jabones, papel, cepillos');

    INSERT INTO categoria (nombre, descripcion, activo)
    SELECT cn.nombre, cn.desc_cat, 1
    FROM @CatNombres cn
    WHERE NOT EXISTS (SELECT 1 FROM categoria c WHERE c.nombre = cn.nombre);

    -- 0.6 Proveedores especializados por rubro (10 Proveedores mayoristas realistas)
    DECLARE @ProvSeed TABLE (razon VARCHAR(100), cuit VARCHAR(30), tel VARCHAR(30), mail VARCHAR(100), dir VARCHAR(200));
    INSERT INTO @ProvSeed VALUES
    ('Molinos y Panificados Río Paraná', '30-71001006-0', '3794101006', 'ventas@molinosparana.com.ar', 'Puerto Exterior Galpón 4'),
    ('Embotelladora del Norte S.A.', '30-71001005-1', '3794101005', 'atencion@embotelladoranorte.com', 'Av. Cazadores Correntinos 1900'),
    ('Frigorífico Regional NEA', '30-71001003-5', '3794101003', 'comercial@frigonea.com.ar', 'Av. Maipú 3400, Corrientes'),
    ('Mercado Concentrador Frutihortícola', '30-71001009-4', '3794101009', 'puesto14@concentrador.com.ar', 'Ruta 5 Km 2.5'),
    ('Alimentos Selectos y Fiambres S.A.', '30-71001010-8', '3794101010', 'ventas@alimentosselectos.com', 'Av. 3 de Abril 980'),
    ('Distribuidora Comestibles del Litoral S.A.', '30-71001001-9', '3794101001', 'ventas@dlitoral.com.ar', 'Parque Industrial Corrientes Calle 2'),
    ('Lácteos y Frescos del Sur', '30-71001004-3', '3794101004', 'contacto@lacteossur.com.ar', 'Av. Independencia 4520'),
    ('Cosmética y Perfumería Global', '30-71001008-6', '3794101008', 'pedidos@perfumeriaglobal.com', 'Belgrano 1540, Corrientes'),
    ('Química y Limpieza Industrial', '30-71001007-8', '3794101007', 'info@quimicalimpieza.com.ar', 'Av. Armenia 2890'),
    ('Mayorista Higiene y Cuidado S.R.L.', '30-71001002-7', '3794101002', 'pedidos@elabasto.com.ar', 'Ruta Nacional 12 Km 1032');

    INSERT INTO proveedor (razon_social, cuit, telefono, email, direccion, activo)
    SELECT ps.razon, ps.cuit, ps.tel, ps.mail, ps.dir, 1
    FROM @ProvSeed ps
    WHERE NOT EXISTS (SELECT 1 FROM proveedor p WHERE p.cuit = ps.cuit);

    DECLARE @ProveedoresDisponibles TABLE (idx INT IDENTITY(1,1), id_proveedor INT);
    INSERT INTO @ProveedoresDisponibles (id_proveedor)
    SELECT id_proveedor FROM proveedor WHERE activo = 1 ORDER BY id_proveedor;
    DECLARE @CantProveedores INT = (SELECT COUNT(*) FROM @ProveedoresDisponibles);

    -- =========================================================================
    -- PASO 1: GENERACIÓN DE 50 USUARIOS (15 DADOS DE BAJA + 35 ACTIVOS)
    -- =========================================================================
    PRINT 'Paso 1/4 -> Generando lote de 50 usuarios (15 dados de baja y 35 activos)...';

    -- Asegurar que el usuario Admin de prueba (43000111 / admin123) siempre exista
    IF NOT EXISTS (SELECT 1 FROM empleado WHERE email = 'admin@stockos.com' OR dni = '43000111')
    BEGIN
        INSERT INTO empleado (nombre, apellido, dni, email, direccion, telefono, password_hash, estado, id_rol, id_sucursal)
        VALUES ('Admin', '1', '43000111', 'admin@stockos.com', 'Sin especificar', '3794562092', '$2a$11$sOYBDzMcMNbkep7brvXLNec8OyiG84BDlNbPWyAev8qt/oZ5ZXkyi', 1, 1, @IdSucursal);
    END

    DECLARE @NuevosEmpleados TABLE (
        nro_orden INT,
        id_empleado INT,
        estado BIT,
        id_rol INT
    );

    DECLARE @Nombres TABLE (id INT IDENTITY(1,1), texto VARCHAR(50));
    INSERT INTO @Nombres (texto) VALUES
    ('Lucas'), ('Mateo'), ('Sofia'), ('Valentina'), ('Santiago'),
    ('Camila'), ('Nicolas'), ('Martina'), ('Benjamin'), ('Lucia'),
    ('Joaquin'), ('Catalina'), ('Tomas'), ('Agustina'), ('Facundo'),
    ('Julieta'), ('Ignacio'), ('Micaela'), ('Franco'), ('Florencia'),
    ('Lautaro'), ('Antonella'), ('Gonzalo'), ('Milagros'), ('Ramiro');

    DECLARE @Apellidos TABLE (id INT IDENTITY(1,1), texto VARCHAR(50));
    INSERT INTO @Apellidos (texto) VALUES
    ('Gonzalez'), ('Rodriguez'), ('Gomez'), ('Fernandez'), ('Lopez'),
    ('Diaz'), ('Martinez'), ('Perez'), ('Garcia'), ('Sanchez'),
    ('Romero'), ('Sosa'), ('Torres'), ('Alvarez'), ('Ruiz'),
    ('Ramirez'), ('Flores'), ('Acosta'), ('Benitez'), ('Medina'),
    ('Suarez'), ('Herrera'), ('Aguirre'), ('Pereyra'), ('Gutierrez');

    DECLARE @HashPassword VARCHAR(255) = '$2a$11$xMrrzhWA8FO3azb1QPHuKeyX0FqEvq/LHtet198OhcGNCgXlptHBy';

    DECLARE @i INT = 1;
    WHILE @i <= 50
    BEGIN
        DECLARE @NombreEmp VARCHAR(50) = (SELECT texto FROM @Nombres WHERE id = ((@i - 1) % 25) + 1);
        DECLARE @ApellidoEmp VARCHAR(50) = (SELECT texto FROM @Apellidos WHERE id = (((@i * 3) - 1) % 25) + 1);
        DECLARE @DniBase VARCHAR(20) = CAST(36200000 + @i AS VARCHAR(20));

        DECLARE @IdEmpActual INT = (SELECT id_empleado FROM empleado WHERE dni = @DniBase);
        DECLARE @EstadoEmp BIT = CASE WHEN @i <= 15 THEN 0 ELSE 1 END;
        DECLARE @RolEmp INT = CASE 
            WHEN @i % 10 = 0 THEN 1 -- Administrador
            WHEN @i % 3 = 0 THEN 3  -- Encargado de Depósito
            WHEN @i % 5 = 0 THEN 4  -- Repositor
            ELSE 2                  -- Cajero
        END;

        IF @IdEmpActual IS NULL
        BEGIN
            DECLARE @EmailEmp VARCHAR(100) = LOWER(@NombreEmp + '.' + @ApellidoEmp + '.' + @DniBase + '@stockos.com.ar');
            DECLARE @TelefonoEmp VARCHAR(30) = '3794' + RIGHT('000000' + CAST(300000 + (@i * 157) AS VARCHAR(10)), 6);
            DECLARE @DireccionEmp VARCHAR(200) = 'Av. Independencia ' + CAST(200 + (@i * 18) AS VARCHAR(10)) + ', Corrientes';

            INSERT INTO empleado (nombre, apellido, dni, email, telefono, direccion, password_hash, estado, id_rol, id_sucursal)
            VALUES (@NombreEmp, @ApellidoEmp, @DniBase, @EmailEmp, @TelefonoEmp, @DireccionEmp, @HashPassword, @EstadoEmp, @RolEmp, @IdSucursal);

            SET @IdEmpActual = SCOPE_IDENTITY();
        END
        ELSE
        BEGIN
            UPDATE empleado SET estado = @EstadoEmp WHERE id_empleado = @IdEmpActual;
        END

        INSERT INTO @NuevosEmpleados (nro_orden, id_empleado, estado, id_rol)
        VALUES (@i, @IdEmpActual, @EstadoEmp, @RolEmp);

        SET @i = @i + 1;
    END

    -- =========================================================================
    -- PASO 2: GENERACIÓN DE 1.000 PRODUCTOS CLASIFICADOS POR SU CATEGORÍA REAL
    -- =========================================================================
    PRINT 'Paso 2/4 -> Clasificando y generando 1.000 productos por Categoría, Marca y Proveedor...';

    -- Matriz maestra donde cada artículo está vinculado estrictamente a su Categoría real,
    -- con 4 marcas reales propias de ese rubro y el CUIT de su proveedor específico.
    DECLARE @MatrizRubros TABLE (
        id INT IDENTITY(1,1),
        categoria_nombre VARCHAR(100),
        cuit_proveedor VARCHAR(30),
        nombre_base VARCHAR(60),
        presentacion VARCHAR(40),
        precio_base DECIMAL(12,2),
        iva DECIMAL(5,2),
        marca1 VARCHAR(40),
        marca2 VARCHAR(40),
        marca3 VARCHAR(40),
        marca4 VARCHAR(40)
    );

    INSERT INTO @MatrizRubros
    (categoria_nombre, cuit_proveedor, nombre_base, presentacion, precio_base, iva, marca1, marca2, marca3, marca4)
    VALUES
    -- 1. PANADERIA
    ('Panaderia', '30-71001006-0', 'Pan Francés Horneado', 'Bolsa 1kg', 1500.00, 10.50, 'Artesano', 'Espiga Dorada', 'La Tradición', 'Horno Real'),
    ('Panaderia', '30-71001006-0', 'Pan Lactal Blanco', 'Paquete 500g', 2350.00, 10.50, 'Bimbo', 'Fargo', 'Lactal', 'Veneziana'),
    ('Panaderia', '30-71001006-0', 'Medialunas de Manteca', 'Bandeja x 6 u.', 2800.00, 21.00, 'Horno Real', 'La Tradición', 'Espiga Dorada', 'Artesano'),
    ('Panaderia', '30-71001006-0', 'Bizcochos de Grasa', 'Paquete 200g', 1100.00, 21.00, 'Don Satur', '9 de Oro', 'Tía Maruca', ' Cuquer'),

    -- 2. BEBIDAS
    ('Bebidas', '30-71001005-1', 'Gaseosa Sabor Cola', 'Botella 2.25L', 2450.00, 21.00, 'Coca-Cola', 'Pepsi', 'Manaos', 'Cunnington'),
    ('Bebidas', '30-71001005-1', 'Agua Mineral sin Gas', 'Botella 2L', 1150.00, 21.00, 'Villavicencio', 'Eco de los Andes', 'Glaciar', 'Kin'),
    ('Bebidas', '30-71001005-1', 'Cerveza Rubia Lager', 'Lata 473ml', 1750.00, 21.00, 'Quilmes', 'Brahma', 'Stella Artois', 'Patagonia'),
    ('Bebidas', '30-71001005-1', 'Jugo de Naranja 100%', 'Tetra 1L', 1550.00, 21.00, 'Cepita', 'Citric', 'Baggio', 'Ades'),

    -- 3. CARNICERIA
    ('Carniceria', '30-71001003-5', 'Asado de Tira Novillo', 'Bandeja 1kg', 7400.00, 10.50, 'Frigorífico NEA', 'Swift', 'Estancia del Sur', 'La Rural'),
    ('Carniceria', '30-71001003-5', 'Carne Picada Especial', 'Bandeja 1kg', 6200.00, 10.50, 'Frigorífico NEA', 'Swift', 'Estancia del Sur', 'La Rural'),
    ('Carniceria', '30-71001003-5', 'Pechuga de Pollo Deshuesada', 'Bandeja 1kg', 5600.00, 10.50, 'Granja Tres Arroyos', 'Soychú', 'Cresta Roja', 'Avícola Norte'),
    ('Carniceria', '30-71001003-5', 'Bondiola de Cerdo Seleccionada', 'Corte 1kg', 6900.00, 10.50, 'Paladini', 'Cagnoli', 'Campo Austral', 'Frigorífico NEA'),

    -- 4. VERDULERIA
    ('Verduleria', '30-71001009-4', 'Tomate Perita Fresco', 'Malla 1kg', 1400.00, 10.50, 'Huerta Correntina', 'Campo Verde', 'Quinta del Sol', 'FrescoSur'),
    ('Verduleria', '30-71001009-4', 'Papa Blanca Cepillada', 'Bolsa 2kg', 1850.00, 10.50, 'Campo Verde', 'Pampa Fértil', 'Huerta Correntina', 'FrescoSur'),
    ('Verduleria', '30-71001009-4', 'Manzana Roja Deliciosa', 'Bandeja 1kg', 2100.00, 10.50, 'Moño Azul', 'Valle de Río Negro', 'Patagonia Frut', 'Quinta del Sol'),
    ('Verduleria', '30-71001009-4', 'Cebolla Seleccionada', 'Malla 1kg', 1250.00, 10.50, 'Campo Verde', 'Huerta Correntina', 'Pampa Fértil', 'FrescoSur'),

    -- 5. ROTISERIA
    ('Rotiseria', '30-71001010-8', 'Empanadas de Carne Cortada', 'Docena Lista', 8500.00, 21.00, 'Cocina StockOS', 'El Noble', 'Sabores Caseros', 'Doña Clara'),
    ('Rotiseria', '30-71001010-8', 'Tarta Pascualina de Verdura', 'Unidad 450g', 4200.00, 21.00, 'Cocina StockOS', 'Sabores Caseros', 'Doña Clara', 'El Noble'),
    ('Rotiseria', '30-71001010-8', 'Pollo al Spiedo con Papas', 'Porción Familiar', 9800.00, 21.00, 'Cocina StockOS', 'Rosticería Real', 'Sabores Caseros', 'Doña Clara'),
    ('Rotiseria', '30-71001010-8', 'Pizza Muzzarella Precocida', 'Unidad 8 Porciones', 5400.00, 21.00, 'Cocina StockOS', 'Sibarita', 'Sabores Caseros', 'El Noble'),

    -- 6. FIAMBRERIA
    ('Fiambreria', '30-71001010-8', 'Jamón Cocido Etiqueta Azul', 'Feteado 200g', 2300.00, 21.00, 'Paladini', 'Bocatti', 'Cagnoli', 'Campo Austral'),
    ('Fiambreria', '30-71001010-8', 'Queso Tybo en Barra', 'Feteado 250g', 2750.00, 21.00, 'La Paulina', 'Ilolay', 'Milkaut', 'Verónica'),
    ('Fiambreria', '30-71001010-8', 'Salame Milán Picado Fino', 'Bandeja 150g', 2150.00, 21.00, 'Cagnoli', 'Paladini', 'Calchaquí', 'Lario'),
    ('Fiambreria', '30-71001010-8', 'Queso Cremoso Horma', 'Fracción 500g', 3800.00, 21.00, 'La Paulina', 'Tregar', 'Punta del Agua', 'Ilolay'),

    -- 7. COMESTIBLES
    ('Comestibles', '30-71001001-9', 'Arroz Largo Fino 00000', 'Paquete 1kg', 1800.00, 21.00, 'Gallo Oro', 'Molinos Ala', 'Dos Hermanos', 'Marolio'),
    ('Comestibles', '30-71001001-9', 'Fideos Tallarines al Huevo', 'Paquete 500g', 1200.00, 21.00, 'Matarazzo', 'Lucchetti', 'Don Vicente', 'Knorr'),
    ('Comestibles', '30-71001001-9', 'Aceite de Girasol Puro', 'Botella 1.5L', 2900.00, 21.00, 'Natura', 'Cocinero', 'Cañuelas', 'Lira'),
    ('Comestibles', '30-71001001-9', 'Yerba Mate Selección Especial', 'Paquete 1kg', 3500.00, 21.00, 'Taragüi', 'Playadito', 'Rosamonte', 'La Merced'),
    ('Comestibles', '30-71001001-9', 'Galletitas Dulces Rellenas', 'Paquete 150g', 1350.00, 21.00, 'Arcor', 'Bagley', 'Terrabusi', 'Oreo'),
    ('Comestibles', '30-71001001-9', 'Café Tostado Molido', 'Frasco 250g', 5200.00, 21.00, 'La Virginia', 'Cabrales', 'Nescafé', 'Bonafide'),

    -- 8. LACTEOS
    ('Lacteos', '30-71001004-3', 'Leche Entera Larga Vida', 'Tetra 1L', 1420.00, 21.00, 'La Serenísima', 'Sancor', 'Ilolay', 'Tregar'),
    ('Lacteos', '30-71001004-3', 'Yogur Bebible Frutilla', 'Sachet 900g', 1680.00, 21.00, 'La Serenísima', 'Yogurísimo', 'Tregar', 'Milkaut'),
    ('Lacteos', '30-71001004-3', 'Manteca Extra Calidad', 'Pan 200g', 2250.00, 21.00, 'La Serenísima', 'Tonadita', 'Milkaut', 'Sancor'),
    ('Lacteos', '30-71001004-3', 'Dulce de Leche Clásico', 'Pote 400g', 2400.00, 21.00, 'La Serenísima', 'Ilolay', 'Milkaut', 'Vacalin'),

    -- 9. PERFUMERIA
    ('Perfumeria', '30-71001008-6', 'Shampoo Restauración Capilar', 'Envase 400ml', 3300.00, 21.00, 'Pantene', 'Sedal', 'Dove', 'Elvive'),
    ('Perfumeria', '30-71001008-6', 'Desodorante Antitranspirante', 'Aerosol 150ml', 2550.00, 21.00, 'Rexona', 'Dove', 'Axe', 'Nivea'),
    ('Perfumeria', '30-71001008-6', 'Crema Corporal Nutrición', 'Pote 250ml', 3100.00, 21.00, 'Nivea', 'Dermaglós', 'Bagóvit', 'Dove'),
    ('Perfumeria', '30-71001008-6', 'Acondicionador Brillo Sedoso', 'Envase 400ml', 3350.00, 21.00, 'Sedal', 'Pantene', 'Plusbelle', 'Tresemmé'),

    -- 10. LIMPIEZA
    ('Limpieza', '30-71001007-8', 'Detergente Lavavajillas Concentrado', 'Botella 750ml', 1650.00, 21.00, 'Magistral', 'Cif', 'Ala', 'Zorro'),
    ('Limpieza', '30-71001007-8', 'Lavandina Desinfectante', 'Bidón 2L', 1750.00, 21.00, 'Ayudín', 'Querubín', 'Vim', 'Sello Azul'),
    ('Limpieza', '30-71001007-8', 'Jabón Líquido Baja Espuma', 'Doypack 800ml', 3150.00, 21.00, 'Skip', 'Ala', 'Ariel', 'Drive'),
    ('Limpieza', '30-71001007-8', 'Limpiador de Pisos Aromático', 'Botella 900ml', 1500.00, 21.00, 'Poett', 'Procenex', 'Lysoform', 'Pinoluz'),

    -- 11. HIGIENE
    ('Higiene', '30-71001002-7', 'Papel Higiénico Doble Hoja', 'Pack x 4 Rollos', 2450.00, 21.00, 'Higienol', 'Elite', 'Felpita', 'Campanita'),
    ('Higiene', '30-71001002-7', 'Jabón de Tocador Cremoso', 'Pack x 3 u.', 1950.00, 21.00, 'Dove', 'Lux', 'Palmolive', 'Rexona'),
    ('Higiene', '30-71001002-7', 'Pasta Dental Protección Total', 'Pomo 90g', 2200.00, 21.00, 'Colgate', 'Oral-B', 'Sensodyne', 'Odol'),
    ('Higiene', '30-71001002-7', 'Rollo de Cocina Absorbente', 'Pack x 3 Rollos', 2100.00, 21.00, 'Sussex', 'Elite', 'Elegante', 'Felpita');

    DECLARE @TotalPlantillas INT = (SELECT COUNT(*) FROM @MatrizRubros);

    DECLARE @ProductosLote TABLE (
        idx INT IDENTITY(1,1),
        id_producto INT,
        precio_venta DECIMAL(12,2),
        costo_compra DECIMAL(12,2)
    );

    DECLARE @p INT = 1;
    WHILE @p <= 1000
    BEGIN
        DECLARE @CodBarra VARCHAR(50) = '7791000' + RIGHT('000000' + CAST(@p AS VARCHAR(10)), 6);
        DECLARE @IdProdExistente INT = (SELECT id_producto FROM producto WHERE codigo_barra = @CodBarra);

        -- Seleccionar plantilla de rubro para que categoría, producto, marca y proveedor coincidan 100%
        DECLARE @PlantillaId INT = ((@p - 1) % @TotalPlantillas) + 1;
        DECLARE @VarianteMarca INT = (((@p - 1) / @TotalPlantillas) % 4) + 1;

        DECLARE @CatNombre VARCHAR(100), @CuitProv VARCHAR(30), @NombreBase VARCHAR(60),
                @Presentacion VARCHAR(40), @PrecioBase DECIMAL(12,2), @IvaProd DECIMAL(5,2),
                @MarcaElegida VARCHAR(40);

        SELECT 
            @CatNombre = categoria_nombre,
            @CuitProv = cuit_proveedor,
            @NombreBase = nombre_base,
            @Presentacion = presentacion,
            @PrecioBase = precio_base,
            @IvaProd = iva,
            @MarcaElegida = CASE @VarianteMarca
                WHEN 1 THEN marca1
                WHEN 2 THEN marca2
                WHEN 3 THEN marca3
                ELSE marca4
            END
        FROM @MatrizRubros
        WHERE id = @PlantillaId;

        -- Resolver el id_categoria e id_proveedor exactos de ese rubro
        DECLARE @IdCatProd INT = (SELECT TOP 1 id_categoria FROM categoria WHERE nombre = @CatNombre);
        IF @IdCatProd IS NULL SET @IdCatProd = (SELECT TOP 1 id_categoria FROM categoria ORDER BY id_categoria);

        DECLARE @IdProvProd INT = (SELECT TOP 1 id_proveedor FROM proveedor WHERE cuit = @CuitProv);
        IF @IdProvProd IS NULL SET @IdProvProd = (SELECT TOP 1 id_proveedor FROM proveedor ORDER BY id_proveedor);

        DECLARE @PrecioVentaProd DECIMAL(12,2) = ROUND(@PrecioBase + ((@p % 10) * 55.00), 2);
        DECLARE @CostoCompraProd DECIMAL(12,2) = ROUND(@PrecioVentaProd * 0.65, 2);
        DECLARE @NombreFinal VARCHAR(100) = LEFT(@NombreBase + ' ' + @MarcaElegida + ' ' + @Presentacion + ' #' + CAST(@p AS VARCHAR(10)), 100);
        DECLARE @DescFinal VARCHAR(255) = 'Rubro ' + @CatNombre + ': ' + @NombreBase + ' marca ' + @MarcaElegida + ' (' + @Presentacion + ') - SKU ' + @CodBarra;

        IF @IdProdExistente IS NULL
        BEGIN
            INSERT INTO producto (codigo_barra, nombre, descripcion, precio_venta_actual, porcentaje_iva, id_categoria, id_proveedor, activo)
            VALUES (@CodBarra, @NombreFinal, @DescFinal, @PrecioVentaProd, @IvaProd, @IdCatProd, @IdProvProd, 1);

            SET @IdProdExistente = SCOPE_IDENTITY();
        END
        ELSE
        BEGIN
            -- Si el producto del lote ya había sido insertado antes, actualiza su categoría, marca y proveedor coherentes
            UPDATE producto
            SET nombre = @NombreFinal,
                descripcion = @DescFinal,
                precio_venta_actual = @PrecioVentaProd,
                porcentaje_iva = @IvaProd,
                id_categoria = @IdCatProd,
                id_proveedor = @IdProvProd
            WHERE id_producto = @IdProdExistente;
        END

        -- Asegurar registro inicial de stock en la sucursal
        IF NOT EXISTS (SELECT 1 FROM stock_sucursal WHERE id_producto = @IdProdExistente AND id_sucursal = @IdSucursal)
        BEGIN
            INSERT INTO stock_sucursal (id_producto, id_sucursal, stock_actual, stock_minimo)
            VALUES (@IdProdExistente, @IdSucursal, 40 + (@p % 60), 10);
        END

        INSERT INTO @ProductosLote (id_producto, precio_venta, costo_compra)
        VALUES (@IdProdExistente, @PrecioVentaProd, @CostoCompraProd);

        SET @p = @p + 1;
    END

    -- =========================================================================
    -- DEFINICIÓN DEL PERÍODO TEMPORAL: DICIEMBRE DEL AÑO PASADO HASTA ESTE MES
    -- =========================================================================
    DECLARE @FechaInicioPeriodo DATETIME2(0) = DATEFROMPARTS(YEAR(GETDATE()) - 1, 12, 1); -- 1 de Diciembre del año pasado
    DECLARE @FechaFinPeriodo DATETIME2(0) = SYSDATETIME();                                -- Fecha/hora actual de este mes
    DECLARE @TotalSegundosPeriodo BIGINT = DATEDIFF_BIG(SECOND, @FechaInicioPeriodo, @FechaFinPeriodo);
    IF @TotalSegundosPeriodo <= 0 SET @TotalSegundosPeriodo = 28000000;

    -- Los 15 usuarios dados de baja operaron durante el primer 65% del período antes de su cese
    DECLARE @SegundosCorteBaja BIGINT = (@TotalSegundosPeriodo * 65) / 100;

    -- =========================================================================
    -- PASO 3: GENERACIÓN DE 2.000 COMPRAS Y SUS DETALLES + ACTUALIZACIÓN DE STOCK
    -- =========================================================================
    PRINT 'Paso 3/4 -> Generando 2.000 compras de los 1.000 productos y actualizando stock...';

    DECLARE @c INT = 1;
    WHILE @c <= 2000
    BEGIN
        DECLARE @OrdenEmpCompra INT;
        DECLARE @SegundosOffsetCompra BIGINT;

        IF @c <= 600
        BEGIN
            SET @OrdenEmpCompra = ((@c - 1) % 15) + 1; -- Empleados 1 a 15 (dados de baja)
            SET @SegundosOffsetCompra = (@c * @SegundosCorteBaja) / 600;
        END
        ELSE
        BEGIN
            SET @OrdenEmpCompra = 15 + (((@c - 601) % 35) + 1); -- Empleados 16 a 50 (activos)
            SET @SegundosOffsetCompra = (@c * @TotalSegundosPeriodo) / 2000;
        END

        DECLARE @IdEmpleadoCompra INT = (SELECT id_empleado FROM @NuevosEmpleados WHERE nro_orden = @OrdenEmpCompra);
        DECLARE @FechaCompra DATETIME2(0) = DATEADD(SECOND, @SegundosOffsetCompra, @FechaInicioPeriodo);
        DECLARE @IdProvCompra INT = (SELECT id_proveedor FROM @ProveedoresDisponibles WHERE idx = ((@c - 1) % @CantProveedores) + 1);
        DECLARE @NroComprobante VARCHAR(50) = 'FC-A-0002-' + RIGHT('00000000' + CAST(200000 + @c AS VARCHAR(10)), 8);

        -- Rotar sobre los 1.000 productos para que todos tengan compras registradas
        DECLARE @IdxProdC1 INT = ((@c - 1) % 1000) + 1;
        DECLARE @IdxProdC2 INT = ((@c + 499) % 1000) + 1;

        DECLARE @IdProdC1 INT, @CostoC1 DECIMAL(12,2), @CantC1 INT;
        SELECT @IdProdC1 = id_producto, @CostoC1 = costo_compra FROM @ProductosLote WHERE idx = @IdxProdC1;
        SET @CantC1 = 12 + (@c % 20);

        DECLARE @IdProdC2 INT, @CostoC2 DECIMAL(12,2), @CantC2 INT;
        SELECT @IdProdC2 = id_producto, @CostoC2 = costo_compra FROM @ProductosLote WHERE idx = @IdxProdC2;
        SET @CantC2 = 8 + ((@c * 3) % 15);

        DECLARE @TotalCompra DECIMAL(12,2) = (@CantC1 * @CostoC1) + (@CantC2 * @CostoC2);

        INSERT INTO compra (fecha_hora, total, numero_comprobante, id_proveedor, id_empleado, id_sucursal)
        VALUES (@FechaCompra, @TotalCompra, @NroComprobante, @IdProvCompra, @IdEmpleadoCompra, @IdSucursal);

        DECLARE @IdCompraNueva INT = SCOPE_IDENTITY();

        INSERT INTO detalle_compra (cantidad, precio_unitario_compra, id_compra, id_producto)
        VALUES 
        (@CantC1, @CostoC1, @IdCompraNueva, @IdProdC1),
        (@CantC2, @CostoC2, @IdCompraNueva, @IdProdC2);

        UPDATE stock_sucursal SET stock_actual = stock_actual + @CantC1 WHERE id_producto = @IdProdC1 AND id_sucursal = @IdSucursal;
        UPDATE stock_sucursal SET stock_actual = stock_actual + @CantC2 WHERE id_producto = @IdProdC2 AND id_sucursal = @IdSucursal;

        SET @c = @c + 1;
    END

    -- =========================================================================
    -- PASO 4: GENERACIÓN DE SESIONES DE CAJA Y 2.000 VENTAS (DICIEMBRE A HOY)
    -- =========================================================================
    PRINT 'Paso 4/4 -> Generando sesiones de caja asignadas y 2.000 ventas de los 1.000 productos...';

    DECLARE @SesionesLote TABLE (
        nro_sesion INT,
        id_caja_sesion INT,
        fecha_sesion DATETIME2(0)
    );

    -- 100 Sesiones de caja distribuidas en las 5 cajas físicas y los 50 empleados
    DECLARE @s INT = 1;
    WHILE @s <= 100
    BEGIN
        DECLARE @OrdenEmpSesion INT;
        DECLARE @SegundosSesion BIGINT;

        IF @s <= 30
        BEGIN
            SET @OrdenEmpSesion = ((@s - 1) % 15) + 1; -- Empleados dados de baja (1 al 15)
            SET @SegundosSesion = ((@s - 1) * @SegundosCorteBaja) / 30;
        END
        ELSE
        BEGIN
            SET @OrdenEmpSesion = 15 + (((@s - 31) % 35) + 1); -- Empleados activos (16 al 50)
            SET @SegundosSesion = ((@s - 1) * @TotalSegundosPeriodo) / 100;
        END

        DECLARE @IdEmpSesion INT = (SELECT id_empleado FROM @NuevosEmpleados WHERE nro_orden = @OrdenEmpSesion);
        DECLARE @IdCajaAsignada INT = (SELECT id_caja FROM @CajasDisponibles WHERE idx = ((@s - 1) % @CantCajas) + 1);
        DECLARE @FechaAperturaSesion DATETIME2(0) = DATEADD(SECOND, @SegundosSesion, @FechaInicioPeriodo);
        DECLARE @FechaCierreSesion DATETIME2(0) = DATEADD(HOUR, 8, @FechaAperturaSesion);
        IF @FechaCierreSesion > @FechaFinPeriodo SET @FechaCierreSesion = @FechaFinPeriodo;

        INSERT INTO caja_sesion (fecha_apertura, fecha_cierre, monto_apertura, monto_cierre_real, diferencia_cierre, estado, id_caja, id_empleado)
        VALUES (@FechaAperturaSesion, @FechaCierreSesion, 30000.00, 30000.00, 0.00, 0, @IdCajaAsignada, @IdEmpSesion);

        INSERT INTO @SesionesLote (nro_sesion, id_caja_sesion, fecha_sesion)
        VALUES (@s, SCOPE_IDENTITY(), @FechaAperturaSesion);

        SET @s = @s + 1;
    END

    -- Insertar las 2.000 ventas rotando sobre los 1.000 productos
    DECLARE @v INT = 1;
    WHILE @v <= 2000
    BEGIN
        DECLARE @SegundosOffsetVenta BIGINT = ((CAST(@v - 1 AS BIGINT)) * @TotalSegundosPeriodo) / 2000;
        DECLARE @FechaVenta DATETIME2(0) = DATEADD(SECOND, @SegundosOffsetVenta, @FechaInicioPeriodo);

        DECLARE @NroSesionAsignada INT = ((@v - 1) / 20) + 1;
        DECLARE @IdCajaSesionVenta INT = (SELECT id_caja_sesion FROM @SesionesLote WHERE nro_sesion = @NroSesionAsignada);

        -- Rotar sobre los 1.000 productos para que todos registren ventas
        DECLARE @IdxProdV1 INT = ((@v - 1) % 1000) + 1;
        DECLARE @IdxProdV2 INT = ((@v + 249) % 1000) + 1;

        DECLARE @IdProdV1 INT, @PrecioV1 DECIMAL(12,2), @CantV1 INT;
        SELECT @IdProdV1 = id_producto, @PrecioV1 = precio_venta FROM @ProductosLote WHERE idx = @IdxProdV1;
        SET @CantV1 = 1 + (@v % 4);

        DECLARE @IdProdV2 INT, @PrecioV2 DECIMAL(12,2), @CantV2 INT;
        SELECT @IdProdV2 = id_producto, @PrecioV2 = precio_venta FROM @ProductosLote WHERE idx = @IdxProdV2;
        SET @CantV2 = 1 + ((@v * 2) % 3);

        DECLARE @SubtotalVenta DECIMAL(12,2) = (@CantV1 * @PrecioV1) + (@CantV2 * @PrecioV2);
        DECLARE @TotalVentaFinal DECIMAL(12,2) = @SubtotalVenta;

        INSERT INTO venta (fecha_hora, subtotal, descuento_total, total_venta, estado, id_caja_sesion, id_cliente)
        VALUES (@FechaVenta, @SubtotalVenta, 0.00, @TotalVentaFinal, 1, @IdCajaSesionVenta, NULL);

        DECLARE @IdVentaNueva INT = SCOPE_IDENTITY();

        INSERT INTO detalle_venta (cantidad, precio_unitario_historico, descuento, id_venta, id_producto)
        VALUES 
        (@CantV1, @PrecioV1, 0.00, @IdVentaNueva, @IdProdV1),
        (@CantV2, @PrecioV2, 0.00, @IdVentaNueva, @IdProdV2);

        UPDATE stock_sucursal
        SET stock_actual = CASE WHEN stock_actual > @CantV1 THEN stock_actual - @CantV1 ELSE stock_minimo + 15 END
        WHERE id_producto = @IdProdV1 AND id_sucursal = @IdSucursal;

        UPDATE stock_sucursal
        SET stock_actual = CASE WHEN stock_actual > @CantV2 THEN stock_actual - @CantV2 ELSE stock_minimo + 15 END
        WHERE id_producto = @IdProdV2 AND id_sucursal = @IdSucursal;

        DECLARE @IdMetodoPagoVenta INT = (SELECT id_metodo_pago FROM @MetodosPagoDisponibles WHERE idx = ((@v - 1) % @CantMetodosPago) + 1);
        DECLARE @RefPago VARCHAR(100) = CASE WHEN @IdMetodoPagoVenta = 1 THEN NULL ELSE 'OP-' + CAST(900000 + @v AS VARCHAR(20)) END;

        INSERT INTO pago (monto, referencia_transaccion, id_metodo_pago, id_venta)
        VALUES (@TotalVentaFinal, @RefPago, @IdMetodoPagoVenta, @IdVentaNueva);

        SET @v = @v + 1;
    END

    -- Cuadrar monto_cierre_real de las 100 sesiones de caja según sus ventas en efectivo
    UPDATE cs
    SET cs.monto_cierre_real = cs.monto_apertura + ISNULL((
            SELECT SUM(p.monto)
            FROM venta v
            INNER JOIN pago p ON p.id_venta = v.id_venta
            WHERE v.id_caja_sesion = cs.id_caja_sesion AND p.id_metodo_pago = 1
        ), 0),
        cs.diferencia_cierre = 0.00
    FROM caja_sesion cs
    INNER JOIN @SesionesLote sl ON cs.id_caja_sesion = sl.id_caja_sesion;

    COMMIT TRANSACTION;

    PRINT '=========================================================================';
    PRINT '¡CARGA MASIVA INTEGRAL COMPLETADA CON ÉXITO!';
    PRINT '  -> 5 Cajas operativas y 10 Proveedores asegurados';
    PRINT '  -> 50 Usuarios (15 dados de baja con historial + 35 activos) y Admin (43000111)';
    PRINT '  -> 1.000 Productos clasificados estrictamente por su categoría real';
    PRINT '  -> 2.000 Compras y 4.000 renglones en detalle_compra';
    PRINT '  -> 100 Sesiones de Caja y 2.000 Ventas (4.000 renglones en detalle_venta)';
    PRINT '=========================================================================';
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
    DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
    DECLARE @ErrorState INT = ERROR_STATE();

    RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
END CATCH;
GO
