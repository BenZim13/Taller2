using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Moq;
using Xunit;
using StockOS.Application.Reports;
using StockOS.Application.Services;
using StockOS.Domain.Entities;
using StockOS.Domain.Enums;
using StockOS.Domain.Interfaces;
using StockOS.UI.WinForms.Forms;

namespace StockOS.Application.Tests
{
    /// <summary>
    /// Suite exhaustiva de pruebas de robustez, consistencia, límites e integridad
    /// para el ciclo de vida del IVA fijo del 21% y la formación de precios en StockOS.
    /// </summary>
    public class IvaRobustezTestSuite
    {
        // Helper para ejecutar código de controles WinForms en un hilo STA
        private void RunInSta(Action action)
        {
            Exception? threadEx = null;
            var thread = new Thread(() =>
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    threadEx = ex;
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();

            if (threadEx != null)
            {
                throw new AggregateException("Error en ejecución de hilo STA WinForms", threadEx);
            }
        }

        // =========================================================================
        // 1. ROBUSTEZ MATEMÁTICA Y LÍMITES DE DOMINIO (Producto.CalcularPrecioVentaFinal)
        // =========================================================================

        [Fact]
        public void CalculoPrecioVenta_CasoExactoPan1000Margen9Iva21_RetornaExactamente1300()
        {
            // 1 kg de pan: Costo $1000, Margen 9%, IVA 21% -> $1000 * 1.30 = $1300.00
            decimal costo = 1000.00m;
            decimal margen = 9.00m;

            decimal precioFinal = Producto.CalcularPrecioVentaFinal(costo, margen, Producto.IvaFijoDefault);

            Assert.Equal(1300.00m, precioFinal);
        }

        [Fact]
        public void CalculoPrecioVenta_MargenCero_AplicaSoloIva21()
        {
            // Costo $100, Margen 0%, IVA 21% -> $100 * 1.21 = $121.00
            decimal costo = 100.00m;
            decimal margen = 0.00m;

            decimal precioFinal = Producto.CalcularPrecioVentaFinal(costo, margen);

            Assert.Equal(121.00m, precioFinal);
        }

        [Fact]
        public void CalculoPrecioVenta_CostoCero_RetornaCero()
        {
            // Costo $0, Margen 10%, IVA 21% -> $0
            decimal precioFinal = Producto.CalcularPrecioVentaFinal(0m, 10m);
            Assert.Equal(0.00m, precioFinal);
        }

        [Theory]
        [InlineData(10.50, 15.0, 21.0, 14.28)]    // 10.50 * (1 + 0.36) = 14.28
        [InlineData(33.33, 10.0, 21.0, 43.66)]    // 33.33 * 1.31 = 43.6623 -> 43.66
        [InlineData(199.99, 5.5, 21.0, 252.99)]   // 199.99 * 1.265 = 252.98735 -> 252.99
        [InlineData(500.00, 200.0, 21.0, 1605.00)] // Margen alto: 500 * (1 + 2.21) = 1605.00
        [InlineData(1000000.00, 10.0, 21.0, 1310000.00)] // Monto grande
        public void CalculoPrecioVenta_VariadosValoresYMargenes_AplicaRedondeoFinancieroCorrecto(
            decimal costo, decimal margen, decimal iva, decimal esperado)
        {
            decimal precioFinal = Producto.CalcularPrecioVentaFinal(costo, margen, iva);
            Assert.Equal(esperado, precioFinal);
        }

        [Fact]
        public void CalculoPrecioVenta_CostoNegativo_LanzaArgumentException()
        {
            var ex = Assert.Throws<ArgumentException>(() => Producto.CalcularPrecioVentaFinal(-50m, 10m));
            Assert.Contains("El precio de compra no puede ser negativo", ex.Message);
        }

        [Fact]
        public void CalculoPrecioVenta_MargenNegativo_LanzaArgumentException()
        {
            var ex = Assert.Throws<ArgumentException>(() => Producto.CalcularPrecioVentaFinal(100m, -5m));
            Assert.Contains("El margen de ganancia no puede ser negativo", ex.Message);
        }

        [Fact]
        public void EntidadProducto_InstanciacionPorDefecto_PoseeIvaFijo21()
        {
            var prod = new Producto();
            Assert.Equal(21.00m, prod.PorcentajeIva);
            Assert.Equal(21.00m, Producto.IvaFijoDefault);
        }

        // =========================================================================
        // 2. ROBUSTEZ EN CAPA DE SERVICIOS (ProductoService)
        // =========================================================================

        [Fact]
        public void ProductoService_AgregarProductoConIvaCeroONegativo_NormalizaAIva21()
        {
            var mockRepo = new Mock<IProductoRepository>();
            var mockAuth = new Mock<IAuthorizationService>();
            var service = new ProductoService(mockRepo.Object, mockAuth.Object);

            Producto? capturado = null;
            mockRepo.Setup(r => r.Agregar(It.IsAny<Producto>())).Callback<Producto>(p => capturado = p);

            var productoInvalido = new Producto
            {
                CodigoBarra = "TEST-IVA-NORM",
                Nombre = "Producto Normalizado",
                PrecioVentaActual = 1300m,
                PorcentajeIva = 0m
            };

            service.Agregar(productoInvalido);

            Assert.NotNull(capturado);
            Assert.Equal(Producto.IvaFijoDefault, capturado.PorcentajeIva);
        }

        [Fact]
        public void ProductoService_ActualizarProductoConIvaCero_RestauraIva21()
        {
            var mockRepo = new Mock<IProductoRepository>();
            var mockAuth = new Mock<IAuthorizationService>();
            var service = new ProductoService(mockRepo.Object, mockAuth.Object);

            Producto? capturado = null;
            mockRepo.Setup(r => r.Actualizar(It.IsAny<Producto>())).Callback<Producto>(p => capturado = p);

            var productoExistente = new Producto
            {
                IdProducto = 88,
                CodigoBarra = "TEST-IVA-ACT",
                Nombre = "Producto Actualizable",
                PrecioVentaActual = 1300m,
                PorcentajeIva = -10m // Inválido
            };

            mockRepo.Setup(r => r.BuscarPorCodigoBarra("TEST-IVA-ACT")).Returns(productoExistente);

            service.Actualizar(productoExistente);

            Assert.NotNull(capturado);
            Assert.Equal(Producto.IvaFijoDefault, capturado.PorcentajeIva);
        }

        [Fact]
        public void ProductoService_AgregarSinPermisos_LanzaSecurityException()
        {
            var mockRepo = new Mock<IProductoRepository>();
            var mockAuth = new Mock<IAuthorizationService>();
            mockAuth.Setup(a => a.ValidarPermiso(Permisos.PRODUCTOS_CREAR))
                    .Throws(new System.Security.SecurityException("Acceso denegado"));

            var service = new ProductoService(mockRepo.Object, mockAuth.Object);

            var producto = new Producto
            {
                CodigoBarra = "SIN-PERMISO",
                Nombre = "Producto Bloqueado",
                PrecioVentaActual = 1300m
            };

            Assert.Throws<System.Security.SecurityException>(() => service.Agregar(producto));
            mockRepo.Verify(r => r.Agregar(It.IsAny<Producto>()), Times.Never);
        }

        // =========================================================================
        // 3. TRANSPARENCIA FISCAL Y MOTOR DE COMPROBANTES (TicketService)
        // =========================================================================

        [Fact]
        public void TicketService_VentaSimpleConIva21_CalculaBaseImponibleEImpuestoSinDesfase()
        {
            var ticketService = new TicketService();
            var comercio = new DatosComercio { Nombre = "Local Test", Cuit = "30-12345678-9" };
            var pago = new DatosPago { MetodoPago = "Efectivo", MontoRecibido = 1300m, Vuelto = 0m };

            // Venta del pan de $1300 (Total = $1300)
            var venta = new Venta
            {
                IdVenta = 10,
                FechaHora = DateTime.Now,
                Subtotal = 1300m,
                DescuentoTotal = 0m,
                TotalVenta = 1300m,
                DetalleVenta = new List<DetalleVenta>
                {
                    new DetalleVenta
                    {
                        IdProducto = 1,
                        Cantidad = 1,
                        PrecioUnitarioHistorico = 1300m,
                        Descuento = 0m,
                        IdProductoNavigation = new Producto
                        {
                            Nombre = "Pan 1kg",
                            PorcentajeIva = Producto.IvaFijoDefault
                        }
                    }
                }
            };

            // Cálculo fiscal teórico:
            // Base Imponible = 1300 / 1.21 = 1074.380165... -> $1074.38
            // Monto IVA = 1300 - 1074.380165... = 225.6198... -> $225.62
            // Suma Base + IVA = $1300.00 exacto
            decimal baseImponibleEsperada = Math.Round(1300m / 1.21m, 2);
            decimal ivaEsperado = Math.Round(1300m - (1300m / 1.21m), 2);
            Assert.Equal(1300.00m, baseImponibleEsperada + ivaEsperado);

            // Generar PDF y verificar que se genera sin excepciones
            byte[] pdf = ticketService.GenerarTicketPdf(venta, "Cajero Test", comercio, pago);
            Assert.NotNull(pdf);
            Assert.True(pdf.Length > 0);
            string header = Encoding.ASCII.GetString(pdf, 0, 5);
            Assert.StartsWith("%PDF", header);
        }

        [Fact]
        public void TicketService_MultiplesProductosConIva21_AgrupaConsolidadamenteBajoAlicuota21()
        {
            var ticketService = new TicketService();
            var comercio = new DatosComercio { Nombre = "Supermercado", Cuit = "30-22222222-2" };
            var pago = new DatosPago { MetodoPago = "Tarjeta Débito", MontoRecibido = 2510m, Vuelto = 0m };

            var venta = new Venta
            {
                IdVenta = 11,
                FechaHora = DateTime.Now,
                Subtotal = 2510m,
                DescuentoTotal = 0m,
                TotalVenta = 2510m,
                DetalleVenta = new List<DetalleVenta>
                {
                    new DetalleVenta
                    {
                        IdProducto = 1,
                        Cantidad = 1,
                        PrecioUnitarioHistorico = 1300m,
                        Descuento = 0m,
                        IdProductoNavigation = new Producto { Nombre = "Pan 1kg", PorcentajeIva = 21m }
                    },
                    new DetalleVenta
                    {
                        IdProducto = 2,
                        Cantidad = 1,
                        PrecioUnitarioHistorico = 1210m,
                        Descuento = 0m,
                        IdProductoNavigation = new Producto { Nombre = "Leche 1L", PorcentajeIva = 21m }
                    }
                }
            };

            byte[] pdf = ticketService.GenerarTicketPdf(venta, "Cajero 2", comercio, pago);
            Assert.NotNull(pdf);
            Assert.True(pdf.Length > 0);
        }

        [Fact]
        public void TicketService_VentaConDescuento_CalculaIvaSobreBaseNetaPostDescuento()
        {
            var ticketService = new TicketService();
            var comercio = new DatosComercio { Nombre = "Tienda", Cuit = "30-33333333-3" };
            var pago = new DatosPago { MetodoPago = "Efectivo", MontoRecibido = 1000m, Vuelto = 0m };

            // Subtotal 1300, Descuento 300 -> Total Venta 1000
            var venta = new Venta
            {
                IdVenta = 12,
                FechaHora = DateTime.Now,
                Subtotal = 1300m,
                DescuentoTotal = 300m,
                TotalVenta = 1000m,
                DetalleVenta = new List<DetalleVenta>
                {
                    new DetalleVenta
                    {
                        IdProducto = 1,
                        Cantidad = 1,
                        PrecioUnitarioHistorico = 1300m,
                        Descuento = 300m, // Descuento aplicado directamente al item
                        IdProductoNavigation = new Producto { Nombre = "Pan 1kg", PorcentajeIva = 21m }
                    }
                }
            };

            // Base Imponible Neta = (1300 - 300) = 1000 -> 1000 / 1.21 = 826.45
            // IVA Neto = 1000 - 826.45 = 173.55
            byte[] pdf = ticketService.GenerarTicketPdf(venta, "Cajero Descuento", comercio, pago);
            Assert.NotNull(pdf);
            Assert.True(pdf.Length > 0);
        }

        [Fact]
        public void TicketService_ProductoConIvaCeroONullEnNavegacion_AplicaFallbackRobusto21()
        {
            var ticketService = new TicketService();
            var comercio = new DatosComercio { Nombre = "Almacén", Cuit = "30-44444444-4" };
            var pago = new DatosPago { MetodoPago = "Efectivo", MontoRecibido = 1300m, Vuelto = 0m };

            // Producto cuya entidad de navegación tiene PorcentajeIva = 0m (caso histórico)
            var venta = new Venta
            {
                IdVenta = 13,
                FechaHora = DateTime.Now,
                Subtotal = 1300m,
                DescuentoTotal = 0m,
                TotalVenta = 1300m,
                DetalleVenta = new List<DetalleVenta>
                {
                    new DetalleVenta
                    {
                        IdProducto = 99,
                        Cantidad = 1,
                        PrecioUnitarioHistorico = 1300m,
                        Descuento = 0m,
                        IdProductoNavigation = new Producto { Nombre = "Legacy Item", PorcentajeIva = 0m }
                    }
                }
            };

            // No debe fallar ni arrojar alícuota 0%, sino aplicar el 21% por defecto
            byte[] pdf = ticketService.GenerarTicketPdf(venta, "Cajero Fallback", comercio, pago);
            Assert.NotNull(pdf);
            Assert.True(pdf.Length > 0);
        }

        // =========================================================================
        // 4. INTEGRACIÓN UI EN FORMULARIO DE INGRESO DE STOCK (FormIngresoStock)
        // =========================================================================

        [Fact]
        public void FormIngresoStock_MargenVacioOInvalido_AplicaSoloIvaFijo21SinLanzarExcepcion()
        {
            RunInSta(() =>
            {
                var mockProdService = new Mock<IProductoService>();
                var mockStockService = new Mock<IStockService>();
                var mockCatService = new Mock<ICategoriaService>();
                var mockCompraService = new Mock<ICompraService>();
                var mockProvService = new Mock<IProveedorService>();

                mockCatService.Setup(c => c.ObtenerTodos()).Returns(new List<Categoria> { new Categoria { IdCategoria = 1, Nombre = "General" } });
                mockProvService.Setup(p => p.ObtenerTodos()).Returns(new List<Proveedor> { new Proveedor { IdProveedor = 1, RazonSocial = "Proveedor 1", Activo = true } });

                using var form = new FormIngresoStock(
                    mockProdService.Object,
                    mockStockService.Object,
                    mockCatService.Object,
                    mockCompraService.Object,
                    mockProvService.Object);

                var txtPrecioCompra = (TextBox)typeof(FormIngresoStock).GetField("txtPrecioCompra", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(form)!;
                var txtCantidad = (TextBox)typeof(FormIngresoStock).GetField("txtCantidad", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(form)!;
                var txtPorcentaje = (TextBox)typeof(FormIngresoStock).GetField("txtPorcentajeExtra", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(form)!;
                var txtPrecioVenta = (TextBox)typeof(FormIngresoStock).GetField("txtPrecioVenta", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(form)!;

                var calcularMethod = typeof(FormIngresoStock).GetMethod("CalcularValores", BindingFlags.NonPublic | BindingFlags.Instance)!;

                // 1. Margen vacío -> Costo 1000 -> Precio Final = 1210 (0% ganancia + 21% IVA)
                txtPrecioCompra.Text = "1000";
                txtCantidad.Text = "1";
                txtPorcentaje.Text = "";
                calcularMethod.Invoke(form, new object?[] { null, EventArgs.Empty });
                Assert.Equal(1210.00m, decimal.Parse(txtPrecioVenta.Text));

                // 2. Margen texto inválido -> Costo 1000 -> Precio Final = 1210
                txtPorcentaje.Text = "texto-invalido";
                calcularMethod.Invoke(form, new object?[] { null, EventArgs.Empty });
                Assert.Equal(1210.00m, decimal.Parse(txtPrecioVenta.Text));
            });
        }

        [Fact]
        public void FormIngresoStock_GuardarNuevoProducto_PersistePorcentajeIva21EnProductoService()
        {
            RunInSta(() =>
            {
                var mockProdService = new Mock<IProductoService>();
                var mockStockService = new Mock<IStockService>();
                var mockCatService = new Mock<ICategoriaService>();
                var mockCompraService = new Mock<ICompraService>();
                var mockProvService = new Mock<IProveedorService>();

                mockCatService.Setup(c => c.ObtenerTodos()).Returns(new List<Categoria> { new Categoria { IdCategoria = 1, Nombre = "General" } });
                mockProvService.Setup(p => p.ObtenerTodos()).Returns(new List<Proveedor> { new Proveedor { IdProveedor = 1, RazonSocial = "Molino", Activo = true } });

                Producto? productoGuardado = null;
                mockProdService.Setup(p => p.ObtenerTodos()).Returns(new List<Producto>()); // No existe previamente
                mockProdService.Setup(p => p.Agregar(It.IsAny<Producto>()))
                               .Callback<Producto>(p => productoGuardado = p);

                string? mensajeDialogo = null;
                FormIngresoStock.MostrarMensaje = (msg, title, btns, icon) => { mensajeDialogo = msg; };

                using var form = new FormIngresoStock(
                    mockProdService.Object,
                    mockStockService.Object,
                    mockCatService.Object,
                    mockCompraService.Object,
                    mockProvService.Object);

                var txtCodigo = (TextBox)typeof(FormIngresoStock).GetField("txtCodigo", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(form)!;
                var txtNombre = (TextBox)typeof(FormIngresoStock).GetField("txtNombreProducto", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(form)!;
                var cmbCategoria = (ComboBox)typeof(FormIngresoStock).GetField("cmbCategoria", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(form)!;
                var cmbProveedor = (ComboBox)typeof(FormIngresoStock).GetField("cmbProveedor", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(form)!;
                var txtCantidad = (TextBox)typeof(FormIngresoStock).GetField("txtCantidad", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(form)!;
                var txtPrecioCompra = (TextBox)typeof(FormIngresoStock).GetField("txtPrecioCompra", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(form)!;
                var txtPorcentaje = (TextBox)typeof(FormIngresoStock).GetField("txtPorcentajeExtra", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(form)!;

                txtCodigo.Text = "PAN-1000";
                txtNombre.Text = "1 kg de Pan";
                cmbCategoria.SelectedValue = 1;
                cmbProveedor.SelectedValue = 1;
                txtCantidad.Text = "5";
                txtPrecioCompra.Text = "1000";
                txtPorcentaje.Text = "9"; // 9% margen + 21% IVA = 30% -> $1300

                var btnGuardarClick = typeof(FormIngresoStock).GetMethod("BtnGuardar_Click", BindingFlags.NonPublic | BindingFlags.Instance)!;
                btnGuardarClick.Invoke(form, new object?[] { null, EventArgs.Empty });

                // ASSERT: El producto guardado debe tener IVA del 21% y precio de venta de $1300
                Assert.NotNull(productoGuardado);
                Assert.Equal(Producto.IvaFijoDefault, productoGuardado.PorcentajeIva);
                Assert.Equal(1300.00m, productoGuardado.PrecioVentaActual);
                Assert.NotNull(mensajeDialogo);
                Assert.Contains("Ingreso de stock y compra registrados exitosamente", mensajeDialogo);
            });
        }

        [Fact]
        public void FormIngresoStock_GuardarProductoExistente_ActualizaPorcentajeIva21EnProductoService()
        {
            RunInSta(() =>
            {
                var mockProdService = new Mock<IProductoService>();
                var mockStockService = new Mock<IStockService>();
                var mockCatService = new Mock<ICategoriaService>();
                var mockCompraService = new Mock<ICompraService>();
                var mockProvService = new Mock<IProveedorService>();

                mockCatService.Setup(c => c.ObtenerTodos()).Returns(new List<Categoria> { new Categoria { IdCategoria = 1, Nombre = "General" } });
                mockProvService.Setup(p => p.ObtenerTodos()).Returns(new List<Proveedor> { new Proveedor { IdProveedor = 1, RazonSocial = "Molino", Activo = true } });

                var prodExistente = new Producto
                {
                    IdProducto = 55,
                    CodigoBarra = "PAN-EXISTENTE",
                    Nombre = "Pan Francés",
                    IdCategoria = 1,
                    IdProveedor = 1,
                    PrecioVentaActual = 1000m,
                    PorcentajeIva = 0m // Tenía 0 antes de la actualización
                };

                mockProdService.Setup(p => p.ObtenerTodos()).Returns(new List<Producto> { prodExistente });

                Producto? productoActualizado = null;
                mockProdService.Setup(p => p.Actualizar(It.IsAny<Producto>()))
                               .Callback<Producto>(p => productoActualizado = p);

                FormIngresoStock.MostrarMensaje = (msg, title, btns, icon) => { };

                using var form = new FormIngresoStock(
                    mockProdService.Object,
                    mockStockService.Object,
                    mockCatService.Object,
                    mockCompraService.Object,
                    mockProvService.Object);

                var txtCodigo = (TextBox)typeof(FormIngresoStock).GetField("txtCodigo", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(form)!;
                var txtNombre = (TextBox)typeof(FormIngresoStock).GetField("txtNombreProducto", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(form)!;
                var cmbCategoria = (ComboBox)typeof(FormIngresoStock).GetField("cmbCategoria", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(form)!;
                var cmbProveedor = (ComboBox)typeof(FormIngresoStock).GetField("cmbProveedor", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(form)!;
                var txtCantidad = (TextBox)typeof(FormIngresoStock).GetField("txtCantidad", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(form)!;
                var txtPrecioCompra = (TextBox)typeof(FormIngresoStock).GetField("txtPrecioCompra", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(form)!;
                var txtPorcentaje = (TextBox)typeof(FormIngresoStock).GetField("txtPorcentajeExtra", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(form)!;

                txtCodigo.Text = "PAN-EXISTENTE";
                txtNombre.Text = "Pan Francés";
                cmbCategoria.SelectedValue = 1;
                cmbProveedor.SelectedValue = 1;
                txtCantidad.Text = "10";
                txtPrecioCompra.Text = "1000";
                txtPorcentaje.Text = "9"; // 9% margen + 21% IVA = $1300

                var btnGuardarClick = typeof(FormIngresoStock).GetMethod("BtnGuardar_Click", BindingFlags.NonPublic | BindingFlags.Instance)!;
                btnGuardarClick.Invoke(form, new object?[] { null, EventArgs.Empty });

                // ASSERT: El producto existente actualizado pasa a tener IVA 21% y precio de $1300
                Assert.NotNull(productoActualizado);
                Assert.Equal(Producto.IvaFijoDefault, productoActualizado.PorcentajeIva);
                Assert.Equal(1300.00m, productoActualizado.PrecioVentaActual);
            });
        }

        // =========================================================================
        // 5. INTEGRACIÓN UI EN FORMULARIO DE REGISTRO DE PRODUCTO (FormRegistroProducto)
        // =========================================================================

        [Fact]
        public void FormRegistroProducto_CrearNuevoProducto_PersisteIvaFijo21()
        {
            RunInSta(() =>
            {
                var mockProdService = new Mock<IProductoService>();
                var mockCatService = new Mock<ICategoriaService>();
                var mockStockService = new Mock<IStockService>();
                var mockCompraService = new Mock<ICompraService>();
                var mockProvService = new Mock<IProveedorService>();

                mockCatService.Setup(c => c.ObtenerTodos()).Returns(new List<Categoria> { new Categoria { IdCategoria = 1, Nombre = "Almacén" } });
                mockProvService.Setup(p => p.ObtenerTodos()).Returns(new List<Proveedor> { new Proveedor { IdProveedor = 1, RazonSocial = "Distribuidora", Activo = true } });
                mockProdService.Setup(p => p.ObtenerTodos()).Returns(new List<Producto>());

                Producto? nuevoGuardado = null;
                mockProdService.Setup(p => p.Agregar(It.IsAny<Producto>())).Callback<Producto>(p => nuevoGuardado = p);

                FormRegistroProducto.MostrarMensaje = (msg, title, btns, icon) => { };

                using var form = new FormRegistroProducto(
                    mockProdService.Object,
                    mockCatService.Object,
                    mockStockService.Object,
                    mockCompraService.Object,
                    mockProvService.Object);

                var loadMethod = typeof(FormRegistroProducto).GetMethod("FormRegistroProducto_Load", BindingFlags.NonPublic | BindingFlags.Instance)!;
                loadMethod.Invoke(form, new object?[] { null, EventArgs.Empty });

                var txtCodigo = (TextBox)typeof(FormRegistroProducto).GetField("txtCodigoBarra", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(form)!;
                var txtNombre = (TextBox)typeof(FormRegistroProducto).GetField("txtNombre", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(form)!;
                var txtPrecio = (TextBox)typeof(FormRegistroProducto).GetField("txtPrecio", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(form)!;
                var cmbCategoria = (ComboBox)typeof(FormRegistroProducto).GetField("cmbCategoria", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(form)!;
                var cmbProveedor = (ComboBox)typeof(FormRegistroProducto).GetField("cmbProveedor", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(form)!;
                var txtStock = (TextBox)typeof(FormRegistroProducto).GetField("txtStock", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(form)!;

                txtCodigo.Text = "PROD-ALTA-01";
                txtNombre.Text = "Producto Nuevo";
                txtPrecio.Text = "1300";
                cmbCategoria.SelectedValue = 1;
                cmbProveedor.SelectedValue = 1;
                txtStock.Text = "0";

                var btnGuardarClick = typeof(FormRegistroProducto).GetMethod("BtnGuardar_Click", BindingFlags.NonPublic | BindingFlags.Instance)!;
                btnGuardarClick.Invoke(form, new object?[] { null, EventArgs.Empty });

                Assert.NotNull(nuevoGuardado);
                Assert.Equal(Producto.IvaFijoDefault, nuevoGuardado.PorcentajeIva);
                Assert.Equal(1300.00m, nuevoGuardado.PrecioVentaActual);
            });
        }

        [Fact]
        public void FormRegistroProducto_ModificarProductoExistente_PersisteIvaFijo21()
        {
            RunInSta(() =>
            {
                var mockProdService = new Mock<IProductoService>();
                var mockCatService = new Mock<ICategoriaService>();
                var mockStockService = new Mock<IStockService>();
                var mockCompraService = new Mock<ICompraService>();
                var mockProvService = new Mock<IProveedorService>();

                mockCatService.Setup(c => c.ObtenerTodos()).Returns(new List<Categoria> { new Categoria { IdCategoria = 1, Nombre = "Almacén" } });
                mockProvService.Setup(p => p.ObtenerTodos()).Returns(new List<Proveedor> { new Proveedor { IdProveedor = 1, RazonSocial = "Distribuidora", Activo = true } });

                var prodOriginal = new Producto
                {
                    IdProducto = 99,
                    CodigoBarra = "PROD-MOD-01",
                    Nombre = "Producto Original",
                    PrecioVentaActual = 1000m,
                    PorcentajeIva = 0m, // Sin IVA o 0 previo
                    IdCategoria = 1,
                    IdProveedor = 1,
                    Activo = true
                };

                mockProdService.Setup(p => p.ObtenerTodos()).Returns(new List<Producto> { prodOriginal });

                Producto? modificadoGuardado = null;
                mockProdService.Setup(p => p.Actualizar(It.IsAny<Producto>())).Callback<Producto>(p => modificadoGuardado = p);

                FormRegistroProducto.MostrarMensaje = (msg, title, btns, icon) => { };

                using var form = new FormRegistroProducto(
                    mockProdService.Object,
                    mockCatService.Object,
                    mockStockService.Object,
                    mockCompraService.Object,
                    mockProvService.Object);

                var loadMethod = typeof(FormRegistroProducto).GetMethod("FormRegistroProducto_Load", BindingFlags.NonPublic | BindingFlags.Instance)!;
                loadMethod.Invoke(form, new object?[] { null, EventArgs.Empty });

                form.PrepararParaEdicion(prodOriginal);

                var txtPrecio = (TextBox)typeof(FormRegistroProducto).GetField("txtPrecio", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(form)!;
                txtPrecio.Text = "1300"; // Actualizar precio a 1300

                var btnGuardarClick = typeof(FormRegistroProducto).GetMethod("BtnGuardar_Click", BindingFlags.NonPublic | BindingFlags.Instance)!;
                btnGuardarClick.Invoke(form, new object?[] { null, EventArgs.Empty });

                Assert.NotNull(modificadoGuardado);
                Assert.Equal(Producto.IvaFijoDefault, modificadoGuardado.PorcentajeIva);
                Assert.Equal(1300.00m, modificadoGuardado.PrecioVentaActual);
            });
        }

        // =========================================================================
        // 6. INTEGRACIÓN EN PUNTO DE VENTA Y CARRITO (UcVentas)
        // =========================================================================

        [Fact]
        public void UcVentas_AgregarProductoConIvaCero_NormalizaA21EnTrackingInterno()
        {
            RunInSta(() =>
            {
                var mockProdService = new Mock<IProductoService>();
                var mockVentaService = new Mock<IVentaService>();
                var mockCajaService = new Mock<ICajaService>();
                var mockTicketService = new Mock<ITicketService>();
                var mockConfigService = new Mock<IConfiguracionService>();
                var mockStockService = new Mock<IStockService>();

                mockStockService.Setup(s => s.ObtenerCantidadActual(It.IsAny<int>(), It.IsAny<int>())).Returns(100);

                using var ucVentas = new UcVentas(
                    mockProdService.Object,
                    mockVentaService.Object,
                    mockCajaService.Object,
                    mockTicketService.Object,
                    mockConfigService.Object,
                    mockStockService.Object);

                var productoLegacy = new Producto
                {
                    IdProducto = 77,
                    Nombre = "Producto Legacy",
                    PrecioVentaActual = 1300m,
                    PorcentajeIva = 0m // Producto histórico con 0
                };

                var agregarMethod = typeof(UcVentas).GetMethod("AgregarProductoAlTicket", BindingFlags.NonPublic | BindingFlags.Instance)!;
                agregarMethod.Invoke(ucVentas, new object[] { productoLegacy });

                var ivaDiccionario = (Dictionary<int, decimal>)typeof(UcVentas).GetField("_ivaProductos", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(ucVentas)!;

                // ASSERT: El tracking interno debe haber normalizado el IVA a 21m
                Assert.True(ivaDiccionario.ContainsKey(77));
                Assert.Equal(Producto.IvaFijoDefault, ivaDiccionario[77]);
            });
        }

        [Fact]
        public void UcVentas_MultiplesProductos_MantieneIvaPorProductoSinMezclar()
        {
            RunInSta(() =>
            {
                var mockProdService = new Mock<IProductoService>();
                var mockVentaService = new Mock<IVentaService>();
                var mockCajaService = new Mock<ICajaService>();
                var mockTicketService = new Mock<ITicketService>();
                var mockConfigService = new Mock<IConfiguracionService>();
                var mockStockService = new Mock<IStockService>();

                mockStockService.Setup(s => s.ObtenerCantidadActual(It.IsAny<int>(), It.IsAny<int>())).Returns(100);

                using var ucVentas = new UcVentas(
                    mockProdService.Object,
                    mockVentaService.Object,
                    mockCajaService.Object,
                    mockTicketService.Object,
                    mockConfigService.Object,
                    mockStockService.Object);

                var prod1 = new Producto { IdProducto = 10, Nombre = "Pan", PrecioVentaActual = 1300m, PorcentajeIva = 21m };
                var prod2 = new Producto { IdProducto = 20, Nombre = "Leche", PrecioVentaActual = 1210m, PorcentajeIva = 21m };

                var agregarMethod = typeof(UcVentas).GetMethod("AgregarProductoAlTicket", BindingFlags.NonPublic | BindingFlags.Instance)!;
                agregarMethod.Invoke(ucVentas, new object[] { prod1 });
                agregarMethod.Invoke(ucVentas, new object[] { prod2 });

                var ivaDiccionario = (Dictionary<int, decimal>)typeof(UcVentas).GetField("_ivaProductos", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(ucVentas)!;

                // ASSERT: Ambos productos deben estar registrados en el diccionario de IVA con 21%
                Assert.Equal(2, ivaDiccionario.Count);
                Assert.Equal(Producto.IvaFijoDefault, ivaDiccionario[10]);
                Assert.Equal(Producto.IvaFijoDefault, ivaDiccionario[20]);
            });
        }
    }
}
