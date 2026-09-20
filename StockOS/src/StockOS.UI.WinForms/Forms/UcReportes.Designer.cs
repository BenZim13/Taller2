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
            this.pnlOpciones = new System.Windows.Forms.Panel();
            this.btnReporteCompras = new System.Windows.Forms.Button();
            this.btnReporteVentas = new System.Windows.Forms.Button();
            this.pnlFiltros = new System.Windows.Forms.Panel();
            this.btnVolver = new System.Windows.Forms.Button();
            this.pnlFiltrosCard = new System.Windows.Forms.Panel();
            this.lblTituloFiltro = new System.Windows.Forms.Label();
            this.lblPeriodo = new System.Windows.Forms.Label();
            this.cboPeriodo = new System.Windows.Forms.ComboBox();
            this.btnGenerarPdf = new System.Windows.Forms.Button();
            this.pnlOpciones.SuspendLayout();
            this.pnlFiltros.SuspendLayout();
            this.pnlFiltrosCard.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlOpciones
            // 
            this.pnlOpciones.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.pnlOpciones.Controls.Add(this.btnReporteCompras);
            this.pnlOpciones.Controls.Add(this.btnReporteVentas);
            this.pnlOpciones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlOpciones.Location = new System.Drawing.Point(0, 0);
            this.pnlOpciones.Name = "pnlOpciones";
            this.pnlOpciones.Size = new System.Drawing.Size(1000, 600);
            this.pnlOpciones.TabIndex = 0;
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
            this.btnReporteCompras.Location = new System.Drawing.Point(520, 200);
            this.btnReporteCompras.Name = "btnReporteCompras";
            this.btnReporteCompras.Size = new System.Drawing.Size(300, 150);
            this.btnReporteCompras.TabIndex = 1;
            this.btnReporteCompras.Text = "📦\r\n\r\nReporte de Compras";
            this.btnReporteCompras.UseVisualStyleBackColor = false;
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
            this.btnReporteVentas.Location = new System.Drawing.Point(180, 200);
            this.btnReporteVentas.Name = "btnReporteVentas";
            this.btnReporteVentas.Size = new System.Drawing.Size(300, 150);
            this.btnReporteVentas.TabIndex = 0;
            this.btnReporteVentas.Text = "📈\r\n\r\nReporte de Ventas";
            this.btnReporteVentas.UseVisualStyleBackColor = false;
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
            this.lblTituloFiltro.Text = "Generar Reporte de Ventas";
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
            "Día",
            "Semana",
            "Mes"});
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
            // UcReportes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.Controls.Add(this.pnlOpciones);
            this.Controls.Add(this.pnlFiltros);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "UcReportes";
            this.Size = new System.Drawing.Size(1000, 600);
            this.pnlOpciones.ResumeLayout(false);
            this.pnlFiltros.ResumeLayout(false);
            this.pnlFiltrosCard.ResumeLayout(false);
            this.pnlFiltrosCard.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlOpciones;
        private System.Windows.Forms.Button btnReporteVentas;
        private System.Windows.Forms.Button btnReporteCompras;
        private System.Windows.Forms.Panel pnlFiltros;
        private System.Windows.Forms.Panel pnlFiltrosCard;
        private System.Windows.Forms.Label lblTituloFiltro;
        private System.Windows.Forms.Label lblPeriodo;
        private System.Windows.Forms.ComboBox cboPeriodo;
        private System.Windows.Forms.Button btnGenerarPdf;
        private System.Windows.Forms.Button btnVolver;
    }
}

