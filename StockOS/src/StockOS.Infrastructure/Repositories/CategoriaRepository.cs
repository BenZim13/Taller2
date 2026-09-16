using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StockOS.DataAccess.Persistence;
using StockOS.Domain.Entities;
using StockOS.Domain.Interfaces;

namespace StockOS.DataAccess.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly StockOsContext _context;

        public CategoriaRepository(StockOsContext context)
        {
            _context = context;
        }

        public IEnumerable<Categoria> ObtenerTodos()
        {
            // Ejecutamos el SP para cargar el ComboBox
            return _context.Categorias 
                .FromSqlRaw("EXEC sp_Categorias_ObtenerTodas")
                .ToList();
        }
    }
}