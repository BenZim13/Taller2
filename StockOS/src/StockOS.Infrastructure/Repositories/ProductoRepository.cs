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

        public IEnumerable<Producto> ObtenerTodos()
        {
            // 1. Cargamos las categorías en la memoria interna de EF usando nuestro SP
            _context.Categorias.FromSqlRaw("EXEC sp_Categorias_ObtenerTodas").Load();

            // 2. Traemos los productos (SIN el .Include). 
            // EF Core los conectará automáticamente con las categorías que ya están en memoria.
            return _context.Productos
                .FromSqlRaw("EXEC sp_Productos_ObtenerTodos")
                .ToList();
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

            // Ejecutamos el SP
            _context.Database.ExecuteSqlRaw(
                "EXEC sp_Productos_Insertar @CodigoBarra={0}, @Nombre={1}, @Descripcion={2}, @PrecioVentaActual={3}, @PorcentajeIva={4}, @IdCategoria={5}, @IdProducto=@IdProducto OUTPUT",
                producto.CodigoBarra,
                producto.Nombre,
                producto.Descripcion ?? "", // Manejo de nulos por si no tiene descripción
                producto.PrecioVentaActual,
                producto.PorcentajeIva,
                producto.IdCategoria,
                idParam);

            // Asignamos el nuevo ID al objeto
            producto.IdProducto = (int)idParam.Value;
        }

        public void Actualizar(Producto producto)
        {
            // Ejecutamos el SP de actualización (Sirve tanto para modificar datos como para cambiar el estado Activo/Inactivo)
            _context.Database.ExecuteSqlRaw(
                "EXEC sp_Productos_Actualizar @IdProducto={0}, @CodigoBarra={1}, @Nombre={2}, @Descripcion={3}, @PrecioVentaActual={4}, @PorcentajeIva={5}, @IdCategoria={6}, @Activo={7}",
                producto.IdProducto,
                producto.CodigoBarra,
                producto.Nombre,
                producto.Descripcion ?? "",
                producto.PrecioVentaActual,
                producto.PorcentajeIva,
                producto.IdCategoria,
                producto.Activo);
        }
    }
}