using System;
using System.Collections.Generic;
using StockOS.Domain.Entities;
using StockOS.Domain.Interfaces;
using StockOS.Domain.Enums;

namespace StockOS.Application.Services
{
    public class ProveedorService : IProveedorService
    {
        private readonly IProveedorRepository _proveedorRepository;
        private readonly IAuthorizationService _authService;

        public ProveedorService(IProveedorRepository proveedorRepository, IAuthorizationService authService)
        {
            _proveedorRepository = proveedorRepository;
            _authService = authService;
        }

        public IEnumerable<Proveedor> ObtenerTodos()
        {
            return _proveedorRepository.ObtenerTodos();
        }

        public void Agregar(Proveedor proveedor)
        {
            _authService.ValidarPermiso(Permisos.PROVEEDORES_GESTIONAR);

            if (proveedor == null)
            {
                throw new ArgumentNullException(nameof(proveedor));
            }

            if (string.IsNullOrWhiteSpace(proveedor.RazonSocial))
            {
                throw new ArgumentException("El nombre o razón social del proveedor es obligatorio.");
            }

            // Sanitizar valores por defecto para columnas no nulas
            proveedor.RazonSocial = proveedor.RazonSocial.Trim();
            proveedor.Cuit = string.IsNullOrWhiteSpace(proveedor.Cuit) ? null : proveedor.Cuit.Trim();
            proveedor.Telefono ??= "";
            proveedor.Email ??= "";
            proveedor.Direccion ??= "";

            _proveedorRepository.Agregar(proveedor);
        }

        public void Actualizar(Proveedor proveedor)
        {
            _authService.ValidarPermiso(Permisos.PROVEEDORES_GESTIONAR);

            if (proveedor == null)
            {
                throw new ArgumentNullException(nameof(proveedor));
            }

            if (string.IsNullOrWhiteSpace(proveedor.RazonSocial))
            {
                throw new ArgumentException("El nombre o razón social del proveedor es obligatorio.");
            }

            proveedor.RazonSocial = proveedor.RazonSocial.Trim();
            proveedor.Cuit = string.IsNullOrWhiteSpace(proveedor.Cuit) ? null : proveedor.Cuit.Trim();
            proveedor.Telefono ??= "";
            proveedor.Email ??= "";
            proveedor.Direccion ??= "";

            _proveedorRepository.Actualizar(proveedor);
        }

        public void CambiarEstado(int idProveedor, bool activo)
        {
            _authService.ValidarPermiso(Permisos.PROVEEDORES_GESTIONAR);
            _proveedorRepository.CambiarEstado(idProveedor, activo);
        }
    }
}

