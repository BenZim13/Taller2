namespace StockOS.UI.WinForms.Forms
{
    partial class FormCobro
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
            pnlHeader = new System.Windows.Forms.Panel();
            lblTitulo = new System.Windows.Forms.Label();
            lblTextoTotal = new System.Windows.Forms.Label();
            lblTotal = new System.Windows.Forms.Label();
            btnEfectivo = new System.Windows.Forms.Button();
            btnTarjetaDebito = new System.Windows.Forms.Button();
            btnTarjetaCredito = new System.Windows.Forms.Button();
            btnMercadoPago = new System.Windows.Forms.Button();
            btnCancelar = new System.Windows.Forms.Button();
            pnlHeader.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = System.Drawing.Color.FromArgb(16, 185, 129);
            pnlHeader.Controls.Add(lblTitulo);
            pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            pnlHeader.Location = new System.Drawing.Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new System.Drawing.Size(380, 60);
            pnlHeader.TabIndex = 0;
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            lblTitulo.ForeColor = System.Drawing.Color.White;
            lblTitulo.Location = new System.Drawing.Point(12, 18);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new System.Drawing.Size(263, 25);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Seleccione Método de Pago";
            // 
            // lblTextoTotal
            // 
            lblTextoTotal.AutoSize = true;
            lblTextoTotal.Font = new System.Drawing.Font("Segoe UI", 14F);
            lblTextoTotal.ForeColor = System.Drawing.Color.White;
            lblTextoTotal.Location = new System.Drawing.Point(135, 80);
            lblTextoTotal.Name = "lblTextoTotal";
            lblTextoTotal.Size = new System.Drawing.Size(111, 25);
            lblTextoTotal.TabIndex = 1;
            lblTextoTotal.Text = "Total a Pagar";
            // 
            // lblTotal
            // 
            lblTotal.Font = new System.Drawing.Font("Segoe UI", 32F, System.Drawing.FontStyle.Bold);
            lblTotal.ForeColor = System.Drawing.Color.FromArgb(16, 185, 129);
            lblTotal.Location = new System.Drawing.Point(12, 110);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new System.Drawing.Size(356, 60);
            lblTotal.TabIndex = 2;
            lblTotal.Text = "$ 0.00";
            lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnEfectivo
            // 
            btnEfectivo.BackColor = System.Drawing.Color.FromArgb(51, 65, 85);
            btnEfectivo.Cursor = System.Windows.Forms.Cursors.Hand;
            btnEfectivo.FlatAppearance.BorderSize = 0;
            btnEfectivo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnEfectivo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            btnEfectivo.ForeColor = System.Drawing.Color.White;
            btnEfectivo.Location = new System.Drawing.Point(40, 190);
            btnEfectivo.Name = "btnEfectivo";
            btnEfectivo.Size = new System.Drawing.Size(300, 50);
            btnEfectivo.TabIndex = 3;
            btnEfectivo.Text = "💵 Efectivo";
            btnEfectivo.UseVisualStyleBackColor = false;
            btnEfectivo.Click += btnEfectivo_Click;
            // 
            // btnTarjetaDebito
            // 
            btnTarjetaDebito.BackColor = System.Drawing.Color.FromArgb(51, 65, 85);
            btnTarjetaDebito.Cursor = System.Windows.Forms.Cursors.Hand;
            btnTarjetaDebito.FlatAppearance.BorderSize = 0;
            btnTarjetaDebito.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnTarjetaDebito.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            btnTarjetaDebito.ForeColor = System.Drawing.Color.White;
            btnTarjetaDebito.Location = new System.Drawing.Point(40, 250);
            btnTarjetaDebito.Name = "btnTarjetaDebito";
            btnTarjetaDebito.Size = new System.Drawing.Size(300, 50);
            btnTarjetaDebito.TabIndex = 4;
            btnTarjetaDebito.Text = "💳 Tarjeta Débito";
            btnTarjetaDebito.UseVisualStyleBackColor = false;
            btnTarjetaDebito.Click += btnTarjetaDebito_Click;
            // 
            // btnTarjetaCredito
            // 
            btnTarjetaCredito.BackColor = System.Drawing.Color.FromArgb(51, 65, 85);
            btnTarjetaCredito.Cursor = System.Windows.Forms.Cursors.Hand;
            btnTarjetaCredito.FlatAppearance.BorderSize = 0;
            btnTarjetaCredito.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnTarjetaCredito.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            btnTarjetaCredito.ForeColor = System.Drawing.Color.White;
            btnTarjetaCredito.Location = new System.Drawing.Point(40, 310);
            btnTarjetaCredito.Name = "btnTarjetaCredito";
            btnTarjetaCredito.Size = new System.Drawing.Size(300, 50);
            btnTarjetaCredito.TabIndex = 5;
            btnTarjetaCredito.Text = "💳 Tarjeta Crédito";
            btnTarjetaCredito.UseVisualStyleBackColor = false;
            btnTarjetaCredito.Click += btnTarjetaCredito_Click;
            // 
            // btnMercadoPago
            // 
            btnMercadoPago.BackColor = System.Drawing.Color.FromArgb(51, 65, 85);
            btnMercadoPago.Cursor = System.Windows.Forms.Cursors.Hand;
            btnMercadoPago.FlatAppearance.BorderSize = 0;
            btnMercadoPago.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnMercadoPago.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            btnMercadoPago.ForeColor = System.Drawing.Color.White;
            btnMercadoPago.Location = new System.Drawing.Point(40, 370);
            btnMercadoPago.Name = "btnMercadoPago";
            btnMercadoPago.Size = new System.Drawing.Size(300, 50);
            btnMercadoPago.TabIndex = 6;
            btnMercadoPago.Text = "📱 Mercado Pago";
            btnMercadoPago.UseVisualStyleBackColor = false;
            btnMercadoPago.Click += btnMercadoPago_Click;
            // 
            // btnCancelar
            // 
            btnCancelar.BackColor = System.Drawing.Color.FromArgb(239, 68, 68);
            btnCancelar.Cursor = System.Windows.Forms.Cursors.Hand;
            btnCancelar.FlatAppearance.BorderSize = 0;
            btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnCancelar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnCancelar.ForeColor = System.Drawing.Color.White;
            btnCancelar.Location = new System.Drawing.Point(40, 440);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new System.Drawing.Size(300, 40);
            btnCancelar.TabIndex = 7;
            btnCancelar.Text = "Cancelar Venta";
            btnCancelar.UseVisualStyleBackColor = false;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // FormCobro
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            ClientSize = new System.Drawing.Size(380, 510);
            Controls.Add(btnCancelar);
            Controls.Add(btnMercadoPago);
            Controls.Add(btnTarjetaCredito);
            Controls.Add(btnTarjetaDebito);
            Controls.Add(btnEfectivo);
            Controls.Add(lblTotal);
            Controls.Add(lblTextoTotal);
            Controls.Add(pnlHeader);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormCobro";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Cobro";
            CancelButton = btnCancelar;
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblTextoTotal;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Button btnEfectivo;
        private System.Windows.Forms.Button btnTarjetaDebito;
        private System.Windows.Forms.Button btnTarjetaCredito;
        private System.Windows.Forms.Button btnMercadoPago;
        private System.Windows.Forms.Button btnCancelar;
    }
}