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
            this.btnVolver = new System.Windows.Forms.Button();
            this.lblTitulo = new System.Windows.Forms.Label();
            
            this.tlpCentrado = new System.Windows.Forms.TableLayoutPanel();
            this.pnlFormulario = new System.Windows.Forms.Panel();
            
            this.lblDNI = new System.Windows.Forms.Label();
            this.txtDNI = new System.Windows.Forms.TextBox();
            this.lblNombre = new System.Windows.Forms.Label();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.lblApellido = new System.Windows.Forms.Label();
            this.textApellido = new System.Windows.Forms.TextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.textEmail = new System.Windows.Forms.TextBox();
            this.lblDireccion = new System.Windows.Forms.Label();
            this.txtDireccion = new System.Windows.Forms.TextBox();
            this.lblCelular = new System.Windows.Forms.Label();
            this.txtCelular = new System.Windows.Forms.TextBox();
            this.lblRol = new System.Windows.Forms.Label();
            this.cmbRol = new System.Windows.Forms.ComboBox();
            this.lblSucursal = new System.Windows.Forms.Label();
            this.cmbSucursal = new System.Windows.Forms.ComboBox();
            this.lblCodigo = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnGuardar = new System.Windows.Forms.Button();
            
            this.pnlHeader.SuspendLayout();
            this.tlpCentrado.SuspendLayout();
            this.pnlFormulario.SuspendLayout();
            this.SuspendLayout();
            
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.pnlHeader.Controls.Add(this.btnVolver);
            this.pnlHeader.Controls.Add(this.lblTitulo);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Padding = new System.Windows.Forms.Padding(25, 10, 25, 5);
            this.pnlHeader.Size = new System.Drawing.Size(1000, 50);
            this.pnlHeader.TabIndex = 0;
            // 
            // btnVolver
            // 
            this.btnVolver.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.btnVolver.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnVolver.FlatAppearance.BorderSize = 0;
            this.btnVolver.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVolver.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnVolver.ForeColor = System.Drawing.Color.White;
            this.btnVolver.Location = new System.Drawing.Point(25, 10);
            this.btnVolver.Name = "btnVolver";
            this.btnVolver.Size = new System.Drawing.Size(80, 32);
            this.btnVolver.TabIndex = 4;
            this.btnVolver.Text = "← Volver";
            this.btnVolver.UseVisualStyleBackColor = false;
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.Location = new System.Drawing.Point(120, 7);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(216, 30);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Carga de Personal";
            // 
            // tlpCentrado
            // 
            this.tlpCentrado.ColumnCount = 3;
            this.tlpCentrado.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpCentrado.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 420F));
            this.tlpCentrado.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpCentrado.Controls.Add(this.pnlFormulario, 1, 0);
            this.tlpCentrado.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpCentrado.Location = new System.Drawing.Point(0, 50);
            this.tlpCentrado.Name = "tlpCentrado";
            this.tlpCentrado.Padding = new System.Windows.Forms.Padding(0, 20, 0, 70); // 70px bottom padding protects from bottom menu dock
            this.tlpCentrado.RowCount = 1;
            this.tlpCentrado.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpCentrado.Size = new System.Drawing.Size(1000, 698);
            this.tlpCentrado.TabIndex = 1;
            // 
            // pnlFormulario
            // 
            this.pnlFormulario.AutoScroll = true;
            this.pnlFormulario.Controls.Add(this.lblDNI);
            this.pnlFormulario.Controls.Add(this.txtDNI);
            this.pnlFormulario.Controls.Add(this.lblNombre);
            this.pnlFormulario.Controls.Add(this.txtNombre);
            this.pnlFormulario.Controls.Add(this.lblApellido);
            this.pnlFormulario.Controls.Add(this.textApellido);
            this.pnlFormulario.Controls.Add(this.lblEmail);
            this.pnlFormulario.Controls.Add(this.textEmail);
            this.pnlFormulario.Controls.Add(this.lblDireccion);
            this.pnlFormulario.Controls.Add(this.txtDireccion);
            this.pnlFormulario.Controls.Add(this.lblCelular);
            this.pnlFormulario.Controls.Add(this.txtCelular);
            this.pnlFormulario.Controls.Add(this.lblRol);
            this.pnlFormulario.Controls.Add(this.cmbRol);
            this.pnlFormulario.Controls.Add(this.lblSucursal);
            this.pnlFormulario.Controls.Add(this.cmbSucursal);
            this.pnlFormulario.Controls.Add(this.lblCodigo);
            this.pnlFormulario.Controls.Add(this.txtPassword);
            this.pnlFormulario.Controls.Add(this.btnGuardar);
            this.pnlFormulario.Controls.Add(this.btnCancelar);
            this.pnlFormulario.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFormulario.Location = new System.Drawing.Point(293, 23);
            this.pnlFormulario.Name = "pnlFormulario";
            this.pnlFormulario.Size = new System.Drawing.Size(414, 605);
            this.pnlFormulario.TabIndex = 0;
            // 
            // lblDNI
            // 
            this.lblDNI.AutoSize = true;
            this.lblDNI.ForeColor = System.Drawing.Color.White;
            this.lblDNI.Location = new System.Drawing.Point(25, 10);
            this.lblDNI.Name = "lblDNI";
            this.lblDNI.Size = new System.Drawing.Size(33, 19);
            this.lblDNI.TabIndex = 1;
            this.lblDNI.Text = "DNI";
            // 
            // txtDNI
            // 
            this.txtDNI.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.txtDNI.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDNI.ForeColor = System.Drawing.Color.White;
            this.txtDNI.Location = new System.Drawing.Point(25, 33);
            this.txtDNI.Name = "txtDNI";
            this.txtDNI.Size = new System.Drawing.Size(350, 25);
            this.txtDNI.TabIndex = 2;
            // 
            // lblNombre
            // 
            this.lblNombre.AutoSize = true;
            this.lblNombre.ForeColor = System.Drawing.Color.White;
            this.lblNombre.Location = new System.Drawing.Point(25, 61);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new System.Drawing.Size(59, 19);
            this.lblNombre.TabIndex = 3;
            this.lblNombre.Text = "Nombre";
            // 
            // txtNombre
            // 
            this.txtNombre.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.txtNombre.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNombre.ForeColor = System.Drawing.Color.White;
            this.txtNombre.Location = new System.Drawing.Point(25, 84);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(350, 25);
            this.txtNombre.TabIndex = 4;
            // 
            // lblApellido
            // 
            this.lblApellido.AutoSize = true;
            this.lblApellido.ForeColor = System.Drawing.Color.White;
            this.lblApellido.Location = new System.Drawing.Point(25, 112);
            this.lblApellido.Name = "lblApellido";
            this.lblApellido.Size = new System.Drawing.Size(58, 19);
            this.lblApellido.TabIndex = 5;
            this.lblApellido.Text = "Apellido";
            // 
            // textApellido
            // 
            this.textApellido.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.textApellido.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textApellido.ForeColor = System.Drawing.Color.White;
            this.textApellido.Location = new System.Drawing.Point(25, 135);
            this.textApellido.Name = "textApellido";
            this.textApellido.Size = new System.Drawing.Size(350, 25);
            this.textApellido.TabIndex = 6;
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.ForeColor = System.Drawing.Color.White;
            this.lblEmail.Location = new System.Drawing.Point(25, 163);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(121, 19);
            this.lblEmail.TabIndex = 7;
            this.lblEmail.Text = "Correo Electrónico";
            // 
            // textEmail
            // 
            this.textEmail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.textEmail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textEmail.ForeColor = System.Drawing.Color.White;
            this.textEmail.Location = new System.Drawing.Point(25, 186);
            this.textEmail.Name = "textEmail";
            this.textEmail.Size = new System.Drawing.Size(350, 25);
            this.textEmail.TabIndex = 8;
            // 
            // lblDireccion
            // 
            this.lblDireccion.AutoSize = true;
            this.lblDireccion.ForeColor = System.Drawing.Color.White;
            this.lblDireccion.Location = new System.Drawing.Point(25, 214);
            this.lblDireccion.Name = "lblDireccion";
            this.lblDireccion.Size = new System.Drawing.Size(65, 19);
            this.lblDireccion.TabIndex = 9;
            this.lblDireccion.Text = "Dirección";
            // 
            // txtDireccion
            // 
            this.txtDireccion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.txtDireccion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDireccion.ForeColor = System.Drawing.Color.White;
            this.txtDireccion.Location = new System.Drawing.Point(25, 237);
            this.txtDireccion.Name = "txtDireccion";
            this.txtDireccion.Size = new System.Drawing.Size(350, 25);
            this.txtDireccion.TabIndex = 10;
            // 
            // lblCelular
            // 
            this.lblCelular.AutoSize = true;
            this.lblCelular.ForeColor = System.Drawing.Color.White;
            this.lblCelular.Location = new System.Drawing.Point(25, 265);
            this.lblCelular.Name = "lblCelular";
            this.lblCelular.Size = new System.Drawing.Size(51, 19);
            this.lblCelular.TabIndex = 11;
            this.lblCelular.Text = "Celular";
            // 
            // txtCelular
            // 
            this.txtCelular.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.txtCelular.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCelular.ForeColor = System.Drawing.Color.White;
            this.txtCelular.Location = new System.Drawing.Point(25, 288);
            this.txtCelular.Name = "txtCelular";
            this.txtCelular.Size = new System.Drawing.Size(350, 25);
            this.txtCelular.TabIndex = 12;
            this.txtCelular.PlaceholderText = "Ej: 1123456789";
            // 
            // lblRol
            // 
            this.lblRol.AutoSize = true;
            this.lblRol.ForeColor = System.Drawing.Color.White;
            this.lblRol.Location = new System.Drawing.Point(25, 316);
            this.lblRol.Name = "lblRol";
            this.lblRol.Size = new System.Drawing.Size(28, 19);
            this.lblRol.TabIndex = 13;
            this.lblRol.Text = "Rol";
            // 
            // cmbRol
            // 
            this.cmbRol.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.cmbRol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRol.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbRol.ForeColor = System.Drawing.Color.White;
            this.cmbRol.FormattingEnabled = true;
            this.cmbRol.Location = new System.Drawing.Point(25, 339);
            this.cmbRol.Name = "cmbRol";
            this.cmbRol.Size = new System.Drawing.Size(350, 25);
            this.cmbRol.TabIndex = 14;
            // 
            // lblSucursal
            // 
            this.lblSucursal.AutoSize = true;
            this.lblSucursal.ForeColor = System.Drawing.Color.White;
            this.lblSucursal.Location = new System.Drawing.Point(25, 367);
            this.lblSucursal.Name = "lblSucursal";
            this.lblSucursal.Size = new System.Drawing.Size(59, 19);
            this.lblSucursal.TabIndex = 15;
            this.lblSucursal.Text = "Sucursal";
            // 
            // cmbSucursal
            // 
            this.cmbSucursal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.cmbSucursal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSucursal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbSucursal.ForeColor = System.Drawing.Color.White;
            this.cmbSucursal.FormattingEnabled = true;
            this.cmbSucursal.Location = new System.Drawing.Point(25, 390);
            this.cmbSucursal.Name = "cmbSucursal";
            this.cmbSucursal.Size = new System.Drawing.Size(350, 25);
            this.cmbSucursal.TabIndex = 16;
            // 
            // lblCodigo
            // 
            this.lblCodigo.AutoSize = true;
            this.lblCodigo.ForeColor = System.Drawing.Color.White;
            this.lblCodigo.Location = new System.Drawing.Point(25, 417);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.Size = new System.Drawing.Size(149, 19);
            this.lblCodigo.TabIndex = 17;
            this.lblCodigo.Text = "Contraseña (8 dígitos)";
            // 
            // txtPassword
            // 
            this.txtPassword.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPassword.ForeColor = System.Drawing.Color.White;
            this.txtPassword.Location = new System.Drawing.Point(25, 440);
            this.txtPassword.MaxLength = 8;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PlaceholderText = "••••••••";
            this.txtPassword.Size = new System.Drawing.Size(350, 25);
            this.txtPassword.TabIndex = 18;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // btnGuardar
            // 
            this.btnGuardar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(130)))), ((int)(((byte)(246)))));
            this.btnGuardar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGuardar.FlatAppearance.BorderSize = 0;
            this.btnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGuardar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnGuardar.ForeColor = System.Drawing.Color.White;
            this.btnGuardar.Location = new System.Drawing.Point(25, 490);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(165, 40);
            this.btnGuardar.TabIndex = 19;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = false;
            // 
            // btnCancelar
            // 
            this.btnCancelar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnCancelar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancelar.FlatAppearance.BorderSize = 0;
            this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCancelar.ForeColor = System.Drawing.Color.White;
            this.btnCancelar.Location = new System.Drawing.Point(210, 490);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(165, 40);
            this.btnCancelar.TabIndex = 20;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = false;
            // 
            // FormRegistroUsuario
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = false;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.ClientSize = new System.Drawing.Size(1000, 748);
            this.Controls.Add(this.tlpCentrado);
            this.Controls.Add(this.pnlHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.Name = "FormRegistroUsuario";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Registro de Usuario";
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.tlpCentrado.ResumeLayout(false);
            this.pnlFormulario.ResumeLayout(false);
            this.pnlFormulario.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Button btnVolver;
        private System.Windows.Forms.Label lblTitulo;
        
        private System.Windows.Forms.TableLayoutPanel tlpCentrado;
        private System.Windows.Forms.Panel pnlFormulario;
        
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
        private System.Windows.Forms.TextBox textApellido;
        private System.Windows.Forms.Label lblApellido;
        private System.Windows.Forms.TextBox textEmail;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label lblSucursal;
        private System.Windows.Forms.ComboBox cmbSucursal;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnCancelar;
    }
}

