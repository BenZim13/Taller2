using System;
using System.Collections.Generic;

namespace StockOS.Domain.Entities;

/// <summary>
/// Representa la cabecera de una venta realizada en el sistema.
/// Incluye totales, descuentos y referencia al turno de caja y cliente (opcional).
/// </summary>
public partial class Venta
{
    public int IdVenta { get; set; }
    public DateTime FechaHora { get; set; }
    public decimal Subtotal { get; set; }
    public decimal DescuentoTotal { get; set; }
    public decimal TotalVenta { get; set; }

    // 1=Activa, 0=Cancelada
    public byte Estado { get; set; }

    // Turno de caja en el que se registró esta venta
    public int IdCajaSesion { get; set; }

    // Cliente asociado (opcional, null = consumidor final)
    public int? IdCliente { get; set; }

    // Propiedades de navegación
    public virtual ICollection<DetalleVenta> DetalleVenta { get; set; } = new List<DetalleVenta>();

    public virtual Factura? Factura { get; set; }

    public virtual CajaSesion IdCajaSesionNavigation { get; set; } = null!;

    public virtual Cliente? IdClienteNavigation { get; set; }

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
