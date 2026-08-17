namespace Practico2
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
            LDni = new Label();
            LNya = new Label();
            LApellido = new Label();
            LNombre = new Label();
            TDni = new TextBox();
            TNombre = new TextBox();
            TApellido = new TextBox();
            BGuardar = new Button();
            BEliminar = new Button();
            LModificar = new Label();
            SuspendLayout();
            // 
            // LDni
            // 
            LDni.AutoSize = true;
            LDni.Location = new Point(12, 69);
            LDni.Name = "LDni";
            LDni.Size = new Size(35, 20);
            LDni.TabIndex = 0;
            LDni.Text = "DNI";
           
            // 
            // LNya
            // 
            LNya.AutoSize = true;
            LNya.Location = new Point(12, 19);
            LNya.Name = "LNya";
            LNya.Size = new Size(139, 20);
            LNya.TabIndex = 1;
            LNya.Text = "Nombre y Apellido:";
            // 
            // LApellido
            // 
            LApellido.AutoSize = true;
            LApellido.Location = new Point(12, 111);
            LApellido.Name = "LApellido";
            LApellido.Size = new Size(66, 20);
            LApellido.TabIndex = 2;
            LApellido.Text = "Apellido";
            // 
            // LNombre
            // 
            LNombre.AutoSize = true;
            LNombre.Location = new Point(12, 148);
            LNombre.Name = "LNombre";
            LNombre.Size = new Size(64, 20);
            LNombre.TabIndex = 3;
            LNombre.Text = "Nombre";
            // 
            // TDni
            // 
            TDni.Location = new Point(145, 62);
            TDni.Name = "TDni";
            TDni.Size = new Size(125, 27);
            TDni.TabIndex = 5;
            TDni.TextChanged += TDni_TextChanged;
            TDni.KeyPress += TDni_KeyPress_1;
            // 
            // TNombre
            // 
            TNombre.Location = new Point(145, 148);
            TNombre.Name = "TNombre";
            TNombre.Size = new Size(125, 27);
            TNombre.TabIndex = 6;
            TNombre.TextChanged += TNombre_TextChanged;
            TNombre.KeyPress += TNombre_KeyPress;
            // 
            // TApellido
            // 
            TApellido.Location = new Point(145, 104);
            TApellido.Name = "TApellido";
            TApellido.Size = new Size(125, 27);
            TApellido.TabIndex = 7;
            TApellido.TextChanged += TApellido_TextChanged;
            TApellido.KeyPress += TApellido_KeyPress;
            // 
            // BGuardar
            // 
            BGuardar.Location = new Point(12, 259);
            BGuardar.Name = "BGuardar";
            BGuardar.Size = new Size(94, 29);
            BGuardar.TabIndex = 8;
            BGuardar.Text = "Guardar";
            BGuardar.UseVisualStyleBackColor = true;
            BGuardar.Click += BGuardar_Click;
            // 
            // BEliminar
            // 
            BEliminar.Location = new Point(176, 259);
            BEliminar.Name = "BEliminar";
            BEliminar.Size = new Size(94, 29);
            BEliminar.TabIndex = 9;
            BEliminar.Text = "Eliminar";
            BEliminar.UseVisualStyleBackColor = true;
            BEliminar.Click += BEliminar_Click;
            // 
            // LModificar
            // 
            LModificar.AutoSize = true;
            LModificar.ForeColor = Color.Red;
            LModificar.Location = new Point(176, 19);
            LModificar.Name = "LModificar";
            LModificar.Size = new Size(73, 20);
            LModificar.TabIndex = 10;
            LModificar.Text = "modificar";
            LModificar.Click += LModificar_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(282, 353);
            Controls.Add(LModificar);
            Controls.Add(BEliminar);
            Controls.Add(BGuardar);
            Controls.Add(TApellido);
            Controls.Add(TNombre);
            Controls.Add(TDni);
            Controls.Add(LNombre);
            Controls.Add(LApellido);
            Controls.Add(LNya);
            Controls.Add(LDni);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Pequeño Formulario";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label LDni;
        private Label LNya;
        private Label LApellido;
        private Label LNombre;
        private TextBox TDni;
        private TextBox TNombre;
        private TextBox TApellido;
        private Button BGuardar;
        private Button BEliminar;
        private Label LModificar;
    }
}
