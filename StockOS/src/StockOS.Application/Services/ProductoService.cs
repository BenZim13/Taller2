using System.Collections.Generic;
using StockOS.Domain.Entities;
using StockOS.Domain.Interfaces;
using StockOS.Domain.Enums;

namespace StockOS.Application.Services
{
    /// <summary>
    /// Servicio de gestión de productos con validaciones de negocio y control de permisos.
    /// </summary>
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _productoRepository;
        private readonly IAuthorizationService _authService;
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
            _authService.ValidarPermiso(Permisos.PRODUCTOS_CREAR);

            ValidarReglasDeNegocio(producto);

            // Verificar que el código de barras no esté duplicado
            var existente = _productoRepository.BuscarPorCodigoBarra(producto.CodigoBarra);
            if (existente != null)
            {
                throw new System.InvalidOperationException($"Ya existe un producto registrado con el código '{producto.CodigoBarra}'.");
            }

            _productoRepository.Agregar(producto);
        }

        public void Actualizar(Producto producto)
        {
            _authService.ValidarPermiso(Permisos.PRODUCTOS_EDITAR);

            ValidarReglasDeNegocio(producto);

            // Verificar que el código no pertenezca a otro producto
            var existente = _productoRepository.BuscarPorCodigoBarra(producto.CodigoBarra);
            if (existente != null && existente.IdProducto != producto.IdProducto)
            {
                throw new System.InvalidOperationException($"El código '{producto.CodigoBarra}' ya pertenece a otro producto ('{existente.Nombre}').");
            }

            _productoRepository.Actualizar(producto);
        }
        private void ValidarReglasDeNegocio(Producto producto)
        {
            if (producto == null) throw new System.ArgumentNullException(nameof(producto));

            producto.CodigoBarra = producto.CodigoBarra?.Trim() ?? "";
            producto.Nombre = producto.Nombre?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(producto.CodigoBarra))
                throw new System.ArgumentException("El código de barra es obligatorio.");

            // Validar que sea alfanumérico (sin espacios ni caracteres especiales)
            if (!producto.CodigoBarra.All(char.IsLetterOrDigit))
                throw new System.ArgumentException("El código de barra solo puede contener letras y números, sin espacios.");

            if (producto.CodigoBarra.Length > 50)
                throw new System.ArgumentException("El código de barra no puede superar los 50 caracteres.");

            if (string.IsNullOrWhiteSpace(producto.Nombre))
                throw new System.ArgumentException("El nombre del producto es obligatorio.");

            if (producto.Nombre.Length > 100)
                throw new System.ArgumentException("El nombre no puede superar los 100 caracteres.");

            if (producto.PrecioVentaActual <= 0)
                throw new System.ArgumentException("El precio de venta debe ser mayor a cero.");

            if (producto.IdCategoria <= 0)
                throw new System.ArgumentException("Debe asignar una categoría válida al producto.");

            if (producto.PorcentajeIva <= 0)
            {
                producto.PorcentajeIva = Producto.IvaFijoDefault;
            }
        }

        public void CambiarEstado(int idProducto, bool activo)
        {
            // Activar/desactivar requiere permiso de edición
            _authService.ValidarPermiso(Permisos.PRODUCTOS_EDITAR);
            _productoRepository.CambiarEstado(idProducto, activo);
        }
    }
}