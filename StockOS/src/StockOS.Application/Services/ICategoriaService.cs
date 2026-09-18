using StockOS.Domain.Entities;
using System.Collections.Generic;

namespace StockOS.Application.Services
{
    public interface ICategoriaService
    {
        IEnumerable<Categoria> ObtenerTodos();
        void Agregar(Categoria categoria);
        void Actualizar(Categoria categoria);
    }
}

