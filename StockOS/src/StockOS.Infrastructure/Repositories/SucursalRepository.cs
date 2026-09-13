using System.Collections.Generic;
using System.Linq;
using StockOS.Domain.Entities;
using StockOS.Domain.Interfaces;
using StockOS.DataAccess.Persistence;

namespace StockOS.DataAccess.Repositories
{
    public class SucursalRepository : ISucursalRepository
    {
        private readonly StockOsContext _context;

        public SucursalRepository(StockOsContext context)
        {
            _context = context;
        }

        public IEnumerable<Sucursal> ObtenerTodas()
        {
            // Traemos solo las sucursales activas (asumiendo que tienes un campo Activo/Estado)
            return _context.Sucursales.ToList();
        }
    }
}