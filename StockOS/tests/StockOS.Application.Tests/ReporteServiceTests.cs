using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using StockOS.Application.Services;
using StockOS.Domain.DTOs;
using StockOS.Domain.Enums;
using StockOS.Domain.Interfaces;
using Xunit;

namespace StockOS.Application.Tests
{
    public class ReporteServiceTests
    {
        [Fact]
        public async Task GenerarReporteVentasPdfAsync_SinPermiso_LanzaUnauthorizedAccessException()
        {
            // ARRANGE
            var mockRepo = new Mock<IReporteRepository>();
            var mockAuth = new Mock<IAuthorizationService>();
            mockAuth.Setup(a => a.ValidarPermiso(Permisos.REPORTES_VER))
                    .Throws(new UnauthorizedAccessException("Acceso denegado"));

            var service = new ReporteService(mockRepo.Object, mockAuth.Object);

            // ACT & ASSERT
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                service.GenerarReporteVentasPdfAsync(DateTime.Today.AddDays(-7), DateTime.Today, "Semana"));

            mockRepo.Verify(r => r.ObtenerReporteVentasAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never);
        }

        [Fact]
        public async Task GenerarReporteComprasPdfAsync_SinPermiso_LanzaUnauthorizedAccessException()
        {
            // ARRANGE
            var mockRepo = new Mock<IReporteRepository>();
            var mockAuth = new Mock<IAuthorizationService>();
            mockAuth.Setup(a => a.ValidarPermiso(Permisos.REPORTES_VER))
                    .Throws(new UnauthorizedAccessException("Acceso denegado"));

            var service = new ReporteService(mockRepo.Object, mockAuth.Object);

            // ACT & ASSERT
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                service.GenerarReporteComprasPdfAsync(DateTime.Today.AddDays(-30), DateTime.Today, "Mes"));

            mockRepo.Verify(r => r.ObtenerReporteComprasAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never);
        }

        [Fact]
        public async Task GenerarReporteVentasPdfAsync_ConPermiso_InvocaRepositorioYGeneraPdf()
        {
            // ARRANGE
            var mockRepo = new Mock<IReporteRepository>();
            var mockAuth = new Mock<IAuthorizationService>();

            var detalles = new List<ReporteItemDTO>
            {
                new ReporteItemDTO { CodigoBarra = "7791234567890", NombreProducto = "Café 500g", CantidadTotal = 10, Subtotal = 25000m }
            };

            mockRepo.Setup(r => r.ObtenerReporteVentasAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                    .ReturnsAsync((25000m, detalles));

            var service = new ReporteService(mockRepo.Object, mockAuth.Object);

            // ACT
            await service.GenerarReporteVentasPdfAsync(DateTime.Today.AddDays(-1), DateTime.Today, "Día");

            // ASSERT
            mockAuth.Verify(a => a.ValidarPermiso(Permisos.REPORTES_VER), Times.Once);
            mockRepo.Verify(r => r.ObtenerReporteVentasAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
        }

        [Fact]
        public async Task GenerarReporteComprasPdfAsync_ConPermiso_InvocaRepositorioYGeneraPdf()
        {
            // ARRANGE
            var mockRepo = new Mock<IReporteRepository>();
            var mockAuth = new Mock<IAuthorizationService>();

            var detalles = new List<ReporteItemDTO>
            {
                new ReporteItemDTO { CodigoBarra = "7799876543210", NombreProducto = "Harina 1kg", CantidadTotal = 50, Subtotal = 30000m }
            };

            mockRepo.Setup(r => r.ObtenerReporteComprasAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                    .ReturnsAsync((30000m, detalles));

            var service = new ReporteService(mockRepo.Object, mockAuth.Object);

            // ACT
            await service.GenerarReporteComprasPdfAsync(DateTime.Today.AddDays(-7), DateTime.Today, "Semana");

            // ASSERT
            mockAuth.Verify(a => a.ValidarPermiso(Permisos.REPORTES_VER), Times.Once);
            mockRepo.Verify(r => r.ObtenerReporteComprasAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
        }
    }
}

