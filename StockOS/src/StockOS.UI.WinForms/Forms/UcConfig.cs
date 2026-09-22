using System;
using System.Drawing;
using System.Windows.Forms;

namespace StockOS.UI.WinForms.Forms
{
    public partial class UcConfig : UserControl
    {
        private ContenedorConfigScroll _pnlContenedor = null!;
        private bool _isAdjustingLayout = false;

        // Tarjetas principales
        private RoundedPanel _pnlAcerca = null!;
        private RoundedPanel _pnlSpecs = null!;
        private RoundedPanel _pnlAdmin = null!;
        private RoundedPanel _pnlSoporte = null!;

        // Controles de Tarjeta 1: Acerca de StockOS
        private Label _lblAcercaTitulo = null!;
        private Label _lblAcercaBadge = null!;
        private Label _lblAcercaDesc = null!;

        // Controles de Tarjeta 2: Especificaciones Técnicas
        private Label _lblSpecsTitulo = null!;
        private Label _lblSpecsSub = null!;
        private Label[] _lblSpecsItems = null!;

        // Controles de Tarjeta 3: Administrador de Sistemas
        private Label _lblAdminTitulo = null!;
        private Label _lblAdminDesc = null!;
        private Button _btnAdmin = null!;

        // Controles de Tarjeta 4: Soporte Técnico
        private Label _lblSoporteTitulo = null!;
        private Label _lblSoporteDesc = null!;
        private RoundedPanel _pnlEmail = null!;
        private Label _lblEmail = null!;

        public UcConfig()
        {
            InitializeComponent();
            InicializarComponentesVisuales();
        }

        private void InicializarComponentesVisuales()
        {
            this.BackColor = Color.FromArgb(30, 41, 59);

            // Contenedor principal con scroll vertical preciso y buffer doble
            _pnlContenedor = new ContenedorConfigScroll
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.FromArgb(30, 41, 59)
            };
            this.Controls.Add(_pnlContenedor);

            // 1. Tarjeta: Acerca de StockOS
            _pnlAcerca = CrearTarjeta();
            _lblAcercaTitulo = CrearLabelTitulo("📦 Acerca de StockOS");
            _lblAcercaBadge = new Label
            {
                Text = "Año de creación: 2026   |   Versión: 1.0.0 (Release)",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(56, 189, 248),
                TextAlign = ContentAlignment.MiddleCenter
            };
            _lblAcercaDesc = CrearLabelTexto(
                "StockOS es un sistema integral de Punto de Venta (POS) y Administración de Stock Comercial desarrollado en C# y .NET 8 para el control de inventario en tiempo real con alertas de reposición, facturación rápida por código de barras, gestión de compras y reportes analíticos para comercios minoristas."
            );
            _pnlAcerca.Controls.AddRange(new Control[] { _lblAcercaTitulo, _lblAcercaBadge, _lblAcercaDesc });

            // 2. Tarjeta: Especificaciones Técnicas
            _pnlSpecs = CrearTarjeta();
            _lblSpecsTitulo = CrearLabelTitulo("💻 Especificaciones Técnicas para la Instalación");
            _lblSpecsSub = new Label
            {
                Text = "Requisitos mínimos y recomendados de hardware y software para el correcto despliegue del sistema:",
                Font = new Font("Segoe UI", 9F, FontStyle.Italic),
                ForeColor = Color.FromArgb(148, 163, 184),
                TextAlign = ContentAlignment.MiddleCenter
            };

            string[] especificaciones = new string[]
            {
                "• Sistema Operativo y Runtime: Windows 10/11 / Windows Server (64-bit)   |   Microsoft .NET 8.0 Desktop Runtime (x64)",
                "• Base de Datos: Microsoft SQL Server 2019 / 2022 (LocalDB, Express, Standard o Enterprise)",
                "• Procesador y Memoria: CPU x64 Dual-Core 2.0 GHz o superior   |   Memoria RAM: 4 GB mín. (8 GB recomendado)",
                "• Pantalla y Periféricos: Resolución 1280 × 720 mín. (1080p FHD recomendado)   |   Lector USB e impresora ESC/POS"
            };

            _lblSpecsItems = new Label[especificaciones.Length];
            for (int i = 0; i < especificaciones.Length; i++)
            {
                _lblSpecsItems[i] = new Label
                {
                    Text = especificaciones[i],
                    Font = new Font("Segoe UI", 9.25F, FontStyle.Regular),
                    ForeColor = Color.FromArgb(226, 232, 240),
                    TextAlign = ContentAlignment.MiddleCenter
                };
                _pnlSpecs.Controls.Add(_lblSpecsItems[i]);
            }
            _pnlSpecs.Controls.AddRange(new Control[] { _lblSpecsTitulo, _lblSpecsSub });

