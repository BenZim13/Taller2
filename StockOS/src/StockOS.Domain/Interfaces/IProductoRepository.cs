using StockOS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockOS.Domain.Interfaces
{
    public interface IProductoRepository
    {
        IEnumerable<Producto> ObtenerTodos();
        void Agregar(Producto producto);
        void Actualizar(Producto producto);
        void CambiarEstado(int idProducto, bool activo);
    }
}
