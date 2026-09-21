using System;
using Moq;
using Xunit;
using StockOS.Application.Services;
using StockOS.Domain.Interfaces;
using StockOS.Domain.Enums;

namespace StockOS.Application.Tests
{
    public class CajaServiceTests
    {
        // ==========================================
        // PRUEBAS DE ÉXITO
        // ==========================================

        [Fact]
        public void AbrirCaja_ConPermiso_EjecutaRepositorio()
        {
            // ARRANGE
            var mockCajaSesionRepo = new Mock<ICajaSesionRepository>();
            var mockCajaRepo = new Mock<ICajaRepository>();
            var mockAuthService = new Mock<IAuthorizationService>();

            mockCajaSesionRepo.Setup(r => r.AbrirCaja(1, 10, 5000m)).Returns(101);

            var cajaService = new CajaService(mockCajaSesionRepo.Object, mockCajaRepo.Object, mockAuthService.Object);

            // ACT
            int idSesion = cajaService.AbrirCaja(1, 10, 5000m);

            // ASSERT
            Assert.Equal(101, idSesion);
            mockAuthService.Verify(a => a.ValidarPermiso(Permisos.CAJA_ABRIR), Times.Once);
            mockCajaSesionRepo.Verify(r => r.AbrirCaja(1, 10, 5000m), Times.Once);
        }

        [Fact]
        public void CerrarCaja_ConMontoValido_EjecutaRepositorioCorrectamente()
        {
            // ARRANGE
            var mockCajaSesionRepo = new Mock<ICajaSesionRepository>();
            var mockCajaRepo = new Mock<ICajaRepository>();
            var mockAuthService = new Mock<IAuthorizationService>();

            var cajaService = new CajaService(mockCajaSesionRepo.Object, mockCajaRepo.Object, mockAuthService.Object);
            decimal montoValido = 15000.50m;
            int idCajaSesion = 1;

            // ACT
            cajaService.CerrarCaja(idCajaSesion, montoValido);

            // ASSERT
            mockAuthService.Verify(a => a.ValidarPermiso(Permisos.CAJA_CERRAR), Times.Once);
            mockCajaRepo.Verify(r => r.CerrarCaja(idCajaSesion, montoValido), Times.Once);
        }

        [Fact]
        public void RegistrarMovimiento_ConDatosValidos_EjecutaRepositorio()
        {
            // ARRANGE
            var mockCajaSesionRepo = new Mock<ICajaSesionRepository>();
            var mockCajaRepo = new Mock<ICajaRepository>();
            var mockAuthService = new Mock<IAuthorizationService>();

            var cajaService = new CajaService(mockCajaSesionRepo.Object, mockCajaRepo.Object, mockAuthService.Object);

            // ACT
            cajaService.RegistrarMovimiento(1, "EGRESO", 1500m, "Pago de flete");

            // ASSERT
            mockAuthService.Verify(a => a.ValidarPermiso(Permisos.CAJA_MOVIMIENTOS), Times.Once);
            mockCajaRepo.Verify(r => r.RegistrarMovimiento(1, "EGRESO", 1500m, "Pago de flete"), Times.Once);
        }

        // ==========================================
        // PRUEBAS DE FRACASO
        // ==========================================

        [Fact]
        public void AbrirCaja_SinPermiso_LanzaUnauthorizedAccessException()
        {
            // ARRANGE
            var mockCajaSesionRepo = new Mock<ICajaSesionRepository>();
            var mockCajaRepo = new Mock<ICajaRepository>();
            var mockAuthService = new Mock<IAuthorizationService>();

            mockAuthService.Setup(a => a.ValidarPermiso(Permisos.CAJA_ABRIR))
                           .Throws(new UnauthorizedAccessException("Denegado"));

            var cajaService = new CajaService(mockCajaSesionRepo.Object, mockCajaRepo.Object, mockAuthService.Object);

            // ACT & ASSERT
            Assert.Throws<UnauthorizedAccessException>(() => cajaService.AbrirCaja(1, 10, 5000m));
            mockCajaSesionRepo.Verify(r => r.AbrirCaja(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<decimal>()), Times.Never);
        }

