using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using StockOS.Application.Services;
using StockOS.Domain.Entities;

namespace StockOS.UI.WinForms.Forms
{
    public partial class UcInventario : UserControl
    {
        private readonly IProductoService _productoService;
        private readonly IStockService _stockService;
        private readonly ICategoriaService _categoriaService;
        private readonly IServiceProvider _serviceProvider;
        
        private List<Producto> _listaProductos = new();

        public UcInventario(IProductoService productoService, ICategoriaService categoriaService,
            IServiceProvider serviceProvider, IStockService stockService)
        {
            InitializeComponent();
            _productoService = productoService;
            _stockService = stockService;
            _categoriaService = categoriaService;
            _serviceProvider = serviceProvider;

            ConfigurarGrid();
            ConfigurarEventos();
        }

        private void ConfigurarGrid()
        {
            dgvProductos.Columns.Clear();

            dgvProductos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colId", HeaderText = "ID", DataPropertyName = "IdProducto", Visible = false });

            dgvProductos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colCodigo", HeaderText = "Cód. Barra", FillWeight = 100 });
            dgvProductos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colNombre", HeaderText = "Nombre Producto", FillWeight = 180 });
            dgvProductos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colCategoria", HeaderText = "Categoría", FillWeight = 120 });
            dgvProductos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colPrecio", HeaderText = "Precio Venta", FillWeight = 90, DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" } });
            dgvProductos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colStock", HeaderText = "Stock", FillWeight = 70 });
            dgvProductos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colEstado", HeaderText = "Estado", FillWeight = 75 });
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
            btnEditar.Click += (s, e) => EditarSeleccionado();
            btnEliminar.Click += BtnEliminar_Click;
            // Capturar cuando la lectora de barras presiona ENTER
            txtBuscar.KeyDown += TxtBuscar_KeyDown;
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
                    (p.Nombre != null && p.Nombre.ToLowerInvariant().Contains(texto)));
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
                string estadoTexto = (prod.Activo == true) ? "Activo" : "Inactivo";

                int index = dgvProductos.Rows.Add(
                    prod.IdProducto,
                    prod.CodigoBarra,
                    prod.Nombre,
                    categoriaNombre,
                    prod.PrecioVentaActual,
                    estadoTexto
                );

                // Colorear el estado al igual que hizo tu compañero con los usuarios
                if (prod.Activo == false)
                {
                    dgvProductos.Rows[index].DefaultCellStyle.ForeColor = Color.FromArgb(148, 163, 184); // Color gris oscuro
                }
                else
                {
                    dgvProductos.Rows[index].Cells["colEstado"].Style.ForeColor = Color.FromArgb(52, 211, 153); // Color esmeralda
                }
            }

            dgvProductos.Rows.Add();

            foreach (var prod in filtrados)
            {
                string categoriaNombre = prod.IdCategoriaNavigation?.Nombre ?? $"Categoría {prod.IdCategoria}";
                string estadoTexto = (prod.Activo == true) ? "Activo" : "Inactivo";

                // Le preguntamos al servicio de stock cuántas unidades hay en la sucursal actual
                int stockActual = _stockService.ObtenerCantidadActual(prod.IdProducto, SesionActual.IdSucursal);

                int index = dgvProductos.Rows.Add(
                    prod.IdProducto,
                    prod.CodigoBarra,
                    prod.Nombre,
                    categoriaNombre,
                    prod.PrecioVentaActual,
                    stockActual,
                    estadoTexto
                );

                
            }
        }

        private void BtnNuevo_Click(object? sender, EventArgs e)
        {
            // Pedimos prestado el ServiceProvider para abrir la ventana con sus dependencias
            using var scope = _serviceProvider.CreateScope();
            var formRegistro = scope.ServiceProvider.GetRequiredService<FormRegistroProducto>();

            // Lo mostramos como una ventana de diálogo (bloqueante)
            if (formRegistro.ShowDialog() == DialogResult.OK)
            {
                // Si el usuario guardó, recargamos la grilla para ver el nuevo producto
                CargarDatos();
            }
        }
        private Producto? ObtenerProductoSeleccionado()
        {
            // Verificamos que haya una fila seleccionada
            if (dgvProductos.CurrentRow == null || dgvProductos.CurrentRow.Index < 0)
            {
                MessageBox.Show("Por favor seleccione un producto de la lista.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }

            // Buscamos el ID en la columna oculta ("colId")
            int idProducto = Convert.ToInt32(dgvProductos.CurrentRow.Cells["colId"].Value);

            // Devolvemos el producto buscándolo en la lista cargada en memoria
            return _listaProductos.FirstOrDefault(p => p.IdProducto == idProducto);
        }
        private void EditarSeleccionado()
        {
            // 1. Obtenemos el producto de la grilla
            var productoSeleccionado = ObtenerProductoSeleccionado();

            // Si no seleccionó nada (es null), salimos y no hacemos nada
            if (productoSeleccionado == null) return;

            // 2. Abrimos el formulario inyectándolo desde el ServiceProvider
            using var scope = _serviceProvider.CreateScope();
            var formRegistro = scope.ServiceProvider.GetRequiredService<FormRegistroProducto>();

            // 3. ¡LA MAGIA! Le pasamos el producto para que los TextBoxes se llenen solos
            formRegistro.PrepararParaEdicion(productoSeleccionado);

            // 4. Mostramos el formulario y si el usuario da click en "Guardar" (DialogResult.OK), recargamos la grilla
            if (formRegistro.ShowDialog() == DialogResult.OK)
            {
                CargarDatos();
            }
        }
        private void BtnEliminar_Click(object? sender, EventArgs e)
        {
            // 1. Obtenemos el producto seleccionado de la grilla
            var producto = ObtenerProductoSeleccionado();

            // Si no seleccionó nada, salimos
            if (producto == null) return;

            // 2. Determinamos qué mensaje mostrar
            // (Usamos producto.Activo == true para saber si está activo)
            bool estaActivo = producto.Activo == true;
            string accion = estaActivo ? "dar de baja (desactivar)" : "reactivar";

            // 3. confirmación del usuario
            var confirmacion = MessageBox.Show(
                $"¿Está seguro de que desea {accion} el producto '{producto.Nombre}'?",
                "Confirmación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            // 4. Si el usuario dice que "Sí"
            if (confirmacion == DialogResult.Yes)
            {
                try
                {
                    // Invertimos el estado (si era true pasa a false, y viceversa)
                    producto.Activo = !estaActivo;

                    // Usamos el método Actualizar que creamos en el paso anterior para guardarlo
                    _productoService.Actualizar(producto);

                    MessageBox.Show($"Producto {(estaActivo ? "dado de baja" : "reactivado")} correctamente.", "Operación Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Recargamos la grilla para que se actualice el color (gris o verde)
                    CargarDatos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ocurrió un error al intentar cambiar el estado: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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