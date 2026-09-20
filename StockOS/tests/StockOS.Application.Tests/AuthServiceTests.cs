using System.Threading.Tasks;
using Moq;
using Xunit;
using StockOS.Application.Services;
using StockOS.Domain.Interfaces;
using StockOS.Domain.Entities;
using BCrypt.Net;

namespace StockOS.Application.Tests
{
    public class AuthServiceTests
    {
        [Fact]
        public async Task LoginAsync_ConCredencialesValidas_RetornaExitoYUsuario()
        {
            var mockRepo = new Mock<IEmpleadoRepository>();
            var empleado = new Empleado { Dni = "11111111", PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"), Estado = true };
            mockRepo.Setup(r => r.ConsultarEstado("11111111")).Returns(true);
            mockRepo.Setup(r => r.ObtenerPorDni("11111111")).Returns(empleado);

            var service = new AuthService(mockRepo.Object);
            var resultado = await service.LoginAsync("11111111", "admin123");

            Assert.True(resultado.Exito);
        }

        [Fact]
        public async Task LoginAsync_ConContrasenaIncorrecta_RetornaFalso()
        {
            var mockRepo = new Mock<IEmpleadoRepository>();
            var empleado = new Empleado { Dni = "11111111", PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"), Estado = true };
            mockRepo.Setup(r => r.ConsultarEstado("11111111")).Returns(true);
            mockRepo.Setup(r => r.ObtenerPorDni("11111111")).Returns(empleado);

            var service = new AuthService(mockRepo.Object);

            // ACT: Le pasamos una clave mala a propósito
            var resultado = await service.LoginAsync("11111111", "claveEquivocada");

            // ASSERT
            Assert.False(resultado.Exito);
            Assert.Null(resultado.Usuario);
            Assert.Equal("Credenciales incorrectas.", resultado.MensajeError);
        }

        [Fact]
        public async Task LoginAsync_ConUsuarioInactivo_RetornaFalso()
        {
            var mockRepo = new Mock<IEmpleadoRepository>();
            var empleado = new Empleado { Dni = "22222222", Estado = false }; // Usuario dado de baja

            mockRepo.Setup(r => r.ConsultarEstado("22222222")).Returns(false);
            mockRepo.Setup(r => r.ObtenerPorDni("22222222")).Returns(empleado);

            var service = new AuthService(mockRepo.Object);
            var resultado = await service.LoginAsync("22222222", "admin123");

            Assert.False(resultado.Exito);
            Assert.Equal("Usuario deshabilitado", resultado.MensajeError);
        }
    }
}