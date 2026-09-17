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
        private decimal _subtotalVenta = 0;
        private decimal _descuentoTotal = 0;
        private decimal _totalFinal = 0;
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
            _subtotalVenta = 0;

            // 1. Calculamos el subtotal puro sumando la grilla
            foreach (DataGridViewRow row in dgvTicket.Rows)
            {
                _subtotalVenta += Convert.ToDecimal(row.Cells["Subtotal"].Value);
            }

            // 2. Leemos el descuento ingresado por el cajero
            _descuentoTotal = 0;
            if (decimal.TryParse(txtDescuento.Text, out decimal descuentoIngresado))
            {
                _descuentoTotal = descuentoIngresado;
            }

            // 3. Evitamos descuentos negativos o mayores al subtotal
            if (_descuentoTotal < 0) _descuentoTotal = 0;
            if (_descuentoTotal > _subtotalVenta) _descuentoTotal = _subtotalVenta;

            // 4. Calculamos total final
            _totalFinal = _subtotalVenta - _descuentoTotal;

            // 5. Actualizamos la interfaz
            // Asumiendo que agregaste un lblSubtotal en tu UI, si no, puedes omitir esa línea
            // lblSubtotal.Text = $"Subtotal: ${_subtotalVenta:N2}"; 
            lblTotal.Text = $"$ {_totalFinal:N2}";
        }

        private void btnCobrar_Click(object sender, EventArgs e)
        {
            if (dgvTicket.Rows.Count == 0)
            {
                MessageBox.Show("El ticket está vacío.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 1. Abrimos el modal de cobro y le pasamos el total
            using (var formCobro = new FormCobro(_totalFinal))
            {
                var resultado = formCobro.ShowDialog();

                // 2. Si el cajero seleccionó un método y NO canceló, avanzamos con el guardado
                if (resultado == DialogResult.OK)
                {
                    string metodoPago = formCobro.MetodoPagoSeleccionado;

                    try
                    {
                        var nuevaVenta = new Venta
                        {
                            Subtotal = _subtotalVenta,
                            DescuentoTotal = _descuentoTotal,
                            TotalVenta = _totalFinal,
                            IdCajaSesion = 1, // Asegúrate de que SesionActual.Usuario no sea null
                            IdCliente = null
                        };

                        var detalles = new List<DetalleVenta>();
                        decimal descuentoPorItem = dgvTicket.Rows.Count > 0 ? (_descuentoTotal / dgvTicket.Rows.Count) : 0;

                        foreach (DataGridViewRow row in dgvTicket.Rows)
                        {
                            detalles.Add(new DetalleVenta
                            {
                                IdProducto = Convert.ToInt32(row.Cells["IdProducto"].Value),
                                Cantidad = Convert.ToInt32(row.Cells["Cantidad"].Value),
                                PrecioUnitarioHistorico = Convert.ToDecimal(row.Cells["Precio"].Value),
                                Descuento = descuentoPorItem
                            });
                        }

                        int idSucursal = SesionActual.Usuario!.IdSucursal;

                        // Impactar Venta y Stock en BD
                        int idVenta = _ventaService.RegistrarVenta(nuevaVenta, detalles, idSucursal);
                        //Aca generamo el PDF brother

                        var itemsParaTicket = new List<ItemTicket>();
                        foreach (DataGridViewRow row in dgvTicket.Rows)
                        {
                            itemsParaTicket.Add(new ItemTicket
                            {
                                Producto = row.Cells["Producto"].Value.ToString() ?? "Producto",
                                Cantidad = Convert.ToInt32(row.Cells["Cantidad"].Value),
                                PrecioTotal = Convert.ToDecimal(row.Cells["Subtotal"].Value)
                            });
                        }
                        // Obtenemos el nombre del cajero de la sesión actual
                        string nombreCajero = $"{SesionActual.Usuario!.Nombre} {SesionActual.Usuario!.Apellido}";

                        // Disparamos el PDF (se guardará en temporales y se abrirá solo)
                        TicketService.GenerarYAbrirTicket(idVenta, nombreCajero, _totalFinal, itemsParaTicket);
                        // ==========================================
                        
                        MessageBox.Show($"¡Venta procesada con {metodoPago}!\nTicket N°: {idVenta}",
                                         "Cobro Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Limpiamo la UI
                        dgvTicket.Rows.Clear();
                        txtDescuento.Clear();
                        ActualizarTotal();
                        txtCodigoBarra.Focus();
                    }
                    catch (Exception excepcion)
                    {
                        MessageBox.Show($"Error al procesar la venta: {excepcion.Message}", "Error Crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void txtDescuento_TextChanged(object sender, EventArgs e)
        {
            ActualizarTotal();
        }
    }
}