using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using StockOS.Application;
using StockOS.Application.Services;
using StockOS.Domain.Entities;
using StockOS.Domain.Interfaces;

namespace StockOS.UI.WinForms.Forms
{
    public partial class UcVentas : UserControl
    {
        private readonly IProductoService _productoService;
        private readonly IVentaService _ventaService;
        private readonly ICajaService _cajaService;
        private readonly ITicketService _ticketService;
        private readonly IConfiguracionService _configuracionService;

        private decimal _subtotalVenta = 0;
        private decimal _descuentoTotal = 0;
        private decimal _totalFinal = 0;

        // Diccionario para rastrear el IVA de cada producto en el ticket
        private readonly Dictionary<int, decimal> _ivaProductos = new Dictionary<int, decimal>();

        // Inyectamos todas las dependencias necesarias en el constructor
        public UcVentas(IProductoService productoService, IVentaService ventaService, ICajaService cajaService, ITicketService ticketService, IConfiguracionService configuracionService)
        {
            InitializeComponent();
            _productoService = productoService;
            _ventaService = ventaService;
            _cajaService = cajaService;
            _ticketService = ticketService;
            _configuracionService = configuracionService;

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

            // Guardamos el IVA del producto para usarlo en el ticket
            if (!_ivaProductos.ContainsKey(prod.IdProducto))
            {
                _ivaProductos[prod.IdProducto] = prod.PorcentajeIva;
            }

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

                        // 1. Registramos la venta en la base de datos
                        int idVenta = _ventaService.RegistrarVenta(nuevaVenta, detalles, idSucursal, idMetodoPago);

                        // 2. Preparamos los datos visuales que necesita el PDF
                        nuevaVenta.IdVenta = idVenta;
                        nuevaVenta.FechaHora = DateTime.Now;

                        for (int i = 0; i < dgvTicket.Rows.Count; i++)
                        {
                            int idProd = Convert.ToInt32(dgvTicket.Rows[i].Cells["IdProducto"].Value);
                            decimal ivaProducto = _ivaProductos.ContainsKey(idProd) ? _ivaProductos[idProd] : 21m;

                            detalles[i].IdProductoNavigation = new Producto
                            {
                                Nombre = dgvTicket.Rows[i].Cells["Producto"].Value?.ToString() ?? "Producto",
                                PorcentajeIva = ivaProducto
                            };
                        }
                        nuevaVenta.DetalleVenta = detalles;

                        // 3. Obtenemos los datos del comercio desde la configuración (appsettings.json)
                        var datosComercio = _configuracionService.ObtenerDatosComercio();

                        // 4. Preparamos los datos del pago con el monto recibido y vuelto real
                        var datosPago = new DatosPago
                        {
                            MetodoPago = metodoPago,
                            MontoRecibido = formCobro.MontoRecibido,
                            Vuelto = formCobro.Vuelto
                        };

                        // 5. Generamos el archivo PDF con todos los parámetros
                        string nombreCajero = $"{SesionActual.Usuario!.Nombre} {SesionActual.Usuario!.Apellido}";
                        byte[] pdfBytes = _ticketService.GenerarTicketPdf(nuevaVenta, nombreCajero, datosComercio, datosPago);

                        // 5. Lo guardamos en temporales y lo abrimos automáticamente
                        string rutaTemp = System.IO.Path.Combine(System.IO.Path.GetTempPath(), $"Ticket_{idVenta}.pdf");
                        System.IO.File.WriteAllBytes(rutaTemp, pdfBytes);

                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(rutaTemp)
                        {
                            UseShellExecute = true
                        });
                        //---------------
                        MessageBox.Show($"¡Venta procesada con {metodoPago}!\nTicket N°: {idVenta}",
                                         "Cobro Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        dgvTicket.Rows.Clear();
                        _ivaProductos.Clear();
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