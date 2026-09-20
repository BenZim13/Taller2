using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using StockOS.Domain.Interfaces;
using StockOS.Domain.DTOs;

namespace StockOS.Application.Services
{
    public class ReporteService : IReporteService
    {
        private readonly IReporteRepository _reporteRepository;

        public ReporteService(IReporteRepository reporteRepository)
        {
            _reporteRepository = reporteRepository;
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public async Task GenerarReporteVentasPdfAsync(DateTime fechaInicio, DateTime fechaFin, string periodoDescripcion)
        {
            var data = await _reporteRepository.ObtenerReporteVentasAsync(fechaInicio, fechaFin);
            var titulo = $"StockOS Reporte \"Ventas\" en el periodo {periodoDescripcion}";
            var fileName = $"Reporte_Ventas_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
            
            GenerarPdf(titulo, data.Total, data.Detalles, fileName);
        }

        public async Task GenerarReporteComprasPdfAsync(DateTime fechaInicio, DateTime fechaFin, string periodoDescripcion)
        {
            var data = await _reporteRepository.ObtenerReporteComprasAsync(fechaInicio, fechaFin);
            var titulo = $"StockOS Reporte \"Compras\" en el periodo {periodoDescripcion}";
            var fileName = $"Reporte_Compras_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
            
            GenerarPdf(titulo, data.Total, data.Detalles, fileName);
        }

        private void GenerarPdf(string titulo, decimal totalGeneral, System.Collections.Generic.IEnumerable<ReporteItemDTO> detalles, string fileName)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(1, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Arial"));

                    page.Header().Element(c => ComposeHeader(c, titulo));
                    page.Content().Element(c => ComposeContent(c, detalles, totalGeneral));
                    page.Footer().Element(ComposeFooter);
                });
            });

            var filePath = Path.Combine(Path.GetTempPath(), fileName);
            document.GeneratePdf(filePath);

            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al abrir el PDF de reporte: {ex.Message}");
            }
        }

        private void ComposeHeader(IContainer container, string titulo)
        {
            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text(titulo)
                        .FontSize(18).SemiBold().FontColor(Colors.Blue.Darken2);

                    column.Item().Text($"Fecha de emisión: {DateTime.Now:dd/MM/yyyy HH:mm}")
                        .FontSize(10).FontColor(Colors.Grey.Medium);
                });
            });
        }

        private void ComposeContent(IContainer container, System.Collections.Generic.IEnumerable<ReporteItemDTO> detalles, decimal totalGeneral)
        {
            container.PaddingVertical(1, Unit.Centimetre).Column(column =>
            {
                column.Item().Element(c => ComposeTable(c, detalles));

                column.Item().PaddingTop(15).AlignRight().Text($"Total General: {totalGeneral:C2}")
                    .FontSize(14).Bold();
            });
        }

        private void ComposeTable(IContainer container, System.Collections.Generic.IEnumerable<ReporteItemDTO> detalles)
        {
            container.Table(table =>
            {
                // Definir las columnas
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(2); // Código
                    columns.RelativeColumn(5); // Producto
                    columns.RelativeColumn(2); // Cantidad
                    columns.RelativeColumn(3); // Subtotal
                });

                // Header
                table.Header(header =>
                {
                    header.Cell().Element(CellStyle).Text("Código");
                    header.Cell().Element(CellStyle).Text("Producto");
                    header.Cell().Element(CellStyle).AlignRight().Text("Cantidad");
                    header.Cell().Element(CellStyle).AlignRight().Text("Subtotal");

                    static IContainer CellStyle(IContainer container)
                    {
                        return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                    }
                });

                // Datos
                foreach (var item in detalles)
                {
                    table.Cell().Element(CellStyle).Text(item.CodigoBarra);
                    table.Cell().Element(CellStyle).Text(item.NombreProducto);
                    table.Cell().Element(CellStyle).AlignRight().Text(item.CantidadTotal.ToString());
                    table.Cell().Element(CellStyle).AlignRight().Text($"{item.Subtotal:C2}");

                    static IContainer CellStyle(IContainer container)
                    {
                        return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                    }
                }
                
                if (!detalles.Any())
                {
                    table.Cell().ColumnSpan(4).PaddingVertical(10).AlignCenter()
                         .Text("No hay registros en el período seleccionado.").Italic().FontColor(Colors.Grey.Medium);
                }
            });
        }

        private void ComposeFooter(IContainer container)
        {
            container.AlignCenter().Text(x =>
            {
                x.Span("Página ");
                x.CurrentPageNumber();
                x.Span(" de ");
                x.TotalPages();
            });
        }
    }
}
