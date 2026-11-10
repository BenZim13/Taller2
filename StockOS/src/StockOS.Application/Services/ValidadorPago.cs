using System;
using System.Globalization;

namespace StockOS.Application.Services
{
    /// <summary>
    /// Resultado de validar un pago contra el total a cobrar.
    /// </summary>
    public sealed class ResultadoPago
    {
        public bool EsValido { get; init; }
        public string Titulo { get; init; } = string.Empty;
        public string Mensaje { get; init; } = string.Empty;
        public decimal MontoRecibido { get; init; }
        public decimal Vuelto { get; init; }

        public static ResultadoPago Valido(decimal montoRecibido, decimal vuelto) =>
            new() { EsValido = true, MontoRecibido = montoRecibido, Vuelto = vuelto };

        public static ResultadoPago Invalido(string titulo, string mensaje) =>
            new() { EsValido = false, Titulo = titulo, Mensaje = mensaje };
    }

    /// <summary>
    /// Regla de negocio centralizada para validar el monto recibido en un cobro.
    /// Es pura y sin estado: la usan tanto el formulario de cobro como la pantalla de ventas
    /// (segunda barrera antes de registrar la venta).
    /// </summary>
    public static class ValidadorPago
    {
        public const decimal MontoMaximo = 999_999_999.99m;

        /// <param name="metodoPago">Nombre del método ("Efectivo", "Tarjeta de Débito", etc.).</param>
        /// <param name="total">Total a cobrar de la venta.</param>
        /// <param name="montoIngresado">Monto escrito por el cajero; null si dejó el campo vacío.</param>
        public static ResultadoPago Validar(string? metodoPago, decimal total, decimal? montoIngresado)
        {
            if (string.IsNullOrWhiteSpace(metodoPago))
            {
                return ResultadoPago.Invalido("Método de Pago", "Debe seleccionarse un método de pago.");
            }

            if (total < 0)
            {
                return ResultadoPago.Invalido("Total Inválido", "El total a cobrar no puede ser negativo.");
            }

            decimal totalRedondeado = Redondear(total);
            bool esEfectivo = metodoPago.Contains("efectivo", StringComparison.OrdinalIgnoreCase);

            if (esEfectivo)
            {
                // Campo vacío: se asume pago exacto (comportamiento histórico del sistema).
                if (!montoIngresado.HasValue)
                {
                    return ResultadoPago.Valido(totalRedondeado, 0m);
                }

                decimal monto = Redondear(montoIngresado.Value);

                if (monto < 0)
                {
                    return ResultadoPago.Invalido("Dato Inválido", "El monto recibido no puede ser negativo.");
                }

                if (monto < totalRedondeado)
                {
                    return ResultadoPago.Invalido("Monto Insuficiente",
                        $"No es posible realizar el cobro. El monto ingresado (${monto:N2}) es menor al total a cobrar (${totalRedondeado:N2}).\n\n" +
                        "Se requiere ingresar un monto igual o mayor a lo que se va a cobrar.");
                }

                if (monto > MontoMaximo)
                {
                    return ResultadoPago.Invalido("Monto Excesivo",
                        $"El monto ingresado supera el máximo permitido (${MontoMaximo:N2}).");
                }

                return ResultadoPago.Valido(monto, monto - totalRedondeado);
            }

            // Pagos electrónicos: el monto es exacto. Si el cajero dejó un valor escrito, debe coincidir.
            if (montoIngresado.HasValue)
            {
                decimal montoEscrito = Redondear(montoIngresado.Value);

                if (montoEscrito < totalRedondeado)
                {
                    return ResultadoPago.Invalido("Monto Insuficiente",
                        $"No es posible realizar el cobro. El monto ingresado (${montoEscrito:N2}) es menor al total a cobrar (${totalRedondeado:N2}).\n\n" +
                        $"Ingrese un monto igual o mayor al total, o borre el campo para cobrar con '{metodoPago}' por el total exacto.");
                }

                if (montoEscrito != totalRedondeado)
                {
                    return ResultadoPago.Invalido("Monto Inconsistente",
                        $"El monto escrito (${montoEscrito:N2}) no coincide con el total a cobrar (${totalRedondeado:N2}).\n\n" +
                        $"Los pagos con '{metodoPago}' se cobran por el total exacto. Borre el campo de monto recibido o ingrese el total.");
                }
            }

            return ResultadoPago.Valido(totalRedondeado, 0m);
        }

        private static decimal Redondear(decimal valor) =>
            Math.Round(valor, 2, MidpointRounding.AwayFromZero);
    }
}
