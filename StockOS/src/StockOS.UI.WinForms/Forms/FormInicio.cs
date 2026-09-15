using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using StockOS.Domain.Entities;

namespace StockOS.UI.WinForms.Forms
{
    public partial class FormInicio : Form
    {
        private readonly IServiceProvider _serviceProvider;
        private RoundedPanel? dockPanel;
        private Panel pnlContenedor;
        private Empleado? _usuarioActual;

        public FormInicio(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            InitializeComponent();

            // Inicializar el contenedor principal donde cargarán las vistas
            pnlContenedor = new Panel();
            pnlContenedor.Dock = DockStyle.Fill;
            pnlContenedor.BackColor = Color.FromArgb(30, 41, 59);
            this.Controls.Add(pnlContenedor);
            
            // Fix Z-order: pnlHeader must be SendToBack so it evaluates first for Dock=Top
            // pnlContenedor must be BringToFront so it evaluates last for Dock=Fill
            pnlContenedor.BringToFront();
            pnlHeader.SendToBack();

            this.Resize += FormInicio_Resize;
        }

        public void EstablecerUsuario(Empleado usuario)
        {
            _usuarioActual = usuario;

            string rolTexto = _usuarioActual.IdRol == 1 ? "Administrador / Gerente" : "Personal";
            this.Text = $"StockOS | Sucursal: {_usuarioActual.IdSucursal} | {_usuarioActual.Nombre} {_usuarioActual.Apellido} ({rolTexto})";
            lblTitulo.Text = $"Bienvenido a StockOS — {_usuarioActual.Nombre} {_usuarioActual.Apellido}";

            InicializarDock();
            CambiarVista("Inicio");
        }

        private void InicializarDock()
        {
            if (dockPanel != null)
            {
                this.Controls.Remove(dockPanel);
                dockPanel.Dispose();
                dockPanel = null;
            }

            // Construir lista de secciones según rol
            var secciones = new List<string> { "Inicio", "Inventario", "Ventas", "Reportes" };

            // Si el usuario es Administrador (IdRol == 1), habilitar opción de gestión de Usuarios
            if (_usuarioActual != null && _usuarioActual.IdRol == 1)
            {
                secciones.Add("Usuarios");
            }

            secciones.Add("Config.");

            // Panel Dock Contenedor
            dockPanel = new RoundedPanel();
            dockPanel.BackColor = Color.FromArgb(30, 38, 56); // Fondo oscuro #1E2638
            dockPanel.BorderRadius = 25;
            dockPanel.Height = 70;
            dockPanel.Width = secciones.Count * 130;
            dockPanel.Anchor = AnchorStyles.None;

            int btnWidth = dockPanel.Width / secciones.Count;

            for (int i = 0; i < secciones.Count; i++)
            {
                Button btn = new Button();
                btn.Text = secciones[i];
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(45, 55, 72);
                btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(16, 185, 129);
                btn.BackColor = Color.Transparent;
                btn.ForeColor = Color.White;
                btn.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
                btn.Cursor = Cursors.Hand;

                btn.Width = btnWidth;
                btn.Height = dockPanel.Height;
                btn.Left = i * btnWidth;
                btn.Top = 0;

                btn.MouseEnter += (s, e) =>
                {
                    btn.ForeColor = Color.FromArgb(16, 185, 129);
                };
                btn.MouseLeave += (s, e) =>
                {
                    btn.ForeColor = Color.White;
                };

                btn.Tag = secciones[i];
                btn.Click += BotonDock_Click;

                dockPanel.Controls.Add(btn);
            }

            this.Controls.Add(dockPanel);
            dockPanel.BringToFront();
            PosicionarDock();
        }

        private void BotonDock_Click(object? sender, EventArgs e)
        {
            if (sender is Button btn && btn.Tag != null)
            {
                CambiarVista(btn.Tag?.ToString() ?? "");
            }
        }

        private void CambiarVista(string nombreVista)
        {
            pnlContenedor.Controls.Clear();
            UserControl? nuevaVista = null;

            switch (nombreVista)
            {
                case "Inicio":
                    lblTitulo.Text = $"Bienvenido a StockOS — {_usuarioActual?.Nombre} {_usuarioActual?.Apellido}";
                    nuevaVista = new UcInicio();
                    break;
                case "Inventario":
                    lblTitulo.Text = "Inventario";
                    nuevaVista = _serviceProvider.GetRequiredService<UcInventario>();
                    break;
                case "Ventas":
                    lblTitulo.Text = "Ventas";
                    nuevaVista = _serviceProvider.GetRequiredService<UcVentas>();
                    break;
                case "Reportes":
                    lblTitulo.Text = "Reportes";
                    nuevaVista = _serviceProvider.GetRequiredService<UcReportes>();
                    break;
                case "Usuarios":
                    lblTitulo.Text = "Usuarios";
                    if (_usuarioActual != null && _usuarioActual.IdRol == 1)
                    {
                        nuevaVista = _serviceProvider.GetRequiredService<UcUsuarios>();
                    }
                    else
                    {
                        MessageBox.Show("No tiene permisos suficientes para acceder a este módulo.", "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    break;
                case "Config.":
                    lblTitulo.Text = "Configuración";
                    nuevaVista = new UcConfig();
                    break;
            }

            if (nuevaVista != null)
            {
                nuevaVista.Dock = DockStyle.Fill;
                pnlContenedor.Controls.Add(nuevaVista);
            }
        }

        private void FormInicio_Resize(object? sender, EventArgs e)
        {
            PosicionarDock();
        }

        private void PosicionarDock()
        {
            if (dockPanel != null)
            {
                int margenInferior = 20;
                dockPanel.Left = (this.ClientSize.Width - dockPanel.Width) / 2;
                dockPanel.Top = this.ClientSize.Height - dockPanel.Height - margenInferior;
            }
        }
    }

    public class RoundedPanel : Panel
    {
        public int BorderRadius { get; set; } = 20;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
            using (GraphicsPath path = GetRoundedPath(rect, BorderRadius))
            {
                this.Region = new Region(path);
                using (SolidBrush brush = new SolidBrush(this.BackColor))
                {
                    e.Graphics.FillPath(brush, path);
                }
            }
        }

        private GraphicsPath GetRoundedPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            float curveSize = radius * 2F;

            path.StartFigure();
            path.AddArc(rect.X, rect.Y, curveSize, curveSize, 180, 90);
            path.AddArc(rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 90);
            path.AddArc(rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize, 0, 90);
            path.AddArc(rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90);
            path.CloseFigure();

            return path;
        }
    }
}