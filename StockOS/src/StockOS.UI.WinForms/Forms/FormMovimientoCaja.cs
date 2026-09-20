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
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!SesionActual.IdCajaSesionAbierta.HasValue || SesionActual.IdCajaSesionAbierta.Value == 0)
            {
                MessageBox.Show("No hay caja abierta.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtMonto.Text, out decimal monto))
            {
                MessageBox.Show("Ingrese un monto válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != ','))
            {
                e.Handled = true;
            }
        }
    }
}