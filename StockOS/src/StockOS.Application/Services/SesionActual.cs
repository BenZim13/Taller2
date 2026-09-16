using StockOS.Domain.Entities;

namespace StockOS.Application.Services
{
    public static class SesionActual
    {
        // Guardamos al usuario completo que inició sesión
        public static Empleado? Usuario { get; set; }

        // para obtener la sucursal de forma segura
        public static int IdSucursal
        {
            get
            {
                return Usuario != null ? Usuario.IdSucursal : 1; // 1 como respaldo por si algo falla
            }
        }
    }
}