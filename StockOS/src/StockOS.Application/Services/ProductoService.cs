using System.Collections.Generic;
using StockOS.Domain.Entities;
using StockOS.Domain.Interfaces;

namespace StockOS.Application.Services
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _productoRepository;

        public ProductoService(IProductoRepository productoRepository)
        {
            _productoRepository = productoRepository;
        }

        public IEnumerable<Producto> ObtenerTodos()
        {
            return _productoRepository.ObtenerTodos();
        }

        // NUEVO: Método rápido expuesto para las pantallas
        public Producto BuscarPorCodigoBarra(string codigoBarra)
        {
            if (string.IsNullOrWhiteSpace(codigoBarra)) return null;
            return _productoRepository.BuscarPorCodigoBarra(codigoBarra.Trim());
        }

        public void Agregar(Producto producto)
        {
            if (producto == null) throw new System.ArgumentNullException(nameof(producto));
            if (string.IsNullOrWhiteSpace(producto.CodigoBarra)) throw new System.ArgumentException("El código de barra del producto es obligatorio.");
            if (string.IsNullOrWhiteSpace(producto.Nombre)) throw new System.ArgumentException("El nombre del producto es obligatorio.");
            if (producto.PrecioVentaActual <= 0) throw new System.ArgumentException("El precio de venta debe ser un número mayor a cero.");

            //Búsqueda ultra-rápida de duplicados
            var existente = _productoRepository.BuscarPorCodigoBarra(producto.CodigoBarra.Trim());

            if (existente != null)
            {
                throw new System.InvalidOperationException($"Ya existe un producto registrado con el código '{producto.CodigoBarra}'.");
            }

            _productoRepository.Agregar(producto);
        }

        public void Actualizar(Producto producto)
        {
            if (producto == null) throw new System.ArgumentNullException(nameof(producto));
            if (string.IsNullOrWhiteSpace(producto.CodigoBarra)) throw new System.ArgumentException("El código de barra del producto es obligatorio.");
            if (string.IsNullOrWhiteSpace(producto.Nombre)) throw new System.ArgumentException("El nombre del producto es obligatorio.");
            if (producto.PrecioVentaActual <= 0) throw new System.ArgumentException("El precio de venta debe ser un número mayor a cero.");

            //Búsqueda ultra-rápida de duplicados
            var existente = _productoRepository.BuscarPorCodigoBarra(producto.CodigoBarra.Trim());

            // Si encontró uno con ese código, y NO es el mismo producto que estamos editando...
            if (existente != null && existente.IdProducto != producto.IdProducto)
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