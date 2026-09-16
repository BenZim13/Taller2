using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StockOS.DataAccess.Persistence;
using StockOS.Domain.Entities;
using StockOS.Domain.Interfaces;

namespace StockOS.DataAccess.Repositories
{
    public class CajaRepository : ICajaRepository
    {
        private readonly StockOsContext _context;

        public CajaRepository(StockOsContext context)
        {
            _context = context;
        }

        public IEnumerable<Caja> ObtenerPorSucursal(int idSucursal)
        {
            // Ejecutamos el nuevo SP que creamos
            return _context.Cajas
                .FromSqlRaw("EXEC sp_Cajas_ObtenerPorSucursal @IdSucursal={0}", idSucursal)
                .ToList();
        }

        
    }
}