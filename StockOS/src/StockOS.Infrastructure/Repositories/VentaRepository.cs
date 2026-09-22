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
    /// <summary>
    /// Repositorio para operaciones de venta con manejo transaccional.
    /// Utiliza stored procedures para garantizar la integridad de datos y el descuento automático de stock.
    /// </summary>
    public class VentaRepository : IVentaRepository
    {
        private readonly StockOsContext _context;

        public VentaRepository(StockOsContext context)
        {
            _context = context;
        }

        public int RegistrarVenta(Venta cabecera, List<DetalleVenta> detalles, int idSucursal, int idMetodoPago)
        {
            // Transacción para asegurar atomicidad: si algo falla, todo se revierte
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // Parámetro de salida para obtener el ID generado
                    var idVentaParam = new SqlParameter
                    {
                        ParameterName = "@IdVenta",
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Output
                    };

                    var idClienteParam = new SqlParameter("@IdCliente", SqlDbType.Int)
                    {
                        Value = cabecera.IdCliente ?? (object)DBNull.Value
                    };

                    _context.Database.ExecuteSqlRaw(
                        "EXEC sp_Ventas_Insertar @Subtotal={0}, @DescuentoTotal={1}, @TotalVenta={2}, @IdCajaSesion={3}, @IdCliente={4}, @IdVenta=@IdVenta OUTPUT",
                        cabecera.Subtotal, cabecera.DescuentoTotal, cabecera.TotalVenta, cabecera.IdCajaSesion, idClienteParam, idVentaParam);

                    int idVentaGenerado = (int)idVentaParam.Value;

                    // Insertar cada producto vendido y descontar del stock
                    foreach (var item in detalles)
                    {
                        _context.Database.ExecuteSqlRaw(
                            "EXEC sp_DetalleVenta_Insertar @Cantidad={0}, @PrecioUnitario={1}, @Descuento={2}, @IdVenta={3}, @IdProducto={4}",
                            item.Cantidad, item.PrecioUnitarioHistorico, item.Descuento, idVentaGenerado, item.IdProducto);

                        _context.Database.ExecuteSqlRaw(
                            "EXEC sp_Stock_Descontar @IdProducto={0}, @IdSucursal={1}, @CantidadAVender={2}",
                            item.IdProducto, idSucursal, item.Cantidad);
                    }


                    // Registrar el método de pago utilizado
                    var idPagoParam = new SqlParameter
                    {
                        ParameterName = "@IdPago",
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Output
                    };

                    var referenciaParam = new SqlParameter("@ReferenciaTransaccion", SqlDbType.VarChar, 100)
                    {
                        Value = DBNull.Value
                    };

                    _context.Database.ExecuteSqlRaw(
                        "EXEC sp_Pago_Insertar @Monto={0}, @IdMetodoPago={1}, @IdVenta={2}, @ReferenciaTransaccion={3}, @IdPago=@IdPago OUTPUT",
                        cabecera.TotalVenta, idMetodoPago, idVentaGenerado, referenciaParam, idPagoParam);

                    transaction.Commit();

                    return idVentaGenerado;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}