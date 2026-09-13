using System.Linq;
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
            // Busca en la base de datos el primer empleado cuyo Email coincida
            return _context.Empleados.FirstOrDefault(e => e.Email == email);
        }

        public Empleado? ObtenerPorDni(string dni)
        {
            // Busca en la base de datos el primer empleado cuyo DNI coincida
            return _context.Empleados.FirstOrDefault(e => e.Dni == dni);
        }
        public void Agregar(Empleado empleado)
        {
            _context.Empleados.Add(empleado);
            _context.SaveChanges();
        }
    }
}