namespace StockOS.UI.WinForms.Forms
{
    partial class FormInicio
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
            pnlHeader = new Panel();
            btnSalirApp = new Button();
            lblTitulo = new Label();
            pnlHeader.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(16, 185, 129);
            pnlHeader.Controls.Add(lblTitulo);
            pnlHeader.Controls.Add(btnSalirApp);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(800, 80);
            pnlHeader.TabIndex = 0;
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(30, 25);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(261, 32);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Bienvenido a StockOS";
            
            // 
            // btnSalirApp
            // 
            btnSalirApp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSalirApp.BackColor = Color.FromArgb(239, 68, 68);
            btnSalirApp.Cursor = Cursors.Hand;
            btnSalirApp.FlatAppearance.BorderSize = 0;
            btnSalirApp.FlatStyle = FlatStyle.Flat;
            btnSalirApp.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSalirApp.ForeColor = Color.White;
            btnSalirApp.Location = new Point(640, 20);
            btnSalirApp.Name = "btnSalirApp";
            btnSalirApp.Size = new Size(130, 40);
            btnSalirApp.TabIndex = 1;
            btnSalirApp.Text = "Cerrar Sesión";
            btnSalirApp.UseVisualStyleBackColor = false;
            // 
            // FormInicio
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(30, 41, 59);
            ClientSize = new Size(800, 600);
            Controls.Add(pnlHeader);
            Font = new Font("Segoe UI", 10F);
            Name = "FormInicio";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "StockOS - Inicio";
            WindowState = FormWindowState.Maximized;
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSalirApp;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitulo;
    }
}




