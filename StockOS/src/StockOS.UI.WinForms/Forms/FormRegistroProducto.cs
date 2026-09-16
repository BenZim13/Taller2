using System;
using System.Linq;
using System.Windows.Forms;
using StockOS.Application.Services;
using StockOS.Domain.Entities;

namespace StockOS.UI.WinForms.Forms
{
    public partial class FormRegistroProducto : Form
    {
        private readonly IProductoService _productoService;
        private readonly ICategoriaService _categoriaService;
        private Producto? _productoEdicion; // Si es null, es modo "Nuevo"

        public FormRegistroProducto(IProductoService productoService, ICategoriaService categoriaService)
        {
            InitializeComponent();
            _productoService = productoService;
            _categoriaService = categoriaService;

            this.Load += FormRegistroProducto_Load;
            btnGuardar.Click += BtnGuardar_Click;
            btnCancelar.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };
        }

        private void FormRegistroProducto_Load(object? sender, EventArgs e)
        {
            CargarCategorias();
        }

        private void CargarCategorias()
        {
            // Traemos las categorías de la BD para el ComboBox
            var categorias = _categoriaService.ObtenerTodos().ToList();
            if (categorias.Any())
            {
                cmbCategoria.DataSource = categorias;
                cmbCategoria.DisplayMember = "Nombre";
                cmbCategoria.ValueMember = "IdCategoria";
                cmbCategoria.SelectedIndex = 0;
            }
        }

        // Este método lo llamaremos desde UcInventario cuando queramos editar
        public void PrepararParaEdicion(Producto producto)
        {
            _productoEdicion = producto;
            this.Text = "Modificar Producto";

            txtCodigoBarra.Text = producto.CodigoBarra;
            txtNombre.Text = producto.Nombre;
            txtDescripcion.Text = producto.Descripcion;
            txtPrecio.Text = producto.PrecioVentaActual.ToString();

            if (cmbCategoria.DataSource != null)
            {
                cmbCategoria.SelectedValue = producto.IdCategoria;
            }
        }

        private void BtnGuardar_Click(object? sender, EventArgs e)
        {
            // 1. Validaciones básicas
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtPrecio.Text))
            {
                MessageBox.Show("El nombre y el precio son obligatorios.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtPrecio.Text, out decimal precio))
            {
                MessageBox.Show("El precio debe ser un número válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 2. Si _productoEdicion es NULL, estamos creando uno nuevo
            if (_productoEdicion == null)
            {
                var nuevoProducto = new Producto
                {
                    CodigoBarra = txtCodigoBarra.Text.Trim(),
                    Nombre = txtNombre.Text.Trim(),
                    Descripcion = txtDescripcion.Text.Trim(),
                    PrecioVentaActual = precio,
                    IdCategoria = Convert.ToInt32(cmbCategoria.SelectedValue),
                    Activo = true
                };

                // Guardar en la BD (Asumiendo que tienes un método Agregar en tu servicio)
                _productoService.Agregar(nuevoProducto);
                MessageBox.Show("Producto creado exitosamente.");
            }
            else
            {
                // 3. Si no es NULL, estamos modificando
                _productoEdicion.CodigoBarra = txtCodigoBarra.Text.Trim();
                _productoEdicion.Nombre = txtNombre.Text.Trim();
                _productoEdicion.Descripcion = txtDescripcion.Text.Trim();
                _productoEdicion.PrecioVentaActual = precio;
                _productoEdicion.IdCategoria = Convert.ToInt32(cmbCategoria.SelectedValue);

                // Actualizar en BD
                _productoService.Actualizar(_productoEdicion);
                MessageBox.Show("Producto actualizado exitosamente.");
            }

            // Cerramos la ventana indicando que todo salió OK
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}