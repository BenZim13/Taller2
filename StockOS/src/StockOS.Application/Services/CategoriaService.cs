using StockOS.Domain.Entities;
using StockOS.Domain.Interfaces;
using System.Collections.Generic;

namespace StockOS.Application.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public CategoriaService(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public IEnumerable<Categoria> ObtenerTodos()
        {
            return _categoriaRepository.ObtenerTodos();
        }

        public void Agregar(Categoria categoria)
        {
            _categoriaRepository.Agregar(categoria);
        }

        public void Actualizar(Categoria categoria)
        {
            _categoriaRepository.Actualizar(categoria);
        }
    }
}

