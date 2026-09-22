using System;
using System.Collections.Generic;
using StockOS.Domain.Enums;
using StockOS.Domain.Interfaces;

namespace StockOS.Application.Services
{
    /// <summary>
    /// Servicio de autorización basado en roles (RBAC - Role-Based Access Control).
    /// Define qué permisos tiene cada rol en el sistema.
    /// </summary>
    public class AuthorizationService : IAuthorizationService
    {
        // Matriz de permisos: cada rol tiene un conjunto de permisos asignados
        private readonly Dictionary<int, HashSet<string>> _permisosPorRol = new()
        {
            // Rol 1: Administrador - Acceso completo al sistema
            { 1, new HashSet<string> {
                Permisos.USUARIOS_VER, Permisos.USUARIOS_CREAR, Permisos.USUARIOS_EDITAR,
                Permisos.PRODUCTOS_VER, Permisos.PRODUCTOS_CREAR, Permisos.PRODUCTOS_EDITAR,
                Permisos.CATEGORIAS_GESTIONAR, Permisos.STOCK_VER, Permisos.STOCK_INGRESAR,
                Permisos.VENTAS_REALIZAR, Permisos.CAJA_ABRIR, Permisos.CAJA_CERRAR, Permisos.CAJA_MOVIMIENTOS,
                Permisos.REPORTES_VER, Permisos.COMPRAS_GESTIONAR, Permisos.PROVEEDORES_GESTIONAR,
                Permisos.CLIENTES_GESTIONAR, Permisos.CONFIGURACION_GESTIONAR
            }},
            
            // Rol 2: Cajero - Acceso a ventas, consulta de productos y manejo de caja
            { 2, new HashSet<string> {
                Permisos.PRODUCTOS_VER, Permisos.STOCK_VER,
                Permisos.VENTAS_REALIZAR, Permisos.CAJA_ABRIR, Permisos.CAJA_CERRAR, Permisos.CAJA_MOVIMIENTOS
            }},
            
            // Rol 3: Encargado de Depósito - Gestión de inventario, compras y proveedores
            { 3, new HashSet<string> {
                Permisos.PRODUCTOS_VER, Permisos.PRODUCTOS_CREAR, Permisos.PRODUCTOS_EDITAR,
                Permisos.CATEGORIAS_GESTIONAR, Permisos.STOCK_VER, Permisos.STOCK_INGRESAR,
                Permisos.COMPRAS_GESTIONAR, Permisos.PROVEEDORES_GESTIONAR
            }},
            
            // Rol 4: Repositor - Solo consulta de productos y stock (sin modificaciones)
            { 4, new HashSet<string> {
                Permisos.PRODUCTOS_VER, Permisos.STOCK_VER
            }}
        };

        public bool TienePermiso(string permiso)
        {
            var usuario = SesionActual.Usuario;
            if (usuario == null) return false;

            return _permisosPorRol.TryGetValue(usuario.IdRol, out var permisos) && permisos.Contains(permiso);
        }

        public void ValidarPermiso(string permiso)
        {
            if (!TienePermiso(permiso))
            {
                throw new UnauthorizedAccessException($"Operación denegada. Tu rol actual no tiene el permiso: {permiso}");
            }
        }
    }
}