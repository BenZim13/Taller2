using System.Collections.Generic;
using StockOS.Domain.Entities;
using StockOS.Domain.Interfaces;

namespace StockOS.Application.Services
{
    public class VentaService : IVentaService
    {
        private readonly IVentaRepository _ventaRepo;

        public VentaService(IVentaRepository ventaRepo)
        {
            _ventaRepo = ventaRepo;
        }

        public int RegistrarVenta(Venta cabecera, List<DetalleVenta> detalles, int idSucursal)
        {
            return _ventaRepo.RegistrarVenta(cabecera, detalles, idSucursal);
        }
    }
}