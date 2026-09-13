using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using StockOS.DataAccess.Persistence;
using StockOS.Domain.Interfaces;
using StockOS.DataAccess.Repositories;
using StockOS.Application.Services;
// using StockOS.Application.Services; (Aun no lo vamos a usar, ya que aun no hice eso je)

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
                    // Configurar BD con tu cadena real
                    services.AddDbContext<StockOsContext>(options =>
                        options.UseSqlServer("Server=localhost;Database=StockOS;Trusted_Connection=True;TrustServerCertificate=True;"));

                    // Repositorios y Servicios
                    services.AddScoped<IEmpleadoRepository, EmpleadoRepository>();
                    services.AddScoped<IAuthService, AuthService>();
                    services.AddScoped<ISucursalRepository, SucursalRepository>();
                    services.AddScoped<ISucursalService, SucursalService>();
                    services.AddTransient<FormRegistroUsuario>();

                    // Registrar Formularios
                    services.AddTransient<FormLogin>();
                    services.AddTransient<FormInicio>();
                }).Build();

            /*

            // 3. Obtenemo el formulario de login desde el contenedor
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
            */
            // --- PRUEBA TEMPORAL DE REGISTRO --- luego de que funque, esto se comenta o se borra.
            var formRegistro = host.Services.GetRequiredService<FormRegistroUsuario>();
            System.Windows.Forms.Application.Run(formRegistro);
        }
    }

}