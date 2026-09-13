using System;
using System.Collections.Generic;

namespace StockOS.Domain.Entities;

public partial class Venta
{
    public int IdVenta { get; set; }

    public DateTime FechaHora { get; set; }

    public decimal Subtotal { get; set; }

    public decimal DescuentoTotal { get; set; }

    public decimal TotalVenta { get; set; }

    public byte Estado { get; set; }

    public int IdCajaSesion { get; set; }

    public int? IdCliente { get; set; }

    public virtual ICollection<DetalleVenta> DetalleVenta { get; set; } = new List<DetalleVenta>();

    public virtual Factura? Factura { get; set; }

    public virtual CajaSesion IdCajaSesionNavigation { get; set; } = null!;

    public virtual Cliente? IdClienteNavigation { get; set; }

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
