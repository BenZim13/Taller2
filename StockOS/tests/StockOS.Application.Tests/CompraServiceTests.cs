using System;
using Moq;
using Xunit;
using StockOS.Application.Services;
using StockOS.Domain.Entities;
using StockOS.Domain.Interfaces;
using StockOS.Domain.Enums;

namespace StockOS.Application.Tests
{
    public class CompraServiceTests
    {
        // ==========================================
        // PRUEBAS DE ÉXITO (Casos Felices)
        // ==========================================

        [Fact]
        public void RegistrarCompra_ConDatosValidos_EjecutaRepositorioYRetornaId()
        {
            // ARRANGE
            var mockRepo = new Mock<ICompraRepository>();
            var mockAuth = new Mock<IAuthorizationService>();
            var cabecera = new Compra
            {
                FechaHora = DateTime.Now,
                Total = 25000m,
                IdProveedor = 1,
                IdEmpleado = 2,
                IdSucursal = 1
            };
            var detalle = new DetalleCompra
            {
                IdProducto = 10,
                Cantidad = 50,
                PrecioUnitarioCompra = 500m
            };

            mockRepo.Setup(r => r.RegistrarCompra(cabecera, detalle)).Returns(99);

            var service = new CompraService(mockRepo.Object, mockAuth.Object);

            // ACT
            int idCompra = service.RegistrarCompra(cabecera, detalle);

            // ASSERT
            Assert.Equal(99, idCompra);
            mockAuth.Verify(a => a.ValidarPermiso(Permisos.COMPRAS_GESTIONAR), Times.Once);
            mockRepo.Verify(r => r.RegistrarCompra(cabecera, detalle), Times.Once);
        }

        [Fact]
        public void ObtenerUltimoPrecioCompra_ProductoConHistorial_RetornaPrecio()
        {
            // ARRANGE
            var mockRepo = new Mock<ICompraRepository>();
            var mockAuth = new Mock<IAuthorizationService>();
            mockRepo.Setup(r => r.ObtenerUltimoPrecioCompra(15)).Returns(1250.75m);

            var service = new CompraService(mockRepo.Object, mockAuth.Object);

            // ACT
            decimal? precio = service.ObtenerUltimoPrecioCompra(15);

            // ASSERT
            Assert.NotNull(precio);
            Assert.Equal(1250.75m, precio.Value);
            mockRepo.Verify(r => r.ObtenerUltimoPrecioCompra(15), Times.Once);
        }

        [Fact]
        public void ActualizarPrecioCompra_ConParametrosValidos_EjecutaRepositorio()
        {
            // ARRANGE
            var mockRepo = new Mock<ICompraRepository>();
            var mockAuth = new Mock<IAuthorizationService>();
            var service = new CompraService(mockRepo.Object, mockAuth.Object);

            // ACT
            service.ActualizarPrecioCompra(15, 1400.00m);

            // ASSERT
            mockAuth.Verify(a => a.ValidarPermiso(Permisos.COMPRAS_GESTIONAR), Times.Once);
            mockRepo.Verify(r => r.ActualizarPrecioCompra(15, 1400.00m), Times.Once);
        }

        // ==========================================
        // PRUEBAS DE CASOS LÍMITE Y FALLO
        // ==========================================

        [Fact]
        public void ObtenerUltimoPrecioCompra_ProductoSinComprasPrevias_RetornaNull()
        {
            // ARRANGE
            var mockRepo = new Mock<ICompraRepository>();
            var mockAuth = new Mock<IAuthorizationService>();
            mockRepo.Setup(r => r.ObtenerUltimoPrecioCompra(999)).Returns((decimal?)null);

            var service = new CompraService(mockRepo.Object, mockAuth.Object);

            // ACT
            decimal? precio = service.ObtenerUltimoPrecioCompra(999);

            // ASSERT
            Assert.Null(precio);
            mockRepo.Verify(r => r.ObtenerUltimoPrecioCompra(999), Times.Once);
        }

        [Fact]
        public void RegistrarCompra_SinPermiso_LanzaUnauthorizedAccessException()
        {
            // ARRANGE
            var mockRepo = new Mock<ICompraRepository>();
            var mockAuth = new Mock<IAuthorizationService>();
            mockAuth.Setup(a => a.ValidarPermiso(Permisos.COMPRAS_GESTIONAR))
                    .Throws(new UnauthorizedAccessException("Denegado"));

            var service = new CompraService(mockRepo.Object, mockAuth.Object);
            var cabecera = new Compra { Total = 1000m, IdProveedor = 1, IdEmpleado = 1, IdSucursal = 1 };
            var detalle = new DetalleCompra { IdProducto = 1, Cantidad = 10, PrecioUnitarioCompra = 100m };

            // ACT & ASSERT
            Assert.Throws<UnauthorizedAccessException>(() => service.RegistrarCompra(cabecera, detalle));
            mockRepo.Verify(r => r.RegistrarCompra(It.IsAny<Compra>(), It.IsAny<DetalleCompra>()), Times.Never);
        }

