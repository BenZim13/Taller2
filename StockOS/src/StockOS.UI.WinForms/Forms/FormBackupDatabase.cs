using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using StockOS.Domain.Enums;
using StockOS.Domain.Interfaces;

namespace StockOS.UI.WinForms.Forms
{
    public partial class FormBackupDatabase : Form
    {
        private readonly IBackupService _backupService;
        private PeriodoBackup _periodoSeleccionado = PeriodoBackup.Diario;
        private bool _isBusy = false;

        public FormBackupDatabase(IBackupService backupService)
        {
            _backupService = backupService;
            InitializeComponent();
            InicializarValores();
        }

        private void InicializarValores()
        {
            try
            {
                var config = _backupService.ObtenerProgramacionPeriodica();
                txtRutaDestino.Text = !string.IsNullOrWhiteSpace(config.RutaDestino)
                    ? config.RutaDestino
                    : _backupService.ObtenerDirectorioBackupsPorDefecto();

                chkProgramarAuto.Checked = config.Habilitado;

                if (TimeSpan.TryParse(config.HoraEjecucion, out TimeSpan hora))
                {
                    dtpHoraProgramada.Value = DateTime.Today.Add(hora);
                }
                else
                {
                    dtpHoraProgramada.Value = DateTime.Today.AddHours(23);
                }

                SeleccionarPeriodo(config.Periodo);

                if (config.Habilitado && config.ProximaEjecucion.HasValue)
                {
                    lblBackupEstado.ForeColor = Color.FromArgb(74, 222, 128);
                    lblBackupEstado.Text = $"Programado ({config.Periodo}): Próx. {config.ProximaEjecucion.Value:dd/MM/yyyy HH:mm}";
                }
            }
            catch
            {
                txtRutaDestino.Text = @"C:\StockOS\Backups";
                dtpHoraProgramada.Value = DateTime.Today.AddHours(23);
                SeleccionarPeriodo(PeriodoBackup.Diario);
            }
        }

        private void SeleccionarPeriodo(PeriodoBackup periodo)
        {
            _periodoSeleccionado = periodo;

            // Resetear estilo de todos los botones de período
            EstilizarBotonPeriodo(btnPeriodoDia, periodo == PeriodoBackup.Diario);
            EstilizarBotonPeriodo(btnPeriodoSemana, periodo == PeriodoBackup.Semanal);
            EstilizarBotonPeriodo(btnPeriodoMes, periodo == PeriodoBackup.Mensual);
            EstilizarBotonPeriodo(btnPeriodoAnio, periodo == PeriodoBackup.Anual);

            switch (periodo)
            {
                case PeriodoBackup.Diario:
                    lblInfoPeriodo.Text = @"📌 Período DIARIO: Almacena en subcarpeta '\Diario' y conserva los últimos 7 días de copias.";
                    btnGenerarBackup.Text = "Crear Backup (Por Día)";
                    break;
                case PeriodoBackup.Semanal:
                    lblInfoPeriodo.Text = @"📌 Período SEMANAL: Almacena en subcarpeta '\Semanal' y conserva las últimas 4 semanas (30 días).";
                    btnGenerarBackup.Text = "Crear Backup (Por Semana)";
                    break;
                case PeriodoBackup.Mensual:
                    lblInfoPeriodo.Text = @"📌 Período MENSUAL: Almacena en subcarpeta '\Mensual' y conserva los últimos 12 meses (1 año).";
                    btnGenerarBackup.Text = "Crear Backup (Por Mes)";
                    break;
                case PeriodoBackup.Anual:
                    lblInfoPeriodo.Text = @"📌 Período ANUAL: Almacena en subcarpeta '\Anual' y conserva histórico contable por 5 años.";
                    btnGenerarBackup.Text = "Crear Backup (Por Año)";
                    break;
            }
        }

        private static void EstilizarBotonPeriodo(Button btn, bool activo)
        {
            if (activo)
            {
                btn.BackColor = Color.FromArgb(37, 99, 235);
                btn.ForeColor = Color.White;
                btn.FlatAppearance.BorderColor = Color.FromArgb(96, 165, 250);
            }
            else
            {
                btn.BackColor = Color.FromArgb(30, 41, 59);
                btn.ForeColor = Color.FromArgb(203, 213, 225);
                btn.FlatAppearance.BorderColor = Color.FromArgb(71, 85, 105);
            }
        }

