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

            ValidarReglasDeNegocio(empleado);

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

            ValidarReglasDeNegocio(empleado);

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
        private void ValidarReglasDeNegocio(Empleado empleado)
        {
            if (empleado == null) throw new System.ArgumentNullException(nameof(empleado));

            empleado.Nombre = empleado.Nombre?.Trim() ?? "";
            empleado.Apellido = empleado.Apellido?.Trim() ?? "";
            empleado.Dni = empleado.Dni?.Trim() ?? "";
            empleado.Email = empleado.Email?.Trim() ?? "";
            empleado.Telefono = empleado.Telefono?.Trim() ?? "";
            empleado.Direccion = empleado.Direccion?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(empleado.Nombre) || empleado.Nombre.Length > 50)
                throw new System.ArgumentException("El nombre es obligatorio y no puede superar los 50 caracteres.");

            if (string.IsNullOrWhiteSpace(empleado.Apellido) || empleado.Apellido.Length > 50)
                throw new System.ArgumentException("El apellido es obligatorio y no puede superar los 50 caracteres.");

            if (string.IsNullOrWhiteSpace(empleado.Dni) || !empleado.Dni.All(char.IsDigit) || empleado.Dni.Length > 20)
                throw new System.ArgumentException("El DNI es obligatorio, debe contener solo números y no superar los 20 caracteres.");

            if (string.IsNullOrWhiteSpace(empleado.Email) || !empleado.Email.Contains("@") || empleado.Email.Length > 100)
                throw new System.ArgumentException("Debe ingresar un correo electrónico válido (máximo 100 caracteres).");

            if (string.IsNullOrWhiteSpace(empleado.Telefono) || empleado.Telefono.Length > 30)
                throw new System.ArgumentException("El teléfono es obligatorio y no puede superar los 30 caracteres.");

            if (string.IsNullOrWhiteSpace(empleado.Direccion) || empleado.Direccion.Length > 200)
                throw new System.ArgumentException("La dirección es obligatoria y no puede superar los 200 caracteres.");
        }
    }
}