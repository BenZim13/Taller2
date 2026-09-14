namespace StockOS.UI.WinForms.Forms
{
    partial class UcUsuarios
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
            this.pnlOpciones = new System.Windows.Forms.Panel();
            this.btnOpcionCargar = new System.Windows.Forms.Button();
            this.btnOpcionListar = new System.Windows.Forms.Button();
            this.pnlContenedorSubVista = new System.Windows.Forms.Panel();
            this.pnlOpciones.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlOpciones
            // 
            this.pnlOpciones.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.pnlOpciones.Controls.Add(this.btnOpcionCargar);
            this.pnlOpciones.Controls.Add(this.btnOpcionListar);
            this.pnlOpciones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlOpciones.Location = new System.Drawing.Point(0, 0);
            this.pnlOpciones.Name = "pnlOpciones";
            this.pnlOpciones.Size = new System.Drawing.Size(1000, 600);
            this.pnlOpciones.TabIndex = 0;
            // 
            // btnOpcionCargar
            // 
            this.btnOpcionCargar.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnOpcionCargar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.btnOpcionCargar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpcionCargar.FlatAppearance.BorderSize = 0;
            this.btnOpcionCargar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpcionCargar.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnOpcionCargar.ForeColor = System.Drawing.Color.White;
            this.btnOpcionCargar.Location = new System.Drawing.Point(520, 200);
            this.btnOpcionCargar.Name = "btnOpcionCargar";
            this.btnOpcionCargar.Size = new System.Drawing.Size(300, 150);
            this.btnOpcionCargar.TabIndex = 1;
            this.btnOpcionCargar.Text = "➕\r\n\r\nRegistrar Usuarios";
            this.btnOpcionCargar.UseVisualStyleBackColor = false;
            // 
            // btnOpcionListar
            // 
            this.btnOpcionListar.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnOpcionListar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.btnOpcionListar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpcionListar.FlatAppearance.BorderSize = 0;
            this.btnOpcionListar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpcionListar.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnOpcionListar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.btnOpcionListar.Location = new System.Drawing.Point(180, 200);
            this.btnOpcionListar.Name = "btnOpcionListar";
            this.btnOpcionListar.Size = new System.Drawing.Size(300, 150);
            this.btnOpcionListar.TabIndex = 0;
            this.btnOpcionListar.Text = "📋\r\n\r\nListar Usuarios";
            this.btnOpcionListar.UseVisualStyleBackColor = false;
            // 
            // pnlContenedorSubVista
            // 
            this.pnlContenedorSubVista.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.pnlContenedorSubVista.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContenedorSubVista.Location = new System.Drawing.Point(0, 0);
            this.pnlContenedorSubVista.Name = "pnlContenedorSubVista";
            this.pnlContenedorSubVista.Size = new System.Drawing.Size(1000, 600);
            this.pnlContenedorSubVista.TabIndex = 1;
            this.pnlContenedorSubVista.Visible = false;
            // 
            // UcUsuarios
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.Controls.Add(this.pnlOpciones);
            this.Controls.Add(this.pnlContenedorSubVista);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "UcUsuarios";
            this.Size = new System.Drawing.Size(1000, 600);
            this.pnlOpciones.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlOpciones;
        private System.Windows.Forms.Button btnOpcionListar;
        private System.Windows.Forms.Button btnOpcionCargar;
        private System.Windows.Forms.Panel pnlContenedorSubVista;
    }
}

