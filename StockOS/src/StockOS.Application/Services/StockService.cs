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
            // El servicio ahora es un simple "pasamanos", la base de datos hace la magia
            _stockRepo.IngresarMercaderia(idProducto, idSucursal, cantidadAAgregar);
        }

        public int ObtenerCantidadActual(int idProducto, int idSucursal)
        {
            return _stockRepo.ObtenerCantidadActual(idProducto, idSucursal);
        }
    }
}