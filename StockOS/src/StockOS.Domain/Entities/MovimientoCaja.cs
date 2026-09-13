using System;
using System.Collections.Generic;

namespace StockOS.Domain.Entities;

public partial class MovimientoCaja
{
    public int IdMovimiento { get; set; }

    public DateTime FechaHora { get; set; }

    public string TipoMovimiento { get; set; } = null!;

    public decimal Monto { get; set; }

    public string Descripcion { get; set; } = null!;

    public int IdCajaSesion { get; set; }

    public virtual CajaSesion IdCajaSesionNavigation { get; set; } = null!;
}
