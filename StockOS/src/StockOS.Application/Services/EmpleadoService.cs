using System.Collections.Generic;
using System.Threading.Tasks;
using StockOS.Domain.Entities;
using StockOS.Domain.Interfaces;
using StockOS.Domain.Enums; // Mapeo de permisos

namespace StockOS.Application.Services
{
    public class EmpleadoService : IEmpleadoService
    {
        private readonly IEmpleadoRepository _empleadoRepository;
        private readonly IAuthorizationService _authService; // Guardián

        public EmpleadoService(IEmpleadoRepository empleadoRepository, IAuthorizationService authService)
        {
            _empleadoRepository = empleadoRepository;
            _authService = authService;
        }

        public IEnumerable<Empleado> ObtenerTodos()
        {
            _authService.ValidarPermiso(Permisos.USUARIOS_VER);
            return _empleadoRepository.ObtenerTodos();
        }

        public Empleado? ObtenerPorId(int id)
        {
            _authService.ValidarPermiso(Permisos.USUARIOS_VER);
            return _empleadoRepository.ObtenerPorId(id);
        }

        public async Task<bool> CrearAsync(Empleado empleado)
        {
            _authService.ValidarPermiso(Permisos.USUARIOS_CREAR);

            if (_empleadoRepository.ObtenerPorDni(empleado.Dni) != null) return false;
            if (_empleadoRepository.ObtenerPorEmail(empleado.Email) != null) return false;

            if (!string.IsNullOrWhiteSpace(empleado.PasswordHash) && !empleado.PasswordHash.StartsWith("$2"))
            {
                empleado.PasswordHash = BCrypt.Net.BCrypt.HashPassword(empleado.PasswordHash);
            }

            _empleadoRepository.Agregar(empleado);
            return true;
        }

        public async Task<(bool Exito, string Mensaje)> ActualizarAsync(Empleado empleado)
        {
            _authService.ValidarPermiso(Permisos.USUARIOS_EDITAR);

            var empConMismoDni = _empleadoRepository.ObtenerPorDni(empleado.Dni);
            if (empConMismoDni != null && empConMismoDni.IdEmpleado != empleado.IdEmpleado)
            {
                return (false, "El DNI ya se encuentra registrado por otro empleado.");
            }

            var empConMismoEmail = _empleadoRepository.ObtenerPorEmail(empleado.Email);
            if (empConMismoEmail != null && empConMismoEmail.IdEmpleado != empleado.IdEmpleado)
            {
                return (false, "El correo electrónico ya se encuentra registrado por otro empleado.");
            }

            if (!string.IsNullOrWhiteSpace(empleado.PasswordHash) && !empleado.PasswordHash.StartsWith("$2"))
            {
                empleado.PasswordHash = BCrypt.Net.BCrypt.HashPassword(empleado.PasswordHash);
            }

            _empleadoRepository.Actualizar(empleado);
            return (true, "Empleado actualizado correctamente.");
        }

        public async Task<bool> EliminarAsync(int id)
        {
            _authService.ValidarPermiso(Permisos.USUARIOS_EDITAR);
            _empleadoRepository.Eliminar(id);
            return true;
        }

        public async Task<bool> CambiarEstadoAsync(int id, bool nuevoEstado)
        {
            _authService.ValidarPermiso(Permisos.USUARIOS_EDITAR);

            var emp = _empleadoRepository.ObtenerPorId(id);
            if (emp == null) return false;

            emp.Estado = nuevoEstado;
            _empleadoRepository.Actualizar(emp);
            return true;
        }
    }
}