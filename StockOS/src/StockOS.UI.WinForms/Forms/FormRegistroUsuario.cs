using StockOS.Application.Services;
using StockOS.Domain.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using BCrypt.Net;

namespace StockOS.UI.WinForms.Forms
{
    public partial class FormRegistroUsuario : Form
    {
        private readonly IEmpleadoService _empleadoService;
        private readonly ISucursalService _sucursalService;
        private readonly IRolService _rolService;
        private readonly IAuthService _authService;
        private Empleado? _empleadoEdicion;

        public event Action? OperacionTerminada;
        public event Action? AlVolver;

        public FormRegistroUsuario(IEmpleadoService empleadoService, ISucursalService sucursalService,
            IRolService rolService, IAuthService authService)
        {
            InitializeComponent();
            _empleadoService = empleadoService;
            _sucursalService = sucursalService;
            _rolService = rolService;
            _authService = authService;


            btnGuardar.Click += btnGuardar_Click;
            btnVolver.Click += (s, e) => AlVolver?.Invoke();
            btnCancelar.Click += btnCancelar_Click;
            this.Load += FormRegistroUsuario_Load;
        }


        public void PrepararParaEdicion(Empleado empleado)
        {
            _empleadoEdicion = empleado;
            CargarDatosEdicion();
        }

        public void LimpiarFormulario()
        {
            _empleadoEdicion = null;
            txtDNI.Clear();
            txtNombre.Clear();
            textApellido.Clear();
            textEmail.Clear();
            txtDireccion.Clear();
            txtCelular.Clear();
            txtPassword.Clear();
            lblTitulo.Text = "Carga de Personal";
            this.Text = "Registro de Usuario";
            lblCodigo.Text = "Contraseña (8 dígitos)";
            if (cmbRol.Items.Count > 0) cmbRol.SelectedIndex = 0;
            if (cmbSucursal.Items.Count > 0) cmbSucursal.SelectedIndex = 0;
        }

        private void CargarDatosEdicion()
        {
            if (_empleadoEdicion != null)
            {
                lblTitulo.Text = "Modificar Personal";
                this.Text = "Modificar Empleado";
                lblCodigo.Text = "Nueva Contraseña (dejar vacío si no cambia)";

                txtDNI.Text = _empleadoEdicion.Dni;
                txtNombre.Text = _empleadoEdicion.Nombre;
                textApellido.Text = _empleadoEdicion.Apellido;
                textEmail.Text = _empleadoEdicion.Email;
                txtDireccion.Text = _empleadoEdicion.Direccion;
                txtCelular.Text = _empleadoEdicion.Telefono;
                txtPassword.Clear();

                if (_empleadoEdicion.IdRol > 0 && cmbRol.DataSource != null)
                {
                    cmbRol.SelectedValue = _empleadoEdicion.IdRol;
                }

                if (_empleadoEdicion.IdSucursal > 0 && cmbSucursal.DataSource != null)
                {
                    cmbSucursal.SelectedValue = _empleadoEdicion.IdSucursal;
                }
            }
        }

        private void CargarCombos()
        {
            // Cargar sucursales
            var sucursales = _sucursalService.ObtenerSucursales().ToList();
            if (sucursales.Any())
            {
                cmbSucursal.DataSource = sucursales;
                cmbSucursal.DisplayMember = "Nombre";
                cmbSucursal.ValueMember = "IdSucursal";
                cmbSucursal.SelectedIndex = 0;
            }

            // Cargar roles dinámicamente desde la BD
            var roles = _rolService.ObtenerRoles().ToList();
            if (roles.Any())
            {
                cmbRol.DataSource = roles;
                cmbRol.DisplayMember = "Nombre";
                cmbRol.ValueMember = "IdRol";
                cmbRol.SelectedIndex = 0;
            }
        }

