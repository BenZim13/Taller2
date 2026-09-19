using System;
using System.Linq;
using System.Windows.Forms;
using StockOS.Application.Services;
using StockOS.Domain.Entities;

namespace StockOS.UI.WinForms.Forms
{
    public partial class FormCategoria : Form
    {
        private readonly ICategoriaService _categoriaService;

        public FormCategoria(ICategoriaService categoriaService)
        {
            InitializeComponent();
            _categoriaService = categoriaService;

            txtNombre.MaxLength = 50;
            txtDescripcion.MaxLength = 100;

            btnAgregar.Click += BtnAgregar_Click;
            btnDarBaja.Click += BtnDarBaja_Click;
            btnCerrar.Click += (s, e) => this.Close();

            this.Load += (s, e) => CargarCategorias();
        }

        private void CargarCategorias()
        {
            dgvCategorias.Rows.Clear();
            var categorias = _categoriaService.ObtenerTodos().ToList();

            foreach (var cat in categorias)
            {
                int idx = dgvCategorias.Rows.Add(
                    cat.IdCategoria.ToString(),
                    cat.Nombre,
                    cat.Descripcion,
                    cat.Activo ? "Activa" : "Inactiva");

                dgvCategorias.Rows[idx].DefaultCellStyle.ForeColor = cat.Activo
                    ? System.Drawing.Color.FromArgb(52, 211, 153)   // verde
                    : System.Drawing.Color.FromArgb(148, 163, 184); // gris
            }
        }

        private void BtnAgregar_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("Ingrese el nombre de la categoría.", "Campo Requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return;
            }

            // Evitar duplicados
            bool existe = _categoriaService.ObtenerTodos()
                .Any(c => c.Nombre.Equals(txtNombre.Text.Trim(), StringComparison.OrdinalIgnoreCase));

            if (existe)
            {
                MessageBox.Show("Ya existe una categoría registrada con ese nombre.", "Duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return;
            }

            var nueva = new Categoria
            {
                Nombre      = txtNombre.Text.Trim(),
                Descripcion = txtDescripcion.Text.Trim(),
                Activo      = true
            };

            try
            {
                _categoriaService.Agregar(nueva);

                txtNombre.Clear();
                txtDescripcion.Clear();
                txtNombre.Focus();

                CargarCategorias();
                MessageBox.Show("Categoría agregada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar la categoría: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDarBaja_Click(object? sender, EventArgs e)
        {
            if (dgvCategorias.CurrentRow == null || dgvCategorias.CurrentRow.Index < 0)
            {
                MessageBox.Show("Seleccione una categoría de la lista.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int idCategoria = Convert.ToInt32(dgvCategorias.CurrentRow.Cells["colId"].Value);
            string nombre   = dgvCategorias.CurrentRow.Cells["colNombre"].Value?.ToString() ?? "";
            string estadoActual = dgvCategorias.CurrentRow.Cells["colActivo"].Value?.ToString() ?? "";

            bool estaActiva = estadoActual == "Activa";
            string accion   = estaActiva ? "dar de baja" : "reactivar";

            var confirmacion = MessageBox.Show(
                $"¿Desea {accion} la categoría '{nombre}'?",
                "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmacion != DialogResult.Yes) return;

            var categoria = _categoriaService.ObtenerTodos()
                .FirstOrDefault(c => c.IdCategoria == idCategoria);

            if (categoria == null) return;

            categoria.Activo = !estaActiva;
            _categoriaService.Actualizar(categoria);

            CargarCategorias();
        }
    }
}

