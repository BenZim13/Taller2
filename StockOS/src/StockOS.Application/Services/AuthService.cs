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

            // Aca le validamos si esta o no el gente cuera.
            // BCrypt.Verify toma la contraseña plana (password) y la compara matemáticamente con el Hash guardado.
            if (empleado != null && BCrypt.Net.BCrypt.Verify(password, empleado.PasswordHash))
            {
                return empleado;
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