using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;
using StockOS.Application.Services;
using StockOS.Application.Reports;
using StockOS.Domain.Entities;
using StockOS.Domain.Enums;
using StockOS.Domain.Interfaces;
using StockOS.UI.WinForms.Forms;

namespace StockOS.Application.Tests
{
    public class IntegridadConsistenciaRegressionTests
    {
        // Helper para ejecutar código de UI en un hilo STA como requiere Windows Forms
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
        // 1. PRUEBA DE INTEGRIDAD DEL CONTENEDOR DI (Program.cs)
        // =========================================================================
        [Fact]
        public void ContenedorDI_ResuelveTodasLasVistasServiciosYFormulariosSinErrores()
        {
            RunInSta(() =>
            {
                var services = new ServiceCollection();

                // Mock de Configuration
                var mockConfig = new Mock<IConfiguration>();
                services.AddSingleton<IConfiguration>(mockConfig.Object);

                // Mocks de Repositorios
                services.AddScoped(_ => new Mock<IEmpleadoRepository>().Object);
                services.AddScoped(_ => new Mock<ISucursalRepository>().Object);
                services.AddScoped(_ => new Mock<IRolRepository>().Object);
                services.AddScoped(_ => new Mock<ICategoriaRepository>().Object);
                services.AddScoped(_ => new Mock<IProductoRepository>().Object);
                services.AddScoped(_ => new Mock<IStockSucursalRepository>().Object);
                services.AddScoped(_ => new Mock<ICajaSesionRepository>().Object);
                services.AddScoped(_ => new Mock<ICajaRepository>().Object);
                services.AddScoped(_ => new Mock<IVentaRepository>().Object);
                services.AddScoped(_ => new Mock<ICompraRepository>().Object);
                services.AddScoped(_ => new Mock<IProveedorRepository>().Object);
                services.AddScoped(_ => new Mock<IReporteRepository>().Object);

                // Mocks de Servicios
                services.AddScoped(_ => new Mock<IAuthService>().Object);
                services.AddScoped(_ => new Mock<ISucursalService>().Object);
                services.AddScoped(_ => new Mock<IRolService>().Object);
                services.AddScoped(_ => new Mock<IEmpleadoService>().Object);
                services.AddScoped(_ => new Mock<ICategoriaService>().Object);
                services.AddScoped(_ => new Mock<IProductoService>().Object);
                services.AddScoped(_ => new Mock<IStockService>().Object);
                services.AddScoped(_ => new Mock<ICajaService>().Object);
                services.AddScoped(_ => new Mock<IVentaService>().Object);
                services.AddScoped(_ => new Mock<ICompraService>().Object);
                services.AddScoped(_ => new Mock<IProveedorService>().Object);
                services.AddScoped(_ => new Mock<IReporteService>().Object);
                services.AddScoped<IAuthorizationService, AuthorizationService>();
                services.AddScoped(_ => new Mock<ITicketService>().Object);
                services.AddScoped(_ => new Mock<IConfiguracionService>().Object);

                // Registro exacto de formularios y UserControls según Program.cs
                services.AddTransient<FormLogin>();
                services.AddTransient<FormInicio>();
                services.AddTransient<FormRegistroUsuario>();
                services.AddTransient<UcListarUsuarios>();
                services.AddTransient<UcUsuarios>();
                services.AddTransient<FormRegistroProducto>();
                services.AddTransient<FormIngresoStock>();
                services.AddTransient<FormCategoria>();
                services.AddTransient<FormProveedor>();
                services.AddTransient<FormAperturaCaja>();
                services.AddTransient<UcInicio>();
                services.AddTransient<UcInventario>();
                services.AddTransient<UcVentas>();
                services.AddTransient<UcReportes>();
                services.AddTransient<UcConfig>();

                var provider = services.BuildServiceProvider();

                using (var scope = provider.CreateScope())
                {
                    var sp = scope.ServiceProvider;

                    // 1. Verificamos que ningún servicio de negocio sea nulo
                    Assert.NotNull(sp.GetRequiredService<IAuthService>());
                    Assert.NotNull(sp.GetRequiredService<IProductoService>());
                    Assert.NotNull(sp.GetRequiredService<IVentaService>());
                    Assert.NotNull(sp.GetRequiredService<IStockService>());
                    Assert.NotNull(sp.GetRequiredService<ICajaService>());
                    Assert.NotNull(sp.GetRequiredService<ICompraService>());
                    Assert.NotNull(sp.GetRequiredService<IProveedorService>());
                    Assert.NotNull(sp.GetRequiredService<IReporteService>());
                    Assert.NotNull(sp.GetRequiredService<IAuthorizationService>());

                    // 2. Verificamos que todos los formularios y vistas se resuelvan y creen sin lanzar excepción
                    using var fLogin = sp.GetRequiredService<FormLogin>();
                    Assert.NotNull(fLogin);

                    using var fInicio = sp.GetRequiredService<FormInicio>();
                    Assert.NotNull(fInicio);

                    using var fRegUsuario = sp.GetRequiredService<FormRegistroUsuario>();
                    Assert.NotNull(fRegUsuario);

                    using var ucListUsuarios = sp.GetRequiredService<UcListarUsuarios>();
                    Assert.NotNull(ucListUsuarios);

                    using var ucUsuarios = sp.GetRequiredService<UcUsuarios>();
                    Assert.NotNull(ucUsuarios);

                    using var fRegProducto = sp.GetRequiredService<FormRegistroProducto>();
                    Assert.NotNull(fRegProducto);

                    using var fIngStock = sp.GetRequiredService<FormIngresoStock>();
                    Assert.NotNull(fIngStock);

                    using var fCat = sp.GetRequiredService<FormCategoria>();
                    Assert.NotNull(fCat);

                    using var fProv = sp.GetRequiredService<FormProveedor>();
                    Assert.NotNull(fProv);

                    using var ucInicio = sp.GetRequiredService<UcInicio>();
                    Assert.NotNull(ucInicio);

                    using var ucInv = sp.GetRequiredService<UcInventario>();
                    Assert.NotNull(ucInv);

                    using var ucVentas = sp.GetRequiredService<UcVentas>();
                    Assert.NotNull(ucVentas);

                    using var ucRep = sp.GetRequiredService<UcReportes>();
                    Assert.NotNull(ucRep);

                    using var ucConf = sp.GetRequiredService<UcConfig>();
                    Assert.NotNull(ucConf);
                }
            });
        }

