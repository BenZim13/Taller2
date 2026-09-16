using StockOS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockOS.Application.Services
{
    public interface IProductoService
    {
        IEnumerable<Producto> ObtenerTodos();
        void Agregar(Producto producto);
        void Actualizar(Producto producto);
    }
}
