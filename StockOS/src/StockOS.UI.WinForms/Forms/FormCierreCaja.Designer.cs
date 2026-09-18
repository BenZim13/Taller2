namespace StockOS.UI.WinForms.Forms
{
    partial class FormCierreCaja
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
            lblIndicacion = new System.Windows.Forms.Label();
            txtMontoReal = new System.Windows.Forms.TextBox();
            btnConfirmarCierre = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            lblTitulo.ForeColor = System.Drawing.Color.FromArgb(239, 68, 68);
            lblTitulo.Location = new System.Drawing.Point(20, 20);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new System.Drawing.Size(161, 30);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Cierre de Caja";
            // 
            // lblIndicacion
            // 
            lblIndicacion.AutoSize = true;
            lblIndicacion.Font = new System.Drawing.Font("Segoe UI", 12F);
            lblIndicacion.ForeColor = System.Drawing.Color.White;
            lblIndicacion.Location = new System.Drawing.Point(20, 70);
            lblIndicacion.Name = "lblIndicacion";
            lblIndicacion.Size = new System.Drawing.Size(280, 21);
            lblIndicacion.TabIndex = 1;
            lblIndicacion.Text = "Ingrese el dinero físico en la caja ($):";
            // 
            // txtMontoReal
            // 
            txtMontoReal.BackColor = System.Drawing.Color.FromArgb(15, 23, 42);
            txtMontoReal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtMontoReal.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            txtMontoReal.ForeColor = System.Drawing.Color.White;
            txtMontoReal.Location = new System.Drawing.Point(24, 100);
            txtMontoReal.Name = "txtMontoReal";
            txtMontoReal.Size = new System.Drawing.Size(320, 43);
            txtMontoReal.TabIndex = 2;
            txtMontoReal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnConfirmarCierre
            // 
            btnConfirmarCierre.BackColor = System.Drawing.Color.FromArgb(239, 68, 68);
            btnConfirmarCierre.FlatAppearance.BorderSize = 0;
            btnConfirmarCierre.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnConfirmarCierre.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            btnConfirmarCierre.ForeColor = System.Drawing.Color.White;
            btnConfirmarCierre.Location = new System.Drawing.Point(24, 160);
            btnConfirmarCierre.Name = "btnConfirmarCierre";
            btnConfirmarCierre.Size = new System.Drawing.Size(320, 50);
            btnConfirmarCierre.TabIndex = 3;
            btnConfirmarCierre.Text = "CERRAR TURNO";
            btnConfirmarCierre.UseVisualStyleBackColor = false;
            btnConfirmarCierre.Click += btnConfirmarCierre_Click;
            // 
            // FormCierreCaja
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            ClientSize = new System.Drawing.Size(364, 241);
            Controls.Add(btnConfirmarCierre);
            Controls.Add(txtMontoReal);
            Controls.Add(lblIndicacion);
            Controls.Add(lblTitulo);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormCierreCaja";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Cerrar Caja";
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblIndicacion;
        private System.Windows.Forms.TextBox txtMontoReal;
        private System.Windows.Forms.Button btnConfirmarCierre;
    }
}