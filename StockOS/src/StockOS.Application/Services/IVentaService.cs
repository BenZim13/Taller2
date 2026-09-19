using System.Collections.Generic;
using StockOS.Domain.Entities;

namespace StockOS.Application.Services
{
    public interface IVentaService
    {
        int RegistrarVenta(Venta cabecera, List<DetalleVenta> detalles, int idSucursal, int idMetodoPago);
    }
}