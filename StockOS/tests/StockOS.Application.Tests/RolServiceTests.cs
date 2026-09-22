using System.Collections.Generic;
using Moq;
using StockOS.Application.Services;
using StockOS.Domain.Entities;
using StockOS.Domain.Interfaces;
using Xunit;

namespace StockOS.Application.Tests
{
    public class RolServiceTests
    {
        [Fact]
        public void ObtenerRoles_CuandoExistenRoles_RetornaColeccionDeRoles()
        {
            // ARRANGE
            var mockRepo = new Mock<IRolRepository>();
            var roles = new List<Rol>
            {
                new Rol { IdRol = 1, Nombre = "Administrador" },
                new Rol { IdRol = 2, Nombre = "Cajero" },
                new Rol { IdRol = 3, Nombre = "Encargado de Depósito" },
                new Rol { IdRol = 4, Nombre = "Repositor" }
            };

            mockRepo.Setup(r => r.ObtenerTodos()).Returns(roles);

            var service = new RolService(mockRepo.Object);

            // ACT
            var resultado = service.ObtenerRoles();

            // ASSERT
            Assert.NotNull(resultado);
            Assert.Equal(4, (resultado as List<Rol>)?.Count);
            mockRepo.Verify(r => r.ObtenerTodos(), Times.Once);
        }

        [Fact]
        public void ObtenerRoles_CuandoNoHayRoles_RetornaVacio()
        {
            // ARRANGE
            var mockRepo = new Mock<IRolRepository>();
            mockRepo.Setup(r => r.ObtenerTodos()).Returns(new List<Rol>());

            var service = new RolService(mockRepo.Object);

            // ACT
            var resultado = service.ObtenerRoles();

            // ASSERT
            Assert.NotNull(resultado);
            Assert.Empty(resultado);
            mockRepo.Verify(r => r.ObtenerTodos(), Times.Once);
        }
    }
}

