namespace Practico1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void BGuardar_Click(object sender, EventArgs e)
        {
            string nombreCompleto = TBApellido.Text.Trim() + " " + TBNombre.Text.Trim();
            TBGrande.Text = nombreCompleto.ToUpper();
        }
        private void BTEliminar_Click(object sender, EventArgs e)
        {
            TBGrande.Clear();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void BTSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // Verifica si se presionó la combinación de teclas Ctrl + S
            if (e.KeyCode == Keys.S && e.Control)
            {
                // Cierra directamente el formulario
                this.Close();
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            KeyPreview = true;
        }

        private void TBGrande_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
