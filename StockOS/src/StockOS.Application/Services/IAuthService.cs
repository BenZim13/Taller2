using System.Threading.Tasks;
using StockOS.Domain.Entities;

namespace StockOS.Application.Services
{
    public interface IAuthService
    {
        Task<(bool Exito, Empleado? Usuario, string? MensajeError)> LoginAsync(string dni, string password);
        //
        Task<bool> RegistrarAsync(Empleado empleado);
    }
}
