using StockOS.Domain.Entities;
using StockOS.Domain.Interfaces;

namespace StockOS.Application.Services
{
    public class CompraService : ICompraService
    {
        private readonly ICompraRepository _compraRepo;

        public CompraService(ICompraRepository compraRepo)
        {
            _compraRepo = compraRepo;
        }

        public int RegistrarCompra(Compra cabecera, DetalleCompra detalle)
        {
            return _compraRepo.RegistrarCompra(cabecera, detalle);
        }

        public decimal? ObtenerUltimoPrecioCompra(int idProducto)
        {
            return _compraRepo.ObtenerUltimoPrecioCompra(idProducto);
        }

        public void ActualizarPrecioCompra(int idProducto, decimal nuevoPrecioCompra)
        {
            _compraRepo.ActualizarPrecioCompra(idProducto, nuevoPrecioCompra);
        }
    }
}

