using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using StockOS.DataAccess.Persistence;
using StockOS.Domain.Interfaces;
using StockOS.DataAccess.Repositories;
using StockOS.Application.Services;
using System;
using System.Windows.Forms;

namespace StockOS.UI.WinForms.Forms
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // 1. Configuración visual de WinForms (SIEMPRE VA PRIMERO)
            ApplicationConfiguration.Initialize();

            // 2. Configurar Inyección de Dependencias
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    // Configurar BD con la cadena de conexión
                    services.AddDbContext<StockOsContext>(options =>
                        options.UseSqlServer("Server=localhost;Database=StockOS;Trusted_Connection=True;TrustServerCertificate=True;"));

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

                    // Formularios y Vistas
                    services.AddTransient<FormLogin>();
                    services.AddTransient<FormInicio>();
                    services.AddTransient<FormRegistroUsuario>();
                    services.AddTransient<UcListarUsuarios>();
                    services.AddTransient<UcUsuarios>();
                    services.AddTransient<FormRegistroProducto>();
                    services.AddTransient<FormIngresoStock>();
                    services.AddTransient<FormCategoria>();
                    services.AddTransient<FormAperturaCaja>();
                    

                    // --- ACA AGREGAMOS LAS VISTAS FALTANTES DEL MENÚ ---
                    services.AddTransient<UcInicio>();
                    services.AddTransient<UcInventario>();
                    services.AddTransient<UcVentas>();
                    services.AddTransient<UcReportes>();
                    services.AddTransient<UcConfig>();

                }).Build();
            
            // 3. Bucle principal para soportar cierre de sesión y cambio de usuario
            while (true)
            {
                var formLogin = host.Services.GetRequiredService<FormLogin>();

                // Mostrar el login como un cuadro de diálogo (bloqueante)
                if (formLogin.ShowDialog() == DialogResult.OK)
                {
                    // 4. Capturamos el usuario logueado
                    var usuario = formLogin.UsuarioAutenticado;
                    var formInicio = host.Services.GetRequiredService<FormInicio>();

                    // 5. Le pasamos el empleado al menú principal
                    if (usuario != null)
                    {
                        formInicio.EstablecerUsuario(usuario);
                    }

                    // 6. Iniciamos el ciclo de vida de la app con el formulario principal
                    System.Windows.Forms.Application.Run(formInicio);

                    // 7. Al cerrarse el formulario principal, verificamos si fue por cierre de sesión
                    if (formInicio.LogoutRequested)
                    {
                        // Limpiamos la sesión global
                        StockOS.Application.Services.SesionActual.Usuario = null;
                        continue; // Volvemos a mostrar el login
                    }
                    else
                    {
                        // Se cerró desde la 'X', por lo tanto salimos de la aplicación
                        break;
                    }
                }
                else
                {
                    // 8. Si cerró la ventana de login (X o botón Salir), salimos de la aplicación
                    break;
                }
            }
        }
    }
}