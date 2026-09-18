using System.Collections.Generic;
using StockOS.Domain.Entities;

namespace StockOS.Domain.Interfaces
{
    public interface ICajaRepository
    {
        IEnumerable<Caja> ObtenerPorSucursal(int idSucursal);
        void CerrarCaja(int idCajaSesion, decimal montoCierreReal);
        void RegistrarMovimiento(int idCajaSesion, string tipo, decimal monto, string descripcion);
    }
}