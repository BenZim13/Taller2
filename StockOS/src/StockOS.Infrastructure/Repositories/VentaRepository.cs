using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StockOS.DataAccess.Persistence;
using StockOS.Domain.Entities;
using StockOS.Domain.Interfaces;

namespace StockOS.DataAccess.Repositories
{
    public class VentaRepository : IVentaRepository
    {
        private readonly StockOsContext _context;

        public VentaRepository(StockOsContext context)
        {
            _context = context;
        }

        public int RegistrarVenta(Venta cabecera, List<DetalleVenta> detalles, int idSucursal)
        {
            // Iniciamos la Transacción: Todo o Nada
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // 1. Guardar la Cabecera de la Venta
                    var idVentaParam = new SqlParameter
                    {
                        ParameterName = "@IdVenta",
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Output
                    };

                    _context.Database.ExecuteSqlRaw(
                        "EXEC sp_Ventas_Insertar @Subtotal={0}, @DescuentoTotal={1}, @TotalVenta={2}, @IdCajaSesion={3}, @IdCliente={4}, @IdVenta=@IdVenta OUTPUT",
                        cabecera.Subtotal,
                        cabecera.DescuentoTotal,
                        cabecera.TotalVenta,
                        cabecera.IdCajaSesion,
                        cabecera.IdCliente ?? (object)DBNull.Value, // Si no hay cliente, pasamos NULL a SQL
                        idVentaParam);

                    int idVentaGenerado = (int)idVentaParam.Value;

                    // 2. Guardar cada Detalle y Descontar el Stock
                    foreach (var item in detalles)
                    {
                        // Insertar el detalle
                        _context.Database.ExecuteSqlRaw(
                            "EXEC sp_DetalleVenta_Insertar @Cantidad={0}, @PrecioUnitario={1}, @Descuento={2}, @IdVenta={3}, @IdProducto={4}",
                            item.Cantidad, item.PrecioUnitarioHistorico, item.Descuento, idVentaGenerado, item.IdProducto);

                        // Descontar del inventario
                        _context.Database.ExecuteSqlRaw(
                            "EXEC sp_Stock_Descontar @IdProducto={0}, @IdSucursal={1}, @CantidadAVender={2}",
                            item.IdProducto, idSucursal, item.Cantidad);
                    }

                    // 3. Si llegamos hasta acá sin errores, confirmamos todo en la base de datos
                    transaction.Commit();

                    return idVentaGenerado;
                }
                catch (Exception)
                {
                    // Si algo falla (ej. error de SQL, corte de conexión), se reverte TODO
                    transaction.Rollback();
                    throw; // Lanzamos el error hacia arriba para que el formulario lo muestre
                }
            }
        }
    }
}