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
        private readonly ICompraService _compraService;
        private readonly IProveedorService _proveedorService;
        private Producto? _productoEdicion; // Si es null, es modo "Nuevo"
        private int _stockOriginal = 0; // Para calcular la diferencia al guardar

        public FormRegistroProducto(IProductoService productoService, ICategoriaService categoriaService, 
                                   IStockService stockService, ICompraService compraService,
                                   IProveedorService proveedorService)
        {
            InitializeComponent();
            _productoService = productoService;
            _categoriaService = categoriaService;
            _stockService = stockService;
            _compraService = compraService;
            _proveedorService = proveedorService;

            // Restricción de longitud de caracteres
            txtCodigoBarra.MaxLength = 50;
            txtNombre.MaxLength = 100;
            txtPrecio.MaxLength = 12;
            txtStock.MaxLength = 8;
            txtPrecioCompra.MaxLength = 12;

            // Restricciones de entrada por teclado (KeyPress)
            txtStock.KeyPress += (s, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            };

            txtPrecio.KeyPress += PermitirSoloDecimales;
            txtPrecioCompra.KeyPress += PermitirSoloDecimales;

            this.Load += FormRegistroProducto_Load;
            btnGuardar.Click += BtnGuardar_Click;
            btnCancelar.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };
        }

        private void PermitirSoloDecimales(object? sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar)) return;

            TextBox tb = (TextBox)sender!;
            char decSep = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];

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

        private void FormRegistroProducto_Load(object? sender, EventArgs e)
        {
            CargarCategorias();
            CargarProveedores();
            if (_productoEdicion == null)
            {
                cmbEstado.SelectedIndex = 0; // "Activo" por defecto
                txtStock.Text = "0";
                txtPrecioCompra.Text = "0.00";
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

        private void CargarProveedores()
        {
            var proveedores = _proveedorService.ObtenerTodos().Where(p => p.Activo).ToList();
            if (proveedores.Any())
            {
                cmbProveedor.DataSource = proveedores;
                cmbProveedor.DisplayMember = "RazonSocial";
                cmbProveedor.ValueMember = "IdProveedor";
                cmbProveedor.SelectedIndex = 0;
            }
            else
            {
                cmbProveedor.DataSource = null;
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

            if (cmbProveedor.DataSource != null && producto.IdProveedor.HasValue)
            {
                cmbProveedor.SelectedValue = producto.IdProveedor.Value;
            }

            cmbEstado.SelectedItem = (producto.Activo == true) ? "Activo" : "Inactivo";

            // Cargar Stock actual
            _stockOriginal = _stockService.ObtenerCantidadActual(producto.IdProducto, SesionActual.IdSucursal);
            txtStock.Text = _stockOriginal.ToString();

            // Cargar Precio de Compra desde la base de datos (tabla detalle_compra)
            decimal? ultimoPrecioCompra = _compraService.ObtenerUltimoPrecioCompra(producto.IdProducto);
            txtPrecioCompra.Text = ultimoPrecioCompra.HasValue ? ultimoPrecioCompra.Value.ToString("0.00") : "0.00";
        }

        private void BtnGuardar_Click(object? sender, EventArgs e)
        {
            // --- Validaciones de Entrada ---
            string codigo = txtCodigoBarra.Text.Trim();
            if (string.IsNullOrWhiteSpace(codigo))
            {
                MessageBox.Show("El código de barra del producto es obligatorio.", "Campo Requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCodigoBarra.Focus();
                return;
            }

            // Validación de unicidad de código de barra
            var todosProductos = _productoService.ObtenerTodos().ToList();
            if (_productoEdicion == null)
            {
                var dup = todosProductos.FirstOrDefault(p => p.CodigoBarra.Equals(codigo, StringComparison.OrdinalIgnoreCase));
                if (dup != null)
                {
                    MessageBox.Show(
                        $"El código de barra '{codigo}' ya está registrado para el producto '{dup.Nombre}'.\n\nCada producto debe contar con un código único.",
                        "Código Duplicado",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    txtCodigoBarra.Focus();
                    return;
                }
            }
            else
            {
                var dup = todosProductos.FirstOrDefault(p => p.IdProducto != _productoEdicion.IdProducto && 
                                                             p.CodigoBarra.Equals(codigo, StringComparison.OrdinalIgnoreCase));
                if (dup != null)
                {
                    MessageBox.Show(
                        $"El código de barra '{codigo}' ya está siendo utilizado por otro producto ('{dup.Nombre}').\n\nIngrese un código diferente.",
                        "Código Duplicado",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    txtCodigoBarra.Focus();
                    return;
                }
            }

            string nombre = txtNombre.Text.Trim();
            if (string.IsNullOrWhiteSpace(nombre))
            {
                MessageBox.Show("El nombre del producto es obligatorio.", "Campo Requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return;
            }

            if (!decimal.TryParse(txtPrecio.Text, out decimal precio) || precio <= 0)
            {
                MessageBox.Show("El precio de venta debe ser un número válido mayor a cero.", "Dato Inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrecio.Focus();
                return;
            }

            if (precio > 100000000m)
            {
                MessageBox.Show("El precio de venta supera el límite máximo permitido.", "Límite Superado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrecio.Focus();
                return;
            }

            if (cmbCategoria.SelectedValue == null || Convert.ToInt32(cmbCategoria.SelectedValue) <= 0)
            {
                MessageBox.Show("Debe seleccionar una categoría válida de la lista.", "Campo Requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbCategoria.Focus();
                return;
            }

            if (cmbProveedor.SelectedValue == null || Convert.ToInt32(cmbProveedor.SelectedValue) <= 0)
            {
                MessageBox.Show("Debe seleccionar un proveedor de la lista. Si no hay proveedores disponibles, primero debe dar de alta al menos uno desde la sección 'Proveedores'.", "Campo Requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbProveedor.Focus();
                return;
            }

            int idProveedor = Convert.ToInt32(cmbProveedor.SelectedValue);

            if (!int.TryParse(txtStock.Text, out int stockFinal) || stockFinal < 0)
            {
                MessageBox.Show("El stock debe ser un número entero mayor o igual a cero.", "Dato Inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtStock.Focus();
                return;
            }

            if (stockFinal > 1000000)
            {
                MessageBox.Show("El stock ingresado supera el límite permitido (máx. 1.000.000 unidades).", "Límite Superado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtStock.Focus();
                return;
            }

            bool esActivo = cmbEstado.SelectedItem?.ToString() == "Activo";

            decimal precioCompra = 0;
            if (!string.IsNullOrWhiteSpace(txtPrecioCompra.Text))
            {
                if (!decimal.TryParse(txtPrecioCompra.Text, out precioCompra) || precioCompra < 0)
                {
                    MessageBox.Show("El precio de compra debe ser un número mayor o igual a cero.", "Dato Inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPrecioCompra.Focus();
                    return;
                }
            }

            int idSucursalActual = SesionActual.IdSucursal > 0 ? SesionActual.IdSucursal : 1;
            int idEmpleadoActual = SesionActual.Usuario?.IdEmpleado ?? 1;

            try
            {
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
                        IdProveedor = idProveedor,
                        Activo = esActivo
                    };

                    _productoService.Agregar(nuevoProducto);

                    if (stockFinal != 0)
                    {
                        _stockService.AgregarStock(nuevoProducto.IdProducto, idSucursalActual, stockFinal);
                    }

                    // Si se especificó precio de compra, guardamos el registro en compra / detalle_compra
                    if (precioCompra > 0)
                    {
                        int cantCompra = stockFinal > 0 ? stockFinal : 1;
                        var nuevaCompra = new Compra
                        {
                            FechaHora = DateTime.Now,
                            Total = precioCompra * cantCompra,
                            NumeroComprobante = null,
                            IdProveedor = idProveedor,
                            IdEmpleado = idEmpleadoActual,
                            IdSucursal = idSucursalActual
                        };

                        var nuevoDetalle = new DetalleCompra
                        {
                            IdProducto = nuevoProducto.IdProducto,
                            Cantidad = cantCompra,
                            PrecioUnitarioCompra = precioCompra
                        };

                        _compraService.RegistrarCompra(nuevaCompra, nuevoDetalle);
                    }

                    MessageBox.Show("Producto creado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // MODO EDICIÓN
                    _productoEdicion.CodigoBarra = txtCodigoBarra.Text.Trim();
                    _productoEdicion.Nombre = txtNombre.Text.Trim();
                    _productoEdicion.Descripcion = ""; // Mantiene vacío
                    _productoEdicion.PrecioVentaActual = precio;
                    _productoEdicion.IdCategoria = Convert.ToInt32(cmbCategoria.SelectedValue);
                    _productoEdicion.IdProveedor = idProveedor;
                    _productoEdicion.Activo = esActivo;

                    _productoService.Actualizar(_productoEdicion);
                    // Garantizamos el cambio de estado en la BD
                    _productoService.CambiarEstado(_productoEdicion.IdProducto, esActivo);

                    // Calcular diferencia de stock si se modificó manualmente
                    int diferencia = stockFinal - _stockOriginal;
                    if (diferencia != 0)
                    {
                        _stockService.AgregarStock(_productoEdicion.IdProducto, idSucursalActual, diferencia);
                    }

                    // Modificar / guardar precio de compra en la base de datos
                    if (precioCompra > 0)
                    {
                        if (_compraService.ObtenerUltimoPrecioCompra(_productoEdicion.IdProducto) != null)
                        {
                            _compraService.ActualizarPrecioCompra(_productoEdicion.IdProducto, precioCompra);
                        }
                        else
                        {
                            int cantCompra = stockFinal > 0 ? stockFinal : 1;
                            var nuevaCompra = new Compra
                            {
                                FechaHora = DateTime.Now,
                                Total = precioCompra * cantCompra,
                                NumeroComprobante = null,
                                IdProveedor = idProveedor,
                                IdEmpleado = idEmpleadoActual,
                                IdSucursal = idSucursalActual
                            };

                            var nuevoDetalle = new DetalleCompra
                            {
                                IdProducto = _productoEdicion.IdProducto,
                                Cantidad = cantCompra,
                                PrecioUnitarioCompra = precioCompra
                            };

                            _compraService.RegistrarCompra(nuevaCompra, nuevoDetalle);
                        }
                    }

                    MessageBox.Show("Producto actualizado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar el producto: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSalirApp_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}

