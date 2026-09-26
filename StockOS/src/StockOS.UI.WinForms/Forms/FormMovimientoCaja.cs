using System;
using System.Windows.Forms;
using StockOS.Application;
using StockOS.Application.Services;

namespace StockOS.UI.WinForms.Forms
{
    public partial class FormMovimientoCaja : Form
    {
        private readonly ICajaService _cajaService;

        public FormMovimientoCaja(ICajaService cajaService)
        {
            InitializeComponent();
            _cajaService = cajaService;
            cmbTipo.SelectedIndex = 0; // Selecciona 'EGRESO' por defecto
            txtMonto.MaxLength = 12;
            StockOS.UI.WinForms.Helpers.ValidadorUI.ConfigurarSoloDecimales(txtMonto, "Monto");
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!SesionActual.IdCajaSesionAbierta.HasValue || SesionActual.IdCajaSesionAbierta.Value == 0)
            {
                MessageBox.Show("No hay caja abierta.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtMonto.Text, out decimal monto) || monto <= 0)
            {
                MessageBox.Show("Por favor, ingrese un monto válido mayor a cero (solo números y decimales).", "Dato Inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMonto.Focus();
                txtMonto.SelectAll();
                return;
            }

            try
            {
                string tipo = cmbTipo.SelectedItem?.ToString() ?? "EGRESO";
                string descripcion = txtDescripcion.Text.Trim();

                _cajaService.RegistrarMovimiento(SesionActual.IdCajaSesionAbierta.Value, tipo, monto, descripcion);

                MessageBox.Show("Movimiento registrado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtMonto_KeyPress(object sender, KeyPressEventArgs e)
        {
            // La validación se gestiona de forma centralizada con ValidadorUI
        }
    }
}