using System;
using System.Threading.Tasks;

namespace StockOS.Application.Services
{
    public interface IReporteService
    {
        Task GenerarReporteVentasPdfAsync(DateTime fechaInicio, DateTime fechaFin, string periodoDescripcion);
        Task GenerarReporteComprasPdfAsync(DateTime fechaInicio, DateTime fechaFin, string periodoDescripcion);
    }
}

