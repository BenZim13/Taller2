using StockOS.Domain.Entities;
using System.Collections.Generic;

namespace StockOS.Domain.Interfaces
{
    public interface ICompraRepository
    {
        int RegistrarCompra(Compra cabecera, DetalleCompra detalle);
        decimal? ObtenerUltimoPrecioCompra(int idProducto);
        void ActualizarPrecioCompra(int idProducto, decimal nuevoPrecioCompra);
    }
}

