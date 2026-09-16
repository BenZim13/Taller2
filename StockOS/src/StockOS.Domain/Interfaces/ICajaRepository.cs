using System.Collections.Generic;
using StockOS.Domain.Entities;

namespace StockOS.Domain.Interfaces
{
    public interface ICajaRepository
    {
        IEnumerable<Caja> ObtenerPorSucursal(int idSucursal);
    }
}