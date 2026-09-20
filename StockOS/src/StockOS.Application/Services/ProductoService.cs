using System.Collections.Generic;
using StockOS.Domain.Entities;
using StockOS.Domain.Interfaces;
using StockOS.Domain.Enums;

namespace StockOS.Application.Services
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _productoRepository;
        private readonly IAuthorizationService _authService; // 1. Declaramos el guardián

        // 2. Lo inyectamos en el constructor
        public ProductoService(IProductoRepository productoRepository, IAuthorizationService authService)
        {
            _productoRepository = productoRepository;
            _authService = authService;
        }

        public IEnumerable<Producto> ObtenerTodos()
        {
            _authService.ValidarPermiso(Permisos.PRODUCTOS_VER);
            return _productoRepository.ObtenerTodos();
        }

        public Producto? BuscarPorCodigoBarra(string codigoBarra)
        {
            _authService.ValidarPermiso(Permisos.PRODUCTOS_VER);
            if (string.IsNullOrWhiteSpace(codigoBarra)) return null;
            return _productoRepository.BuscarPorCodigoBarra(codigoBarra.Trim());
        }

        public void Agregar(Producto producto)
        {
            // 3. Blindaje de creación
            _authService.ValidarPermiso(Permisos.PRODUCTOS_CREAR);

            if (producto == null) throw new System.ArgumentNullException(nameof(producto));
            if (string.IsNullOrWhiteSpace(producto.CodigoBarra)) throw new System.ArgumentException("El código de barra del producto es obligatorio.");
            if (string.IsNullOrWhiteSpace(producto.Nombre)) throw new System.ArgumentException("El nombre del producto es obligatorio.");
            if (producto.PrecioVentaActual <= 0) throw new System.ArgumentException("El precio de venta debe ser un número mayor a cero.");

            var existente = _productoRepository.BuscarPorCodigoBarra(producto.CodigoBarra.Trim());

            if (existente != null)
            {
                throw new System.InvalidOperationException($"Ya existe un producto registrado con el código '{producto.CodigoBarra}'.");
            }

            _productoRepository.Agregar(producto);
        }

        public void Actualizar(Producto producto)
        {
            // 3. Blindaje de edición
            _authService.ValidarPermiso(Permisos.PRODUCTOS_EDITAR);

            if (producto == null) throw new System.ArgumentNullException(nameof(producto));
            if (string.IsNullOrWhiteSpace(producto.CodigoBarra)) throw new System.ArgumentException("El código de barra del producto es obligatorio.");
            if (string.IsNullOrWhiteSpace(producto.Nombre)) throw new System.ArgumentException("El nombre del producto es obligatorio.");
            if (producto.PrecioVentaActual <= 0) throw new System.ArgumentException("El precio de venta debe ser un número mayor a cero.");

            var existente = _productoRepository.BuscarPorCodigoBarra(producto.CodigoBarra.Trim());

            if (existente != null && existente.IdProducto != producto.IdProducto)
            {
                throw new System.InvalidOperationException($"El código '{producto.CodigoBarra}' ya pertenece a otro producto ('{existente.Nombre}').");
            }

            _productoRepository.Actualizar(producto);
        }

        public void CambiarEstado(int idProducto, bool activo)
        {
            // Apagar o prender un producto cuenta como edición
            _authService.ValidarPermiso(Permisos.PRODUCTOS_EDITAR);
            _productoRepository.CambiarEstado(idProducto, activo);
        }
    }
}