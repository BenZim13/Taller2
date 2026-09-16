using System.Collections.Generic;
using StockOS.Domain.Entities;

namespace StockOS.Application.Services
{
    public interface ICajaService
    {
        int AbrirCaja(int idCaja, int idEmpleado, decimal montoApertura);
        IEnumerable<Caja> ObtenerCajasPorSucursal(int idSucursal);

        bool VerificarCajaAbierta(int idEmpleado);
    }
}
