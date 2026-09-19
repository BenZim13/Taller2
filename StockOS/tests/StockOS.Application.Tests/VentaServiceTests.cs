using System.Collections.Generic;
using Moq;
using Xunit;
using StockOS.Application.Services;
using StockOS.Domain.Interfaces;
using StockOS.Domain.Entities;
using StockOS.Domain.Enums;

namespace StockOS.Application.Tests
{
    public class VentaServiceTests
    {
        [Fact]
        public void RegistrarVenta_VerificaPermisoYEjecutaRepositorio()
        {
            // 1. ARRANGE
            var mockRepo = new Mock<IVentaRepository>();
            var mockAuth = new Mock<IAuthorizationService>();

            // Simulamos que el repositorio devuelve el ID 100 cuando inserta la venta
            mockRepo.Setup(r => r.RegistrarVenta(It.IsAny<Venta>(), It.IsAny<List<DetalleVenta>>(), It.IsAny<int>(), It.IsAny<int>()))
                    .Returns(100);

            var service = new VentaService(mockRepo.Object, mockAuth.Object);

            var venta = new Venta();
            var detalles = new List<DetalleVenta>();

            // 2. ACT
            int resultadoId = service.RegistrarVenta(venta, detalles, 1, 1);

            // 3. ASSERT
            Assert.Equal(100, resultadoId);
            mockAuth.Verify(a => a.ValidarPermiso(Permisos.VENTAS_REALIZAR), Times.Once);
        }
    }
}