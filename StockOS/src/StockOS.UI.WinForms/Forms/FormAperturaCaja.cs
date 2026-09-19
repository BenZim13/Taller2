using System;
using System.Windows.Forms;
using StockOS.Application;
using StockOS.Application.Services;

namespace StockOS.UI.WinForms.Forms
{
    public partial class FormAperturaCaja : Form
    {
        private readonly ICajaService _cajaService;

        // Quitamos SesionActual del constructor
        public FormAperturaCaja(ICajaService cajaService)
        {
            InitializeComponent();
            _cajaService = cajaService;

            // Enganchamos el evento Load
            this.Load += FormAperturaCaja_Load;
        }

        private void FormAperturaCaja_Load(object? sender, EventArgs e)
        {
            // Cargamos las cajas de la sucursal del empleado actual
            int idSucursal = SesionActual.Usuario!.IdSucursal;
            var cajas = _cajaService.ObtenerCajasPorSucursal(idSucursal);

            cmbCaja.DataSource = cajas;
            cmbCaja.DisplayMember = "NombreNumero"; // Lo que ve el usuario (Ej: "Caja 1")
            cmbCaja.ValueMember = "IdCaja";         // El ID oculto
        }

        private void btnAbrir_Click(object sender, EventArgs e)
        {
            if (cmbCaja.SelectedValue == null)
            {
                MessageBox.Show("Debe seleccionar una caja válida.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtMonto.Text, out decimal montoApertura) || montoApertura < 0)
            {
                MessageBox.Show("Por favor, ingrese un monto inicial válido.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int idEmpleado = SesionActual.Usuario!.IdEmpleado;
                int idCaja = Convert.ToInt32(cmbCaja.SelectedValue); // <-- Tomamos la caja elegida

                // 1. Abrimos la caja en la base de datos y obtenemos el ID real generado
                int idCajaSesion = _cajaService.AbrirCaja(idCaja, idEmpleado, montoApertura);

                // 2.Guardamos ese ID en nuestra memoria global para que UcVentas lo pueda leer
                SesionActual.IdCajaSesionAbierta = idCajaSesion;

                MessageBox.Show($"¡Caja abierta exitosamente!\nSesión N°: {idCajaSesion}", "Apertura Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al abrir la caja: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void txtMonto_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != ','))
            {
                e.Handled = true;
            }
        }

        private void btnSalirApp_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}