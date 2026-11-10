using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using Moq;
using StockOS.Application.Services;
using StockOS.DataAccess.Services;
using StockOS.Domain.Entities;
using StockOS.Domain.Enums;
using StockOS.Domain.Interfaces;
using StockOS.UI.WinForms.Forms;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace StockOS.Application.Tests
{
    public class BackupServiceSecurityTests
    {
        private static IConfiguration CrearConfiguracionPrueba()
        {
            var settings = new Dictionary<string, string?>
            {
                { "ConnectionStrings:StockOS", "Server=localhost;Database=StockOS_Test;Trusted_Connection=True;TrustServerCertificate=True;" }
            };
            return new ConfigurationBuilder()
                .AddInMemoryCollection(settings)
                .Build();
        }

        [Theory]
        [InlineData(2)] // Cajero
        [InlineData(3)] // Encargado de Depósito
        [InlineData(4)] // Repositor
        public async Task CrearBackupAsync_ConRolNoAdministrador_LanzaUnauthorizedAccessException(int idRol)
        {
            var empleadoAnterior = SesionActual.Usuario;
            try
            {
                SesionActual.Usuario = new Empleado { IdEmpleado = 10, Nombre = "Test", Apellido = "User", IdRol = idRol };
                var authService = new AuthorizationService();
                var backupService = new BackupService(CrearConfiguracionPrueba(), authService);

                await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                    backupService.CrearBackupAsync(null, PeriodoBackup.Diario));
            }
            finally
            {
                SesionActual.Usuario = empleadoAnterior;
            }
        }

        [Theory]
        [InlineData(2)] // Cajero
        [InlineData(3)] // Encargado de Depósito
        [InlineData(4)] // Repositor
        public async Task RestaurarBackupAsync_ConRolNoAdministrador_LanzaUnauthorizedAccessException(int idRol)
        {
            var empleadoAnterior = SesionActual.Usuario;
            try
            {
                SesionActual.Usuario = new Empleado { IdEmpleado = 10, Nombre = "Test", Apellido = "User", IdRol = idRol };
                var authService = new AuthorizationService();
                var backupService = new BackupService(CrearConfiguracionPrueba(), authService);

                await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                    backupService.RestaurarBackupAsync(@"C:\Backups\test.bak"));
            }
            finally
            {
                SesionActual.Usuario = empleadoAnterior;
            }
        }

        [Theory]
        [InlineData(2)] // Cajero
        [InlineData(3)] // Encargado de Depósito
        [InlineData(4)] // Repositor
        public async Task GuardarProgramacionPeriodicaAsync_ConRolNoAdministrador_LanzaUnauthorizedAccessException(int idRol)
        {
            var empleadoAnterior = SesionActual.Usuario;
            try
            {
                SesionActual.Usuario = new Empleado { IdEmpleado = 10, Nombre = "Test", Apellido = "User", IdRol = idRol };
                var authService = new AuthorizationService();
                var backupService = new BackupService(CrearConfiguracionPrueba(), authService);

                var config = new ConfiguracionBackupPeriodico
                {
                    Habilitado = true,
                    Periodo = PeriodoBackup.Semanal,
                    HoraEjecucion = "22:00"
                };

                await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                    backupService.GuardarProgramacionPeriodicaAsync(config));
            }
            finally
            {
                SesionActual.Usuario = empleadoAnterior;
            }
        }

        [Fact]
        public async Task GuardarProgramacionPeriodicaAsync_ConRolAdministrador_PermiteGuardarProgramacion()
        {
            var empleadoAnterior = SesionActual.Usuario;
            try
            {
                SesionActual.Usuario = new Empleado { IdEmpleado = 1, Nombre = "Admin", Apellido = "Root", IdRol = 1 };
                var authService = new AuthorizationService();
                var backupService = new BackupService(CrearConfiguracionPrueba(), authService);

                var config = new ConfiguracionBackupPeriodico
                {
                    Habilitado = false,
                    Periodo = PeriodoBackup.Diario,
                    HoraEjecucion = "23:00",
                    RutaDestino = System.IO.Path.GetTempPath()
                };

                var resultado = await backupService.GuardarProgramacionPeriodicaAsync(config);
                Assert.True(resultado.Exito);
            }
            finally
            {
                SesionActual.Usuario = empleadoAnterior;
            }
        }

        [Fact]
        public void UcConfig_ClickBotonBackupConCajero_BloqueaAccesoYMuestraAdvertencia()
        {
            var empleadoAnterior = SesionActual.Usuario;
            var handlerAnterior = UcConfig.MostrarMensaje;
            try
            {
                SesionActual.Usuario = new Empleado { IdEmpleado = 2, Nombre = "Cajero", Apellido = "Uno", IdRol = 2 };
                var authService = new AuthorizationService();
                var mockBackup = new Mock<IBackupService>();

                string? mensajeCapturado = null;
                UcConfig.MostrarMensaje = (msg, title, btns, icon) => { mensajeCapturado = msg; };

                using var ucConfig = new UcConfig(mockBackup.Object, authService);
                var btnAdminMethod = typeof(UcConfig).GetMethod("BtnAdmin_Click", BindingFlags.NonPublic | BindingFlags.Instance)!;
                btnAdminMethod.Invoke(ucConfig, new object?[] { null, EventArgs.Empty });

                Assert.NotNull(mensajeCapturado);
                Assert.Contains("Administrador", mensajeCapturado);
            }
            finally
            {
                UcConfig.MostrarMensaje = handlerAnterior;
                SesionActual.Usuario = empleadoAnterior;
            }
        }

        [Fact]
        public void CalcularProximaEjecucion_SegunPeriodo_CalculaFechasCorrectamente()
        {
            var baseDate = new DateTime(2026, 11, 9, 10, 0, 0);

            Assert.Equal(new DateTime(2026, 11, 10, 23, 0, 0), BackupService.CalcularProximaEjecucion(PeriodoBackup.Diario, "23:00", baseDate));
            Assert.Equal(new DateTime(2026, 11, 16, 23, 0, 0), BackupService.CalcularProximaEjecucion(PeriodoBackup.Semanal, "23:00", baseDate));
            Assert.Equal(new DateTime(2026, 12, 9, 23, 0, 0), BackupService.CalcularProximaEjecucion(PeriodoBackup.Mensual, "23:00", baseDate));
            Assert.Equal(new DateTime(2027, 11, 9, 23, 0, 0), BackupService.CalcularProximaEjecucion(PeriodoBackup.Anual, "23:00", baseDate));
        }
    }
}
