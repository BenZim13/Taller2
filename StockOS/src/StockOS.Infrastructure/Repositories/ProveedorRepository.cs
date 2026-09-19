using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StockOS.DataAccess.Persistence;
using StockOS.Domain.Entities;
using StockOS.Domain.Interfaces;

namespace StockOS.DataAccess.Repositories
{
    public class ProveedorRepository : IProveedorRepository
    {
        private readonly StockOsContext _context;

        public ProveedorRepository(StockOsContext context)
        {
            _context = context;
        }

        public IEnumerable<Proveedor> ObtenerTodos()
        {
            return _context.Proveedores
                .FromSqlRaw("EXEC sp_Proveedores_ObtenerTodos")
                .AsNoTracking()
                .ToList();
        }

        public void Agregar(Proveedor proveedor)
        {
            var idParam = new SqlParameter
            {
                ParameterName = "@IdProveedor",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };

            var cuitParam = new SqlParameter("@Cuit", SqlDbType.VarChar)
            {
                Value = string.IsNullOrWhiteSpace(proveedor.Cuit) ? (object)DBNull.Value : proveedor.Cuit
            };

            _context.Database.ExecuteSqlRaw(
                "EXEC sp_Proveedores_Insertar @RazonSocial={0}, @Cuit={1}, @Telefono={2}, @Email={3}, @Direccion={4}, @Activo={5}, @IdProveedor=@IdProveedor OUTPUT",
                proveedor.RazonSocial,
                cuitParam,
                proveedor.Telefono ?? "",
                proveedor.Email ?? "",
                proveedor.Direccion ?? "",
                proveedor.Activo,
                idParam);

            proveedor.IdProveedor = (int)idParam.Value;
        }

        public void Actualizar(Proveedor proveedor)
        {
            var cuitParam = new SqlParameter("@Cuit", SqlDbType.VarChar)
            {
                Value = string.IsNullOrWhiteSpace(proveedor.Cuit) ? (object)DBNull.Value : proveedor.Cuit
            };

            _context.Database.ExecuteSqlRaw(
                "EXEC sp_Proveedores_Actualizar @IdProveedor={0}, @RazonSocial={1}, @Cuit={2}, @Telefono={3}, @Email={4}, @Direccion={5}, @Activo={6}",
                proveedor.IdProveedor,
                proveedor.RazonSocial,
                cuitParam,
                proveedor.Telefono ?? "",
                proveedor.Email ?? "",
                proveedor.Direccion ?? "",
                proveedor.Activo);
        }

        public void CambiarEstado(int idProveedor, bool activo)
        {
            _context.Database.ExecuteSqlRaw(
                "EXEC sp_Proveedores_CambiarEstado @IdProveedor={0}, @Activo={1}",
                idProveedor,
                activo);
        }
    }
}

