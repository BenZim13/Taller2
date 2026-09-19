using System;
using System.Collections.Generic;
using StockOS.Domain.Enums;
using StockOS.Domain.Interfaces;

namespace StockOS.Application.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly Dictionary<int, HashSet<string>> _permisosPorRol = new()
        {
            // 1: Administrador -> Tiene acceso absoluto a todo
            { 1, new HashSet<string> {
                Permisos.USUARIOS_VER, Permisos.USUARIOS_CREAR, Permisos.USUARIOS_EDITAR,
                Permisos.PRODUCTOS_VER, Permisos.PRODUCTOS_CREAR, Permisos.PRODUCTOS_EDITAR,
                Permisos.CATEGORIAS_GESTIONAR, Permisos.STOCK_VER, Permisos.STOCK_INGRESAR,
                Permisos.VENTAS_REALIZAR, Permisos.CAJA_ABRIR, Permisos.CAJA_CERRAR, Permisos.CAJA_MOVIMIENTOS,
                Permisos.REPORTES_VER, Permisos.COMPRAS_GESTIONAR, Permisos.PROVEEDORES_GESTIONAR,
                Permisos.CLIENTES_GESTIONAR, Permisos.CONFIGURACION_GESTIONAR
            }},
            
            // 2: Cajero -> Solo atiende al público, lee productos y maneja su dinero
            { 2, new HashSet<string> {
                Permisos.PRODUCTOS_VER, Permisos.STOCK_VER,
                Permisos.VENTAS_REALIZAR, Permisos.CAJA_ABRIR, Permisos.CAJA_CERRAR, Permisos.CAJA_MOVIMIENTOS
            }},
            
            // 3: Encargado de Depósito -> Dueño del inventario y compras, no toca la caja
            { 3, new HashSet<string> {
                Permisos.PRODUCTOS_VER, Permisos.PRODUCTOS_CREAR, Permisos.PRODUCTOS_EDITAR,
                Permisos.CATEGORIAS_GESTIONAR, Permisos.STOCK_VER, Permisos.STOCK_INGRESAR,
                Permisos.COMPRAS_GESTIONAR, Permisos.PROVEEDORES_GESTIONAR
            }},
            
            // 4: Repositor -> Consulta precios y pasillos, no modifica nada
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