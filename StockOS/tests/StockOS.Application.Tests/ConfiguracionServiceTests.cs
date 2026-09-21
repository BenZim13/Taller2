using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Xunit;
using StockOS.Application.Services;
using StockOS.Domain.Entities;

namespace StockOS.Application.Tests
{
    public class ConfiguracionServiceTests
    {
        // ==========================================
        // PRUEBAS DE ÉXITO (Lectura de Configuración)
        // ==========================================

        [Fact]
        public void ObtenerDatosComercio_ConSeccionConfigurada_RetornaDatosCorrectos()
        {
            // ARRANGE
            var memoriaConfig = new Dictionary<string, string?>
            {
                { "DatosComercio:Nombre", "StockOS Supermercado" },
                { "DatosComercio:Cuit", "30-99887766-5" },
                { "DatosComercio:Direccion", "Calle Comercial 456" },
                { "DatosComercio:IngresosBrutos", "901-123456-7" },
                { "DatosComercio:InicioActividades", "15/05/2020" },
                { "DatosComercio:CondicionIva", "IVA RESPONSABLE INSCRIPTO" },
                { "DatosComercio:PuntoVenta", "00002" },
                { "DatosComercio:RegistroFiscal", "EPEPAA999999" }
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(memoriaConfig)
                .Build();

            var service = new ConfiguracionService(configuration);

            // ACT
            var datos = service.ObtenerDatosComercio();

            // ASSERT
            Assert.NotNull(datos);
            Assert.Equal("StockOS Supermercado", datos.Nombre);
            Assert.Equal("30-99887766-5", datos.Cuit);
            Assert.Equal("Calle Comercial 456", datos.Direccion);
            Assert.Equal("00002", datos.PuntoVenta);
            Assert.Equal("EPEPAA999999", datos.RegistroFiscal);
        }

        // ==========================================
        // PRUEBAS DE RESILIENCIA (Sección Faltante)
        // ==========================================

        [Fact]
        public void ObtenerDatosComercio_SinSeccionEnConfiguracion_RetornaInstanciaPorDefecto()
        {
            // ARRANGE
            var memoriaConfig = new Dictionary<string, string?>();
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(memoriaConfig)
                .Build();

            var service = new ConfiguracionService(configuration);

            // ACT
            var datos = service.ObtenerDatosComercio();

            // ASSERT
            Assert.NotNull(datos);
            Assert.Equal("StockOS", datos.Nombre);
            Assert.Equal("00-00000000-0", datos.Cuit);
        }
    }
}
