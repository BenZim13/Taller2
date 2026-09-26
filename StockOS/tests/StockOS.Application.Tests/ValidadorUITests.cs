using System.Windows.Forms;
using StockOS.UI.WinForms.Helpers;
using Xunit;

namespace StockOS.Application.Tests
{
    public class ValidadorUITests
    {
        // ==========================================
        // PRUEBAS DE VALIDACIÓN: SOLO LETRAS
        // ==========================================

        [Theory]
        [InlineData("Juan")]
        [InlineData("María")]
        [InlineData("José de San Martín")]
        [InlineData("Pérez-Galdós")]
        [InlineData("O'Connor")]
        [InlineData("Ángel")]
        [InlineData("Iñaki")]
        public void EsSoloLetras_ConNombresValidos_RetornaTrue(string texto)
        {
            bool resultado = ValidadorUI.EsSoloLetras(texto);
            Assert.True(resultado);
        }

        [Theory]
        [InlineData("Juan123")]
        [InlineData("12345")]
        [InlineData("Carlos!")]
        [InlineData("Nombre @ Apellido")]
        [InlineData("Pedro#")]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void EsSoloLetras_ConNumerosOSimbolos_RetornaFalse(string? texto)
        {
            bool resultado = ValidadorUI.EsSoloLetras(texto);
            Assert.False(resultado);
        }

        // ==========================================
        // PRUEBAS DE VALIDACIÓN: SOLO NÚMEROS
        // ==========================================

        [Theory]
        [InlineData("12345678")]
        [InlineData("0")]
        [InlineData("40111222")]
        [InlineData("9999999999")]
        public void EsSoloNumeros_ConDigitosValidos_RetornaTrue(string texto)
        {
            bool resultado = ValidadorUI.EsSoloNumeros(texto);
            Assert.True(resultado);
        }

        [Theory]
        [InlineData("123a")]
        [InlineData("abc")]
        [InlineData("-123")]
        [InlineData("12.3")]
        [InlineData("12,3")]
        [InlineData("123 456")]
        [InlineData("")]
        [InlineData(null)]
        public void EsSoloNumeros_ConLetrasOSimbolos_RetornaFalse(string? texto)
        {
            bool resultado = ValidadorUI.EsSoloNumeros(texto);
            Assert.False(resultado);
        }

        // ==========================================
        // PRUEBAS DE VALIDACIÓN: TELÉFONO
        // ==========================================

        [Theory]
        [InlineData("+54 9 11 1234-5678")]
        [InlineData("1155667788")]
        [InlineData("+1-800-555")]
        [InlineData("011-4455-6677")]
        public void EsTelefonoValido_ConFormatosValidos_RetornaTrue(string texto)
        {
            bool resultado = ValidadorUI.EsTelefonoValido(texto);
            Assert.True(resultado);
        }

        [Theory]
        [InlineData("telefono123")]
        [InlineData("11 22 aa 44")]
        [InlineData("+-")]
        [InlineData("  ")]
        [InlineData("")]
        [InlineData(null)]
        public void EsTelefonoValido_ConLetrasOSinDigitos_RetornaFalse(string? texto)
        {
            bool resultado = ValidadorUI.EsTelefonoValido(texto);
            Assert.False(resultado);
        }

        // ==========================================
        // PRUEBAS DE VALIDACIÓN: ALFANUMÉRICO
        // ==========================================

        [Theory]
        [InlineData("7790000000001")]
        [InlineData("ABC123")]
        [InlineData("PROD99X")]
        public void EsAlfanumerico_ConCodigosValidos_RetornaTrue(string texto)
        {
            bool resultado = ValidadorUI.EsAlfanumerico(texto);
            Assert.True(resultado);
        }

        [Theory]
        [InlineData("PROD 123")]
        [InlineData("PROD-123")]
        [InlineData("COD!")]
        [InlineData("")]
        [InlineData(null)]
        public void EsAlfanumerico_ConEspaciosOSimbolos_RetornaFalse(string? texto)
        {
            bool resultado = ValidadorUI.EsAlfanumerico(texto);
            Assert.False(resultado);
        }

        // ==========================================
        // PRUEBAS DE VALIDACIÓN: CUIT
        // ==========================================

