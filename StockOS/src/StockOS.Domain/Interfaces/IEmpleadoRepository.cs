using StockOS.Domain.Entities;

namespace StockOS.Domain.Interfaces
{
    public interface IEmpleadoRepository
    {
        Empleado? ObtenerPorEmail(string email);
        Empleado? ObtenerPorDni(string dni);
        void Agregar(Empleado empleado);
    }


}