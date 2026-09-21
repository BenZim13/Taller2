using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Xunit;
using StockOS.Application.Services;
using StockOS.Domain.Interfaces;
using StockOS.Domain.Entities;
using StockOS.Domain.Enums;

namespace StockOS.Application.Tests
{
    public class CategoriaServiceTests
    {
        // ==========================================
        // PRUEBAS DE ÉXITO (Casos Felices)
        // ==========================================

        [Fact]
        public void ObtenerTodos_RetornaListadoDeCategoriasSinRestriccion()
        {
            // ARRANGE
            var mockRepo = new Mock<ICategoriaRepository>();
            var mockAuth = new Mock<IAuthorizationService>();
            var categorias = new List<Categoria>
            {
                new Categoria { IdCategoria = 1, Nombre = "Bebidas", Activo = true },
                new Categoria { IdCategoria = 2, Nombre = "Golosinas", Activo = true }
            };
            mockRepo.Setup(r => r.ObtenerTodos()).Returns(categorias);

            var service = new CategoriaService(mockRepo.Object, mockAuth.Object);

            // ACT
            var resultado = service.ObtenerTodos().ToList();

            // ASSERT
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count);
            mockRepo.Verify(r => r.ObtenerTodos(), Times.Once);
        }

        [Fact]
        public void Agregar_ConPermiso_GuardaCategoria()
        {
            // 1. ARRANGE
            var mockRepo = new Mock<ICategoriaRepository>();
            var mockAuth = new Mock<IAuthorizationService>();
            var service = new CategoriaService(mockRepo.Object, mockAuth.Object);
            var categoria = new Categoria { Nombre = "Bebidas" };

            // 2. ACT
            service.Agregar(categoria);

            // 3. ASSERT
            mockAuth.Verify(a => a.ValidarPermiso(Permisos.CATEGORIAS_GESTIONAR), Times.Once);
            mockRepo.Verify(r => r.Agregar(categoria), Times.Once);
        }

        [Fact]
        public void Actualizar_ConPermiso_EjecutaRepositorio()
        {
            // ARRANGE
            var mockRepo = new Mock<ICategoriaRepository>();
            var mockAuth = new Mock<IAuthorizationService>();
            var service = new CategoriaService(mockRepo.Object, mockAuth.Object);
            var categoria = new Categoria { IdCategoria = 3, Nombre = "Limpieza", Activo = false };

            // ACT
            service.Actualizar(categoria);

            // ASSERT
            mockAuth.Verify(a => a.ValidarPermiso(Permisos.CATEGORIAS_GESTIONAR), Times.Once);
            mockRepo.Verify(r => r.Actualizar(categoria), Times.Once);
        }

        // ==========================================
        // PRUEBAS DE FRACASO (Permisos y Seguridad)
        // ==========================================

        [Fact]
        public void Agregar_SinPermiso_LanzaUnauthorizedAccessException()
        {
            // ARRANGE
            var mockRepo = new Mock<ICategoriaRepository>();
            var mockAuth = new Mock<IAuthorizationService>();
            mockAuth.Setup(a => a.ValidarPermiso(Permisos.CATEGORIAS_GESTIONAR))
                    .Throws(new UnauthorizedAccessException("Denegado"));

            var service = new CategoriaService(mockRepo.Object, mockAuth.Object);
            var categoria = new Categoria { Nombre = "Snacks" };

            // ACT & ASSERT
            Assert.Throws<UnauthorizedAccessException>(() => service.Agregar(categoria));
            mockRepo.Verify(r => r.Agregar(It.IsAny<Categoria>()), Times.Never);
        }

        [Fact]
        public void Actualizar_SinPermiso_LanzaUnauthorizedAccessException()
        {
            // ARRANGE
            var mockRepo = new Mock<ICategoriaRepository>();
            var mockAuth = new Mock<IAuthorizationService>();
            mockAuth.Setup(a => a.ValidarPermiso(Permisos.CATEGORIAS_GESTIONAR))
                    .Throws(new UnauthorizedAccessException("Denegado"));

            var service = new CategoriaService(mockRepo.Object, mockAuth.Object);
            var categoria = new Categoria { IdCategoria = 1, Nombre = "Modificada" };

            // ACT & ASSERT
            Assert.Throws<UnauthorizedAccessException>(() => service.Actualizar(categoria));
            mockRepo.Verify(r => r.Actualizar(It.IsAny<Categoria>()), Times.Never);
        }
    }
}