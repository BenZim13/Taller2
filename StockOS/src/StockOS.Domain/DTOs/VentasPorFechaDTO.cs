using System;

namespace StockOS.Domain.DTOs
{
    public class VentasPorFechaDTO
    {
        public DateTime Fecha { get; set; }
        public decimal TotalVendido { get; set; }
    }
}
