using StockOS.Application.Services;
using StockOS.Domain.Entities;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockOS.UI.WinForms.Forms
{
    /// <summary>
    /// Formulario de autenticación de usuarios con validación de credenciales.
    /// </summary>
    public partial class FormLogin : Form
    {
        private readonly IAuthService _authService;
        private readonly ICajaService _cajaService;
        private bool _ignorandoCambios = false;

        public Empleado? UsuarioAutenticado { get; private set; }

        public FormLogin(IAuthService authService, ICajaService cajaService)
        {
            InitializeComponent();
            _authService = authService;
            _cajaService = cajaService;

            txtUsuario.MaxLength = 20;
            StockOS.UI.WinForms.Helpers.ValidadorUI.ConfigurarSoloNumeros(txtUsuario, "DNI");

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

            if (!StockOS.UI.WinForms.Helpers.ValidadorUI.EsSoloNumeros(dni))
            {
                MostrarError("El DNI debe contener únicamente números.");
                txtUsuario.Focus();
                txtUsuario.SelectAll();
                return;
            }

            btnIngresar.Enabled = false;
            try
            {
                // Llamada al backend real (Base de Datos)
                var (exito, empleado, mensajeError) = await Task.Run(() => _authService.LoginAsync(dni, password));

                if (exito && empleado != null)
                {
                    UsuarioAutenticado = empleado;

                    // Guardamos el usuario en la memoria global
                    SesionActual.Usuario = empleado;

                    // Asegurar que si quedó una sesión huérfana de una ejecución previa se cierre en la base de datos
                    _cajaService.CerrarCajaPorCierreSesion(empleado.IdEmpleado);

                    // Toda nueva sesión inicia con la caja cerrada; el cajero debe abrir su caja explícitamente para su turno
                    SesionActual.IdCajaSesionAbierta = null;

                    MostrarExito("Ingreso exitoso");
                    await Task.Delay(1200);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                    return;
                }
                else
                {
                    MostrarError(mensajeError ?? "Credenciales incorrectas.");
                    _ignorandoCambios = true;
                    txtPassword.SelectAll();
                    txtPassword.Focus();
                    _ignorandoCambios = false;
                }
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                Serilog.Log.Error(ex, "Error de conexión a SQL Server durante el inicio de sesión.");
                MostrarError("Sin conexión a SQL Server (verifique el servicio MSSQLSERVER).");
                MessageBox.Show(
                    "No se pudo conectar a la base de datos SQL Server en 'localhost'.\n\n" +
                    "Causa probable: El servicio de Windows 'SQL Server (MSSQLSERVER)' está detenido.\n\n" +
                    "Solución: Abra PowerShell como Administrador y ejecute:\n" +
                    "Start-Service MSSQLSERVER",
                    "Error de Conexión a Base de Datos",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "Error inesperado durante el inicio de sesión.");
                MostrarError("Error al conectar con el servidor de datos.");
                MessageBox.Show(
                    $"Ocurrió un error al intentar iniciar sesión:\n{ex.Message}",
                    "Error de Inicio de Sesión",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                if (!this.IsDisposed && this.DialogResult != DialogResult.OK)
                {
                    btnIngresar.Enabled = true;
                }
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