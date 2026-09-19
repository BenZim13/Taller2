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
        // Prueba 1: Regla de negocio (No se puede cerrar caja con saldo negativo)
        [Fact]
        public void CerrarCaja_ConMontoNegativo_LanzaExcepcion()
        {
            // 1. ARRANGE (Preparar)
            // Simulamos la base de datos y el guardián de seguridad con Moq
            var mockCajaSesionRepo = new Mock<ICajaSesionRepository>();
            var mockCajaRepo = new Mock<ICajaRepository>();
            var mockAuthService = new Mock<IAuthorizationService>();

            // Le decimos al guardián falso que deje pasar cualquier permiso (para no trabar el test)
            mockAuthService.Setup(a => a.ValidarPermiso(It.IsAny<string>()));

            // Instanciamos el servicio real, inyectándole los simulacros
            var cajaService = new CajaService(mockCajaSesionRepo.Object, mockCajaRepo.Object, mockAuthService.Object);

            // 2. ACT (Actuar) & 3. ASSERT (Afirmar)
            // Afirmamos que si intentamos cerrar la caja 1 con saldo -500, el sistema DEBE lanzar un error
            var excepcion = Assert.Throws<Exception>(() => cajaService.CerrarCaja(1, -500m));

            // Comprobamos que el mensaje de error sea exactamente el que programaste
            Assert.Equal("El monto no puede ser negativo.", excepcion.Message);
        }

        // Prueba 2: Camino feliz (Si todo está bien, debe llamar a la base de datos)
        [Fact]
        public void CerrarCaja_ConMontoValido_EjecutaRepositorioCorrectamente()
        {
            // 1. ARRANGE
            var mockCajaSesionRepo = new Mock<ICajaSesionRepository>();
            var mockCajaRepo = new Mock<ICajaRepository>();
            var mockAuthService = new Mock<IAuthorizationService>();

            var cajaService = new CajaService(mockCajaSesionRepo.Object, mockCajaRepo.Object, mockAuthService.Object);

            decimal montoValido = 15000.50m;
            int idCajaSesion = 1;

            // 2. ACT
            cajaService.CerrarCaja(idCajaSesion, montoValido);

            // 3. ASSERT
            // Verificamos que el servicio haya llamado al guardián para pedir permiso de CAJA_CERRAR exactamente 1 vez
            mockAuthService.Verify(a => a.ValidarPermiso(Permisos.CAJA_CERRAR), Times.Once);

            // Verificamos que el servicio le haya enviado la orden al repositorio (base de datos falsa) exactamente 1 vez con los montos correctos
            mockCajaRepo.Verify(r => r.CerrarCaja(idCajaSesion, montoValido), Times.Once);
        }
    }
}