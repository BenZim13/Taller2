using System.Collections.Generic;
using StockOS.Domain.Entities;

namespace StockOS.Domain.Interfaces
{
    public interface IRolRepository
    {
        IEnumerable<Rol> ObtenerTodos();
    }
}

