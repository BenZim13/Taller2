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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            LDni = new Label();
            LNya = new Label();
            LApellido = new Label();
            LNombre = new Label();
            TDni = new TextBox();
            TNombre = new TextBox();
            TApellido = new TextBox();
            LModificar = new Label();
            TTelefono = new TextBox();
            LTelefono = new Label();
            LTarjeta = new Label();
            CBNaranja = new CheckBox();
            CBVisa = new CheckBox();
            CBMastercard = new CheckBox();
            groupBox1 = new GroupBox();
            LCliente = new Label();
            RBVaron = new RadioButton();
            RBMujer = new RadioButton();
            pictureBox1 = new PictureBox();
            IBSalir = new FontAwesome.Sharp.IconButton();
            IBEliminar = new FontAwesome.Sharp.IconButton();
            IBGuardar = new FontAwesome.Sharp.IconButton();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // LDni
            // 
            LDni.AutoSize = true;
            LDni.Location = new Point(3, 52);
            LDni.Name = "LDni";
            LDni.Size = new Size(45, 20);
            LDni.TabIndex = 0;
            LDni.Text = "* DNI";
            // 
            // LNya
            // 
            LNya.AutoSize = true;
            LNya.Location = new Point(3, 2);
            LNya.Name = "LNya";
            LNya.Size = new Size(139, 20);
            LNya.TabIndex = 1;
            LNya.Text = "Nombre y Apellido:";
            // 
            // LApellido
            // 
            LApellido.AutoSize = true;
            LApellido.Location = new Point(3, 94);
            LApellido.Name = "LApellido";
            LApellido.Size = new Size(76, 20);
            LApellido.TabIndex = 2;
            LApellido.Text = "* Apellido";
            // 
            // LNombre
            // 
            LNombre.AutoSize = true;
            LNombre.Location = new Point(3, 131);
            LNombre.Name = "LNombre";
            LNombre.Size = new Size(74, 20);
            LNombre.TabIndex = 3;
            LNombre.Text = "* Nombre";
            // 
            // TDni
            // 
            TDni.Location = new Point(136, 45);
            TDni.Name = "TDni";
            TDni.Size = new Size(125, 27);
            TDni.TabIndex = 5;
            TDni.TextChanged += TDni_TextChanged;
            TDni.KeyPress += TDni_KeyPress_1;
            // 
            // TNombre
            // 
            TNombre.Location = new Point(136, 131);
            TNombre.Name = "TNombre";
            TNombre.Size = new Size(125, 27);
            TNombre.TabIndex = 6;
            TNombre.TextChanged += TNombre_TextChanged;
            TNombre.KeyPress += TNombre_KeyPress;
            // 
            // TApellido
            // 
            TApellido.Location = new Point(136, 87);
            TApellido.Name = "TApellido";
            TApellido.Size = new Size(125, 27);
            TApellido.TabIndex = 7;
            TApellido.TextChanged += TApellido_TextChanged;
            TApellido.KeyPress += TApellido_KeyPress;
            // 
            // LModificar
            // 
            LModificar.AutoSize = true;
            LModificar.ForeColor = Color.Red;
            LModificar.Location = new Point(167, 2);
            LModificar.Name = "LModificar";
            LModificar.Size = new Size(73, 20);
            LModificar.TabIndex = 10;
            LModificar.Text = "modificar";
            LModificar.Click += LModificar_Click;
            // 
            // TTelefono
            // 
            TTelefono.Location = new Point(136, 177);
            TTelefono.Name = "TTelefono";
            TTelefono.Size = new Size(125, 27);
            TTelefono.TabIndex = 11;
            // 
            // LTelefono
            // 
            LTelefono.AutoSize = true;
            LTelefono.Location = new Point(3, 177);
            LTelefono.Name = "LTelefono";
            LTelefono.Size = new Size(67, 20);
            LTelefono.TabIndex = 12;
            LTelefono.Text = "Telefono";
            // 
            // LTarjeta
            // 
            LTarjeta.AutoSize = true;
            LTarjeta.Location = new Point(3, 230);
            LTarjeta.Name = "LTarjeta";
            LTarjeta.Size = new Size(138, 20);
            LTarjeta.TabIndex = 13;
            LTarjeta.Text = "Tarjeta de creditos: ";
            // 
            // CBNaranja
            // 
            CBNaranja.AutoSize = true;
            CBNaranja.Location = new Point(147, 241);
            CBNaranja.Name = "CBNaranja";
            CBNaranja.Size = new Size(83, 24);
            CBNaranja.TabIndex = 14;
            CBNaranja.Text = "Naranja";
            CBNaranja.UseVisualStyleBackColor = true;
            CBNaranja.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // CBVisa
            // 
            CBVisa.AutoSize = true;
            CBVisa.Location = new Point(147, 271);
            CBVisa.Name = "CBVisa";
            CBVisa.Size = new Size(58, 24);
            CBVisa.TabIndex = 15;
            CBVisa.Text = "Visa";
            CBVisa.UseVisualStyleBackColor = true;
            // 
            // CBMastercard
            // 
            CBMastercard.AutoSize = true;
            CBMastercard.Location = new Point(147, 301);
            CBMastercard.Name = "CBMastercard";
            CBMastercard.Size = new Size(105, 24);
            CBMastercard.TabIndex = 16;
            CBMastercard.Text = "Mastercard";
            CBMastercard.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.BackColor = SystemColors.ControlDark;
            groupBox1.Controls.Add(CBMastercard);
            groupBox1.Controls.Add(CBVisa);
            groupBox1.Controls.Add(CBNaranja);
            groupBox1.Controls.Add(LTarjeta);
            groupBox1.Controls.Add(LTelefono);
            groupBox1.Controls.Add(TTelefono);
            groupBox1.Controls.Add(LModificar);
            groupBox1.Controls.Add(TApellido);
            groupBox1.Controls.Add(TNombre);
            groupBox1.Controls.Add(TDni);
            groupBox1.Controls.Add(LNombre);
            groupBox1.Controls.Add(LApellido);
            groupBox1.Controls.Add(LNya);
            groupBox1.Controls.Add(LDni);
            groupBox1.Location = new Point(39, 72);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(268, 332);
            groupBox1.TabIndex = 17;
            groupBox1.TabStop = false;
            groupBox1.Text = "groupBox1";
            // 
            // LCliente
            // 
            LCliente.AutoSize = true;
            LCliente.BackColor = Color.Transparent;
            LCliente.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LCliente.ForeColor = SystemColors.Highlight;
            LCliente.Location = new Point(206, 9);
            LCliente.Name = "LCliente";
            LCliente.Size = new Size(217, 41);
            LCliente.TabIndex = 18;
            LCliente.Text = "Nuevo Cliente";
            LCliente.TextAlign = ContentAlignment.TopCenter;
            // 
            // RBVaron
            // 
            RBVaron.AutoSize = true;
            RBVaron.Checked = true;
            RBVaron.Location = new Point(359, 312);
            RBVaron.Name = "RBVaron";
            RBVaron.Size = new Size(68, 24);
            RBVaron.TabIndex = 20;
            RBVaron.TabStop = true;
            RBVaron.Text = "Varon";
            RBVaron.UseVisualStyleBackColor = true;
            RBVaron.CheckedChanged += RBVaron_CheckedChanged;
            // 
            // RBMujer
            // 
            RBMujer.AutoSize = true;
            RBMujer.Location = new Point(454, 312);
            RBMujer.Name = "RBMujer";
            RBMujer.Size = new Size(68, 24);
            RBMujer.TabIndex = 21;
            RBMujer.Text = "Mujer";
            RBMujer.UseVisualStyleBackColor = true;
            RBMujer.CheckedChanged += RBMujer_CheckedChanged;
            // 
            // pictureBox1
            // 
            pictureBox1.BorderStyle = BorderStyle.Fixed3D;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(359, 117);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(163, 159);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 19;
            pictureBox1.TabStop = false;
            // 
            // IBSalir
            // 
            IBSalir.IconChar = FontAwesome.Sharp.IconChar.SignOut;
            IBSalir.IconColor = Color.Black;
            IBSalir.IconFont = FontAwesome.Sharp.IconFont.Auto;
            IBSalir.ImageAlign = ContentAlignment.MiddleLeft;
            IBSalir.Location = new Point(381, 437);
            IBSalir.Name = "IBSalir";
            IBSalir.Size = new Size(113, 55);
            IBSalir.TabIndex = 22;
            IBSalir.Text = "Salir";
            IBSalir.TextAlign = ContentAlignment.MiddleRight;
            IBSalir.UseVisualStyleBackColor = true;
            IBSalir.Click += IBSalir_Click;
            // 
            // IBEliminar
            // 
            IBEliminar.Flip = FontAwesome.Sharp.FlipOrientation.Horizontal;
            IBEliminar.IconChar = FontAwesome.Sharp.IconChar.Trash;
            IBEliminar.IconColor = Color.Black;
            IBEliminar.IconFont = FontAwesome.Sharp.IconFont.Auto;
            IBEliminar.IconSize = 35;
            IBEliminar.ImageAlign = ContentAlignment.MiddleLeft;
            IBEliminar.Location = new Point(194, 437);
            IBEliminar.Name = "IBEliminar";
            IBEliminar.Size = new Size(113, 55);
            IBEliminar.TabIndex = 23;
            IBEliminar.Text = "Eliminar";
            IBEliminar.TextAlign = ContentAlignment.MiddleRight;
            IBEliminar.UseVisualStyleBackColor = true;
            IBEliminar.Click += IBEliminar_Click;
            // 
            // IBGuardar
            // 
            IBGuardar.IconChar = FontAwesome.Sharp.IconChar.Save;
            IBGuardar.IconColor = Color.Black;
            IBGuardar.IconFont = FontAwesome.Sharp.IconFont.Auto;
            IBGuardar.IconSize = 40;
            IBGuardar.ImageAlign = ContentAlignment.MiddleLeft;
            IBGuardar.Location = new Point(42, 437);
            IBGuardar.Name = "IBGuardar";
            IBGuardar.Size = new Size(113, 55);
            IBGuardar.TabIndex = 24;
            IBGuardar.Text = "Guardar";
            IBGuardar.TextAlign = ContentAlignment.MiddleRight;
            IBGuardar.UseVisualStyleBackColor = true;
            IBGuardar.Click += IBGuardar_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(582, 553);
            Controls.Add(IBGuardar);
            Controls.Add(IBEliminar);
            Controls.Add(IBSalir);
            Controls.Add(RBMujer);
            Controls.Add(RBVaron);
            Controls.Add(pictureBox1);
            Controls.Add(LCliente);
            Controls.Add(groupBox1);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Pequeño Formulario";
            Load += Form1_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
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
        private Label LModificar;
        private TextBox TTelefono;
        private Label LTelefono;
        private Label LTarjeta;
        private CheckBox CBNaranja;
        private CheckBox CBVisa;
        private CheckBox CBMastercard;
        private GroupBox groupBox1;
        private Label LCliente;
        private RadioButton RBVaron;
        private RadioButton RBMujer;
        private PictureBox pictureBox1;
        private FontAwesome.Sharp.IconButton IBSalir;
        private FontAwesome.Sharp.IconButton IBEliminar;
        private FontAwesome.Sharp.IconButton IBGuardar;
    }
}