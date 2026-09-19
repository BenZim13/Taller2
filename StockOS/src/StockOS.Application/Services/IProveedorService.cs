using System.Collections.Generic;
using StockOS.Domain.Entities;

namespace StockOS.Application.Services
{
    public interface IProveedorService
    {
        IEnumerable<Proveedor> ObtenerTodos();
        void Agregar(Proveedor proveedor);
        void Actualizar(Proveedor proveedor);
        void CambiarEstado(int idProveedor, bool activo);
    }
}

