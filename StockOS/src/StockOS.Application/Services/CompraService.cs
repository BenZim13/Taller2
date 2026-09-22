using System;
using StockOS.Domain.Entities;
using StockOS.Domain.Interfaces;
using StockOS.Domain.Enums;

namespace StockOS.Application.Services
{
    /// <summary>
    /// Servicio de gestión de compras a proveedores con actualización de precios de costo.
    /// </summary>
    public class CompraService : ICompraService
    {
        private readonly ICompraRepository _compraRepo;
        private readonly IAuthorizationService _authService;

        public CompraService(ICompraRepository compraRepo, IAuthorizationService authService)
        {
            _compraRepo = compraRepo;
            _authService = authService;
        }

        public int RegistrarCompra(Compra cabecera, DetalleCompra detalle)
        {
            _authService.ValidarPermiso(Permisos.COMPRAS_GESTIONAR);

            if (cabecera == null) throw new ArgumentNullException(nameof(cabecera));
            if (detalle == null) throw new ArgumentNullException(nameof(detalle));
            if (detalle.Cantidad <= 0) throw new ArgumentException("La cantidad comprada debe ser mayor a cero.");
            if (detalle.PrecioUnitarioCompra <= 0) throw new ArgumentException("El precio unitario de compra debe ser mayor a cero.");

            return _compraRepo.RegistrarCompra(cabecera, detalle);
        }

        public decimal? ObtenerUltimoPrecioCompra(int idProducto)
        {
            // No requiere permisos especiales: usado para cálculos de margen
            return _compraRepo.ObtenerUltimoPrecioCompra(idProducto);
        }

        public void ActualizarPrecioCompra(int idProducto, decimal nuevoPrecioCompra)
        {
            _authService.ValidarPermiso(Permisos.COMPRAS_GESTIONAR);
            _compraRepo.ActualizarPrecioCompra(idProducto, nuevoPrecioCompra);
        }
    }
}

