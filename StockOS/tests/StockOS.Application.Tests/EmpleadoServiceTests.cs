using System;
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
        // ==========================================
        // PRUEBAS DE ÉXITO
        // ==========================================

        [Fact]
        public async Task CrearAsync_ConDatosValidos_RetornaTrue()
        {
            // ARRANGE
            var mockRepo = new Mock<IEmpleadoRepository>();
            var mockAuth = new Mock<IAuthorizationService>();

            mockRepo.Setup(r => r.ObtenerPorDni(It.IsAny<string>())).Returns((Empleado?)null);
            mockRepo.Setup(r => r.ObtenerPorEmail(It.IsAny<string>())).Returns((Empleado?)null);

            var service = new EmpleadoService(mockRepo.Object, mockAuth.Object);
            var nuevoEmpleado = new Empleado { Dni = "40111222", Email = "valido@mail.com" };

            // ACT
            bool resultado = await service.CrearAsync(nuevoEmpleado);

            // ASSERT
            Assert.True(resultado);
            mockAuth.Verify(a => a.ValidarPermiso(Permisos.USUARIOS_CREAR), Times.Once);
            mockRepo.Verify(r => r.Agregar(nuevoEmpleado), Times.Once);
        }

        [Fact]
        public async Task ActualizarAsync_ConMismoDniDeMismoEmpleado_RetornaExito()
        {
            // ARRANGE
            var mockRepo = new Mock<IEmpleadoRepository>();
            var mockAuth = new Mock<IAuthorizationService>();

            var empEditado = new Empleado { IdEmpleado = 5, Dni = "33444555", Email = "editado@mail.com" };

            // El DNI encontrado es el del MISMO empleado
            mockRepo.Setup(r => r.ObtenerPorDni("33444555"))
                    .Returns(new Empleado { IdEmpleado = 5, Dni = "33444555" });
            mockRepo.Setup(r => r.ObtenerPorEmail(It.IsAny<string>())).Returns((Empleado?)null);

            var service = new EmpleadoService(mockRepo.Object, mockAuth.Object);

            // ACT
            var (exito, mensaje) = await service.ActualizarAsync(empEditado);

            // ASSERT
            Assert.True(exito);
            Assert.Contains("correctamente", mensaje);
            mockRepo.Verify(r => r.Actualizar(empEditado), Times.Once);
        }

        [Fact]
        public async Task CambiarEstadoAsync_EmpleadoExistente_RetornaTrue()
        {
            // ARRANGE
            var mockRepo = new Mock<IEmpleadoRepository>();
            var mockAuth = new Mock<IAuthorizationService>();

            var emp = new Empleado { IdEmpleado = 1, Estado = true };
            mockRepo.Setup(r => r.ObtenerPorId(1)).Returns(emp);

            var service = new EmpleadoService(mockRepo.Object, mockAuth.Object);

            // ACT
            bool resultado = await service.CambiarEstadoAsync(1, false);

            // ASSERT
            Assert.True(resultado);
            Assert.False(emp.Estado);
            mockAuth.Verify(a => a.ValidarPermiso(Permisos.USUARIOS_EDITAR), Times.Once);
            mockRepo.Verify(r => r.Actualizar(emp), Times.Once);
        }

        // ==========================================
        // PRUEBAS DE FRACASO
        // ==========================================

        [Fact]
        public async Task CrearAsync_ConDniDuplicado_RetornaFalse()
        {
            // ARRANGE
            var mockRepo = new Mock<IEmpleadoRepository>();
            var mockAuth = new Mock<IAuthorizationService>();

            mockRepo.Setup(r => r.ObtenerPorDni("35000111"))
                    .Returns(new Empleado { IdEmpleado = 1, Dni = "35000111" });

            var service = new EmpleadoService(mockRepo.Object, mockAuth.Object);
            var nuevoEmpleado = new Empleado { Dni = "35000111", Email = "nuevo@mail.com" };

            // ACT
            bool resultado = await service.CrearAsync(nuevoEmpleado);

            // ASSERT
            Assert.False(resultado);
            mockRepo.Verify(r => r.Agregar(It.IsAny<Empleado>()), Times.Never);
        }

        [Fact]
        public async Task CrearAsync_ConEmailDuplicado_RetornaFalse()
        {
            // ARRANGE
            var mockRepo = new Mock<IEmpleadoRepository>();
            var mockAuth = new Mock<IAuthorizationService>();

            mockRepo.Setup(r => r.ObtenerPorDni("40111222")).Returns((Empleado?)null);
            mockRepo.Setup(r => r.ObtenerPorEmail("duplicado@mail.com"))
                    .Returns(new Empleado { IdEmpleado = 2, Email = "duplicado@mail.com" });

            var service = new EmpleadoService(mockRepo.Object, mockAuth.Object);
            var nuevoEmpleado = new Empleado { Dni = "40111222", Email = "duplicado@mail.com" };

            // ACT
            bool resultado = await service.CrearAsync(nuevoEmpleado);

            // ASSERT
            Assert.False(resultado);
            mockRepo.Verify(r => r.Agregar(It.IsAny<Empleado>()), Times.Never);
        }

        [Fact]
        public async Task CrearAsync_SinPermiso_LanzaUnauthorizedAccessException()
        {
            // ARRANGE
            var mockRepo = new Mock<IEmpleadoRepository>();
            var mockAuth = new Mock<IAuthorizationService>();

            mockAuth.Setup(a => a.ValidarPermiso(Permisos.USUARIOS_CREAR))
                    .Throws(new UnauthorizedAccessException("Denegado"));

            var service = new EmpleadoService(mockRepo.Object, mockAuth.Object);
            var nuevoEmpleado = new Empleado { Dni = "40111222", Email = "test@mail.com" };

            // ACT & ASSERT
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => service.CrearAsync(nuevoEmpleado));
            mockRepo.Verify(r => r.Agregar(It.IsAny<Empleado>()), Times.Never);
        }

        [Fact]
        public async Task ActualizarAsync_ConDniDeOtroEmpleado_RetornaFalse()
        {
            // ARRANGE
            var mockRepo = new Mock<IEmpleadoRepository>();
            var mockAuth = new Mock<IAuthorizationService>();

            var empEditado = new Empleado { IdEmpleado = 5, Dni = "20111222", Email = "editado@mail.com" };

            // DNI le pertenece al IdEmpleado = 9
            mockRepo.Setup(r => r.ObtenerPorDni("20111222"))
                    .Returns(new Empleado { IdEmpleado = 9, Dni = "20111222" });

            var service = new EmpleadoService(mockRepo.Object, mockAuth.Object);

            // ACT
            var (exito, mensaje) = await service.ActualizarAsync(empEditado);

            // ASSERT
            Assert.False(exito);
            Assert.Contains("DNI ya se encuentra registrado", mensaje);
            mockRepo.Verify(r => r.Actualizar(It.IsAny<Empleado>()), Times.Never);
        }

        [Fact]
        public async Task CambiarEstadoAsync_EmpleadoInexistente_RetornaFalse()
        {
            // ARRANGE
            var mockRepo = new Mock<IEmpleadoRepository>();
            var mockAuth = new Mock<IAuthorizationService>();

            mockRepo.Setup(r => r.ObtenerPorId(999)).Returns((Empleado?)null);

            var service = new EmpleadoService(mockRepo.Object, mockAuth.Object);

            // ACT
            bool resultado = await service.CambiarEstadoAsync(999, false);

            // ASSERT
            Assert.False(resultado);
            mockRepo.Verify(r => r.Actualizar(It.IsAny<Empleado>()), Times.Never);
        }
    
}
}
