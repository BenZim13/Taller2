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
        public async Task<IEnumerable<ReporteItemDTO>> ObtenerTopProductosVendidosAsync(int topN, DateTime fechaInicio, DateTime fechaFin)
        {
            return await _context.DetalleVentas
                .Where(dv => dv.IdVentaNavigation.Estado == 1 &&
                             dv.IdVentaNavigation.FechaHora >= fechaInicio &&
                             dv.IdVentaNavigation.FechaHora <= fechaFin)
                .GroupBy(dv => new { dv.IdProductoNavigation.CodigoBarra, dv.IdProductoNavigation.Nombre })
                .Select(g => new ReporteItemDTO
                {
                    CodigoBarra = g.Key.CodigoBarra,
                    NombreProducto = g.Key.Nombre,
                    CantidadTotal = g.Sum(dv => dv.Cantidad),
                    Subtotal = g.Sum(dv => (dv.Cantidad * dv.PrecioUnitarioHistorico) - dv.Descuento)
                })
                .OrderByDescending(x => x.CantidadTotal)
                .Take(topN)
                .ToListAsync();
        }

        public async Task<IEnumerable<VentasPorFechaDTO>> ObtenerVentasPorLineaTiempoAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            return await _context.Ventas
                .Where(v => v.Estado == 1 && v.FechaHora >= fechaInicio && v.FechaHora <= fechaFin)
                .GroupBy(v => v.FechaHora.Date) // Agrupa por el día exacto
                .Select(g => new VentasPorFechaDTO
                {
                    Fecha = g.Key,
                    TotalVendido = g.Sum(v => v.TotalVenta)
                })
                .OrderBy(x => x.Fecha)
                .ToListAsync();
        }
        public async Task<ResumenFinancieroDTO> ObtenerResumenFinancieroAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            var resumen = new ResumenFinancieroDTO();

            // Hacemos un SELECT múltiple para traer todos los totales de un solo viaje a la BD
            var query = @"
        SELECT 
            ISNULL((SELECT SUM(subtotal) FROM venta WHERE estado = 1 AND fecha_hora >= @Inicio AND fecha_hora <= @Fin), 0) AS IngresosBrutos,
            ISNULL((SELECT SUM(descuento_total) FROM venta WHERE estado = 1 AND fecha_hora >= @Inicio AND fecha_hora <= @Fin), 0) AS Descuentos,
            ISNULL((SELECT SUM(total_venta) FROM venta WHERE estado = 1 AND fecha_hora >= @Inicio AND fecha_hora <= @Fin), 0) AS IngresosNetos,
            ISNULL((SELECT SUM(total) FROM compra WHERE fecha_hora >= @Inicio AND fecha_hora <= @Fin), 0) AS EgresosBrutos";

            using (var connection = CreateConnection())
            {
                await connection.OpenAsync();
                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Inicio", fechaInicio);
                    cmd.Parameters.AddWithValue("@Fin", fechaFin);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            resumen.IngresosBrutos = Convert.ToDecimal(reader["IngresosBrutos"]);
                            resumen.DescuentosOtorgados = Convert.ToDecimal(reader["Descuentos"]);
                            resumen.IngresosNetos = Convert.ToDecimal(reader["IngresosNetos"]);
                            resumen.EgresosBrutos = Convert.ToDecimal(reader["EgresosBrutos"]);
                        }
                    }
                }
            }
            return resumen;
        }
    }
}

