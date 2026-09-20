using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StockOS.DataAccess.Persistence;
using StockOS.Domain.Entities;
using StockOS.Domain.Interfaces;

namespace StockOS.DataAccess.Repositories
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly StockOsContext _context;

        public ProductoRepository(StockOsContext context)
        {
            _context = context;
        }
        public Producto? BuscarPorCodigoBarra(string codigoBarra)
        {
            return _context.Productos
                .AsNoTracking()
                .FirstOrDefault(p => p.CodigoBarra == codigoBarra);
        }

        public IEnumerable<Producto> ObtenerTodos()
        {
            var categorias = _context.Categorias
                .FromSqlRaw("EXEC sp_Categorias_ObtenerTodas")
                .AsNoTracking()
                .ToDictionary(c => c.IdCategoria);

            var proveedores = _context.Proveedores
                .FromSqlRaw("EXEC sp_Proveedores_ObtenerTodos")
                .AsNoTracking()
                .ToDictionary(p => p.IdProveedor);

            var productos = _context.Productos
                .FromSqlRaw("EXEC sp_Productos_ObtenerTodos")
                .AsNoTracking()
                .ToList();

            foreach (var p in productos)
            {
                if (categorias.TryGetValue(p.IdCategoria, out var cat))
                {
                    p.IdCategoriaNavigation = cat;
                }

                if (p.IdProveedor.HasValue && proveedores.TryGetValue(p.IdProveedor.Value, out var prov))
                {
                    p.IdProveedorNavigation = prov;
                }
            }

            return productos;
        }

        public void Agregar(Producto producto)
        {
            // Preparamos el parámetro de salida para capturar el ID generado
            var idParam = new SqlParameter
            {
                ParameterName = "@IdProducto",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };

            var idProveedorParam = new SqlParameter("@IdProveedor", System.Data.SqlDbType.Int)
            {
                Value = producto.IdProveedor.HasValue ? (object)producto.IdProveedor.Value : DBNull.Value
            };

            // Ejecutamos el SP
            _context.Database.ExecuteSqlRaw(
                "EXEC sp_Productos_Insertar @CodigoBarra={0}, @Nombre={1}, @Descripcion={2}, @PrecioVentaActual={3}, @PorcentajeIva={4}, @IdCategoria={5}, @IdProveedor={6}, @IdProducto=@IdProducto OUTPUT",
                producto.CodigoBarra,
                producto.Nombre,
                producto.Descripcion ?? "", // Manejo de nulos por si no tiene descripción
                producto.PrecioVentaActual,
                producto.PorcentajeIva,
                producto.IdCategoria,
                idProveedorParam,
                idParam);

            // Asignamos el nuevo ID al objeto
            producto.IdProducto = (int)idParam.Value;
        }

        public void Actualizar(Producto producto)
        {
            var idProveedorParam = new SqlParameter("@IdProveedor", System.Data.SqlDbType.Int)
            {
                Value = producto.IdProveedor.HasValue ? (object)producto.IdProveedor.Value : DBNull.Value
            };

            // Ejecutamos el SP de actualización (Sirve tanto para modificar datos como para cambiar el estado Activo/Inactivo)
            _context.Database.ExecuteSqlRaw(
                "EXEC sp_Productos_Actualizar @IdProducto={0}, @CodigoBarra={1}, @Nombre={2}, @Descripcion={3}, @PrecioVentaActual={4}, @PorcentajeIva={5}, @IdCategoria={6}, @Activo={7}, @IdProveedor={8}",
                producto.IdProducto,
                producto.CodigoBarra,
                producto.Nombre,
                producto.Descripcion ?? "",
                producto.PrecioVentaActual,
                producto.PorcentajeIva,
                producto.IdCategoria,
                producto.Activo,
                idProveedorParam);
        }

        public void CambiarEstado(int idProducto, bool activo)
        {
            // [Procedimiento sp_Productos_CambiarEstado]
            _context.Database.ExecuteSqlRaw(
                "EXEC sp_Productos_CambiarEstado @IdProducto={0}, @Activo={1}",
                idProducto, activo);
        }
    }
}