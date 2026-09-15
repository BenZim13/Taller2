using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using StockOS.Domain.Entities;

namespace StockOS.UI.WinForms.Forms
{
    public partial class UcUsuarios : UserControl
    {
        private readonly IServiceProvider _serviceProvider;
        private UcListarUsuarios? _vistaListar;

        public UcUsuarios(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;

            btnOpcionListar.Click += (s, e) => MostrarListarUsuarios();
            btnOpcionCargar.Click += (s, e) => MostrarCargarUsuario();

            // Al inicio, mostrar solo las opciones
            pnlOpciones.Visible = true;
            pnlContenedorSubVista.Visible = false;
        }

        public void MostrarListarUsuarios()
        {
            pnlOpciones.Visible = false;
            pnlContenedorSubVista.Visible = true;
            pnlContenedorSubVista.Controls.Clear();

            if (_vistaListar == null)
            {
                _vistaListar = _serviceProvider.GetRequiredService<UcListarUsuarios>();
                _vistaListar.Dock = DockStyle.Fill;
                _vistaListar.SolicitarEdicionUsuario += (emp) => MostrarCargarUsuario(emp);
                _vistaListar.AlVolver += () => MostrarOpciones();
            }

            pnlContenedorSubVista.Controls.Add(_vistaListar);
            _vistaListar.CargarDatos();
        }

        public void MostrarCargarUsuario(Empleado? empleadoAEditar = null)
        {
            pnlOpciones.Visible = false;
            pnlContenedorSubVista.Visible = true;
            pnlContenedorSubVista.Controls.Clear();

            var formRegistro = _serviceProvider.GetRequiredService<FormRegistroUsuario>();
            formRegistro.TopLevel = false;
            formRegistro.FormBorderStyle = FormBorderStyle.None;
            formRegistro.Dock = DockStyle.Fill;
            formRegistro.AutoScroll = true;
            formRegistro.WindowState = FormWindowState.Normal;

            if (empleadoAEditar != null)
            {
                formRegistro.PrepararParaEdicion(empleadoAEditar);
            }
            else
            {
                formRegistro.LimpiarFormulario();
            }

            formRegistro.OperacionTerminada += () =>
            {
                MostrarListarUsuarios();
            };

            formRegistro.AlVolver += () => MostrarOpciones();

            pnlContenedorSubVista.Controls.Add(formRegistro);
            formRegistro.Show();
        }

        private void MostrarOpciones()
        {
            pnlContenedorSubVista.Controls.Clear();
            pnlContenedorSubVista.Visible = false;
            pnlOpciones.Visible = true;
        }
    }
}

