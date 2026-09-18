namespace StockOS.UI.WinForms.Forms
{
    partial class FormMovimientoCaja
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

        private void InitializeComponent()
        {
            lblTitulo = new System.Windows.Forms.Label();
            lblTipo = new System.Windows.Forms.Label();
            cmbTipo = new System.Windows.Forms.ComboBox();
            lblMonto = new System.Windows.Forms.Label();
            txtMonto = new System.Windows.Forms.TextBox();
            lblDescripcion = new System.Windows.Forms.Label();
            txtDescripcion = new System.Windows.Forms.TextBox();
            btnGuardar = new System.Windows.Forms.Button();
            btnCancelar = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            lblTitulo.ForeColor = System.Drawing.Color.White;
            lblTitulo.Location = new System.Drawing.Point(20, 20);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new System.Drawing.Size(228, 30);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Movimiento de Caja";
            // 
            // lblTipo
            // 
            lblTipo.AutoSize = true;
            lblTipo.ForeColor = System.Drawing.Color.White;
            lblTipo.Location = new System.Drawing.Point(25, 75);
            lblTipo.Name = "lblTipo";
            lblTipo.Size = new System.Drawing.Size(33, 15);
            lblTipo.TabIndex = 1;
            lblTipo.Text = "Tipo:";
            // 
            // cmbTipo
            // 
            cmbTipo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbTipo.FormattingEnabled = true;
            cmbTipo.Items.AddRange(new object[] { "EGRESO", "INGRESO" });
            cmbTipo.Location = new System.Drawing.Point(25, 95);
            cmbTipo.Name = "cmbTipo";
            cmbTipo.Size = new System.Drawing.Size(121, 23);
            cmbTipo.TabIndex = 2;
            // 
            // lblMonto
            // 
            lblMonto.AutoSize = true;
            lblMonto.ForeColor = System.Drawing.Color.White;
            lblMonto.Location = new System.Drawing.Point(170, 75);
            lblMonto.Name = "lblMonto";
            lblMonto.Size = new System.Drawing.Size(59, 15);
            lblMonto.TabIndex = 3;
            lblMonto.Text = "Monto ($):";
            // 
            // txtMonto
            // 
            txtMonto.Location = new System.Drawing.Point(170, 95);
            txtMonto.Name = "txtMonto";
            txtMonto.Size = new System.Drawing.Size(145, 23);
            txtMonto.TabIndex = 4;
            txtMonto.KeyPress += txtMonto_KeyPress;
            // 
            // lblDescripcion
            // 
            lblDescripcion.AutoSize = true;
            lblDescripcion.ForeColor = System.Drawing.Color.White;
            lblDescripcion.Location = new System.Drawing.Point(25, 135);
            lblDescripcion.Name = "lblDescripcion";
            lblDescripcion.Size = new System.Drawing.Size(72, 15);
            lblDescripcion.TabIndex = 5;
            lblDescripcion.Text = "Descripción:";
            // 
            // txtDescripcion
            // 
            txtDescripcion.Location = new System.Drawing.Point(25, 155);
            txtDescripcion.Name = "txtDescripcion";
            txtDescripcion.Size = new System.Drawing.Size(290, 23);
            txtDescripcion.TabIndex = 6;
            // 
            // btnGuardar
            // 
            btnGuardar.BackColor = System.Drawing.Color.FromArgb(16, 185, 129);
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnGuardar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            btnGuardar.ForeColor = System.Drawing.Color.White;
            btnGuardar.Location = new System.Drawing.Point(215, 205);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new System.Drawing.Size(100, 35);
            btnGuardar.TabIndex = 7;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = false;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // btnCancelar
            // 
            btnCancelar.BackColor = System.Drawing.Color.FromArgb(239, 68, 68);
            btnCancelar.FlatAppearance.BorderSize = 0;
            btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnCancelar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            btnCancelar.ForeColor = System.Drawing.Color.White;
            btnCancelar.Location = new System.Drawing.Point(105, 205);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new System.Drawing.Size(100, 35);
            btnCancelar.TabIndex = 8;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // FormMovimientoCaja
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            ClientSize = new System.Drawing.Size(344, 261);
            Controls.Add(btnCancelar);
            Controls.Add(btnGuardar);
            Controls.Add(txtDescripcion);
            Controls.Add(lblDescripcion);
            Controls.Add(txtMonto);
            Controls.Add(lblMonto);
            Controls.Add(cmbTipo);
            Controls.Add(lblTipo);
            Controls.Add(lblTitulo);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormMovimientoCaja";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Movimiento de Caja";
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblTipo;
        private System.Windows.Forms.ComboBox cmbTipo;
        private System.Windows.Forms.Label lblMonto;
        private System.Windows.Forms.TextBox txtMonto;
        private System.Windows.Forms.Label lblDescripcion;
        private System.Windows.Forms.TextBox txtDescripcion;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnCancelar;
    }
}