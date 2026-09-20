using System;
using System.Windows.Forms;

namespace StockOS.UI.WinForms.Forms
{
    public partial class FormCobro : Form
    {
        private readonly decimal _totalCobrar;

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
        }

        private void btnEfectivo_Click(object sender, EventArgs e)
        {
            // Validar monto ingresado para efectivo
            if (!decimal.TryParse(txtMontoRecibido.Text, out decimal montoIngresado))
            {
                // Si no ingresó nada, asumimos pago exacto
                montoIngresado = _totalCobrar;
            }

            if (montoIngresado < _totalCobrar)
            {
                MessageBox.Show($"El monto ingresado (${montoIngresado:N2}) es menor al total (${_totalCobrar:N2}).",
                    "Monto Insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMontoRecibido.Focus();
                txtMontoRecibido.SelectAll();
                return;
            }

            MontoRecibido = montoIngresado;
            Vuelto = montoIngresado - _totalCobrar;
            MetodoPagoSeleccionado = "Efectivo";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnTarjetaDebito_Click(object sender, EventArgs e)
        {
            MontoRecibido = _totalCobrar;
            Vuelto = 0;
            MetodoPagoSeleccionado = "Tarjeta de Débito";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnTarjetaCredito_Click(object sender, EventArgs e)
        {
            MontoRecibido = _totalCobrar;
            Vuelto = 0;
            MetodoPagoSeleccionado = "Tarjeta de Crédito";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnMercadoPago_Click(object sender, EventArgs e)
        {
            MontoRecibido = _totalCobrar;
            Vuelto = 0;
            MetodoPagoSeleccionado = "Mercado Pago";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void txtMontoRecibido_TextChanged(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtMontoRecibido.Text, out decimal monto) && monto >= _totalCobrar)
            {
                lblVueltoValor.Text = $"$ {(monto - _totalCobrar):N2}";
                lblVueltoValor.ForeColor = System.Drawing.Color.FromArgb(16, 185, 129);
            }
            else
            {
                lblVueltoValor.Text = "$ 0,00";
                lblVueltoValor.ForeColor = System.Drawing.Color.White;
            }
        }
    }
}