using System.Collections.Generic;
using StockOS.Domain.Entities;

namespace StockOS.Domain.Interfaces
{
    public interface IVentaRepository
    {
        // Devuelve el ID de la venta generada
        int RegistrarVenta(Venta cabecera, List<DetalleVenta> detalles, int idSucursal, int idMetodoPago);
    }
}