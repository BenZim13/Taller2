using StockOS.Domain.Interfaces;
using StockOS.Domain.Enums; // Mapeo de permisos

namespace StockOS.Application.Services
{
    public class StockService : IStockService
    {
        private readonly IStockSucursalRepository _stockRepo;
        private readonly IAuthorizationService _authService; // Guardián

        public StockService(IStockSucursalRepository stockRepo, IAuthorizationService authService)
        {
            _stockRepo = stockRepo;
            _authService = authService;
        }

        public void AgregarStock(int idProducto, int idSucursal, int cantidadAAgregar)
        {
            _authService.ValidarPermiso(Permisos.STOCK_INGRESAR);
        
            if (cantidadAAgregar <= 0)
            {
                throw new ArgumentException("La cantidad a ingresar debe ser mayor a cero.");
            }

            _stockRepo.IngresarMercaderia(idProducto, idSucursal, cantidadAAgregar);
        }

        public int ObtenerCantidadActual(int idProducto, int idSucursal)
        {
            _authService.ValidarPermiso(Permisos.STOCK_VER);
            return _stockRepo.ObtenerCantidadActual(idProducto, idSucursal);
        }
    }
}