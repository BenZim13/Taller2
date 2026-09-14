using System.Collections.Generic;
using System.Linq;
using StockOS.Domain.Entities;
using StockOS.Domain.Interfaces;
using StockOS.DataAccess.Persistence;

namespace StockOS.DataAccess.Repositories
{
    public class RolRepository : IRolRepository
    {
        private readonly StockOsContext _context;

        public RolRepository(StockOsContext context)
        {
            _context = context;
        }

        public IEnumerable<Rol> ObtenerTodos()
        {
            return _context.Roles.ToList();
        }
    }
}

