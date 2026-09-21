using System;
using System.Collections.Generic;

namespace StockOS.Domain.Entities;

public partial class Producto
{
    public int IdProducto { get; set; }

    public string CodigoBarra { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public const decimal IvaFijoDefault = 21.00m;

    public decimal PrecioVentaActual { get; set; }

    public decimal PorcentajeIva { get; set; } = IvaFijoDefault;

    public int IdCategoria { get; set; }

    public int? IdProveedor { get; set; }

    public bool Activo { get; set; }

    /// <summary>
    /// Calcula el precio de venta final agregando al costo de compra el margen de ganancia y el IVA fijo.
    /// Ejemplo: Compra 1000 + 9% ganancia + 21% IVA = 1000 * (1 + 0.30) = 1300.
    /// </summary>
    public static decimal CalcularPrecioVentaFinal(decimal precioCompra, decimal margenGanancia, decimal porcentajeIva = IvaFijoDefault)
    {
        if (precioCompra < 0) throw new ArgumentException("El precio de compra no puede ser negativo.", nameof(precioCompra));
        if (margenGanancia < 0) throw new ArgumentException("El margen de ganancia no puede ser negativo.", nameof(margenGanancia));

        decimal porcentajeTotal = margenGanancia + porcentajeIva;
        return Math.Round(precioCompra * (1 + (porcentajeTotal / 100m)), 2);
    }

    public virtual ICollection<DetalleCompra> DetalleCompras { get; set; } = new List<DetalleCompra>();

    public virtual ICollection<DetalleVenta> DetalleVenta { get; set; } = new List<DetalleVenta>();

    public virtual Categoria IdCategoriaNavigation { get; set; } = null!;

    public virtual Proveedor? IdProveedorNavigation { get; set; }

    public virtual ICollection<StockSucursal> StockSucursals { get; set; } = new List<StockSucursal>();
}
