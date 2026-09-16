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
        StockSucursal? ObtenerPorProductoYSucursal(int idProducto, int idSucursal);
        void Agregar(StockSucursal stock);
        void Actualizar(StockSucursal stock);
    }
}
