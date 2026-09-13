using System;
using System.Collections.Generic;

namespace StockOS.Domain.Entities;

public partial class Empleado
{
    public int IdEmpleado { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string Dni { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public bool Estado { get; set; }

    public int IdRol { get; set; }

    public int IdSucursal { get; set; }

    public virtual ICollection<CajaSesion> CajaSesions { get; set; } = new List<CajaSesion>();

    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();

    public virtual Rol IdRolNavigation { get; set; } = null!;

    public virtual Sucursal IdSucursalNavigation { get; set; } = null!;
}
