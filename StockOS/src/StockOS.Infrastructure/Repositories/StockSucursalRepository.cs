using System.Linq;
using StockOS.DataAccess.Persistence;
using StockOS.Domain.Entities;
using StockOS.Domain.Interfaces;

namespace StockOS.DataAccess.Repositories
{
    public class StockSucursalRepository : IStockSucursalRepository
    {
        private readonly StockOsContext _context;

        public StockSucursalRepository(StockOsContext context)
        {
            _context = context;
        }

        public StockSucursal? ObtenerPorProductoYSucursal(int idProducto, int idSucursal)
        {
            // Busca si ya existe un registro previo para ese producto en esa sucursal
            return _context.StockSucursales // Nota: Revisa si en tu contexto se llama StockSucursal o StockSucursales
                .FirstOrDefault(s => s.IdProducto == idProducto && s.IdSucursal == idSucursal);
        }

        public void Agregar(StockSucursal stock)
        {
            _context.StockSucursales.Add(stock);
            _context.SaveChanges();
        }

        public void Actualizar(StockSucursal stock)
        {
            _context.StockSucursales.Update(stock);
            _context.SaveChanges();
        }
    }
}