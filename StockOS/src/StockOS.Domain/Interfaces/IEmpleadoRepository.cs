using StockOS.Domain.Entities;

namespace StockOS.Domain.Interfaces
{
    public interface IEmpleadoRepository
    {
        Empleado? ObtenerPorEmail(string email);
        Empleado? ObtenerPorDni(string dni);
        Empleado? ObtenerPorId(int id);
        IEnumerable<Empleado> ObtenerTodos();
        void Agregar(Empleado empleado);
        void Actualizar(Empleado empleado);
        void Eliminar(int id);
    }
}