            // 3. Tarjeta: Administrador de Sistemas
            _pnlAdmin = CrearTarjeta();
            _lblAdminTitulo = CrearLabelTitulo("🛠️ Administrador de Sistemas");
            _lblAdminDesc = CrearLabelTexto(
                "Módulo para administración de infraestructura, logs del sistema y utilidades de mantenimiento.\nEsta funcionalidad se encuentra en desarrollo y estará disponible para el administrador en futuras versiones."
            );

            _btnAdmin = new Button
            {
                Text = "Administrador de Sistemas",
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(59, 130, 246),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleCenter,
                TabStop = false // Evita scroll automático indeseado por foco inicial
            };
            _btnAdmin.FlatAppearance.BorderSize = 0;
            _btnAdmin.FlatAppearance.MouseOverBackColor = Color.FromArgb(37, 99, 235);
            _btnAdmin.FlatAppearance.MouseDownBackColor = Color.FromArgb(29, 78, 216);
            _btnAdmin.Click += (s, e) => { };

            _pnlAdmin.Controls.AddRange(new Control[] { _lblAdminTitulo, _lblAdminDesc, _btnAdmin });

            // 4. Tarjeta: Contactar a Soporte
            _pnlSoporte = CrearTarjeta();
            _lblSoporteTitulo = CrearLabelTitulo("📞 Contactar a Soporte");
            _lblSoporteDesc = CrearLabelTexto(
                "Si experimentas algún inconveniente técnico, consultas sobre la instalación o requieres asistencia con la plataforma:"
            );

            _pnlEmail = new RoundedPanel
            {
                BorderRadius = 10,
                BackColor = Color.FromArgb(30, 41, 59)
            };

            _lblEmail = new Label
            {
                Text = "✉  stockos@gmail.com",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(56, 189, 248),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            _pnlEmail.Controls.Add(_lblEmail);

            _pnlSoporte.Controls.AddRange(new Control[] { _lblSoporteTitulo, _lblSoporteDesc, _pnlEmail });

            // Agregar tarjetas al contenedor
            _pnlContenedor.Controls.AddRange(new Control[] { _pnlAcerca, _pnlSpecs, _pnlAdmin, _pnlSoporte });

            // Eventos responsivos
            _pnlContenedor.Resize += (s, e) => AjustarLayout();
            this.Resize += (s, e) => AjustarLayout();

            AjustarLayout();
        }

        private RoundedPanel CrearTarjeta()
        {
            return new RoundedPanel
            {
                BackColor = Color.FromArgb(51, 65, 85),
                BorderRadius = 14
            };
        }

        private Label CrearLabelTitulo(string texto)
        {
            return new Label
            {
                Text = texto,
                Font = new Font("Segoe UI", 13.5F, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter
            };
        }

        private Label CrearLabelTexto(string texto)
        {
            return new Label
            {
                Text = texto,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Regular),
                ForeColor = Color.FromArgb(226, 232, 240),
                TextAlign = ContentAlignment.MiddleCenter
            };
        }

