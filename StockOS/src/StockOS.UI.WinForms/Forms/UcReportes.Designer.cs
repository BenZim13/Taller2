namespace StockOS.UI.WinForms.Forms
{
    partial class UcReportes
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();

            this.pnlOpciones = new System.Windows.Forms.Panel();
            this.btnGraficos = new System.Windows.Forms.Button();
            this.btnReporteCompras = new System.Windows.Forms.Button();
            this.btnReporteVentas = new System.Windows.Forms.Button();

            this.pnlFiltros = new System.Windows.Forms.Panel();
            this.btnVolver = new System.Windows.Forms.Button();
            this.pnlFiltrosCard = new System.Windows.Forms.Panel();
            this.lblTituloFiltro = new System.Windows.Forms.Label();
            this.lblPeriodo = new System.Windows.Forms.Label();
            this.cboPeriodo = new System.Windows.Forms.ComboBox();
            this.btnGenerarPdf = new System.Windows.Forms.Button();

            this.pnlGraficos = new System.Windows.Forms.Panel();
            this.pnlGraficosTop = new System.Windows.Forms.Panel();
            this.btnVolverGraficos = new System.Windows.Forms.Button();
            this.lblPeriodoGraficos = new System.Windows.Forms.Label();
            this.cboPeriodoGraficos = new System.Windows.Forms.ComboBox();
            this.lblTopN = new System.Windows.Forms.Label();
            this.numTopProductos = new System.Windows.Forms.NumericUpDown();
            this.btnGenerarGraficos = new System.Windows.Forms.Button();
            this.tlpGraficos = new System.Windows.Forms.TableLayoutPanel();
            this.chartTopProductos = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartLineaTiempo = new System.Windows.Forms.DataVisualization.Charting.Chart();

            this.pnlOpciones.SuspendLayout();
            this.pnlFiltros.SuspendLayout();
            this.pnlFiltrosCard.SuspendLayout();
            this.pnlGraficos.SuspendLayout();
            this.pnlGraficosTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTopProductos)).BeginInit();
            this.tlpGraficos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartTopProductos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartLineaTiempo)).BeginInit();
            this.SuspendLayout();

            // 
            // pnlOpciones
            // 
            this.pnlOpciones.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.pnlOpciones.Controls.Add(this.btnGraficos);
            this.pnlOpciones.Controls.Add(this.btnReporteCompras);
            this.pnlOpciones.Controls.Add(this.btnReporteVentas);
            this.pnlOpciones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlOpciones.Location = new System.Drawing.Point(0, 0);
            this.pnlOpciones.Name = "pnlOpciones";
            this.pnlOpciones.Size = new System.Drawing.Size(1000, 600);
            this.pnlOpciones.TabIndex = 0;

            // 
            // btnReporteVentas
            // 
            this.btnReporteVentas.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnReporteVentas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.btnReporteVentas.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReporteVentas.FlatAppearance.BorderSize = 0;
            this.btnReporteVentas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReporteVentas.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnReporteVentas.ForeColor = System.Drawing.Color.White;
            this.btnReporteVentas.Location = new System.Drawing.Point(180, 150);
            this.btnReporteVentas.Name = "btnReporteVentas";
            this.btnReporteVentas.Size = new System.Drawing.Size(300, 120);
            this.btnReporteVentas.TabIndex = 0;
            this.btnReporteVentas.Text = "📈\r\n\r\nReporte de Ventas";
            this.btnReporteVentas.UseVisualStyleBackColor = false;

            // 
            // btnReporteCompras
            // 
            this.btnReporteCompras.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnReporteCompras.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(88)))), ((int)(((byte)(12)))));
            this.btnReporteCompras.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReporteCompras.FlatAppearance.BorderSize = 0;
            this.btnReporteCompras.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReporteCompras.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnReporteCompras.ForeColor = System.Drawing.Color.White;
            this.btnReporteCompras.Location = new System.Drawing.Point(520, 150);
            this.btnReporteCompras.Name = "btnReporteCompras";
            this.btnReporteCompras.Size = new System.Drawing.Size(300, 120);
            this.btnReporteCompras.TabIndex = 1;
            this.btnReporteCompras.Text = "📦\r\n\r\nReporte de Compras";
            this.btnReporteCompras.UseVisualStyleBackColor = false;

            // 
            // btnGraficos
            // 
            this.btnGraficos.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnGraficos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(165)))), ((int)(((byte)(233)))));
            this.btnGraficos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGraficos.FlatAppearance.BorderSize = 0;
            this.btnGraficos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGraficos.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnGraficos.ForeColor = System.Drawing.Color.White;
            this.btnGraficos.Location = new System.Drawing.Point(350, 310);
            this.btnGraficos.Name = "btnGraficos";
            this.btnGraficos.Size = new System.Drawing.Size(300, 120);
            this.btnGraficos.TabIndex = 2;
            this.btnGraficos.Text = "📊\r\n\r\nGráficos y Estadísticas";
            this.btnGraficos.UseVisualStyleBackColor = false;

            // 
            // pnlFiltros
            // 
            this.pnlFiltros.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.pnlFiltros.Controls.Add(this.btnVolver);
            this.pnlFiltros.Controls.Add(this.pnlFiltrosCard);
            this.pnlFiltros.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFiltros.Location = new System.Drawing.Point(0, 0);
            this.pnlFiltros.Name = "pnlFiltros";
            this.pnlFiltros.Size = new System.Drawing.Size(1000, 600);
            this.pnlFiltros.TabIndex = 1;
            this.pnlFiltros.Visible = false;

            // 
            // btnVolver
            // 
            this.btnVolver.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.btnVolver.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnVolver.FlatAppearance.BorderSize = 0;
            this.btnVolver.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVolver.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnVolver.ForeColor = System.Drawing.Color.White;
            this.btnVolver.Location = new System.Drawing.Point(30, 30);
            this.btnVolver.Name = "btnVolver";
            this.btnVolver.Size = new System.Drawing.Size(100, 40);
            this.btnVolver.TabIndex = 0;
            this.btnVolver.Text = "⬅ Volver";
            this.btnVolver.UseVisualStyleBackColor = false;

            // 
            // pnlFiltrosCard
            // 
            this.pnlFiltrosCard.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pnlFiltrosCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(53)))), ((int)(((byte)(72)))));
            this.pnlFiltrosCard.Controls.Add(this.lblTituloFiltro);
            this.pnlFiltrosCard.Controls.Add(this.lblPeriodo);
            this.pnlFiltrosCard.Controls.Add(this.cboPeriodo);
            this.pnlFiltrosCard.Controls.Add(this.btnGenerarPdf);
            this.pnlFiltrosCard.Location = new System.Drawing.Point(275, 120);
            this.pnlFiltrosCard.Name = "pnlFiltrosCard";
            this.pnlFiltrosCard.Size = new System.Drawing.Size(450, 320);
            this.pnlFiltrosCard.TabIndex = 1;

            // 
            // lblTituloFiltro
            // 
            this.lblTituloFiltro.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTituloFiltro.ForeColor = System.Drawing.Color.White;
            this.lblTituloFiltro.Location = new System.Drawing.Point(20, 25);
            this.lblTituloFiltro.Name = "lblTituloFiltro";
            this.lblTituloFiltro.Size = new System.Drawing.Size(410, 40);
            this.lblTituloFiltro.TabIndex = 0;
            this.lblTituloFiltro.Text = "Generar Reporte";
            this.lblTituloFiltro.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // 
            // lblPeriodo
            // 
            this.lblPeriodo.AutoSize = true;
            this.lblPeriodo.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblPeriodo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.lblPeriodo.Location = new System.Drawing.Point(40, 90);
            this.lblPeriodo.Name = "lblPeriodo";
            this.lblPeriodo.Size = new System.Drawing.Size(155, 20);
            this.lblPeriodo.TabIndex = 1;
            this.lblPeriodo.Text = "Seleccione el período:";

            // 
            // cboPeriodo
            // 
            this.cboPeriodo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPeriodo.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.cboPeriodo.FormattingEnabled = true;
            this.cboPeriodo.Items.AddRange(new object[] {
            "Diario",
            "Semanal",
            "Mensual",
            "Bimestral",
            "Trimestral",
            "Semestral",
            "Anual"});
            this.cboPeriodo.Location = new System.Drawing.Point(40, 120);
            this.cboPeriodo.Name = "cboPeriodo";
            this.cboPeriodo.Size = new System.Drawing.Size(370, 29);
            this.cboPeriodo.TabIndex = 2;

            // 
            // btnGenerarPdf
            // 
            this.btnGenerarPdf.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(165)))), ((int)(((byte)(233)))));
            this.btnGenerarPdf.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGenerarPdf.FlatAppearance.BorderSize = 0;
            this.btnGenerarPdf.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerarPdf.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnGenerarPdf.ForeColor = System.Drawing.Color.White;
            this.btnGenerarPdf.Location = new System.Drawing.Point(40, 195);
            this.btnGenerarPdf.Name = "btnGenerarPdf";
            this.btnGenerarPdf.Size = new System.Drawing.Size(370, 50);
            this.btnGenerarPdf.TabIndex = 3;
            this.btnGenerarPdf.Text = "📄 Generar PDF";
            this.btnGenerarPdf.UseVisualStyleBackColor = false;

            // 
            // pnlGraficos
            // 
            this.pnlGraficos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.pnlGraficos.Controls.Add(this.tlpGraficos);
            this.pnlGraficos.Controls.Add(this.pnlGraficosTop);
            this.pnlGraficos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGraficos.Location = new System.Drawing.Point(0, 0);
            this.pnlGraficos.Name = "pnlGraficos";
            this.pnlGraficos.Size = new System.Drawing.Size(1000, 600);
            this.pnlGraficos.TabIndex = 2;
            this.pnlGraficos.Visible = false;

            // 
            // pnlGraficosTop
            // 
            this.pnlGraficosTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(53)))), ((int)(((byte)(72)))));
            this.pnlGraficosTop.Controls.Add(this.btnVolverGraficos);
            this.pnlGraficosTop.Controls.Add(this.lblPeriodoGraficos);
            this.pnlGraficosTop.Controls.Add(this.cboPeriodoGraficos);
            this.pnlGraficosTop.Controls.Add(this.lblTopN);
            this.pnlGraficosTop.Controls.Add(this.numTopProductos);
            this.pnlGraficosTop.Controls.Add(this.btnGenerarGraficos);
            this.pnlGraficosTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlGraficosTop.Location = new System.Drawing.Point(0, 0);
            this.pnlGraficosTop.Name = "pnlGraficosTop";
            this.pnlGraficosTop.Size = new System.Drawing.Size(1000, 70);
            this.pnlGraficosTop.TabIndex = 0;

            // 
            // btnVolverGraficos
            // 
            this.btnVolverGraficos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.btnVolverGraficos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnVolverGraficos.FlatAppearance.BorderSize = 0;
            this.btnVolverGraficos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVolverGraficos.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnVolverGraficos.ForeColor = System.Drawing.Color.White;
            this.btnVolverGraficos.Location = new System.Drawing.Point(15, 15);
            this.btnVolverGraficos.Name = "btnVolverGraficos";
            this.btnVolverGraficos.Size = new System.Drawing.Size(90, 40);
            this.btnVolverGraficos.TabIndex = 0;
            this.btnVolverGraficos.Text = "⬅ Volver";
            this.btnVolverGraficos.UseVisualStyleBackColor = false;

            // 
            // lblPeriodoGraficos
            // 
            this.lblPeriodoGraficos.AutoSize = true;
            this.lblPeriodoGraficos.Location = new System.Drawing.Point(130, 26);
            this.lblPeriodoGraficos.Name = "lblPeriodoGraficos";
            this.lblPeriodoGraficos.Size = new System.Drawing.Size(59, 19);
            this.lblPeriodoGraficos.TabIndex = 1;
            this.lblPeriodoGraficos.Text = "Período:";

            // 
            // cboPeriodoGraficos
            // 
            this.cboPeriodoGraficos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPeriodoGraficos.FormattingEnabled = true;
            this.cboPeriodoGraficos.Items.AddRange(new object[] {
            "Semanal",
            "Mensual",
            "Bimestral",
            "Trimestral",
            "Semestral",
            "9 Meses",
            "Anual"});
            this.cboPeriodoGraficos.Location = new System.Drawing.Point(195, 23);
            this.cboPeriodoGraficos.Name = "cboPeriodoGraficos";
            this.cboPeriodoGraficos.Size = new System.Drawing.Size(120, 25);
            this.cboPeriodoGraficos.TabIndex = 2;

            // 
            // lblTopN
            // 
            this.lblTopN.AutoSize = true;
            this.lblTopN.Location = new System.Drawing.Point(340, 26);
            this.lblTopN.Name = "lblTopN";
            this.lblTopN.Size = new System.Drawing.Size(111, 19);
            this.lblTopN.TabIndex = 3;
            this.lblTopN.Text = "Cant. Productos:";

            // 
            // numTopProductos
            // 
            this.numTopProductos.Location = new System.Drawing.Point(460, 24);
            this.numTopProductos.Minimum = new decimal(new int[] { 3, 0, 0, 0 });
            this.numTopProductos.Maximum = new decimal(new int[] { 50, 0, 0, 0 });
            this.numTopProductos.Name = "numTopProductos";
            this.numTopProductos.Size = new System.Drawing.Size(70, 25);
            this.numTopProductos.TabIndex = 4;
            this.numTopProductos.Value = new decimal(new int[] { 5, 0, 0, 0 });

            // 
            // btnGenerarGraficos
            // 
            this.btnGenerarGraficos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.btnGenerarGraficos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGenerarGraficos.FlatAppearance.BorderSize = 0;
            this.btnGenerarGraficos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerarGraficos.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnGenerarGraficos.ForeColor = System.Drawing.Color.White;
            this.btnGenerarGraficos.Location = new System.Drawing.Point(560, 15);
            this.btnGenerarGraficos.Name = "btnGenerarGraficos";
            this.btnGenerarGraficos.Size = new System.Drawing.Size(160, 40);
            this.btnGenerarGraficos.TabIndex = 5;
            this.btnGenerarGraficos.Text = "Cargar Gráficos";
            this.btnGenerarGraficos.UseVisualStyleBackColor = false;

            // 
            // tlpGraficos
            // 
            this.tlpGraficos.ColumnCount = 2;
            this.tlpGraficos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpGraficos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpGraficos.Controls.Add(this.chartTopProductos, 0, 0);
            this.tlpGraficos.Controls.Add(this.chartLineaTiempo, 1, 0);
            this.tlpGraficos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpGraficos.Location = new System.Drawing.Point(0, 70);
            this.tlpGraficos.Name = "tlpGraficos";
            this.tlpGraficos.RowCount = 1;
            this.tlpGraficos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpGraficos.Size = new System.Drawing.Size(1000, 530);
            this.tlpGraficos.TabIndex = 1;

            // 
            // chartTopProductos
            // 
            this.chartTopProductos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            chartArea1.BackColor = System.Drawing.Color.Transparent;
            chartArea1.Name = "ChartArea1";
            this.chartTopProductos.ChartAreas.Add(chartArea1);
            this.chartTopProductos.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.BackColor = System.Drawing.Color.Transparent;
            legend1.ForeColor = System.Drawing.Color.White;
            legend1.Name = "Legend1";
            this.chartTopProductos.Legends.Add(legend1);
            this.chartTopProductos.Location = new System.Drawing.Point(3, 3);
            this.chartTopProductos.Name = "chartTopProductos";
            this.chartTopProductos.Size = new System.Drawing.Size(494, 524);
            this.chartTopProductos.TabIndex = 0;
            this.chartTopProductos.Text = "chart1";

            // 
            // chartLineaTiempo
            // 
            this.chartLineaTiempo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            chartArea2.AxisX.LabelStyle.ForeColor = System.Drawing.Color.White;
            chartArea2.AxisX.LineColor = System.Drawing.Color.White;
            chartArea2.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            chartArea2.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            chartArea2.AxisY.LabelStyle.ForeColor = System.Drawing.Color.White;
            chartArea2.AxisY.LineColor = System.Drawing.Color.White;
            chartArea2.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            chartArea2.AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            chartArea2.BackColor = System.Drawing.Color.Transparent;
            chartArea2.Name = "ChartArea1";
            this.chartLineaTiempo.ChartAreas.Add(chartArea2);
            this.chartLineaTiempo.Dock = System.Windows.Forms.DockStyle.Fill;
            legend2.BackColor = System.Drawing.Color.Transparent;
            legend2.ForeColor = System.Drawing.Color.White;
            legend2.Name = "Legend1";
            this.chartLineaTiempo.Legends.Add(legend2);
            this.chartLineaTiempo.Location = new System.Drawing.Point(503, 3);
            this.chartLineaTiempo.Name = "chartLineaTiempo";
            this.chartLineaTiempo.Size = new System.Drawing.Size(494, 524);
            this.chartLineaTiempo.TabIndex = 1;
            this.chartLineaTiempo.Text = "chart2";

            // 
            // UcReportes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.Controls.Add(this.pnlOpciones);
            this.Controls.Add(this.pnlFiltros);
            this.Controls.Add(this.pnlGraficos);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "UcReportes";
            this.Size = new System.Drawing.Size(1000, 600);
            this.pnlOpciones.ResumeLayout(false);
            this.pnlFiltros.ResumeLayout(false);
            this.pnlFiltrosCard.ResumeLayout(false);
            this.pnlFiltrosCard.PerformLayout();
            this.pnlGraficos.ResumeLayout(false);
            this.pnlGraficosTop.ResumeLayout(false);
            this.pnlGraficosTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTopProductos)).EndInit();
            this.tlpGraficos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartTopProductos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartLineaTiempo)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlOpciones;
        private System.Windows.Forms.Button btnReporteVentas;
        private System.Windows.Forms.Button btnReporteCompras;
        private System.Windows.Forms.Button btnGraficos;
        private System.Windows.Forms.Panel pnlFiltros;
        private System.Windows.Forms.Panel pnlFiltrosCard;
        private System.Windows.Forms.Label lblTituloFiltro;
        private System.Windows.Forms.Label lblPeriodo;
        private System.Windows.Forms.ComboBox cboPeriodo;
        private System.Windows.Forms.Button btnGenerarPdf;
        private System.Windows.Forms.Button btnVolver;
        private System.Windows.Forms.Panel pnlGraficos;
        private System.Windows.Forms.Panel pnlGraficosTop;
        private System.Windows.Forms.Button btnVolverGraficos;
        private System.Windows.Forms.Label lblPeriodoGraficos;
        private System.Windows.Forms.ComboBox cboPeriodoGraficos;
        private System.Windows.Forms.Label lblTopN;
        private System.Windows.Forms.NumericUpDown numTopProductos;
        private System.Windows.Forms.Button btnGenerarGraficos;
        private System.Windows.Forms.TableLayoutPanel tlpGraficos;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartTopProductos;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartLineaTiempo;
    }
}