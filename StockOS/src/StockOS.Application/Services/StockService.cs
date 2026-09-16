using StockOS.Domain.Entities;
using StockOS.Domain.Interfaces;

namespace StockOS.Application.Services
{
    public class StockService : IStockService
    {
        private readonly IStockSucursalRepository _stockRepo;

        public StockService(IStockSucursalRepository stockRepo)
        {
            _stockRepo = stockRepo;
        }

        public void AgregarStock(int idProducto, int idSucursal, int cantidadAAgregar)
        {
            // 1. Buscamos si el producto ya tiene stock en esta sucursal
            var stockActual = _stockRepo.ObtenerPorProductoYSucursal(idProducto, idSucursal);

            if (stockActual == null)
            {
                var nuevoStock = new StockSucursal
                {
                    IdProducto = idProducto,
                    IdSucursal = idSucursal,
                    StockActual = cantidadAAgregar,
                    StockMinimo = 5 // le ponemos 5, luego se puede cambiar
                };
                _stockRepo.Agregar(nuevoStock);
            }
            else
            {
                // 3. CAMBIO AQUÍ: Sumamos a StockActual
                stockActual.StockActual += cantidadAAgregar;
                _stockRepo.Actualizar(stockActual);
            }
        }

        public int ObtenerCantidadActual(int idProducto, int idSucursal)
        {
            var stock = _stockRepo.ObtenerPorProductoYSucursal(idProducto, idSucursal);

            // CAMBIO AQUÍ: Retornamos el StockActual
            return stock != null ? (int)stock.StockActual : 0;
        }
    }
}