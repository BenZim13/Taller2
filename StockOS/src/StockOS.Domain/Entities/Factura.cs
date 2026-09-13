using System;
using System.Collections.Generic;

namespace StockOS.Domain.Entities;

public partial class Factura
{
    public int IdFactura { get; set; }

    public string TipoComprobante { get; set; } = null!;

    public string NumeroFactura { get; set; } = null!;

    public string? CaeAutorizacion { get; set; }

    public DateOnly FechaEmision { get; set; }

    public decimal MontoNeto { get; set; }

    public decimal IvaTotal { get; set; }

    public decimal Total { get; set; }

    public int IdVenta { get; set; }

    public virtual Venta IdVentaNavigation { get; set; } = null!;
}
