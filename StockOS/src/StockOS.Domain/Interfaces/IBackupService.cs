using System;
using System.Threading.Tasks;
using StockOS.Domain.Enums;

namespace StockOS.Domain.Interfaces
{
    /// <summary>
    /// Modelo de configuración para la programación periódica de copias de seguridad.
    /// </summary>
    public class ConfiguracionBackupPeriodico
    {
        public bool Habilitado { get; set; } = false;
        public PeriodoBackup Periodo { get; set; } = PeriodoBackup.Diario;
        public string HoraEjecucion { get; set; } = "23:00";
        public string RutaDestino { get; set; } = string.Empty;
        public DateTime? UltimaEjecucion { get; set; }
        public DateTime? ProximaEjecucion { get; set; }
    }

    /// <summary>
    /// Contrato para la administración de copias de seguridad (Backup) por períodos
    /// (Día, Semana, Mes, Año) y restauración (Restore) de la base de datos SQL Server.
    /// </summary>
    public interface IBackupService
    {
        /// <summary>
        /// Genera una copia de seguridad completa (.bak) clasificada y retenida según el período indicado.
        /// </summary>
        /// <param name="rutaDestinoPersonalizada">Carpeta raíz destino opcional. Si es nula, utiliza la carpeta por defecto.</param>
        /// <param name="periodo">Período del respaldo (Diario, Semanal, Mensual, Anual).</param>
        /// <returns>Tupla con el resultado, mensaje descriptivo y la ruta final del archivo generado.</returns>
        Task<(bool Exito, string Mensaje, string? RutaArchivo)> CrearBackupAsync(
            string? rutaDestinoPersonalizada = null,
            PeriodoBackup periodo = PeriodoBackup.Diario);

        /// <summary>
        /// Restaura la base de datos StockOS a partir de un archivo de backup (.bak).
        /// </summary>
        /// <param name="rutaArchivoBak">Ruta absoluta del archivo .bak a restaurar.</param>
        /// <returns>Tupla con el resultado y mensaje descriptivo.</returns>
        Task<(bool Exito, string Mensaje)> RestaurarBackupAsync(string rutaArchivoBak);

        /// <summary>
        /// Obtiene el directorio por defecto configurado o sugerido para almacenar los backups.
        /// </summary>
        string ObtenerDirectorioBackupsPorDefecto();

        /// <summary>
        /// Guarda y registra la programación automática de backups según el período seleccionado (Día, Semana, Mes, Año).
        /// </summary>
        Task<(bool Exito, string Mensaje)> GuardarProgramacionPeriodicaAsync(ConfiguracionBackupPeriodico config);

        /// <summary>
        /// Obtiene la configuración actual de programación periódica de backups.
        /// </summary>
        ConfiguracionBackupPeriodico ObtenerProgramacionPeriodica();

        /// <summary>
        /// Verifica si existe un backup periódico pendiente de ejecución según el calendario y lo ejecuta.
        /// </summary>
        Task VerificarYEjecutarBackupPendienteAsync();
    }
}
