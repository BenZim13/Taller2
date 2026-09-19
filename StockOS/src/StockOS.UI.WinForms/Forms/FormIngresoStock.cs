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
        private readonly ICompraService _compraService;
        private readonly IProveedorService _proveedorService;

        public FormIngresoStock(IProductoService productoService, IStockService stockService, 
                               ICategoriaService categoriaService, ICompraService compraService,
                               IProveedorService proveedorService)
        {
            InitializeComponent();
            _productoService = productoService;
            _stockService = stockService;
            _categoriaService = categoriaService;
            _compraService = compraService;
            _proveedorService = proveedorService;

            dtpFecha.Value = DateTime.Now;

            // Restricción de longitud máxima de caracteres
            txtCodigo.MaxLength = 50;
            txtNombreProducto.MaxLength = 100;
            txtCantidad.MaxLength = 8;
            txtPrecioCompra.MaxLength = 12;
            txtPorcentajeExtra.MaxLength = 8;

            CargarCategorias();
            CargarProveedores();

            // Restricciones de entrada por teclado (KeyPress)
            txtCantidad.KeyPress += (s, e) =>
            {
                // Solo dígitos enteros y teclas de control (Backspace)
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            };

            txtPrecioCompra.KeyPress += PermitirSoloDecimales;
            txtPorcentajeExtra.KeyPress += PermitirSoloDecimales;

            // Cálculo reactivo del monto total y precio de venta final
            txtCantidad.TextChanged += CalcularValores;
            txtPrecioCompra.TextChanged += CalcularValores;
            txtPorcentajeExtra.TextChanged += CalcularValores;

            // Búsqueda automática si el código ya existe
            txtCodigo.Leave += (s, e) => BuscarProductoPorCodigo();
            txtCodigo.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; BuscarProductoPorCodigo(); } };

            btnGuardar.Click += BtnGuardar_Click;
            btnCancelar.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };
        }

        private void PermitirSoloDecimales(object? sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar)) return;

            TextBox tb = (TextBox)sender!;
            char decSep = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];

            // Si presiona punto o coma, normalizar al separador del sistema
            if (e.KeyChar == '.' || e.KeyChar == ',')
            {
                if (tb.Text.Contains('.') || tb.Text.Contains(',') || tb.SelectionStart == 0)
                {
                    e.Handled = true;
                    return;
                }
                e.KeyChar = decSep;
                return;
            }

            if (!char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void BuscarProductoPorCodigo()
        {
            string codigo = txtCodigo.Text.Trim();
            if (string.IsNullOrWhiteSpace(codigo)) return;

            var prod = _productoService.ObtenerTodos().FirstOrDefault(p => p.CodigoBarra.Equals(codigo, StringComparison.OrdinalIgnoreCase));
            if (prod != null)
            {
                string nombreActual = txtNombreProducto.Text.Trim();
                if (!string.IsNullOrWhiteSpace(nombreActual) && !prod.Nombre.Equals(nombreActual, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show(
                        $"El código '{codigo}' ya está en uso por el producto '{prod.Nombre}'.\n\nIngrese un código diferente.",
                        "Código en uso",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    txtCodigo.Focus();
                    txtCodigo.SelectAll();
                    return;
                }

                txtNombreProducto.Text = prod.Nombre;
                cmbCategoria.SelectedValue = prod.IdCategoria;

                if (prod.IdProveedor.HasValue)
                {
                    cmbProveedor.SelectedValue = prod.IdProveedor.Value;
                }

                decimal? ultimoPrecio = _compraService.ObtenerUltimoPrecioCompra(prod.IdProducto);
                if (ultimoPrecio.HasValue && ultimoPrecio.Value > 0)
                {
                    txtPrecioCompra.Text = ultimoPrecio.Value.ToString("0.00");
                }
            }
        }

        private void CargarCategorias()
        {
            var categorias = _categoriaService.ObtenerTodos().ToList();
            cmbCategoria.DataSource = categorias;
            cmbCategoria.DisplayMember = "Nombre";
            cmbCategoria.ValueMember = "IdCategoria";
            cmbCategoria.SelectedIndex = -1;
        }

        private void CargarProveedores()
        {
            var proveedores = _proveedorService.ObtenerTodos().Where(p => p.Activo).ToList();
            cmbProveedor.DataSource = proveedores;
            cmbProveedor.DisplayMember = "RazonSocial";
            cmbProveedor.ValueMember = "IdProveedor";
            cmbProveedor.SelectedIndex = proveedores.Any() ? 0 : -1;
        }

        private void CalcularValores(object? sender, EventArgs e)
        {
            bool hayPrecio = decimal.TryParse(txtPrecioCompra.Text, out decimal precioCompra) && precioCompra > 0;
            bool hayCantidad = int.TryParse(txtCantidad.Text, out int cantidad) && cantidad > 0;
            
            // 1. Calcular Monto Total Compra
            if (hayPrecio && hayCantidad)
            {
                txtMontoTotal.Text = (precioCompra * cantidad).ToString("0.00");
            }
            else
            {
                txtMontoTotal.Text = "0.00";
            }

            // 2. Calcular Precio Venta Final
            if (hayPrecio && decimal.TryParse(txtPorcentajeExtra.Text, out decimal porcentaje) && porcentaje >= 0)
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
            // --- Validaciones Exhaustivas de Entrada ---
            string codigo = txtCodigo.Text.Trim();
            if (string.IsNullOrWhiteSpace(codigo))
            {
                MessageBox.Show("Ingrese el código del producto.", "Campo Requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCodigo.Focus();
                return;
            }

            string nombre = txtNombreProducto.Text.Trim();
            if (string.IsNullOrWhiteSpace(nombre))
            {
                MessageBox.Show("Ingrese el nombre del producto.", "Campo Requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombreProducto.Focus();
                return;
            }

            if (cmbCategoria.SelectedValue == null || Convert.ToInt32(cmbCategoria.SelectedValue) <= 0)
            {
                MessageBox.Show("Seleccione una categoría válida para el producto.", "Campo Requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbCategoria.Focus();
                return;
            }

            if (cmbProveedor.SelectedValue == null || Convert.ToInt32(cmbProveedor.SelectedValue) <= 0)
            {
                MessageBox.Show("Debe seleccionar un proveedor de la lista. Si no hay proveedores disponibles, primero debe dar de alta al menos uno desde la sección 'Proveedores'.", "Campo Requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbProveedor.Focus();
                return;
            }

            if (!int.TryParse(txtCantidad.Text, out int cantidad) || cantidad <= 0)
            {
                MessageBox.Show("La cantidad a ingresar debe ser un número entero mayor a cero (mínimo 1 unidad).", "Dato Inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCantidad.Focus();
                return;
            }

            if (cantidad > 1000000)
            {
                MessageBox.Show("La cantidad a ingresar supera el límite permitido (máx. 1.000.000 unidades por ingreso).", "Límite Superado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCantidad.Focus();
                return;
            }

            if (!decimal.TryParse(txtPrecioCompra.Text, out decimal precioCompra) || precioCompra <= 0)
            {
                MessageBox.Show("El precio unitario de compra debe ser un número mayor a cero.", "Dato Inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrecioCompra.Focus();
                return;
            }

            if (precioCompra > 100000000m)
            {
                MessageBox.Show("El precio de compra supera el valor monetario máximo permitido.", "Límite Superado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrecioCompra.Focus();
                return;
            }

            if (!decimal.TryParse(txtPorcentajeExtra.Text, out decimal porcentaje) || porcentaje < 0)
            {
                MessageBox.Show("El margen de ganancia debe ser un porcentaje válido mayor o igual a cero.", "Dato Inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPorcentajeExtra.Focus();
                return;
            }

            if (porcentaje > 10000m)
            {
                MessageBox.Show("El margen de ganancia no puede superar el 10.000%.", "Límite Superado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPorcentajeExtra.Focus();
                return;
            }

            // Detección de producto existente por código y bloqueo si el código ya está en uso por otro producto
            var prodExistente = _productoService.ObtenerTodos()
                .FirstOrDefault(p => p.CodigoBarra.Equals(codigo, StringComparison.OrdinalIgnoreCase));

            if (prodExistente != null)
            {
                // Si el nombre ingresado difiere del producto existente con ese código, rechazar el ingreso
                if (!prodExistente.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show(
                        $"El código '{codigo}' ya está en uso por el producto '{prodExistente.Nombre}'.\n\nNo se permite ingresar otro producto con un código ya utilizado. Ingrese un código diferente.",
                        "Código en uso",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    txtCodigo.Focus();
                    return;
                }
            }

            int idProveedor = Convert.ToInt32(cmbProveedor.SelectedValue);
            decimal precioVenta = precioCompra * (1 + (porcentaje / 100m));
            int idProducto;

            try
            {
                if (prodExistente != null)
                {
                    idProducto = prodExistente.IdProducto;
                    prodExistente.Nombre = txtNombreProducto.Text.Trim();
                    prodExistente.IdCategoria = (int)cmbCategoria.SelectedValue!;
                    prodExistente.IdProveedor = idProveedor;
                    prodExistente.PrecioVentaActual = precioVenta;
                    _productoService.Actualizar(prodExistente);
                }
                else
                {
                    // --- Crear el producto nuevo ---
                    var nuevoProducto = new Producto
                    {
                        CodigoBarra       = codigo,
                        Nombre            = txtNombreProducto.Text.Trim(),
                        Descripcion       = "", // Previene error de DBNull en EF Core
                        IdCategoria       = (int)cmbCategoria.SelectedValue!,
                        IdProveedor       = idProveedor,
                        PrecioVentaActual = precioVenta,
                        PorcentajeIva     = 0,
                        Activo            = true
                    };

                    _productoService.Agregar(nuevoProducto);
                    idProducto = nuevoProducto.IdProducto;
                }

                int idSucursalActual = SesionActual.IdSucursal > 0 ? SesionActual.IdSucursal : 1;
                int idEmpleadoActual = SesionActual.Usuario?.IdEmpleado ?? 1;

                // 1. Registrar el stock en la sucursal
                _stockService.AgregarStock(idProducto, idSucursalActual, cantidad);

                // 2. Registrar la Compra y Detalle de Compra en la base de datos
                decimal montoTotal = precioCompra * cantidad;
                var nuevaCompra = new Compra
                {
                    FechaHora = dtpFecha.Value,
                    Total = montoTotal,
                    NumeroComprobante = null,
                    IdProveedor = idProveedor,
                    IdEmpleado = idEmpleadoActual,
                    IdSucursal = idSucursalActual
                };

                var nuevoDetalle = new DetalleCompra
                {
                    IdProducto = idProducto,
                    Cantidad = cantidad,
                    PrecioUnitarioCompra = precioCompra
                };

                _compraService.RegistrarCompra(nuevaCompra, nuevoDetalle);

                MessageBox.Show(
                    $"Ingreso de stock y compra registrados exitosamente.\nProducto: '{txtNombreProducto.Text.Trim()}'\nCantidad ingresada: {cantidad}\nTotal Compra: $ {montoTotal:N2}\nPrecio de Venta: $ {precioVenta:N2}",
                    "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al registrar el ingreso de stock: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSalirApp_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}

