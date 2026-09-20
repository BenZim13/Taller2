using System;
using System.Collections.Generic;
using StockOS.Domain.Entities;
using StockOS.Domain.Interfaces;
using StockOS.Domain.Enums; // Mapeo de permisos

namespace StockOS.Application.Services
{
    public class CajaService : ICajaService
    {
        private readonly ICajaSesionRepository _cajaSesionRepo;
        private readonly ICajaRepository _cajaRepo;
        private readonly IAuthorizationService _authService; // Guardián

        public CajaService(ICajaSesionRepository cajaSesionRepo, ICajaRepository cajaRepo, IAuthorizationService authService)
        {
            _cajaSesionRepo = cajaSesionRepo;
            _cajaRepo = cajaRepo;
            _authService = authService;
        }

        public int AbrirCaja(int idCaja, int idEmpleado, decimal montoApertura)
        {
            _authService.ValidarPermiso(Permisos.CAJA_ABRIR);

            // 1. BLOQUEO: Usamos tu método existente para verificar si ya tiene un turno abierto
            if (_cajaSesionRepo.VerificarCajaAbierta(idEmpleado))
            {
                throw new InvalidOperationException("Ya tienes un turno de caja abierto. Debes cerrarlo antes de iniciar uno nuevo.");
            }

            // 2. Si pasó el control, abrimos la caja pasándole los 3 parámetros como lo tenías
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

        public decimal ObtenerMontoEsperado(int idCajaSesion)
        {
            return _cajaRepo.ObtenerMontoEsperado(idCajaSesion);
        }

        public void CerrarCaja(int idCajaSesion, decimal montoCierreReal)
        {
            _authService.ValidarPermiso(Permisos.CAJA_CERRAR);

            if (montoCierreReal < 0) throw new Exception("El monto no puede ser negativo.");
            _cajaRepo.CerrarCaja(idCajaSesion, montoCierreReal);
        }

        public void RegistrarMovimiento(int idCajaSesion, string tipo, decimal monto, string descripcion)
        {
            _authService.ValidarPermiso(Permisos.CAJA_MOVIMIENTOS);

            if (monto <= 0) throw new Exception("El monto debe ser mayor a cero.");
            if (string.IsNullOrWhiteSpace(descripcion)) throw new Exception("Debe ingresar una descripción.");

            _cajaRepo.RegistrarMovimiento(idCajaSesion, tipo, monto, descripcion);
        }
        public int? ObtenerIdSesionAbierta(int idEmpleado)
        {
            return _cajaSesionRepo.ObtenerIdSesionAbierta(idEmpleado);
        }
    }
}