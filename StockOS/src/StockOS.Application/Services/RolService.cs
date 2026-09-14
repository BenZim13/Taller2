using System.Collections.Generic;
using StockOS.Domain.Entities;
using StockOS.Domain.Interfaces;

namespace StockOS.Application.Services
{
    public class RolService : IRolService
    {
        private readonly IRolRepository _rolRepository;

        public RolService(IRolRepository rolRepository)
        {
            _rolRepository = rolRepository;
        }

        public IEnumerable<Rol> ObtenerRoles()
        {
            return _rolRepository.ObtenerTodos();
        }
    }
}

