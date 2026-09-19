namespace StockOS.UI.WinForms.Forms
{
    partial class FormProveedor
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.btnCerrarHeader = new System.Windows.Forms.Button();
            this.dgvProveedores = new System.Windows.Forms.DataGridView();
            this.pnlFormulario = new System.Windows.Forms.Panel();
            this.lblNombre = new System.Windows.Forms.Label();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.lblCuit = new System.Windows.Forms.Label();
            this.txtCuit = new System.Windows.Forms.TextBox();
            this.btnAgregar = new System.Windows.Forms.Button();
            this.btnDarBaja = new System.Windows.Forms.Button();
            this.btnCerrar = new System.Windows.Forms.Button();

            this.pnlHeader.SuspendLayout();
            this.pnlFormulario.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProveedores)).BeginInit();
            this.SuspendLayout();

            // HEADER
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.pnlHeader.Controls.Add(this.lblTitulo);
            this.pnlHeader.Controls.Add(this.btnCerrarHeader);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Size = new System.Drawing.Size(520, 55);
            this.pnlHeader.Name = "pnlHeader";

            // btnCerrarHeader
            this.btnCerrarHeader.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.btnCerrarHeader.BackColor = System.Drawing.Color.FromArgb(239, 68, 68);
            this.btnCerrarHeader.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCerrarHeader.FlatAppearance.BorderSize = 0;
            this.btnCerrarHeader.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrarHeader.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCerrarHeader.ForeColor = System.Drawing.Color.White;
            this.btnCerrarHeader.Location = new System.Drawing.Point(420, 10);
            this.btnCerrarHeader.Name = "btnCerrarHeader";
            this.btnCerrarHeader.Size = new System.Drawing.Size(80, 35);
            this.btnCerrarHeader.TabIndex = 99;
            this.btnCerrarHeader.Text = "Cerrar";
            this.btnCerrarHeader.UseVisualStyleBackColor = false;
            this.btnCerrarHeader.Click += (s, e) => this.Close();

            // lblTitulo
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.Location = new System.Drawing.Point(20, 14);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Text = "Gestión de Proveedores";

            // GRILLA de proveedores
            var colId = new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "colId", HeaderText = "ID", DataPropertyName = "IdProveedor", Visible = false };
            var colNombre = new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "colNombre", HeaderText = "Nombre / Razón Social", DataPropertyName = "RazonSocial", FillWeight = 160 };
            var colCuit = new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "colCuit", HeaderText = "CUIT", DataPropertyName = "Cuit", FillWeight = 120 };
            var colEstado = new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "colEstado", HeaderText = "Estado", FillWeight = 70 };

            this.dgvProveedores.AllowUserToAddRows = false;
            this.dgvProveedores.AllowUserToDeleteRows = false;
            this.dgvProveedores.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvProveedores.BackgroundColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.dgvProveedores.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvProveedores.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvProveedores.ColumnHeadersHeight = 34;
            this.dgvProveedores.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvProveedores.Columns.AddRange(colId, colNombre, colCuit, colEstado);
            this.dgvProveedores.EnableHeadersVisualStyles = false;
            this.dgvProveedores.GridColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.dgvProveedores.Location = new System.Drawing.Point(20, 65);
            this.dgvProveedores.MultiSelect = false;
            this.dgvProveedores.Name = "dgvProveedores";
            this.dgvProveedores.ReadOnly = true;
            this.dgvProveedores.RowHeadersVisible = false;
            this.dgvProveedores.RowTemplate.Height = 30;
            this.dgvProveedores.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProveedores.Size = new System.Drawing.Size(480, 200);

            var headerStyle = new System.Windows.Forms.DataGridViewCellStyle
            {
                BackColor = System.Drawing.Color.FromArgb(16, 185, 129),
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold),
                Padding = new System.Windows.Forms.Padding(8, 0, 4, 0),
                Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            };
            this.dgvProveedores.ColumnHeadersDefaultCellStyle = headerStyle;

            var rowStyle = new System.Windows.Forms.DataGridViewCellStyle
            {
                BackColor = System.Drawing.Color.FromArgb(30, 41, 59),
                ForeColor = System.Drawing.Color.White,
                SelectionBackColor = System.Drawing.Color.FromArgb(51, 65, 85),
                SelectionForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 9.5F),
                Padding = new System.Windows.Forms.Padding(8, 0, 4, 0),
                Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            };
            this.dgvProveedores.DefaultCellStyle = rowStyle;

            // PANEL formulario Alta
            this.pnlFormulario.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.pnlFormulario.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlFormulario.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.lblNombre, this.txtNombre, this.lblCuit, this.txtCuit, this.btnAgregar
            });
            this.pnlFormulario.Location = new System.Drawing.Point(20, 280);
            this.pnlFormulario.Name = "pnlFormulario";
            this.pnlFormulario.Size = new System.Drawing.Size(480, 130);

            // lblNombre
            this.lblNombre.AutoSize = true;
            this.lblNombre.ForeColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.lblNombre.Location = new System.Drawing.Point(15, 15);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Text = "Nombre / Razón Social";

            // txtNombre
            this.txtNombre.BackColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.txtNombre.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNombre.ForeColor = System.Drawing.Color.White;
            this.txtNombre.Location = new System.Drawing.Point(15, 35);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(220, 25);
            this.txtNombre.TabIndex = 0;

            // lblCuit
            this.lblCuit.AutoSize = true;
            this.lblCuit.ForeColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.lblCuit.Location = new System.Drawing.Point(250, 15);
            this.lblCuit.Name = "lblCuit";
            this.lblCuit.Text = "CUIT";

            // txtCuit
            this.txtCuit.BackColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.txtCuit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCuit.ForeColor = System.Drawing.Color.White;
            this.txtCuit.Location = new System.Drawing.Point(250, 35);
            this.txtCuit.Name = "txtCuit";
            this.txtCuit.Size = new System.Drawing.Size(210, 25);
            this.txtCuit.TabIndex = 1;

            // btnAgregar
            this.btnAgregar.BackColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.btnAgregar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAgregar.FlatAppearance.BorderSize = 0;
            this.btnAgregar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAgregar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnAgregar.ForeColor = System.Drawing.Color.White;
            this.btnAgregar.Location = new System.Drawing.Point(175, 80);
            this.btnAgregar.Name = "btnAgregar";
            this.btnAgregar.Size = new System.Drawing.Size(130, 32);
            this.btnAgregar.TabIndex = 2;
            this.btnAgregar.Text = "+ Agregar";
            this.btnAgregar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnAgregar.UseVisualStyleBackColor = false;

            // Botón Dar de baja / Reactivar
            this.btnDarBaja.BackColor = System.Drawing.Color.FromArgb(239, 68, 68);
            this.btnDarBaja.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDarBaja.FlatAppearance.BorderSize = 0;
            this.btnDarBaja.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDarBaja.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnDarBaja.ForeColor = System.Drawing.Color.White;
            this.btnDarBaja.Location = new System.Drawing.Point(155, 425);
            this.btnDarBaja.Name = "btnDarBaja";
            this.btnDarBaja.Size = new System.Drawing.Size(130, 32);
            this.btnDarBaja.TabIndex = 3;
            this.btnDarBaja.Text = "Dar de Baja";
            this.btnDarBaja.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnDarBaja.UseVisualStyleBackColor = false;

            // Botón Cerrar
            this.btnCerrar.BackColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.btnCerrar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCerrar.FlatAppearance.BorderSize = 0;
            this.btnCerrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnCerrar.ForeColor = System.Drawing.Color.White;
            this.btnCerrar.Location = new System.Drawing.Point(295, 425);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(100, 32);
            this.btnCerrar.TabIndex = 4;
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnCerrar.UseVisualStyleBackColor = false;

            // FORMULARIO
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.ClientSize = new System.Drawing.Size(520, 480);
            this.Controls.Add(this.btnCerrar);
            this.Controls.Add(this.btnDarBaja);
            this.Controls.Add(this.pnlFormulario);
            this.Controls.Add(this.dgvProveedores);
            this.Controls.Add(this.pnlHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormProveedor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Gestión de Proveedores";
            this.CancelButton = this.btnCerrar;

            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlFormulario.ResumeLayout(false);
            this.pnlFormulario.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProveedores)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Button btnCerrarHeader;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.DataGridView dgvProveedores;
        private System.Windows.Forms.Panel pnlFormulario;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label lblCuit;
        private System.Windows.Forms.TextBox txtCuit;
        private System.Windows.Forms.Button btnAgregar;
        private System.Windows.Forms.Button btnDarBaja;
        private System.Windows.Forms.Button btnCerrar;
    }
}

