using System;
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
        // ==========================================
        // PRUEBAS DE ÉXITO (Casos Felices)
        // ==========================================

        [Fact]
        public void RegistrarVenta_ConPermiso_VerificaPermisoYEjecutaRepositorio()
        {
            // 1. ARRANGE
            var mockRepo = new Mock<IVentaRepository>();
            var mockAuth = new Mock<IAuthorizationService>();

            mockRepo.Setup(r => r.RegistrarVenta(It.IsAny<Venta>(), It.IsAny<List<DetalleVenta>>(), It.IsAny<int>(), It.IsAny<int>()))
                    .Returns(100);

            var service = new VentaService(mockRepo.Object, mockAuth.Object);
            var venta = new Venta { TotalVenta = 500m };
            var detalles = new List<DetalleVenta>
            {
                new DetalleVenta { IdProducto = 1, Cantidad = 2, PrecioUnitarioHistorico = 250m }
            };

            // 2. ACT
            int resultadoId = service.RegistrarVenta(venta, detalles, 1, 1);

            // 3. ASSERT
            Assert.Equal(100, resultadoId);
            mockAuth.Verify(a => a.ValidarPermiso(Permisos.VENTAS_REALIZAR), Times.Once);
            mockRepo.Verify(r => r.RegistrarVenta(venta, detalles, 1, 1), Times.Once);
        }

        [Fact]
        public void RegistrarVenta_ConMetodoPagoElectronico_PasaIdMetodoPagoCorrecto()
        {
            // ARRANGE
            var mockRepo = new Mock<IVentaRepository>();
            var mockAuth = new Mock<IAuthorizationService>();

            var venta = new Venta { TotalVenta = 1200m };
            var detalles = new List<DetalleVenta>
            {
                new DetalleVenta { IdProducto = 1, Cantidad = 1, PrecioUnitarioHistorico = 1200m }
            };
            int idMetodoDebito = 2; // Tarjeta de Débito
            int idSucursal = 1;

            mockRepo.Setup(r => r.RegistrarVenta(venta, detalles, idSucursal, idMetodoDebito)).Returns(105);

            var service = new VentaService(mockRepo.Object, mockAuth.Object);

            // ACT
            int resultadoId = service.RegistrarVenta(venta, detalles, idSucursal, idMetodoDebito);

            // ASSERT
            Assert.Equal(105, resultadoId);
            mockRepo.Verify(r => r.RegistrarVenta(venta, detalles, idSucursal, idMetodoDebito), Times.Once);
        }

        // ==========================================
        // PRUEBAS DE FRACASO (Permisos y Validaciones)
        // ==========================================

        [Fact]
        public void RegistrarVenta_SinPermiso_LanzaUnauthorizedAccessException()
        {
            // 1. ARRANGE
            var mockRepo = new Mock<IVentaRepository>();
            var mockAuth = new Mock<IAuthorizationService>();

            mockAuth.Setup(a => a.ValidarPermiso(Permisos.VENTAS_REALIZAR))
                    .Throws(new UnauthorizedAccessException("Denegado"));

            var service = new VentaService(mockRepo.Object, mockAuth.Object);
            var detalles = new List<DetalleVenta>
            {
                new DetalleVenta { IdProducto = 1, Cantidad = 1, PrecioUnitarioHistorico = 100m }
            };

            // 2. ACT & ASSERT
            Assert.Throws<UnauthorizedAccessException>(() => service.RegistrarVenta(new Venta(), detalles, 1, 1));
            mockRepo.Verify(r => r.RegistrarVenta(It.IsAny<Venta>(), It.IsAny<List<DetalleVenta>>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void RegistrarVenta_VentaNula_LanzaArgumentNullException()
        {
            // ARRANGE
            var mockRepo = new Mock<IVentaRepository>();
            var mockAuth = new Mock<IAuthorizationService>();
            var service = new VentaService(mockRepo.Object, mockAuth.Object);
            var detalles = new List<DetalleVenta>
            {
                new DetalleVenta { IdProducto = 1, Cantidad = 1, PrecioUnitarioHistorico = 100m }
            };

            // ACT & ASSERT
            Assert.Throws<ArgumentNullException>(() => service.RegistrarVenta(null!, detalles, 1, 1));
            mockRepo.Verify(r => r.RegistrarVenta(It.IsAny<Venta>(), It.IsAny<List<DetalleVenta>>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void RegistrarVenta_DetallesVaciosONulos_LanzaArgumentException()
        {
            // ARRANGE
            var mockRepo = new Mock<IVentaRepository>();
            var mockAuth = new Mock<IAuthorizationService>();
            var service = new VentaService(mockRepo.Object, mockAuth.Object);
            var venta = new Venta { TotalVenta = 500m };

            // ACT & ASSERT
            Assert.Throws<ArgumentException>(() => service.RegistrarVenta(venta, null!, 1, 1));
            Assert.Throws<ArgumentException>(() => service.RegistrarVenta(venta, new List<DetalleVenta>(), 1, 1));
            mockRepo.Verify(r => r.RegistrarVenta(It.IsAny<Venta>(), It.IsAny<List<DetalleVenta>>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-2)]
        public void RegistrarVenta_CantidadInvalida_LanzaArgumentException(int cantidadInvalida)
        {
            // ARRANGE
            var mockRepo = new Mock<IVentaRepository>();
            var mockAuth = new Mock<IAuthorizationService>();
            var service = new VentaService(mockRepo.Object, mockAuth.Object);
            var venta = new Venta { TotalVenta = 500m };
            var detalles = new List<DetalleVenta>
            {
                new DetalleVenta { IdProducto = 1, Cantidad = cantidadInvalida, PrecioUnitarioHistorico = 100m }
            };

            // ACT & ASSERT
            var ex = Assert.Throws<ArgumentException>(() => service.RegistrarVenta(venta, detalles, 1, 1));
            Assert.Contains("cantidad", ex.Message);
            mockRepo.Verify(r => r.RegistrarVenta(It.IsAny<Venta>(), It.IsAny<List<DetalleVenta>>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void RegistrarVenta_PrecioUnitarioNegativo_LanzaArgumentException()
        {
            // ARRANGE
            var mockRepo = new Mock<IVentaRepository>();
            var mockAuth = new Mock<IAuthorizationService>();
            var service = new VentaService(mockRepo.Object, mockAuth.Object);
            var venta = new Venta { TotalVenta = 500m };
            var detalles = new List<DetalleVenta>
            {
                new DetalleVenta { IdProducto = 1, Cantidad = 1, PrecioUnitarioHistorico = -50m }
            };

            // ACT & ASSERT
            var ex = Assert.Throws<ArgumentException>(() => service.RegistrarVenta(venta, detalles, 1, 1));
            Assert.Contains("precio", ex.Message);
            mockRepo.Verify(r => r.RegistrarVenta(It.IsAny<Venta>(), It.IsAny<List<DetalleVenta>>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }
    }
}

