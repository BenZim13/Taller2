using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StockOS.DataAccess.Persistence;
using StockOS.Domain.Interfaces;

namespace StockOS.DataAccess.Repositories
{
    public class StockSucursalRepository : IStockSucursalRepository
    {
        private readonly StockOsContext _context;

        public StockSucursalRepository(StockOsContext context)
        {
            _context = context;
        }

        public int ObtenerCantidadActual(int idProducto, int idSucursal)
        {
            var paramCantidad = new SqlParameter
            {
                ParameterName = "@StockActual",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };

            // Ejecutamos el SP de lectura
            _context.Database.ExecuteSqlRaw(
                "EXEC sp_Stock_ObtenerActual @IdProducto={0}, @IdSucursal={1}, @StockActual=@StockActual OUTPUT",
                idProducto, idSucursal, paramCantidad);

            // Si devuelve DBNull, retornamos 0, sino retornamos el valor
            return paramCantidad.Value != System.DBNull.Value ? (int)paramCantidad.Value : 0;
        }

        public void IngresarMercaderia(int idProducto, int idSucursal, int cantidad)
        {
            // Ejecutamos el SP de ingreso (SQL Server hace el IF/ELSE por nosotros)
            _context.Database.ExecuteSqlRaw(
                "EXEC sp_Stock_IngresarMercaderia @IdProducto={0}, @IdSucursal={1}, @CantidadAIngresar={2}",
                idProducto, idSucursal, cantidad);
        }
    }
}