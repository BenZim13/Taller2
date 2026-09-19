using StockOS.Domain.Entities;

namespace StockOS.Application.Services
{
    public interface ICompraService
    {
        int RegistrarCompra(Compra cabecera, DetalleCompra detalle);
        decimal? ObtenerUltimoPrecioCompra(int idProducto);
        void ActualizarPrecioCompra(int idProducto, decimal nuevoPrecioCompra);
    }
}

