using System;
using System.Windows.Forms;
using StockOS.Application;
using StockOS.Application.Services;

namespace StockOS.UI.WinForms.Forms
{
    public partial class FormCierreCaja : Form
    {
        private readonly ICajaService _cajaService;

        public FormCierreCaja(ICajaService cajaService)
        {
            InitializeComponent();
            _cajaService = cajaService;
        }

        private void btnConfirmarCierre_Click(object sender, EventArgs e)
        {
            if (SesionActual.IdCajaSesionAbierta == 0)
            {
                MessageBox.Show("No hay ninguna caja abierta actualmente.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtMontoReal.Text, out decimal montoReal))
            {
                MessageBox.Show("Por favor, ingrese un monto válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // Cerramos en la base de datos
                _cajaService.CerrarCaja(SesionActual.IdCajaSesionAbierta, montoReal);

                // Borramos la caja de la memoria de la aplicación
                SesionActual.IdCajaSesionAbierta = 0;

                MessageBox.Show("Turno finalizado y caja cerrada correctamente.", "Cierre Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cerrar la caja: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}