using System.Collections.Generic;
using StockOS.Domain.Entities;

namespace StockOS.Application.Services
{
    public interface ISucursalService
    {
        IEnumerable<Sucursal> ObtenerSucursales();
    }
}