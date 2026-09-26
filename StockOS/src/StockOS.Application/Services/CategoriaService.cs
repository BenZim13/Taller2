using System.Collections.Generic;
using StockOS.Domain.Entities;
using StockOS.Domain.Interfaces;
using StockOS.Domain.Enums;

namespace StockOS.Application.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IAuthorizationService _authService;

        public CategoriaService(ICategoriaRepository categoriaRepository, IAuthorizationService authService)
        {
            _categoriaRepository = categoriaRepository;
            _authService = authService;
        }

        public IEnumerable<Categoria> ObtenerTodos()
        {
            // Cualquier rol puede ver las categorías (necesario para el catálogo)
            return _categoriaRepository.ObtenerTodos();
        }

        public void Agregar(Categoria categoria)
        {
            ValidarReglasDeNegocio(categoria);
            _authService.ValidarPermiso(Permisos.CATEGORIAS_GESTIONAR);
            _categoriaRepository.Agregar(categoria);
        }

        public void Actualizar(Categoria categoria)
        {
            ValidarReglasDeNegocio(categoria);
            _authService.ValidarPermiso(Permisos.CATEGORIAS_GESTIONAR);
            _categoriaRepository.Actualizar(categoria);
        }
        private void ValidarReglasDeNegocio(Categoria categoria)
        {
            if (categoria == null) throw new ArgumentNullException(nameof(categoria));

            categoria.Nombre = categoria.Nombre?.Trim() ?? "";
            categoria.Descripcion = categoria.Descripcion?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(categoria.Nombre) || categoria.Nombre.Length > 100)
                throw new ArgumentException("El nombre es obligatorio (máx 100 caracteres).");

            if (categoria.Descripcion.Length > 255)
                throw new ArgumentException("La descripción no puede superar los 255 caracteres.");
        }
    }
}