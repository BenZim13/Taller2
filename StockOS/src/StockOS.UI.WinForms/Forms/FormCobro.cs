using System;
using System.Windows.Forms;

namespace StockOS.UI.WinForms.Forms
{
    public partial class FormCobro : Form
    {
        public string MetodoPagoSeleccionado { get; private set; } = "";

        public FormCobro(decimal totalCobrar)
        {
            InitializeComponent();
            lblTotal.Text = $"$ {totalCobrar:N2}";
        }

        private void btnEfectivo_Click(object sender, EventArgs e)
        {
            MetodoPagoSeleccionado = "Efectivo";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnTarjetaDebito_Click(object sender, EventArgs e)
        {
            MetodoPagoSeleccionado = "Tarjeta de Débito";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnTarjetaCredito_Click(object sender, EventArgs e)
        {
            MetodoPagoSeleccionado = "Tarjeta de Crédito";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnMercadoPago_Click(object sender, EventArgs e)
        {
            MetodoPagoSeleccionado = "Mercado Pago";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}