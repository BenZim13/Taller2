using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using StockOS.Domain.Entities;

namespace StockOS.UI.WinForms.Forms
{
    public partial class FormInicio : Form
    {
        // Se agregó el '?' para indicar que el panel puede inicializarse vacío antes del constructor
        private RoundedPanel? dockPanel;
        private Panel pnlContenedor;

        public FormInicio()
        {
            InitializeComponent();

            // Inicializar el contenedor principal donde cargarán las vistas
            pnlContenedor = new Panel();
            pnlContenedor.Dock = DockStyle.Fill;
            pnlContenedor.BackColor = Color.FromArgb(240, 242, 245); // Color de fondo general
            this.Controls.Add(pnlContenedor);
            pnlContenedor.SendToBack(); // Enviar al fondo para que no tape el Dock

            InicializarDock();
            this.Resize += FormInicio_Resize;
        }
        private Empleado? _usuarioActual;

        public void EstablecerUsuario(Empleado usuario)
        {
            _usuarioActual = usuario;

            // Reflejamos en el título de la ventana quién ingresó
            this.Text = $"StockOS | Sucursal: {_usuarioActual.IdSucursal} | Cajero: {_usuarioActual.Nombre} {_usuarioActual.Apellido}";
        }
        private void InicializarDock()
        {
            // Panel Dock Contenedor
            dockPanel = new RoundedPanel();
            dockPanel.BackColor = Color.FromArgb(30, 38, 56); // Fondo oscuro #1E2638
            dockPanel.BorderRadius = 25; // Bordes redondeados
            dockPanel.Height = 70;
            dockPanel.Width = 650; // Ancho fijo para 5 botones
            dockPanel.Anchor = AnchorStyles.None; // Permite manejar el centro manualmente

            // 5 Secciones del menú
            string[] secciones = { "Inicio", "Inventario", "Ventas", "Reportes", "Config." };
            int btnWidth = dockPanel.Width / secciones.Length;

            for (int i = 0; i < secciones.Length; i++)
            {
                Button btn = new Button();
                btn.Text = secciones[i];
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(45, 55, 72); // Aclarar fondo en hover
                btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(16, 185, 129); // Fondo esmeralda al hacer clic
                btn.BackColor = Color.Transparent;
                btn.ForeColor = Color.White; // Texto blanco por defecto
                btn.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
                btn.Cursor = Cursors.Hand;

                // Posicionamiento dentro del Dock
                btn.Width = btnWidth;
                btn.Height = dockPanel.Height;
                btn.Left = i * btnWidth;
                btn.Top = 0;

                // Eventos Hover Interactividad
                btn.MouseEnter += (s, e) =>
                {
                    btn.ForeColor = Color.FromArgb(16, 185, 129); // Verde esmeralda (#10B981)
                };
                btn.MouseLeave += (s, e) =>
                {
                    btn.ForeColor = Color.White; // Vuelve a blanco original
                };

                btn.Tag = secciones[i]; // Guardamos el nombre de la sección
                btn.Click += BotonDock_Click;

                dockPanel.Controls.Add(btn);
            }

            this.Controls.Add(dockPanel);
            dockPanel.BringToFront();
            // Forzar el posicionamiento inicial
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
            // Limpiamos la vista anterior
            pnlContenedor.Controls.Clear();
            UserControl? nuevaVista = null;

            // Instanciamos la vista correspondiente según el botón
            switch (nombreVista)
            {
                case "Inicio":
                    nuevaVista = new UcInicio();
                    break;
                case "Inventario":
                    nuevaVista = new UcInventario();
                    break;
                case "Ventas":
                    nuevaVista = new UcVentas();
                    break;
                case "Reportes":
                    nuevaVista = new UcReportes();
                    break;
                case "Config.":
                    nuevaVista = new UcConfig();
                    break;
            }

            if (nuevaVista != null)
            {
                nuevaVista.Dock = DockStyle.Fill; // Hacer que ocupe todo el espacio
                pnlContenedor.Controls.Add(nuevaVista);
            }
        }

        // Se agregó el '?' al parámetro sender para cumplir con la firma estricta de EventHandler
        private void FormInicio_Resize(object? sender, EventArgs e)
        {
            PosicionarDock();
        }

        private void PosicionarDock()
        {
            if (dockPanel != null)
            {
                int margenInferior = 20; // Separación del borde inferior
                // Calcular posición central horizontal
                dockPanel.Left = (this.ClientSize.Width - dockPanel.Width) / 2;
                dockPanel.Top = this.ClientSize.Height - dockPanel.Height - margenInferior;
            }
        }
    }

    // Clase personalizada para Panel con bordes redondeados GDI+
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
                // Limita el área visible al contorno redondeado
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