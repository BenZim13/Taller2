using StockOS.Domain.Entities;
using System.Collections.Generic;

namespace StockOS.Domain.Interfaces
{
    public interface ICategoriaRepository
    {
        IEnumerable<Categoria> ObtenerTodos();
        void Agregar(Categoria categoria);
        void Actualizar(Categoria categoria);
    }
}