        [Fact]
        public void CerrarCaja_ConMontoNegativo_LanzaExcepcion()
        {
            // ARRANGE
            var mockCajaSesionRepo = new Mock<ICajaSesionRepository>();
            var mockCajaRepo = new Mock<ICajaRepository>();
            var mockAuthService = new Mock<IAuthorizationService>();

            var cajaService = new CajaService(mockCajaSesionRepo.Object, mockCajaRepo.Object, mockAuthService.Object);

            // ACT & ASSERT
            var excepcion = Assert.Throws<ArgumentException>(() => cajaService.CerrarCaja(1, -500m));
            Assert.Equal("El monto no puede ser negativo.", excepcion.Message);
            mockCajaRepo.Verify(r => r.CerrarCaja(It.IsAny<int>(), It.IsAny<decimal>()), Times.Never);
        }

        [Fact]
        public void CerrarCaja_SinPermiso_LanzaUnauthorizedAccessException()
        {
            // ARRANGE
            var mockCajaSesionRepo = new Mock<ICajaSesionRepository>();
            var mockCajaRepo = new Mock<ICajaRepository>();
            var mockAuthService = new Mock<IAuthorizationService>();

            // Simulamos que el guardián lanza el error de acceso denegado
            mockAuthService.Setup(a => a.ValidarPermiso(Permisos.CAJA_CERRAR))
                           .Throws(new UnauthorizedAccessException("Denegado"));

            var cajaService = new CajaService(mockCajaSesionRepo.Object, mockCajaRepo.Object, mockAuthService.Object);

            // ACT & ASSERT - Verificamos que el servicio propague la explosión
            Assert.Throws<UnauthorizedAccessException>(() => cajaService.CerrarCaja(1, 1000m));

            // Aseguramos que la base de datos NUNCA fue llamada porque el guardián cortó el flujo antes
            mockCajaRepo.Verify(r => r.CerrarCaja(It.IsAny<int>(), It.IsAny<decimal>()), Times.Never);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-50)]
        public void RegistrarMovimiento_ConMontoInvalido_LanzaExcepcion(decimal montoInvalido)
        {
            // ARRANGE
            var mockCajaSesionRepo = new Mock<ICajaSesionRepository>();
            var mockCajaRepo = new Mock<ICajaRepository>();
            var mockAuthService = new Mock<IAuthorizationService>();

            var cajaService = new CajaService(mockCajaSesionRepo.Object, mockCajaRepo.Object, mockAuthService.Object);

            // ACT & ASSERT
            var ex = Assert.Throws<ArgumentException>(() => cajaService.RegistrarMovimiento(1, "EGRESO", montoInvalido, "Gasto"));
            Assert.Contains("mayor a cero", ex.Message);
            mockCajaRepo.Verify(r => r.RegistrarMovimiento(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<string>()), Times.Never);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void RegistrarMovimiento_ConDescripcionVacia_LanzaExcepcion(string? descripcionInvalida)
        {
            // ARRANGE
            var mockCajaSesionRepo = new Mock<ICajaSesionRepository>();
            var mockCajaRepo = new Mock<ICajaRepository>();
            var mockAuthService = new Mock<IAuthorizationService>();

            var cajaService = new CajaService(mockCajaSesionRepo.Object, mockCajaRepo.Object, mockAuthService.Object);

            // ACT & ASSERT
            var ex = Assert.Throws<ArgumentException>(() => cajaService.RegistrarMovimiento(1, "EGRESO", 100m, descripcionInvalida!));
            Assert.Contains("descripción", ex.Message);
            mockCajaRepo.Verify(r => r.RegistrarMovimiento(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void RegistrarMovimiento_SinPermiso_LanzaUnauthorizedAccessException()
        {
            // ARRANGE
            var mockCajaSesionRepo = new Mock<ICajaSesionRepository>();
            var mockCajaRepo = new Mock<ICajaRepository>();
            var mockAuthService = new Mock<IAuthorizationService>();

            mockAuthService.Setup(a => a.ValidarPermiso(Permisos.CAJA_MOVIMIENTOS))
                           .Throws(new UnauthorizedAccessException("Denegado"));

            var cajaService = new CajaService(mockCajaSesionRepo.Object, mockCajaRepo.Object, mockAuthService.Object);

            // ACT & ASSERT
            Assert.Throws<UnauthorizedAccessException>(() => cajaService.RegistrarMovimiento(1, "EGRESO", 100m, "Gasto"));
            mockCajaRepo.Verify(r => r.RegistrarMovimiento(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void AbrirCaja_CuandoEmpleadoYaTieneCajaAbierta_LanzaInvalidOperationException()
        {
            // ARRANGE
            var mockCajaSesionRepo = new Mock<ICajaSesionRepository>();
            var mockCajaRepo = new Mock<ICajaRepository>();
            var mockAuthService = new Mock<IAuthorizationService>();

            // Simular que el empleado 5 ya tiene una sesión activa
            mockCajaSesionRepo.Setup(r => r.VerificarCajaAbierta(5)).Returns(true);

            var cajaService = new CajaService(mockCajaSesionRepo.Object, mockCajaRepo.Object, mockAuthService.Object);

            // ACT & ASSERT
            var ex = Assert.Throws<InvalidOperationException>(() => cajaService.AbrirCaja(1, 5, 1000m));
            Assert.Contains("Ya tienes un turno de caja abierto", ex.Message);
            mockCajaSesionRepo.Verify(r => r.AbrirCaja(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<decimal>()), Times.Never);
        }

        [Fact]
        public void ObtenerIdSesionAbierta_RetornaIdDeRepositorio()
        {
            // ARRANGE
            var mockCajaSesionRepo = new Mock<ICajaSesionRepository>();
            var mockCajaRepo = new Mock<ICajaRepository>();
            var mockAuthService = new Mock<IAuthorizationService>();

            mockCajaSesionRepo.Setup(r => r.ObtenerIdSesionAbierta(12)).Returns(45);

            var cajaService = new CajaService(mockCajaSesionRepo.Object, mockCajaRepo.Object, mockAuthService.Object);

            // ACT
            int? idSesion = cajaService.ObtenerIdSesionAbierta(12);

            // ASSERT
            Assert.NotNull(idSesion);
            Assert.Equal(45, idSesion.Value);
            mockCajaSesionRepo.Verify(r => r.ObtenerIdSesionAbierta(12), Times.Once);
        }

        [Fact]
        public void AbrirCaja_ConMontoNegativo_LanzaArgumentException()
        {
            // ARRANGE
            var mockCajaSesionRepo = new Mock<ICajaSesionRepository>();
            var mockCajaRepo = new Mock<ICajaRepository>();
            var mockAuthService = new Mock<IAuthorizationService>();

            var cajaService = new CajaService(mockCajaSesionRepo.Object, mockCajaRepo.Object, mockAuthService.Object);

            // ACT & ASSERT
            var ex = Assert.Throws<ArgumentException>(() => cajaService.AbrirCaja(1, 10, -500m));
            Assert.Contains("El monto inicial de apertura no puede ser negativo", ex.Message);
            mockCajaSesionRepo.Verify(r => r.AbrirCaja(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<decimal>()), Times.Never);
        }

        [Theory]
        [InlineData("TRANSFERENCIA")]
        [InlineData("DEPOSITO")]
        [InlineData("OTRO")]
        public void RegistrarMovimiento_ConTipoInvalido_LanzaArgumentException(string tipoInvalido)
        {
            // ARRANGE
            var mockCajaSesionRepo = new Mock<ICajaSesionRepository>();
            var mockCajaRepo = new Mock<ICajaRepository>();
            var mockAuthService = new Mock<IAuthorizationService>();

            var cajaService = new CajaService(mockCajaSesionRepo.Object, mockCajaRepo.Object, mockAuthService.Object);

            // ACT & ASSERT
            var ex = Assert.Throws<ArgumentException>(() => cajaService.RegistrarMovimiento(1, tipoInvalido, 100m, "Prueba"));
            Assert.Contains("El tipo de movimiento debe ser", ex.Message);
            mockCajaRepo.Verify(r => r.RegistrarMovimiento(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void CerrarCajaPorCierreSesion_ConSesionAbierta_ObtieneMontoEsperadoYCierraRepositorio()
        {
            // ARRANGE
            var mockCajaSesionRepo = new Mock<ICajaSesionRepository>();
            var mockCajaRepo = new Mock<ICajaRepository>();
            var mockAuthService = new Mock<IAuthorizationService>();

            int idEmpleado = 10;
            int idSesionAbierta = 77;
            decimal montoEsperado = 12500.50m;

            mockCajaSesionRepo.Setup(r => r.ObtenerIdSesionAbierta(idEmpleado)).Returns(idSesionAbierta);
            mockCajaRepo.Setup(r => r.ObtenerMontoEsperado(idSesionAbierta)).Returns(montoEsperado);

            SesionActual.Limpiar();
            SesionActual.Usuario = new Domain.Entities.Empleado { IdEmpleado = idEmpleado, Nombre = "Juan", Apellido = "Perez" };
            SesionActual.IdCajaSesionAbierta = idSesionAbierta;

            var cajaService = new CajaService(mockCajaSesionRepo.Object, mockCajaRepo.Object, mockAuthService.Object);

            // ACT
            cajaService.CerrarCajaPorCierreSesion(idEmpleado);

            // ASSERT
            mockCajaRepo.Verify(r => r.ObtenerMontoEsperado(idSesionAbierta), Times.Once);
            mockCajaRepo.Verify(r => r.CerrarCaja(idSesionAbierta, montoEsperado), Times.Once);
            Assert.Null(SesionActual.IdCajaSesionAbierta);
            SesionActual.Limpiar();
        }

        [Fact]
        public void CerrarCajaPorCierreSesion_SinSesionAbierta_NoEjecutaCierre()
        {
            // ARRANGE
            var mockCajaSesionRepo = new Mock<ICajaSesionRepository>();
            var mockCajaRepo = new Mock<ICajaRepository>();
            var mockAuthService = new Mock<IAuthorizationService>();

            int idEmpleado = 15;
            mockCajaSesionRepo.Setup(r => r.ObtenerIdSesionAbierta(idEmpleado)).Returns((int?)null);

            var cajaService = new CajaService(mockCajaSesionRepo.Object, mockCajaRepo.Object, mockAuthService.Object);

            // ACT
            cajaService.CerrarCajaPorCierreSesion(idEmpleado);

            // ASSERT
            mockCajaRepo.Verify(r => r.ObtenerMontoEsperado(It.IsAny<int>()), Times.Never);
            mockCajaRepo.Verify(r => r.CerrarCaja(It.IsAny<int>(), It.IsAny<decimal>()), Times.Never);
        }

        [Fact]
        public void AbrirCaja_CajaFisicaYaAbiertaPorOtroCajero_LanzaInvalidOperationException()
        {
            // ARRANGE
            var mockCajaSesionRepo = new Mock<ICajaSesionRepository>();
            var mockCajaRepo = new Mock<ICajaRepository>();
            var mockAuthService = new Mock<IAuthorizationService>();

            int idCaja = 2;
            int idEmpleado = 8;

            // El empleado no tiene caja abierta, pero la caja física 2 ya está abierta por otro empleado
            mockCajaSesionRepo.Setup(r => r.VerificarCajaAbierta(idEmpleado)).Returns(false);
            mockCajaSesionRepo.Setup(r => r.VerificarCajaFisicaAbierta(idCaja)).Returns(true);

            var cajaService = new CajaService(mockCajaSesionRepo.Object, mockCajaRepo.Object, mockAuthService.Object);

            // ACT & ASSERT
            var ex = Assert.Throws<InvalidOperationException>(() => cajaService.AbrirCaja(idCaja, idEmpleado, 1000m));
            Assert.Contains("La caja seleccionada ya tiene un turno abierto", ex.Message);
            mockCajaSesionRepo.Verify(r => r.AbrirCaja(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<decimal>()), Times.Never);
        }
    }
}