using System;
using System.Collections.Generic;

namespace StockOS.Domain.Entities;

/// <summary>
/// Representa un turno de caja abierto por un empleado.
/// Controla el monto inicial, cierre y diferencias de arqueo.
/// </summary>
public partial class CajaSesion
{
    public int IdCajaSesion { get; set; }
    public DateTime FechaApertura { get; set; }

    public DateTime? FechaCierre { get; set; }

    // Monto en efectivo al abrir el turno
    public decimal MontoApertura { get; set; }

    // Monto contado al cerrar (null si aún está abierta)
    public decimal? MontoCierreReal { get; set; }

    // 1=Abierta, 2=Cerrada
    public byte Estado { get; set; }

    public int IdCaja { get; set; }

    public int IdEmpleado { get; set; }

    public virtual Caja IdCajaNavigation { get; set; } = null!;

    public virtual Empleado IdEmpleadoNavigation { get; set; } = null!;

    public virtual ICollection<MovimientoCaja> MovimientoCajas { get; set; } = new List<MovimientoCaja>();

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
