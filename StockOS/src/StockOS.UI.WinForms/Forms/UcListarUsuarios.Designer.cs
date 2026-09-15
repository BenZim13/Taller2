namespace StockOS.UI.WinForms.Forms
{
    partial class UcListarUsuarios
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

        #region Código generado por el Diseñador de componentes

        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            pnlHeaderLista = new Panel();
            btnVolver = new Button();
            btnRecargar = new Button();
            btnDarBaja = new Button();
            btnEditar = new Button();
            lblTituloLista = new Label();
            pnlFiltros = new Panel();
            cmbFiltroEstado = new ComboBox();
            lblFiltroEstado = new Label();
            txtBuscar = new TextBox();
            lblBuscar = new Label();
            dgvUsuarios = new DataGridView();
            pnlHeaderLista.SuspendLayout();
            pnlFiltros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).BeginInit();
            SuspendLayout();
            // 
            // pnlHeaderLista
            // 
            pnlHeaderLista.BackColor = Color.FromArgb(30, 41, 59);
            pnlHeaderLista.Controls.Add(btnVolver);
            pnlHeaderLista.Controls.Add(btnRecargar);
            pnlHeaderLista.Controls.Add(btnDarBaja);
            pnlHeaderLista.Controls.Add(btnEditar);
            pnlHeaderLista.Controls.Add(lblTituloLista);
            pnlHeaderLista.Dock = DockStyle.Top;
            pnlHeaderLista.Location = new Point(0, 0);
            pnlHeaderLista.Name = "pnlHeaderLista";
            pnlHeaderLista.Padding = new Padding(25, 10, 25, 5);
            pnlHeaderLista.Size = new Size(1000, 50);
            pnlHeaderLista.TabIndex = 0;
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
            // btnRecargar
            // 
            btnRecargar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRecargar.BackColor = Color.FromArgb(71, 85, 105);
            btnRecargar.Cursor = Cursors.Hand;
            btnRecargar.FlatAppearance.BorderSize = 0;
            btnRecargar.FlatStyle = FlatStyle.Flat;
            btnRecargar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnRecargar.ForeColor = Color.White;
            btnRecargar.Location = new Point(885, 10);
            btnRecargar.Name = "btnRecargar";
            btnRecargar.Size = new Size(90, 32);
            btnRecargar.TabIndex = 3;
            btnRecargar.Text = "Recargar";
            btnRecargar.UseVisualStyleBackColor = false;
            // 
            // btnDarBaja
            // 
            btnDarBaja.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDarBaja.BackColor = Color.FromArgb(239, 68, 68);
            btnDarBaja.Cursor = Cursors.Hand;
            btnDarBaja.FlatAppearance.BorderSize = 0;
            btnDarBaja.FlatStyle = FlatStyle.Flat;
            btnDarBaja.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnDarBaja.ForeColor = Color.White;
            btnDarBaja.Location = new Point(745, 10);
            btnDarBaja.Name = "btnDarBaja";
            btnDarBaja.Size = new Size(130, 32);
            btnDarBaja.TabIndex = 2;
            btnDarBaja.Text = "Baja / Reactivar";
            btnDarBaja.UseVisualStyleBackColor = false;
            // 
            // btnEditar
            // 
            btnEditar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnEditar.BackColor = Color.FromArgb(59, 130, 246);
            btnEditar.Cursor = Cursors.Hand;
            btnEditar.FlatAppearance.BorderSize = 0;
            btnEditar.FlatStyle = FlatStyle.Flat;
            btnEditar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnEditar.ForeColor = Color.White;
            btnEditar.Location = new Point(635, 10);
            btnEditar.Name = "btnEditar";
            btnEditar.Size = new Size(100, 32);
            btnEditar.TabIndex = 1;
            btnEditar.Text = "Modificar";
            btnEditar.UseVisualStyleBackColor = false;
            // 
            // lblTituloLista
            // 
            lblTituloLista.AutoSize = true;
            lblTituloLista.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTituloLista.ForeColor = Color.White;
            lblTituloLista.Location = new Point(120, 7);
            lblTituloLista.Name = "lblTituloLista";
            lblTituloLista.Size = new Size(183, 30);
            lblTituloLista.TabIndex = 0;
            lblTituloLista.Text = "Lista de Usuarios";
            // 
            // pnlFiltros
            // 
            pnlFiltros.BackColor = Color.FromArgb(30, 41, 59);
            pnlFiltros.Controls.Add(cmbFiltroEstado);
            pnlFiltros.Controls.Add(lblFiltroEstado);
            pnlFiltros.Controls.Add(txtBuscar);
            pnlFiltros.Controls.Add(lblBuscar);
            pnlFiltros.Dock = DockStyle.Top;
            pnlFiltros.Location = new Point(0, 50);
            pnlFiltros.Name = "pnlFiltros";
            pnlFiltros.Padding = new Padding(25, 0, 25, 10);
            pnlFiltros.Size = new Size(1000, 48);
            pnlFiltros.TabIndex = 1;
            // 
            // cmbFiltroEstado
            // 
            cmbFiltroEstado.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cmbFiltroEstado.BackColor = Color.FromArgb(51, 65, 85);
            cmbFiltroEstado.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFiltroEstado.FlatStyle = FlatStyle.Flat;
            cmbFiltroEstado.Font = new Font("Segoe UI", 9.5F);
            cmbFiltroEstado.ForeColor = Color.White;
            cmbFiltroEstado.FormattingEnabled = true;
            cmbFiltroEstado.Items.AddRange(new object[] { "Todos", "Solo Activos", "Solo Inactivos" });
            cmbFiltroEstado.Location = new Point(835, 10);
            cmbFiltroEstado.Name = "cmbFiltroEstado";
            cmbFiltroEstado.Size = new Size(140, 25);
            cmbFiltroEstado.TabIndex = 3;
            // 
            // lblFiltroEstado
            // 
            lblFiltroEstado.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblFiltroEstado.AutoSize = true;
            lblFiltroEstado.Font = new Font("Segoe UI", 9.5F);
            lblFiltroEstado.ForeColor = Color.FromArgb(203, 213, 225);
            lblFiltroEstado.Location = new Point(778, 13);
            lblFiltroEstado.Name = "lblFiltroEstado";
            lblFiltroEstado.Size = new Size(51, 17);
            lblFiltroEstado.TabIndex = 2;
            lblFiltroEstado.Text = "Estado:";
            // 
            // txtBuscar
            // 
            txtBuscar.BackColor = Color.FromArgb(51, 65, 85);
            txtBuscar.BorderStyle = BorderStyle.FixedSingle;
            txtBuscar.Font = new Font("Segoe UI", 9.5F);
            txtBuscar.ForeColor = Color.White;
            txtBuscar.Location = new Point(80, 10);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.PlaceholderText = "Buscar por DNI, Nombre o Apellido...";
            txtBuscar.Size = new Size(350, 24);
            txtBuscar.TabIndex = 1;
            // 
            // lblBuscar
            // 
            lblBuscar.AutoSize = true;
            lblBuscar.Font = new Font("Segoe UI", 9.5F);
            lblBuscar.ForeColor = Color.FromArgb(203, 213, 225);
            lblBuscar.Location = new Point(25, 13);
            lblBuscar.Name = "lblBuscar";
            lblBuscar.Size = new Size(49, 17);
            lblBuscar.TabIndex = 0;
            lblBuscar.Text = "Buscar:";
            // 
            // dgvUsuarios
            // 
            dgvUsuarios.AllowUserToAddRows = false;
            dgvUsuarios.AllowUserToDeleteRows = false;
            dgvUsuarios.AllowUserToResizeRows = false;
            dgvUsuarios.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsuarios.BackgroundColor = Color.FromArgb(15, 23, 42);
            dgvUsuarios.BorderStyle = BorderStyle.None;
            dgvUsuarios.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvUsuarios.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(16, 185, 129);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.Padding = new Padding(6, 4, 6, 4);
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(16, 185, 129);
            dataGridViewCellStyle1.SelectionForeColor = Color.White;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvUsuarios.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvUsuarios.ColumnHeadersHeight = 36;
            dgvUsuarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(30, 41, 59);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9.5F);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.Padding = new Padding(4, 0, 4, 0);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(51, 65, 85);
            dataGridViewCellStyle2.SelectionForeColor = Color.White;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvUsuarios.DefaultCellStyle = dataGridViewCellStyle2;
            dgvUsuarios.EnableHeadersVisualStyles = false;
            dgvUsuarios.GridColor = Color.FromArgb(51, 65, 85);
            dgvUsuarios.Location = new Point(25, 105);
            dgvUsuarios.MultiSelect = false;
            dgvUsuarios.Name = "dgvUsuarios";
            dgvUsuarios.ReadOnly = true;
            dgvUsuarios.RowHeadersVisible = false;
            dgvUsuarios.RowTemplate.Height = 32;
            dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsuarios.Size = new Size(950, 475);
            dgvUsuarios.TabIndex = 2;
            // 
            // ListarUsuarios
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(30, 41, 59);
            Controls.Add(dgvUsuarios);
            Controls.Add(pnlFiltros);
            Controls.Add(pnlHeaderLista);
            Font = new Font("Segoe UI", 10F);
            ForeColor = Color.White;
            Name = "ListarUsuarios";
            Size = new Size(1000, 600);
            pnlHeaderLista.ResumeLayout(false);
            pnlHeaderLista.PerformLayout();
            pnlFiltros.ResumeLayout(false);
            pnlFiltros.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlHeaderLista;
        private Label lblTituloLista;
        private Button btnEditar;
        private Button btnDarBaja;
        private Button btnRecargar;
        private Button btnVolver;
        private Panel pnlFiltros;
        private TextBox txtBuscar;
        private Label lblBuscar;
        private ComboBox cmbFiltroEstado;
        private Label lblFiltroEstado;
        private DataGridView dgvUsuarios;
    }
}

