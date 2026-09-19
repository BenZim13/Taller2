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
        [Fact]
        public void Agregar_ConCodigoDuplicado_LanzaExcepcion()
        {
            // 1. ARRANGE
            var mockRepo = new Mock<IProductoRepository>();
            var mockAuth = new Mock<IAuthorizationService>();

            // Simulamos que ya existe un producto con el código "123"
            mockRepo.Setup(r => r.BuscarPorCodigoBarra("123"))
                    .Returns(new Producto { IdProducto = 1, CodigoBarra = "123", Nombre = "Gaseosa" });

            var service = new ProductoService(mockRepo.Object, mockAuth.Object);

            var productoNuevo = new Producto { CodigoBarra = "123", Nombre = "Jugo", PrecioVentaActual = 1500 };

            // 2 & 3. ACT & ASSERT
            var excepcion = Assert.Throws<InvalidOperationException>(() => service.Agregar(productoNuevo));
            Assert.Contains("Ya existe un producto registrado", excepcion.Message);
        }

        [Fact]
        public void Agregar_ConDatosCompletos_GuardaProducto()
        {
            // 1. ARRANGE
            var mockRepo = new Mock<IProductoRepository>();
            var mockAuth = new Mock<IAuthorizationService>();

            // Simulamos que el código NO existe (retorna null)
            mockRepo.Setup(r => r.BuscarPorCodigoBarra("999")).Returns((Producto?)null);

            var service = new ProductoService(mockRepo.Object, mockAuth.Object);
            var productoNuevo = new Producto { CodigoBarra = "999", Nombre = "Galletitas", PrecioVentaActual = 800 };

            // 2. ACT
            service.Agregar(productoNuevo);

            // 3. ASSERT
            // Verificamos que el guardia pidió el permiso y que el repositorio intentó guardarlo
            mockAuth.Verify(a => a.ValidarPermiso(Permisos.PRODUCTOS_CREAR), Times.Once);
            mockRepo.Verify(r => r.Agregar(productoNuevo), Times.Once);
        }
    }
}