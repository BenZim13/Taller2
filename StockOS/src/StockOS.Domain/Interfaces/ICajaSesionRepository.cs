using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockOS.Domain.Interfaces
{
    public interface ICajaSesionRepository
    {
        int AbrirCaja(int idCaja, int idEmpleado, decimal montoApertura);
        bool VerificarCajaAbierta(int idEmpleado);
        int? ObtenerIdSesionAbierta(int idEmpleado);
    }
}