using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using StockOS.Application.Reports;
using StockOS.Domain.Entities;
using StockOS.Domain.Interfaces;

namespace StockOS.Application.Tests
{
    public class TicketServiceTests
    {
        // ==========================================
        // PRUEBAS DE ÉXITO (Generación de Tickets)
        // ==========================================

        [Fact]
        public void GenerarTicketPdf_ConVentaYDatosValidos_GeneraDocumentoPdfValido()
        {
            // ARRANGE
            var service = new TicketService();

            var comercio = new DatosComercio
            {
                Nombre = "Comercio de Prueba",
                Cuit = "30-12345678-9",
                Direccion = "Calle 1 123",
                IngresosBrutos = "30-12345678-9",
                InicioActividades = "01/01/2024",
                CondicionIva = "IVA RESPONSABLE INSCRIPTO",
                PuntoVenta = "00001",
                RegistroFiscal = "REG12345"
            };

            var pago = new DatosPago
            {
                MetodoPago = "Efectivo",
                MontoRecibido = 1000m,
                Vuelto = 200m
            };

            var venta = new Venta
            {
                IdVenta = 42,
                FechaHora = DateTime.Now,
                Subtotal = 800m,
                DescuentoTotal = 0m,
                TotalVenta = 800m,
                DetalleVenta = new List<DetalleVenta>
                {
                    new DetalleVenta
                    {
                        IdProducto = 1,
                        Cantidad = 2,
                        PrecioUnitarioHistorico = 400m,
                        Descuento = 0m,
                        IdProductoNavigation = new Producto
                        {
                            Nombre = "Arroz 1kg",
                            PorcentajeIva = 21m
                        }
                    }
                }
            };

            // ACT
            byte[] pdfBytes = service.GenerarTicketPdf(venta, "Juan Pérez", comercio, pago);

            // ASSERT
            Assert.NotNull(pdfBytes);
            Assert.True(pdfBytes.Length > 0);

            // Verificar que comienza con el encabezado estándar de archivo PDF (%PDF)
            string pdfHeader = Encoding.ASCII.GetString(pdfBytes, 0, Math.Min(5, pdfBytes.Length));
            Assert.StartsWith("%PDF", pdfHeader);
        }

        [Fact]
        public void GenerarTicketPdf_ConMultiplesAlicuotasIvaYDescuento_GeneraPdfSinExcepciones()
        {
            // ARRANGE
            var service = new TicketService();

            var comercio = new DatosComercio
            {
                Nombre = "Comercio Multi-IVA",
                Cuit = "30-11111111-1",
                Direccion = "Av Central 100",
                IngresosBrutos = "30-11111111-1",
                InicioActividades = "01/01/2023",
                CondicionIva = "RESPONSABLE INSCRIPTO",
                PuntoVenta = "00003",
                RegistroFiscal = "REG-MULTI"
            };

            var pago = new DatosPago
            {
                MetodoPago = "Tarjeta de Débito",
                MontoRecibido = 1500m,
                Vuelto = 0m
            };

            var venta = new Venta
            {
                IdVenta = 101,
                FechaHora = DateTime.Now,
                Subtotal = 1600m,
                DescuentoTotal = 100m,
                TotalVenta = 1500m,
                DetalleVenta = new List<DetalleVenta>
                {
                    new DetalleVenta
                    {
                        IdProducto = 1,
                        Cantidad = 1,
                        PrecioUnitarioHistorico = 1000m,
                        Descuento = 50m,
                        IdProductoNavigation = new Producto { Nombre = "Monitor LED", PorcentajeIva = 21m }
                    },
                    new DetalleVenta
                    {
                        IdProducto = 2,
                        Cantidad = 2,
                        PrecioUnitarioHistorico = 300m,
                        Descuento = 50m,
                        IdProductoNavigation = new Producto { Nombre = "Pan Lactal", PorcentajeIva = 10.5m }
                    }
                }
            };

            // ACT
            byte[] pdfBytes = service.GenerarTicketPdf(venta, "María López", comercio, pago);

            // ASSERT
            Assert.NotNull(pdfBytes);
            Assert.True(pdfBytes.Length > 0);
            string pdfHeader = Encoding.ASCII.GetString(pdfBytes, 0, Math.Min(5, pdfBytes.Length));
            Assert.StartsWith("%PDF", pdfHeader);
        }

        [Fact]
        public void GenerarTicketPdf_SinDetallesDeVenta_GeneraPdfConCalculoFallback()
        {
            // ARRANGE
            var service = new TicketService();
            var comercio = new DatosComercio { Nombre = "Kiosco", Cuit = "20-00000000-0" };
            var pago = new DatosPago { MetodoPago = "Efectivo", MontoRecibido = 500m, Vuelto = 0m };
            var venta = new Venta
            {
                IdVenta = 1,
                FechaHora = DateTime.Now,
                Subtotal = 500m,
                DescuentoTotal = 0m,
                TotalVenta = 500m,
                DetalleVenta = new List<DetalleVenta>() // Lista vacía
            };

            // ACT
            byte[] pdfBytes = service.GenerarTicketPdf(venta, "Cajero 1", comercio, pago);

            // ASSERT
            Assert.NotNull(pdfBytes);
            Assert.True(pdfBytes.Length > 0);
        }
    }
}
