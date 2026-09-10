namespace Vistas_Usuarios
{
    partial class FormLogin
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
            lblTitulo = new Label();
            btnSalir = new Button();
            btnRecargar = new Button();
            btnIngresar = new Button();
            txtPassword = new TextBox();
            lblPassword = new Label();
            txtUsuario = new TextBox();
            lblUsuario = new Label();
            lblError = new Label();
            pnlHeader.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(16, 185, 129);
            pnlHeader.Controls.Add(lblTitulo);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(434, 80);
            pnlHeader.TabIndex = 0;
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(30, 25);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(325, 41);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Bienvenido a StockOS";
            // 
            // btnSalir
            // 
            btnSalir.Anchor = AnchorStyles.None;
            btnSalir.BackColor = Color.FromArgb(239, 68, 68);
            btnSalir.Cursor = Cursors.Hand;
            btnSalir.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            btnSalir.FlatStyle = FlatStyle.Flat;
            btnSalir.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSalir.ForeColor = Color.White;
            btnSalir.Location = new Point(282, 400);
            btnSalir.Name = "btnSalir";
            btnSalir.Size = new Size(105, 40);
            btnSalir.TabIndex = 7;
            btnSalir.Text = "Salir";
            btnSalir.UseVisualStyleBackColor = false;
            btnSalir.Click += btnSalir_Click;
            // 
            // btnRecargar
            // 
            btnRecargar.Anchor = AnchorStyles.None;
            btnRecargar.BackColor = Color.FromArgb(71, 85, 105);
            btnRecargar.Cursor = Cursors.Hand;
            btnRecargar.FlatAppearance.BorderSize = 0;
            btnRecargar.FlatStyle = FlatStyle.Flat;
            btnRecargar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnRecargar.ForeColor = Color.White;
            btnRecargar.Location = new Point(162, 400);
            btnRecargar.Name = "btnRecargar";
            btnRecargar.Size = new Size(105, 40);
            btnRecargar.TabIndex = 6;
            btnRecargar.Text = "Recargar";
            btnRecargar.UseVisualStyleBackColor = false;
            btnRecargar.Click += btnRecargar_Click;
            // 
            // btnIngresar
            // 
            btnIngresar.Anchor = AnchorStyles.None;
            btnIngresar.BackColor = Color.FromArgb(59, 130, 246);
            btnIngresar.Cursor = Cursors.Hand;
            btnIngresar.FlatAppearance.BorderSize = 0;
            btnIngresar.FlatStyle = FlatStyle.Flat;
            btnIngresar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnIngresar.ForeColor = Color.White;
            btnIngresar.Location = new Point(42, 400);
            btnIngresar.Name = "btnIngresar";
            btnIngresar.Size = new Size(105, 40);
            btnIngresar.TabIndex = 5;
            btnIngresar.Text = "Ingresar";
            btnIngresar.UseVisualStyleBackColor = false;
            btnIngresar.Click += btnIngresar_Click;
            // 
            // txtPassword
            // 
            txtPassword.Anchor = AnchorStyles.None;
            txtPassword.BackColor = Color.FromArgb(51, 65, 85);
            txtPassword.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.ForeColor = Color.White;
            txtPassword.Location = new Point(40, 295);
            txtPassword.MaxLength = 50;
            txtPassword.Name = "txtPassword";
            txtPassword.PlaceholderText = "••••••••";
            txtPassword.Size = new Size(350, 30);
            txtPassword.TabIndex = 4;
            txtPassword.UseSystemPasswordChar = true;
            // 
            // lblPassword
            // 
            lblPassword.Anchor = AnchorStyles.None;
            lblPassword.AutoSize = true;
            lblPassword.ForeColor = Color.White;
            lblPassword.Location = new Point(40, 270);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(97, 23);
            lblPassword.TabIndex = 3;
            lblPassword.Text = "Contraseña";
            // 
            // txtUsuario
            // 
            txtUsuario.Anchor = AnchorStyles.None;
            txtUsuario.BackColor = Color.FromArgb(51, 65, 85);
            txtUsuario.BorderStyle = BorderStyle.FixedSingle;
            txtUsuario.ForeColor = Color.White;
            txtUsuario.Location = new Point(40, 205);
            txtUsuario.MaxLength = 20;
            txtUsuario.Name = "txtUsuario";
            txtUsuario.PlaceholderText = "Ej: 11111111";
            txtUsuario.Size = new Size(350, 30);
            txtUsuario.TabIndex = 2;
            // 
            // lblUsuario
            // 
            lblUsuario.Anchor = AnchorStyles.None;
            lblUsuario.AutoSize = true;
            lblUsuario.ForeColor = Color.White;
            lblUsuario.Location = new Point(40, 180);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.Size = new Size(103, 23);
            lblUsuario.TabIndex = 1;
            lblUsuario.Text = "DNI Usuario";
            // 
            // lblError
            // 
            lblError.Anchor = AnchorStyles.None;
            lblError.ForeColor = Color.FromArgb(239, 68, 68);
            lblError.Location = new Point(40, 330);
            lblError.Name = "lblError";
            lblError.Size = new Size(350, 50);
            lblError.TabIndex = 7;
            lblError.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // FormLogin
            // 
            AcceptButton = btnIngresar;
            AutoScaleDimensions = new SizeF(9F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(30, 41, 59);
            CancelButton = btnSalir;
            ClientSize = new Size(434, 511);
            Controls.Add(lblError);
            Controls.Add(btnSalir);
            Controls.Add(btnRecargar);
            Controls.Add(btnIngresar);
            Controls.Add(txtPassword);
            Controls.Add(lblPassword);
            Controls.Add(txtUsuario);
            Controls.Add(lblUsuario);
            Controls.Add(pnlHeader);
            Font = new Font("Segoe UI", 10F);
            Name = "FormLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "StockOS - Iniciar Sesión";
            WindowState = FormWindowState.Maximized;
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Button btnSalir;
        private System.Windows.Forms.Button btnRecargar;
        private System.Windows.Forms.Button btnIngresar;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtUsuario;
        private System.Windows.Forms.Label lblUsuario;
        private System.Windows.Forms.Label lblError;
    }
}





