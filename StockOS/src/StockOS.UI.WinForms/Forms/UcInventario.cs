using Microsoft.Extensions.DependencyInjection;
using StockOS.Application.Services;
using StockOS.Domain.Entities;
using StockOS.Domain.Enums;
using StockOS.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace StockOS.UI.WinForms.Forms
{
    public partial class UcInventario : UserControl
    {
        private readonly IProductoService _productoService;
        private readonly IStockService _stockService;
        private readonly ICategoriaService _categoriaService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IAuthorizationService _authService;
        
        private List<Producto> _listaProductos = new();

        public UcInventario(IProductoService productoService, ICategoriaService categoriaService,
            IServiceProvider serviceProvider, IStockService stockService, IAuthorizationService authService)
        {
            InitializeComponent();
            _productoService = productoService;
            _stockService = stockService;
            _categoriaService = categoriaService;
            _serviceProvider = serviceProvider;
            _authService = authService;

            ConfigurarGrid();
            ConfigurarEventos();
            ConfigurarPermisosModulo();
        }

        private void ConfigurarGrid()
        {
            dgvProductos.Columns.Clear();

            // Garantizar alineación a la izquierda y padding uniforme para todo el encabezado y las celdas
            var paddingUniforme = new Padding(12, 0, 4, 0);

            dgvProductos.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvProductos.ColumnHeadersDefaultCellStyle.Padding = paddingUniforme;
            dgvProductos.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvProductos.DefaultCellStyle.Padding = paddingUniforme;

            dgvProductos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colId", HeaderText = "ID", DataPropertyName = "IdProducto", Visible = false });

            dgvProductos.Columns.Add(new DataGridViewTextBoxColumn 
            { 
                Name = "colCodigo", 
                HeaderText = "Cód. Barra", 
                FillWeight = 85,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleLeft, Padding = paddingUniforme }
            });

            dgvProductos.Columns.Add(new DataGridViewTextBoxColumn 
            { 
                Name = "colNombre", 
                HeaderText = "Nombre Producto", 
                FillWeight = 165,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleLeft, Padding = paddingUniforme }
            });

            dgvProductos.Columns.Add(new DataGridViewTextBoxColumn 
            { 
                Name = "colCategoria", 
                HeaderText = "Categoría", 
                FillWeight = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleLeft, Padding = paddingUniforme }
            });

            dgvProductos.Columns.Add(new DataGridViewTextBoxColumn 
            { 
                Name = "colProveedor", 
                HeaderText = "Proveedor", 
                FillWeight = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleLeft, Padding = paddingUniforme }
            });

            dgvProductos.Columns.Add(new DataGridViewTextBoxColumn 
            { 
                Name = "colPrecio", 
                HeaderText = "Precio Venta", 
                FillWeight = 75, 
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleLeft, Padding = paddingUniforme }
            });

            dgvProductos.Columns.Add(new DataGridViewTextBoxColumn 
            { 
                Name = "colStock", 
                HeaderText = "Stock", 
                FillWeight = 65,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleLeft, Padding = paddingUniforme }
            });

            dgvProductos.Columns.Add(new DataGridViewTextBoxColumn 
            { 
                Name = "colEstado", 
                HeaderText = "Estado", 
                FillWeight = 70,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleLeft, Padding = paddingUniforme }
            });

            // Columna Modificar (solo texto, link clickable alineado a la izquierda)
            var colModificar = new DataGridViewLinkColumn
            {
                Name = "colModificar",
                HeaderText = "Modificar",
                FillWeight = 75,
                LinkBehavior = LinkBehavior.HoverUnderline,
                TrackVisitedState = false,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleLeft,
                    Padding = paddingUniforme,
                    Font = new Font("Segoe UI", 9.5f, FontStyle.Bold)
                }
            };
            dgvProductos.Columns.Add(colModificar);

            // Columna Dar de Baja / Reactivar (solo texto, link clickable alineado a la izquierda)
            var colAccionEstado = new DataGridViewLinkColumn
            {
                Name = "colAccionEstado",
                HeaderText = "Dar de Baja",
                FillWeight = 85,
                LinkBehavior = LinkBehavior.HoverUnderline,
                TrackVisitedState = false,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleLeft,
                    Padding = paddingUniforme,
                    Font = new Font("Segoe UI", 9.5f, FontStyle.Bold)
                }
            };
            dgvProductos.Columns.Add(colAccionEstado);

            // Asegurar que todas las celdas de encabezado tengan explícitamente alineación a la izquierda y padding uniforme
            foreach (DataGridViewColumn col in dgvProductos.Columns)
            {
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                col.HeaderCell.Style.Padding = paddingUniforme;
            }
        }

        private void ConfigurarEventos()
        {
            this.Load += (s, e) =>
            {
                cmbFiltroEstado.SelectedIndex = 0; // "Todos"
                CargarDatos();
            };

            // Filtros en tiempo real
            txtBuscar.TextChanged += (s, e) => AplicarFiltros();
            cmbFiltroEstado.SelectedIndexChanged += (s, e) => AplicarFiltros();

            // Botones
            btnRecargar.Click += (s, e) => CargarDatos();
            btnNuevo.Click += BtnNuevo_Click;
            btnIngresarStock.Click += BtnIngresarStock_Click;
            btnCategorias.Click += BtnCategorias_Click;
            btnProveedores.Click += BtnProveedores_Click;

            // Tooltip para el botón de recargar compacto
            var toolTip = new ToolTip();
            toolTip.SetToolTip(btnRecargar, "Recargar inventario");

            // Acciones por fila en la grilla
            dgvProductos.CellClick += DgvProductos_CellClick;
            dgvProductos.CellDoubleClick += DgvProductos_CellDoubleClick;

            // Capturar cuando la lectora de barras presiona ENTER
            txtBuscar.KeyDown += TxtBuscar_KeyDown;
        }

        private void ConfigurarPermisosModulo()
        {
            btnNuevo.Visible = _authService.TienePermiso(Permisos.PRODUCTOS_CREAR);
            btnIngresarStock.Visible = _authService.TienePermiso(Permisos.STOCK_INGRESAR);
            btnCategorias.Visible = _authService.TienePermiso(Permisos.CATEGORIAS_GESTIONAR);
            btnProveedores.Visible = _authService.TienePermiso(Permisos.PROVEEDORES_GESTIONAR);

            bool puedeEditar = _authService.TienePermiso(Permisos.PRODUCTOS_EDITAR);
            if (dgvProductos.Columns.Contains("colModificar"))
                dgvProductos.Columns["colModificar"].Visible = puedeEditar;
            if (dgvProductos.Columns.Contains("colAccionEstado"))
                dgvProductos.Columns["colAccionEstado"].Visible = puedeEditar;
        }
        private void CargarDatos()
        {
            try
            {
                _listaProductos = _productoService.ObtenerTodos().ToList();
                AplicarFiltros();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el inventario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AplicarFiltros()
        {
            string texto = txtBuscar.Text.Trim().ToLowerInvariant();
            string filtroEstado = cmbFiltroEstado.SelectedItem?.ToString() ?? "Todos";

            var filtrados = _listaProductos.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(texto))
            {
                filtrados = filtrados.Where(p =>
                    (p.CodigoBarra != null && p.CodigoBarra.ToLowerInvariant().Contains(texto)) ||
                    (p.Nombre != null && p.Nombre.ToLowerInvariant().Contains(texto)) ||
                    (p.IdProveedorNavigation?.RazonSocial != null && p.IdProveedorNavigation.RazonSocial.ToLowerInvariant().Contains(texto)));
            }

            if (filtroEstado == "Solo Activos")
            {
                filtrados = filtrados.Where(p => p.Activo == true);
            }
            else if (filtroEstado == "Solo Inactivos")
            {
                filtrados = filtrados.Where(p => p.Activo == false);
            }

            dgvProductos.Rows.Clear();

            foreach (var prod in filtrados)
            {
                string categoriaNombre = prod.IdCategoriaNavigation?.Nombre ?? $"Categoría {prod.IdCategoria}";
                string proveedorNombre = prod.IdProveedorNavigation?.RazonSocial ?? "S/P";
                string estadoTexto = (prod.Activo == true) ? "Activo" : "Inactivo";
                string textoAccion = (prod.Activo == true) ? "Dar de baja" : "Reactivar";

                // Le preguntamos al servicio de stock cuántas unidades hay (usando nuestra sesión dinámica)
                int stockActual = _stockService.ObtenerCantidadActual(prod.IdProducto, SesionActual.IdSucursal);

                int index = dgvProductos.Rows.Add(
                    prod.IdProducto.ToString(),
                    prod.CodigoBarra,
                    prod.Nombre,
                    categoriaNombre,
                    proveedorNombre,
                    $"$ {prod.PrecioVentaActual:N2}",
                    stockActual.ToString(),
                    estadoTexto,
                    "Modificar",
                    textoAccion
                );

                // Colorear el estado 
                if (prod.Activo == false)
                {
                    dgvProductos.Rows[index].DefaultCellStyle.ForeColor = Color.FromArgb(148, 163, 184); // Color gris oscuro
                }
                else
                {
                    dgvProductos.Rows[index].Cells["colEstado"].Style.ForeColor = Color.FromArgb(52, 211, 153); // Color esmeralda
                }

                // Estilo para el link "Modificar"
                if (dgvProductos.Rows[index].Cells["colModificar"] is DataGridViewLinkCell cellModificar)
                {
                    cellModificar.LinkColor = Color.FromArgb(56, 189, 248); // Azul celeste
                    cellModificar.ActiveLinkColor = Color.FromArgb(147, 197, 253);
                    cellModificar.VisitedLinkColor = Color.FromArgb(56, 189, 248);
                }

                // Estilo para el link "Dar de baja" / "Reactivar"
                if (dgvProductos.Rows[index].Cells["colAccionEstado"] is DataGridViewLinkCell cellAccion)
                {
                    if (prod.Activo == true)
                    {
                        cellAccion.LinkColor = Color.FromArgb(248, 113, 113); // Rojo suave / coral
                        cellAccion.ActiveLinkColor = Color.FromArgb(252, 165, 165);
                        cellAccion.VisitedLinkColor = Color.FromArgb(248, 113, 113);
                    }
                    else
                    {
                        cellAccion.LinkColor = Color.FromArgb(52, 211, 153); // Verde esmeralda
                        cellAccion.ActiveLinkColor = Color.FromArgb(110, 231, 183);
                        cellAccion.VisitedLinkColor = Color.FromArgb(52, 211, 153);
                    }
                }
            }
        }

        private void BtnNuevo_Click(object? sender, EventArgs e)
        {
            using var scope = _serviceProvider.CreateScope();
            var formRegistro = scope.ServiceProvider.GetRequiredService<FormRegistroProducto>();

            if (formRegistro.ShowDialog() == DialogResult.OK)
            {
                CargarDatos();
            }
        }

        private void DgvProductos_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvProductos.Rows.Count) return;

            string colName = dgvProductos.Columns[e.ColumnIndex].Name;

            if (colName == "colModificar")
            {
                if (int.TryParse(dgvProductos.Rows[e.RowIndex].Cells["colId"].Value?.ToString(), out int idProducto))
                {
                    var producto = _listaProductos.FirstOrDefault(p => p.IdProducto == idProducto);
                    if (producto != null)
                    {
                        AbrirFormularioEdicion(producto);
                    }
                }
            }
            else if (colName == "colAccionEstado")
            {
                if (int.TryParse(dgvProductos.Rows[e.RowIndex].Cells["colId"].Value?.ToString(), out int idProducto))
                {
                    var producto = _listaProductos.FirstOrDefault(p => p.IdProducto == idProducto);
                    if (producto != null)
                    {
                        CambiarEstadoProducto(producto);
                    }
                }
            }
        }

        private void DgvProductos_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvProductos.Rows.Count) return;
            string colName = dgvProductos.Columns[e.ColumnIndex].Name;
            if (colName == "colModificar" || colName == "colAccionEstado") return;

            if (int.TryParse(dgvProductos.Rows[e.RowIndex].Cells["colId"].Value?.ToString(), out int idProducto))
            {
                var producto = _listaProductos.FirstOrDefault(p => p.IdProducto == idProducto);
                if (producto != null)
                {
                    AbrirFormularioEdicion(producto);
                }
            }
        }

        private void AbrirFormularioEdicion(Producto producto)
        {
            using var scope = _serviceProvider.CreateScope();
            var formRegistro = scope.ServiceProvider.GetRequiredService<FormRegistroProducto>();
            formRegistro.PrepararParaEdicion(producto);

            if (formRegistro.ShowDialog() == DialogResult.OK)
            {
                CargarDatos();
            }
        }

        private void CambiarEstadoProducto(Producto producto)
        {
            bool estaActivo = producto.Activo == true;
            string accion = estaActivo ? "dar de baja (desactivar)" : "reactivar";

            var confirmacion = MessageBox.Show(
                $"¿Está seguro de que desea {accion} el producto '{producto.Nombre}'?",
                "Confirmación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmacion == DialogResult.Yes)
            {
                try
                {
                    bool nuevoEstado = !estaActivo;
                    _productoService.CambiarEstado(producto.IdProducto, nuevoEstado);
                    producto.Activo = nuevoEstado;

                    MessageBox.Show($"Producto {(estaActivo ? "dado de baja" : "reactivado")} correctamente.",
                        "Operación Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    CargarDatos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ocurrió un error al intentar cambiar el estado: {ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void TxtBuscar_KeyDown(object? sender, KeyEventArgs e)
        {
            // Si la tecla presionada es ENTER (lo que manda la pistola al final del código)
            if (e.KeyCode == Keys.Enter)
            {
                // Evitamos el sonido molesto de "beep" de Windows
                e.SuppressKeyPress = true;

                //Ejecutamos el filtro (aunque ya se hace en tiempo real, esto asegura la búsqueda exacta)
                AplicarFiltros();

                //Opcional: Si el filtro dejó un solo producto en la grilla, lo podemos seleccionar automáticamente
                if (dgvProductos.Rows.Count == 1)
                {
                    dgvProductos.Rows[0].Selected = true;
                    // Aquí podrías, por ejemplo, abrirlo para editar directo o mandarlo a ventas
                }
            }
        }
        private void BtnCategorias_Click(object? sender, EventArgs e)
        {
            using var scope = _serviceProvider.CreateScope();
            var formCategoria = scope.ServiceProvider.GetRequiredService<FormCategoria>();
            formCategoria.ShowDialog();
            // Al cerrar, recargamos la grilla por si se cambió alguna categoría
            CargarDatos();
        }

        private void BtnProveedores_Click(object? sender, EventArgs e)
        {
            using var scope = _serviceProvider.CreateScope();
            var formProveedor = scope.ServiceProvider.GetRequiredService<FormProveedor>();
            formProveedor.ShowDialog();
            // Al cerrar, recargamos la grilla por si se cambió algún proveedor
            CargarDatos();
        }

        private void BtnIngresarStock_Click(object? sender, EventArgs e)
        {
            using var scope = _serviceProvider.CreateScope();
            var formStock = scope.ServiceProvider.GetRequiredService<FormIngresoStock>();

            if (formStock.ShowDialog() == DialogResult.OK)
            {
                CargarDatos(); // Recarga la grilla para mostrar el nuevo stock
            }
        }
    }
}