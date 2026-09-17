using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace StockOS.Application.Services
{
    // Clase auxiliar para pasar los datos al PDF
    public class ItemTicket
    {
        public string Producto { get; set; } = "";
        public int Cantidad { get; set; }
        public decimal PrecioTotal { get; set; }
    }

    public static class TicketService
    {
        public static void GenerarYAbrirTicket(int nroTicket, string cajero, decimal total, List<ItemTicket> items)
        {
            // Configuración obligatoria de la licencia gratuita de QuestPDF
            QuestPDF.Settings.License = LicenseType.Community;

            string filePath = Path.Combine(Path.GetTempPath(), $"Ticket_{nroTicket}_{DateTime.Now:yyyyMMddHHmmss}.pdf");

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    // Ticket de 80mm de ancho (aprox 226.7 puntos) y altura automática
                    page.ContinuousSize(226.7f);
                    page.Margin(15);
                    page.PageColor(Colors.White);

                    // CORRECCIÓN: Se usa un string puro "Arial"
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Content().Column(col =>
                    {
                        // 1. Cabecera Centrada
                        col.Item().AlignCenter().Text("StockOS").FontSize(20).Bold();

                        // CORRECCIÓN: El PaddingBottom se aplica a Item(), antes del Text()
                        col.Item().PaddingBottom(10).AlignCenter().Text("Gracias por su compra").FontSize(12);

                        col.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                        col.Item().PaddingVertical(5);

                        // 2. Datos al margen izquierdo
                        col.Item().AlignLeft().Text($"Ticket nro: {nroTicket:D8}").Bold();
                        col.Item().AlignLeft().Text($"Fecha hora: {DateTime.Now:dd/MM/yyyy HH:mm}");

                        // CORRECCIÓN: El PaddingBottom va en el Item()
                        col.Item().PaddingBottom(10).AlignLeft().Text($"Cajero: {cajero}");

                        col.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                        col.Item().PaddingVertical(5);

                        // 3. Lista de Productos
                        foreach (var item in items)
                        {
                            col.Item().Row(row =>
                            {
                                row.RelativeItem().Text($"{item.Cantidad}x {item.Producto}");
                                row.ConstantItem(60).AlignRight().Text($"${item.PrecioTotal:N2}");
                            });
                        }

                        col.Item().PaddingVertical(5);
                        col.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                        col.Item().PaddingVertical(5);

                        // 4. Total Final
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Text("TOTAL").Bold().FontSize(14);
                            row.ConstantItem(80).AlignRight().Text($"${total:N2}").Bold().FontSize(14);
                        });
                    });
                });
            })
            .GeneratePdf(filePath);

            // Abrir el PDF automáticamente con el visor predeterminado de Windows
            AbrirPdf(filePath);
        }

        private static void AbrirPdf(string path)
        {
            try
            {
                var p = new Process();
                p.StartInfo = new ProcessStartInfo(path) { UseShellExecute = true };
                p.Start();
            }
            catch (Exception ex)
            {
                // Manejo silencioso si el equipo no tiene lector de PDF
                Console.WriteLine(ex.Message);
            }
        }
    }
}