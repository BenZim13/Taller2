using System;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StockOS.DataAccess.Persistence;
using StockOS.Domain.Entities;
using StockOS.Domain.Interfaces;

namespace StockOS.DataAccess.Repositories
{
    public class CompraRepository : ICompraRepository
    {
        private readonly StockOsContext _context;

        public CompraRepository(StockOsContext context)
        {
            _context = context;
        }

        public int RegistrarCompra(Compra cabecera, DetalleCompra detalle)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // 1. Insertar Cabecera de Compra
                    var idCompraParam = new SqlParameter
                    {
                        ParameterName = "@IdCompra",
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Output
                    };

                    var numComprobanteParam = new SqlParameter("@NumeroComprobante", SqlDbType.VarChar)
                    {
                        Value = string.IsNullOrWhiteSpace(cabecera.NumeroComprobante) ? (object)DBNull.Value : cabecera.NumeroComprobante
                    };

                    _context.Database.ExecuteSqlRaw(
                        "EXEC sp_Compras_Insertar @FechaHora={0}, @Total={1}, @NumeroComprobante={2}, @IdProveedor={3}, @IdEmpleado={4}, @IdSucursal={5}, @IdCompra=@IdCompra OUTPUT",
                        cabecera.FechaHora,
                        cabecera.Total,
                        numComprobanteParam,
                        cabecera.IdProveedor,
                        cabecera.IdEmpleado,
                        cabecera.IdSucursal,
                        idCompraParam);

                    int idCompraGenerada = (int)idCompraParam.Value;
                    cabecera.IdCompra = idCompraGenerada;

                    // 2. Insertar Detalle de Compra
                    var idDetalleParam = new SqlParameter
                    {
                        ParameterName = "@IdDetalleCompra",
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Output
                    };

                    _context.Database.ExecuteSqlRaw(
                        "EXEC sp_DetalleCompra_Insertar @IdCompra={0}, @IdProducto={1}, @Cantidad={2}, @PrecioUnitarioCompra={3}, @IdDetalleCompra=@IdDetalleCompra OUTPUT",
                        idCompraGenerada,
                        detalle.IdProducto,
                        detalle.Cantidad,
                        detalle.PrecioUnitarioCompra,
                        idDetalleParam);

                    detalle.IdDetalleCompra = (int)idDetalleParam.Value;
                    detalle.IdCompra = idCompraGenerada;

                    transaction.Commit();
                    return idCompraGenerada;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public decimal? ObtenerUltimoPrecioCompra(int idProducto)
        {
            var precioParam = new SqlParameter
            {
                ParameterName = "@PrecioCompra",
                SqlDbType = SqlDbType.Decimal,
                Precision = 12,
                Scale = 2,
                Direction = ParameterDirection.Output
            };

            _context.Database.ExecuteSqlRaw(
                "EXEC sp_Compras_ObtenerUltimoPrecio @IdProducto={0}, @PrecioCompra=@PrecioCompra OUTPUT",
                idProducto, precioParam);

            if (precioParam.Value != DBNull.Value && precioParam.Value != null)
            {
                return (decimal)precioParam.Value;
            }

            return null;
        }

        public void ActualizarPrecioCompra(int idProducto, decimal nuevoPrecioCompra)
        {
            _context.Database.ExecuteSqlRaw(
                "EXEC sp_Compras_ActualizarPrecio @IdProducto={0}, @NuevoPrecioCompra={1}",
                idProducto, nuevoPrecioCompra);
        }
    }
}

