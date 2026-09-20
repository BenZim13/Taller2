using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StockOS.DataAccess.Persistence;
using StockOS.Domain.DTOs;
using StockOS.Domain.Interfaces;

namespace StockOS.DataAccess.Repositories
{
    public class ReporteRepository : IReporteRepository
    {
        private readonly StockOsContext _context;

        public ReporteRepository(StockOsContext context)
        {
            _context = context;
        }

        private SqlConnection CreateConnection()
        {
            var connStr = _context.Database.GetConnectionString()
                ?? _context.Database.GetDbConnection().ConnectionString;
            return new SqlConnection(connStr);
        }

        public async Task<(decimal Total, IEnumerable<ReporteItemDTO> Detalles)> ObtenerReporteVentasAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            decimal total = 0;
            var detalles = new List<ReporteItemDTO>();

            using (var connection = CreateConnection())
            {
                await connection.OpenAsync();

                using (var cmdTotal = new SqlCommand("sp_Reportes_Ventas_Total", connection))
                {
                    cmdTotal.CommandType = CommandType.StoredProcedure;
                    cmdTotal.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                    cmdTotal.Parameters.AddWithValue("@FechaFin", fechaFin);

                    var outParam = new SqlParameter("@Total", SqlDbType.Decimal) { Precision = 12, Scale = 2, Direction = ParameterDirection.Output };
                    cmdTotal.Parameters.Add(outParam);

                    await cmdTotal.ExecuteNonQueryAsync();

                    if (outParam.Value != DBNull.Value)
                    {
                        total = (decimal)outParam.Value;
                    }
                }

                using (var cmdDetalle = new SqlCommand("sp_Reportes_Ventas_Detalle", connection))
                {
                    cmdDetalle.CommandType = CommandType.StoredProcedure;
                    cmdDetalle.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                    cmdDetalle.Parameters.AddWithValue("@FechaFin", fechaFin);

                    using (var reader = await cmdDetalle.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            detalles.Add(new ReporteItemDTO
                            {
                                CodigoBarra = reader["CodigoBarra"]?.ToString() ?? "",
                                NombreProducto = reader["NombreProducto"]?.ToString() ?? "",
                                CantidadTotal = Convert.ToInt32(reader["CantidadTotal"]),
                                Subtotal = Convert.ToDecimal(reader["Subtotal"])
                            });
                        }
                    }
                }
            }

            return (total, detalles);
        }

        public async Task<(decimal Total, IEnumerable<ReporteItemDTO> Detalles)> ObtenerReporteComprasAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            decimal total = 0;
            var detalles = new List<ReporteItemDTO>();

            using (var connection = CreateConnection())
            {
                await connection.OpenAsync();

                using (var cmdTotal = new SqlCommand("sp_Reportes_Compras_Total", connection))
                {
                    cmdTotal.CommandType = CommandType.StoredProcedure;
                    cmdTotal.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                    cmdTotal.Parameters.AddWithValue("@FechaFin", fechaFin);

                    var outParam = new SqlParameter("@Total", SqlDbType.Decimal) { Precision = 12, Scale = 2, Direction = ParameterDirection.Output };
                    cmdTotal.Parameters.Add(outParam);

                    await cmdTotal.ExecuteNonQueryAsync();

                    if (outParam.Value != DBNull.Value)
                    {
                        total = (decimal)outParam.Value;
                    }
                }

                using (var cmdDetalle = new SqlCommand("sp_Reportes_Compras_Detalle", connection))
                {
                    cmdDetalle.CommandType = CommandType.StoredProcedure;
                    cmdDetalle.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                    cmdDetalle.Parameters.AddWithValue("@FechaFin", fechaFin);

                    using (var reader = await cmdDetalle.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            detalles.Add(new ReporteItemDTO
                            {
                                CodigoBarra = reader["CodigoBarra"]?.ToString() ?? "",
                                NombreProducto = reader["NombreProducto"]?.ToString() ?? "",
                                CantidadTotal = Convert.ToInt32(reader["CantidadTotal"]),
                                Subtotal = Convert.ToDecimal(reader["Subtotal"])
                            });
                        }
                    }
                }
            }

            return (total, detalles);
        }
    }
}

