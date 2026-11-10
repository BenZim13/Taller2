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

        [Fact]
        public async Task ObtenerTopProductosVendidosAsync_SinPermiso_LanzaUnauthorizedAccessException()
        {
            var mockRepo = new Mock<IReporteRepository>();
            var mockAuth = new Mock<IAuthorizationService>();
            mockAuth.Setup(a => a.ValidarPermiso(Permisos.REPORTES_VER))
                    .Throws(new UnauthorizedAccessException("Acceso denegado"));

            var service = new ReporteService(mockRepo.Object, mockAuth.Object);

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                service.ObtenerTopProductosVendidosAsync(5, DateTime.Today.AddDays(-30), DateTime.Today));

            mockRepo.Verify(r => r.ObtenerTopProductosVendidosAsync(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never);
        }

        [Fact]
        public async Task ObtenerTopProductosVendidosAsync_ConPermiso_RetornaTopNProductos()
        {
            var mockRepo = new Mock<IReporteRepository>();
            var mockAuth = new Mock<IAuthorizationService>();
            var esperados = new List<ReporteItemDTO>
            {
                new ReporteItemDTO { CodigoBarra = "111", NombreProducto = "Coca Cola 1.5L", CantidadTotal = 40, Subtotal = 80000m },
                new ReporteItemDTO { CodigoBarra = "222", NombreProducto = "Agua Mineral 2L", CantidadTotal = 25, Subtotal = 30000m }
            };

            mockRepo.Setup(r => r.ObtenerTopProductosVendidosAsync(5, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                    .ReturnsAsync(esperados);

            var service = new ReporteService(mockRepo.Object, mockAuth.Object);
            var resultado = await service.ObtenerTopProductosVendidosAsync(5, DateTime.Today.AddDays(-30), DateTime.Today);

            mockAuth.Verify(a => a.ValidarPermiso(Permisos.REPORTES_VER), Times.Once);
            Assert.Equal(esperados, resultado);
        }

        [Fact]
        public async Task ObtenerVentasPorLineaTiempoAsync_SinPermiso_LanzaUnauthorizedAccessException()
        {
            var mockRepo = new Mock<IReporteRepository>();
            var mockAuth = new Mock<IAuthorizationService>();
            mockAuth.Setup(a => a.ValidarPermiso(Permisos.REPORTES_VER))
                    .Throws(new UnauthorizedAccessException("Acceso denegado"));

            var service = new ReporteService(mockRepo.Object, mockAuth.Object);

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                service.ObtenerVentasPorLineaTiempoAsync(DateTime.Today.AddDays(-7), DateTime.Today));

            mockRepo.Verify(r => r.ObtenerVentasPorLineaTiempoAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never);
        }

        [Fact]
        public async Task ObtenerResumenFinancieroAsync_ConPermiso_CalculaBalanceNetoCorrectamente()
        {
            var mockRepo = new Mock<IReporteRepository>();
            var mockAuth = new Mock<IAuthorizationService>();
            var resumenMock = new ResumenFinancieroDTO
            {
                IngresosBrutos = 150000m,
                DescuentosOtorgados = 10000m,
                IngresosNetos = 140000m,
                EgresosBrutos = 90000m
            };

            mockRepo.Setup(r => r.ObtenerResumenFinancieroAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                    .ReturnsAsync(resumenMock);

            var service = new ReporteService(mockRepo.Object, mockAuth.Object);
            var resultado = await service.ObtenerResumenFinancieroAsync(DateTime.Today.AddDays(-30), DateTime.Today);

            mockAuth.Verify(a => a.ValidarPermiso(Permisos.REPORTES_VER), Times.Once);
            Assert.Equal(50000m, resultado.BalanceNeto);
        }
    }
}

