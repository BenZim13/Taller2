namespace StockOS.UI.WinForms.Forms
{
    partial class FormRegistroUsuario
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
            btnVolver = new Button();
            lblTitulo = new Label();
            lblDNI = new Label();
            txtDNI = new TextBox();
            lblNombre = new Label();
            txtNombre = new TextBox();
            lblDireccion = new Label();
            txtDireccion = new TextBox();
            lblCelular = new Label();
            txtCelular = new TextBox();
            lblRol = new Label();
            cmbRol = new ComboBox();
            lblCodigo = new Label();
            txtPassword = new TextBox();
            btnGuardar = new Button();
            btnCancelar = new Button();
            textApellido = new TextBox();
            lblApellido = new Label();
            textEmail = new TextBox();
            lblEmail = new Label();
            lblSucursal = new Label();
            cmbSucursal = new ComboBox();
            pnlHeader.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(30, 41, 59);
            pnlHeader.Controls.Add(btnVolver);
            pnlHeader.Controls.Add(lblTitulo);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Padding = new Padding(25, 10, 25, 5);
            pnlHeader.Size = new Size(434, 50);
            pnlHeader.TabIndex = 0;
            // 
            // btnVolver
            // 
            btnVolver.BackColor = Color.FromArgb(51, 65, 85);
            btnVolver.Cursor = Cursors.Hand;
            btnVolver.FlatAppearance.BorderSize = 0;
            btnVolver.FlatStyle = FlatStyle.Flat;
            btnVolver.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnVolver.ForeColor = Color.White;
            btnVolver.Location = new Point(25, 10);
            btnVolver.Name = "btnVolver";
            btnVolver.Size = new Size(80, 32);
            btnVolver.TabIndex = 4;
            btnVolver.Text = "← Volver";
            btnVolver.UseVisualStyleBackColor = false;
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(120, 7);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(237, 30);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Registro de Usuario";
            // 
            // lblDNI
            // 
            lblDNI.Anchor = AnchorStyles.None;
            lblDNI.AutoSize = true;
            lblDNI.ForeColor = Color.White;
            lblDNI.Location = new Point(40, 111);
            lblDNI.Name = "lblDNI";
            lblDNI.Size = new Size(33, 19);
            lblDNI.TabIndex = 1;
            lblDNI.Text = "DNI";
            // 
            // txtDNI
            // 
            txtDNI.Anchor = AnchorStyles.None;
            txtDNI.BackColor = Color.FromArgb(51, 65, 85);
            txtDNI.BorderStyle = BorderStyle.FixedSingle;
            txtDNI.ForeColor = Color.White;
            txtDNI.Location = new Point(40, 134);
            txtDNI.Name = "txtDNI";
            txtDNI.Size = new Size(350, 25);
            txtDNI.TabIndex = 2;
            // 
            // lblNombre
            // 
            lblNombre.Anchor = AnchorStyles.None;
            lblNombre.AutoSize = true;
            lblNombre.ForeColor = Color.White;
            lblNombre.Location = new Point(40, 162);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(59, 19);
            lblNombre.TabIndex = 3;
            lblNombre.Text = "Nombre";
            // 
            // txtNombre
            // 
            txtNombre.Anchor = AnchorStyles.None;
            txtNombre.BackColor = Color.FromArgb(51, 65, 85);
            txtNombre.BorderStyle = BorderStyle.FixedSingle;
            txtNombre.ForeColor = Color.White;
            txtNombre.Location = new Point(40, 185);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(350, 25);
            txtNombre.TabIndex = 4;
            // 
            // lblDireccion
            // 
            lblDireccion.Anchor = AnchorStyles.None;
            lblDireccion.AutoSize = true;
            lblDireccion.ForeColor = Color.White;
            lblDireccion.Location = new Point(40, 315);
            lblDireccion.Name = "lblDireccion";
            lblDireccion.Size = new Size(65, 19);
            lblDireccion.TabIndex = 5;
            lblDireccion.Text = "Dirección";
            // 
            // txtDireccion
            // 
            txtDireccion.Anchor = AnchorStyles.None;
            txtDireccion.BackColor = Color.FromArgb(51, 65, 85);
            txtDireccion.BorderStyle = BorderStyle.FixedSingle;
            txtDireccion.ForeColor = Color.White;
            txtDireccion.Location = new Point(40, 338);
            txtDireccion.MaxLength = 100;
            txtDireccion.Name = "txtDireccion";
            txtDireccion.Size = new Size(350, 25);
            txtDireccion.TabIndex = 6;
            // 
            // lblCelular
            // 
            lblCelular.Anchor = AnchorStyles.None;
            lblCelular.AutoSize = true;
            lblCelular.ForeColor = Color.White;
            lblCelular.Location = new Point(40, 366);
            lblCelular.Name = "lblCelular";
            lblCelular.Size = new Size(51, 19);
            lblCelular.TabIndex = 7;
            lblCelular.Text = "Celular";
            // 
            // txtCelular
            // 
            txtCelular.Anchor = AnchorStyles.None;
            txtCelular.BackColor = Color.FromArgb(51, 65, 85);
            txtCelular.BorderStyle = BorderStyle.FixedSingle;
            txtCelular.ForeColor = Color.White;
            txtCelular.Location = new Point(40, 389);
            txtCelular.MaxLength = 20;
            txtCelular.Name = "txtCelular";
            txtCelular.PlaceholderText = "Ej: 1123456789";
            txtCelular.Size = new Size(350, 25);
            txtCelular.TabIndex = 8;
            // 
            // lblRol
            // 
            lblRol.Anchor = AnchorStyles.None;
            lblRol.AutoSize = true;
            lblRol.ForeColor = Color.White;
            lblRol.Location = new Point(40, 417);
            lblRol.Name = "lblRol";
            lblRol.Size = new Size(28, 19);
            lblRol.TabIndex = 9;
            lblRol.Text = "Rol";
            // 
            // cmbRol
            // 
            cmbRol.Anchor = AnchorStyles.None;
            cmbRol.BackColor = Color.FromArgb(51, 65, 85);
            cmbRol.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRol.FlatStyle = FlatStyle.Flat;
            cmbRol.ForeColor = Color.White;
            cmbRol.FormattingEnabled = true;
            cmbRol.Location = new Point(40, 440);
            cmbRol.Name = "cmbRol";
            cmbRol.Size = new Size(350, 25);
            cmbRol.TabIndex = 10;
            // 
            // lblCodigo
            // 
            lblCodigo.Anchor = AnchorStyles.None;
            lblCodigo.AutoSize = true;
            lblCodigo.ForeColor = Color.White;
            lblCodigo.Location = new Point(40, 538);
            lblCodigo.Name = "lblCodigo";
            lblCodigo.Size = new Size(144, 19);
            lblCodigo.TabIndex = 11;
            lblCodigo.Text = "Contraseña (8 dígitos)";
            // 
            // txtPassword
            // 
            txtPassword.Anchor = AnchorStyles.None;
            txtPassword.BackColor = Color.FromArgb(51, 65, 85);
            txtPassword.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.ForeColor = Color.White;
            txtPassword.Location = new Point(40, 561);
            txtPassword.MaxLength = 8;
            txtPassword.Name = "txtPassword";
            txtPassword.PlaceholderText = "••••••••";
            txtPassword.Size = new Size(350, 25);
            txtPassword.TabIndex = 12;
            txtPassword.UseSystemPasswordChar = true;
            // 
            // btnGuardar
            // 
            btnGuardar.Anchor = AnchorStyles.None;
            btnGuardar.BackColor = Color.FromArgb(59, 130, 246);
            btnGuardar.Cursor = Cursors.Hand;
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Location = new Point(40, 592);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(120, 40);
            btnGuardar.TabIndex = 13;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = false;
            // 
            // btnCancelar
            // 
            btnCancelar.Anchor = AnchorStyles.None;
            btnCancelar.BackColor = Color.FromArgb(239, 68, 68);
            btnCancelar.Cursor = Cursors.Hand;
            btnCancelar.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnCancelar.ForeColor = Color.White;
            btnCancelar.Location = new Point(270, 592);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(120, 40);
            btnCancelar.TabIndex = 14;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            // 
            // textApellido
            // 
            textApellido.Anchor = AnchorStyles.None;
            textApellido.BackColor = Color.FromArgb(51, 65, 85);
            textApellido.BorderStyle = BorderStyle.FixedSingle;
            textApellido.ForeColor = Color.White;
            textApellido.Location = new Point(40, 236);
            textApellido.Name = "textApellido";
            textApellido.Size = new Size(350, 25);
            textApellido.TabIndex = 16;
            // 
            // lblApellido
            // 
            lblApellido.Anchor = AnchorStyles.None;
            lblApellido.AutoSize = true;
            lblApellido.ForeColor = Color.White;
            lblApellido.Location = new Point(40, 213);
            lblApellido.Name = "lblApellido";
            lblApellido.Size = new Size(58, 19);
            lblApellido.TabIndex = 15;
            lblApellido.Text = "Apellido";
            // 
            // textEmail
            // 
            textEmail.Anchor = AnchorStyles.None;
            textEmail.BackColor = Color.FromArgb(51, 65, 85);
            textEmail.BorderStyle = BorderStyle.FixedSingle;
            textEmail.ForeColor = Color.White;
            textEmail.Location = new Point(40, 287);
            textEmail.Name = "textEmail";
            textEmail.Size = new Size(350, 25);
            textEmail.TabIndex = 18;
            // 
            // lblEmail
            // 
            lblEmail.Anchor = AnchorStyles.None;
            lblEmail.AutoSize = true;
            lblEmail.ForeColor = Color.White;
            lblEmail.Location = new Point(40, 264);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(121, 19);
            lblEmail.TabIndex = 17;
            lblEmail.Text = "Correo Electronico";
            // 
            // lblSucursal
            // 
            lblSucursal.Anchor = AnchorStyles.None;
            lblSucursal.AutoSize = true;
            lblSucursal.ForeColor = Color.White;
            lblSucursal.Location = new Point(40, 470);
            lblSucursal.Name = "lblSucursal";
            lblSucursal.Size = new Size(59, 19);
            lblSucursal.TabIndex = 19;
            lblSucursal.Text = "Sucursal";
            // 
            // cmbSucursal
            // 
            cmbSucursal.Anchor = AnchorStyles.None;
            cmbSucursal.BackColor = Color.FromArgb(51, 65, 85);
            cmbSucursal.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSucursal.FlatStyle = FlatStyle.Flat;
            cmbSucursal.ForeColor = Color.White;
            cmbSucursal.FormattingEnabled = true;
            cmbSucursal.Location = new Point(40, 492);
            cmbSucursal.Name = "cmbSucursal";
            cmbSucursal.Size = new Size(350, 25);
            cmbSucursal.TabIndex = 20;
            // 
            // FormRegistroUsuario
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(30, 41, 59);
            ClientSize = new Size(434, 667);
            Controls.Add(cmbSucursal);
            Controls.Add(lblSucursal);
            Controls.Add(textEmail);
            Controls.Add(lblEmail);
            Controls.Add(textApellido);
            Controls.Add(lblApellido);
            Controls.Add(btnCancelar);
            Controls.Add(btnGuardar);
            Controls.Add(txtPassword);
            Controls.Add(lblCodigo);
            Controls.Add(cmbRol);
            Controls.Add(lblRol);
            Controls.Add(txtCelular);
            Controls.Add(lblCelular);
            Controls.Add(txtDireccion);
            Controls.Add(lblDireccion);
            Controls.Add(txtNombre);
            Controls.Add(lblNombre);
            Controls.Add(txtDNI);
            Controls.Add(lblDNI);
            Controls.Add(pnlHeader);
            Font = new Font("Segoe UI", 10F);
            Name = "FormRegistroUsuario";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Registro de Usuario";
            AutoScroll = true;
            WindowState = FormWindowState.Normal;
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Button btnVolver;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblDNI;
        private System.Windows.Forms.TextBox txtDNI;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label lblDireccion;
        private System.Windows.Forms.TextBox txtDireccion;
        private System.Windows.Forms.Label lblCelular;
        private System.Windows.Forms.TextBox txtCelular;
        private System.Windows.Forms.Label lblRol;
        private System.Windows.Forms.ComboBox cmbRol;
        private System.Windows.Forms.Label lblCodigo;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnCancelar;
        private TextBox textApellido;
        private Label lblApellido;
        private TextBox textEmail;
        private Label lblEmail;
        private Label lblSucursal;
        private ComboBox cmbSucursal;
    }
}





