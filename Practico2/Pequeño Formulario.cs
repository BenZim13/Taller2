namespace Practico2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permite solo números y la tecla de borrar (Control)
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Cancela el caracter
            }
        }

        private void TextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permite solo letras, espacio y tecla de borrar
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void TextBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permite solo letras, espacio y tecla de borrar
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            // Punto 7: Verificar si algún campo está vacío usando el operador || (OR)
            if (string.IsNullOrWhiteSpace(TDni.Text) ||
                string.IsNullOrWhiteSpace(TApellido.Text) ||
                string.IsNullOrWhiteSpace(TNombre.Text))
            {
                MessageBox.Show("Debe Completar todos los campos", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Punto 8 y 9: Variable de tipo DialogResult (equivalente en C# a MsgBoxResult)
            // Mensaje de consulta con botones Sí/No, icono de pregunta y foco por defecto en el primer botón (Sí)
            DialogResult ask = MessageBox.Show("¿Seguro que desea insertar un nuevo Cliente?",
                "Confirmar Insercion",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1);

            if (ask == DialogResult.Yes)
            {
                // Punto 6: Modificar el label LModificar con el Nombre y Apellido
                LModificar.Text = TNombre.Text + " " + TApellido.Text;

                // Punto 10: Mensaje de información
                MessageBox.Show($"El Cliente: {TNombre.Text} {TApellido.Text} se insertó correctamente",
                    "Guardar",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void BEliminar_Click(object sender, EventArgs e)
        {
            // Punto 11: Mensaje de advertencia con foco en la opción "NO" (MessageBoxDefaultButton.Button2)
            DialogResult ask = MessageBox.Show($"Está apunto de eliminar el Cliente: {TNombre.Text} {TApellido.Text}",
                "Confirmar Eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Exclamation,
                MessageBoxDefaultButton.Button2);

            // Punto 12: Si se presionó "Sí", limpiar todo y confirmar
            if (ask == DialogResult.Yes)
            {
                MessageBox.Show($"El Cliente: {TApellido.Text} {TNombre.Text} se eliminó correctamente",
                    "Eliminar",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                // Limpieza de todos los campos
                TDni.Clear();
                TApellido.Clear();
                TNombre.Clear();
                LModificar.Text = "Modificar"; // Restablece el label a su texto original
            }
        }
    }
}
