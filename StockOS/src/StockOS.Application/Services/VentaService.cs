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

        public int ProcesarNuevaVenta(Venta cabecera, List<DetalleVenta> detalles, int idSucursal)
        {
            // Aca en el futuro podemo agregar validaciones lógicas antes de ir a BD (asi no usamos tantos SP's Benja)
            // Por ejemplo: if (detalles.Count == 0) throw new Exception("La venta está vacía");

            return _ventaRepo.RegistrarVenta(cabecera, detalles, idSucursal);
        }
    }
}