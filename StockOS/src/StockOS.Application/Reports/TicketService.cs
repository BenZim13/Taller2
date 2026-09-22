using System.Linq;
using StockOS.Domain.Entities;
using StockOS.Domain.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace StockOS.Application.Reports
{
    /// <summary>
    /// Servicio de generación de tickets de venta en formato PDF usando QuestPDF.
    /// Genera comprobantes térmicos de 80mm con formato fiscal.
    /// </summary>
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
                    // Ancho estándar de papel térmico: 80mm = 226.8 puntos
                    page.ContinuousSize(226.8f);
                    page.Margin(12);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(8.5f).FontFamily("Consolas"));

                    // Composición del ticket: encabezado con datos fiscales y contenido con detalle de venta
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

                // Información fiscal - Declarar alícuota de IVA aplicada (sin mostrar el total en pesos)
                col.Item().Text("REGIMEN TRANSPARENCIA FISCAL CONSUMIDOR");

                if (venta.DetalleVenta != null && venta.DetalleVenta.Any())
                {
                    // Obtener alícuotas de IVA distintas aplicadas a los productos
                    var alicuotas = venta.DetalleVenta
                        .Select(d => (d.IdProductoNavigation != null && d.IdProductoNavigation.PorcentajeIva > 0)
                            ? d.IdProductoNavigation.PorcentajeIva
                            : Producto.IvaFijoDefault)
                        .Distinct()
                        .OrderByDescending(x => x);

                    foreach (var alicuota in alicuotas)
                    {
                        col.Item().Text($"Alicuota IVA ({alicuota:N0}%)");
                    }
                }
                else
                {
                    // Fallback si no hay detalles (declara IVA fijo 21%)
                    col.Item().Text($"Alicuota IVA ({Producto.IvaFijoDefault:N0}%)");
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