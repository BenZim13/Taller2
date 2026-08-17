namespace Practico4
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
            TBListaNro = new TextBox();
            TBHasta = new TextBox();
            LListaNro = new Label();
            LHasta = new Label();
            LDesde = new Label();
            BFuncion = new Button();
            TBDesde = new TextBox();
            SuspendLayout();
            // 
            // TBListaNro
            // 
            TBListaNro.Location = new Point(433, 138);
            TBListaNro.Multiline = true;
            TBListaNro.Name = "TBListaNro";
            TBListaNro.Size = new Size(320, 260);
            TBListaNro.TabIndex = 1;
            // 
            // TBHasta
            // 
            TBHasta.Location = new Point(98, 208);
            TBHasta.Name = "TBHasta";
            TBHasta.Size = new Size(125, 27);
            TBHasta.TabIndex = 3;
            TBHasta.TextChanged += TBHasta_TextChanged;
            // 
            // LListaNro
            // 
            LListaNro.AutoSize = true;
            LListaNro.Location = new Point(537, 76);
            LListaNro.Name = "LListaNro";
            LListaNro.Size = new Size(124, 20);
            LListaNro.TabIndex = 4;
            LListaNro.Text = "Lista de Numeros";
            LListaNro.TextAlign = ContentAlignment.TopCenter;
            // 
            // LHasta
            // 
            LHasta.AutoSize = true;
            LHasta.Location = new Point(29, 215);
            LHasta.Name = "LHasta";
            LHasta.Size = new Size(47, 20);
            LHasta.TabIndex = 5;
            LHasta.Text = "Hasta";
            // 
            // LDesde
            // 
            LDesde.AutoSize = true;
            LDesde.Location = new Point(29, 141);
            LDesde.Name = "LDesde";
            LDesde.Size = new Size(51, 20);
            LDesde.TabIndex = 6;
            LDesde.Text = "Desde";
            // 
            // BFuncion
            // 
            BFuncion.Location = new Point(264, 134);
            BFuncion.Name = "BFuncion";
            BFuncion.Size = new Size(125, 27);
            BFuncion.TabIndex = 7;
            BFuncion.Text = "Generar Funcion";
            BFuncion.UseVisualStyleBackColor = true;
            BFuncion.Click += BFuncion_Click;
            // 
            // TBDesde
            // 
            TBDesde.Location = new Point(98, 138);
            TBDesde.Name = "TBDesde";
            TBDesde.Size = new Size(125, 27);
            TBDesde.TabIndex = 8;
            TBDesde.TextChanged += TBDesde_TextChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(782, 453);
            Controls.Add(TBDesde);
            Controls.Add(BFuncion);
            Controls.Add(LDesde);
            Controls.Add(LHasta);
            Controls.Add(LListaNro);
            Controls.Add(TBHasta);
            Controls.Add(TBListaNro);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox TBListaNro;
        private TextBox TBHasta;
        private Label LListaNro;
        private Label LHasta;
        private Label LDesde;
        private Button BFuncion;
        private TextBox TBDesde;
    }
}
