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
    }
}