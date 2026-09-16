using System.Threading.Tasks;
using StockOS.Domain.Entities;
using StockOS.Domain.Interfaces;
// Importamos la librería de BCrypt que acabamos de instalar
using BCrypt.Net;

namespace StockOS.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IEmpleadoRepository _empleadoRepository;

        public AuthService(IEmpleadoRepository empleadoRepository)
        {
            _empleadoRepository = empleadoRepository;
        }

        public async Task<Empleado?> LoginAsync(string dni, string password)
        {
            var empleado = _empleadoRepository.ObtenerPorDni(dni);

            // Validamos si el empleado existe
            if (empleado != null)
            {
                bool isValid = false;
                try
                {
                    // Intentar verificar asumiendo que es un hash de BCrypt
                    isValid = BCrypt.Net.BCrypt.Verify(password, empleado.PasswordHash);
                }
                catch (SaltParseException)
                {
                    // Si falla porque el formato no es de BCrypt (ej. texto plano),
                    // verificamos si coincide exactamente con lo guardado en la BD
                    if (empleado.PasswordHash == password)
                    {
                        isValid = true;
                        
                        // Actualizamos la contraseña al nuevo formato hash en la BD para la próxima vez
                        empleado.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
                        _empleadoRepository.Actualizar(empleado);
                    }
                }

                if (isValid)
                {
                    return empleado;
                }
            }
            return null;
        }

        public async Task<bool> RegistrarAsync(Empleado empleado)
        {
            // Evitar duplicados por DNI o Email
            if (_empleadoRepository.ObtenerPorDni(empleado.Dni) != null) return false;
            if (_empleadoRepository.ObtenerPorEmail(empleado.Email) != null) return false;

            // Encriptamos la contraseña plana que viene del formulario antes de enviarla al repositorio
            empleado.PasswordHash = BCrypt.Net.BCrypt.HashPassword(empleado.PasswordHash);

            _empleadoRepository.Agregar(empleado);
            return true;
        }
    }
}