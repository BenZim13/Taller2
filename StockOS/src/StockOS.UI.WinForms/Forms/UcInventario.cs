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
        private readonly ICategoriaService _categoriaService;
        private List<Producto> _listaProductos = new();

        public UcInventario(IProductoService productoService, ICategoriaService categoriaService)
        {
            InitializeComponent();
            _productoService = productoService;
            _categoriaService = categoriaService;

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

            // Más adelante implementaremos estos dos:
            // btnEditar.Click += (s, e) => EditarSeleccionado();
            // btnEliminar.Click += BtnEliminar_Click;
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
        }

        private void BtnNuevo_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("Aquí abriremos el formulario para agregar un nuevo producto a la base de datos.", "Nuevo Producto", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}