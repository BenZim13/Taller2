using System;
using System.Collections.Generic;
using StockOS.Domain.Entities;
using StockOS.Domain.Interfaces;
using StockOS.Domain.Enums;

namespace StockOS.Application.Services
{
    /// <summary>
    /// Servicio de registro de ventas con validaciones de integridad y control de permisos.
    /// </summary>
    public class VentaService : IVentaService
    {
        private readonly IVentaRepository _ventaRepo;
        private readonly IAuthorizationService _authService;

        public VentaService(IVentaRepository ventaRepo, IAuthorizationService authService)
        {
            _ventaRepo = ventaRepo;
            _authService = authService;
        }

        public int RegistrarVenta(Venta cabecera, List<DetalleVenta> detalles, int idSucursal, int idMetodoPago)
        {
            _authService.ValidarPermiso(Permisos.VENTAS_REALIZAR);

            // Validaciones de datos obligatorios
            if (cabecera == null)
            {
                throw new ArgumentNullException(nameof(cabecera), "La cabecera de la venta no puede ser nula.");
            }

            if (detalles == null || detalles.Count == 0)
            {
                throw new ArgumentException("La venta debe contener al menos un producto en el detalle.", nameof(detalles));
            }

            if (idSucursal <= 0)
            {
                throw new ArgumentException("Debe especificarse una sucursal válida.", nameof(idSucursal));
            }

            if (idMetodoPago <= 0)
            {
                throw new ArgumentException("Debe especificarse un método de pago válido.", nameof(idMetodoPago));
            }

            // Validar cada item del detalle
            foreach (var item in detalles)
            {
                if (item.Cantidad <= 0)
                {
                    throw new ArgumentException("La cantidad de cada producto a vender debe ser mayor a cero.");
                }

                if (item.PrecioUnitarioHistorico < 0)
                {
                    throw new ArgumentException("El precio unitario del producto no puede ser negativo.");
                }
            }

            return _ventaRepo.RegistrarVenta(cabecera, detalles, idSucursal, idMetodoPago);
        }
    }
}