using System;
using Moq;
using Xunit;
using StockOS.Application.Services;
using StockOS.Domain.Interfaces;
using StockOS.Domain.Enums;

namespace StockOS.Application.Tests
{
    public class StockServiceTests
    {
        [Fact]
        public void AgregarStock_ConPermiso_EjecutaRepositorio()
        {
            // ARRANGE
            var mockRepo = new Mock<IStockSucursalRepository>();
            var mockAuth = new Mock<IAuthorizationService>();

            var service = new StockService(mockRepo.Object, mockAuth.Object);

            // ACT
            service.AgregarStock(10, 1, 50);

            // ASSERT
            mockAuth.Verify(a => a.ValidarPermiso(Permisos.STOCK_INGRESAR), Times.Once);
            mockRepo.Verify(r => r.IngresarMercaderia(10, 1, 50), Times.Once);
        }

        [Fact]
        public void AgregarStock_SinPermiso_LanzaUnauthorizedAccessException()
        {
            // ARRANGE
            var mockRepo = new Mock<IStockSucursalRepository>();
            var mockAuth = new Mock<IAuthorizationService>();

            mockAuth.Setup(a => a.ValidarPermiso(Permisos.STOCK_INGRESAR))
                    .Throws(new UnauthorizedAccessException("Denegado"));

            var service = new StockService(mockRepo.Object, mockAuth.Object);

            // ACT & ASSERT
            Assert.Throws<UnauthorizedAccessException>(() => service.AgregarStock(10, 1, 50));
            mockRepo.Verify(r => r.IngresarMercaderia(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void ObtenerCantidadActual_ConPermiso_RetornaCantidadCorrecta()
        {
            // ARRANGE
            var mockRepo = new Mock<IStockSucursalRepository>();
            var mockAuth = new Mock<IAuthorizationService>();

            mockRepo.Setup(r => r.ObtenerCantidadActual(5, 1)).Returns(42);

            var service = new StockService(mockRepo.Object, mockAuth.Object);

            // ACT
            int cantidad = service.ObtenerCantidadActual(5, 1);

            // ASSERT
            Assert.Equal(42, cantidad);
            mockAuth.Verify(a => a.ValidarPermiso(Permisos.STOCK_VER), Times.Once);
        }

        [Fact]
        public void ObtenerCantidadActual_SinPermiso_LanzaUnauthorizedAccessException()
        {
            // ARRANGE
            var mockRepo = new Mock<IStockSucursalRepository>();
            var mockAuth = new Mock<IAuthorizationService>();

            mockAuth.Setup(a => a.ValidarPermiso(Permisos.STOCK_VER))
                    .Throws(new UnauthorizedAccessException("Denegado"));

            var service = new StockService(mockRepo.Object, mockAuth.Object);

            // ACT & ASSERT
            Assert.Throws<UnauthorizedAccessException>(() => service.ObtenerCantidadActual(5, 1));
            mockRepo.Verify(r => r.ObtenerCantidadActual(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }
    }
}

