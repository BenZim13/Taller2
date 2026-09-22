using StockOS.Domain.Entities;

namespace StockOS.Application.Services
{
    /// <summary>
    /// Contexto global de la sesión actual del usuario.
    /// Almacena información del empleado autenticado y su turno de caja activo.
    /// </summary>
    public static class SesionActual
    {
        // Empleado autenticado actualmente en el sistema
        public static Empleado? Usuario { get; set; }

        // ID del turno de caja abierto por el empleado (null si no tiene turno activo)
        public static int? IdCajaSesionAbierta { get; set; }

        // Sucursal del empleado actual (retorna 1 por defecto si no hay usuario)
        public static int IdSucursal
        {
            get
            {
                return Usuario?.IdSucursal ?? 1;
            }
        }

        /// <summary>
        /// Limpia la sesión actual al cerrar sesión o salir del sistema.
        /// </summary>
        public static void Limpiar()
        {
            Usuario = null;
            IdCajaSesionAbierta = null;
        }
    }
}