using StockOS.Domain.Entities;

namespace StockOS.Domain.Interfaces
{
    /// <summary>
    /// Datos fiscales y comerciales del negocio para imprimir en tickets
    /// </summary>
    public class DatosComercio
    {
        public string Nombre { get; set; } = "StockOS";
        public string Cuit { get; set; } = "00-00000000-0";
        public string Direccion { get; set; } = "Dirección no configurada";
        public string IngresosBrutos { get; set; } = "00-00000000-0";
        public string InicioActividades { get; set; } = "01/01/2024";
        public string CondicionIva { get; set; } = "IVA RESPONSABLE INSCRIPTO";
        public string PuntoVenta { get; set; } = "00001";
        public string RegistroFiscal { get; set; } = "";
    }

    /// <summary>
    /// Información del pago realizado para mostrar en el ticket
    /// </summary>
    public class DatosPago
    {
        public string MetodoPago { get; set; } = "Efectivo";
        public decimal MontoRecibido { get; set; }
        public decimal Vuelto { get; set; }
    }

    public interface ITicketService
    {
        /// <summary>
        /// Genera el ticket de venta en formato PDF
        /// </summary>
        /// <param name="venta">Datos de la venta con sus detalles</param>
        /// <param name="cajeroNombre">Nombre completo del cajero</param>
        /// <param name="comercio">Datos fiscales del comercio</param>
        /// <param name="pago">Información del pago (método, monto recibido, vuelto)</param>
        byte[] GenerarTicketPdf(Venta venta, string cajeroNombre, DatosComercio comercio, DatosPago pago);
    }
}