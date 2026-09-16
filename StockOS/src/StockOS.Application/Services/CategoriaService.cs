using StockOS.Domain.Entities;
using StockOS.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockOS.Application.Services
{
    public class CategoriaService : ICategoriaService
    {
        // 1. Declaramos 
        private readonly ICategoriaRepository _categoriaRepository;

        // 2. recibimos en el constructor
        public CategoriaService(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }
        public IEnumerable<Categoria> ObtenerTodos()
        {
            return _categoriaRepository.ObtenerTodos();
        }
    }
}