        [Fact]
        public void RegistrarCompra_CabeceraNula_LanzaArgumentNullException()
        {
            // ARRANGE
            var mockRepo = new Mock<ICompraRepository>();
            var mockAuth = new Mock<IAuthorizationService>();
            var service = new CompraService(mockRepo.Object, mockAuth.Object);
            var detalle = new DetalleCompra { IdProducto = 1, Cantidad = 10, PrecioUnitarioCompra = 100m };

            // ACT & ASSERT
            Assert.Throws<ArgumentNullException>(() => service.RegistrarCompra(null!, detalle));
            mockRepo.Verify(r => r.RegistrarCompra(It.IsAny<Compra>(), It.IsAny<DetalleCompra>()), Times.Never);
        }

        [Fact]
        public void RegistrarCompra_DetalleNulo_LanzaArgumentNullException()
        {
            // ARRANGE
            var mockRepo = new Mock<ICompraRepository>();
            var mockAuth = new Mock<IAuthorizationService>();
            var service = new CompraService(mockRepo.Object, mockAuth.Object);
            var cabecera = new Compra { Total = 1000m, IdProveedor = 1, IdEmpleado = 1, IdSucursal = 1 };

            // ACT & ASSERT
            Assert.Throws<ArgumentNullException>(() => service.RegistrarCompra(cabecera, null!));
            mockRepo.Verify(r => r.RegistrarCompra(It.IsAny<Compra>(), It.IsAny<DetalleCompra>()), Times.Never);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        public void RegistrarCompra_CantidadInvalida_LanzaArgumentException(int cantidadInvalida)
        {
            // ARRANGE
            var mockRepo = new Mock<ICompraRepository>();
            var mockAuth = new Mock<IAuthorizationService>();
            var service = new CompraService(mockRepo.Object, mockAuth.Object);
            var cabecera = new Compra { Total = 1000m, IdProveedor = 1, IdEmpleado = 1, IdSucursal = 1 };
            var detalle = new DetalleCompra { IdProducto = 1, Cantidad = cantidadInvalida, PrecioUnitarioCompra = 100m };

            // ACT & ASSERT
            var ex = Assert.Throws<ArgumentException>(() => service.RegistrarCompra(cabecera, detalle));
            Assert.Contains("cantidad", ex.Message);
            mockRepo.Verify(r => r.RegistrarCompra(It.IsAny<Compra>(), It.IsAny<DetalleCompra>()), Times.Never);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-100)]
        public void RegistrarCompra_PrecioInvalido_LanzaArgumentException(decimal precioInvalido)
        {
            // ARRANGE
            var mockRepo = new Mock<ICompraRepository>();
            var mockAuth = new Mock<IAuthorizationService>();
            var service = new CompraService(mockRepo.Object, mockAuth.Object);
            var cabecera = new Compra { Total = 1000m, IdProveedor = 1, IdEmpleado = 1, IdSucursal = 1 };
            var detalle = new DetalleCompra { IdProducto = 1, Cantidad = 10, PrecioUnitarioCompra = precioInvalido };

            // ACT & ASSERT
            var ex = Assert.Throws<ArgumentException>(() => service.RegistrarCompra(cabecera, detalle));
            Assert.Contains("precio unitario", ex.Message);
            mockRepo.Verify(r => r.RegistrarCompra(It.IsAny<Compra>(), It.IsAny<DetalleCompra>()), Times.Never);
        }

        [Fact]
        public void ActualizarPrecioCompra_SinPermiso_LanzaUnauthorizedAccessException()
        {
            // ARRANGE
            var mockRepo = new Mock<ICompraRepository>();
            var mockAuth = new Mock<IAuthorizationService>();
            mockAuth.Setup(a => a.ValidarPermiso(Permisos.COMPRAS_GESTIONAR))
                    .Throws(new UnauthorizedAccessException("Denegado"));

            var service = new CompraService(mockRepo.Object, mockAuth.Object);

            // ACT & ASSERT
            Assert.Throws<UnauthorizedAccessException>(() => service.ActualizarPrecioCompra(15, 1400.00m));
            mockRepo.Verify(r => r.ActualizarPrecioCompra(It.IsAny<int>(), It.IsAny<decimal>()), Times.Never);
        }
    }
}

