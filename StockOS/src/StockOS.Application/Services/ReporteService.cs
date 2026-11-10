using System;
using System.Collections.Generic;
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
        private readonly IAuthorizationService _authService;

        public ReporteService(IReporteRepository reporteRepository, IAuthorizationService authService)
        {
            _reporteRepository = reporteRepository;
            _authService = authService;
            QuestPDF.Settings.License = LicenseType.Community;
            QuestPDF.Settings.ThrowOnMissingFontFamilies = false;
            QuestPDF.Settings.UseSystemFonts = true;
        }

        public async Task<ResumenFinancieroDTO> ObtenerResumenFinancieroAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            _authService.ValidarPermiso(StockOS.Domain.Enums.Permisos.REPORTES_VER);
            return await _reporteRepository.ObtenerResumenFinancieroAsync(fechaInicio, fechaFin) ?? new ResumenFinancieroDTO();
        }
        public async Task GenerarReporteVentasPdfAsync(DateTime fechaInicio, DateTime fechaFin, string periodoDescripcion)
        {
            _authService.ValidarPermiso(StockOS.Domain.Enums.Permisos.REPORTES_VER);

            var data = await _reporteRepository.ObtenerReporteVentasAsync(fechaInicio, fechaFin);
            var resumen = await _reporteRepository.ObtenerResumenFinancieroAsync(fechaInicio, fechaFin) ?? new ResumenFinancieroDTO();

            var titulo = $"StockOS Reporte \"Ventas\" en el periodo {periodoDescripcion}";
            var fileName = $"Reporte_Ventas_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

            GenerarPdf(titulo, data.Total, data.Detalles ?? Enumerable.Empty<ReporteItemDTO>(), resumen, fileName, "Ventas");
        }

        public async Task GenerarReporteComprasPdfAsync(DateTime fechaInicio, DateTime fechaFin, string periodoDescripcion)
        {
            _authService.ValidarPermiso(StockOS.Domain.Enums.Permisos.REPORTES_VER);

            var data = await _reporteRepository.ObtenerReporteComprasAsync(fechaInicio, fechaFin);
            var resumen = await _reporteRepository.ObtenerResumenFinancieroAsync(fechaInicio, fechaFin) ?? new ResumenFinancieroDTO();

            var titulo = $"StockOS Reporte \"Compras\" en el periodo {periodoDescripcion}";
            var fileName = $"Reporte_Compras_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

            GenerarPdf(titulo, data.Total, data.Detalles ?? Enumerable.Empty<ReporteItemDTO>(), resumen, fileName, "Compras");
        }

        private void GenerarPdf(string titulo, decimal totalGeneral, IEnumerable<ReporteItemDTO> detalles, ResumenFinancieroDTO? resumen, string fileName, string tipoReporte)
        {
            resumen ??= new ResumenFinancieroDTO();
            var listaDetalles = (detalles ?? Enumerable.Empty<ReporteItemDTO>()).ToList();
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(1, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Arial", "Lato"));

                    page.Header().Element(c => ComposeHeader(c, titulo));
                    page.Content().Element(c => ComposeContent(c, listaDetalles, totalGeneral, resumen, tipoReporte));
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

        private void ComposeContent(IContainer container, IReadOnlyList<ReporteItemDTO> detalles, decimal totalGeneral, ResumenFinancieroDTO resumen, string tipoReporte)
        {
            container.PaddingVertical(1, Unit.Centimetre).Column(column =>
            {
                // Bloque visual Dinámico según el tipo de reporte
                column.Item().PaddingBottom(15).Background(Colors.Grey.Lighten4).Padding(10).Column(resumenCol =>
                {
                    resumenCol.Item().Text($"Resumen de {tipoReporte} del Período").SemiBold().FontSize(12).FontColor(Colors.Blue.Darken2);
                    resumenCol.Item().PaddingTop(5).Row(row =>
                    {
                        if (tipoReporte == "Ventas")
                        {
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text("Ingresos Brutos").FontSize(9).FontColor(Colors.Grey.Darken1);
                                c.Item().Text($"{resumen.IngresosBrutos:C2}").SemiBold().FontSize(11).FontColor(Colors.Green.Darken2);
                            });
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text("Descuentos").FontSize(9).FontColor(Colors.Grey.Darken1);
                                c.Item().Text($"{resumen.DescuentosOtorgados:C2}").SemiBold().FontSize(11).FontColor(Colors.Red.Medium);
                            });
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text("Ingresos Netos").FontSize(9).FontColor(Colors.Grey.Darken1);
                                c.Item().Text($"{resumen.IngresosNetos:C2}").Bold().FontSize(12).FontColor(Colors.Green.Darken3);
                            });
                        }
                        else if (tipoReporte == "Compras")
                        {
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text("Total Egresos (Compras)").FontSize(9).FontColor(Colors.Grey.Darken1);
                                c.Item().Text($"{resumen.EgresosBrutos:C2}").Bold().FontSize(12).FontColor(Colors.Red.Darken2);
                            });
                        }
                    });
                });

                // Tabla de detalles
                column.Item().PaddingBottom(5).Text("Detalle de Movimientos").SemiBold().FontSize(12);
                column.Item().Element(c => ComposeTable(c, detalles));

                // Total al pie de la tabla
                column.Item().PaddingTop(15).AlignRight().Text($"Total Listado: {totalGeneral:C2}")
                    .FontSize(14).Bold();
            });
        }
        private void ComposeTable(IContainer container, IReadOnlyList<ReporteItemDTO> detalles)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(2); // Código
                    columns.RelativeColumn(5); // Producto
                    columns.RelativeColumn(2); // Cantidad
                    columns.RelativeColumn(3); // Subtotal
                });

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

                if (detalles.Count == 0)
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
        public async Task<IEnumerable<ReporteItemDTO>> ObtenerTopProductosVendidosAsync(int topN, DateTime fechaInicio, DateTime fechaFin)
        {
            _authService.ValidarPermiso(StockOS.Domain.Enums.Permisos.REPORTES_VER);
            return await _reporteRepository.ObtenerTopProductosVendidosAsync(topN, fechaInicio, fechaFin) ?? Enumerable.Empty<ReporteItemDTO>();
        }

        public async Task<IEnumerable<VentasPorFechaDTO>> ObtenerVentasPorLineaTiempoAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            _authService.ValidarPermiso(StockOS.Domain.Enums.Permisos.REPORTES_VER);
            return await _reporteRepository.ObtenerVentasPorLineaTiempoAsync(fechaInicio, fechaFin) ?? Enumerable.Empty<VentasPorFechaDTO>();
        }
    }
}