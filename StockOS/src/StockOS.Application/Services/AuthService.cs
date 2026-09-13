using System.Threading.Tasks;
using StockOS.Domain.Entities;
using StockOS.Domain.Interfaces;

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

            // Validamos que exista y que la contraseña coincida (Luego implementaremos el Hash)
            if (empleado != null && empleado.PasswordHash == password)
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

            // Por ahora guardamos la contraseña plana (luego le hacemos que sea Hash)
            _empleadoRepository.Agregar(empleado);
            return true;
        }
    }
}