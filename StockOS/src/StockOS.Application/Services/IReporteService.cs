using StockOS.Domain.DTOs;
using System;
using System.Threading.Tasks;

namespace StockOS.Application.Services
{
    public interface IReporteService
    {
        Task GenerarReporteVentasPdfAsync(DateTime fechaInicio, DateTime fechaFin, string periodoDescripcion);
        Task GenerarReporteComprasPdfAsync(DateTime fechaInicio, DateTime fechaFin, string periodoDescripcion);
        Task<IEnumerable<ReporteItemDTO>> ObtenerTopProductosVendidosAsync(int topN, DateTime fechaInicio, DateTime fechaFin);
        Task<IEnumerable<VentasPorFechaDTO>> ObtenerVentasPorLineaTiempoAsync(DateTime fechaInicio, DateTime fechaFin);
        Task<ResumenFinancieroDTO> ObtenerResumenFinancieroAsync(DateTime fechaInicio, DateTime fechaFin);
    }
}

