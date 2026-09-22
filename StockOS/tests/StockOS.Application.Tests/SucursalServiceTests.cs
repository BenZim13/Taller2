using System.Collections.Generic;
using Moq;
using StockOS.Application.Services;
using StockOS.Domain.Entities;
using StockOS.Domain.Interfaces;
using Xunit;

namespace StockOS.Application.Tests
{
    public class SucursalServiceTests
    {
        [Fact]
        public void ObtenerSucursales_CuandoExistenSucursales_RetornaListadoCompleto()
        {
            // ARRANGE
            var mockRepo = new Mock<ISucursalRepository>();
            var listaEsperada = new List<Sucursal>
            {
                new Sucursal { IdSucursal = 1, Nombre = "Sucursal Central", Direccion = "Av. Principal 123", Activo = true },
                new Sucursal { IdSucursal = 2, Nombre = "Sucursal Norte", Direccion = "Ruta 12 Km 5", Activo = true }
            };

            mockRepo.Setup(r => r.ObtenerTodas()).Returns(listaEsperada);

            var service = new SucursalService(mockRepo.Object);

            // ACT
            var resultado = service.ObtenerSucursales();

            // ASSERT
            Assert.NotNull(resultado);
            Assert.Equal(2, (resultado as List<Sucursal>)?.Count);
            mockRepo.Verify(r => r.ObtenerTodas(), Times.Once);
        }

        [Fact]
        public void ObtenerSucursales_CuandoNoHaySucursales_RetornaColeccionVacia()
        {
            // ARRANGE
            var mockRepo = new Mock<ISucursalRepository>();
            mockRepo.Setup(r => r.ObtenerTodas()).Returns(new List<Sucursal>());

            var service = new SucursalService(mockRepo.Object);

            // ACT
            var resultado = service.ObtenerSucursales();

            // ASSERT
            Assert.NotNull(resultado);
            Assert.Empty(resultado);
            mockRepo.Verify(r => r.ObtenerTodas(), Times.Once);
        }
    }
}

