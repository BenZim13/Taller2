using System;
using Moq;
using Xunit;
using StockOS.Application.Services;
using StockOS.Domain.Interfaces;
using StockOS.Domain.Entities;
using StockOS.Domain.Enums;

namespace StockOS.Application.Tests
{
    public class ProductoServiceTests
    {
        // ==========================================
        // PRUEBAS DE ÉXITO
        // ==========================================

        [Fact]
        public void Agregar_ConDatosCompletos_GuardaProducto()
        {
            // ARRANGE
            var mockRepo = new Mock<IProductoRepository>();
            var mockAuth = new Mock<IAuthorizationService>();

            mockRepo.Setup(r => r.BuscarPorCodigoBarra("999")).Returns((Producto?)null);

            var service = new ProductoService(mockRepo.Object, mockAuth.Object);
            var productoNuevo = new Producto { CodigoBarra = "999", Nombre = "Galletitas", PrecioVentaActual = 800 };

            // ACT
            service.Agregar(productoNuevo);

            // ASSERT
            mockAuth.Verify(a => a.ValidarPermiso(Permisos.PRODUCTOS_CREAR), Times.Once);
            mockRepo.Verify(r => r.Agregar(productoNuevo), Times.Once);
        }

        [Fact]
        public void Actualizar_ConMismoCodigoDelMismoProducto_ActualizaExitosamente()
        {
            // ARRANGE
            var mockRepo = new Mock<IProductoRepository>();
            var mockAuth = new Mock<IAuthorizationService>();

            var producto = new Producto { IdProducto = 10, CodigoBarra = "123", Nombre = "Arroz", PrecioVentaActual = 1200 };
            mockRepo.Setup(r => r.BuscarPorCodigoBarra("123"))
                    .Returns(new Producto { IdProducto = 10, CodigoBarra = "123", Nombre = "Arroz" });

            var service = new ProductoService(mockRepo.Object, mockAuth.Object);

            // ACT
            service.Actualizar(producto);

            // ASSERT
            mockAuth.Verify(a => a.ValidarPermiso(Permisos.PRODUCTOS_EDITAR), Times.Once);
            mockRepo.Verify(r => r.Actualizar(producto), Times.Once);
        }

        [Fact]
        public void BuscarPorCodigoBarra_CuandoExiste_RetornaProducto()
        {
            // ARRANGE
            var mockRepo = new Mock<IProductoRepository>();
            var mockAuth = new Mock<IAuthorizationService>();

            var esperado = new Producto { IdProducto = 1, CodigoBarra = "779123456", Nombre = "Yerba" };
            mockRepo.Setup(r => r.BuscarPorCodigoBarra("779123456")).Returns(esperado);

            var service = new ProductoService(mockRepo.Object, mockAuth.Object);

            // ACT
            var resultado = service.BuscarPorCodigoBarra("779123456");

            // ASSERT
            Assert.NotNull(resultado);
            Assert.Equal("Yerba", resultado.Nombre);
            mockAuth.Verify(a => a.ValidarPermiso(Permisos.PRODUCTOS_VER), Times.Once);
        }

        // ==========================================
        // PRUEBAS DE FRACASO Y VALIDACIÓN
        // ==========================================

        [Fact]
        public void Agregar_ConCodigoDuplicado_LanzaInvalidOperationException()
        {
            // ARRANGE
            var mockRepo = new Mock<IProductoRepository>();
            var mockAuth = new Mock<IAuthorizationService>();

            mockRepo.Setup(r => r.BuscarPorCodigoBarra("123"))
                    .Returns(new Producto { IdProducto = 1, CodigoBarra = "123", Nombre = "Gaseosa" });

            var service = new ProductoService(mockRepo.Object, mockAuth.Object);
            var productoNuevo = new Producto { CodigoBarra = "123", Nombre = "Jugo", PrecioVentaActual = 1500 };

            // ACT & ASSERT
            var excepcion = Assert.Throws<InvalidOperationException>(() => service.Agregar(productoNuevo));
            Assert.Contains("Ya existe un producto registrado", excepcion.Message);
            mockRepo.Verify(r => r.Agregar(It.IsAny<Producto>()), Times.Never);
        }

