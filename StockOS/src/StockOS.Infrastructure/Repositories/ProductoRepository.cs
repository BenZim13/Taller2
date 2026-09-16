using System.Collections.Generic;
using System.Linq;
using StockOS.Domain.Entities;
using StockOS.Domain.Interfaces;
using StockOS.DataAccess.Persistence; // Importante para que reconozca StockOsContext

namespace StockOS.DataAccess.Repositories
{
    public class ProductoRepository : IProductoRepository
    {
        // 1. Declaramos la variable
        private readonly StockOsContext _context;

        // 2. La recibimos en el constructor
        public ProductoRepository(StockOsContext context)
        {
            _context = context;
        }

        public IEnumerable<Producto> ObtenerTodos()
        {
            return _context.Productos.ToList(); // Ahora _context sí existe
        }
        public void Agregar(Producto producto)
        {
            _context.Productos.Add(producto);
            _context.SaveChanges();
        }

        public void Actualizar(Producto producto)
        {
            _context.Productos.Update(producto);
            _context.SaveChanges();
        }
    }
}