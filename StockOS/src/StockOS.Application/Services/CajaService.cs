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
    }
}