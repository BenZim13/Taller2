using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using StockOS.DataAccess.Persistence;
using StockOS.Domain.Interfaces;
using StockOS.DataAccess.Repositories;
using StockOS.Application.Services;
using StockOS.Application.Reports;

using System;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using System.IO;
using Serilog;

namespace StockOS.UI.WinForms.Forms
{
    /// <summary>
    /// Punto de entrada de la aplicación con configuración de inyección de dependencias,
    /// logging y manejo global de excepciones.
    /// </summary>
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // Inicializar configuración visual de WinForms
            ApplicationConfiguration.Initialize();
        
            // Configurar QuestPDF para generación de reportes y tickets
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
            QuestPDF.Settings.UseSystemFonts = true;

            // Configurar sistema de logging con Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File("logs/stockos-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            // Configurar manejo global de excepciones no capturadas
            System.Windows.Forms.Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            System.Windows.Forms.Application.ThreadException += (sender, args) =>
            {
                Log.Error(args.Exception, "Error no controlado en evento de interfaz (ThreadException)");
                MessageBox.Show($"Ocurrió un error en la aplicación: {args.Exception.Message}", "Error Inesperado", MessageBoxButtons.OK, MessageBoxIcon.Error);
            };
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                if (args.ExceptionObject is Exception ex)
                {
                    Log.Fatal(ex, "Error fatal no controlado en AppDomain");
                }
            };

            try
            {
                Log.Information("Iniciando la aplicación StockOS...");

                // Cargar configuración desde appsettings.json
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                // Configurar contenedor de inyección de dependencias
                var host = Host.CreateDefaultBuilder()
                    .UseSerilog()
                    .ConfigureServices((context, services) =>
                    {
                        // Configuración global
                        services.AddSingleton<IConfiguration>(configuration);

                        // Contexto de base de datos
                        services.AddDbContext<StockOsContext>(options =>
                            options.UseSqlServer(configuration.GetConnectionString("StockOS")));

                        // Repositorios
                        services.AddScoped<IEmpleadoRepository, EmpleadoRepository>();
                        services.AddScoped<ISucursalRepository, SucursalRepository>();
                        services.AddScoped<IRolRepository, RolRepository>();
                        services.AddScoped<ICategoriaRepository, CategoriaRepository>();
                        services.AddScoped<IProductoRepository, ProductoRepository>();
                        services.AddScoped<IStockSucursalRepository, StockSucursalRepository>();
                        services.AddScoped<ICajaSesionRepository, CajaSesionRepository>();
                        services.AddScoped<ICajaRepository, CajaRepository>();
                        services.AddScoped<IVentaRepository, VentaRepository>();
                        services.AddScoped<ICompraRepository, CompraRepository>();
                        services.AddScoped<IProveedorRepository, ProveedorRepository>();
                        services.AddScoped<IReporteRepository, ReporteRepository>();

                        // Servicios de Negocio
                        services.AddScoped<IAuthService, AuthService>();
                        services.AddScoped<ISucursalService, SucursalService>();
                        services.AddScoped<IRolService, RolService>();
                        services.AddScoped<IEmpleadoService, EmpleadoService>();
                        services.AddScoped<ICategoriaService, CategoriaService>();
                        services.AddScoped<IProductoService, ProductoService>();
                        services.AddScoped<IStockService, StockService>();
                        services.AddScoped<ICajaService, CajaService>();
                        services.AddScoped<IVentaService, VentaService>();
                        services.AddScoped<ICompraService, CompraService>();
                        services.AddScoped<IProveedorService, ProveedorService>();
                        services.AddScoped<IReporteService, ReporteService>();
                        services.AddScoped<IAuthorizationService, AuthorizationService>();
                        services.AddScoped<ITicketService, TicketService>();
                        services.AddScoped<IConfiguracionService, ConfiguracionService>();

                        // Formularios y Vistas
                        services.AddTransient<FormLogin>();
                        services.AddTransient<FormInicio>();
                        services.AddTransient<FormRegistroUsuario>();
                        services.AddTransient<UcListarUsuarios>();
                        services.AddTransient<UcUsuarios>();
                        services.AddTransient<FormRegistroProducto>();
                        services.AddTransient<FormIngresoStock>();
                        services.AddTransient<FormCategoria>();
                        services.AddTransient<FormProveedor>();
                        services.AddTransient<FormAperturaCaja>();

                        // Controles de usuario (User Controls)
                        services.AddTransient<UcInicio>();
                        services.AddTransient<UcInventario>();
                        services.AddTransient<UcVentas>();
                        services.AddTransient<UcReportes>();
                        services.AddTransient<UcConfig>();

                    }).Build();

                // Bucle principal de autenticación y sesión
                // Cada iteración crea un nuevo scope para evitar fugas de memoria del DbContext
                while (true)
                {
                    using (var scope = host.Services.CreateScope())
                    {
                        var formLogin = scope.ServiceProvider.GetRequiredService<FormLogin>();

                        if (formLogin.ShowDialog() == DialogResult.OK)
                        {
                            var usuario = formLogin.UsuarioAutenticado;
                            var formInicio = scope.ServiceProvider.GetRequiredService<FormInicio>();

                            if (usuario != null)
                            {
                                formInicio.EstablecerUsuario(usuario);
                                Log.Information("Usuario autenticado correctamente: {Nombre} {Apellido} (Rol: {IdRol})", usuario.Nombre, usuario.Apellido, usuario.IdRol);
                            }

                            System.Windows.Forms.Application.Run(formInicio);

                            // Intentar cerrar automáticamente la caja al salir
                            if (usuario != null)
                            {
                                try
                                {
                                    var cajaService = scope.ServiceProvider.GetRequiredService<StockOS.Application.Services.ICajaService>();
                                    cajaService.CerrarCajaPorCierreSesion(usuario.IdEmpleado);
                                }
                                catch (Exception exCaja)
                                {
                                    Log.Warning(exCaja, "No se pudo cerrar automáticamente la caja al finalizar la sesión del usuario {IdEmpleado}.", usuario.IdEmpleado);
                                }
                            }

                            StockOS.Application.Services.SesionActual.Limpiar();

                            if (formInicio.LogoutRequested)
                            {
                                Log.Information("Cierre de sesión solicitado por el usuario.");
                                continue;
                            }
                            else
                            {
                                Log.Information("Cerrando la aplicación desde la ventana principal.");
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Capturar errores críticos que impidan el inicio de la aplicación
                Log.Fatal(ex, "La aplicación sufrió un error fatal y se cerró inesperadamente.");
                MessageBox.Show("Ocurrió un error crítico. Revise el archivo de registro (logs) para más detalles.", "Error Fatal", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Asegurar que todos los logs se escriban en disco antes de cerrar
                Log.CloseAndFlush();
            }
        }
    }
}