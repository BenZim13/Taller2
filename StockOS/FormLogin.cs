namespace Vistas_Usuarios
{
    public partial class FormLogin : Form
    {
        /// <summary>
        /// Modelo interno temporal para simular los usuarios en memoria.
        /// </summary>
        public class UsuarioMock
        {
            public string DNI { get; set; }
            public string Password { get; set; }
            public string Rol { get; set; }

            public UsuarioMock(string dni, string password, string rol)
            {
                DNI = dni;
                Password = password;
                Rol = rol;
            }
        }

        // Lista en memoria con usuarios de prueba según los roles del sistema
        private readonly List<UsuarioMock> _usuarios = new()
        {
            new UsuarioMock("11111111", "admin123", "Gerente"),
            new UsuarioMock("22222222", "caja123", "Cajero"),
            new UsuarioMock("33333333", "depo123", "Repositor")
        };

        /// <summary>
        /// Almacena el usuario autenticado para que pueda ser consumido por el formulario principal.
        /// </summary>
        public UsuarioMock? UsuarioAutenticado { get; private set; }

        public FormLogin()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Maneja el evento de clic en 'Iniciar Sesión' validando campos y credenciales.
        /// </summary>
        private void btnIngresar_Click(object? sender, EventArgs e)
        {
            string dni = txtUsuario.Text.Trim();
            string password = txtPassword.Text;

            // 1. Validación de campos obligatorios
            if (string.IsNullOrWhiteSpace(dni) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show(
                    "Por favor, complete todos los campos para continuar.",
                    "Campos requeridos",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                if (string.IsNullOrWhiteSpace(dni))
                {
                    txtUsuario.Focus();
                }
                else
                {
                    txtPassword.Focus();
                }
                return;
            }

            // 2. Búsqueda y validación de credenciales en la lista simulada
            var usuario = _usuarios.FirstOrDefault(u =>
                u.DNI.Equals(dni, StringComparison.OrdinalIgnoreCase) &&
                u.Password == password
            );

            if (usuario != null)
            {
                UsuarioAutenticado = usuario;

                MessageBox.Show(
                    $"Bienvenido. Ingresando con rol: {usuario.Rol}",
                    "Acceso Concedido",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show(
                    "Credenciales incorrectas.",
                    "Error de Autenticación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                txtPassword.Clear();
                txtPassword.Focus();
            }
        }

        /// <summary>
        /// Cierra la aplicación de forma segura.
        /// </summary>
        private void btnSalir_Click(object? sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}

