using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StockOS.DataAccess.Persistence;
using StockOS.Domain.Entities;
using StockOS.Domain.Interfaces;

namespace StockOS.DataAccess.Repositories
{
    public class CajaSesionRepository : ICajaSesionRepository
    {
        private readonly StockOsContext _context;

        public CajaSesionRepository(StockOsContext context)
        {
            _context = context;
        }

        public int AbrirCaja(int idCaja, int idEmpleado, decimal montoApertura)
        {
            var idParam = new SqlParameter
            {
                ParameterName = "@IdCajaSesion",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };

            _context.Database.ExecuteSqlRaw(
                "EXEC sp_CajaSesion_Abrir @IdCaja={0}, @IdEmpleado={1}, @MontoApertura={2}, @IdCajaSesion=@IdCajaSesion OUTPUT",
                idCaja, idEmpleado, montoApertura, idParam);

            return (int)idParam.Value;
        }

        public bool VerificarCajaAbierta(int idEmpleado)
        {
            var paramAbierta = new SqlParameter
            {
                ParameterName = "@EstaAbierta",
                SqlDbType = System.Data.SqlDbType.Bit,
                Direction = System.Data.ParameterDirection.Output
            };

            _context.Database.ExecuteSqlRaw(
                "EXEC sp_CajaSesion_VerificarAbierta @IdEmpleado={0}, @EstaAbierta=@EstaAbierta OUTPUT",
                idEmpleado, paramAbierta);

            return (bool)paramAbierta.Value;
        }
        public int? ObtenerIdSesionAbierta(int idEmpleado)
        {
            var sesion = _context.Set<CajaSesion>()
                                 .FirstOrDefault(c => c.IdEmpleado == idEmpleado && c.Estado == 1);

            return sesion?.IdCajaSesion;
        }
    }
}