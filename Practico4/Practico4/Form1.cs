namespace Practico4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void BFuncion_Click(object sender, KeyPressEventArgs e)
        {
            int ValIni;
            string TBHasta = ValIni.Text;


            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

            if (TBDesde.Text == "" || TBHasta.Text == "")
            {
                MessageBox.Show("Debe ingresar ambos valores");
                return;
            }
            if (TBDesde.Text <= TBHasta.Text)
            {
                MessageBox.Show("Debe ingresar ambos valores");
                return;
            }
        }


        private void TBHasta_TextChanged(object sender, EventArgs e)
        {

        }

        private void TBDesde_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
