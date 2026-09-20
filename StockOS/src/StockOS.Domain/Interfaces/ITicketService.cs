using StockOS.Domain.Entities;

namespace StockOS.Domain.Interfaces
{
    public interface ITicketService
    {
        // Devuelve el PDF en formato de bytes para guardarlo, enviarlo por mail o previsualizarlo
        byte[] GenerarTicketPdf(Venta venta, string cajeroNombre);
    }
}