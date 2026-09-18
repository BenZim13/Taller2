using System;
using System.Collections.Generic;
using StockOS.Domain.Entities;
using StockOS.Domain.Interfaces;

namespace StockOS.Application.Services
{
    public class CajaService : ICajaService
    {
        private readonly ICajaSesionRepository _cajaSesionRepo;
        private readonly ICajaRepository _cajaRepo; 

        

        public CajaService(ICajaSesionRepository cajaSesionRepo, ICajaRepository cajaRepo)
        {
            _cajaSesionRepo = cajaSesionRepo;
            _cajaRepo = cajaRepo;
        }

        public int AbrirCaja(int idCaja, int idEmpleado, decimal montoApertura)
        {
            return _cajaSesionRepo.AbrirCaja(idCaja, idEmpleado, montoApertura);
        }

        public IEnumerable<Caja> ObtenerCajasPorSucursal(int idSucursal)
        {
            
            return _cajaRepo.ObtenerPorSucursal(idSucursal);
        }
        public bool VerificarCajaAbierta(int idEmpleado)
        {
            return _cajaSesionRepo.VerificarCajaAbierta(idEmpleado);
        }
        public void CerrarCaja(int idCajaSesion, decimal montoCierreReal)
        {
            if (montoCierreReal < 0) throw new Exception("El monto no puede ser negativo.");
            _cajaRepo.CerrarCaja(idCajaSesion, montoCierreReal);
        }
        public void RegistrarMovimiento(int idCajaSesion, string tipo, decimal monto, string descripcion)
        {
            if (monto <= 0) throw new Exception("El monto debe ser mayor a cero.");
            if (string.IsNullOrWhiteSpace(descripcion)) throw new Exception("Debe ingresar una descripción.");

            _cajaRepo.RegistrarMovimiento(idCajaSesion, tipo, monto, descripcion);
        }
    }
}