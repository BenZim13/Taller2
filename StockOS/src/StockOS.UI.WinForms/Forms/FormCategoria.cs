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

        // Variable clave: Nos indica si estamos creando o editando
        private int? _idCategoriaEnEdicion = null;

        public FormCategoria(ICategoriaService categoriaService)
        {
            InitializeComponent();
            _categoriaService = categoriaService;

            txtNombre.MaxLength = 50;
            txtDescripcion.MaxLength = 100;

            btnAgregar.Click += BtnGuardar_Click;
            btnEditar.Click += BtnEditar_Click;
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

        private void BtnEditar_Click(object? sender, EventArgs e)
        {
            if (dgvCategorias.CurrentRow == null || dgvCategorias.CurrentRow.Index < 0)
            {
                MessageBox.Show("Seleccione una categoría de la lista para editar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Cargamos los datos de la grilla a los TextBox
            _idCategoriaEnEdicion = Convert.ToInt32(dgvCategorias.CurrentRow.Cells["colId"].Value);
            txtNombre.Text = dgvCategorias.CurrentRow.Cells["colNombre"].Value?.ToString();
            txtDescripcion.Text = dgvCategorias.CurrentRow.Cells["colDescripcion"].Value?.ToString();

            // Cambiamos el estilo del botón para que sea obvio que estamos editando
            btnAgregar.Text = "Guardar Cambios";
            btnAgregar.BackColor = System.Drawing.Color.FromArgb(245, 158, 11); // Naranja
            txtNombre.Focus();
        }

        private void BtnGuardar_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("Ingrese el nombre de la categoría.", "Campo Requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return;
            }

            try
            {
                if (_idCategoriaEnEdicion == null)
                {
                    /* =========================
                             MODO: ALTA
                       ========================= */
                    bool existe = _categoriaService.ObtenerTodos()
                        .Any(c => c.Nombre.Equals(txtNombre.Text.Trim(), StringComparison.OrdinalIgnoreCase));

                    if (existe)
                    {
                        MessageBox.Show("Ya existe una categoría registrada con ese nombre.", "Duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var nueva = new Categoria
                    {
                        Nombre = txtNombre.Text.Trim(),
                        Descripcion = txtDescripcion.Text.Trim(),
                        Activo = true
                    };

                    _categoriaService.Agregar(nueva);
                    MessageBox.Show("Categoría agregada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    /* =========================
                             MODO: EDICIÓN
                       ========================= */
                    bool existeOtro = _categoriaService.ObtenerTodos()
                        .Any(c => c.IdCategoria != _idCategoriaEnEdicion && c.Nombre.Equals(txtNombre.Text.Trim(), StringComparison.OrdinalIgnoreCase));

                    if (existeOtro)
                    {
                        MessageBox.Show("Ya existe otra categoría registrada con ese nombre.", "Duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Recuperamos la categoría original para no perder su estado Activo/Inactivo
                    var categoriaExistente = _categoriaService.ObtenerTodos().FirstOrDefault(c => c.IdCategoria == _idCategoriaEnEdicion);
                    if (categoriaExistente != null)
                    {
                        categoriaExistente.Nombre = txtNombre.Text.Trim();
                        categoriaExistente.Descripcion = txtDescripcion.Text.Trim();

                        _categoriaService.Actualizar(categoriaExistente);
                        MessageBox.Show("Categoría actualizada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                // Restauramos el formulario al estado original
                ResetearFormulario();
                CargarCategorias();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar la categoría: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetearFormulario()
        {
            _idCategoriaEnEdicion = null;
            txtNombre.Clear();
            txtDescripcion.Clear();
            btnAgregar.Text = "+ Agregar";
            btnAgregar.BackColor = System.Drawing.Color.FromArgb(16, 185, 129); 
            txtNombre.Focus();
        }

        private void BtnDarBaja_Click(object? sender, EventArgs e)
        {
            if (dgvCategorias.CurrentRow == null || dgvCategorias.CurrentRow.Index < 0)
            {
                MessageBox.Show("Seleccione una categoría de la lista.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int idCategoria = Convert.ToInt32(dgvCategorias.CurrentRow.Cells["colId"].Value);
            string nombre = dgvCategorias.CurrentRow.Cells["colNombre"].Value?.ToString() ?? "";
            string estadoActual = dgvCategorias.CurrentRow.Cells["colActivo"].Value?.ToString() ?? "";

            bool estaActiva = estadoActual == "Activa";
            string accion = estaActiva ? "dar de baja" : "reactivar";

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
            ResetearFormulario(); // Por si estaba editando la misma categoría que dio de baja
        }
    }
}