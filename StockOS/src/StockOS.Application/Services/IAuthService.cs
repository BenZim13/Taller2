using System.Threading.Tasks;
using StockOS.Domain.Entities;

namespace StockOS.Application.Services
{
    public interface IAuthService
    {
        //
        Task<Empleado?> LoginAsync(string dni, string password);
        //
        Task<bool> RegistrarAsync(Empleado empleado);
    }
}
