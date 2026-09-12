using StockOS.UI.WinForms.Forms;

namespace StockOS.UI.WinForms.Forms
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

        private bool _ignorandoCambios = false;

        public FormLogin()
        {
            InitializeComponent();
            txtUsuario.TextChanged += (s, e) => { if (!_ignorandoCambios) OcultarError(); };
            txtPassword.TextChanged += (s, e) => { if (!_ignorandoCambios) OcultarError(); };
        }

        private void MostrarError(string mensaje)
        {
            lblError.ForeColor = Color.FromArgb(239, 68, 68); // Rojo
            lblError.Text = mensaje;
            lblError.Refresh();
        }

        private void MostrarExito(string mensaje)
        {
            lblError.ForeColor = Color.FromArgb(16, 185, 129); // Verde
            lblError.Text = mensaje;
            lblError.Refresh();
        }

        private void OcultarError()
        {
            lblError.Text = "";
        }

        /// <summary>
        /// Maneja el evento de clic en 'Iniciar Sesión' validando campos y credenciales.
        /// </summary>
        private async void btnIngresar_Click(object? sender, EventArgs e)
        {
            OcultarError();
            string dni = txtUsuario.Text.Trim();
            string password = txtPassword.Text;

            // 1. Validaciones del frontend
            if (string.IsNullOrWhiteSpace(dni))
            {
                MostrarError("El campo DNI es obligatorio.");
                txtUsuario.Focus();
                return;
            }

            foreach (char c in dni)
            {
                if (!char.IsDigit(c))
                {
                    MostrarError("El campo DNI solo admite números sin nada más.");
                    txtUsuario.Focus();
                    return;
                }
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                MostrarError("El campo contraseña es obligatorio.");
                txtPassword.Focus();
                return;
            }

            if (password.Length < 8)
            {
                MostrarError("La contraseña debe tener al menos 8 caracteres.");
                txtPassword.Focus();
                return;
            }

            // 2. Búsqueda y validación de credenciales (Simulación de Capa de Negocio / Backend)
            // NOTA: Cuando se implemente el Backend / Base de Datos, se reemplazará esta consulta
            // por la llamada al servicio o repositorio correspondiente (ej: await _authService.LoginAsync(dni, password))
            var usuario = _usuarios.FirstOrDefault(u =>
                u.DNI.Equals(dni, StringComparison.OrdinalIgnoreCase) &&
                u.Password == password
            );

            if (usuario != null)
            {
                UsuarioAutenticado = usuario;

                // Muestra "Ingreso exitoso" en la sección de mensajes de la vista de login
                MostrarExito("Ingreso exitoso");

                btnIngresar.Enabled = false;
                await Task.Delay(1200);
                btnIngresar.Enabled = true;

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MostrarError("Credenciales incorrectas.");
                _ignorandoCambios = true;
                txtPassword.SelectAll();
                txtPassword.Focus();
                _ignorandoCambios = false;
            }
        }

        /// <summary>
        /// Restablece y limpia los campos de DNI, contraseña y mensaje de estado.
        /// </summary>
        private void btnRecargar_Click(object? sender, EventArgs e)
        {
            _ignorandoCambios = true;
            txtUsuario.Clear();
            txtPassword.Clear();
            OcultarError();
            _ignorandoCambios = false;
            txtUsuario.Focus();
        }

        /// <summary>
        /// Cierra la aplicación de forma segura.
        /// </summary>
        private void btnSalir_Click(object? sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }
    }
}

