using StockOS.Application.Services;
using StockOS.Domain.Entities;
using StockOS.Domain.Enums;
using StockOS.Domain.Interfaces;
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
        private readonly IAuthorizationService? _authorizationService;
        private Empleado? _empleadoEdicion;

        public event Action? OperacionTerminada;
        public event Action? AlVolver;

        public static Action<string, string, MessageBoxButtons, MessageBoxIcon> MostrarMensaje { get; set; } =
            (msg, title, btns, icon) => MessageBox.Show(msg, title, btns, icon);

        public FormRegistroUsuario(IEmpleadoService empleadoService, ISucursalService sucursalService,
            IRolService rolService, IAuthService authService, IAuthorizationService? authorizationService = null)
        {
            InitializeComponent();
        _empleadoService = empleadoService;
            _sucursalService = sucursalService;
            _rolService = rolService;
            _authService = authService;
            _authorizationService = authorizationService;

            // Restricciones de longitud según la base de datos
            txtNombre.MaxLength = 50;
            textApellido.MaxLength = 50;
            txtDNI.MaxLength = 20;
            textEmail.MaxLength = 100;
            txtCelular.MaxLength = 30;
            txtDireccion.MaxLength = 200;

            // Restricciones de entrada con advertencia visual inmediata (Globos ToolTip)
            StockOS.UI.WinForms.Helpers.ValidadorUI.ConfigurarSoloLetras(txtNombre, "Nombre");
            StockOS.UI.WinForms.Helpers.ValidadorUI.ConfigurarSoloLetras(textApellido, "Apellido");
            StockOS.UI.WinForms.Helpers.ValidadorUI.ConfigurarSoloNumeros(txtDNI, "DNI");
            StockOS.UI.WinForms.Helpers.ValidadorUI.ConfigurarSoloTelefono(txtCelular, "Celular");

            btnGuardar.Click += btnGuardar_Click;
            btnSalirApp.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close();
    };
    btnVolver.Click += (s, e) => { AlVolver?.Invoke(); this.DialogResult = DialogResult.Cancel; this.Close(); };
    btnCancelar.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };
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
        lblTitulo.Text = "Registro de Usuario";
        this.Text = "Registro de Usuario";
        lblCodigo.Text = "Contraseña (8 dígitos)";
        if (cmbRol.Items.Count > 0) cmbRol.SelectedIndex = 0;
        if (cmbSucursal.Items.Count > 0) cmbSucursal.SelectedIndex = 0;
    }

    private void CargarDatosEdicion()
    {
        if (_empleadoEdicion != null)
        {
            lblTitulo.Text = "Modificar Usuario";
            this.Text = "Modificar Usuario";
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
        if (_authorizationService != null)
        {
            if (_empleadoEdicion != null && !_authorizationService.TienePermiso(Permisos.USUARIOS_EDITAR))
            {
                MostrarMensaje("No tiene permisos para modificar usuarios.", "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.DialogResult = DialogResult.Cancel;
                if (this.IsHandleCreated) this.BeginInvoke(new Action(this.Close));
                return;
            }
            if (_empleadoEdicion == null && !_authorizationService.TienePermiso(Permisos.USUARIOS_CREAR))
            {
                MostrarMensaje("No tiene permisos para registrar nuevos usuarios.", "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.DialogResult = DialogResult.Cancel;
                if (this.IsHandleCreated) this.BeginInvoke(new Action(this.Close));
                return;
            }
        }

        CargarCombos();

        // Si es modo modificación, prellenar campos
        if (_empleadoEdicion != null)
        {
            lblTitulo.Text = "Modificar Usuario";
            this.Text = "Modificar Usuario";
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
            MostrarMensaje("Todos los campos (Nombre, Apellido, DNI, Email, Dirección, Celular, Rol y Sucursal) son obligatorios.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // Validaciones de tipo de dato en el Front-End (evita enviar datos inconsistentes)
        if (!StockOS.UI.WinForms.Helpers.ValidadorUI.EsSoloLetras(nombre))
        {
            MostrarMensaje("El campo 'Nombre' solo debe contener letras (sin números ni símbolos especiales). Por favor, cargue el tipo de dato correcto.", "Tipo de dato incorrecto", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtNombre.Focus();
            txtNombre.SelectAll();
            return;
        }

        if (!StockOS.UI.WinForms.Helpers.ValidadorUI.EsSoloLetras(apellido))
        {
            MostrarMensaje("El campo 'Apellido' solo debe contener letras (sin números ni símbolos especiales). Por favor, cargue el tipo de dato correcto.", "Tipo de dato incorrecto", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            textApellido.Focus();
            textApellido.SelectAll();
            return;
        }

        if (!StockOS.UI.WinForms.Helpers.ValidadorUI.EsSoloNumeros(dni))
        {
            MostrarMensaje("El campo 'DNI' solo debe contener números. Por favor, cargue el tipo de dato correcto.", "Tipo de dato incorrecto", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtDNI.Focus();
            txtDNI.SelectAll();
            return;
        }

        if (!StockOS.UI.WinForms.Helpers.ValidadorUI.EsTelefonoValido(celular))
        {
            MostrarMensaje("El campo 'Celular' solo debe contener números (se permiten espacios, '+' y '-'). Por favor, cargue el tipo de dato correcto.", "Tipo de dato incorrecto", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtCelular.Focus();
            txtCelular.SelectAll();
            return;
        }

        if (!StockOS.UI.WinForms.Helpers.ValidadorUI.EsEmailValido(email))
        {
            MostrarMensaje("El campo 'Email' no posee un formato de correo electrónico válido (ejemplo: usuario@correo.com). Por favor, cargue el tipo de dato correcto.", "Tipo de dato incorrecto", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            textEmail.Focus();
            textEmail.SelectAll();
            return;
        }

        int idRol = (int)cmbRol.SelectedValue;
        int idSucursal = (int)cmbSucursal.SelectedValue;

            try
            {
                    if (_empleadoEdicion == null)
                    {
                // ==========================================
                // MODO ALTA (NUEVO USUARIO)
                // ==========================================
                if (string.IsNullOrWhiteSpace(password))
                {
                    MostrarMensaje("La contraseña es obligatoria para un nuevo usuario.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    PasswordHash = password, // Se manda plana, el EmpleadoService la encripta
                    Estado = true,
                    IdRol = idRol,
                    IdSucursal = idSucursal
                };

                bool exito = await _empleadoService.CrearAsync(nuevoEmpleado);

                    if (exito)
                    {
                        MostrarMensaje("¡Usuario registrado exitosamente!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        OperacionTerminada?.Invoke();
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MostrarMensaje("El DNI o el Email ya existen en la base de datos.", "Error de duplicidad", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
            // ==========================================
            // MODO MODIFICACIÓN (No destructivo en memoria)
            // ==========================================
            var empleadoActualizado = new Empleado
            {
                IdEmpleado = _empleadoEdicion.IdEmpleado,
                Nombre = nombre,
                Apellido = apellido,
                Dni = dni,
                Email = email,
                Direccion = direccion,
                Telefono = celular,
                IdRol = idRol,
                IdSucursal = idSucursal,
                Estado = _empleadoEdicion.Estado,
                PasswordHash = !string.IsNullOrWhiteSpace(password) ? password : _empleadoEdicion.PasswordHash
            };

            var (exito, mensaje) = await _empleadoService.ActualizarAsync(empleadoActualizado);
            if (exito)
            {
                _empleadoEdicion.Nombre = nombre;
                _empleadoEdicion.Apellido = apellido;
                _empleadoEdicion.Dni = dni;
                _empleadoEdicion.Email = email;
                _empleadoEdicion.Direccion = direccion;
                _empleadoEdicion.Telefono = celular;
                _empleadoEdicion.IdRol = idRol;
                _empleadoEdicion.IdSucursal = idSucursal;
                _empleadoEdicion.PasswordHash = empleadoActualizado.PasswordHash;

                    MostrarMensaje("¡Empleado actualizado exitosamente!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    OperacionTerminada?.Invoke();
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MostrarMensaje(mensaje, "Error al actualizar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
            catch (Exception ex)
            {
            MostrarMensaje($"Ocurrió un error al guardar el usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
}

