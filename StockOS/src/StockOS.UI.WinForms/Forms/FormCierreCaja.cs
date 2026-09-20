using System;
using System.Windows.Forms;
using StockOS.Application;
using StockOS.Application.Services;
using Serilog;
namespace StockOS.UI.WinForms.Forms
{
    public partial class FormCierreCaja : Form
    {
        private readonly ICajaService _cajaService;

        public FormCierreCaja(ICajaService cajaService)
        {
            InitializeComponent();
            _cajaService = cajaService;
        }

        private void btnConfirmarCierre_Click(object sender, EventArgs e)
        {
            // Corregimos la validación para soportar nulos
            if (!SesionActual.IdCajaSesionAbierta.HasValue || SesionActual.IdCajaSesionAbierta.Value == 0)
            {
                MessageBox.Show("No hay ninguna caja abierta actualmente.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtMontoReal.Text, out decimal montoReal))
            {
                MessageBox.Show("Por favor, ingrese un monto válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                int idCaja = SesionActual.IdCajaSesionAbierta.Value;

                // 1. Calculamos cuánto DEBERÍA haber según el sistema
                decimal montoEsperado = _cajaService.ObtenerMontoEsperado(idCaja);

                // 2. Calculamos la diferencia
                decimal diferencia = montoReal - montoEsperado;

                // 3. Armamos el cuadro de alerta
                string mensaje = $"Resumen de Liquidación:\n\n" +
                                 $"- Monto Esperado (Sistema): $ {montoEsperado:N2}\n" +
                                 $"- Monto Declarado (Cajero): $ {montoReal:N2}\n" +
                                 $"- Diferencia de Caja: $ {diferencia:N2}\n\n" +
                                 "¿Desea confirmar el cierre definitivo?";

                var confirmacion = MessageBox.Show(mensaje, "Confirmar Cierre", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirmacion == DialogResult.Yes)
                {
                    // Cerramos en la base de datos
                    _cajaService.CerrarCaja(idCaja, montoReal);

                    // IMPORTANTE: Limpiamos la caja llamando al método centralizado
                    SesionActual.IdCajaSesionAbierta = null;

                    MessageBox.Show("Turno finalizado y caja cerrada correctamente.", "Cierre Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                // 1. Guardamos el error real y la línea exacta en el archivo oculto (para el programador)
                Log.Error(ex, "Error crítico al intentar cerrar la caja. Usuario ID: {UsuarioId}", SesionActual.Usuario?.IdEmpleado);

                // 2. Le mostramos un mensaje genérico y amigable al usuario
                MessageBox.Show("Ocurrió un error interno al cerrar la caja. Por favor, contacte al administrador.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}