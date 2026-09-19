using System.Collections.Generic;
using StockOS.Domain.Entities;

namespace StockOS.Application.Services
{
    public interface ICajaService
    {
        int AbrirCaja(int idCaja, int idEmpleado, decimal montoApertura);
        IEnumerable<Caja> ObtenerCajasPorSucursal(int idSucursal);
        bool VerificarCajaAbierta(int idEmpleado);
        decimal ObtenerMontoEsperado(int idCajaSesion);
        void CerrarCaja(int idCajaSesion, decimal montoCierreReal);
        void RegistrarMovimiento(int idCajaSesion, string tipo, decimal monto, string descripcion);
    }
}
