using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockOS.Domain.Interfaces;

namespace StockOS.Application.Services
{
    public class CajaService : ICajaService
    {
        private readonly ICajaSesionRepository _cajaRepo;

        public CajaService(ICajaSesionRepository cajaRepo)
        {
            _cajaRepo = cajaRepo;
        }

        public int AbrirCaja(int idCaja, int idEmpleado, decimal montoApertura)
        {
            return _cajaRepo.AbrirCaja(idCaja, idEmpleado, montoApertura);
        }
    }
}
