using System.Collections.Generic;
using System.Linq;
using StockOS.Domain.Entities;
using StockOS.Domain.Interfaces;

namespace StockOS.Application.Services
{
    public class ProductoService : IProductoService
    {
        // 1. Declaramos 
        private readonly IProductoRepository _productoRepository;

        // 2. recibimos en el constructor
        public ProductoService(IProductoRepository productoRepository)
        {
            _productoRepository = productoRepository;
        }

        public IEnumerable<Producto> ObtenerTodos()
        {
            return _productoRepository.ObtenerTodos(); 
        }
        public void Agregar(Producto producto)
        {
            if (producto == null)
                throw new System.ArgumentNullException(nameof(producto));

            if (string.IsNullOrWhiteSpace(producto.CodigoBarra))
                throw new System.ArgumentException("El código de barra del producto es obligatorio.");

            if (string.IsNullOrWhiteSpace(producto.Nombre))
                throw new System.ArgumentException("El nombre del producto es obligatorio.");

            if (producto.PrecioVentaActual <= 0)
                throw new System.ArgumentException("El precio de venta debe ser un número mayor a cero.");

            // Validar unicidad de código de barra
            var existente = _productoRepository.ObtenerTodos()
                .FirstOrDefault(p => p.CodigoBarra.Equals(producto.CodigoBarra.Trim(), System.StringComparison.OrdinalIgnoreCase));

            if (existente != null)
            {
                throw new System.InvalidOperationException($"Ya existe un producto registrado con el código '{producto.CodigoBarra}'.");
            }

            _productoRepository.Agregar(producto);
        }

        public void Actualizar(Producto producto)
        {
            if (producto == null)
                throw new System.ArgumentNullException(nameof(producto));

            if (string.IsNullOrWhiteSpace(producto.CodigoBarra))
                throw new System.ArgumentException("El código de barra del producto es obligatorio.");

            if (string.IsNullOrWhiteSpace(producto.Nombre))
                throw new System.ArgumentException("El nombre del producto es obligatorio.");

            if (producto.PrecioVentaActual <= 0)
                throw new System.ArgumentException("El precio de venta debe ser un número mayor a cero.");

            // Validar que el código no colisione con otro producto diferente
            var existente = _productoRepository.ObtenerTodos()
                .FirstOrDefault(p => p.IdProducto != producto.IdProducto && 
                                     p.CodigoBarra.Equals(producto.CodigoBarra.Trim(), System.StringComparison.OrdinalIgnoreCase));

            if (existente != null)
            {
                throw new System.InvalidOperationException($"El código '{producto.CodigoBarra}' ya pertenece a otro producto ('{existente.Nombre}').");
            }

            _productoRepository.Actualizar(producto);
        }

        public void CambiarEstado(int idProducto, bool activo)
        {
            _productoRepository.CambiarEstado(idProducto, activo);
        }
    }
}