        private void FormRegistroUsuario_Load(object? sender, EventArgs e)
        {
            CargarCombos();

            // Si es modo modificación, prellenar campos
            if (_empleadoEdicion != null)
            {
                lblTitulo.Text = "Modificar Personal";
                this.Text = "Modificar Empleado";
                lblCodigo.Text = "Nueva Contraseña (dejar vacío si no cambia)";

                txtDNI.Text = _empleadoEdicion.Dni;
                txtNombre.Text = _empleadoEdicion.Nombre;
                textApellido.Text = _empleadoEdicion.Apellido;
                textEmail.Text = _empleadoEdicion.Email;
                txtDireccion.Text = _empleadoEdicion.Direccion;
                txtCelular.Text = _empleadoEdicion.Telefono;
                txtPassword.Clear();

                if (_empleadoEdicion.IdRol > 0)
                {
                    cmbRol.SelectedValue = _empleadoEdicion.IdRol;
                }

                if (_empleadoEdicion.IdSucursal > 0)
                {
                    cmbSucursal.SelectedValue = _empleadoEdicion.IdSucursal;
                }
            }
        }

        private async void btnGuardar_Click(object? sender, EventArgs e)
        {
            string nombre = txtNombre.Text.Trim();
            string apellido = textApellido.Text.Trim();
            string dni = txtDNI.Text.Trim();
            string email = textEmail.Text.Trim();
            string direccion = txtDireccion.Text.Trim();
            string celular = txtCelular.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(nombre) ||
                string.IsNullOrWhiteSpace(apellido) ||
                string.IsNullOrWhiteSpace(dni) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(direccion) ||
                string.IsNullOrWhiteSpace(celular) ||
                cmbRol.SelectedValue == null ||
                cmbSucursal.SelectedValue == null)
            {
                MessageBox.Show("Todos los campos (Nombre, Apellido, DNI, Email, Dirección, Celular, Rol y Sucursal) son obligatorios.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idRol = (int)cmbRol.SelectedValue;
            int idSucursal = (int)cmbSucursal.SelectedValue;

            if (_empleadoEdicion == null)
            {
                // ==========================================
                // MODO ALTA (NUEVO USUARIO)
                // ==========================================
                if (string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("La contraseña es obligatoria para un nuevo usuario.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var nuevoEmpleado = new Empleado
                {
                    Nombre = nombre,
                    Apellido = apellido,
                    Dni = dni,
                    Email = email,
                    Direccion = direccion,
                    Telefono = celular,
                    PasswordHash = password, // Se manda plana, el AuthService la encripta
                    Estado = true,
                    IdRol = idRol,
                    IdSucursal = idSucursal
                };

                bool exito = await _empleadoService.CrearAsync(nuevoEmpleado);

                if (exito)
                {
                    MessageBox.Show("¡Usuario registrado exitosamente!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    OperacionTerminada?.Invoke();
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("El DNI o el Email ya existen en la base de datos.", "Error de duplicidad", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                // ==========================================
                // MODO MODIFICACIÓN
                // ==========================================
                _empleadoEdicion.Nombre = nombre;
                _empleadoEdicion.Apellido = apellido;
                _empleadoEdicion.Dni = dni;
                _empleadoEdicion.Email = email;
                _empleadoEdicion.Direccion = direccion;
                _empleadoEdicion.Telefono = celular;
                _empleadoEdicion.IdRol = idRol;
                _empleadoEdicion.IdSucursal = idSucursal;

                if (!string.IsNullOrWhiteSpace(password))
                {
                    _empleadoEdicion.PasswordHash = password;
                }

                var (exito, mensaje) = await _empleadoService.ActualizarAsync(_empleadoEdicion);
                if (exito)
                {
                    MessageBox.Show("¡Empleado actualizado exitosamente!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    OperacionTerminada?.Invoke();
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(mensaje, "Error al actualizar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCancelar_Click(object? sender, EventArgs e)
        {
            LimpiarFormulario();
            MessageBox.Show("Se limpiaron los campos.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void FormRegistroUsuario_Load_1(object sender, EventArgs e)
        {

        }
    }
}

