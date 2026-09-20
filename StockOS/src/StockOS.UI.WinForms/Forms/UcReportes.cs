using System;
using System.Drawing;
using System.Windows.Forms;
using StockOS.Application.Services;
using StockOS.Domain.Enums;
using StockOS.Domain.Interfaces;

namespace StockOS.UI.WinForms.Forms
{
    public partial class UcReportes : UserControl
    {
        private readonly IReporteService _reporteService;
        private readonly IAuthorizationService _authorizationService;
        private string _tipoReporteActual = ""; // "Ventas" o "Compras"

        public UcReportes(IReporteService reporteService, IAuthorizationService authorizationService)
        {
            InitializeComponent();
            _reporteService = reporteService;
            _authorizationService = authorizationService;

            // Configurar vista inicial
            pnlOpciones.Visible = true;
            pnlFiltros.Visible = false;
            cboPeriodo.SelectedIndex = 0; // Día por defecto

            // Eventos
            btnReporteVentas.Click += BtnReporteVentas_Click;
            btnReporteCompras.Click += BtnReporteCompras_Click;
            btnVolver.Click += BtnVolver_Click;
            btnGenerarPdf.Click += BtnGenerarPdf_Click;
        }

        private void BtnReporteVentas_Click(object? sender, EventArgs e)
        {
            if (!_authorizationService.TienePermiso(Permisos.REPORTES_VER))
            {
                MessageBox.Show("No tiene permisos para ver reportes.", "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MostrarFiltros("Ventas");
        }

        private void BtnReporteCompras_Click(object? sender, EventArgs e)
        {
            if (!_authorizationService.TienePermiso(Permisos.REPORTES_VER))
            {
                MessageBox.Show("No tiene permisos para ver reportes.", "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MostrarFiltros("Compras");
        }

        private void MostrarFiltros(string tipoReporte)
        {
            _tipoReporteActual = tipoReporte;
            lblTituloFiltro.Text = $"Generar Reporte de {tipoReporte}";
            pnlOpciones.Visible = false;
            pnlFiltros.Visible = true;
        }

        private void BtnVolver_Click(object? sender, EventArgs e)
        {
            pnlFiltros.Visible = false;
            pnlOpciones.Visible = true;
        }

        private async void BtnGenerarPdf_Click(object? sender, EventArgs e)
        {
            if (cboPeriodo.SelectedItem == null)
            {
                MessageBox.Show("Seleccione un período.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string periodoStr = cboPeriodo.SelectedItem.ToString() ?? "Día";
            var (fechaInicio, fechaFin) = CalcularRangoFechas(periodoStr);
            string periodoDescripcion = $"{periodoStr} ({fechaInicio:dd/MM/yyyy} - {fechaFin:dd/MM/yyyy})";

            try
            {
                btnGenerarPdf.Enabled = false;
                btnGenerarPdf.Text = "Generando...";

                if (_tipoReporteActual == "Ventas")
                {
                    await _reporteService.GenerarReporteVentasPdfAsync(fechaInicio, fechaFin, periodoDescripcion);
                }
                else if (_tipoReporteActual == "Compras")
                {
                    await _reporteService.GenerarReporteComprasPdfAsync(fechaInicio, fechaFin, periodoDescripcion);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al generar el reporte:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnGenerarPdf.Enabled = true;
                btnGenerarPdf.Text = "📄 Generar PDF";
            }
        }

        private (DateTime Inicio, DateTime Fin) CalcularRangoFechas(string periodo)
        {
            DateTime ahora = DateTime.Now;
            DateTime hoy = ahora.Date;
            DateTime fin = hoy.AddDays(1).AddTicks(-1); // Hasta el final del día de hoy (23:59:59)

            switch (periodo)
            {
                case "Día":
                    return (hoy, fin);
                case "Semana":
                    // Últimos 7 días incluyendo hoy
                    DateTime inicioSemana = hoy.AddDays(-6);
                    return (inicioSemana, fin);
                case "Mes":
                    // Primer día del mes actual
                    DateTime inicioMes = new DateTime(ahora.Year, ahora.Month, 1);
                    return (inicioMes, fin);
                default:
                    return (hoy, fin);
            }
        }
    }
}
