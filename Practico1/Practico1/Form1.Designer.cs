namespace Practico1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            BGuardar = new Button();
            TBApellido = new TextBox();
            LApellido = new Label();
            LNombre = new Label();
            TBNombre = new TextBox();
            TBGrande = new TextBox();
            BTEliminar = new Button();
            BTSalir = new Button();
            SuspendLayout();
            // 
            // BGuardar
            // 
            BGuardar.Location = new Point(21, 172);
            BGuardar.Name = "BGuardar";
            BGuardar.Size = new Size(94, 29);
            BGuardar.TabIndex = 1;
            BGuardar.Text = "Guardar\r\n";
            BGuardar.UseVisualStyleBackColor = true;
            BGuardar.Click += BGuardar_Click;
            // 
            // TBApellido
            // 
            TBApellido.Location = new Point(115, 52);
            TBApellido.Name = "TBApellido";
            TBApellido.Size = new Size(125, 27);
            TBApellido.TabIndex = 4;
            TBApellido.TextChanged += textBox1_TextChanged;
            // 
            // LApellido
            // 
            LApellido.AutoSize = true;
            LApellido.Location = new Point(21, 52);
            LApellido.Name = "LApellido";
            LApellido.Size = new Size(66, 20);
            LApellido.TabIndex = 5;
            LApellido.Text = "Apellido";
            // 
            // LNombre
            // 
            LNombre.AutoSize = true;
            LNombre.Location = new Point(21, 105);
            LNombre.Name = "LNombre";
            LNombre.Size = new Size(64, 20);
            LNombre.TabIndex = 6;
            LNombre.Text = "Nombre";
            // 
            // TBNombre
            // 
            TBNombre.Location = new Point(115, 102);
            TBNombre.Name = "TBNombre";
            TBNombre.Size = new Size(125, 27);
            TBNombre.TabIndex = 7;
            TBNombre.TextChanged += textBox2_TextChanged;
            // 
            // TBGrande
            // 
            TBGrande.Location = new Point(276, 52);
            TBGrande.Multiline = true;
            TBGrande.Name = "TBGrande";
            TBGrande.Size = new Size(180, 149);
            TBGrande.TabIndex = 8;
            // 
            // BTEliminar
            // 
            BTEliminar.Location = new Point(146, 172);
            BTEliminar.Name = "BTEliminar";
            BTEliminar.Size = new Size(94, 29);
            BTEliminar.TabIndex = 9;
            BTEliminar.Text = "Eliminar";
            BTEliminar.UseVisualStyleBackColor = true;
            BTEliminar.Click += BTEliminar_Click;
            // 
            // BTSalir
            // 
            BTSalir.Location = new Point(362, 207);
            BTSalir.Name = "BTSalir";
            BTSalir.Size = new Size(94, 29);
            BTSalir.TabIndex = 10;
            BTSalir.Text = "Salir";
            BTSalir.UseVisualStyleBackColor = true;
            BTSalir.Click += BTSalir_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(482, 253);
            Controls.Add(BTSalir);
            Controls.Add(BTEliminar);
            Controls.Add(TBGrande);
            Controls.Add(TBNombre);
            Controls.Add(LNombre);
            Controls.Add(LApellido);
            Controls.Add(TBApellido);
            Controls.Add(BGuardar);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Mi Primer Forms";
            Load += Form1_Load;
            KeyDown += Form1_KeyDown;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button BGuardar;
        private TextBox TBApellido;
        private Label LApellido;
        private Label LNombre;
        private TextBox TBNombre;
        private TextBox TBGrande;
        private Button BTEliminar;
        private Button BTSalir;
    }
}
