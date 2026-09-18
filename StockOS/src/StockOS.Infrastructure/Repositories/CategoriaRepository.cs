using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StockOS.DataAccess.Persistence;
using StockOS.Domain.Entities;
using StockOS.Domain.Interfaces;

namespace StockOS.DataAccess.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly StockOsContext _context;

        public CategoriaRepository(StockOsContext context)
        {
            _context = context;
        }

        public IEnumerable<Categoria> ObtenerTodos()
        {
            return _context.Categorias
                .FromSqlRaw("EXEC sp_Categorias_ObtenerTodas")
                .ToList();
        }

        public void Agregar(Categoria categoria)
        {
            var idParam = new SqlParameter
            {
                ParameterName = "@IdCategoria",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };

            _context.Database.ExecuteSqlRaw(
                "EXEC sp_Categorias_Insertar @Nombre={0}, @Descripcion={1}, @IdCategoria=@IdCategoria OUTPUT",
                categoria.Nombre,
                categoria.Descripcion,
                idParam);

            categoria.IdCategoria = (int)idParam.Value;
        }

        public void Actualizar(Categoria categoria)
        {
            _context.Database.ExecuteSqlRaw(
                "EXEC sp_Categorias_Actualizar @IdCategoria={0}, @Nombre={1}, @Descripcion={2}, @Activo={3}",
                categoria.IdCategoria,
                categoria.Nombre,
                categoria.Descripcion,
                categoria.Activo);
        }
    }
}