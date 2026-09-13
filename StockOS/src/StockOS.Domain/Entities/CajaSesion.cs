using System;
using System.Collections.Generic;

namespace StockOS.Domain.Entities;

public partial class CajaSesion
{
    public int IdCajaSesion { get; set; }

    public DateTime FechaApertura { get; set; }

    public DateTime? FechaCierre { get; set; }

    public decimal MontoApertura { get; set; }

    public decimal? MontoCierreReal { get; set; }

    public byte Estado { get; set; }

    public int IdCaja { get; set; }

    public int IdEmpleado { get; set; }

    public virtual Caja IdCajaNavigation { get; set; } = null!;

    public virtual Empleado IdEmpleadoNavigation { get; set; } = null!;

    public virtual ICollection<MovimientoCaja> MovimientoCajas { get; set; } = new List<MovimientoCaja>();

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
