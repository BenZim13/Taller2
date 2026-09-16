namespace StockOS.UI.WinForms.Forms
{
    partial class FormAperturaCaja
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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            lblTitulo = new System.Windows.Forms.Label();
            pnlHeader = new System.Windows.Forms.Panel();
            lblCaja = new System.Windows.Forms.Label();
            cmbCaja = new System.Windows.Forms.ComboBox();
            lblMonto = new System.Windows.Forms.Label();
            txtMonto = new System.Windows.Forms.TextBox();
            btnAbrir = new System.Windows.Forms.Button();
            btnCancelar = new System.Windows.Forms.Button();
            lblMoneda = new System.Windows.Forms.Label();
            pnlHeader.SuspendLayout();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            lblTitulo.ForeColor = System.Drawing.Color.White;
            lblTitulo.Location = new System.Drawing.Point(12, 18);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new System.Drawing.Size(163, 25);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Apertura de Caja";
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = System.Drawing.Color.FromArgb(16, 185, 129);
            pnlHeader.Controls.Add(lblTitulo);
            pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            pnlHeader.Location = new System.Drawing.Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new System.Drawing.Size(350, 60);
            pnlHeader.TabIndex = 0;
            // 
            // lblCaja
            // 
            lblCaja.AutoSize = true;
            lblCaja.Font = new System.Drawing.Font("Segoe UI", 11F);
            lblCaja.ForeColor = System.Drawing.Color.White;
            lblCaja.Location = new System.Drawing.Point(30, 80);
            lblCaja.Name = "lblCaja";
            lblCaja.Size = new System.Drawing.Size(123, 20);
            lblCaja.TabIndex = 6;
            lblCaja.Text = "Seleccione Caja:";
            // 
            // cmbCaja
            // 
            cmbCaja.BackColor = System.Drawing.Color.FromArgb(15, 23, 42);
            cmbCaja.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbCaja.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            cmbCaja.Font = new System.Drawing.Font("Segoe UI", 12F);
            cmbCaja.ForeColor = System.Drawing.Color.White;
            cmbCaja.FormattingEnabled = true;
            cmbCaja.Location = new System.Drawing.Point(30, 105);
            cmbCaja.Name = "cmbCaja";
            cmbCaja.Size = new System.Drawing.Size(280, 29);
            cmbCaja.TabIndex = 7;
            // 
            // lblMonto
            // 
            lblMonto.AutoSize = true;
            lblMonto.Font = new System.Drawing.Font("Segoe UI", 11F);
            lblMonto.ForeColor = System.Drawing.Color.White;
            lblMonto.Location = new System.Drawing.Point(30, 150);
            lblMonto.Name = "lblMonto";
            lblMonto.Size = new System.Drawing.Size(229, 20);
            lblMonto.TabIndex = 1;
            lblMonto.Text = "Monto Inicial (Cambio en billetes):";
            // 
            // txtMonto
            // 
            txtMonto.BackColor = System.Drawing.Color.FromArgb(15, 23, 42);
            txtMonto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtMonto.Font = new System.Drawing.Font("Segoe UI", 14F);
            txtMonto.ForeColor = System.Drawing.Color.White;
            txtMonto.Location = new System.Drawing.Point(60, 175);
            txtMonto.Name = "txtMonto";
            txtMonto.Size = new System.Drawing.Size(250, 32);
            txtMonto.TabIndex = 2;
            txtMonto.KeyPress += txtMonto_KeyPress;
            // 
            // lblMoneda
            // 
            lblMoneda.AutoSize = true;
            lblMoneda.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            lblMoneda.ForeColor = System.Drawing.Color.White;
            lblMoneda.Location = new System.Drawing.Point(30, 177);
            lblMoneda.Name = "lblMoneda";
            lblMoneda.Size = new System.Drawing.Size(23, 25);
            lblMoneda.TabIndex = 5;
            lblMoneda.Text = "$";
            // 
            // btnAbrir
            // 
            btnAbrir.BackColor = System.Drawing.Color.FromArgb(16, 185, 129);
            btnAbrir.Cursor = System.Windows.Forms.Cursors.Hand;
            btnAbrir.FlatAppearance.BorderSize = 0;
            btnAbrir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnAbrir.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnAbrir.ForeColor = System.Drawing.Color.White;
            btnAbrir.Location = new System.Drawing.Point(190, 240);
            btnAbrir.Name = "btnAbrir";
            btnAbrir.Size = new System.Drawing.Size(120, 40);
            btnAbrir.TabIndex = 3;
            btnAbrir.Text = "Abrir Caja";
            btnAbrir.UseVisualStyleBackColor = false;
            btnAbrir.Click += btnAbrir_Click;
            // 
            // btnCancelar
            // 
            btnCancelar.BackColor = System.Drawing.Color.FromArgb(148, 163, 184);
            btnCancelar.Cursor = System.Windows.Forms.Cursors.Hand;
            btnCancelar.FlatAppearance.BorderSize = 0;
            btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnCancelar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnCancelar.ForeColor = System.Drawing.Color.White;
            btnCancelar.Location = new System.Drawing.Point(40, 240);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new System.Drawing.Size(120, 40);
            btnCancelar.TabIndex = 4;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // FormAperturaCaja
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            ClientSize = new System.Drawing.Size(350, 310);
            Controls.Add(cmbCaja);
            Controls.Add(lblCaja);
            Controls.Add(lblMoneda);
            Controls.Add(btnCancelar);
            Controls.Add(btnAbrir);
            Controls.Add(txtMonto);
            Controls.Add(lblMonto);
            Controls.Add(pnlHeader);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormAperturaCaja";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Abrir Caja";
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblCaja;
        private System.Windows.Forms.ComboBox cmbCaja;
        private System.Windows.Forms.Label lblMonto;
        private System.Windows.Forms.TextBox txtMonto;
        private System.Windows.Forms.Button btnAbrir;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Label lblMoneda;
    }
}