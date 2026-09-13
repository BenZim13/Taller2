using System;
using System.Collections.Generic;

namespace StockOS.Domain.Entities;

public partial class DetalleCompra
{
    public int IdDetalleCompra { get; set; }

    public int Cantidad { get; set; }

    public decimal PrecioUnitarioCompra { get; set; }

    public int IdCompra { get; set; }

    public int IdProducto { get; set; }

    public virtual Compra IdCompraNavigation { get; set; } = null!;

    public virtual Producto IdProductoNavigation { get; set; } = null!;
}
