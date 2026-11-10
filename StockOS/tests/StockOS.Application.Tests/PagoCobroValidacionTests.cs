using System;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using StockOS.Application.Services;
using StockOS.UI.WinForms.Forms;
using Xunit;

namespace StockOS.Application.Tests
{
    public class ValidadorPagoTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(1309.99)]
        [InlineData(1309.994)]
        public void Efectivo_MontoMenorAlTotal_EsInvalido(double monto)
        {
            var r = ValidadorPago.Validar("Efectivo", 1310m, (decimal)monto);

            Assert.False(r.EsValido);
            Assert.Equal("Monto Insuficiente", r.Titulo);
            Assert.Contains("menor al total", r.Mensaje);
        }

        [Theory]
        [InlineData(1310, 0)]
        [InlineData(1310.004, 0)]
        [InlineData(2000, 690)]
        public void Efectivo_MontoIgualOMayor_EsValidoYCalculaVuelto(double monto, double vueltoEsperado)
        {
            var r = ValidadorPago.Validar("Efectivo", 1310m, (decimal)monto);

            Assert.True(r.EsValido);
            Assert.Equal((decimal)vueltoEsperado, r.Vuelto);
        }

        [Fact]
        public void Efectivo_CampoVacio_SeAsumePagoExacto()
        {
            var r = ValidadorPago.Validar("Efectivo", 1310m, null);

            Assert.True(r.EsValido);
            Assert.Equal(1310m, r.MontoRecibido);
            Assert.Equal(0m, r.Vuelto);
        }

        [Fact]
        public void Efectivo_MontoNegativoOExcesivo_EsInvalido()
        {
            Assert.False(ValidadorPago.Validar("Efectivo", 1310m, -5m).EsValido);
            Assert.False(ValidadorPago.Validar("Efectivo", 1310m, ValidadorPago.MontoMaximo + 1m).EsValido);
        }

        [Theory]
        [InlineData("Tarjeta de Débito")]
        [InlineData("Tarjeta de Crédito")]
        [InlineData("Mercado Pago")]
        public void Electronico_SinMontoOMontoExacto_EsValidoSinVuelto(string metodo)
        {
            var sinMonto = ValidadorPago.Validar(metodo, 1310m, null);
            var exacto = ValidadorPago.Validar(metodo, 1310m, 1310m);

            Assert.True(sinMonto.EsValido);
            Assert.True(exacto.EsValido);
            Assert.Equal(1310m, sinMonto.MontoRecibido);
            Assert.Equal(0m, exacto.Vuelto);
        }

        [Theory]
        [InlineData("Tarjeta de Débito", 0)]
        [InlineData("Tarjeta de Crédito", 500)]
        [InlineData("Mercado Pago", 0)]
        public void Electronico_ConMontoMenorEscrito_EsInvalidoPorInsuficiente(string metodo, double monto)
        {
            var r = ValidadorPago.Validar(metodo, 1310m, (decimal)monto);

            Assert.False(r.EsValido);
            Assert.Equal("Monto Insuficiente", r.Titulo);
        }

        [Fact]
        public void Electronico_ConMontoMayorEscrito_EsInvalidoPorInconsistente()
        {
            var r = ValidadorPago.Validar("Mercado Pago", 1310m, 2000m);

            Assert.False(r.EsValido);
            Assert.Equal("Monto Inconsistente", r.Titulo);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void MetodoPagoVacio_EsInvalido(string? metodo)
        {
            Assert.False(ValidadorPago.Validar(metodo, 1310m, 1310m).EsValido);
        }

        [Fact]
        public void TotalNegativo_EsInvalido()
        {
            Assert.False(ValidadorPago.Validar("Efectivo", -1m, 10m).EsValido);
        }
    }

    public class FormCobroValidacionTests
    {
        private static void RunInSta(Action action)
        {
            Exception? threadEx = null;
            var thread = new Thread(() =>
            {
                try { action(); }
                catch (Exception ex) { threadEx = ex; }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();

            if (threadEx != null)
            {
                throw new AggregateException("Error en ejecución de hilo STA WinForms", threadEx);
            }
        }

        private static void Click(FormCobro form, string metodo) =>
            typeof(FormCobro).GetMethod(metodo, BindingFlags.NonPublic | BindingFlags.Instance)!
                .Invoke(form, new object?[] { null, EventArgs.Empty });

        private static void SetMonto(FormCobro form, string texto)
        {
            var txt = (TextBox)typeof(FormCobro).GetField("txtMontoRecibido", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(form)!;
            txt.Text = texto;
        }

        [Theory]
        [InlineData("btnEfectivo_Click")]
        [InlineData("btnTarjetaDebito_Click")]
        [InlineData("btnTarjetaCredito_Click")]
        [InlineData("btnMercadoPago_Click")]
        public void CualquierMetodo_ConCeroEscrito_NoCierraConOkYMuestraMensaje(string boton)
        {
            RunInSta(() =>
            {
                string? mensaje = null;
                FormCobro.MostrarMensaje = (msg, title, btns, icon) => mensaje = msg;

                using var form = new FormCobro(1310m);
                SetMonto(form, "0");
                Click(form, boton);

                Assert.NotNull(mensaje);
                Assert.Contains("menor al total", mensaje);
                Assert.NotEqual(DialogResult.OK, form.DialogResult);
                Assert.Equal("", form.MetodoPagoSeleccionado);
            });
        }

        [Fact]
        public void Efectivo_ConMontoMayor_CierraConOkYCalculaVuelto()
        {
            RunInSta(() =>
            {
                string? mensaje = null;
                FormCobro.MostrarMensaje = (msg, title, btns, icon) => mensaje = msg;

                using var form = new FormCobro(1310m);
                SetMonto(form, "2000");
                Click(form, "btnEfectivo_Click");

                Assert.Null(mensaje);
                Assert.Equal(DialogResult.OK, form.DialogResult);
                Assert.Equal("Efectivo", form.MetodoPagoSeleccionado);
                Assert.Equal(2000m, form.MontoRecibido);
                Assert.Equal(690m, form.Vuelto);
            });
        }

        [Theory]
        [InlineData("btnTarjetaDebito_Click", "Tarjeta de Débito")]
        [InlineData("btnTarjetaCredito_Click", "Tarjeta de Crédito")]
        [InlineData("btnMercadoPago_Click", "Mercado Pago")]
        public void Electronico_SinMontoEscrito_CierraConOkPorElTotal(string boton, string metodoEsperado)
        {
            RunInSta(() =>
            {
                string? mensaje = null;
                FormCobro.MostrarMensaje = (msg, title, btns, icon) => mensaje = msg;

                using var form = new FormCobro(1310m);
                Click(form, boton);

                Assert.Null(mensaje);
                Assert.Equal(DialogResult.OK, form.DialogResult);
                Assert.Equal(metodoEsperado, form.MetodoPagoSeleccionado);
                Assert.Equal(1310m, form.MontoRecibido);
                Assert.Equal(0m, form.Vuelto);
            });
        }

        [Fact]
        public void PresionarEnter_ConCeroEscrito_BloqueaCobroYMuestraMensaje()
        {
            RunInSta(() =>
            {
                string? mensaje = null;
                FormCobro.MostrarMensaje = (msg, title, btns, icon) => mensaje = msg;

                using var form = new FormCobro(1310m);
                SetMonto(form, "0");

                var txt = (TextBox)typeof(FormCobro).GetField("txtMontoRecibido", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(form)!;
                var onKeyDown = txt.GetType().GetMethod("OnKeyDown", BindingFlags.NonPublic | BindingFlags.Instance)!;
                onKeyDown.Invoke(txt, new object[] { new KeyEventArgs(Keys.Enter) });

                Assert.NotNull(mensaje);
                Assert.Contains("menor al total", mensaje);
                Assert.NotEqual(DialogResult.OK, form.DialogResult);
            });
        }

        [Fact]
        public void PresionarEnter_ConMontoMayor_CierraConOkYCalculaVuelto()
        {
            RunInSta(() =>
            {
                string? mensaje = null;
                FormCobro.MostrarMensaje = (msg, title, btns, icon) => mensaje = msg;

                using var form = new FormCobro(1310m);
                SetMonto(form, "1500");

                var txt = (TextBox)typeof(FormCobro).GetField("txtMontoRecibido", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(form)!;
                var onKeyDown = txt.GetType().GetMethod("OnKeyDown", BindingFlags.NonPublic | BindingFlags.Instance)!;
                onKeyDown.Invoke(txt, new object[] { new KeyEventArgs(Keys.Enter) });

                Assert.Null(mensaje);
                Assert.Equal(DialogResult.OK, form.DialogResult);
                Assert.Equal("Efectivo", form.MetodoPagoSeleccionado);
                Assert.Equal(1500m, form.MontoRecibido);
                Assert.Equal(190m, form.Vuelto);
            });
        }
    }
}

