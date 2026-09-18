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
        private readonly IStockService _stockService;
        private Producto? _productoEdicion; // Si es null, es modo "Nuevo"
        private int _stockOriginal = 0; // Para calcular la diferencia al guardar

        public FormRegistroProducto(IProductoService productoService, ICategoriaService categoriaService, IStockService stockService)
        {
            InitializeComponent();
            _productoService = productoService;
            _categoriaService = categoriaService;
            _stockService = stockService;

            this.Load += FormRegistroProducto_Load;
            btnGuardar.Click += BtnGuardar_Click;
            btnCancelar.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };
        }

        private void FormRegistroProducto_Load(object? sender, EventArgs e)
        {
            CargarCategorias();
            if (_productoEdicion == null)
            {
                cmbEstado.SelectedIndex = 0; // "Activo" por defecto
                txtStock.Text = "0";
            }
        }

        private void CargarCategorias()
        {
            var categorias = _categoriaService.ObtenerTodos().ToList();
            if (categorias.Any())
            {
                cmbCategoria.DataSource = categorias;
                cmbCategoria.DisplayMember = "Nombre";
                cmbCategoria.ValueMember = "IdCategoria";
                cmbCategoria.SelectedIndex = 0;
            }
        }

        public void PrepararParaEdicion(Producto producto)
        {
            _productoEdicion = producto;
            this.Text = "Modificar Producto";

            txtCodigoBarra.Text = producto.CodigoBarra;
            txtNombre.Text = producto.Nombre;
            txtPrecio.Text = producto.PrecioVentaActual.ToString("0.00");

            if (cmbCategoria.DataSource != null)
            {
                cmbCategoria.SelectedValue = producto.IdCategoria;
            }

            cmbEstado.SelectedItem = (producto.Activo == true) ? "Activo" : "Inactivo";

            // Cargar Stock actual
            _stockOriginal = _stockService.ObtenerCantidadActual(producto.IdProducto, SesionActual.IdSucursal);
            txtStock.Text = _stockOriginal.ToString();
        }

        private void BtnGuardar_Click(object? sender, EventArgs e)
        {
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

            if (cmbCategoria.SelectedValue == null || Convert.ToInt32(cmbCategoria.SelectedValue) <= 0)
            {
                MessageBox.Show("Debe seleccionar una categoría de la lista.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtStock.Text, out int stockFinal))
            {
                MessageBox.Show("El stock debe ser un número entero válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            bool esActivo = cmbEstado.SelectedItem?.ToString() == "Activo";

            if (_productoEdicion == null)
            {
                // MODO NUEVO
                var nuevoProducto = new Producto
                {
                    CodigoBarra = txtCodigoBarra.Text.Trim(),
                    Nombre = txtNombre.Text.Trim(),
                    Descripcion = "", // Se quitó la descripción, se guarda vacía
                    PrecioVentaActual = precio,
                    IdCategoria = Convert.ToInt32(cmbCategoria.SelectedValue),
                    Activo = esActivo
                };

                _productoService.Agregar(nuevoProducto);

                if (stockFinal != 0)
                {
                    _stockService.AgregarStock(nuevoProducto.IdProducto, SesionActual.IdSucursal, stockFinal);
                }

                MessageBox.Show("Producto creado exitosamente.");
            }
            else
            {
                // MODO EDICIÓN
                _productoEdicion.CodigoBarra = txtCodigoBarra.Text.Trim();
                _productoEdicion.Nombre = txtNombre.Text.Trim();
                _productoEdicion.Descripcion = ""; // Mantiene vacío
                _productoEdicion.PrecioVentaActual = precio;
                _productoEdicion.IdCategoria = Convert.ToInt32(cmbCategoria.SelectedValue);
                _productoEdicion.Activo = esActivo;

                _productoService.Actualizar(_productoEdicion);

                // Calcular diferencia de stock si se modificó manualmente
                int diferencia = stockFinal - _stockOriginal;
                if (diferencia != 0)
                {
                    _stockService.AgregarStock(_productoEdicion.IdProducto, SesionActual.IdSucursal, diferencia);
                }

                MessageBox.Show("Producto actualizado exitosamente.");
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnSalirApp_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }
    }
}

