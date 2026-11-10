using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using StockOS.Application.Services;
using StockOS.Domain.Entities;
using StockOS.Domain.Enums;
using StockOS.Domain.Interfaces;

namespace StockOS.UI.WinForms.Forms
{
    public partial class UcListarUsuarios : UserControl
    {
        private readonly IEmpleadoService _empleadoService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IAuthorizationService? _authService;
        private List<Empleado> _listaCompleta = new();

        public event Action<Empleado>? SolicitarEdicionUsuario;
        public event Action? AlVolver;

        public static Action<string, string, MessageBoxButtons, MessageBoxIcon> MostrarMensaje { get; set; } =
            (msg, title, btns, icon) => MessageBox.Show(msg, title, btns, icon);

        public UcListarUsuarios(IEmpleadoService empleadoService, IServiceProvider serviceProvider, IAuthorizationService? authService = null)
        {
            InitializeComponent();
            _empleadoService = empleadoService;
            _serviceProvider = serviceProvider;
            _authService = authService ?? serviceProvider.GetService<IAuthorizationService>();

            ConfigurarGrid();
            ConfigurarEventos();
            ConfigurarPermisosModulo();
        }

        private void ConfigurarPermisosModulo()
        {
            if (_authService == null) return;

            btnNuevo.Visible = _authService.TienePermiso(Permisos.USUARIOS_CREAR);
            bool puedeEditar = _authService.TienePermiso(Permisos.USUARIOS_EDITAR);
            btnEditar.Visible = puedeEditar;
            btnDarBaja.Visible = puedeEditar;
        }

