namespace StockOS.UI.WinForms.Forms
{
    partial class UcVentas
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

        #region Component Designer generated code

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            pnlTop = new System.Windows.Forms.Panel();
            lblBuscador = new System.Windows.Forms.Label();
            txtCodigoBarra = new System.Windows.Forms.TextBox();
            btnAbrirCaja = new System.Windows.Forms.Button();
            btnMovimientoCaja = new System.Windows.Forms.Button();
            btnCerrarCaja = new System.Windows.Forms.Button();
            pnlBottom = new System.Windows.Forms.Panel();
            lblTextoTotal = new System.Windows.Forms.Label();
            lblTotal = new System.Windows.Forms.Label();
            lblDescuento = new System.Windows.Forms.Label();
            txtDescuento = new System.Windows.Forms.TextBox();
            btnCobrar = new System.Windows.Forms.Button();
            dgvTicket = new System.Windows.Forms.DataGridView();
            IdProducto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Producto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Cantidad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Precio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Subtotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Eliminar = new System.Windows.Forms.DataGridViewButtonColumn();
            pnlTop.SuspendLayout();
            pnlBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTicket).BeginInit();
            SuspendLayout();
            // 
            // pnlTop
            // 
            pnlTop.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            pnlTop.Controls.Add(btnCerrarCaja);
            pnlTop.Controls.Add(btnMovimientoCaja);
            pnlTop.Controls.Add(btnAbrirCaja);
            pnlTop.Controls.Add(lblBuscador);
            pnlTop.Controls.Add(txtCodigoBarra);
            pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            pnlTop.Location = new System.Drawing.Point(0, 0);
            pnlTop.Name = "pnlTop";
            pnlTop.Size = new System.Drawing.Size(900, 80);
            pnlTop.TabIndex = 0;
            // 
            // lblBuscador
            // 
            lblBuscador.AutoSize = true;
            lblBuscador.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            lblBuscador.ForeColor = System.Drawing.Color.White;
            lblBuscador.Location = new System.Drawing.Point(20, 30);
            lblBuscador.Name = "lblBuscador";
            lblBuscador.Size = new System.Drawing.Size(70, 21);
            lblBuscador.TabIndex = 1;
            lblBuscador.Text = "Código:";
            // 
            // txtCodigoBarra
            // 
            txtCodigoBarra.BackColor = System.Drawing.Color.FromArgb(15, 23, 42);
            txtCodigoBarra.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtCodigoBarra.Font = new System.Drawing.Font("Segoe UI", 16F);
            txtCodigoBarra.ForeColor = System.Drawing.Color.White;
            txtCodigoBarra.Location = new System.Drawing.Point(100, 22);
            txtCodigoBarra.Name = "txtCodigoBarra";
            txtCodigoBarra.Size = new System.Drawing.Size(320, 36);
            txtCodigoBarra.TabIndex = 0;
            txtCodigoBarra.KeyDown += txtCodigoBarra_KeyDown;
            // 
            // btnAbrirCaja
            // 
            btnAbrirCaja.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnAbrirCaja.BackColor = System.Drawing.Color.FromArgb(16, 185, 129);
            btnAbrirCaja.Cursor = System.Windows.Forms.Cursors.Hand;
            btnAbrirCaja.FlatAppearance.BorderSize = 0;
            btnAbrirCaja.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnAbrirCaja.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            btnAbrirCaja.ForeColor = System.Drawing.Color.White;
            btnAbrirCaja.Location = new System.Drawing.Point(440, 20);
            btnAbrirCaja.Name = "btnAbrirCaja";
            btnAbrirCaja.Size = new System.Drawing.Size(120, 40);
            btnAbrirCaja.TabIndex = 2;
            btnAbrirCaja.Text = "🔓 Abrir Caja";
            btnAbrirCaja.UseVisualStyleBackColor = false;
            btnAbrirCaja.Click += btnAbrirCaja_Click;
            // 
            // btnMovimientoCaja
            // 
            btnMovimientoCaja.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnMovimientoCaja.BackColor = System.Drawing.Color.FromArgb(59, 130, 246);
            btnMovimientoCaja.Cursor = System.Windows.Forms.Cursors.Hand;
            btnMovimientoCaja.FlatAppearance.BorderSize = 0;
            btnMovimientoCaja.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnMovimientoCaja.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            btnMovimientoCaja.ForeColor = System.Drawing.Color.White;
            btnMovimientoCaja.Location = new System.Drawing.Point(570, 20);
            btnMovimientoCaja.Name = "btnMovimientoCaja";
            btnMovimientoCaja.Size = new System.Drawing.Size(130, 40);
            btnMovimientoCaja.TabIndex = 4;
            btnMovimientoCaja.Text = "📝 Movimiento";
            btnMovimientoCaja.UseVisualStyleBackColor = false;
            btnMovimientoCaja.Click += btnMovimientoCaja_Click;
            // 
            // btnCerrarCaja
            // 
            btnCerrarCaja.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnCerrarCaja.BackColor = System.Drawing.Color.FromArgb(239, 68, 68);
            btnCerrarCaja.Cursor = System.Windows.Forms.Cursors.Hand;
            btnCerrarCaja.FlatAppearance.BorderSize = 0;
            btnCerrarCaja.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnCerrarCaja.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            btnCerrarCaja.ForeColor = System.Drawing.Color.White;
            btnCerrarCaja.Location = new System.Drawing.Point(710, 20);
            btnCerrarCaja.Name = "btnCerrarCaja";
            btnCerrarCaja.Size = new System.Drawing.Size(120, 40);
            btnCerrarCaja.TabIndex = 3;
            btnCerrarCaja.Text = "🔒 Cerrar Caja";
            btnCerrarCaja.UseVisualStyleBackColor = false;
            btnCerrarCaja.Click += btnCerrarCaja_Click;
            // 
            // pnlBottom
            // 
            pnlBottom.BackColor = System.Drawing.Color.FromArgb(15, 23, 42);
            pnlBottom.Controls.Add(lblDescuento);
            pnlBottom.Controls.Add(txtDescuento);
            pnlBottom.Controls.Add(lblTextoTotal);
            pnlBottom.Controls.Add(lblTotal);
            pnlBottom.Controls.Add(btnCobrar);
            pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            pnlBottom.Location = new System.Drawing.Point(0, 500);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Size = new System.Drawing.Size(900, 100);
            pnlBottom.TabIndex = 2;
            // 
            // lblTextoTotal
            // 
            lblTextoTotal.AutoSize = true;
            lblTextoTotal.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            lblTextoTotal.ForeColor = System.Drawing.Color.White;
            lblTextoTotal.Location = new System.Drawing.Point(20, 25);
            lblTextoTotal.Name = "lblTextoTotal";
            lblTextoTotal.Size = new System.Drawing.Size(126, 45);
            lblTextoTotal.TabIndex = 1;
            lblTextoTotal.Text = "TOTAL:";
            // 
            // lblTotal
            // 
            lblTotal.AutoSize = true;
            lblTotal.Font = new System.Drawing.Font("Segoe UI", 32F, System.Drawing.FontStyle.Bold);
            lblTotal.ForeColor = System.Drawing.Color.FromArgb(16, 185, 129);
            lblTotal.Location = new System.Drawing.Point(140, 15);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new System.Drawing.Size(137, 59);
            lblTotal.TabIndex = 2;
            lblTotal.Text = "$ 0.00";
            // 
            // lblDescuento
            // 
            lblDescuento.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            lblDescuento.AutoSize = true;
            lblDescuento.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            lblDescuento.ForeColor = System.Drawing.Color.White;
            lblDescuento.Location = new System.Drawing.Point(400, 38);
            lblDescuento.Name = "lblDescuento";
            lblDescuento.Size = new System.Drawing.Size(107, 21);
            lblDescuento.TabIndex = 3;
            lblDescuento.Text = "Descuento $:";
            // 
            // txtDescuento
            // 
            txtDescuento.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            txtDescuento.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            txtDescuento.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtDescuento.Font = new System.Drawing.Font("Segoe UI", 16F);
            txtDescuento.ForeColor = System.Drawing.Color.White;
            txtDescuento.Location = new System.Drawing.Point(515, 32);
            txtDescuento.Name = "txtDescuento";
            txtDescuento.Size = new System.Drawing.Size(110, 36);
            txtDescuento.TabIndex = 4;
            txtDescuento.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            txtDescuento.TextChanged += txtDescuento_TextChanged;
            // 
            // btnCobrar
            // 
            btnCobrar.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnCobrar.BackColor = System.Drawing.Color.FromArgb(16, 185, 129);
            btnCobrar.Cursor = System.Windows.Forms.Cursors.Hand;
            btnCobrar.FlatAppearance.BorderSize = 0;
            btnCobrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnCobrar.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            btnCobrar.ForeColor = System.Drawing.Color.White;
            btnCobrar.Location = new System.Drawing.Point(650, 20);
            btnCobrar.Name = "btnCobrar";
            btnCobrar.Size = new System.Drawing.Size(220, 60);
            btnCobrar.TabIndex = 0;
            btnCobrar.Text = "COBRAR (F12)";
            btnCobrar.UseVisualStyleBackColor = false;
            btnCobrar.Click += btnCobrar_Click;
            // 
            // dgvTicket
            // 
            dgvTicket.AllowUserToAddRows = false;
            dgvTicket.AllowUserToDeleteRows = false;
            dgvTicket.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvTicket.BackgroundColor = System.Drawing.Color.FromArgb(30, 41, 59);
            dgvTicket.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(15, 23, 42);
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(15, 23, 42);
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dgvTicket.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvTicket.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTicket.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { IdProducto, Producto, Cantidad, Precio, Subtotal, Eliminar });
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 12F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(51, 65, 85);
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dgvTicket.DefaultCellStyle = dataGridViewCellStyle2;
            dgvTicket.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvTicket.EnableHeadersVisualStyles = false;
            dgvTicket.Location = new System.Drawing.Point(0, 80);
            dgvTicket.Name = "dgvTicket";
            dgvTicket.ReadOnly = true;
            dgvTicket.RowHeadersVisible = false;
            dgvTicket.RowTemplate.Height = 40;
            dgvTicket.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvTicket.Size = new System.Drawing.Size(900, 420);
            dgvTicket.TabIndex = 1;
            dgvTicket.CellContentClick += dgvTicket_CellContentClick;
            // 
            // IdProducto
            // 
            IdProducto.HeaderText = "Id";
            IdProducto.Name = "IdProducto";
            IdProducto.ReadOnly = true;
            IdProducto.Visible = false;
            // 
            // Producto
            // 
            Producto.FillWeight = 40F;
            Producto.HeaderText = "Producto";
            Producto.Name = "Producto";
            Producto.ReadOnly = true;
            // 
            // Cantidad
            // 
            Cantidad.FillWeight = 15F;
            Cantidad.HeaderText = "Cant.";
            Cantidad.Name = "Cantidad";
            Cantidad.ReadOnly = true;
            // 
            // Precio
            // 
            Precio.FillWeight = 20F;
            Precio.HeaderText = "Precio Unit.";
            Precio.Name = "Precio";
            Precio.ReadOnly = true;
            // 
            // Subtotal
            // 
            Subtotal.FillWeight = 20F;
            Subtotal.HeaderText = "Subtotal";
            Subtotal.Name = "Subtotal";
            Subtotal.ReadOnly = true;
            // 
            // Eliminar
            // 
            Eliminar.FillWeight = 5F;
            Eliminar.HeaderText = "X";
            Eliminar.Name = "Eliminar";
            Eliminar.ReadOnly = true;
            Eliminar.Text = "X";
            Eliminar.UseColumnTextForButtonValue = true;
            // 
            // UcVentas
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(dgvTicket);
            Controls.Add(pnlBottom);
            Controls.Add(pnlTop);
            Name = "UcVentas";
            Size = new System.Drawing.Size(900, 600);
            pnlTop.ResumeLayout(false);
            pnlTop.PerformLayout();
            pnlBottom.ResumeLayout(false);
            pnlBottom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTicket).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblBuscador;
        private System.Windows.Forms.TextBox txtCodigoBarra;
        private System.Windows.Forms.Button btnAbrirCaja;
        private System.Windows.Forms.Button btnCerrarCaja;
        private System.Windows.Forms.Button btnMovimientoCaja;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.DataGridView dgvTicket;
        private System.Windows.Forms.Label lblTextoTotal;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Button btnCobrar;
        private System.Windows.Forms.Label lblDescuento;
        private System.Windows.Forms.TextBox txtDescuento;
        private System.Windows.Forms.DataGridViewTextBoxColumn IdProducto;
        private System.Windows.Forms.DataGridViewTextBoxColumn Producto;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cantidad;
        private System.Windows.Forms.DataGridViewTextBoxColumn Precio;
        private System.Windows.Forms.DataGridViewTextBoxColumn Subtotal;
        private System.Windows.Forms.DataGridViewButtonColumn Eliminar;
    }
}