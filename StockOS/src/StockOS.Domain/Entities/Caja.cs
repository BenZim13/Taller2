using System;
using System.Collections.Generic;

namespace StockOS.Domain.Entities;

public partial class Caja
{
    public int IdCaja { get; set; }

    public string NombreNumero { get; set; } = null!;

    public int IdSucursal { get; set; }

    public bool Activo { get; set; }

    public virtual ICollection<CajaSesion> CajaSesions { get; set; } = new List<CajaSesion>();

    public virtual Sucursal IdSucursalNavigation { get; set; } = null!;
}
