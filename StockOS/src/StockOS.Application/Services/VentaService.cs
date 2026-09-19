using StockOS.Domain.Entities;
using StockOS.Domain.Interfaces;
using System.Collections.Generic;
using StockOS.Domain.Enums;

namespace StockOS.Application.Services
{
    public class VentaService : IVentaService
    {
        private readonly IVentaRepository _ventaRepo;
        private readonly IAuthorizationService _authService;

        public VentaService(IVentaRepository ventaRepo, IAuthorizationService authService)
        {
            _ventaRepo = ventaRepo;
            _authService = authService;
        }

        public int RegistrarVenta(Venta cabecera, List<DetalleVenta> detalles, int idSucursal, int idMetodoPago)
        {
            _authService.ValidarPermiso(Permisos.VENTAS_REALIZAR);
            return _ventaRepo.RegistrarVenta(cabecera, detalles, idSucursal, idMetodoPago);
        }
    }
}