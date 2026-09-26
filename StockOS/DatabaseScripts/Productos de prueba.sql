USE StockOS;
GO

-- ==========================================
-- LOTE DE PRUEBAS: PRODUCTOS
-- ==========================================
-- Se inserta 1 producto por cada categoría existente (ID del 1 al 11)
-- Todos asignados al Proveedor Predeterminado (ID 1)

INSERT INTO producto (codigo_barra, nombre, descripcion, precio_venta_actual, porcentaje_iva, id_categoria, id_proveedor, activo) 
VALUES 
('7790000000001', 'Pan Frances x 1kg', 'Pan horneado del día', 1500.00, 21.00, 1, 1, 1),
('7790000000002', 'Coca Cola 2.25L', 'Gaseosa sabor cola original', 2200.00, 21.00, 2, 1, 1),
('7790000000003', 'Carne Picada Especial 1kg', 'Carne de novillo sin grasa', 6500.00, 21.00, 3, 1, 1),
('7790000000004', 'Tomate Redondo x 1kg', 'Tomate fresco de huerta', 1200.00, 21.00, 4, 1, 1),
('7790000000005', 'Empanadas de Carne x 12', 'Docena de empanadas fritas listas', 8000.00, 21.00, 5, 1, 1),
('7790000000006', 'Jamon Cocido x 100g', 'Jamon primera marca feteado', 850.00, 21.00, 6, 1, 1),
('7790000000007', 'Fideos Tallarines 500g', 'Fideos de trigo candeal', 950.00, 21.00, 7, 1, 1),
('7790000000008', 'Leche Entera 1L', 'Leche sachet clásica', 1100.00, 21.00, 8, 1, 1),
('7790000000009', 'Desodorante Axe', 'Desodorante corporal spray 150ml', 2500.00, 21.00, 9, 1, 1),
('7790000000010', 'Lavandina 1L', 'Lavandina concentrada uso doméstico', 900.00, 21.00, 10, 1, 1),
('7790000000011', 'Jabon de Tocador x 3', 'Pack de jabones de tocador', 1800.00, 21.00, 11, 1, 1);
GO

-- ==========================================
-- LOTE DE PRUEBAS: INYECCIÓN DE STOCK
-- ==========================================
-- Le damos stock a los 11 productos creados arriba en la Casa Central (id_sucursal = 1)
-- Asumimos que los IDs de producto generados serán del 1 al 11 al ser los primeros en insertarse.

INSERT INTO stock_sucursal (id_producto, id_sucursal, stock_actual, stock_minimo) 
VALUES 
(1, 1, 50, 10),  -- Pan
(2, 1, 120, 24), -- Coca Cola
(3, 1, 30, 5),   -- Carne Picada
(4, 1, 40, 10),  -- Tomate
(5, 1, 20, 5),   -- Empanadas
(6, 1, 50, 10),  -- Jamon
(7, 1, 200, 30), -- Fideos
(8, 1, 80, 20),  -- Leche
(9, 1, 45, 10),  -- Desodorante
(10, 1, 60, 15), -- Lavandina
(11, 1, 100, 20); -- Jabon
GO