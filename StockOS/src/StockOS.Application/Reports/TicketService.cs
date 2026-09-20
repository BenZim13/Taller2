using System.Linq;
using StockOS.Domain.Entities;
using StockOS.Domain.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace StockOS.Application.Reports
{
    public class TicketService : ITicketService
    {
        public TicketService()
        {
            // Requisito obligatorio de QuestPDF para la licencia comunitaria
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public byte[] GenerarTicketPdf(Venta venta, string cajeroNombre)
        {
            var documento = Document.Create(container =>
            {
                // Formato ticket térmico: 80mm de ancho (equivale a 226.8 puntos continuos)
                container.Page(page =>
                {
                    page.ContinuousSize(226.8f);
                    page.Margin(15);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(9).FontFamily("Arial"));

                    page.Header().Element(ComposeHeader);
                    page.Content().Element(x => ComposeContent(x, venta, cajeroNombre));
                    page.Footer().Element(ComposeFooter);
                });
            });

            return documento.GeneratePdf();
        }

        private void ComposeHeader(IContainer container)
        {
            container.Column(col =>
            {
                col.Item().AlignCenter().Text("STOCK OS").Bold().FontSize(16);
                col.Item().AlignCenter().Text("Tu comercio de confianza");
                col.Item().AlignCenter().Text("CUIT: 30-12345678-9");
                col.Item().PaddingTop(5).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
            });
        }

        private void ComposeContent(IContainer container, Venta venta, string cajeroNombre)
        {
            container.PaddingVertical(10).Column(col =>
            {
                col.Item().Text($"Ticket N°: {venta.IdVenta:D8}").Bold();
                col.Item().Text($"Fecha: {venta.FechaHora:dd/MM/yyyy HH:mm}");
                col.Item().Text($"Cajero: {cajeroNombre}");

                col.Item().PaddingVertical(5).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                // Tabla de productos
                col.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(3); // Cantidad + Nombre
                        columns.RelativeColumn(1); // Subtotal
                    });

                    table.Header(header =>
                    {
                        header.Cell().Text("Cant x Producto").Bold();
                        header.Cell().AlignRight().Text("Subtotal").Bold();
                    });

                    // Iteramos sobre tu colección "DetalleVenta"
                    if (venta.DetalleVenta != null)
                    {
                        foreach (var detalle in venta.DetalleVenta)
                        {
                            // Navegamos al producto usando "IdProductoNavigation"
                            string nombreProd = detalle.IdProductoNavigation?.Nombre ?? "Producto";

                            // Calculamos el subtotal del renglón
                            decimal subtotal = (detalle.Cantidad * detalle.PrecioUnitarioHistorico) - detalle.Descuento;

                            table.Cell().Text($"{detalle.Cantidad} x {nombreProd}");
                            table.Cell().AlignRight().Text($"${subtotal:N2}");
                        }
                    }
                });

                col.Item().PaddingVertical(5).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                // Total usando tu propiedad "TotalVenta"
                col.Item().AlignRight().Text($"TOTAL: ${venta.TotalVenta:N2}").Bold().FontSize(12);
            });
        }

        private void ComposeFooter(IContainer container)
        {
            container.AlignCenter().Column(col =>
            {
                col.Item().Text("¡Gracias por su compra!").Bold();
                col.Item().Text("Conserve este ticket para devoluciones").FontSize(8);
            });
        }
    }
}