        [Fact]
        public void Agregar_SinPermiso_LanzaUnauthorizedAccessException()
        {
            // ARRANGE
            var mockRepo = new Mock<IProductoRepository>();
            var mockAuth = new Mock<IAuthorizationService>();

            mockAuth.Setup(a => a.ValidarPermiso(Permisos.PRODUCTOS_CREAR))
                    .Throws(new UnauthorizedAccessException("Denegado"));

            var service = new ProductoService(mockRepo.Object, mockAuth.Object);
            var producto = new Producto { CodigoBarra = "001", Nombre = "Pan", PrecioVentaActual = 500 };

            // ACT & ASSERT
            Assert.Throws<UnauthorizedAccessException>(() => service.Agregar(producto));
            mockRepo.Verify(r => r.Agregar(It.IsAny<Producto>()), Times.Never);
        }

        [Fact]
        public void Agregar_ConProductoNulo_LanzaArgumentNullException()
        {
            // ARRANGE
            var mockRepo = new Mock<IProductoRepository>();
            var mockAuth = new Mock<IAuthorizationService>();

            var service = new ProductoService(mockRepo.Object, mockAuth.Object);

            // ACT & ASSERT
            Assert.Throws<ArgumentNullException>(() => service.Agregar(null!));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Agregar_ConCodigoBarraVacio_LanzaArgumentException(string? codigoInvalido)
        {
            // ARRANGE
            var mockRepo = new Mock<IProductoRepository>();
            var mockAuth = new Mock<IAuthorizationService>();

            var service = new ProductoService(mockRepo.Object, mockAuth.Object);
            var producto = new Producto { CodigoBarra = codigoInvalido!, Nombre = "Pan", PrecioVentaActual = 500 };

            // ACT & ASSERT
            var ex = Assert.Throws<ArgumentException>(() => service.Agregar(producto));
            Assert.Contains("código de barra", ex.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Agregar_ConNombreVacio_LanzaArgumentException(string? nombreInvalido)
        {
            // ARRANGE
            var mockRepo = new Mock<IProductoRepository>();
            var mockAuth = new Mock<IAuthorizationService>();

            var service = new ProductoService(mockRepo.Object, mockAuth.Object);
            var producto = new Producto { CodigoBarra = "12345", Nombre = nombreInvalido!, PrecioVentaActual = 500 };

            // ACT & ASSERT
            var ex = Assert.Throws<ArgumentException>(() => service.Agregar(producto));
            Assert.Contains("nombre", ex.Message);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-50)]
        public void Agregar_ConPrecioInvalido_LanzaArgumentException(decimal precioInvalido)
        {
            // ARRANGE
            var mockRepo = new Mock<IProductoRepository>();
            var mockAuth = new Mock<IAuthorizationService>();

            var service = new ProductoService(mockRepo.Object, mockAuth.Object);
            var producto = new Producto { CodigoBarra = "12345", Nombre = "Fideos", PrecioVentaActual = precioInvalido };

            // ACT & ASSERT - Verificamos que frene ambos casos
            var ex = Assert.Throws<ArgumentException>(() => service.Agregar(producto));
            Assert.Contains("precio de venta", ex.Message);
        }

        [Fact]
        public void Actualizar_ConCodigoPertenecienteAOtroProducto_LanzaInvalidOperationException()
        {
            // ARRANGE
            var mockRepo = new Mock<IProductoRepository>();
            var mockAuth = new Mock<IAuthorizationService>();

            // El producto a editar tiene IdProducto = 1
            var productoAEditar = new Producto { IdProducto = 1, CodigoBarra = "999", Nombre = "Jabon", PrecioVentaActual = 100 };

            // Pero el código "999" ya le pertenece al IdProducto = 2
            mockRepo.Setup(r => r.BuscarPorCodigoBarra("999"))
                    .Returns(new Producto { IdProducto = 2, CodigoBarra = "999", Nombre = "Shampoo" });

            var service = new ProductoService(mockRepo.Object, mockAuth.Object);

            // ACT & ASSERT
            var ex = Assert.Throws<InvalidOperationException>(() => service.Actualizar(productoAEditar));
            Assert.Contains("ya pertenece a otro producto", ex.Message);
            mockRepo.Verify(r => r.Actualizar(It.IsAny<Producto>()), Times.Never);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void BuscarPorCodigoBarra_ConParametroVacioONulo_RetornaNullSinLlamarRepositorio(string? codigo)
        {
            // ARRANGE
            var mockRepo = new Mock<IProductoRepository>();
            var mockAuth = new Mock<IAuthorizationService>();

            var service = new ProductoService(mockRepo.Object, mockAuth.Object);

            // ACT
            var resultado = service.BuscarPorCodigoBarra(codigo!);

            // ASSERT
            Assert.Null(resultado);
            mockRepo.Verify(r => r.BuscarPorCodigoBarra(It.IsAny<string>()), Times.Never);
        }
    }
}