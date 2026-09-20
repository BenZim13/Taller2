using System;

namespace StockOS.Domain.DTOs
{
    public class ReporteItemDTO
    {
        public string CodigoBarra { get; set; } = string.Empty;
        public string NombreProducto { get; set; } = string.Empty;
        public int CantidadTotal { get; set; }
        public decimal Subtotal { get; set; }
    }
}

