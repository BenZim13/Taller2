using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StockOS.Domain.DTOs;

namespace StockOS.Domain.Interfaces
{
    public interface IReporteRepository
    {
        Task<(decimal Total, IEnumerable<ReporteItemDTO> Detalles)> ObtenerReporteVentasAsync(DateTime fechaInicio, DateTime fechaFin);
        Task<(decimal Total, IEnumerable<ReporteItemDTO> Detalles)> ObtenerReporteComprasAsync(DateTime fechaInicio, DateTime fechaFin);
        Task<IEnumerable<ReporteItemDTO>> ObtenerTopProductosVendidosAsync(int topN, DateTime fechaInicio, DateTime fechaFin);
        Task<IEnumerable<VentasPorFechaDTO>> ObtenerVentasPorLineaTiempoAsync(DateTime fechaInicio, DateTime fechaFin);

        Task<ResumenFinancieroDTO> ObtenerResumenFinancieroAsync(DateTime fechaInicio, DateTime fechaFin);
    }
}

