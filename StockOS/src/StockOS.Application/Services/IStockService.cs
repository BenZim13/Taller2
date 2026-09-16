using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockOS.Application.Services
{
    public interface IStockService
    {
        // Método principal para ingresar mercadería
        void AgregarStock(int idProducto, int idSucursal, int cantidadAAgregar);

        // Método para saber cuánto hay actualmente
        int ObtenerCantidadActual(int idProducto, int idSucursal);
    }
}
