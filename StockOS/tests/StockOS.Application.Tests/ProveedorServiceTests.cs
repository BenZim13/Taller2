using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Xunit;
using StockOS.Application.Services;
using StockOS.Domain.Entities;
using StockOS.Domain.Interfaces;
using StockOS.Domain.Enums;

namespace StockOS.Application.Tests
{
    public class ProveedorServiceTests
    {
        // ==========================================
        // PRUEBAS DE ÉXITO (Casos Felices)
        // ==========================================

        [Fact]
        public void ObtenerTodos_RetornaListadoCompletoDeProveedores()
        {
            // ARRANGE
            var mockRepo = new Mock<IProveedorRepository>();
            var mockAuth = new Mock<IAuthorizationService>();
            var listaEsperada = new List<Proveedor>
            {
                new Proveedor { IdProveedor = 1, RazonSocial = "Distribuidora Norte SRL", Activo = true },
                new Proveedor { IdProveedor = 2, RazonSocial = "Lácteos del Valle SA", Activo = true }
            };
            mockRepo.Setup(r => r.ObtenerTodos()).Returns(listaEsperada);

            var service = new ProveedorService(mockRepo.Object, mockAuth.Object);

            // ACT
            var resultado = service.ObtenerTodos().ToList();

            // ASSERT
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count);
            Assert.Equal("Distribuidora Norte SRL", resultado[0].RazonSocial);
            mockRepo.Verify(r => r.ObtenerTodos(), Times.Once);
        }

        [Fact]
        public void Agregar_ConDatosValidos_SanitizaNulosYGuardaCorrectamente()
        {
            // ARRANGE
            var mockRepo = new Mock<IProveedorRepository>();
            var mockAuth = new Mock<IAuthorizationService>();
            var service = new ProveedorService(mockRepo.Object, mockAuth.Object);

            var proveedor = new Proveedor
            {
                RazonSocial = "  Bebidas y Alimentos SA  ",
                Cuit = " 30-71234567-8 ",
                Telefono = null!,
                Email = null!,
                Direccion = null!
            };

            // ACT
            service.Agregar(proveedor);

            // ASSERT
            mockAuth.Verify(a => a.ValidarPermiso(Permisos.PROVEEDORES_GESTIONAR), Times.Once);
            Assert.Equal("Bebidas y Alimentos SA", proveedor.RazonSocial);
            Assert.Equal("30-71234567-8", proveedor.Cuit);
            Assert.Equal("", proveedor.Telefono);
            Assert.Equal("", proveedor.Email);
            Assert.Equal("", proveedor.Direccion);
            mockRepo.Verify(r => r.Agregar(proveedor), Times.Once);
        }

        [Fact]
        public void Actualizar_ConDatosValidos_EjecutaRepositorioCorrectamente()
        {
            // ARRANGE
            var mockRepo = new Mock<IProveedorRepository>();
            var mockAuth = new Mock<IAuthorizationService>();
            var service = new ProveedorService(mockRepo.Object, mockAuth.Object);

            var proveedor = new Proveedor
            {
                IdProveedor = 5,
                RazonSocial = "Molinos Centrales",
                Cuit = "30-55667788-9",
                Telefono = "3794123456",
                Email = "ventas@molinos.com",
                Direccion = "Ruta 12 Km 5"
            };

            // ACT
            service.Actualizar(proveedor);

            // ASSERT
            mockAuth.Verify(a => a.ValidarPermiso(Permisos.PROVEEDORES_GESTIONAR), Times.Once);
            mockRepo.Verify(r => r.Actualizar(proveedor), Times.Once);
        }

        [Fact]
        public void CambiarEstado_ConParametroValido_EjecutaRepositorio()
        {
            // ARRANGE
            var mockRepo = new Mock<IProveedorRepository>();
            var mockAuth = new Mock<IAuthorizationService>();
            var service = new ProveedorService(mockRepo.Object, mockAuth.Object);

            // ACT
            service.CambiarEstado(10, false);

            // ASSERT
            mockAuth.Verify(a => a.ValidarPermiso(Permisos.PROVEEDORES_GESTIONAR), Times.Once);
            mockRepo.Verify(r => r.CambiarEstado(10, false), Times.Once);
        }

        // ==========================================
        // PRUEBAS DE FRACASO (Validaciones y Permisos)
        // ==========================================

