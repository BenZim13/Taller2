using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using StockOS.Application.Services;
using StockOS.Domain.DTOs;
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
            pnlGraficos.Visible = false;

            cboPeriodo.SelectedIndex = 0; // Día por defecto en PDF
            cboPeriodoGraficos.SelectedIndex = 1; // Mes por defecto en Gráficos

            // Eventos PDF
            btnReporteVentas.Click += BtnReporteVentas_Click;
            btnReporteCompras.Click += BtnReporteCompras_Click;
            btnVolver.Click += BtnVolver_Click;
            btnGenerarPdf.Click += BtnGenerarPdf_Click;

            // Eventos Gráficos
            btnGraficos.Click += BtnGraficos_Click;
            btnVolverGraficos.Click += BtnVolverGraficos_Click;
            btnGenerarGraficos.Click += BtnGenerarGraficos_Click;
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

        private void BtnGraficos_Click(object? sender, EventArgs e)
        {
            if (!_authorizationService.TienePermiso(Permisos.REPORTES_VER))
            {
                MessageBox.Show("No tiene permisos para ver estadísticas.", "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            pnlOpciones.Visible = false;
            pnlGraficos.Visible = true;
            pnlGraficos.BringToFront();

            // Cargar gráficos por defecto la primera vez que se entra
            if (chartTopProductos.Series.Count == 0)
            {
                btnGenerarGraficos.PerformClick();
            }
        }

        private void BtnVolverGraficos_Click(object? sender, EventArgs e)
        {
            pnlGraficos.Visible = false;
            pnlOpciones.Visible = true;
        }

        private async void BtnGenerarGraficos_Click(object? sender, EventArgs e)
        {
            if (cboPeriodoGraficos.SelectedItem == null) return;

            string periodoStr = cboPeriodoGraficos.SelectedItem.ToString() ?? "Mes";
            var (fechaInicio, fechaFin) = CalcularRangoFechas(periodoStr);
            int topN = (int)numTopProductos.Value;

            try
            {
                btnGenerarGraficos.Enabled = false;
                btnGenerarGraficos.Text = "Cargando...";

                var topProductos = await _reporteService.ObtenerTopProductosVendidosAsync(topN, fechaInicio, fechaFin);
                var ventasTiempo = await _reporteService.ObtenerVentasPorLineaTiempoAsync(fechaInicio, fechaFin);

                RenderizarGraficoTorta(topProductos);
                RenderizarGraficoLinea(ventasTiempo);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al generar los gráficos:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnGenerarGraficos.Enabled = true;
                btnGenerarGraficos.Text = "Cargar Gráficos";
            }
        }

        private void RenderizarGraficoTorta(IEnumerable<ReporteItemDTO> datos)
        {
            chartTopProductos.Series.Clear();
            chartTopProductos.Titles.Clear();

            Title titulo = new Title("Top Productos Más Vendidos", Docking.Top, new Font("Segoe UI", 14, FontStyle.Bold), Color.White);
            chartTopProductos.Titles.Add(titulo);

            Series serie = new Series("TopProductos")
            {
                ChartType = SeriesChartType.Pie,
                IsValueShownAsLabel = true,
                LabelForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                LabelFormat = "#,##0"
            };

            foreach (var item in datos)
            {
                int puntoIndex = serie.Points.AddXY(item.NombreProducto, item.CantidadTotal);
                serie.Points[puntoIndex].ToolTip = $"Subtotal: {item.Subtotal:C2}";
            }

            chartTopProductos.Series.Add(serie);
        }

        private void RenderizarGraficoLinea(IEnumerable<VentasPorFechaDTO> datos)
        {
            chartLineaTiempo.Series.Clear();
            chartLineaTiempo.Titles.Clear();

            Title titulo = new Title("Evolución de Ingresos por Ventas", Docking.Top, new Font("Segoe UI", 14, FontStyle.Bold), Color.White);
            chartLineaTiempo.Titles.Add(titulo);

            Series serie = new Series("Ingresos")
            {
                ChartType = SeriesChartType.SplineArea,
                BorderWidth = 3,
                Color = Color.FromArgb(14, 165, 233),
                BackGradientStyle = GradientStyle.TopBottom,
                BackSecondaryColor = Color.Transparent,
                MarkerStyle = MarkerStyle.Circle,
                MarkerSize = 8,
                IsValueShownAsLabel = false,
                // 1. CLAVE: Le decimos al gráfico que va a recibir Fechas reales, no textos
                XValueType = ChartValueType.Date
            };

            foreach (var item in datos)
            {
                // 2. CLAVE: Le pasamos 'item.Fecha' directo (objeto DateTime), sin el ToString()
                serie.Points.AddXY(item.Fecha, item.TotalVendido);
            }

            chartLineaTiempo.Series.Add(serie);

            // 3. CLAVE: Le damos el formato visual al eje X para que diga "19/sept"
            chartLineaTiempo.ChartAreas[0].AxisX.LabelStyle.Format = "dd/MMM";

            // Opcional: aseguramos que muestre etiquetas para cada día si el rango es corto
            chartLineaTiempo.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Days;
            chartLineaTiempo.ChartAreas[0].AxisX.Interval = 1;
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
            // Hasta el último milisegundo del día actual para que incluya todas las ventas de hoy
            DateTime fin = hoy.AddDays(1).AddTicks(-1);

            switch (periodo.Trim().ToLower())
            {
                case "diario":
                    return (hoy, fin);

                case "semanal":
                    return (hoy.AddDays(-6), fin);

                case "mensual":
                    return (ahora.AddMonths(-1).Date, fin);

                case "bimestral":
                    return (ahora.AddMonths(-2).Date, fin);

                case "trimestral":
                    return (ahora.AddMonths(-3).Date, fin);

                case "semestral":
                    return (ahora.AddMonths(-6).Date, fin);

                case "9 meses":
                    return (ahora.AddMonths(-9).Date, fin);

                case "anual":
                    return (ahora.AddYears(-1).Date, fin);

                default:
                    return (hoy, fin); // Fallback de seguridad
            }
        }
    }
}