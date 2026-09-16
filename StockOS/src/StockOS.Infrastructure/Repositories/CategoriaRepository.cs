using Microsoft.EntityFrameworkCore;
using StockOS.Domain.Entities;
using StockOS.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockOS.DataAccess.Persistence; // pa que reconozca StockOsContext

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
            return _context.Categorias.ToList();
        }
    }
}
