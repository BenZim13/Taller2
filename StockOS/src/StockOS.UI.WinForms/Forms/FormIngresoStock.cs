using System;
using System.Linq;
using System.Windows.Forms;
using StockOS.Application.Services;
using StockOS.Domain.Entities;

namespace StockOS.UI.WinForms.Forms
{
    public partial class FormIngresoStock : Form
    {
        private readonly IProductoService _productoService;
        private readonly IStockService _stockService;
        private readonly ICategoriaService _categoriaService;

        public FormIngresoStock(IProductoService productoService, IStockService stockService, ICategoriaService categoriaService)
        {
            InitializeComponent();
            _productoService = productoService;
            _stockService = stockService;
            _categoriaService = categoriaService;

            dtpFecha.Value = DateTime.Now;

            CargarCategorias();

            // Cálculo reactivo del monto total y precio de venta final
            txtCantidad.TextChanged += CalcularValores;
            txtPrecioCompra.TextChanged += CalcularValores;
            txtPorcentajeExtra.TextChanged += CalcularValores;

            btnGuardar.Click += BtnGuardar_Click;
            btnCancelar.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };
        }

        private void CargarCategorias()
        {
            var categorias = _categoriaService.ObtenerTodos().ToList();
            cmbCategoria.DataSource = categorias;
            cmbCategoria.DisplayMember = "Nombre";
            cmbCategoria.ValueMember = "IdCategoria";
            cmbCategoria.SelectedIndex = -1;
        }

        private void CalcularValores(object? sender, EventArgs e)
        {
            bool hayPrecio = decimal.TryParse(txtPrecioCompra.Text, out decimal precioCompra);
            
            // 1. Calcular Monto Total Compra
            if (hayPrecio && int.TryParse(txtCantidad.Text, out int cantidad))
            {
                txtMontoTotal.Text = (precioCompra * cantidad).ToString("0.00");
            }
            else
            {
                txtMontoTotal.Text = "0.00";
            }

            // 2. Calcular Precio Venta Final
            if (hayPrecio && decimal.TryParse(txtPorcentajeExtra.Text, out decimal porcentaje))
            {
                decimal precioVenta = precioCompra * (1 + (porcentaje / 100m));
                txtPrecioVenta.Text = precioVenta.ToString("0.00");
            }
            else if (hayPrecio)
            {
                txtPrecioVenta.Text = precioCompra.ToString("0.00");
            }
            else
            {
                txtPrecioVenta.Text = "0.00";
            }
        }

        private void BtnGuardar_Click(object? sender, EventArgs e)
        {
            // --- Validaciones ---
            if (string.IsNullOrWhiteSpace(txtCodigo.Text))
            {
                MessageBox.Show("Ingrese el código del producto.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCodigo.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNombreProducto.Text))
            {
                MessageBox.Show("Ingrese el nombre del producto.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombreProducto.Focus();
                return;
            }

            if (cmbCategoria.SelectedValue == null)
            {
                MessageBox.Show("Seleccione una categoría.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbCategoria.Focus();
                return;
            }

            if (!int.TryParse(txtCantidad.Text, out int cantidad) || cantidad <= 0)
            {
                MessageBox.Show("Ingrese una cantidad válida mayor a cero.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtCantidad.Focus();
                return;
            }

            if (!decimal.TryParse(txtPrecioCompra.Text, out decimal precioCompra) || precioCompra <= 0)
            {
                MessageBox.Show("Ingrese un precio de compra válido mayor a cero.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPrecioCompra.Focus();
                return;
            }

            if (!decimal.TryParse(txtPorcentajeExtra.Text, out decimal porcentaje) || porcentaje < 0)
            {
                MessageBox.Show("Ingrese un margen de venta válido (puede ser 0).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPorcentajeExtra.Focus();
                return;
            }

            // Verificar que el código no exista ya en la BD
            bool codigoExiste = _productoService.ObtenerTodos()
                .Any(p => p.CodigoBarra == txtCodigo.Text.Trim());

            if (codigoExiste)
            {
                MessageBox.Show("Ya existe un producto con ese código de barras.", "Código duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCodigo.Focus();
                return;
            }

            // --- Crear el producto nuevo ---
            decimal precioVenta = precioCompra * (1 + (porcentaje / 100m));

            var nuevoProducto = new Producto
            {
                CodigoBarra       = txtCodigo.Text.Trim(),
                Nombre            = txtNombreProducto.Text.Trim(),
                Descripcion       = "", // Previene error de DBNull en EF Core
                IdCategoria       = (int)cmbCategoria.SelectedValue!,
                PrecioVentaActual = precioVenta,
                PorcentajeIva     = 0,
                Activo            = true
            };

            _productoService.Agregar(nuevoProducto);

            // --- Registrar el stock inicial en la sucursal ---
            _stockService.AgregarStock(nuevoProducto.IdProducto, SesionActual.IdSucursal, cantidad);

            MessageBox.Show(
                $"Producto '{nuevoProducto.Nombre}' creado correctamente.\nPrecio de venta: $ {precioVenta:N2}",
                "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnSalirApp_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }
    }
}

