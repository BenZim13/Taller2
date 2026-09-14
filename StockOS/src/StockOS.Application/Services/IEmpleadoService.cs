using System.Collections.Generic;
using System.Threading.Tasks;
using StockOS.Domain.Entities;

namespace StockOS.Application.Services
{
    public interface IEmpleadoService
    {
        IEnumerable<Empleado> ObtenerTodos();
        Empleado? ObtenerPorId(int id);
        Task<bool> CrearAsync(Empleado empleado);
        Task<(bool Exito, string Mensaje)> ActualizarAsync(Empleado empleado);
        Task<bool> EliminarAsync(int id);
        Task<bool> CambiarEstadoAsync(int id, bool nuevoEstado);
    }
}

