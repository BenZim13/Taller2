using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using StockOS.Application.Services;
using StockOS.Domain.Entities;

namespace StockOS.UI.WinForms.Forms
{
    public partial class UcVentas : UserControl
    {
        private readonly IProductoService _productoService;
        private readonly IVentaService _ventaService;
        private decimal _totalVenta = 0;

        public UcVentas(IProductoService productoService, IVentaService ventaService)
        {
            InitializeComponent();
            _productoService = productoService;
            _ventaService = ventaService;

            // Foco automático en el buscador al abrir
            this.Load += (s, e) => txtCodigoBarra.Focus();
        }

        private void txtCodigoBarra_KeyDown(object sender, KeyEventArgs e)
        {
            // Solo actuamos si presionan la tecla Enter
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Evita el sonido de "beep" de Windows
                string codigoBuscar = txtCodigoBarra.Text.Trim();

                if (string.IsNullOrEmpty(codigoBuscar)) return;

                // TODO: Necesitas tener este método en tu ProductoService
                // var producto = _productoService.ObtenerPorCodigoBarra(codigoBuscar);

                // MOCK PROVISORIO (Bórralo y usa el de arriba cuando tengas el método real):
                var producto = _productoService.ObtenerTodos().FirstOrDefault(p => p.CodigoBarra == codigoBuscar);

                if (producto != null)
                {
                    AgregarProductoAlTicket(producto);
                    txtCodigoBarra.Clear();
                }
                else
                {
                    MessageBox.Show("Producto no encontrado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtCodigoBarra.SelectAll();
                }
            }
        }

        private void AgregarProductoAlTicket(Producto prod)
        {
            // 1. Buscamos si el producto ya está en la grilla para sumar cantidad
            foreach (DataGridViewRow row in dgvTicket.Rows)
            {
                if (Convert.ToInt32(row.Cells["IdProducto"].Value) == prod.IdProducto)
                {
                    int cantidadActual = Convert.ToInt32(row.Cells["Cantidad"].Value);
                    row.Cells["Cantidad"].Value = cantidadActual + 1;

                    decimal nuevoSubtotal = (cantidadActual + 1) * prod.PrecioVentaActual;
                    row.Cells["Subtotal"].Value = nuevoSubtotal;

                    ActualizarTotal();
                    return;
                }
            }

            // 2. Si no estaba, lo agregamos como fila nueva
            dgvTicket.Rows.Add(prod.IdProducto, prod.Nombre, 1, prod.PrecioVentaActual, prod.PrecioVentaActual);
            ActualizarTotal();
        }

        private void dgvTicket_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Si el usuario hace clic en el botón de la columna "Eliminar" (índice 5)
            if (e.ColumnIndex == 5 && e.RowIndex >= 0)
            {
                dgvTicket.Rows.RemoveAt(e.RowIndex);
                ActualizarTotal();
            }
        }

        private void ActualizarTotal()
        {
            _totalVenta = 0;
            foreach (DataGridViewRow row in dgvTicket.Rows)
            {
                _totalVenta += Convert.ToDecimal(row.Cells["Subtotal"].Value);
            }
            lblTotal.Text = $"$ {_totalVenta:N2}";
        }

        private void btnCobrar_Click(object sender, EventArgs e)
        {
            if (dgvTicket.Rows.Count == 0)
            {
                MessageBox.Show("El ticket está vacío.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // En el próximo paso armaremos la ventanita modal para elegir el método de pago 
            // (Efectivo, Tarjeta, etc) y ahí ejecutaremos _ventaService.ProcesarNuevaVenta.

            MessageBox.Show($"¡Listo para cobrar ${_totalVenta:N2}!\n\nEn el próximo paso conectaremos esto con tu VentaService.",
                            "Caja", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}