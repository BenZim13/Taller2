using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace StockOS.UI.WinForms.Helpers
{
    /// <summary>
    /// Provee utilidades centralizadas para validación y restricción de entradas en controles WinForms,
    /// emitiendo advertencias visuales inmediatas (Globos ToolTip) al usuario cuando se intenta ingresar
    /// un tipo de dato incorrecto (ej. números en nombres o letras en campos numéricos).
    /// </summary>
    public static class ValidadorUI
    {
        private static readonly ToolTip _balloonToolTip = new ToolTip
        {
            IsBalloon = true,
            ToolTipIcon = ToolTipIcon.Warning,
            ToolTipTitle = "Tipo de dato no permitido",
            AutoPopDelay = 3500,
            InitialDelay = 0,
            ReshowDelay = 100
        };

        /// <summary>
        /// Muestra un aviso emergente en forma de globo apuntando al control especificado.
        /// </summary>
        public static void MostrarAviso(Control control, string mensaje, string titulo = "Tipo de dato no permitido")
        {
            if (control == null || control.IsDisposed) return;

            try
            {
                _balloonToolTip.ToolTipTitle = titulo;
                // Posicionar el globo ligeramente por encima del TextBox
                _balloonToolTip.Show(mensaje, control, 10, -45, 3000);
            }
            catch
            {
                // Silenciar excepciones potenciales en caso de cambios rápidos de foco
            }
        }

        /// <summary>
        /// Restringe el campo para admitir únicamente letras (con tildes, diéresis y eñes), espacios, apóstrofes y guiones.
        /// Bloquea números y caracteres especiales mostrando un aviso al usuario.
        /// </summary>
        public static void ConfigurarSoloLetras(TextBox textBox, string nombreCampo = "este campo")
        {
            if (textBox == null) return;

            textBox.KeyPress += (s, e) =>
            {
                if (char.IsControl(e.KeyChar)) return;

                // Permitir letras (incluye tildes y eñes), espacios, guiones y apóstrofes
                if (char.IsLetter(e.KeyChar) || e.KeyChar == ' ' || e.KeyChar == '\'' || e.KeyChar == '-')
                {
                    return;
                }

                e.Handled = true;
                if (char.IsDigit(e.KeyChar))
                {
                    MostrarAviso(textBox, $"No se permiten números en el campo '{nombreCampo}'. Por favor, ingrese solo letras.", "Solo Letras");
                }
                else
                {
                    MostrarAviso(textBox, $"Solo se permiten letras en el campo '{nombreCampo}'. No se permiten símbolos ni caracteres especiales.", "Solo Letras");
                }
            };
        }

        /// <summary>
        /// Restringe el campo para admitir únicamente dígitos numéricos enteros y teclas de control.
        /// Bloquea letras y símbolos mostrando un aviso explicativo.
        /// </summary>
        public static void ConfigurarSoloNumeros(TextBox textBox, string nombreCampo = "este campo")
        {
            if (textBox == null) return;

            textBox.KeyPress += (s, e) =>
            {
                if (char.IsControl(e.KeyChar)) return;

                if (char.IsDigit(e.KeyChar)) return;

                e.Handled = true;
                MostrarAviso(textBox, $"No se permiten letras ni símbolos en el campo '{nombreCampo}'. Por favor, ingrese solo números enteros.", "Solo Números");
            };
        }

        /// <summary>
        /// Restringe el campo para admitir números y un único separador decimal (punto o coma normalizado).
        /// Bloquea letras y caracteres no numéricos con un aviso explicativo.
        /// </summary>
        public static void ConfigurarSoloDecimales(TextBox textBox, string nombreCampo = "este campo")
        {
            if (textBox == null) return;

            textBox.KeyPress += (s, e) =>
            {
                if (char.IsControl(e.KeyChar)) return;

                char decSep = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];

                if (e.KeyChar == '.' || e.KeyChar == ',')
                {
                    if (textBox.Text.Contains('.') || textBox.Text.Contains(',') || textBox.SelectionStart == 0)
                    {
                        e.Handled = true;
                        MostrarAviso(textBox, $"El campo '{nombreCampo}' ya contiene un separador decimal o no puede comenzar con coma/punto.", "Separador Inválido");
                        return;
                    }
                    e.KeyChar = decSep;
                    return;
                }

                if (!char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                    MostrarAviso(textBox, $"No se permiten letras en el campo '{nombreCampo}'. Por favor, ingrese solo números y decimales.", "Solo Valores Numéricos");
                }
            };
        }

        /// <summary>
        /// Restringe el campo para números de teléfono/celular (números, espacios, '+' y '-').
        /// Bloquea letras mostrando aviso explicativo.
        /// </summary>
        public static void ConfigurarSoloTelefono(TextBox textBox, string nombreCampo = "teléfono")
        {
            if (textBox == null) return;

            textBox.KeyPress += (s, e) =>
            {
                if (char.IsControl(e.KeyChar)) return;

                if (char.IsDigit(e.KeyChar) || e.KeyChar == ' ' || e.KeyChar == '+' || e.KeyChar == '-')
                {
                    return;
                }

                e.Handled = true;
                MostrarAviso(textBox, $"No se permiten letras ni caracteres no válidos en el campo '{nombreCampo}'. Use números, espacios, '+' o '-'.", "Solo Teléfono");
            };
        }

        /// <summary>
        /// Restringe el campo para códigos de barra (solo caracteres alfanuméricos: letras y números sin espacios).
        /// Bloquea espacios y caracteres especiales mostrando un aviso explicativo.
        /// </summary>
        public static void ConfigurarSoloAlfanumerico(TextBox textBox, string nombreCampo = "código")
        {
            if (textBox == null) return;

            textBox.KeyPress += (s, e) =>
            {
                if (char.IsControl(e.KeyChar)) return;

                if (char.IsLetterOrDigit(e.KeyChar)) return;

                e.Handled = true;
                MostrarAviso(textBox, $"El campo '{nombreCampo}' solo admite letras y números (sin espacios ni símbolos especiales).", "Solo Alfanumérico");
            };
        }

        /// <summary>
        /// Restringe el campo para CUIT (números y guiones).
        /// Bloquea letras mostrando aviso explicativo.
        /// </summary>
        public static void ConfigurarSoloCuit(TextBox textBox, string nombreCampo = "CUIT")
        {
            if (textBox == null) return;

            textBox.KeyPress += (s, e) =>
            {
                if (char.IsControl(e.KeyChar)) return;

                if (char.IsDigit(e.KeyChar) || e.KeyChar == '-') return;

                e.Handled = true;
                MostrarAviso(textBox, $"El campo '{nombreCampo}' solo admite números y guiones. No ingrese letras.", "Formato CUIT");
            };
        }

        // =========================================================================
        // MÉTODOS DE VALIDACIÓN ESTÁTICA (Para Submit y validación de texto pegado)
        // =========================================================================

        /// <summary>
        /// Verifica si la cadena contiene únicamente letras, espacios, guiones y apóstrofes, y al menos una letra.
        /// </summary>
        public static bool EsSoloLetras(string? texto)
        {
            if (string.IsNullOrWhiteSpace(texto)) return false;

            bool contieneLetra = false;
            foreach (char c in texto)
            {
                if (char.IsLetter(c))
                {
                    contieneLetra = true;
                }
                else if (c != ' ' && c != '\'' && c != '-')
                {
                    return false;
                }
            }

            return contieneLetra;
        }

        /// <summary>
        /// Verifica si la cadena contiene exclusivamente dígitos numéricos.
        /// </summary>
        public static bool EsSoloNumeros(string? texto)
        {
            if (string.IsNullOrWhiteSpace(texto)) return false;
            return texto.All(char.IsDigit);
        }

        /// <summary>
        /// Verifica si la cadena es un teléfono válido (números, espacios, '+', '-' con al menos un dígito).
        /// </summary>
        public static bool EsTelefonoValido(string? texto)
        {
            if (string.IsNullOrWhiteSpace(texto)) return false;

            bool contieneDigito = false;
            foreach (char c in texto)
            {
                if (char.IsDigit(c))
                {
                    contieneDigito = true;
                }
                else if (c != ' ' && c != '+' && c != '-')
                {
                    return false;
                }
            }

            return contieneDigito;
        }

        /// <summary>
        /// Verifica si la cadena es alfanumérica pura (solo letras y números, sin espacios ni símbolos).
        /// </summary>
        public static bool EsAlfanumerico(string? texto)
        {
            if (string.IsNullOrWhiteSpace(texto)) return false;
            return texto.All(char.IsLetterOrDigit);
        }

        /// <summary>
        /// Verifica si la cadena tiene caracteres válidos de CUIT (números y guiones, al menos 8 dígitos).
        /// </summary>
        public static bool EsCuitValido(string? texto)
        {
            if (string.IsNullOrWhiteSpace(texto)) return false;
            if (!texto.All(c => char.IsDigit(c) || c == '-')) return false;

            int cantDigitos = texto.Count(char.IsDigit);
            return cantDigitos >= 8 && cantDigitos <= 15;
        }

        /// <summary>
        /// Valida el formato básico de un correo electrónico.
        /// </summary>
        public static bool EsEmailValido(string? email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            if (email.Length > 100) return false;

            try
            {
                return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch
            {
                return false;
            }
        }
    }
}

