using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using StockOS.Application.Services;

namespace StockOS.UI.WinForms.Forms
{
    public partial class FormCobro : Form
    {
        private readonly decimal _totalCobrar;

        // Delegado para mostrar mensajes (permite interceptar en pruebas automatizadas sin bloquear la UI)
        public static Action<string, string, MessageBoxButtons, MessageBoxIcon> MostrarMensaje { get; set; } =
            (msg, title, btns, icon) => MessageBox.Show(msg, title, btns, icon);

        public string MetodoPagoSeleccionado { get; private set; } = "";
        public decimal MontoRecibido { get; private set; }
        public decimal Vuelto { get; private set; }

        public FormCobro(decimal totalCobrar)
        {
            InitializeComponent();
            _totalCobrar = totalCobrar;
            lblTotal.Text = $"$ {totalCobrar:N2}";

            // Por defecto, el monto recibido es el total (para pagos electrónicos)
            MontoRecibido = totalCobrar;
            Vuelto = 0;
            txtMontoRecibido.MaxLength = 12;
            StockOS.UI.WinForms.Helpers.ValidadorUI.ConfigurarSoloDecimales(txtMontoRecibido, "Monto Recibido");

            // Al presionar ENTER en el campo de monto recibido, ejecutar cobro en Efectivo
            txtMontoRecibido.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    ConfirmarPago("Efectivo");
                }
            };

            this.Shown += (s, e) => txtMontoRecibido.Focus();
        }

        /// <summary>
        /// Valida el pago con la regla centralizada (ValidadorPago) para CUALQUIER método de pago.
        /// Solo cierra el formulario con OK si el pago es válido.
        /// </summary>
        private void ConfirmarPago(string metodoPago)
        {
            decimal? montoIngresado = null;

            if (!string.IsNullOrWhiteSpace(txtMontoRecibido.Text))
            {
                if (!decimal.TryParse(txtMontoRecibido.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out decimal parsed) || parsed < 0)
                {
                    MostrarMensaje("Por favor, ingrese un monto válido (solo números y decimales).", "Dato Inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMontoRecibido.Focus();
                    txtMontoRecibido.SelectAll();
                    return;
                }
                montoIngresado = parsed;
            }

            var resultado = ValidadorPago.Validar(metodoPago, _totalCobrar, montoIngresado);

            if (!resultado.EsValido)
            {
                MostrarMensaje(resultado.Mensaje, resultado.Titulo, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMontoRecibido.Focus();
                txtMontoRecibido.SelectAll();
                return;
            }

            MontoRecibido = resultado.MontoRecibido;
            Vuelto = resultado.Vuelto;
            MetodoPagoSeleccionado = metodoPago;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnEfectivo_Click(object sender, EventArgs e) => ConfirmarPago("Efectivo");

        private void btnTarjetaDebito_Click(object sender, EventArgs e) => ConfirmarPago("Tarjeta de Débito");

        private void btnTarjetaCredito_Click(object sender, EventArgs e) => ConfirmarPago("Tarjeta de Crédito");

        private void btnMercadoPago_Click(object sender, EventArgs e) => ConfirmarPago("Mercado Pago");

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void txtMontoRecibido_TextChanged(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtMontoRecibido.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out decimal monto))
            {
                decimal total = Math.Round(_totalCobrar, 2, MidpointRounding.AwayFromZero);
                decimal montoRedondeado = Math.Round(monto, 2, MidpointRounding.AwayFromZero);

                if (montoRedondeado >= total)
                {
                    lblVuelto.Text = "Vuelto:";
                    lblVueltoValor.Text = $"$ {(montoRedondeado - total):N2}";
                    lblVueltoValor.ForeColor = Color.FromArgb(16, 185, 129);
                }
                else
                {
                    // Aviso en vivo: falta dinero para cubrir el total
                    lblVuelto.Text = "Falta:";
                    lblVueltoValor.Text = $"$ {(total - montoRedondeado):N2}";
                    lblVueltoValor.ForeColor = Color.FromArgb(239, 68, 68);
                }
            }
            else
            {
                lblVuelto.Text = "Vuelto:";
                lblVueltoValor.Text = "$ 0,00";
                lblVueltoValor.ForeColor = Color.White;
            }
        }
    }
}
