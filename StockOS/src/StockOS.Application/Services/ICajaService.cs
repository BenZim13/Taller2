using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockOS.Application.Services
{
    public interface ICajaService
    {
        int AbrirCaja(int idCaja, int idEmpleado, decimal montoApertura);
    }
}
