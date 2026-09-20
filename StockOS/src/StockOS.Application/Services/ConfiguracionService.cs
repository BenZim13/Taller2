using Microsoft.Extensions.Configuration;
using StockOS.Domain.Interfaces;

namespace StockOS.Application.Services
{
    public class ConfiguracionService : IConfiguracionService
    {
        private readonly IConfiguration _configuration;

        public ConfiguracionService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DatosComercio ObtenerDatosComercio()
        {
            var datos = new DatosComercio();

            var section = _configuration.GetSection("DatosComercio");
            if (section.Exists())
            {
                datos.Nombre = section["Nombre"] ?? datos.Nombre;
                datos.Cuit = section["Cuit"] ?? datos.Cuit;
                datos.Direccion = section["Direccion"] ?? datos.Direccion;
                datos.IngresosBrutos = section["IngresosBrutos"] ?? datos.IngresosBrutos;
                datos.InicioActividades = section["InicioActividades"] ?? datos.InicioActividades;
                datos.CondicionIva = section["CondicionIva"] ?? datos.CondicionIva;
                datos.PuntoVenta = section["PuntoVenta"] ?? datos.PuntoVenta;
                datos.RegistroFiscal = section["RegistroFiscal"] ?? datos.RegistroFiscal;
            }

            return datos;
        }
    }
}
