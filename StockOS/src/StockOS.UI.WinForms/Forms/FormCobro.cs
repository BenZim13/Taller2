using System;
using System.Windows.Forms;

namespace StockOS.UI.WinForms.Forms
{
    public partial class FormCobro : Form
    {
        // Propiedad pública para que UcVentas sepa qué eligió el cajero
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

        private void btnTarjeta_Click(object sender, EventArgs e)
        {
            MetodoPagoSeleccionado = "Tarjeta";
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