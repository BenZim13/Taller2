using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StockOS.DataAccess.Persistence;
using StockOS.Domain.Entities;
using StockOS.Domain.Interfaces;

namespace StockOS.DataAccess.Repositories
{
    public class CajaRepository : ICajaRepository
    {
        private readonly StockOsContext _context;

        public CajaRepository(StockOsContext context)
        {
            _context = context;
        }

        public IEnumerable<Caja> ObtenerPorSucursal(int idSucursal)
        {
            // Ejecutamos el nuevo SP que creamos
            return _context.Cajas
                .FromSqlRaw("EXEC sp_Cajas_ObtenerPorSucursal @IdSucursal={0}", idSucursal)
                .ToList();
        }
        public decimal ObtenerMontoEsperado(int idCajaSesion)
        {
            var montoEsperadoParam = new Microsoft.Data.SqlClient.SqlParameter
            {
                ParameterName = "@MontoEsperado",
                SqlDbType = System.Data.SqlDbType.Decimal,
                Direction = System.Data.ParameterDirection.Output,
                Precision = 18,
                Scale = 2
            };

            _context.Database.ExecuteSqlRaw(
                "EXEC sp_Caja_CalcularMontoEsperado @IdCajaSesion={0}, @MontoEsperado=@MontoEsperado OUTPUT",
                idCajaSesion, montoEsperadoParam);

            return montoEsperadoParam.Value != DBNull.Value ? (decimal)montoEsperadoParam.Value : 0;
        }
        public void CerrarCaja(int idCajaSesion, decimal montoCierreReal)
        {
            _context.Database.ExecuteSqlRaw(
                "EXEC sp_CajaSesion_Cerrar @IdCajaSesion={0}, @MontoCierreReal={1}",
                idCajaSesion, montoCierreReal);
        }
        public void RegistrarMovimiento(int idCajaSesion, string tipo, decimal monto, string descripcion)
        {
            _context.Database.ExecuteSqlRaw(
                "EXEC sp_MovimientoCaja_Insertar @IdCajaSesion={0}, @Tipo={1}, @Monto={2}, @Descripcion={3}",
                idCajaSesion, tipo, monto, descripcion);
        }

    }
}