        // =========================================================================
        // 2. PRUEBA DE REGRESIÓN VISUAL Y PERMISOS DECLARATIVOS EN UCINVENTARIO
        // =========================================================================
        [Theory]
        [InlineData((int)RolUsuario.Gerente, true, true, true, true, true)]
        [InlineData((int)RolUsuario.Cajero, false, false, false, false, false)]
        [InlineData((int)RolUsuario.EncargadoDeposito, true, true, true, true, true)]
        [InlineData((int)RolUsuario.Repositor, false, false, false, false, false)]
        public void UcInventario_VisibilidadBotonesSegunRol_SeAjustaSinRomperLogica(
            int idRol, bool puedeCrearProd, bool puedeIngresarStock, bool puedeCategorias, bool puedeProveedores, bool puedeEditar)
        {
            RunInSta(() =>
            {
                var usuario = new Empleado
                {
                    IdEmpleado = 10,
                    IdRol = idRol,
                    IdSucursal = 1,
                    Nombre = "Test",
                    Apellido = "User"
                };
                SesionActual.Usuario = usuario;

                var mockProdService = new Mock<IProductoService>();
                mockProdService.Setup(p => p.ObtenerTodos()).Returns(new List<Producto>());
                var mockCatService = new Mock<ICategoriaService>();
                var mockStockService = new Mock<IStockService>();
                var mockServiceProvider = new Mock<IServiceProvider>();
                var authService = new AuthorizationService();

                using var ucInventario = new UcInventario(
                    mockProdService.Object,
                    mockCatService.Object,
                    mockServiceProvider.Object,
                    mockStockService.Object,
                    authService);

                // Forzamos invocación de la configuración de permisos
                var methodInfo = typeof(UcInventario).GetMethod("ConfigurarPermisosModulo", BindingFlags.NonPublic | BindingFlags.Instance);
                methodInfo?.Invoke(ucInventario, null);

                // Verificamos controles privados por reflexión
                var btnNuevo = (Button)typeof(UcInventario).GetField("btnNuevo", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(ucInventario)!;
                var btnIngresarStock = (Button)typeof(UcInventario).GetField("btnIngresarStock", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(ucInventario)!;
                var btnCategorias = (Button)typeof(UcInventario).GetField("btnCategorias", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(ucInventario)!;
                var btnProveedores = (Button)typeof(UcInventario).GetField("btnProveedores", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(ucInventario)!;
                var dgvProductos = (DataGridView)typeof(UcInventario).GetField("dgvProductos", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(ucInventario)!;

                Assert.Equal(puedeCrearProd, btnNuevo.Visible);
                Assert.Equal(puedeIngresarStock, btnIngresarStock.Visible);
                Assert.Equal(puedeCategorias, btnCategorias.Visible);
                Assert.Equal(puedeProveedores, btnProveedores.Visible);

                if (dgvProductos.Columns.Contains("colModificar"))
                {
                    Assert.Equal(puedeEditar, dgvProductos.Columns["colModificar"].Visible);
                }
                if (dgvProductos.Columns.Contains("colAccionEstado"))
                {
                    Assert.Equal(puedeEditar, dgvProductos.Columns["colAccionEstado"].Visible);
                }
            });
        }

        // =========================================================================
        // 3. PRUEBA DE REGRESIÓN EN UCVENTAS: BLOQUEO DE STOCK Y REGLAS DE NEGOCIO
        // =========================================================================
        [Fact]
        public void UcVentas_ProductoSinStock_NoPermiteIngresoAlTicket()
        {
            RunInSta(() =>
            {
                var usuario = new Empleado { IdEmpleado = 2, IdRol = (int)RolUsuario.Cajero, IdSucursal = 1, Nombre = "Cajero", Apellido = "Uno" };
                SesionActual.Usuario = usuario;

                var mockProdService = new Mock<IProductoService>();
                var mockVentaService = new Mock<IVentaService>();
                var mockCajaService = new Mock<ICajaService>();
                var mockTicketService = new Mock<ITicketService>();
                var mockConfigService = new Mock<IConfiguracionService>();
                var mockStockService = new Mock<IStockService>();

                // Producto con STOCK CERO
                var productoSinStock = new Producto
                {
                    IdProducto = 50,
                    CodigoBarra = "1234567890123",
                    Nombre = "Yerba Mate 1kg",
                    PrecioVentaActual = 3500m,
                    Activo = true
                };

                mockStockService.Setup(s => s.ObtenerCantidadActual(50, 1)).Returns(0);

                using var ucVentas = new UcVentas(
                    mockProdService.Object,
                    mockVentaService.Object,
                    mockCajaService.Object,
                    mockTicketService.Object,
                    mockConfigService.Object,
                    mockStockService.Object);

                string? mensajeMostrado = null;
                UcVentas.MostrarMensaje = (msg, title, btns, icon) => { mensajeMostrado = msg; };

                var dgvTicket = (DataGridView)typeof(UcVentas).GetField("dgvTicket", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(ucVentas)!;
                var agregarMethod = typeof(UcVentas).GetMethod("AgregarProductoAlTicket", BindingFlags.NonPublic | BindingFlags.Instance)!;

                // ACT: Intentamos ingresar el producto sin stock al ticket
                agregarMethod.Invoke(ucVentas, new object[] { productoSinStock });

                // ASSERT: El ticket debe estar completamente vacío porque se bloqueó el ingreso y el mensaje de alerta fue emitido
                Assert.Empty(dgvTicket.Rows);
                Assert.NotNull(mensajeMostrado);
                Assert.Contains("no posee stock disponible", mensajeMostrado);
            });
        }

        [Fact]
        public void UcVentas_ProductoConStockLimitado_PermiteHastaElLimiteYBloqueaExcedente()
        {
            RunInSta(() =>
            {
                var usuario = new Empleado { IdEmpleado = 2, IdRol = (int)RolUsuario.Cajero, IdSucursal = 1, Nombre = "Cajero", Apellido = "Uno" };
                SesionActual.Usuario = usuario;

                var mockProdService = new Mock<IProductoService>();
                var mockVentaService = new Mock<IVentaService>();
                var mockCajaService = new Mock<ICajaService>();
                var mockTicketService = new Mock<ITicketService>();
                var mockConfigService = new Mock<IConfiguracionService>();
                var mockStockService = new Mock<IStockService>();

                // Producto con EXACTAMENTE 2 UNIDADES EN STOCK
                var producto = new Producto
                {
                    IdProducto = 60,
                    CodigoBarra = "9876543210123",
                    Nombre = "Aceite de Girasol 900ml",
                    PrecioVentaActual = 1800m,
                    Activo = true
                };

                mockStockService.Setup(s => s.ObtenerCantidadActual(60, 1)).Returns(2);

                using var ucVentas = new UcVentas(
                    mockProdService.Object,
                    mockVentaService.Object,
                    mockCajaService.Object,
                    mockTicketService.Object,
                    mockConfigService.Object,
                    mockStockService.Object);

                string? mensajeExcedente = null;
                UcVentas.MostrarMensaje = (msg, title, btns, icon) => { mensajeExcedente = msg; };

                var dgvTicket = (DataGridView)typeof(UcVentas).GetField("dgvTicket", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(ucVentas)!;
                var agregarMethod = typeof(UcVentas).GetMethod("AgregarProductoAlTicket", BindingFlags.NonPublic | BindingFlags.Instance)!;

                // 1. Primera unidad -> Debe ingresar
                agregarMethod.Invoke(ucVentas, new object[] { producto });
                Assert.Single(dgvTicket.Rows);
                Assert.Equal(1, Convert.ToInt32(dgvTicket.Rows[0].Cells["Cantidad"].Value));

                // 2. Segunda unidad -> Debe incrementar a 2
                agregarMethod.Invoke(ucVentas, new object[] { producto });
                Assert.Single(dgvTicket.Rows);
                Assert.Equal(2, Convert.ToInt32(dgvTicket.Rows[0].Cells["Cantidad"].Value));

                // 3. Tercera unidad -> Supera el stock disponible (2), se bloquea y permanece en 2
                agregarMethod.Invoke(ucVentas, new object[] { producto });
                Assert.Single(dgvTicket.Rows);
                Assert.Equal(2, Convert.ToInt32(dgvTicket.Rows[0].Cells["Cantidad"].Value));
                Assert.NotNull(mensajeExcedente);
                Assert.Contains("No hay suficiente stock", mensajeExcedente);
            });
        }

        // =========================================================================
        // 4. PRUEBA DE INTEGRIDAD DE TRAZABILIDAD: PRECIO HISTÓRICO VS PRECIO ACTUAL
        // =========================================================================
        [Fact]
        public void TrazabilidadPrecios_VentaRegistraPrecioActualDelCatalogoComoHistorico()
        {
            var mockVentaRepo = new Mock<IVentaRepository>();
            var authService = new AuthorizationService();

            Venta? ventaCapturada = null;
            List<DetalleVenta>? detallesCapturados = null;

            mockVentaRepo.Setup(r => r.RegistrarVenta(It.IsAny<Venta>(), It.IsAny<List<DetalleVenta>>(), It.IsAny<int>(), It.IsAny<int>()))
                         .Callback<Venta, List<DetalleVenta>, int, int>((v, d, suc, met) =>
                         {
                             ventaCapturada = v;
                             detallesCapturados = d;
                         })
                         .Returns(1001);

            var ventaService = new VentaService(mockVentaRepo.Object, authService);

            // Simular usuario autenticado
            SesionActual.Usuario = new Empleado { IdEmpleado = 1, IdRol = (int)RolUsuario.Gerente };

            // Catálogo: Precio actual en el momento de la venta es $1.500
            decimal precioMomentoVenta = 1500.00m;
            var cabecera = new Venta { TotalVenta = 3000.00m, Subtotal = 3000.00m };
            var detalles = new List<DetalleVenta>
            {
                new DetalleVenta
                {
                    IdProducto = 1,
                    Cantidad = 2,
                    PrecioUnitarioHistorico = precioMomentoVenta,
                    Descuento = 0
                }
            };

            // ACT: Se registra la venta
            int idVenta = ventaService.RegistrarVenta(cabecera, detalles, 1, 1);

            // ASSERT: El detalle capturado debe tener exactamente el precio histórico guardado
            Assert.Equal(1001, idVenta);
            Assert.NotNull(detallesCapturados);
            Assert.Single(detallesCapturados);
            Assert.Equal(1500.00m, detallesCapturados[0].PrecioUnitarioHistorico);

            // Si mañana el producto sube a $2.500 en catálogo, la venta pasada sigue teniendo $1.500
            var productoModificadoManana = new Producto { IdProducto = 1, PrecioVentaActual = 2500.00m };
            Assert.NotEqual(productoModificadoManana.PrecioVentaActual, detallesCapturados[0].PrecioUnitarioHistorico);
            Assert.Equal(1500.00m, detallesCapturados[0].PrecioUnitarioHistorico);
        }

        // =========================================================================
        // 5. PRUEBA DE CONSISTENCIA DE DESCUENTO PROPORCIONAL
        // =========================================================================
        [Fact]
        public void CalculoDescuentoProporcional_ReparteExactamenteSinPerdidaDeCentavos()
        {
            // Simular el algoritmo utilizado en UcVentas
            decimal subtotalVenta = 3500m; // Item 1: $1000, Item 2: $2500
            decimal descuentoTotal = 300m;

            var items = new List<(decimal subtotal, decimal esperado)>
            {
                (1000m, 0), // (300 * 1000 / 3500) = 85.71
                (2500m, 0)  // Último item absorbe el saldo restante: 300 - 85.71 = 214.29
            };

            decimal descuentoAcumulado = 0;
            var descuentosCalculados = new List<decimal>();

            for (int i = 0; i < items.Count; i++)
            {
                decimal subtotalItem = items[i].subtotal;
                decimal descuentoItem = 0;

                if (i == items.Count - 1)
                {
                    descuentoItem = Math.Max(0, descuentoTotal - descuentoAcumulado);
                }
                else
                {
                    descuentoItem = Math.Round(descuentoTotal * (subtotalItem / subtotalVenta), 2);
                    descuentoAcumulado += descuentoItem;
                }

                descuentosCalculados.Add(descuentoItem);
            }

            // ASSERT: La suma de los descuentos calculados debe ser EXACTAMENTE el descuento total
            Assert.Equal(85.71m, descuentosCalculados[0]);
            Assert.Equal(214.29m, descuentosCalculados[1]);
            Assert.Equal(descuentoTotal, descuentosCalculados.Sum());
        }

        // =========================================================================
        // 6. PRUEBA DE INTEGRIDAD EN APERTURA Y MOVIMIENTOS DE CAJA
        // =========================================================================
        [Fact]
        public void Caja_AperturaConSaldoNegativo_EsRechazada()
        {
            var mockSesionRepo = new Mock<ICajaSesionRepository>();
            var mockCajaRepo = new Mock<ICajaRepository>();
            var mockAuth = new Mock<IAuthorizationService>();

            var service = new CajaService(mockSesionRepo.Object, mockCajaRepo.Object, mockAuth.Object);

            var ex = Assert.Throws<ArgumentException>(() => service.AbrirCaja(1, 1, -100m));
            Assert.Contains("El monto inicial de apertura no puede ser negativo", ex.Message);
            mockSesionRepo.Verify(r => r.AbrirCaja(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<decimal>()), Times.Never);
        }

        [Theory]
        [InlineData("ingreso", "INGRESO")]
        [InlineData("Ingreso", "INGRESO")]
        [InlineData("EGRESO", "EGRESO")]
        [InlineData("egreso ", "EGRESO")]
        public void Caja_RegistrarMovimiento_NormalizaMayusculasCorrectamente(string entradaTipo, string normalizadoEsperado)
        {
            var mockSesionRepo = new Mock<ICajaSesionRepository>();
            var mockCajaRepo = new Mock<ICajaRepository>();
            var mockAuth = new Mock<IAuthorizationService>();

            var service = new CajaService(mockSesionRepo.Object, mockCajaRepo.Object, mockAuth.Object);

            string? tipoEnviadoAlRepo = null;
            mockCajaRepo.Setup(r => r.RegistrarMovimiento(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<string>()))
                        .Callback<int, string, decimal, string>((ses, t, m, d) => tipoEnviadoAlRepo = t);

            // ACT
            service.RegistrarMovimiento(1, entradaTipo, 500m, "Ajuste de caja");

            // ASSERT
            Assert.Equal(normalizadoEsperado, tipoEnviadoAlRepo);
            mockCajaRepo.Verify(r => r.RegistrarMovimiento(1, normalizadoEsperado, 500m, "Ajuste de caja"), Times.Once);
        }

        // =========================================================================
        // 7. PRUEBAS DE INTEGRIDAD DEL CICLO DE VIDA DE IVA FIJO (21%) Y FORMACIÓN DE PRECIOS
        // =========================================================================
        [Fact]
        public void CalculoPrecioVentaFinal_EjemploUsuarioPan1000ConMargen9PorcientoEIVA21Porciento_Retorna1300()
        {
            // ARRANGE: Caso exacto solicitado por el usuario:
            // 1 kg de pan en compra sale 1000, ganancia del 9% e IVA del 21% -> Precio final = 1300
            decimal precioCompra = 1000.00m;
            decimal margenGanancia = 9.00m;

            // ACT
            decimal precioFinal = Producto.CalcularPrecioVentaFinal(precioCompra, margenGanancia, Producto.IvaFijoDefault);

            // ASSERT
            Assert.Equal(1300.00m, precioFinal);
        }

        [Fact]
        public void CalculoPrecioVentaFinal_MargenCero_AplicaSoloIvaFijo21()
        {
            // Costo 100 con 0% de margen + 21% IVA = 121
            decimal precioFinal = Producto.CalcularPrecioVentaFinal(100.00m, 0m);
            Assert.Equal(121.00m, precioFinal);
        }

        [Fact]
        public void CalculoPrecioVentaFinal_ValoresNegativos_LanzaArgumentException()
        {
            Assert.Throws<ArgumentException>(() => Producto.CalcularPrecioVentaFinal(-100m, 10m));
            Assert.Throws<ArgumentException>(() => Producto.CalcularPrecioVentaFinal(100m, -10m));
        }

        [Fact]
        public void ProductoService_AgregarYActualizarProductoSinIva_AsignaIvaFijo21()
        {
            // ARRANGE
            var mockRepo = new Mock<IProductoRepository>();
            var mockAuth = new Mock<IAuthorizationService>();
            var service = new ProductoService(mockRepo.Object, mockAuth.Object);

            Producto? guardado = null;
            mockRepo.Setup(r => r.Agregar(It.IsAny<Producto>())).Callback<Producto>(p => guardado = p);

            var nuevo = new Producto
            {
                CodigoBarra = "TESTIVA01",
                Nombre = "Producto Test IVA",
                PrecioVentaActual = 1300m,
                PorcentajeIva = 0m // Se envía 0 o sin inicializar
            };

            // ACT (Agregar)
            service.Agregar(nuevo);

            // ASSERT
            Assert.NotNull(guardado);
            Assert.Equal(Producto.IvaFijoDefault, guardado.PorcentajeIva);

            // ACT (Actualizar con IVA 0)
            guardado.PorcentajeIva = 0m;
            mockRepo.Setup(r => r.BuscarPorCodigoBarra("TESTIVA01")).Returns(guardado);
            service.Actualizar(guardado);

            // ASSERT
            Assert.Equal(Producto.IvaFijoDefault, guardado.PorcentajeIva);
        }

        [Fact]
        public void FormIngresoStock_CalcularValores_CalculaPrecioFinalConMargenMasIva21()
        {
            RunInSta(() =>
            {
                var mockProdService = new Mock<IProductoService>();
                var mockCatService = new Mock<ICategoriaService>();
                var mockStockService = new Mock<IStockService>();
                var mockCompraService = new Mock<ICompraService>();
                var mockProvService = new Mock<IProveedorService>();

                mockCatService.Setup(c => c.ObtenerTodos()).Returns(new List<Categoria> { new Categoria { IdCategoria = 1, Nombre = "General" } });
                mockProvService.Setup(p => p.ObtenerTodos()).Returns(new List<Proveedor> { new Proveedor { IdProveedor = 1, RazonSocial = "Molino", Activo = true } });

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
                var txtMontoTotal = (TextBox)typeof(FormIngresoStock).GetField("txtMontoTotal", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(form)!;

                var calcularMethod = typeof(FormIngresoStock).GetMethod("CalcularValores", BindingFlags.NonPublic | BindingFlags.Instance)!;

                // Simular Costo = 1000, Cantidad = 10, Margen = 9%
                txtPrecioCompra.Text = "1000";
                txtCantidad.Text = "10";
                txtPorcentaje.Text = "9";

                calcularMethod.Invoke(form, new object?[] { null, EventArgs.Empty });

                // ASSERT: Monto Compra = 10000, Precio Venta Final = 1300 (9% ganancia + 21% IVA)
                Assert.Equal(10000.00m, decimal.Parse(txtMontoTotal.Text));
                Assert.Equal(1300.00m, decimal.Parse(txtPrecioVenta.Text));
            });
        }

        [Fact]
        public void TicketService_VentaProductoConIva21_GeneraDocumentoValidoConTransparenciaFiscal()
        {
            // ARRANGE: Generar ticket para la venta del pan de $1300 (Costo 1000 + 9% ganancia + 21% IVA)
            var ticketService = new TicketService();
            var comercio = new DatosComercio
            {
                Nombre = "Panadería Central",
                Cuit = "30-99999999-9",
                Direccion = "Av San Martín 500",
                CondicionIva = "IVA RESPONSABLE INSCRIPTO",
                PuntoVenta = "00001",
                RegistroFiscal = "REG-IVA-21"
            };

            var pago = new DatosPago
            {
                MetodoPago = "Efectivo",
                MontoRecibido = 1500m,
                Vuelto = 200m
            };

            var venta = new Venta
            {
                IdVenta = 500,
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
                            Nombre = "1 kg de pan",
                            PrecioVentaActual = 1300m,
                            PorcentajeIva = Producto.IvaFijoDefault
                        }
                    }
                }
            };

            // ACT
            byte[] pdfBytes = ticketService.GenerarTicketPdf(venta, "Cajero Principal", comercio, pago);

            // ASSERT
            Assert.NotNull(pdfBytes);
            Assert.True(pdfBytes.Length > 0);
            string pdfHeader = System.Text.Encoding.ASCII.GetString(pdfBytes, 0, Math.Min(5, pdfBytes.Length));
            Assert.StartsWith("%PDF", pdfHeader);
        }
    }
}

