using StockOS.Application;
using StockOS.Application.Reports;
using StockOS.Application.Services;
using StockOS.Domain.Entities;
using StockOS.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;


namespace StockOS.UI.WinForms.Forms
{
    public partial class UcVentas : UserControl
    {
        private readonly IProductoService _productoService;
        private readonly IVentaService _ventaService;
        private readonly ICajaService _cajaService;

        private decimal _subtotalVenta = 0;
        private decimal _descuentoTotal = 0;
        private decimal _totalFinal = 0;
        private readonly ITicketService _ticketService;

        // Inyectamos ICajaService en el constructor
        public UcVentas(IProductoService productoService, IVentaService ventaService,
                            ICajaService cajaService, ITicketService ticketService)
        {
            InitializeComponent();
            _productoService = productoService;
            _ventaService = ventaService;
            _cajaService = cajaService;
            _ticketService = ticketService;

            this.Load += (s, e) => txtCodigoBarra.Focus();
        }

        // ==========================================
        // BOTONES DE CAJA
        // ==========================================
        private void btnAbrirCaja_Click(object sender, EventArgs e)
        {
            using (var formApertura = new FormAperturaCaja(_cajaService))
            {
                formApertura.ShowDialog();
            }
        }
        private void btnMovimientoCaja_Click(object sender, EventArgs e)
        {
            using (var formMov = new FormMovimientoCaja(_cajaService))
            {
                formMov.ShowDialog();
            }
        }

        private void btnCerrarCaja_Click(object sender, EventArgs e)
        {
            using (var formCierre = new FormCierreCaja(_cajaService))
            {
                formCierre.ShowDialog();
            }
        }
        

        private void txtCodigoBarra_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                string codigoBuscar = txtCodigoBarra.Text.Trim();

                if (string.IsNullOrEmpty(codigoBuscar)) return;

                var producto = _productoService.BuscarPorCodigoBarra(codigoBuscar);

                if (producto != null)
                {
                    if (producto.Activo == false)
                    {
                        MessageBox.Show($"El producto '{producto.Nombre}' está deshabilitado / inactivo y no puede ser vendido.", "Producto Inactivo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtCodigoBarra.SelectAll();
                        return;
                    }

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

            dgvTicket.Rows.Add(prod.IdProducto, prod.Nombre, 1, prod.PrecioVentaActual, prod.PrecioVentaActual);
            ActualizarTotal();
        }

        private void dgvTicket_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 5 && e.RowIndex >= 0)
            {
                dgvTicket.Rows.RemoveAt(e.RowIndex);
                ActualizarTotal();
            }
        }

        private void ActualizarTotal()
        {
            _subtotalVenta = 0;
            foreach (DataGridViewRow row in dgvTicket.Rows)
            {
                _subtotalVenta += Convert.ToDecimal(row.Cells["Subtotal"].Value);
            }

            _descuentoTotal = 0;
            if (decimal.TryParse(txtDescuento.Text, out decimal descuentoIngresado))
            {
                _descuentoTotal = descuentoIngresado;
            }

            if (_descuentoTotal < 0) _descuentoTotal = 0;
            if (_descuentoTotal > _subtotalVenta) _descuentoTotal = _subtotalVenta;

            _totalFinal = _subtotalVenta - _descuentoTotal;
            lblTotal.Text = $"$ {_totalFinal:N2}";
        }

        private void btnCobrar_Click(object sender, EventArgs e)
        {
            // Corrección: ahora validamos que no sea nulo además de 0
            if (!SesionActual.IdCajaSesionAbierta.HasValue || SesionActual.IdCajaSesionAbierta.Value == 0)
            {
                MessageBox.Show("No puedes cobrar porque no has abierto la caja. Haz clic en 'Abrir Caja' en la barra superior.",
                                "Caja Cerrada", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (dgvTicket.Rows.Count == 0)
            {
                MessageBox.Show("El ticket está vacío.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var formCobro = new FormCobro(_totalFinal))
            {
                var resultado = formCobro.ShowDialog();

                if (resultado == DialogResult.OK)
                {
                    string metodoPago = formCobro.MetodoPagoSeleccionado;

                    // Mapeamos el string al ID de la base de datos
                    int idMetodoPago = 1; // Efectivo por defecto
                    string metodoLower = metodoPago.ToLower();

                    if (metodoLower.Contains("débito") || metodoLower.Contains("debito"))
                    {
                        idMetodoPago = 2;
                    }
                    else if (metodoLower.Contains("crédito") || metodoLower.Contains("credito") || metodoLower == "tarjeta")
                    {
                        idMetodoPago = 3;
                    }
                    else if (metodoLower.Contains("mercado") || metodoLower.Contains("mp"))
                    {
                        idMetodoPago = 4;
                    }

                    try
                    {
                        var nuevaVenta = new Venta
                        {
                            Subtotal = _subtotalVenta,
                            DescuentoTotal = _descuentoTotal,
                            TotalVenta = _totalFinal,
                            IdCajaSesion = SesionActual.IdCajaSesionAbierta.Value,
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

                        // Le pasamos el idMetodoPago al servicio
                        int idVenta = _ventaService.RegistrarVenta(nuevaVenta, detalles, idSucursal, idMetodoPago);

                        // --- NUEVA LÓGICA DE TICKET PDF ---
                        // 1. Preparamos los datos visuales que necesita el PDF
                        nuevaVenta.IdVenta = idVenta;
                        nuevaVenta.FechaHora = DateTime.Now;

                        for (int i = 0; i < dgvTicket.Rows.Count; i++)
                        {
                            detalles[i].IdProductoNavigation = new Producto
                            {
                                Nombre = dgvTicket.Rows[i].Cells["Producto"].Value?.ToString() ?? "Producto"
                            };
                        }
                        nuevaVenta.DetalleVenta = detalles;

                        // 2. Generamos el archivo
                        string nombreCajero = $"{SesionActual.Usuario!.Nombre} {SesionActual.Usuario!.Apellido}";
                        byte[] pdfBytes = _ticketService.GenerarTicketPdf(nuevaVenta, nombreCajero);

                        // 3. Lo guardamos en una carpeta temporal y lo abrimos automáticamente
                        string rutaTemp = System.IO.Path.Combine(System.IO.Path.GetTempPath(), $"Ticket_{idVenta}.pdf");
                        System.IO.File.WriteAllBytes(rutaTemp, pdfBytes);

                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(rutaTemp)
                        {
                            UseShellExecute = true
                        });
                        // -----------------------------------

                        MessageBox.Show($"¡Venta procesada con {metodoPago}!\nTicket N°: {idVenta}",
                                         "Cobro Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        dgvTicket.Rows.Clear();
                        txtDescuento.Clear();
                        ActualizarTotal();
                        txtCodigoBarra.Focus();
                    }
                    catch (Exception excepcion)
                    {
                        MessageBox.Show($"Error al procesar la venta: {excepcion.Message}",
                                            "Error Crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
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