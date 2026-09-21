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

        // Delegado para mostrar mensajes (permite interceptar en pruebas automatizadas sin bloquear la UI)
        public static Action<string, string, MessageBoxButtons, MessageBoxIcon> MostrarMensaje { get; set; } =
            (msg, title, btns, icon) => MessageBox.Show(msg, title, btns, icon);

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
            string codigoBuscar = txtCodigo.Text.Trim();
            if (string.IsNullOrWhiteSpace(codigoBuscar)) return;

            var producto = _productoService.BuscarPorCodigoBarra(codigoBuscar);
            if (producto != null)
            {
                string nombreActual = txtNombreProducto.Text.Trim();
                if (!string.IsNullOrWhiteSpace(nombreActual) && !producto.Nombre.Equals(nombreActual, StringComparison.OrdinalIgnoreCase))
                {
                    MostrarMensaje(
                        $"El código '{codigoBuscar}' ya está en uso por el producto '{producto.Nombre}'.\n\nIngrese un código diferente.",
                        "Código en uso",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    txtCodigo.Focus();
                    txtCodigo.SelectAll();
                    return;
                }

                txtNombreProducto.Text = producto.Nombre;
                cmbCategoria.SelectedValue = producto.IdCategoria;

                if (producto.IdProveedor.HasValue)
                {
                    cmbProveedor.SelectedValue = producto.IdProveedor.Value;
                }

                decimal? ultimoPrecio = _compraService.ObtenerUltimoPrecioCompra(producto.IdProducto);
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

            // 2. Calcular Precio Venta Final: Costo * (1 + (Margen + 21% IVA) / 100)
            if (hayPrecio && decimal.TryParse(txtPorcentajeExtra.Text, out decimal porcentaje) && porcentaje >= 0)
            {
                decimal precioVenta = Producto.CalcularPrecioVentaFinal(precioCompra, porcentaje, Producto.IvaFijoDefault);
                txtPrecioVenta.Text = precioVenta.ToString("0.00");
            }
            else if (hayPrecio)
            {
                decimal precioVenta = Producto.CalcularPrecioVentaFinal(precioCompra, 0m, Producto.IvaFijoDefault);
                txtPrecioVenta.Text = precioVenta.ToString("0.00");
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
                MostrarMensaje("Ingrese el código del producto.", "Campo Requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCodigo.Focus();
                return;
            }

            string nombre = txtNombreProducto.Text.Trim();
            if (string.IsNullOrWhiteSpace(nombre))
            {
                MostrarMensaje("Ingrese el nombre del producto.", "Campo Requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombreProducto.Focus();
                return;
            }

            if (cmbCategoria.SelectedValue == null || Convert.ToInt32(cmbCategoria.SelectedValue) <= 0)
            {
                MostrarMensaje("Seleccione una categoría válida para el producto.", "Campo Requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbCategoria.Focus();
                return;
            }

            if (cmbProveedor.SelectedValue == null || Convert.ToInt32(cmbProveedor.SelectedValue) <= 0)
            {
                MostrarMensaje("Debe seleccionar un proveedor de la lista. Si no hay proveedores disponibles, primero debe dar de alta al menos uno desde la sección 'Proveedores'.", "Campo Requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbProveedor.Focus();
                return;
            }

            if (!int.TryParse(txtCantidad.Text, out int cantidad) || cantidad <= 0)
            {
                MostrarMensaje("La cantidad a ingresar debe ser un número entero mayor a cero (mínimo 1 unidad).", "Dato Inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCantidad.Focus();
                return;
            }

            if (cantidad > 1000000)
            {
                MostrarMensaje("La cantidad a ingresar supera el límite permitido (máx. 1.000.000 unidades por ingreso).", "Límite Superado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCantidad.Focus();
                return;
            }

            if (!decimal.TryParse(txtPrecioCompra.Text, out decimal precioCompra) || precioCompra <= 0)
            {
                MostrarMensaje("El precio unitario de compra debe ser un número mayor a cero.", "Dato Inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrecioCompra.Focus();
                return;
            }

            if (precioCompra > 100000000m)
            {
                MostrarMensaje("El precio de compra supera el valor monetario máximo permitido.", "Límite Superado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrecioCompra.Focus();
                return;
            }

            if (!decimal.TryParse(txtPorcentajeExtra.Text, out decimal porcentaje) || porcentaje < 0)
            {
                MostrarMensaje("El margen de ganancia debe ser un porcentaje válido mayor o igual a cero.", "Dato Inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPorcentajeExtra.Focus();
                return;
            }

            if (porcentaje > 10000m)
            {
                MostrarMensaje("El margen de ganancia no puede superar el 10.000%.", "Límite Superado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    MostrarMensaje(
                        $"El código '{codigo}' ya está en uso por el producto '{prodExistente.Nombre}'.\n\nNo se permite ingresar otro producto con un código ya utilizado. Ingrese un código diferente.",
                        "Código en uso",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    txtCodigo.Focus();
                    return;
                }
            }

            int idProveedor = Convert.ToInt32(cmbProveedor.SelectedValue);
            decimal precioVenta = Producto.CalcularPrecioVentaFinal(precioCompra, porcentaje, Producto.IvaFijoDefault);
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
                    prodExistente.PorcentajeIva = Producto.IvaFijoDefault;
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
                        PorcentajeIva     = Producto.IvaFijoDefault,
                        Activo            = true
                    };

                    _productoService.Agregar(nuevoProducto);
                    idProducto = nuevoProducto.IdProducto;
                }

                int idSucursalActual = SesionActual.IdSucursal > 0 ? SesionActual.IdSucursal : 1;
                int idEmpleadoActual = SesionActual.Usuario?.IdEmpleado ?? 1;

                // Registrar la Compra, Detalle de Compra y el incremento de Stock de forma atómica
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

                MostrarMensaje(
                    $"Ingreso de stock y compra registrados exitosamente.\nProducto: '{txtNombreProducto.Text.Trim()}'\nCantidad ingresada: {cantidad}\nTotal Compra: $ {montoTotal:N2}\nPrecio de Venta: $ {precioVenta:N2}",
                    "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error al registrar el ingreso de stock: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSalirApp_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}

