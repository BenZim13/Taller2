using System;
using System.Collections.Generic;

namespace StockOS.Domain.Entities;

public partial class Producto
{
    public int IdProducto { get; set; }

    public string CodigoBarra { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public decimal PrecioVentaActual { get; set; }

    public decimal PorcentajeIva { get; set; }

    public int IdCategoria { get; set; }

    public int? IdProveedor { get; set; }

    public bool Activo { get; set; }

    public virtual ICollection<DetalleCompra> DetalleCompras { get; set; } = new List<DetalleCompra>();

    public virtual ICollection<DetalleVenta> DetalleVenta { get; set; } = new List<DetalleVenta>();

    public virtual Categoria IdCategoriaNavigation { get; set; } = null!;

    public virtual Proveedor? IdProveedorNavigation { get; set; }

    public virtual ICollection<StockSucursal> StockSucursals { get; set; } = new List<StockSucursal>();
}
