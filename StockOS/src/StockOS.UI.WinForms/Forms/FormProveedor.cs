using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using StockOS.Application.Services;
using StockOS.Domain.Entities;

namespace StockOS.UI.WinForms.Forms
{
    public partial class FormProveedor : Form
    {
        private readonly IProveedorService _proveedorService;

        public FormProveedor(IProveedorService proveedorService)
        {
            InitializeComponent();
            _proveedorService = proveedorService;

            txtNombre.MaxLength = 100;
            txtCuit.MaxLength = 30;

            // Restricción de caracteres en CUIT (solo números y guiones)
            txtCuit.KeyPress += (s, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '-')
                {
                    e.Handled = true;
                }
            };

            btnAgregar.Click += BtnAgregar_Click;
            btnDarBaja.Click += BtnDarBaja_Click;
            btnCerrar.Click += (s, e) => this.Close();

            dgvProveedores.SelectionChanged += DgvProveedores_SelectionChanged;
            this.Load += (s, e) => CargarProveedores();
        }

        private void CargarProveedores()
        {
            dgvProveedores.Rows.Clear();
            var proveedores = _proveedorService.ObtenerTodos().ToList();

            foreach (var prov in proveedores)
            {
                int idx = dgvProveedores.Rows.Add(
                    prov.IdProveedor.ToString(),
                    prov.RazonSocial,
                    string.IsNullOrWhiteSpace(prov.Cuit) ? "S/D" : prov.Cuit,
                    prov.Activo ? "Activo" : "Inactivo");

                dgvProveedores.Rows[idx].DefaultCellStyle.ForeColor = prov.Activo
                    ? Color.FromArgb(52, 211, 153)   // verde esmeralda
                    : Color.FromArgb(148, 163, 184); // gris oscuro
            }

            ActualizarBotonDarBaja();
        }

        private void DgvProveedores_SelectionChanged(object? sender, EventArgs e)
        {
            ActualizarBotonDarBaja();
        }

        private void ActualizarBotonDarBaja()
        {
            if (dgvProveedores.CurrentRow == null || dgvProveedores.CurrentRow.Index < 0) return;

            string estadoActual = dgvProveedores.CurrentRow.Cells["colEstado"].Value?.ToString() ?? "";
            bool estaActivo = estadoActual == "Activo";

            if (estaActivo)
            {
                btnDarBaja.Text = "Dar de Baja";
                btnDarBaja.BackColor = Color.FromArgb(239, 68, 68); // Rojo
            }
            else
            {
                btnDarBaja.Text = "Reactivar";
                btnDarBaja.BackColor = Color.FromArgb(16, 185, 129); // Verde
            }
        }

        private void BtnAgregar_Click(object? sender, EventArgs e)
        {
            string nombre = txtNombre.Text.Trim();
            string cuit = txtCuit.Text.Trim();

            if (string.IsNullOrWhiteSpace(nombre))
            {
                MessageBox.Show("Ingrese el nombre o razón social del proveedor.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return;
            }

            var proveedoresExistentes = _proveedorService.ObtenerTodos().ToList();

            // Evitar duplicados por nombre
            bool nombreExiste = proveedoresExistentes
                .Any(p => p.RazonSocial.Equals(nombre, StringComparison.OrdinalIgnoreCase));

            if (nombreExiste)
            {
                MessageBox.Show("Ya existe un proveedor con ese nombre / razón social.", "Duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return;
            }

            // Evitar duplicados por CUIT si se especificó
            if (!string.IsNullOrWhiteSpace(cuit))
            {
                bool cuitExiste = proveedoresExistentes
                    .Any(p => !string.IsNullOrWhiteSpace(p.Cuit) && p.Cuit.Trim().Equals(cuit, StringComparison.OrdinalIgnoreCase));

                if (cuitExiste)
                {
                    MessageBox.Show("Ya existe un proveedor registrado con ese CUIT.", "Duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtCuit.Focus();
                    return;
                }
            }

            var nuevo = new Proveedor
            {
                RazonSocial = nombre,
                Cuit = string.IsNullOrWhiteSpace(cuit) ? null : cuit,
                Telefono = "",
                Email = "",
                Direccion = "",
                Activo = true
            };

            try
            {
                _proveedorService.Agregar(nuevo);

                txtNombre.Clear();
                txtCuit.Clear();
                txtNombre.Focus();

                CargarProveedores();
                MessageBox.Show("Proveedor agregado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar el proveedor: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDarBaja_Click(object? sender, EventArgs e)
        {
            if (dgvProveedores.CurrentRow == null || dgvProveedores.CurrentRow.Index < 0)
            {
                MessageBox.Show("Seleccione un proveedor de la lista.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int idProveedor = Convert.ToInt32(dgvProveedores.CurrentRow.Cells["colId"].Value);
            string nombre = dgvProveedores.CurrentRow.Cells["colNombre"].Value?.ToString() ?? "";
            string estadoActual = dgvProveedores.CurrentRow.Cells["colEstado"].Value?.ToString() ?? "";

            bool estaActivo = estadoActual == "Activo";
            string accion = estaActivo ? "dar de baja" : "reactivar";

            var confirmacion = MessageBox.Show(
                $"¿Desea {accion} el proveedor '{nombre}'?",
                "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmacion != DialogResult.Yes) return;

            try
            {
                _proveedorService.CambiarEstado(idProveedor, !estaActivo);
                CargarProveedores();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cambiar el estado del proveedor: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