        [Fact]
        public void Agregar_SinPermiso_LanzaUnauthorizedAccessException()
        {
            // ARRANGE
            var mockRepo = new Mock<IProveedorRepository>();
            var mockAuth = new Mock<IAuthorizationService>();
            mockAuth.Setup(a => a.ValidarPermiso(Permisos.PROVEEDORES_GESTIONAR))
                    .Throws(new UnauthorizedAccessException("Denegado"));

            var service = new ProveedorService(mockRepo.Object, mockAuth.Object);
            var proveedor = new Proveedor { RazonSocial = "Test" };

            // ACT & ASSERT
            Assert.Throws<UnauthorizedAccessException>(() => service.Agregar(proveedor));
            mockRepo.Verify(r => r.Agregar(It.IsAny<Proveedor>()), Times.Never);
        }

        [Fact]
        public void Actualizar_SinPermiso_LanzaUnauthorizedAccessException()
        {
            // ARRANGE
            var mockRepo = new Mock<IProveedorRepository>();
            var mockAuth = new Mock<IAuthorizationService>();
            mockAuth.Setup(a => a.ValidarPermiso(Permisos.PROVEEDORES_GESTIONAR))
                    .Throws(new UnauthorizedAccessException("Denegado"));

            var service = new ProveedorService(mockRepo.Object, mockAuth.Object);
            var proveedor = new Proveedor { IdProveedor = 1, RazonSocial = "Test" };

            // ACT & ASSERT
            Assert.Throws<UnauthorizedAccessException>(() => service.Actualizar(proveedor));
            mockRepo.Verify(r => r.Actualizar(It.IsAny<Proveedor>()), Times.Never);
        }

        [Fact]
        public void CambiarEstado_SinPermiso_LanzaUnauthorizedAccessException()
        {
            // ARRANGE
            var mockRepo = new Mock<IProveedorRepository>();
            var mockAuth = new Mock<IAuthorizationService>();
            mockAuth.Setup(a => a.ValidarPermiso(Permisos.PROVEEDORES_GESTIONAR))
                    .Throws(new UnauthorizedAccessException("Denegado"));

            var service = new ProveedorService(mockRepo.Object, mockAuth.Object);

            // ACT & ASSERT
            Assert.Throws<UnauthorizedAccessException>(() => service.CambiarEstado(1, false));
            mockRepo.Verify(r => r.CambiarEstado(It.IsAny<int>(), It.IsAny<bool>()), Times.Never);
        }

        [Fact]
        public void Agregar_ProveedorNulo_LanzaArgumentNullException()
        {
            // ARRANGE
            var mockRepo = new Mock<IProveedorRepository>();
            var mockAuth = new Mock<IAuthorizationService>();
            var service = new ProveedorService(mockRepo.Object, mockAuth.Object);

            // ACT & ASSERT
            Assert.Throws<ArgumentNullException>(() => service.Agregar(null!));
            mockRepo.Verify(r => r.Agregar(It.IsAny<Proveedor>()), Times.Never);
        }

        [Fact]
        public void Actualizar_ProveedorNulo_LanzaArgumentNullException()
        {
            // ARRANGE
            var mockRepo = new Mock<IProveedorRepository>();
            var mockAuth = new Mock<IAuthorizationService>();
            var service = new ProveedorService(mockRepo.Object, mockAuth.Object);

            // ACT & ASSERT
            Assert.Throws<ArgumentNullException>(() => service.Actualizar(null!));
            mockRepo.Verify(r => r.Actualizar(It.IsAny<Proveedor>()), Times.Never);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Agregar_ConRazonSocialVaciaONula_LanzaArgumentException(string? razonSocialInvalida)
        {
            // ARRANGE
            var mockRepo = new Mock<IProveedorRepository>();
            var mockAuth = new Mock<IAuthorizationService>();
            var service = new ProveedorService(mockRepo.Object, mockAuth.Object);

            var proveedor = new Proveedor
            {
                RazonSocial = razonSocialInvalida!,
                Cuit = "30-12345678-9"
            };

            // ACT & ASSERT
            var ex = Assert.Throws<ArgumentException>(() => service.Agregar(proveedor));
            Assert.Contains("obligatorio", ex.Message);
            mockRepo.Verify(r => r.Agregar(It.IsAny<Proveedor>()), Times.Never);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Actualizar_ConRazonSocialVaciaONula_LanzaArgumentException(string? razonSocialInvalida)
        {
            // ARRANGE
            var mockRepo = new Mock<IProveedorRepository>();
            var mockAuth = new Mock<IAuthorizationService>();
            var service = new ProveedorService(mockRepo.Object, mockAuth.Object);

            var proveedor = new Proveedor
            {
                IdProveedor = 1,
                RazonSocial = razonSocialInvalida!
            };

            // ACT & ASSERT
            var ex = Assert.Throws<ArgumentException>(() => service.Actualizar(proveedor));
            Assert.Contains("obligatorio", ex.Message);
            mockRepo.Verify(r => r.Actualizar(It.IsAny<Proveedor>()), Times.Never);
        }
    }
}

