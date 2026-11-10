using System.Drawing;
using System.Windows.Forms;

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
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.btnSalirApp = new System.Windows.Forms.Button();
            this.btnVolver = new System.Windows.Forms.Button();

            this.lblDNI = new System.Windows.Forms.Label();
            this.txtDNI = new System.Windows.Forms.TextBox();
            this.lblCelular = new System.Windows.Forms.Label();
            this.txtCelular = new System.Windows.Forms.TextBox();

            this.lblNombre = new System.Windows.Forms.Label();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.lblApellido = new System.Windows.Forms.Label();
            this.textApellido = new System.Windows.Forms.TextBox();

            this.lblEmail = new System.Windows.Forms.Label();
            this.textEmail = new System.Windows.Forms.TextBox();
            this.lblDireccion = new System.Windows.Forms.Label();
            this.txtDireccion = new System.Windows.Forms.TextBox();

            this.lblRol = new System.Windows.Forms.Label();
            this.cmbRol = new System.Windows.Forms.ComboBox();
            this.lblSucursal = new System.Windows.Forms.Label();
            this.cmbSucursal = new System.Windows.Forms.ComboBox();

            this.lblCodigo = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();

            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();

            this.pnlHeader.SuspendLayout();
            this.SuspendLayout();

            // ======================
            // HEADER (Slate 900)
            // ======================
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.pnlHeader.Controls.Add(this.lblTitulo);
            this.pnlHeader.Controls.Add(this.btnSalirApp);
            this.pnlHeader.Controls.Add(this.btnVolver);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(560, 60);
            this.pnlHeader.TabIndex = 0;

            // lblTitulo
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.Location = new System.Drawing.Point(20, 16);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(185, 25);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Registro de Usuario";

            // btnSalirApp (Cerrar)
            this.btnSalirApp.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.btnSalirApp.BackColor = System.Drawing.Color.FromArgb(239, 68, 68);
            this.btnSalirApp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSalirApp.FlatAppearance.BorderSize = 0;
            this.btnSalirApp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSalirApp.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSalirApp.ForeColor = System.Drawing.Color.White;
            this.btnSalirApp.Location = new System.Drawing.Point(460, 10);
            this.btnSalirApp.Name = "btnSalirApp";
            this.btnSalirApp.Size = new System.Drawing.Size(80, 40);
            this.btnSalirApp.TabIndex = 99;
            this.btnSalirApp.Text = "Cerrar";
            this.btnSalirApp.UseVisualStyleBackColor = false;

            // btnVolver (oculto por compatibilidad)
            this.btnVolver.Visible = false;
            this.btnVolver.Size = new System.Drawing.Size(0, 0);

            // ======================
            // FILA 1: DNI (Izq) | Celular (Der)
            // ======================
            this.lblDNI.AutoSize = true;
            this.lblDNI.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblDNI.ForeColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.lblDNI.Location = new System.Drawing.Point(35, 75);
            this.lblDNI.Name = "lblDNI";
            this.lblDNI.Size = new System.Drawing.Size(30, 17);
            this.lblDNI.TabIndex = 1;
            this.lblDNI.Text = "DNI";

            this.txtDNI.BackColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.txtDNI.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDNI.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtDNI.ForeColor = System.Drawing.Color.White;
            this.txtDNI.Location = new System.Drawing.Point(35, 96);
            this.txtDNI.Name = "txtDNI";
            this.txtDNI.Size = new System.Drawing.Size(230, 24);
            this.txtDNI.TabIndex = 2;

            this.lblCelular.AutoSize = true;
            this.lblCelular.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblCelular.ForeColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.lblCelular.Location = new System.Drawing.Point(295, 75);
            this.lblCelular.Name = "lblCelular";
            this.lblCelular.Size = new System.Drawing.Size(48, 17);
            this.lblCelular.TabIndex = 3;
            this.lblCelular.Text = "Celular";

            this.txtCelular.BackColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.txtCelular.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCelular.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtCelular.ForeColor = System.Drawing.Color.White;
            this.txtCelular.Location = new System.Drawing.Point(295, 96);
            this.txtCelular.Name = "txtCelular";
            this.txtCelular.PlaceholderText = "Ej: 1123456789";
            this.txtCelular.Size = new System.Drawing.Size(230, 24);
            this.txtCelular.TabIndex = 4;

            // ======================
            // FILA 2: Nombre (Izq) | Apellido (Der)
            // ======================
            this.lblNombre.AutoSize = true;
            this.lblNombre.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblNombre.ForeColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.lblNombre.Location = new System.Drawing.Point(35, 131);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new System.Drawing.Size(57, 17);
            this.lblNombre.TabIndex = 5;
            this.lblNombre.Text = "Nombre";

            this.txtNombre.BackColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.txtNombre.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNombre.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtNombre.ForeColor = System.Drawing.Color.White;
            this.txtNombre.Location = new System.Drawing.Point(35, 152);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(230, 24);
            this.txtNombre.TabIndex = 6;

            this.lblApellido.AutoSize = true;
            this.lblApellido.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblApellido.ForeColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.lblApellido.Location = new System.Drawing.Point(295, 131);
            this.lblApellido.Name = "lblApellido";
            this.lblApellido.Size = new System.Drawing.Size(56, 17);
            this.lblApellido.TabIndex = 7;
            this.lblApellido.Text = "Apellido";

            this.textApellido.BackColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.textApellido.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textApellido.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.textApellido.ForeColor = System.Drawing.Color.White;
            this.textApellido.Location = new System.Drawing.Point(295, 152);
            this.textApellido.Name = "textApellido";
            this.textApellido.Size = new System.Drawing.Size(230, 24);
            this.textApellido.TabIndex = 8;

            // ======================
            // FILA 3: Email (Izq) | Dirección (Der)
            // ======================
            this.lblEmail.AutoSize = true;
            this.lblEmail.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblEmail.ForeColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.lblEmail.Location = new System.Drawing.Point(35, 187);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(117, 17);
            this.lblEmail.TabIndex = 9;
            this.lblEmail.Text = "Correo Electrónico";

            this.textEmail.BackColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.textEmail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textEmail.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.textEmail.ForeColor = System.Drawing.Color.White;
            this.textEmail.Location = new System.Drawing.Point(35, 208);
            this.textEmail.Name = "textEmail";
            this.textEmail.Size = new System.Drawing.Size(230, 24);
            this.textEmail.TabIndex = 10;

            this.lblDireccion.AutoSize = true;
            this.lblDireccion.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblDireccion.ForeColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.lblDireccion.Location = new System.Drawing.Point(295, 187);
            this.lblDireccion.Name = "lblDireccion";
            this.lblDireccion.Size = new System.Drawing.Size(62, 17);
            this.lblDireccion.TabIndex = 11;
            this.lblDireccion.Text = "Dirección";

            this.txtDireccion.BackColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.txtDireccion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDireccion.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtDireccion.ForeColor = System.Drawing.Color.White;
            this.txtDireccion.Location = new System.Drawing.Point(295, 208);
            this.txtDireccion.Name = "txtDireccion";
            this.txtDireccion.Size = new System.Drawing.Size(230, 24);
            this.txtDireccion.TabIndex = 12;

            // ======================
            // FILA 4: Rol (Izq) | Sucursal (Der)
            // ======================
            this.lblRol.AutoSize = true;
            this.lblRol.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblRol.ForeColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.lblRol.Location = new System.Drawing.Point(35, 243);
            this.lblRol.Name = "lblRol";
            this.lblRol.Size = new System.Drawing.Size(27, 17);
            this.lblRol.TabIndex = 13;
            this.lblRol.Text = "Rol";

            this.cmbRol.BackColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.cmbRol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRol.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbRol.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cmbRol.ForeColor = System.Drawing.Color.White;
            this.cmbRol.FormattingEnabled = true;
            this.cmbRol.Location = new System.Drawing.Point(35, 264);
            this.cmbRol.Name = "cmbRol";
            this.cmbRol.Size = new System.Drawing.Size(230, 25);
            this.cmbRol.TabIndex = 14;

            this.lblSucursal.AutoSize = true;
            this.lblSucursal.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblSucursal.ForeColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.lblSucursal.Location = new System.Drawing.Point(295, 243);
            this.lblSucursal.Name = "lblSucursal";
            this.lblSucursal.Size = new System.Drawing.Size(56, 17);
            this.lblSucursal.TabIndex = 15;
            this.lblSucursal.Text = "Sucursal";

            this.cmbSucursal.BackColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.cmbSucursal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSucursal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbSucursal.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cmbSucursal.ForeColor = System.Drawing.Color.White;
            this.cmbSucursal.FormattingEnabled = true;
            this.cmbSucursal.Location = new System.Drawing.Point(295, 264);
            this.cmbSucursal.Name = "cmbSucursal";
            this.cmbSucursal.Size = new System.Drawing.Size(230, 25);
            this.cmbSucursal.TabIndex = 16;

            // ======================
            // FILA 5: Contraseña (Ancho completo)
            // ======================
            this.lblCodigo.AutoSize = true;
            this.lblCodigo.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblCodigo.ForeColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.lblCodigo.Location = new System.Drawing.Point(35, 299);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.Size = new System.Drawing.Size(142, 17);
            this.lblCodigo.TabIndex = 17;
            this.lblCodigo.Text = "Contraseña (8 dígitos)";

            this.txtPassword.BackColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPassword.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtPassword.ForeColor = System.Drawing.Color.White;
            this.txtPassword.Location = new System.Drawing.Point(35, 320);
            this.txtPassword.MaxLength = 8;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PlaceholderText = "••••••••";
            this.txtPassword.Size = new System.Drawing.Size(490, 24);
            this.txtPassword.TabIndex = 18;
            this.txtPassword.UseSystemPasswordChar = true;

            // ======================
            // BOTONES DE ACCIÓN (Guardar / Cancelar)
            // ======================
            this.btnGuardar.BackColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.btnGuardar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGuardar.FlatAppearance.BorderSize = 0;
            this.btnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGuardar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnGuardar.ForeColor = System.Drawing.Color.White;
            this.btnGuardar.Location = new System.Drawing.Point(285, 375);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(115, 36);
            this.btnGuardar.TabIndex = 19;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = false;

            this.btnCancelar.BackColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.btnCancelar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancelar.FlatAppearance.BorderSize = 0;
            this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCancelar.ForeColor = System.Drawing.Color.White;
            this.btnCancelar.Location = new System.Drawing.Point(410, 375);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(115, 36);
            this.btnCancelar.TabIndex = 20;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = false;

            // ======================
            // FORM PROPIEDADES MODAL
            // ======================
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.ClientSize = new System.Drawing.Size(560, 435);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblCodigo);
            this.Controls.Add(this.cmbSucursal);
            this.Controls.Add(this.lblSucursal);
            this.Controls.Add(this.cmbRol);
            this.Controls.Add(this.lblRol);
            this.Controls.Add(this.txtDireccion);
            this.Controls.Add(this.lblDireccion);
            this.Controls.Add(this.textEmail);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.textApellido);
            this.Controls.Add(this.lblApellido);
            this.Controls.Add(this.txtNombre);
            this.Controls.Add(this.lblNombre);
            this.Controls.Add(this.txtCelular);
            this.Controls.Add(this.lblCelular);
            this.Controls.Add(this.txtDNI);
            this.Controls.Add(this.lblDNI);
            this.Controls.Add(this.pnlHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormRegistroUsuario";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Registro de Usuario";
            this.AcceptButton = this.btnGuardar;
            this.CancelButton = this.btnCancelar;
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Button btnSalirApp;
        private System.Windows.Forms.Button btnVolver;

        private System.Windows.Forms.Label lblDNI;
        private System.Windows.Forms.TextBox txtDNI;
        private System.Windows.Forms.Label lblCelular;
        private System.Windows.Forms.TextBox txtCelular;

        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label lblApellido;
        private System.Windows.Forms.TextBox textApellido;

        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox textEmail;
        private System.Windows.Forms.Label lblDireccion;
        private System.Windows.Forms.TextBox txtDireccion;

        private System.Windows.Forms.Label lblRol;
        private System.Windows.Forms.ComboBox cmbRol;
        private System.Windows.Forms.Label lblSucursal;
        private System.Windows.Forms.ComboBox cmbSucursal;

        private System.Windows.Forms.Label lblCodigo;
        private System.Windows.Forms.TextBox txtPassword;

        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnCancelar;
    }
}