        [Theory]
        [InlineData("30-12345678-9")]
        [InlineData("20-98765432-1")]
        [InlineData("27112233445")]
        public void EsCuitValido_ConFormatosValidos_RetornaTrue(string texto)
        {
            bool resultado = ValidadorUI.EsCuitValido(texto);
            Assert.True(resultado);
        }

        [Theory]
        [InlineData("30-ABCD-9")]
        [InlineData("123")] // Muy corto
        [InlineData("30 12345678 9")] // Espacios
        [InlineData("")]
        [InlineData(null)]
        public void EsCuitValido_ConLetrasOFormatoInvalido_RetornaFalse(string? texto)
        {
            bool resultado = ValidadorUI.EsCuitValido(texto);
            Assert.False(resultado);
        }

        // ==========================================
        // PRUEBAS DE VALIDACIÓN: EMAIL
        // ==========================================

        [Theory]
        [InlineData("usuario@empresa.com")]
        [InlineData("juan.perez@dominio.com.ar")]
        [InlineData("contacto123@sub.dominio.net")]
        public void EsEmailValido_ConEmailsValidos_RetornaTrue(string email)
        {
            bool resultado = ValidadorUI.EsEmailValido(email);
            Assert.True(resultado);
        }

        [Theory]
        [InlineData("sinarroba.com")]
        [InlineData("@sinusuario.com")]
        [InlineData("usuario@sindominio")]
        [InlineData("espacio en@mail.com")]
        [InlineData("")]
        [InlineData(null)]
        public void EsEmailValido_ConEmailsInvalidos_RetornaFalse(string? email)
        {
            bool resultado = ValidadorUI.EsEmailValido(email);
            Assert.False(resultado);
        }

        // ==========================================
        // PRUEBAS DE EVENTOS: KEYPRESS SIMULATION
        // ==========================================

        [Fact]
        public void ConfigurarSoloLetras_BloqueaNumerosYSimbolos_PermiteLetras()
        {
            using var tb = new TextBox();
            ValidadorUI.ConfigurarSoloLetras(tb, "Nombre");

            // Permitir 'J'
            var evLetra = new KeyPressEventArgs('J');
            tb.Text = "";
            tb.GetType().GetMethod("OnKeyPress", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                .Invoke(tb, new object[] { evLetra });
            Assert.False(evLetra.Handled);

            // Bloquear '1'
            var evNumero = new KeyPressEventArgs('1');
            tb.GetType().GetMethod("OnKeyPress", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                .Invoke(tb, new object[] { evNumero });
            Assert.True(evNumero.Handled);

            // Bloquear '$'
            var evSimbolo = new KeyPressEventArgs('$');
            tb.GetType().GetMethod("OnKeyPress", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                .Invoke(tb, new object[] { evSimbolo });
            Assert.True(evSimbolo.Handled);
        }

        [Fact]
        public void ConfigurarSoloNumeros_BloqueaLetrasYSimbolos_PermiteDigitos()
        {
            using var tb = new TextBox();
            ValidadorUI.ConfigurarSoloNumeros(tb, "DNI");

            // Permitir '5'
            var evNumero = new KeyPressEventArgs('5');
            tb.GetType().GetMethod("OnKeyPress", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                .Invoke(tb, new object[] { evNumero });
            Assert.False(evNumero.Handled);

            // Bloquear 'a'
            var evLetra = new KeyPressEventArgs('a');
            tb.GetType().GetMethod("OnKeyPress", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                .Invoke(tb, new object[] { evLetra });
            Assert.True(evLetra.Handled);

            // Bloquear '.'
            var evPunto = new KeyPressEventArgs('.');
            tb.GetType().GetMethod("OnKeyPress", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                .Invoke(tb, new object[] { evPunto });
            Assert.True(evPunto.Handled);
        }

        [Fact]
        public void ConfigurarSoloDecimales_BloqueaLetras_PermiteDigitosYSeparador()
        {
            using var tb = new TextBox();
            ValidadorUI.ConfigurarSoloDecimales(tb, "Precio");

            // Permitir '9'
            var evNumero = new KeyPressEventArgs('9');
            tb.GetType().GetMethod("OnKeyPress", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                .Invoke(tb, new object[] { evNumero });
            Assert.False(evNumero.Handled);

            // Bloquear letra 'x'
            var evLetra = new KeyPressEventArgs('x');
            tb.GetType().GetMethod("OnKeyPress", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                .Invoke(tb, new object[] { evLetra });
            Assert.True(evLetra.Handled);
        }
    }
}

