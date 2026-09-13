using System;
using System.Collections.Generic;

namespace StockOS.Domain.Entities;

public partial class StockSucursal
{
    public int IdProducto { get; set; }

    public int IdSucursal { get; set; }

    public int StockActual { get; set; }

    public int StockMinimo { get; set; }

    public virtual Producto IdProductoNavigation { get; set; } = null!;

    public virtual Sucursal IdSucursalNavigation { get; set; } = null!;
}
