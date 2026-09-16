using System.Collections.Generic;
using StockOS.Domain.Entities;

namespace StockOS.Application.Services
{
    public interface IVentaService
    {
        int ProcesarNuevaVenta(Venta cabecera, List<DetalleVenta> detalles, int idSucursal);
    }
}