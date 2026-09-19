namespace StockOS.UI.WinForms.Forms
{
    partial class FormIngresoStock
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
            // --- Instanciación de todos los controles ---
            this.pnlHeader          = new System.Windows.Forms.Panel();
            this.btnSalirApp        = new System.Windows.Forms.Button();
            this.lblTitulo          = new System.Windows.Forms.Label();
            this.lblCodigo          = new System.Windows.Forms.Label();
            this.txtCodigo          = new System.Windows.Forms.TextBox();
            this.lblNombreProducto  = new System.Windows.Forms.Label();
            this.txtNombreProducto  = new System.Windows.Forms.TextBox();
            this.lblCategoria       = new System.Windows.Forms.Label();
            this.cmbCategoria       = new System.Windows.Forms.ComboBox();
            this.lblProveedor       = new System.Windows.Forms.Label();
            this.cmbProveedor       = new System.Windows.Forms.ComboBox();
            this.lblCantidad        = new System.Windows.Forms.Label();
            this.txtCantidad        = new System.Windows.Forms.TextBox();
            this.lblFecha           = new System.Windows.Forms.Label();
            this.dtpFecha           = new System.Windows.Forms.DateTimePicker();
            this.lblPrecioCompra    = new System.Windows.Forms.Label();
            this.txtPrecioCompra    = new System.Windows.Forms.TextBox();
            this.lblPorcentajeExtra = new System.Windows.Forms.Label();
            this.txtPorcentajeExtra = new System.Windows.Forms.TextBox();
            this.lblMontoTotal      = new System.Windows.Forms.Label();
            this.txtMontoTotal      = new System.Windows.Forms.TextBox();
            this.lblPrecioVenta     = new System.Windows.Forms.Label();
            this.txtPrecioVenta     = new System.Windows.Forms.TextBox();
            this.btnGuardar         = new System.Windows.Forms.Button();
            this.btnCancelar        = new System.Windows.Forms.Button();

            this.pnlHeader.SuspendLayout();
            this.SuspendLayout();

            // ======================
            // HEADER
            // ======================
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.pnlHeader.Controls.Add(this.lblTitulo);
            this.pnlHeader.Controls.Add(this.btnSalirApp);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(420, 60);
            this.pnlHeader.TabIndex = 0;

            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.Location = new System.Drawing.Point(20, 16);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Ingreso de Stock";

            this.btnSalirApp.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.btnSalirApp.BackColor = System.Drawing.Color.FromArgb(239, 68, 68);
            this.btnSalirApp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSalirApp.FlatAppearance.BorderSize = 0;
            this.btnSalirApp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSalirApp.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSalirApp.ForeColor = System.Drawing.Color.White;
            this.btnSalirApp.Location = new System.Drawing.Point(320, 10);
            this.btnSalirApp.Name = "btnSalirApp";
            this.btnSalirApp.Size = new System.Drawing.Size(80, 40);
            this.btnSalirApp.TabIndex = 99;
            this.btnSalirApp.Text = "Cerrar";
            this.btnSalirApp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnSalirApp.UseVisualStyleBackColor = false;
            this.btnSalirApp.Click += new System.EventHandler(this.btnSalirApp_Click);

            // ======================
            // FILA 1: Código (izq) | Nombre del Producto (der)
            // ======================
            this.lblCodigo.AutoSize = true;
            this.lblCodigo.ForeColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.lblCodigo.Location = new System.Drawing.Point(40, 80);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.TabIndex = 10;
            this.lblCodigo.Text = "Código";

            this.txtCodigo.BackColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.txtCodigo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCodigo.ForeColor = System.Drawing.Color.White;
            this.txtCodigo.Location = new System.Drawing.Point(40, 100);
            this.txtCodigo.Name = "txtCodigo";
            this.txtCodigo.Size = new System.Drawing.Size(155, 25);
            this.txtCodigo.TabIndex = 0;

            this.lblNombreProducto.AutoSize = true;
            this.lblNombreProducto.ForeColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.lblNombreProducto.Location = new System.Drawing.Point(225, 80);
            this.lblNombreProducto.Name = "lblNombreProducto";
            this.lblNombreProducto.TabIndex = 11;
            this.lblNombreProducto.Text = "Nombre del Producto";

            this.txtNombreProducto.BackColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.txtNombreProducto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNombreProducto.ForeColor = System.Drawing.Color.White;
            this.txtNombreProducto.Location = new System.Drawing.Point(225, 100);
            this.txtNombreProducto.Name = "txtNombreProducto";
            this.txtNombreProducto.Size = new System.Drawing.Size(155, 25);
            this.txtNombreProducto.TabIndex = 1;

            // ======================
            // FILA 2: Categoría (izq) | Proveedor (der)
            // ======================
            this.lblCategoria.AutoSize = true;
            this.lblCategoria.ForeColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.lblCategoria.Location = new System.Drawing.Point(40, 140);
            this.lblCategoria.Name = "lblCategoria";
            this.lblCategoria.TabIndex = 12;
            this.lblCategoria.Text = "Categoría";

            this.cmbCategoria.BackColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.cmbCategoria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategoria.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbCategoria.ForeColor = System.Drawing.Color.White;
            this.cmbCategoria.Location = new System.Drawing.Point(40, 160);
            this.cmbCategoria.Name = "cmbCategoria";
            this.cmbCategoria.Size = new System.Drawing.Size(155, 25);
            this.cmbCategoria.TabIndex = 2;

            this.lblProveedor.AutoSize = true;
            this.lblProveedor.ForeColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.lblProveedor.Location = new System.Drawing.Point(225, 140);
            this.lblProveedor.Name = "lblProveedor";
            this.lblProveedor.TabIndex = 13;
            this.lblProveedor.Text = "Proveedor";

            this.cmbProveedor.BackColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.cmbProveedor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProveedor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbProveedor.ForeColor = System.Drawing.Color.White;
            this.cmbProveedor.Location = new System.Drawing.Point(225, 160);
            this.cmbProveedor.Name = "cmbProveedor";
            this.cmbProveedor.Size = new System.Drawing.Size(155, 25);
            this.cmbProveedor.TabIndex = 3;

            // ======================
            // FILA 3: Cantidad (izq) | Fecha (der)
            // ======================
            this.lblCantidad.AutoSize = true;
            this.lblCantidad.ForeColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.lblCantidad.Location = new System.Drawing.Point(40, 202);
            this.lblCantidad.Name = "lblCantidad";
            this.lblCantidad.TabIndex = 13;
            this.lblCantidad.Text = "Cantidad a Ingresar";

            this.txtCantidad.BackColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.txtCantidad.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCantidad.ForeColor = System.Drawing.Color.White;
            this.txtCantidad.Location = new System.Drawing.Point(40, 222);
            this.txtCantidad.Name = "txtCantidad";
            this.txtCantidad.Size = new System.Drawing.Size(155, 25);
            this.txtCantidad.TabIndex = 3;

            this.lblFecha.AutoSize = true;
            this.lblFecha.ForeColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.lblFecha.Location = new System.Drawing.Point(225, 202);
            this.lblFecha.Name = "lblFecha";
            this.lblFecha.TabIndex = 14;
            this.lblFecha.Text = "Fecha";

            this.dtpFecha.Enabled = false;
            this.dtpFecha.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFecha.Location = new System.Drawing.Point(225, 222);
            this.dtpFecha.Name = "dtpFecha";
            this.dtpFecha.Size = new System.Drawing.Size(155, 25);
            this.dtpFecha.TabIndex = 97;
            this.dtpFecha.TabStop = false;

            // ======================
            // FILA 4: Precio Unit. Compra (izq) | Margen Venta % (der)
            // ======================
            this.lblPrecioCompra.AutoSize = true;
            this.lblPrecioCompra.ForeColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.lblPrecioCompra.Location = new System.Drawing.Point(40, 264);
            this.lblPrecioCompra.Name = "lblPrecioCompra";
            this.lblPrecioCompra.TabIndex = 15;
            this.lblPrecioCompra.Text = "Precio Unit. Compra";

            this.txtPrecioCompra.BackColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.txtPrecioCompra.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPrecioCompra.ForeColor = System.Drawing.Color.White;
            this.txtPrecioCompra.Location = new System.Drawing.Point(40, 284);
            this.txtPrecioCompra.Name = "txtPrecioCompra";
            this.txtPrecioCompra.Size = new System.Drawing.Size(155, 25);
            this.txtPrecioCompra.TabIndex = 4;

            this.lblPorcentajeExtra.AutoSize = true;
            this.lblPorcentajeExtra.ForeColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.lblPorcentajeExtra.Location = new System.Drawing.Point(225, 264);
            this.lblPorcentajeExtra.Name = "lblPorcentajeExtra";
            this.lblPorcentajeExtra.TabIndex = 16;
            this.lblPorcentajeExtra.Text = "Margen Venta %";

            this.txtPorcentajeExtra.BackColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.txtPorcentajeExtra.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPorcentajeExtra.ForeColor = System.Drawing.Color.White;
            this.txtPorcentajeExtra.Location = new System.Drawing.Point(225, 284);
            this.txtPorcentajeExtra.Name = "txtPorcentajeExtra";
            this.txtPorcentajeExtra.Size = new System.Drawing.Size(155, 25);
            this.txtPorcentajeExtra.TabIndex = 5;

            // ======================
            // FILA 5: Monto Total Compra (izq) | Precio Venta Final (der)
            // ======================
            this.lblMontoTotal.AutoSize = true;
            this.lblMontoTotal.ForeColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.lblMontoTotal.Location = new System.Drawing.Point(40, 326);
            this.lblMontoTotal.Name = "lblMontoTotal";
            this.lblMontoTotal.TabIndex = 17;
            this.lblMontoTotal.Text = "Monto Total Compra";

            this.txtMontoTotal.BackColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.txtMontoTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMontoTotal.ForeColor = System.Drawing.Color.White;
            this.txtMontoTotal.Location = new System.Drawing.Point(40, 346);
            this.txtMontoTotal.Name = "txtMontoTotal";
            this.txtMontoTotal.ReadOnly = true;
            this.txtMontoTotal.Size = new System.Drawing.Size(155, 25);
            this.txtMontoTotal.TabIndex = 96;
            this.txtMontoTotal.TabStop = false;

            this.lblPrecioVenta.AutoSize = true;
            this.lblPrecioVenta.ForeColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.lblPrecioVenta.Location = new System.Drawing.Point(225, 326);
            this.lblPrecioVenta.Name = "lblPrecioVenta";
            this.lblPrecioVenta.TabIndex = 18;
            this.lblPrecioVenta.Text = "Precio Venta Final";

            this.txtPrecioVenta.BackColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.txtPrecioVenta.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPrecioVenta.ForeColor = System.Drawing.Color.FromArgb(52, 211, 153);
            this.txtPrecioVenta.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.txtPrecioVenta.Location = new System.Drawing.Point(225, 346);
            this.txtPrecioVenta.Name = "txtPrecioVenta";
            this.txtPrecioVenta.ReadOnly = true;
            this.txtPrecioVenta.Size = new System.Drawing.Size(155, 25);
            this.txtPrecioVenta.TabIndex = 97;
            this.txtPrecioVenta.TabStop = false;

            // ======================
            // FILA 6: Botones centrados
            // FormWidth=420, 2 botones 110px + gap 20px = 240px
            // X inicio = (420 - 240) / 2 = 90
            // ======================
            this.btnGuardar.BackColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.btnGuardar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGuardar.FlatAppearance.BorderSize = 0;
            this.btnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGuardar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnGuardar.ForeColor = System.Drawing.Color.White;
            this.btnGuardar.Location = new System.Drawing.Point(90, 400);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(110, 36);
            this.btnGuardar.TabIndex = 6;
            this.btnGuardar.Text = "Ingresar";
            this.btnGuardar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnGuardar.UseVisualStyleBackColor = false;

            this.btnCancelar.BackColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.btnCancelar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancelar.FlatAppearance.BorderSize = 0;
            this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCancelar.ForeColor = System.Drawing.Color.White;
            this.btnCancelar.Location = new System.Drawing.Point(220, 400);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(110, 36);
            this.btnCancelar.TabIndex = 7;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnCancelar.UseVisualStyleBackColor = false;

            // ======================
            // FORMULARIO
            // ======================
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.ClientSize = new System.Drawing.Size(420, 460);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.txtPrecioVenta);
            this.Controls.Add(this.lblPrecioVenta);
            this.Controls.Add(this.txtMontoTotal);
            this.Controls.Add(this.lblMontoTotal);
            this.Controls.Add(this.txtPorcentajeExtra);
            this.Controls.Add(this.lblPorcentajeExtra);
            this.Controls.Add(this.txtPrecioCompra);
            this.Controls.Add(this.lblPrecioCompra);
            this.Controls.Add(this.dtpFecha);
            this.Controls.Add(this.lblFecha);
            this.Controls.Add(this.txtCantidad);
            this.Controls.Add(this.lblCantidad);
            this.Controls.Add(this.cmbProveedor);
            this.Controls.Add(this.lblProveedor);
            this.Controls.Add(this.cmbCategoria);
            this.Controls.Add(this.lblCategoria);
            this.Controls.Add(this.txtNombreProducto);
            this.Controls.Add(this.lblNombreProducto);
            this.Controls.Add(this.txtCodigo);
            this.Controls.Add(this.lblCodigo);
            this.Controls.Add(this.pnlHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormIngresoStock";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Ingreso de Stock";
            this.CancelButton = this.btnCancelar;
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Button btnSalirApp;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblCodigo;
        private System.Windows.Forms.TextBox txtCodigo;
        private System.Windows.Forms.Label lblNombreProducto;
        private System.Windows.Forms.TextBox txtNombreProducto;
        private System.Windows.Forms.Label lblCategoria;
        private System.Windows.Forms.ComboBox cmbCategoria;
        private System.Windows.Forms.Label lblProveedor;
        private System.Windows.Forms.ComboBox cmbProveedor;
        private System.Windows.Forms.Label lblCantidad;
        private System.Windows.Forms.TextBox txtCantidad;
        private System.Windows.Forms.Label lblFecha;
        private System.Windows.Forms.DateTimePicker dtpFecha;
        private System.Windows.Forms.Label lblPrecioCompra;
        private System.Windows.Forms.TextBox txtPrecioCompra;
        private System.Windows.Forms.Label lblPorcentajeExtra;
        private System.Windows.Forms.TextBox txtPorcentajeExtra;
        private System.Windows.Forms.Label lblMontoTotal;
        private System.Windows.Forms.TextBox txtMontoTotal;
        private System.Windows.Forms.Label lblPrecioVenta;
        private System.Windows.Forms.TextBox txtPrecioVenta;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnCancelar;
    }
}