        private void btnPeriodoDia_Click(object? sender, EventArgs e)
        {
            if (!_isBusy) SeleccionarPeriodo(PeriodoBackup.Diario);
        }

        private void btnPeriodoSemana_Click(object? sender, EventArgs e)
        {
            if (!_isBusy) SeleccionarPeriodo(PeriodoBackup.Semanal);
        }

        private void btnPeriodoMes_Click(object? sender, EventArgs e)
        {
            if (!_isBusy) SeleccionarPeriodo(PeriodoBackup.Mensual);
        }

        private void btnPeriodoAnio_Click(object? sender, EventArgs e)
        {
            if (!_isBusy) SeleccionarPeriodo(PeriodoBackup.Anual);
        }

        private async void btnGuardarProgramacion_Click(object? sender, EventArgs e)
        {
            if (_isBusy) return;

            string destino = txtRutaDestino.Text.Trim();
            if (string.IsNullOrWhiteSpace(destino))
            {
                MessageBox.Show("Por favor indique una carpeta raíz de destino válida.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var configActual = _backupService.ObtenerProgramacionPeriodica();
            var nuevaConfig = new ConfiguracionBackupPeriodico
            {
                Habilitado = chkProgramarAuto.Checked,
                Periodo = _periodoSeleccionado,
                HoraEjecucion = dtpHoraProgramada.Value.ToString("HH:mm"),
                RutaDestino = destino,
                UltimaEjecucion = configActual.UltimaEjecucion
            };

            SetBusyState(true, "Guardando programación periódica...");

            try
            {
                var resultado = await _backupService.GuardarProgramacionPeriodicaAsync(nuevaConfig);

                if (resultado.Exito)
                {
                    var configGuardada = _backupService.ObtenerProgramacionPeriodica();
                    if (configGuardada.Habilitado && configGuardada.ProximaEjecucion.HasValue)
                    {
                        lblBackupEstado.ForeColor = Color.FromArgb(74, 222, 128);
                        lblBackupEstado.Text = $"Programado ({configGuardada.Periodo}): Próx. {configGuardada.ProximaEjecucion.Value:dd/MM/yyyy HH:mm}";
                    }
                    else
                    {
                        lblBackupEstado.ForeColor = Color.FromArgb(148, 163, 184);
                        lblBackupEstado.Text = "Programación automática desactivada.";
                    }

                    MessageBox.Show(resultado.Mensaje, "Programación de Respaldo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(resultado.Mensaje, "Error al Programar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            finally
            {
                SetBusyState(false);
            }
        }

        private void btnCerrar_Click(object? sender, EventArgs e)
        {
            if (_isBusy)
            {
                MessageBox.Show("Hay una operación de base de datos en curso. Por favor espere a que finalice.", "Operación en progreso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            this.Close();
        }

        private void btnExaminarDestino_Click(object? sender, EventArgs e)
        {
            if (_isBusy) return;

            using var fbd = new FolderBrowserDialog();
            fbd.Description = "Seleccione la carpeta raíz donde se organizarán las copias de seguridad por período:";
            fbd.UseDescriptionForTitle = true;

            if (Directory.Exists(txtRutaDestino.Text))
            {
                fbd.InitialDirectory = txtRutaDestino.Text;
            }

            if (fbd.ShowDialog(this) == DialogResult.OK)
            {
                txtRutaDestino.Text = fbd.SelectedPath;
            }
        }

        private async void btnGenerarBackup_Click(object? sender, EventArgs e)
        {
            if (_isBusy) return;

            string destino = txtRutaDestino.Text.Trim();
            if (string.IsNullOrWhiteSpace(destino))
            {
                MessageBox.Show("Por favor indique una carpeta de destino válida.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SetBusyState(true, $"Generando copia [{_periodoSeleccionado}]... Por favor espere.");

            try
            {
                var resultado = await _backupService.CrearBackupAsync(destino, _periodoSeleccionado);

                if (resultado.Exito)
                {
                    lblBackupEstado.ForeColor = Color.FromArgb(74, 222, 128);
                    lblBackupEstado.Text = $"Último backup ({_periodoSeleccionado}): {DateTime.Now:HH:mm:ss} - Completado";
                    MessageBox.Show(resultado.Mensaje, "Backup por Período Completado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    lblBackupEstado.ForeColor = Color.FromArgb(248, 113, 113);
                    lblBackupEstado.Text = "Error al generar la copia de seguridad.";
                    MessageBox.Show(resultado.Mensaje, "Error al generar Backup", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                lblBackupEstado.ForeColor = Color.FromArgb(248, 113, 113);
                lblBackupEstado.Text = "Excepción inesperada.";
                MessageBox.Show($"Ocurrió un error inesperado:\n{ex.Message}", "Error Inesperado", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetBusyState(false);
            }
        }

        private void btnExaminarOrigen_Click(object? sender, EventArgs e)
        {
            if (_isBusy) return;

            using var ofd = new OpenFileDialog();
            ofd.Title = "Seleccionar archivo de copia de seguridad (.bak)";
            ofd.Filter = "Copias de Seguridad SQL Server (*.bak)|*.bak|Todos los archivos (*.*)|*.*";
            ofd.FilterIndex = 1;

            if (Directory.Exists(txtRutaDestino.Text))
            {
                ofd.InitialDirectory = txtRutaDestino.Text;
            }

            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                txtRutaOrigen.Text = ofd.FileName;
                lblRestoreEstado.ForeColor = Color.FromArgb(56, 189, 248);
                lblRestoreEstado.Text = "Archivo seleccionado. Listo para restaurar.";
            }
        }

        private async void btnRestaurarBackup_Click(object? sender, EventArgs e)
        {
            if (_isBusy) return;

            string rutaBak = txtRutaOrigen.Text.Trim();
            if (string.IsNullOrWhiteSpace(rutaBak) || !File.Exists(rutaBak))
            {
                MessageBox.Show("Por favor seleccione un archivo .bak existente para restaurar.", "Archivo no encontrado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show(
                "¿Está COMPLETAMENTE SEGURO de restaurar la base de datos?\n\n" +
                "• Todos los datos actuales del sistema serán sobrescritos por los del archivo seleccionado.\n" +
                "• Esta operación es irreversible.\n\n" +
                "¿Desea continuar con la restauración?",
                "⚠️ Confirmación de Restauración Crítica",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            if (confirm != DialogResult.Yes) return;

            SetBusyState(true, "Restaurando base de datos en SQL Server... Por favor espere.");

            try
            {
                var resultado = await _backupService.RestaurarBackupAsync(rutaBak);

                if (resultado.Exito)
                {
                    lblRestoreEstado.ForeColor = Color.FromArgb(74, 222, 128);
                    lblRestoreEstado.Text = $"Restauración finalizada con éxito a las {DateTime.Now:HH:mm:ss}";
                    MessageBox.Show(resultado.Mensaje, "Restauración Completada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    lblRestoreEstado.ForeColor = Color.FromArgb(248, 113, 113);
                    lblRestoreEstado.Text = "Error durante la restauración.";
                    MessageBox.Show(resultado.Mensaje, "Error de Restauración", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                lblRestoreEstado.ForeColor = Color.FromArgb(248, 113, 113);
                lblRestoreEstado.Text = "Excepción inesperada.";
                MessageBox.Show($"Ocurrió un error inesperado al restaurar:\n{ex.Message}", "Error Inesperado", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetBusyState(false);
            }
        }

        private void SetBusyState(bool busy, string mensaje = "")
        {
            _isBusy = busy;
            this.Cursor = busy ? Cursors.WaitCursor : Cursors.Default;
            btnPeriodoDia.Enabled = !busy;
            btnPeriodoSemana.Enabled = !busy;
            btnPeriodoMes.Enabled = !busy;
            btnPeriodoAnio.Enabled = !busy;
            btnGuardarProgramacion.Enabled = !busy;
            btnGenerarBackup.Enabled = !busy;
            btnExaminarDestino.Enabled = !busy;
            btnRestaurarBackup.Enabled = !busy;
            btnExaminarOrigen.Enabled = !busy;
            btnCerrar.Enabled = !busy;

            if (busy)
            {
                if (mensaje.Contains("Generando") || mensaje.Contains("Guardando"))
                {
                    lblBackupEstado.ForeColor = Color.FromArgb(250, 204, 21);
                    lblBackupEstado.Text = mensaje;
                }
                if (mensaje.Contains("Restaurando"))
                {
                    lblRestoreEstado.ForeColor = Color.FromArgb(250, 204, 21);
                    lblRestoreEstado.Text = mensaje;
                }
            }
        }
    }
}
