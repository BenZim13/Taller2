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
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            pnlTop = new Panel();
            btnCerrarCaja = new Button();
            btnMovimientoCaja = new Button();
            btnAbrirCaja = new Button();
            lblBuscador = new Label();
            txtCodigoBarra = new TextBox();
            pnlBottom = new Panel();
            lblDescuento = new Label();
            txtDescuento = new TextBox();
            lblTextoTotal = new Label();
            lblTotal = new Label();
            btnCobrar = new Button();
            dgvTicket = new DataGridView();
            IdProducto = new DataGridViewTextBoxColumn();
            Producto = new DataGridViewTextBoxColumn();
            Cantidad = new DataGridViewTextBoxColumn();
            Precio = new DataGridViewTextBoxColumn();
            Subtotal = new DataGridViewTextBoxColumn();
            Eliminar = new DataGridViewButtonColumn();
            pnlTop.SuspendLayout();
            pnlBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTicket).BeginInit();
            SuspendLayout();
            // 
            // pnlTop
            // 
            pnlTop.BackColor = Color.FromArgb(30, 41, 59);
            pnlTop.Controls.Add(btnCerrarCaja);
            pnlTop.Controls.Add(btnMovimientoCaja);
            pnlTop.Controls.Add(btnAbrirCaja);
            pnlTop.Controls.Add(lblBuscador);
            pnlTop.Controls.Add(txtCodigoBarra);
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Location = new Point(0, 0);
            pnlTop.Name = "pnlTop";
            pnlTop.Size = new Size(900, 80);
            pnlTop.TabIndex = 0;
            // 
            // btnCerrarCaja
            // 
            btnCerrarCaja.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCerrarCaja.BackColor = Color.FromArgb(239, 68, 68);
            btnCerrarCaja.Cursor = Cursors.Hand;
            btnCerrarCaja.FlatAppearance.BorderSize = 0;
            btnCerrarCaja.FlatStyle = FlatStyle.Flat;
            btnCerrarCaja.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnCerrarCaja.ForeColor = Color.White;
            btnCerrarCaja.Location = new Point(710, 20);
            btnCerrarCaja.Name = "btnCerrarCaja";
            btnCerrarCaja.Size = new Size(120, 40);
            btnCerrarCaja.TabIndex = 3;
            btnCerrarCaja.Text = "🔒 Cerrar Caja";
            btnCerrarCaja.UseVisualStyleBackColor = false;
            btnCerrarCaja.Click += btnCerrarCaja_Click;
            // 
            // btnMovimientoCaja
            // 
            btnMovimientoCaja.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnMovimientoCaja.BackColor = Color.FromArgb(59, 130, 246);
            btnMovimientoCaja.Cursor = Cursors.Hand;
            btnMovimientoCaja.FlatAppearance.BorderSize = 0;
            btnMovimientoCaja.FlatStyle = FlatStyle.Flat;
            btnMovimientoCaja.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnMovimientoCaja.ForeColor = Color.White;
            btnMovimientoCaja.Location = new Point(570, 20);
            btnMovimientoCaja.Name = "btnMovimientoCaja";
            btnMovimientoCaja.Size = new Size(130, 40);
            btnMovimientoCaja.TabIndex = 4;
            btnMovimientoCaja.Text = "📝 Movimiento";
            btnMovimientoCaja.UseVisualStyleBackColor = false;
            btnMovimientoCaja.Click += btnMovimientoCaja_Click;
            // 
            // btnAbrirCaja
            // 
            btnAbrirCaja.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAbrirCaja.BackColor = Color.FromArgb(16, 185, 129);
            btnAbrirCaja.Cursor = Cursors.Hand;
            btnAbrirCaja.FlatAppearance.BorderSize = 0;
            btnAbrirCaja.FlatStyle = FlatStyle.Flat;
            btnAbrirCaja.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnAbrirCaja.ForeColor = Color.White;
            btnAbrirCaja.Location = new Point(440, 20);
            btnAbrirCaja.Name = "btnAbrirCaja";
            btnAbrirCaja.Size = new Size(120, 40);
            btnAbrirCaja.TabIndex = 2;
            btnAbrirCaja.Text = "🔓 Abrir Caja";
            btnAbrirCaja.UseVisualStyleBackColor = false;
            btnAbrirCaja.Click += btnAbrirCaja_Click;
            // 
            // lblBuscador
            // 
            lblBuscador.AutoSize = true;
            lblBuscador.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblBuscador.ForeColor = Color.White;
            lblBuscador.Location = new Point(10, 31);
            lblBuscador.Name = "lblBuscador";
            lblBuscador.Size = new Size(84, 21);
            lblBuscador.TabIndex = 1;
            lblBuscador.Text = "Producto:";
            lblBuscador.Click += btnCobrar_Click;
            // 
            // txtCodigoBarra
            // 
            txtCodigoBarra.BackColor = Color.FromArgb(15, 23, 42);
            txtCodigoBarra.BorderStyle = BorderStyle.FixedSingle;
            txtCodigoBarra.Font = new Font("Segoe UI", 16F);
            txtCodigoBarra.ForeColor = Color.White;
            txtCodigoBarra.Location = new Point(100, 22);
            txtCodigoBarra.Name = "txtCodigoBarra";
            txtCodigoBarra.PlaceholderText = "Nombre o Código de prod.";
            txtCodigoBarra.Size = new Size(320, 36);
            txtCodigoBarra.TabIndex = 0;
            txtCodigoBarra.KeyDown += txtCodigoBarra_KeyDown;
            // 
            // pnlBottom
            // 
            pnlBottom.BackColor = Color.FromArgb(15, 23, 42);
            pnlBottom.Controls.Add(lblDescuento);
            pnlBottom.Controls.Add(txtDescuento);
            pnlBottom.Controls.Add(lblTextoTotal);
            pnlBottom.Controls.Add(lblTotal);
            pnlBottom.Controls.Add(btnCobrar);
            pnlBottom.Dock = DockStyle.Bottom;
            pnlBottom.Location = new Point(0, 500);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Size = new Size(900, 100);
            pnlBottom.TabIndex = 2;
            // 
            // lblDescuento
            // 
            lblDescuento.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblDescuento.AutoSize = true;
            lblDescuento.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblDescuento.ForeColor = Color.White;
            lblDescuento.Location = new Point(400, 38);
            lblDescuento.Name = "lblDescuento";
            lblDescuento.Size = new Size(108, 21);
            lblDescuento.TabIndex = 3;
            lblDescuento.Text = "Descuento $:";
            // 
            // txtDescuento
            // 
            txtDescuento.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtDescuento.BackColor = Color.FromArgb(30, 41, 59);
            txtDescuento.BorderStyle = BorderStyle.FixedSingle;
            txtDescuento.Font = new Font("Segoe UI", 16F);
            txtDescuento.ForeColor = Color.White;
            txtDescuento.Location = new Point(515, 32);
            txtDescuento.Name = "txtDescuento";
            txtDescuento.Size = new Size(110, 36);
            txtDescuento.TabIndex = 4;
            txtDescuento.TextAlign = HorizontalAlignment.Right;
            txtDescuento.TextChanged += txtDescuento_TextChanged;
            // 
            // lblTextoTotal
            // 
            lblTextoTotal.AutoSize = true;
            lblTextoTotal.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblTextoTotal.ForeColor = Color.White;
            lblTextoTotal.Location = new Point(20, 25);
            lblTextoTotal.Name = "lblTextoTotal";
            lblTextoTotal.Size = new Size(126, 45);
            lblTextoTotal.TabIndex = 1;
            lblTextoTotal.Text = "TOTAL:";
            // 
            // lblTotal
            // 
            lblTotal.AutoSize = true;
            lblTotal.Font = new Font("Segoe UI", 32F, FontStyle.Bold);
            lblTotal.ForeColor = Color.FromArgb(16, 185, 129);
            lblTotal.Location = new Point(140, 15);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(149, 59);
            lblTotal.TabIndex = 2;
            lblTotal.Text = "$ 0.00";
            // 
            // btnCobrar
            // 
            btnCobrar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCobrar.BackColor = Color.FromArgb(16, 185, 129);
            btnCobrar.Cursor = Cursors.Hand;
            btnCobrar.FlatAppearance.BorderSize = 0;
            btnCobrar.FlatStyle = FlatStyle.Flat;
            btnCobrar.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            btnCobrar.ForeColor = Color.White;
            btnCobrar.Location = new Point(650, 20);
            btnCobrar.Name = "btnCobrar";
            btnCobrar.Size = new Size(220, 60);
            btnCobrar.TabIndex = 0;
            btnCobrar.Text = "COBRAR (F12)";
            btnCobrar.UseVisualStyleBackColor = false;
            btnCobrar.Click += btnCobrar_Click;
            // 
            // dgvTicket
            // 
            dgvTicket.AllowUserToAddRows = false;
            dgvTicket.AllowUserToDeleteRows = false;
            dgvTicket.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTicket.BackgroundColor = Color.FromArgb(30, 41, 59);
            dgvTicket.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(15, 23, 42);
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            dataGridViewCellStyle3.ForeColor = Color.White;
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(15, 23, 42);
            dataGridViewCellStyle3.SelectionForeColor = Color.White;
            dgvTicket.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dgvTicket.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTicket.Columns.AddRange(new DataGridViewColumn[] { IdProducto, Producto, Cantidad, Precio, Subtotal, Eliminar });
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = Color.FromArgb(30, 41, 59);
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 12F);
            dataGridViewCellStyle4.ForeColor = Color.White;
            dataGridViewCellStyle4.SelectionBackColor = Color.FromArgb(51, 65, 85);
            dataGridViewCellStyle4.SelectionForeColor = Color.White;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.False;
            dgvTicket.DefaultCellStyle = dataGridViewCellStyle4;
            dgvTicket.Dock = DockStyle.Fill;
            dgvTicket.EnableHeadersVisualStyles = false;
            dgvTicket.Location = new Point(0, 80);
            dgvTicket.Name = "dgvTicket";
            dgvTicket.ReadOnly = true;
            dgvTicket.RowHeadersVisible = false;
            dgvTicket.RowTemplate.Height = 40;
            dgvTicket.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTicket.Size = new Size(900, 420);
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
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(dgvTicket);
            Controls.Add(pnlBottom);
            Controls.Add(pnlTop);
            Name = "UcVentas";
            Size = new Size(900, 600);
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