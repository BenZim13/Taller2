namespace Practico2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void TDni_TextChanged(object sender, EventArgs e)
        {

        }

        private void TNombre_TextChanged(object sender, EventArgs e)
        {

        }

        private void TApellido_TextChanged(object sender, EventArgs e)
        {

        }



        private void TDni_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            // Permite solo dígitos y la tecla de retroceso (Backspace).
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true; // Ignora el carácter si no es un número.
            }
        }


        private void TApellido_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permite solo letras, espacios y la tecla de retroceso (Backspace).
            if (!char.IsLetter(e.KeyChar) && e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Space)
            {
                e.Handled = true; // Ignora el carácter si no es una letra.
            }
        }

        private void TNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permite solo letras, espacios y la tecla de retroceso (Backspace).
            if (!char.IsLetter(e.KeyChar) && e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Space)
            {
                e.Handled = true; // Ignora el carácter si no es una letra.
            }
        }



        private void BGuardar_Click(object sender, EventArgs e)
        {
            // Verifica si alguno de los campos de texto está vacío o con espacios en blanco.
            if (string.IsNullOrWhiteSpace(TDni.Text) ||
                string.IsNullOrWhiteSpace(TApellido.Text) ||
                string.IsNullOrWhiteSpace(TNombre.Text))
            {
                // Muestra un mensaje de error si los campos están incompletos.
                MessageBox.Show("Debe Completar todos los campos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {// equivalente de MsgBoxResult es DialogResult
                DialogResult ask = MessageBox.Show(
                    "¿Seguro que deseas insertar un nuevo cliente?",
                    "Confirmar Inserción",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                    );

                if (ask == DialogResult.Yes)
                {
                    // Si todos los campos están completos, concatena el nombre y apellido
                    // y asigna el resultado al Label "Lmodificar".
                    string nombreCompleto = TNombre.Text + " " + TApellido.Text;
                    LModificar.Text = nombreCompleto;

                    MessageBox.Show(
                        //Compuesto por:
                        //Texto de mensaje
                        //Titulo ventana
                        //Botones 
                        //Icono
                     "El cliente: " + nombreCompleto + " se insertó correctamente.",
                     "Confirmación",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Information
                    );
                }
                else
                {
                    LModificar.Text = "Inserción cancelada.";
                }
            }
        }

        private void BEliminar_Click(object sender, EventArgs e)
        {
            string nombreCompleto = TNombre.Text + " " + TApellido.Text;
            if (string.IsNullOrWhiteSpace(TDni.Text) ||
                string.IsNullOrWhiteSpace(TApellido.Text) ||
                string.IsNullOrWhiteSpace(TNombre.Text))
            {
                // Muestra un mensaje de error si los campos están incompletos.
                MessageBox.Show("Campos incompletos, no existe cliente para eliminar",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else
            {
                DialogResult ask = MessageBox.Show(
                    "Esta a punto de eliminar el cliente: " + nombreCompleto,
                    "Confirmar Eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Exclamation
                    );
                if (ask == DialogResult.Yes)
                {
                    LModificar.Text = "modificar";
                    TNombre.Text = "";
                    TApellido.Text = "";
                    TDni.Text = "";
                    MessageBox.Show(
                        "El cliente: " + nombreCompleto + " se elimino correctamente.",
                        "Eliminar",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                        );
                }
             }
        }

        private void LModificar_Click(object sender, EventArgs e)
        {

        }

       
    }
}
