using System.Threading.Tasks;
using StockOS.Domain.Entities;
using StockOS.Domain.Interfaces;
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

        public async Task<(bool Exito, Empleado? Usuario, string? MensajeError)> LoginAsync(string dni, string password)
        {
            // Verificar primero si el usuario está activo en la base de datos
            bool? estadoDb = _empleadoRepository.ConsultarEstado(dni);
            if (estadoDb.HasValue && !estadoDb.Value)
            {
                return (false, null, "Usuario deshabilitado");
            }

            // Buscar empleado por DNI
            var empleado = _empleadoRepository.ObtenerPorDni(dni);

            // Si no existe, credenciales incorrectas
            if (empleado == null)
            {
                return (false, null, "Credenciales incorrectas.");
            }

            // Doble verificación de estado activo
            if (!empleado.Estado)
            {
                return (false, null, "Usuario deshabilitado");
            }

            // Validar contraseña con BCrypt
            bool isValid = false;
            try
            {
                // Intentar verificar con BCrypt (contraseñas hasheadas)
                isValid = BCrypt.Net.BCrypt.Verify(password, empleado.PasswordHash);
            }
            catch (SaltParseException)
            {
                // Retrocompatibilidad: si la contraseña está en texto plano, verificarla directamente
                // y migrarla automáticamente a BCrypt
                if (empleado.PasswordHash == password)
                {
                    isValid = true;

                    // Migración automática: hashear la contraseña para el próximo login
                    empleado.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
                    _empleadoRepository.Actualizar(empleado);
                }
            }

            if (isValid)
            {
                return (true, empleado, null);
            }

            return (false, null, "Credenciales incorrectas.");
        }

        public async Task<bool> RegistrarAsync(Empleado empleado)
        {
            // Validar que no exista otro empleado con el mismo DNI o email
            if (_empleadoRepository.ObtenerPorDni(empleado.Dni) != null) return false;
            if (_empleadoRepository.ObtenerPorEmail(empleado.Email) != null) return false;

            // Hashear la contraseña antes de guardarla en la base de datos
            empleado.PasswordHash = BCrypt.Net.BCrypt.HashPassword(empleado.PasswordHash);

            _empleadoRepository.Agregar(empleado);
            return true;
        }
    }
}