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

            if (montoApertura < 0)
            {
                throw new ArgumentException("El monto inicial de apertura no puede ser negativo.");
            }

            // 1. BLOQUEO: Verificar si el empleado ya tiene un turno abierto
            if (_cajaSesionRepo.VerificarCajaAbierta(idEmpleado))
            {
                throw new InvalidOperationException("Ya tienes un turno de caja abierto. Debes cerrarlo antes de iniciar uno nuevo.");
            }

            // 2. BLOQUEO: Verificar si la caja física seleccionada ya está abierta en otro turno
            if (_cajaSesionRepo.VerificarCajaFisicaAbierta(idCaja))
            {
                throw new InvalidOperationException("La caja seleccionada ya tiene un turno abierto por otro cajero. Debe cerrarse antes de poder utilizarla.");
            }

            // 3. Si pasó los controles, abrimos la caja en el repositorio
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

            if (montoCierreReal < 0) throw new ArgumentException("El monto no puede ser negativo.");
            _cajaRepo.CerrarCaja(idCajaSesion, montoCierreReal);
        }

        public void RegistrarMovimiento(int idCajaSesion, string tipo, decimal monto, string descripcion)
        {
            _authService.ValidarPermiso(Permisos.CAJA_MOVIMIENTOS);

            string tipoNormalizado = (tipo ?? "").Trim().ToUpper();
            if (tipoNormalizado != "INGRESO" && tipoNormalizado != "EGRESO")
            {
                throw new ArgumentException("El tipo de movimiento debe ser 'INGRESO' o 'EGRESO'.");
            }

            if (monto <= 0) throw new ArgumentException("El monto debe ser mayor a cero.");
            if (string.IsNullOrWhiteSpace(descripcion)) throw new ArgumentException("Debe ingresar una descripción.");

            _cajaRepo.RegistrarMovimiento(idCajaSesion, tipoNormalizado, monto, descripcion.Trim());
        }
        public int? ObtenerIdSesionAbierta(int idEmpleado)
        {
            return _cajaSesionRepo.ObtenerIdSesionAbierta(idEmpleado);
        }

        public void CerrarCajaPorCierreSesion(int idEmpleado)
        {
            var idCajaSesion = _cajaSesionRepo.ObtenerIdSesionAbierta(idEmpleado);
            if (idCajaSesion.HasValue && idCajaSesion.Value > 0)
            {
                decimal montoEsperado = _cajaRepo.ObtenerMontoEsperado(idCajaSesion.Value);
                _cajaRepo.CerrarCaja(idCajaSesion.Value, montoEsperado);
            }

            if (SesionActual.Usuario == null || SesionActual.Usuario.IdEmpleado == idEmpleado || (idCajaSesion.HasValue && SesionActual.IdCajaSesionAbierta == idCajaSesion))
            {
                SesionActual.IdCajaSesionAbierta = null;
            }
        }
    }
}