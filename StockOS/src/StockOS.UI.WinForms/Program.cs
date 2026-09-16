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
                    services.AddScoped<ICategoriaRepository, CategoriaRepository>();
                    services.AddScoped<IProductoRepository, ProductoRepository>();
                    services.AddScoped<IStockSucursalRepository, StockSucursalRepository>();

                    // Servicios de Negocio
                    services.AddScoped<IAuthService, AuthService>();
                    services.AddScoped<ISucursalService, SucursalService>();
                    services.AddScoped<IRolService, RolService>();
                    services.AddScoped<IEmpleadoService, EmpleadoService>();
                    services.AddScoped<ICategoriaService, CategoriaService>();
                    services.AddScoped<ICategoriaService, CategoriaService>();
                    services.AddScoped<IProductoService, ProductoService>();
                    services.AddScoped<IStockService, StockService>();

                    // Formularios y Vistas
                    services.AddTransient<FormLogin>();
                    services.AddTransient<FormInicio>();
                    services.AddTransient<FormRegistroUsuario>();
                    services.AddTransient<UcListarUsuarios>();
                    services.AddTransient<UcUsuarios>();
                    services.AddTransient<FormRegistroProducto>();
                    services.AddTransient<FormIngresoStock>();

                    // --- ACA AGREGAMOS LAS VISTAS FALTANTES DEL MENÚ ---
                    services.AddTransient<UcInicio>();
                    services.AddTransient<UcInventario>();
                    services.AddTransient<UcVentas>();
                    services.AddTransient<UcReportes>();
                    services.AddTransient<UcConfig>();

                }).Build();
            
            // 3. Obtenemos el formulario de login desde el contenedor
            var formLogin = host.Services.GetRequiredService<FormLogin>();

            // Mostrarlo como un cuadro de diálogo (bloqueante)
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
            }
            else
            {
                // 7. Si cerró la ventana en la "X" sin loguearse, salimos
                System.Windows.Forms.Application.Exit();
            }
            /*// 2. Ejecutamos directamente el formulario de registro
            var formRegistro = host.Services.GetRequiredService<FormRegistroUsuario>();
            System.Windows.Forms.Application.Run(formRegistro);*/
        }
    }
}