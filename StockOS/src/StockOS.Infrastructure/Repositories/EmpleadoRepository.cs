using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StockOS.Domain.Entities;
using StockOS.Domain.Interfaces;
using StockOS.DataAccess.Persistence;

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
            return _context.Empleados
                .Include(e => e.IdRolNavigation)
                .Include(e => e.IdSucursalNavigation)
                .FirstOrDefault(e => e.Dni == dni);
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
            _context.Empleados.Add(empleado);
            _context.SaveChanges();
        }

        public void Actualizar(Empleado empleado)
        {
            var existente = _context.Empleados.FirstOrDefault(e => e.IdEmpleado == empleado.IdEmpleado);
            if (existente != null)
            {
                existente.Nombre = empleado.Nombre;
                existente.Apellido = empleado.Apellido;
                existente.Dni = empleado.Dni;
                existente.Email = empleado.Email;
                existente.Telefono = empleado.Telefono;
                existente.IdRol = empleado.IdRol;
                existente.IdSucursal = empleado.IdSucursal;
                existente.Estado = empleado.Estado;

                if (!string.IsNullOrWhiteSpace(empleado.PasswordHash))
                {
                    existente.PasswordHash = empleado.PasswordHash;
                }

                _context.SaveChanges();
            }
        }

        public void Eliminar(int id)
        {
            var empleado = _context.Empleados.FirstOrDefault(e => e.IdEmpleado == id);
            if (empleado != null)
            {
                // Si el empleado tiene relaciones históricas, se realiza baja lógica
                bool tieneHistorial = _context.CajaSesiones.Any(c => c.IdEmpleado == id) ||
                                      _context.Compras.Any(c => c.IdEmpleado == id);

                if (tieneHistorial)
                {
                    empleado.Estado = false;
                }
                else
                {
                    try
                    {
                        _context.Empleados.Remove(empleado);
                    }
                    catch
                    {
                        // En caso de conflicto de integridad de base de datos imprevisto, respaldar con baja lógica
                        _context.Entry(empleado).State = EntityState.Unchanged;
                        empleado.Estado = false;
                    }
                }

                _context.SaveChanges();
            }
        }
    }
}