        private void ConfigurarGrid()
        {
            dgvUsuarios.Columns.Clear();

            var paddingUniforme = new Padding(12, 0, 4, 0);
            dgvUsuarios.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvUsuarios.ColumnHeadersDefaultCellStyle.Padding = paddingUniforme;
            dgvUsuarios.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvUsuarios.DefaultCellStyle.Padding = paddingUniforme;

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colId",
                HeaderText = "ID",
                DataPropertyName = "IdEmpleado",
                Visible = false
            });

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colDni",
                HeaderText = "DNI",
                FillWeight = 85,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleLeft, Padding = paddingUniforme }
            });

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colNombre",
                HeaderText = "Nombre",
                FillWeight = 95,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleLeft, Padding = paddingUniforme }
            });

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colApellido",
                HeaderText = "Apellido",
                FillWeight = 95,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleLeft, Padding = paddingUniforme }
            });

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colEmail",
                HeaderText = "Correo Electrónico",
                FillWeight = 140,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleLeft, Padding = paddingUniforme }
            });

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colTelefono",
                HeaderText = "Celular",
                FillWeight = 90,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleLeft, Padding = paddingUniforme }
            });

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colRol",
                HeaderText = "Rol",
                FillWeight = 85,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleLeft, Padding = paddingUniforme }
            });

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colSucursal",
                HeaderText = "Sucursal",
                FillWeight = 95,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleLeft, Padding = paddingUniforme }
            });

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colEstado",
                HeaderText = "Estado",
                FillWeight = 75,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleLeft, Padding = paddingUniforme }
            });

            foreach (DataGridViewColumn col in dgvUsuarios.Columns)
            {
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                col.HeaderCell.Style.Padding = paddingUniforme;
            }
        }

        private void ConfigurarEventos()
        {
            this.Load += (s, e) =>
            {
                cmbFiltroEstado.SelectedIndex = 0; // "Todos"
                CargarDatos();
            };

            txtBuscar.TextChanged += (s, e) => AplicarFiltros();
            cmbFiltroEstado.SelectedIndexChanged += (s, e) => AplicarFiltros();

            btnNuevo.Click += BtnNuevo_Click;
            btnEditar.Click += (s, e) => EditarSeleccionado();
            btnDarBaja.Click += BtnDarBaja_Click;
            btnRecargar.Click += (s, e) => CargarDatos();
            btnVolver.Click += (s, e) => AlVolver?.Invoke();

            dgvUsuarios.CellDoubleClick += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    EditarSeleccionado();
                }
            };
        }

        public void CargarDatos()
        {
            try
            {
                _listaCompleta = _empleadoService.ObtenerTodos().ToList();
                AplicarFiltros();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar la lista de usuarios: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AplicarFiltros()
        {
            string texto = txtBuscar.Text.Trim().ToLowerInvariant();
            string filtroEstado = cmbFiltroEstado.SelectedItem?.ToString() ?? "Todos";

            var filtrados = _listaCompleta.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(texto))
            {
                filtrados = filtrados.Where(e =>
                (e.Dni != null && e.Dni.ToLowerInvariant().Contains(texto)) ||
                (e.Nombre != null && e.Nombre.ToLowerInvariant().Contains(texto)) ||
                (e.Apellido != null && e.Apellido.ToLowerInvariant().Contains(texto)) ||
                (e.Email != null && e.Email.ToLowerInvariant().Contains(texto)));
            }

            if (filtroEstado == "Solo Activos")
            {
                filtrados = filtrados.Where(e => e.Estado);
            }
            else if (filtroEstado == "Solo Inactivos")
            {
                filtrados = filtrados.Where(e => !e.Estado);
            }

            dgvUsuarios.Rows.Clear();

            foreach (var emp in filtrados)
            {
                string rolNombre = emp.IdRolNavigation?.Nombre ?? $"Rol {emp.IdRol}";
                string sucursalNombre = emp.IdSucursalNavigation?.Nombre ?? $"Sucursal {emp.IdSucursal}";
                string estadoTexto = emp.Estado ? "Activo" : "Inactivo";

                int index = dgvUsuarios.Rows.Add(
                    emp.IdEmpleado,
                    emp.Dni,
                    emp.Nombre,
                    emp.Apellido,
                    emp.Email,
                    emp.Telefono,
                    rolNombre,
                    sucursalNombre,
                    estadoTexto
                );

                if (!emp.Estado)
                {
                    dgvUsuarios.Rows[index].DefaultCellStyle.ForeColor = Color.FromArgb(148, 163, 184);
                }
                else
                {
                    dgvUsuarios.Rows[index].Cells["colEstado"].Style.ForeColor = Color.FromArgb(52, 211, 153);
                }
            }
        }

        private Empleado? ObtenerEmpleadoSeleccionado()
        {
            if (dgvUsuarios.CurrentRow == null || dgvUsuarios.CurrentRow.Index < 0)
            {
                MostrarMensaje("Por favor seleccione un usuario de la lista.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }

            int idEmpleado = Convert.ToInt32(dgvUsuarios.CurrentRow.Cells["colId"].Value);
            return _listaCompleta.FirstOrDefault(e => e.IdEmpleado == idEmpleado);
        }

        private void BtnNuevo_Click(object? sender, EventArgs e)
        {
            if (_authService != null && !_authService.TienePermiso(Permisos.USUARIOS_CREAR))
            {
                MostrarMensaje("No tiene permisos para registrar nuevos usuarios.", "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var scope = _serviceProvider.CreateScope();
            var formRegistro = scope.ServiceProvider.GetRequiredService<FormRegistroUsuario>();
            formRegistro.LimpiarFormulario();

            if (formRegistro.ShowDialog() == DialogResult.OK)
            {
                CargarDatos();
            }
        }

        private void EditarSeleccionado()
        {
            if (_authService != null && !_authService.TienePermiso(Permisos.USUARIOS_EDITAR))
            {
                MostrarMensaje("No tiene permisos para modificar usuarios.", "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var emp = ObtenerEmpleadoSeleccionado();
            if (emp == null) return;

            if (SolicitarEdicionUsuario != null)
            {
                SolicitarEdicionUsuario.Invoke(emp);
                return;
            }

            using var scope = _serviceProvider.CreateScope();
            var formRegistro = scope.ServiceProvider.GetRequiredService<FormRegistroUsuario>();
            formRegistro.PrepararParaEdicion(emp);

            if (formRegistro.ShowDialog() == DialogResult.OK)
            {
                CargarDatos();
            }
        }

        private async void BtnDarBaja_Click(object? sender, EventArgs e)
        {
            if (_authService != null && !_authService.TienePermiso(Permisos.USUARIOS_EDITAR))
            {
                MostrarMensaje("No tiene permisos para modificar el estado de usuarios.", "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var emp = ObtenerEmpleadoSeleccionado();
            if (emp == null) return;

            string accion = emp.Estado ? "dar de baja (desactivar)" : "reactivar";
            var confirmacion = MessageBox.Show(
                $"¿Está seguro de que desea {accion} al usuario {emp.Nombre} {emp.Apellido} (DNI: {emp.Dni})?",
                "Confirmación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmacion == DialogResult.Yes)
            {
                try
                {
                    bool nuevoEstado = !emp.Estado;
                    bool exito = await _empleadoService.CambiarEstadoAsync(emp.IdEmpleado, nuevoEstado);

                        if (exito)
                        {
                            MostrarMensaje($"Usuario {(nuevoEstado ? "reactivado" : "dado de baja")} correctamente.", "Operación Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            CargarDatos();
                        }
                        else
                        {
                            MostrarMensaje("No se pudo actualizar el estado del usuario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                catch (Exception ex)
                        {
                            MostrarMensaje($"Ocurrió un error al intentar cambiar el estado: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                }
        }
        }
    }

