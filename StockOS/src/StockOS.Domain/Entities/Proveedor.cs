using System;
using System.Collections.Generic;

namespace StockOS.Domain.Entities;

public partial class Proveedor
{
    public int IdProveedor { get; set; }

    public string RazonSocial { get; set; } = null!;

    public string? Cuit { get; set; }

    public string Telefono { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public bool Activo { get; set; }

    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();
}
