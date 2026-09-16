using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using StockOS.Domain.Entities;
using StockOS.Domain.Interfaces;
using StockOS.DataAccess.Persistence;
using System.Data;

namespace StockOS.DataAccess.Repositories
{
    public class EmpleadoRepository : IEmpleadoRepository
    {
        private readonly StockOsContext _context;

        public EmpleadoRepository(StockOsContext context)
        {
            _context = context;
        }

        public Empleado? ObtenerPorEmail(string email)
        {
            return _context.Empleados
                .Include(e => e.IdRolNavigation)
                .Include(e => e.IdSucursalNavigation)
                .FirstOrDefault(e => e.Email == email);
        }

        public Empleado? ObtenerPorDni(string dni)
        {
            // [Procedimiento 3: sp_Usuarios_Autenticar]
            var empleado = _context.Empleados
                .FromSqlRaw("EXEC sp_Usuarios_Autenticar @Dni={0}", dni)
                .AsEnumerable()
                .FirstOrDefault();

            if (empleado != null)
            {
                _context.Entry(empleado).Reference(e => e.IdRolNavigation).Load();
                _context.Entry(empleado).Reference(e => e.IdSucursalNavigation).Load();
            }

            return empleado;
        }

        public Empleado? ObtenerPorId(int id)
        {
            return _context.Empleados
                .Include(e => e.IdRolNavigation)
                .Include(e => e.IdSucursalNavigation)
                .FirstOrDefault(e => e.IdEmpleado == id);
        }

        public IEnumerable<Empleado> ObtenerTodos()
        {
            return _context.Empleados
                .Include(e => e.IdRolNavigation)
                .Include(e => e.IdSucursalNavigation)
                .ToList();
        }

        public void Agregar(Empleado empleado)
        {
            // [Procedimiento 1: sp_Usuarios_Insertar]
            var idParam = new SqlParameter
            {
                ParameterName = "@IdEmpleado",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };

            _context.Database.ExecuteSqlRaw(
                "EXEC sp_Usuarios_Insertar @Nombre, @Apellido, @Dni, @Email, @Telefono, @PasswordHash, @IdRol, @IdSucursal, @IdEmpleado OUTPUT",
                new SqlParameter("@Nombre", empleado.Nombre),
                new SqlParameter("@Apellido", empleado.Apellido),
                new SqlParameter("@Dni", empleado.Dni),
                new SqlParameter("@Email", empleado.Email),
                new SqlParameter("@Telefono", empleado.Telefono ?? (object)DBNull.Value),
                new SqlParameter("@PasswordHash", empleado.PasswordHash),
                new SqlParameter("@IdRol", empleado.IdRol),
                new SqlParameter("@IdSucursal", empleado.IdSucursal),
                idParam);

            empleado.IdEmpleado = (int)idParam.Value;
        }

        public void Actualizar(Empleado empleado)
        {
            // [Procedimiento 4: sp_Usuarios_Actualizar]
            _context.Database.ExecuteSqlRaw(
                "EXEC sp_Usuarios_Actualizar @IdEmpleado, @Nombre, @Apellido, @Dni, @Email, @Telefono, @IdRol, @IdSucursal, @Estado, @PasswordHash",
                new SqlParameter("@IdEmpleado", empleado.IdEmpleado),
                new SqlParameter("@Nombre", empleado.Nombre),
                new SqlParameter("@Apellido", empleado.Apellido),
                new SqlParameter("@Dni", empleado.Dni),
                new SqlParameter("@Email", empleado.Email),
                new SqlParameter("@Telefono", empleado.Telefono ?? (object)DBNull.Value),
                new SqlParameter("@IdRol", empleado.IdRol),
                new SqlParameter("@IdSucursal", empleado.IdSucursal),
                new SqlParameter("@Estado", empleado.Estado),
                new SqlParameter("@PasswordHash", string.IsNullOrWhiteSpace(empleado.PasswordHash) ? DBNull.Value : empleado.PasswordHash));
        }

        public void Eliminar(int id)
        {
            var empleado = _context.Empleados.FirstOrDefault(e => e.IdEmpleado == id);
            if (empleado != null)
            {
                bool tieneHistorial = _context.CajaSesiones.Any(c => c.IdEmpleado == id) ||
                                      _context.Compras.Any(c => c.IdEmpleado == id);

                if (tieneHistorial)
                {
                    // [Procedimiento 5: sp_Usuarios_CambiarEstado]
                    _context.Database.ExecuteSqlRaw("EXEC sp_Usuarios_CambiarEstado @IdEmpleado={0}, @Estado={1}", id, false);
                }
                else
                {
                    try
                    {
                        _context.Empleados.Remove(empleado);
                        _context.SaveChanges();
                    }
                    catch
                    {
                        // [Procedimiento 5: sp_Usuarios_CambiarEstado] (Fallback)
                        _context.Entry(empleado).State = EntityState.Unchanged;
                        _context.Database.ExecuteSqlRaw("EXEC sp_Usuarios_CambiarEstado @IdEmpleado={0}, @Estado={1}", id, false);
                    }
                }
            }
        }
    }
}