        private void AjustarLayout()
        {
            if (_isAdjustingLayout || _pnlContenedor == null) return;

            try
            {
                _isAdjustingLayout = true;
                _pnlContenedor.SuspendLayout();

                int savedScroll = Math.Abs(_pnlContenedor.AutoScrollPosition.Y);
                _pnlContenedor.AutoScrollPosition = new Point(0, 0);

                int clientW = _pnlContenedor.ClientSize.Width;
                int clientH = _pnlContenedor.ClientSize.Height;
                if (clientW <= 80 || clientH <= 80) return;

                int scrollbarWidth = SystemInformation.VerticalScrollBarWidth + 4;
                int targetWidth = Math.Max(320, Math.Min(840, clientW - scrollbarWidth - 24));
                int paddingX = 20;
                int controlWidth = targetWidth - (paddingX * 2);

                int cardX = Math.Max(8, (clientW - targetWidth - scrollbarWidth) / 2);
                int gap = 8;
                int paddingY = 10;
                int currentY = paddingY;

                // --- 1. Tarjeta: Acerca de StockOS ---
                _pnlAcerca.Width = targetWidth;
                int yAcerca = 12;
                PosicionarLabel(_lblAcercaTitulo, paddingX, controlWidth, ref yAcerca, 5);
                PosicionarLabel(_lblAcercaBadge, paddingX, controlWidth, ref yAcerca, 6);
                PosicionarLabel(_lblAcercaDesc, paddingX, controlWidth, ref yAcerca, 6);
                _pnlAcerca.Height = yAcerca + 6;
                _pnlAcerca.Location = new Point(cardX, currentY);
                currentY += _pnlAcerca.Height + gap;

                // --- 2. Tarjeta: Especificaciones Técnicas ---
                _pnlSpecs.Width = targetWidth;
                int ySpecs = 12;
                PosicionarLabel(_lblSpecsTitulo, paddingX, controlWidth, ref ySpecs, 4);
                PosicionarLabel(_lblSpecsSub, paddingX, controlWidth, ref ySpecs, 6);
                for (int i = 0; i < _lblSpecsItems.Length; i++)
                {
                    PosicionarLabel(_lblSpecsItems[i], paddingX, controlWidth, ref ySpecs, 3);
                }
                _pnlSpecs.Height = ySpecs + 6;
                _pnlSpecs.Location = new Point(cardX, currentY);
                currentY += _pnlSpecs.Height + gap;

                // --- 3. Tarjeta: Administrador de Sistemas ---
                _pnlAdmin.Width = targetWidth;
                int yAdmin = 12;
                PosicionarLabel(_lblAdminTitulo, paddingX, controlWidth, ref yAdmin, 5);
                PosicionarLabel(_lblAdminDesc, paddingX, controlWidth, ref yAdmin, 8);
                int btnW = Math.Min(260, controlWidth);
                _btnAdmin.Size = new Size(btnW, 36);
                _btnAdmin.Location = new Point((targetWidth - btnW) / 2, yAdmin);
                yAdmin += _btnAdmin.Height + 8;
                _pnlAdmin.Height = yAdmin + 4;
                _pnlAdmin.Location = new Point(cardX, currentY);
                currentY += _pnlAdmin.Height + gap;

                // --- 4. Tarjeta: Contactar a Soporte ---
                _pnlSoporte.Width = targetWidth;
                int ySoporte = 12;
                PosicionarLabel(_lblSoporteTitulo, paddingX, controlWidth, ref ySoporte, 5);
                PosicionarLabel(_lblSoporteDesc, paddingX, controlWidth, ref ySoporte, 8);
                int emailW = Math.Min(300, controlWidth);
                _pnlEmail.Size = new Size(emailW, 36);
                _pnlEmail.Location = new Point((targetWidth - emailW) / 2, ySoporte);
                ySoporte += _pnlEmail.Height + 8;
                _pnlSoporte.Height = ySoporte + 4;
                _pnlSoporte.Location = new Point(cardX, currentY);
                currentY += _pnlSoporte.Height + 10; // Margen final inferior justo debajo del último ítem

                _pnlContenedor.AutoScrollMinSize = new Size(0, currentY);

                if (_pnlContenedor.VerticalScroll.Visible && savedScroll > 0)
                {
                    _pnlContenedor.AutoScrollPosition = new Point(0, Math.Min(savedScroll, _pnlContenedor.VerticalScroll.Maximum));
                }

                _pnlAcerca.Invalidate();
                _pnlSpecs.Invalidate();
                _pnlAdmin.Invalidate();
                _pnlSoporte.Invalidate();

                _pnlContenedor.ResumeLayout(true);
            }
            finally
            {
                _isAdjustingLayout = false;
            }
        }

        private void PosicionarLabel(Label lbl, int paddingX, int controlWidth, ref int currentY, int espacioInferior)
        {
            lbl.AutoSize = false;
            lbl.Width = controlWidth;
            lbl.TextAlign = ContentAlignment.MiddleCenter;

            Size tamanoMedido = TextRenderer.MeasureText(
                lbl.Text,
                lbl.Font,
                new Size(controlWidth, int.MaxValue),
                TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl
            );

            int altoCalculado = tamanoMedido.Height + 4;
            lbl.Height = altoCalculado;
            lbl.Location = new Point(paddingX, currentY);

            currentY += altoCalculado + espacioInferior;
        }

        private class ContenedorConfigScroll : Panel
        {
            public ContenedorConfigScroll()
            {
                this.DoubleBuffered = true;
            }

            protected override Point ScrollToControl(Control activeControl)
            {
                // Evita que WinForms fuerce saltos de desplazamiento inesperados al activar foco
                return this.AutoScrollPosition;
            }
        }
    }
}

