using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using StockOS.Application.Services;
using StockOS.Domain.Entities;

namespace StockOS.UI.WinForms.Forms
{
    public partial class UcListarUsuarios : UserControl
    {
        private readonly IEmpleadoService _empleadoService;
        private readonly IServiceProvider _serviceProvider;
        private List<Empleado> _listaCompleta = new();

        public event Action<Empleado>? SolicitarEdicionUsuario;
        public event Action? AlVolver;

        public UcListarUsuarios(IEmpleadoService empleadoService, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _empleadoService = empleadoService;
            _serviceProvider = serviceProvider;

            ConfigurarGrid();
            ConfigurarEventos();
        }

        private void ConfigurarGrid()
        {
            dgvUsuarios.Columns.Clear();

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
                FillWeight = 85
            });

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colNombre",
                HeaderText = "Nombre",
                FillWeight = 95
            });

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colApellido",
                HeaderText = "Apellido",
                FillWeight = 95
            });

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colEmail",
                HeaderText = "Correo Electrónico",
                FillWeight = 140
            });

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colTelefono",
                HeaderText = "Celular",
                FillWeight = 90
            });

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colRol",
                HeaderText = "Rol",
                FillWeight = 85
            });

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colSucursal",
                HeaderText = "Sucursal",
                FillWeight = 95
            });

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colEstado",
                HeaderText = "Estado",
                FillWeight = 75
            });
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
                    e.Dni.ToLowerInvariant().Contains(texto) ||
                    e.Nombre.ToLowerInvariant().Contains(texto) ||
                    e.Apellido.ToLowerInvariant().Contains(texto) ||
                    e.Email.ToLowerInvariant().Contains(texto));
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
                MessageBox.Show("Por favor seleccione un usuario de la lista.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }

            int idEmpleado = Convert.ToInt32(dgvUsuarios.CurrentRow.Cells["colId"].Value);
            return _listaCompleta.FirstOrDefault(e => e.IdEmpleado == idEmpleado);
        }

        private void EditarSeleccionado()
        {
            var emp = ObtenerEmpleadoSeleccionado();
            if (emp == null) return;

            if (SolicitarEdicionUsuario != null)
            {
                SolicitarEdicionUsuario.Invoke(emp);
            }
            else
            {
                using var scope = _serviceProvider.CreateScope();
                var formRegistro = scope.ServiceProvider.GetRequiredService<FormRegistroUsuario>();
                formRegistro.PrepararParaEdicion(emp);

                if (formRegistro.ShowDialog() == DialogResult.OK)
                {
                    CargarDatos();
                }
            }
        }

        private async void BtnDarBaja_Click(object? sender, EventArgs e)
        {
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
                bool nuevoEstado = !emp.Estado;
                bool exito = await _empleadoService.CambiarEstadoAsync(emp.IdEmpleado, nuevoEstado);

                if (exito)
                {
                    MessageBox.Show($"Usuario {(nuevoEstado ? "reactivado" : "dado de baja")} correctamente.", "Operación Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarDatos();
                }
                else
                {
                    MessageBox.Show("No se pudo actualizar el estado del usuario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}

