using System.Collections.Generic;
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
            _productoRepository.Agregar(producto);
        }

        public void Actualizar(Producto producto)
        {
            _productoRepository.Actualizar(producto);
        }
    }
}