using System;
using System.Collections.Generic;

namespace StockOS.Domain.Entities;

public partial class Cliente
{
    public int IdCliente { get; set; }

    public string RazonSocial { get; set; } = null!;

    public string? CuitDni { get; set; }

    public string? CondicionIva { get; set; }

    public string? Telefono { get; set; }

    public string? Email { get; set; }

    public string? Direccion { get; set; }

    public bool Activo { get; set; }

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
