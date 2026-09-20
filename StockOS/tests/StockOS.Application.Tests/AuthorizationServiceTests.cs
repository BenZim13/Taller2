using System;
using StockOS.Application.Services;
using StockOS.Domain.Entities;
using StockOS.Domain.Enums;
using Xunit;

namespace StockOS.Application.Tests
{
    public class AuthorizationServiceTests : IDisposable
    {
        private readonly AuthorizationService _authService;

        public AuthorizationServiceTests()
        {
            _authService = new AuthorizationService();
            SesionActual.Limpiar();
        }

        public void Dispose()
        {
            SesionActual.Limpiar();
        }

        // ==========================================
        // PRUEBAS DE ÉXITO (Permisos concedidos)
        // ==========================================

        [Fact]
        public void ValidarPermiso_Administrador_TieneAccesoATodo()
        {
            // ARRANGE: Usuario con Rol 1 (Administrador / Gerente)
            SesionActual.Usuario = new Empleado { IdEmpleado = 1, IdRol = 1, Nombre = "Admin" };

            // ACT & ASSERT: Debe validar con éxito sin lanzar excepción
            _authService.ValidarPermiso(Permisos.USUARIOS_VER);
            _authService.ValidarPermiso(Permisos.PRODUCTOS_CREAR);
            _authService.ValidarPermiso(Permisos.CAJA_ABRIR);
            _authService.ValidarPermiso(Permisos.VENTAS_REALIZAR);
            _authService.ValidarPermiso(Permisos.COMPRAS_GESTIONAR);
            _authService.ValidarPermiso(Permisos.CONFIGURACION_GESTIONAR);

            Assert.True(_authService.TienePermiso(Permisos.USUARIOS_VER));
        }

        [Fact]
        public void ValidarPermiso_Cajero_TienePermisoVentaYCaja()
        {
            // ARRANGE: Usuario con Rol 2 (Cajero)
            SesionActual.Usuario = new Empleado { IdEmpleado = 2, IdRol = (int)RolUsuario.Cajero, Nombre = "Cajero" };

            // ACT & ASSERT: Permisos que sí le corresponden
            _authService.ValidarPermiso(Permisos.VENTAS_REALIZAR);
            _authService.ValidarPermiso(Permisos.CAJA_ABRIR);
            _authService.ValidarPermiso(Permisos.CAJA_CERRAR);
            _authService.ValidarPermiso(Permisos.CAJA_MOVIMIENTOS);
            _authService.ValidarPermiso(Permisos.PRODUCTOS_VER);

            Assert.True(_authService.TienePermiso(Permisos.VENTAS_REALIZAR));
        }

        [Fact]
        public void ValidarPermiso_EncargadoDeposito_TienePermisoStockYCompras()
        {
            // ARRANGE: Usuario con Rol 3 (EncargadoDeposito)
            SesionActual.Usuario = new Empleado { IdEmpleado = 3, IdRol = (int)RolUsuario.EncargadoDeposito, Nombre = "Deposito" };

            // ACT & ASSERT
            _authService.ValidarPermiso(Permisos.STOCK_INGRESAR);
            _authService.ValidarPermiso(Permisos.PRODUCTOS_CREAR);
            _authService.ValidarPermiso(Permisos.CATEGORIAS_GESTIONAR);
            _authService.ValidarPermiso(Permisos.COMPRAS_GESTIONAR);

            Assert.True(_authService.TienePermiso(Permisos.STOCK_INGRESAR));
        }

        // ==========================================
        // PRUEBAS DE FRACASO (Denegación de accesos)
        // ==========================================

        [Fact]
        public void ValidarPermiso_SinSesionIniciada_LanzaUnauthorizedAccessException()
        {
            // ARRANGE: No hay usuario en sesión
            SesionActual.Usuario = null;

            // ACT & ASSERT: Cualquier permiso debe fallar
            Assert.False(_authService.TienePermiso(Permisos.VENTAS_REALIZAR));
            var ex = Assert.Throws<UnauthorizedAccessException>(() => _authService.ValidarPermiso(Permisos.VENTAS_REALIZAR));
            Assert.Contains("Operación denegada", ex.Message);
        }

        [Fact]
        public void ValidarPermiso_CajeroIntentandoCrearProducto_LanzaUnauthorizedAccessException()
        {
            // ARRANGE: Cajero no debe poder crear productos ni ver usuarios
            SesionActual.Usuario = new Empleado { IdEmpleado = 2, IdRol = (int)RolUsuario.Cajero, Nombre = "Cajero" };

            // ACT & ASSERT
            Assert.False(_authService.TienePermiso(Permisos.PRODUCTOS_CREAR));
            Assert.Throws<UnauthorizedAccessException>(() => _authService.ValidarPermiso(Permisos.PRODUCTOS_CREAR));

            Assert.False(_authService.TienePermiso(Permisos.USUARIOS_VER));
            Assert.Throws<UnauthorizedAccessException>(() => _authService.ValidarPermiso(Permisos.USUARIOS_VER));
        }

        [Fact]
        public void ValidarPermiso_EncargadoDepositoIntentandoCobrar_LanzaUnauthorizedAccessException()
        {
            // ARRANGE: Encargado de Depósito no debe tocar ventas ni caja
            SesionActual.Usuario = new Empleado { IdEmpleado = 3, IdRol = (int)RolUsuario.EncargadoDeposito, Nombre = "Deposito" };

            // ACT & ASSERT
            Assert.False(_authService.TienePermiso(Permisos.VENTAS_REALIZAR));
            Assert.Throws<UnauthorizedAccessException>(() => _authService.ValidarPermiso(Permisos.VENTAS_REALIZAR));

            Assert.False(_authService.TienePermiso(Permisos.CAJA_ABRIR));
            Assert.Throws<UnauthorizedAccessException>(() => _authService.ValidarPermiso(Permisos.CAJA_ABRIR));
        }

        [Fact]
        public void ValidarPermiso_RepositorIntentandoIngresarStock_LanzaUnauthorizedAccessException()
        {
            // ARRANGE: Repositor solo consulta, no ingresa stock ni modifica nada
            SesionActual.Usuario = new Empleado { IdEmpleado = 4, IdRol = (int)RolUsuario.Repositor, Nombre = "Repositor" };

            // ACT & ASSERT
            Assert.False(_authService.TienePermiso(Permisos.STOCK_INGRESAR));
            Assert.Throws<UnauthorizedAccessException>(() => _authService.ValidarPermiso(Permisos.STOCK_INGRESAR));

            Assert.False(_authService.TienePermiso(Permisos.PRODUCTOS_CREAR));
            Assert.Throws<UnauthorizedAccessException>(() => _authService.ValidarPermiso(Permisos.PRODUCTOS_CREAR));
        }

        [Fact]
        public void ValidarPermiso_RolInexistente_LanzaUnauthorizedAccessException()
        {
            // ARRANGE: Rol 99 no existe en el mapeo
            SesionActual.Usuario = new Empleado { IdEmpleado = 99, IdRol = 99, Nombre = "Desconocido" };

            // ACT & ASSERT
            Assert.False(_authService.TienePermiso(Permisos.PRODUCTOS_VER));
            Assert.Throws<UnauthorizedAccessException>(() => _authService.ValidarPermiso(Permisos.PRODUCTOS_VER));
        }
    }
}

