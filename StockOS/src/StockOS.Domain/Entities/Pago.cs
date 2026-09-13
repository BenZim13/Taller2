using System;
using System.Collections.Generic;

namespace StockOS.Domain.Entities;

public partial class Pago
{
    public int IdPago { get; set; }

    public decimal Monto { get; set; }

    public string? ReferenciaTransaccion { get; set; }

    public int IdMetodoPago { get; set; }

    public int IdVenta { get; set; }

    public virtual MetodoPago IdMetodoPagoNavigation { get; set; } = null!;

    public virtual Venta IdVentaNavigation { get; set; } = null!;
}
