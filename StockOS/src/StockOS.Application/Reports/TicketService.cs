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
            QuestPDF.Settings.License = LicenseType.Community;
            QuestPDF.Settings.UseSystemFonts = true;
        }

        public byte[] GenerarTicketPdf(Venta venta, string cajeroNombre, DatosComercio comercio, DatosPago pago)
        {
            var documento = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.ContinuousSize(226.8f); // 80mm
                    page.Margin(12);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(8.5f).FontFamily("Consolas"));

                    // Le pasamos la info del comercio, cajero y pago al Header y al Content
                    page.Header().Element(x => ComposeHeader(x, comercio));
                    page.Content().Element(x => ComposeContent(x, venta, cajeroNombre, comercio, pago));
                });
            });

            return documento.GeneratePdf();
        }

        private void ComposeHeader(IContainer container, DatosComercio comercio)
        {
            container.Column(col =>
            {
                // Datos dinámicos inyectados
                col.Item().AlignCenter().Text(comercio.Nombre).FontSize(14).Bold();
                col.Item().Text($"C.U.I.T. Nro.: {comercio.Cuit}");
                col.Item().Text($"Ing. Brutos: {comercio.IngresosBrutos}");
                col.Item().Text($"Domicilio: {comercio.Direccion}");
                col.Item().Text($"Inicio de Actividades: {comercio.InicioActividades}");
                col.Item().Text(comercio.CondicionIva);
                col.Item().Text("A CONSUMIDOR FINAL");

                col.Item().PaddingVertical(5).LineHorizontal(1).LineColor(Colors.Black);
            });
        }

        private void ComposeContent(IContainer container, Venta venta, string cajeroNombre, DatosComercio comercio, DatosPago pago)
        {
            container.Column(col =>
            {
                // Encabezado del ticket con datos dinámicos
                col.Item().Text($"TIQUE (Cod.083)    P.V. N° {comercio.PuntoVenta} Nro. T. {venta.IdVenta:D8}");
                col.Item().Text($"Fecha {venta.FechaHora:dd/MM/yyyy} Hora {venta.FechaHora:HH:mm:ss}");
                col.Item().Text($"Cajero: {cajeroNombre}");

                col.Item().PaddingVertical(5).LineHorizontal(1).LineColor(Colors.Black);

                // Detalle de productos
                if (venta.DetalleVenta != null)
                {
                    foreach (var detalle in venta.DetalleVenta)
                    {
                        string nombreProd = detalle.IdProductoNavigation?.Nombre?.ToUpper() ?? "PRODUCTO";
                        decimal subtotalItem = (detalle.Cantidad * detalle.PrecioUnitarioHistorico) - detalle.Descuento;

                        col.Item().Text($"{detalle.Cantidad:N2} (00) x {detalle.PrecioUnitarioHistorico:N2}");

                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Text($"{nombreProd} (0)");
                            row.ConstantItem(50).AlignRight().Text($"{subtotalItem:N2}");
                        });
                    }
                }

                col.Item().PaddingVertical(5).LineHorizontal(1).LineColor(Colors.Black);

                // Subtotal (antes de descuentos)
                col.Item().Row(row =>
                {
                    row.RelativeItem().Text("Subtotal");
                    row.ConstantItem(60).AlignRight().Text($"{venta.Subtotal:N2}");
                });

                // Mostrar descuento solo si existe
                if (venta.DescuentoTotal > 0)
                {
                    col.Item().Row(row =>
                    {
                        row.RelativeItem().Text("Descuento");
                        row.ConstantItem(60).AlignRight().Text($"-{venta.DescuentoTotal:N2}");
                    });
                }

                col.Item().PaddingVertical(3);

                // Total final
                col.Item().Row(row =>
                {
                    row.RelativeItem().Text("TOTAL").FontSize(13).Bold();
                    row.ConstantItem(80).AlignRight().Text($"{venta.TotalVenta:N2}").FontSize(13).Bold();
                });

                col.Item().PaddingVertical(5).LineHorizontal(1).LineColor(Colors.Black);

                // Sección de pago - ahora dinámica
                col.Item().Text("RECIBI/MOS");
                col.Item().Row(row => 
                { 
                    row.RelativeItem().Text(pago.MetodoPago); 
                    row.ConstantItem(60).AlignRight().Text($"{pago.MontoRecibido:N2}"); 
                });
                col.Item().Row(row => 
                { 
                    row.RelativeItem().Text("Suma de sus pagos"); 
                    row.ConstantItem(60).AlignRight().Text($"{pago.MontoRecibido:N2}"); 
                });
                col.Item().Row(row => 
                { 
                    row.RelativeItem().Text("Su Vuelto").Bold(); 
                    row.ConstantItem(60).AlignRight().Text($"{pago.Vuelto:N2}").Bold(); 
                });

                col.Item().PaddingVertical(5).LineHorizontal(1).LineColor(Colors.Black);

                // Información fiscal - Calcular IVA por alícuota según cada producto
                col.Item().Text("REGIMEN TRANSPARENCIA FISCAL CONSUMIDOR");

                if (venta.DetalleVenta != null && venta.DetalleVenta.Any())
                {
                    // Agrupar por alícuota de IVA
                    var ivasPorAlicuota = venta.DetalleVenta
                        .GroupBy(d => d.IdProductoNavigation?.PorcentajeIva ?? 21m)
                        .Select(g => new
                        {
                            Alicuota = g.Key,
                            MontoIva = g.Sum(d =>
                            {
                                decimal subtotalItem = (d.Cantidad * d.PrecioUnitarioHistorico) - d.Descuento;
                                return subtotalItem - (subtotalItem / (1 + g.Key / 100m));
                            })
                        })
                        .OrderByDescending(x => x.Alicuota);

                    foreach (var iva in ivasPorAlicuota)
                    {
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Text($"Alicuota {iva.Alicuota:N0}%");
                            row.ConstantItem(60).AlignRight().Text($"{iva.MontoIva:N2}");
                        });
                    }
                }
                else
                {
                    // Fallback si no hay detalles
                    decimal ivaCalculado = venta.TotalVenta - (venta.TotalVenta / 1.21m);
                    col.Item().Row(row =>
                    {
                        row.RelativeItem().Text("Alicuota 21%");
                        row.ConstantItem(60).AlignRight().Text($"{ivaCalculado:N2}");
                    });
                }

                // Registro fiscal dinámico (si está configurado)
                if (!string.IsNullOrWhiteSpace(comercio.RegistroFiscal))
                {
                    col.Item().AlignCenter().Text($"REGISTRO: {comercio.RegistroFiscal}");
                }
            });
        }
    }
}