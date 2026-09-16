using StockOS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockOS.Domain.Interfaces
{
    public interface IStockSucursalRepository
    {
        int ObtenerCantidadActual(int idProducto, int idSucursal);
        void IngresarMercaderia(int idProducto, int idSucursal, int cantidad);
    }
}
