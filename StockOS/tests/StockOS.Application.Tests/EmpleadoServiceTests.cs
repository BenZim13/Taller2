using System.Threading.Tasks;
using Moq;
using Xunit;
using StockOS.Application.Services;
using StockOS.Domain.Interfaces;
using StockOS.Domain.Entities;
using StockOS.Domain.Enums;

namespace StockOS.Application.Tests
{
    public class EmpleadoServiceTests
    {
        [Fact]
        public async Task CrearAsync_ConDniDuplicado_RetornaFalse()
        {
            // 1. ARRANGE
            var mockRepo = new Mock<IEmpleadoRepository>();
            var mockAuth = new Mock<IAuthorizationService>();

            // Simulamos que el DNI ya existe en la base
            mockRepo.Setup(r => r.ObtenerPorDni("35000111"))
                    .Returns(new Empleado { IdEmpleado = 1, Dni = "35000111" });

            var service = new EmpleadoService(mockRepo.Object, mockAuth.Object);
            var nuevoEmpleado = new Empleado { Dni = "35000111", Email = "nuevo@mail.com" };

            // 2. ACT
            bool resultado = await service.CrearAsync(nuevoEmpleado);

            // 3. ASSERT
            Assert.False(resultado);
            // Verificamos que NUNCA se haya llamado al método Agregar porque falló antes
            mockRepo.Verify(r => r.Agregar(It.IsAny<Empleado>()), Times.Never);
        }

        [Fact]
        public async Task CrearAsync_ConDatosValidos_RetornaTrue()
        {
            // 1. ARRANGE
            var mockRepo = new Mock<IEmpleadoRepository>();
            var mockAuth = new Mock<IAuthorizationService>();

            // Simulamos que ni el DNI ni el Email existen
            mockRepo.Setup(r => r.ObtenerPorDni(It.IsAny<string>())).Returns((Empleado?)null);
            mockRepo.Setup(r => r.ObtenerPorEmail(It.IsAny<string>())).Returns((Empleado?)null);

            var service = new EmpleadoService(mockRepo.Object, mockAuth.Object);
            var nuevoEmpleado = new Empleado { Dni = "40111222", Email = "valido@mail.com" };

            // 2. ACT
            bool resultado = await service.CrearAsync(nuevoEmpleado);

            // 3. ASSERT
            Assert.True(resultado);
            mockRepo.Verify(r => r.Agregar(nuevoEmpleado), Times.Once);
        }
    }
}