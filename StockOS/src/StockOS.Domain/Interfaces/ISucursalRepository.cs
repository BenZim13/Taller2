using System.Collections.Generic;
using StockOS.Domain.Entities;

namespace StockOS.Domain.Interfaces
{
    public interface ISucursalRepository
    {
        IEnumerable<Sucursal> ObtenerTodas();
    }
}