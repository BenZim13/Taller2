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
        private Producto? _productoEncontrado;

        public FormIngresoStock(IProductoService productoService, IStockService stockService)
        {
            InitializeComponent();
            _productoService = productoService;
            _stockService = stockService;

            // Eventos
            txtCodigo.KeyDown += TxtCodigo_KeyDown;
            btnGuardar.Click += BtnGuardar_Click;
            btnCancelar.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };
        }

        private void TxtCodigo_KeyDown(object? sender, KeyEventArgs e)
        {
            // Cuando la lectora de barras presiona ENTER
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Evita el ruidito molesto de Windows
                BuscarProducto(txtCodigo.Text.Trim());
            }
        }

        private void BuscarProducto(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo)) return;

            // Buscamos el producto por su código de barra
            _productoEncontrado = _productoService.ObtenerTodos().FirstOrDefault(p => p.CodigoBarra == codigo);

            if (_productoEncontrado != null)
            {
                lblNombreProducto.Text = $"Producto: {_productoEncontrado.Nombre}";
                txtCantidad.Focus(); // Pasamos el cursor automáticamente a la cantidad
            }
            else
            {
                lblNombreProducto.Text = "Producto no encontrado.";
                _productoEncontrado = null;
            }
        }

        private void BtnGuardar_Click(object? sender, EventArgs e)
        {
            if (_productoEncontrado == null)
            {
                MessageBox.Show("Primero escanee un producto válido.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtCantidad.Text, out int cantidad) || cantidad <= 0)
            {
                MessageBox.Show("Ingrese una cantidad válida mayor a cero.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Ingresamos el stock 
            _stockService.AgregarStock(_productoEncontrado.IdProducto, SesionActual.IdSucursal, cantidad);

            MessageBox.Show("Stock ingresado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}