using StockOS.Application.Services;
using StockOS.Domain.Entities;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockOS.UI.WinForms.Forms
{
    public partial class FormLogin : Form
    {
        private readonly IAuthService _authService;
        private bool _ignorandoCambios = false;

        public Empleado? UsuarioAutenticado { get; private set; }

        public FormLogin(IAuthService authService)
        {
            InitializeComponent();
            _authService = authService;

            txtUsuario.TextChanged += (s, e) => { if (!_ignorandoCambios) OcultarError(); };
            txtPassword.TextChanged += (s, e) => { if (!_ignorandoCambios) OcultarError(); };
        }

        private void MostrarError(string mensaje)
        {
            lblError.ForeColor = Color.FromArgb(239, 68, 68);
            lblError.Text = mensaje;
            lblError.Refresh();
        }

        private void MostrarExito(string mensaje)
        {
            lblError.ForeColor = Color.FromArgb(16, 185, 129);
            lblError.Text = mensaje;
            lblError.Refresh();
        }

        private void OcultarError()
        {
            lblError.Text = "";
        }

        private async void btnIngresar_Click(object? sender, EventArgs e)
        {
            OcultarError();
            string dni = txtUsuario.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(dni) || string.IsNullOrWhiteSpace(password))
            {
                MostrarError("DNI y contraseña son obligatorios.");
                return;
            }

            // Llamada al backend real (Base de Datos)
            var empleado = await _authService.LoginAsync(dni, password);

            if (empleado != null)
            {
                UsuarioAutenticado = empleado;

                //Guardamo el usuario en la memoria global
                SesionActual.Usuario = empleado;

                MostrarExito("Ingreso exitoso");
                btnIngresar.Enabled = false;
                await Task.Delay(1200);
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

        private void btnRecargar_Click(object? sender, EventArgs e)
        {
            _ignorandoCambios = true;
            txtUsuario.Clear();
            txtPassword.Clear();
            OcultarError();
            _ignorandoCambios = false;
            txtUsuario.Focus();
        }

        private void btnSalir_Click(object? sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }
    }
}