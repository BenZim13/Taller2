using System;

namespace StockOS.Domain.DTOs
{
    public class ResumenFinancieroDTO
    {
        public decimal IngresosBrutos { get; set; } // Ventas sin descuentos
        public decimal DescuentosOtorgados { get; set; }
        public decimal IngresosNetos { get; set; } // Ventas finales cobradas
        public decimal EgresosBrutos { get; set; } // Total gastado en compras a proveedores

        // Propiedad calculada: La diferencia entre lo que entró y lo que salió
        public decimal BalanceNeto => IngresosNetos - EgresosBrutos;
    }
}
