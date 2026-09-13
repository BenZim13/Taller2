using System;
using System.Collections.Generic;

namespace StockOS.Domain.Entities;

public partial class Compra
{
    public int IdCompra { get; set; }

    public DateTime FechaHora { get; set; }

    public decimal Total { get; set; }

    public string? NumeroComprobante { get; set; }

    public int IdProveedor { get; set; }

    public int IdEmpleado { get; set; }

    public int IdSucursal { get; set; }

    public virtual ICollection<DetalleCompra> DetalleCompras { get; set; } = new List<DetalleCompra>();

    public virtual Empleado IdEmpleadoNavigation { get; set; } = null!;

    public virtual Proveedor IdProveedorNavigation { get; set; } = null!;

    public virtual Sucursal IdSucursalNavigation { get; set; } = null!;
}
