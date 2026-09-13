using StockOS.Application.Services;
using StockOS.Domain.Entities;
using System;
using System.Linq;
using System.Windows.Forms;

namespace StockOS.UI.WinForms.Forms
{
    public partial class FormRegistroUsuario : Form
    {
        private readonly IAuthService _authService;
        private readonly ISucursalService _sucursalService; // <-- Nuevo servicio

        // Modificamos el constructor para recibir ambos servicios
        public FormRegistroUsuario(IAuthService authService, ISucursalService sucursalService)
        {
            InitializeComponent();
            _authService = authService;
            _sucursalService = sucursalService;

            btnGuardar.Click += btnGuardar_Click;
            btnCancelar.Click += btnCancelar_Click;
            this.Load += FormRegistroUsuario_Load; // <-- Evento al cargar la ventana
        }

        private void FormRegistroUsuario_Load(object? sender, EventArgs e)
        {
            // Cargamos las sucursales desde la BD
            var sucursales = _sucursalService.ObtenerSucursales().ToList();

            if (sucursales.Any())
            {
                cmbSucursal.DataSource = sucursales;
                cmbSucursal.DisplayMember = "Nombre"; // Lo que el usuario lee
                // NOTA: Si tu entidad Sucursal tiene la clave primaria llamada "IdSucursal", cambia "Id" por "IdSucursal" aquí abajo.
                cmbSucursal.ValueMember = "IdSucursal";       // El valor interno que se guarda
                cmbSucursal.SelectedIndex = 0;
            }
        }

        private async void btnGuardar_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(textApellido.Text) ||
                string.IsNullOrWhiteSpace(txtDNI.Text) ||
                string.IsNullOrWhiteSpace(textEmail.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text) ||
                cmbRol.SelectedIndex == -1 ||
                cmbSucursal.SelectedIndex == -1) // <-- Validamos que elija sucursal
            {
                MessageBox.Show("Todos los campos y selectores son obligatorios.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var nuevoEmpleado = new Empleado
            {
                Nombre = txtNombre.Text.Trim(),
                Apellido = textApellido.Text.Trim(),
                Dni = txtDNI.Text.Trim(),
                Email = textEmail.Text.Trim(),
                Telefono = txtCelular.Text.Trim(),
                PasswordHash = txtPassword.Text,
                Estado = true,
                IdRol = cmbRol.SelectedIndex + 1,

                // Capturamos la ID real de la sucursal seleccionada en el ComboBox
                IdSucursal = cmbSucursal.SelectedValue as int? ?? 1
            };

            bool exito = await _authService.RegistrarAsync(nuevoEmpleado);

            if (exito)
            {
                MessageBox.Show("¡Usuario registrado correctamente!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("El DNI o el Email ya existen en la base de datos.", "Error de duplicidad", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object? sender, EventArgs e)
        {
            this.Close();
        }
    }
}