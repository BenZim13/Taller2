using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StockOS.Domain.Enums;
using StockOS.Domain.Interfaces;

namespace StockOS.DataAccess.Services
{
    /// <summary>
    /// Implementación del servicio de copia de seguridad por períodos (Día, Semana, Mes, Año)
    /// bajo el esquema de rotación GFS (Grandfather-Father-Son) y restauración para SQL Server.
    /// </summary>
    public class BackupService : IBackupService
    {
        private readonly string _connectionString;
        private readonly IAuthorizationService? _authService;
        private readonly ILogger<BackupService>? _logger;
        private readonly string _rutaConfigProgramacion;

        public BackupService(IConfiguration configuration, IAuthorizationService? authService = null, ILogger<BackupService>? logger = null)
        {
            _connectionString = configuration.GetConnectionString("StockOS")
                ?? "Server=localhost;Database=StockOS;Trusted_Connection=True;TrustServerCertificate=True;";
            _authService = authService;
            _logger = logger;
            _rutaConfigProgramacion = Path.Combine(AppContext.BaseDirectory, "backup_schedule.json");
        }

        private void ValidarPermisoAdministrador()
        {
            if (_authService == null)
            {
                throw new UnauthorizedAccessException("Operación denegada. No se pudo verificar el permiso de Administrador.");
            }
            _authService.ValidarPermiso(Permisos.CONFIGURACION_GESTIONAR);
        }

        public string ObtenerDirectorioBackupsPorDefecto()
        {
            try
            {
                using var conexion = new SqlConnection(_connectionString);
                conexion.Open();
                using var comando = new SqlCommand("SELECT CAST(SERVERPROPERTY('InstanceDefaultBackupPath') AS NVARCHAR(512))", conexion);
                var resultado = comando.ExecuteScalar()?.ToString();

                if (!string.IsNullOrWhiteSpace(resultado) && Directory.Exists(resultado) && TienePermisosDeEscritura(resultado))
                {
                    return resultado;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "No se pudo consultar InstanceDefaultBackupPath en SQL Server o no tiene permisos. Se usará ruta alternativa.");
            }

            string rutaLocal = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "StockOS", "Backups");
            AsegurarDirectorio(rutaLocal);
            return rutaLocal;
        }

        private static bool TienePermisosDeEscritura(string ruta)
        {
            try
            {
                if (!Directory.Exists(ruta)) return false;
                string archivoPrueba = Path.Combine(ruta, $".stockos_write_test_{Guid.NewGuid():N}.tmp");
                File.WriteAllText(archivoPrueba, "test");
                File.Delete(archivoPrueba);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void AsegurarDirectorio(string ruta)
        {
            try
            {
                if (!Directory.Exists(ruta))
                {
                    Directory.CreateDirectory(ruta);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "No se pudo crear el directorio con .NET: {Ruta}", ruta);
            }
        }

        /// <summary>
        /// Obtiene los días de retención según la regla de negocio del período seleccionado (Esquema GFS).
        /// </summary>
        private static int ObtenerDiasRetencionPorPeriodo(PeriodoBackup periodo)
        {
            return periodo switch
            {
                PeriodoBackup.Diario => 7,       // Conserva los últimos 7 días
                PeriodoBackup.Semanal => 30,     // Conserva las últimas 4 semanas (~30 días)
                PeriodoBackup.Mensual => 365,    // Conserva los últimos 12 meses (1 año)
                PeriodoBackup.Anual => 1825,     // Conserva los últimos 5 años (auditoría legal/contable)
                _ => 30
            };
        }

        /// <summary>
        /// Calcula la próxima fecha y hora de ejecución según el período elegido.
        /// </summary>
        public static DateTime CalcularProximaEjecucion(PeriodoBackup periodo, string horaEjecucion, DateTime? baseTime = null)
        {
            DateTime referencia = baseTime ?? DateTime.Now;
            if (!TimeSpan.TryParse(horaEjecucion, out TimeSpan hora))
            {
                hora = new TimeSpan(23, 0, 0);
            }

            DateTime fechaObjetivo = periodo switch
            {
                PeriodoBackup.Diario => referencia.Date.AddDays(1).Add(hora),
                PeriodoBackup.Semanal => referencia.Date.AddDays(7).Add(hora),
                PeriodoBackup.Mensual => referencia.Date.AddMonths(1).Add(hora),
                PeriodoBackup.Anual => referencia.Date.AddYears(1).Add(hora),
                _ => referencia.Date.AddDays(1).Add(hora)
            };

            return fechaObjetivo;
        }

        public async Task<(bool Exito, string Mensaje, string? RutaArchivo)> CrearBackupAsync(
            string? rutaDestinoPersonalizada = null,
            PeriodoBackup periodo = PeriodoBackup.Diario)
        {
            ValidarPermisoAdministrador();
            return await EjecutarBackupInternoAsync(rutaDestinoPersonalizada, periodo);
        }

        private async Task<(bool Exito, string Mensaje, string? RutaArchivo)> EjecutarBackupInternoAsync(
            string? rutaDestinoPersonalizada = null,
            PeriodoBackup periodo = PeriodoBackup.Diario)
        {
            return await Task.Run(async () =>
            {
                string? archivoStaging = null;
                try
                {
                    var builder = new SqlConnectionStringBuilder(_connectionString);
                    string dbName = string.IsNullOrWhiteSpace(builder.InitialCatalog) ? "StockOS" : builder.InitialCatalog;
                    string defaultBackupDir = ObtenerDirectorioBackupsPorDefecto();

                    string etiquetaPeriodo = periodo.ToString(); // Diario, Semanal, Mensual, Anual
                    int diasRetencion = ObtenerDiasRetencionPorPeriodo(periodo);
                    string nombreArchivo = $"{dbName}_{etiquetaPeriodo}_{DateTime.Now:yyyyMMdd_HHmmss}.bak";

                    string directorioRaizDestino = string.IsNullOrWhiteSpace(rutaDestinoPersonalizada)
                        ? defaultBackupDir
                        : rutaDestinoPersonalizada;

                    string directorioPeriodoDestino = Path.Combine(directorioRaizDestino, etiquetaPeriodo);
                    AsegurarDirectorio(directorioPeriodoDestino);

                    string rutaFinal = Path.Combine(directorioPeriodoDestino, nombreArchivo);

                    _logger?.LogInformation(
                        "Iniciando copia de seguridad [{Periodo}] de la base de datos '{Db}' en '{Ruta}'",
                        etiquetaPeriodo, dbName, rutaFinal);

                    // 1. Ejecutar comando T-SQL nativo de Backup con compresión y descripción del período
                    string descripcionBackup = $"StockOS Backup {etiquetaPeriodo} - Retencion {diasRetencion} dias";
                    string sqlBackup = $@"
                        BACKUP DATABASE [{dbName}]
                        TO DISK = @RutaBackup
                        WITH FORMAT, INIT, COMPRESSION, 
                             NAME = @NombreBackup,
                             DESCRIPTION = @DescripcionBackup,
                             STATS = 10;";

                    using (var conexion = new SqlConnection(_connectionString))
                    {
                        await conexion.OpenAsync();

                        // Asegurar con SQL Server que la subcarpeta existe en caso de que C# no haya tenido permisos
                        if (!Directory.Exists(directorioPeriodoDestino))
                        {
                            try
                            {
                                using var cmdSub = new SqlCommand("EXEC master.dbo.xp_create_subdir @dir", conexion);
                                cmdSub.Parameters.AddWithValue("@dir", directorioPeriodoDestino);
                                await cmdSub.ExecuteNonQueryAsync();
                            }
                            catch { }
                        }

                        bool backupDirectoExitoso = false;
                        try
                        {
                            using var comando = new SqlCommand(sqlBackup, conexion);
                            comando.CommandTimeout = 600;
                            comando.Parameters.AddWithValue("@RutaBackup", rutaFinal);
                            comando.Parameters.AddWithValue("@NombreBackup", $"{dbName}-{etiquetaPeriodo}");
                            comando.Parameters.AddWithValue("@DescripcionBackup", descripcionBackup);
                            await comando.ExecuteNonQueryAsync();
                            backupDirectoExitoso = true;
                        }
                        catch (SqlException exSql)
                        {
                            _logger?.LogWarning(exSql, "SQL Server no pudo escribir directamente en '{RutaFinal}'. Se intentará vía staging en '{StagingDir}'.", rutaFinal, defaultBackupDir);
                        }

                        // Si el backup directo falló (por ejemplo, por permisos en una ruta de red o carpeta personal),
                        // realizamos el backup en el área de staging nativa de SQL Server y luego lo copiamos
                        if (!backupDirectoExitoso)
                        {
                            archivoStaging = Path.Combine(defaultBackupDir, nombreArchivo);
                            using var comandoStaging = new SqlCommand(sqlBackup, conexion);
                            comandoStaging.CommandTimeout = 600;
                            comandoStaging.Parameters.AddWithValue("@RutaBackup", archivoStaging);
                            comandoStaging.Parameters.AddWithValue("@NombreBackup", $"{dbName}-{etiquetaPeriodo}");
                            comandoStaging.Parameters.AddWithValue("@DescripcionBackup", descripcionBackup);
                            await comandoStaging.ExecuteNonQueryAsync();

                            File.Copy(archivoStaging, rutaFinal, overwrite: true);
                            try { File.Delete(archivoStaging); } catch { /* Ignorar si está en uso */ }
                        }
                    }

                    // 2. Aplicar regla de negocio de retención (Purga de backups vencidos del mismo período)
                    int archivosEliminados = AplicarPoliticaRetencion(directorioPeriodoDestino, dbName, etiquetaPeriodo, diasRetencion);

                    // 3. Actualizar registro si existe programación activa
                    ActualizarRegistroUltimaEjecucion(periodo);

                    _logger?.LogInformation(
                        "Copia de seguridad [{Periodo}] completada exitosamente en '{RutaFinal}'. Backups expirados depurados: {Eliminados}",
                        etiquetaPeriodo, rutaFinal, archivosEliminados);

                    string mensajeExito =
                        $"¡Copia de seguridad [{etiquetaPeriodo.ToUpper()}] generada con éxito!\n\n" +
                        $"• Archivo: {nombreArchivo}\n" +
                        $"• Carpeta del período: {directorioPeriodoDestino}\n" +
                        $"• Política de retención aplicada: {diasRetencion} días";

                    return (true, mensajeExito, (string?)rutaFinal);
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "Fallo al generar copia de seguridad de la base de datos.");
                    return (false, $"Ocurrió un error al generar la copia de seguridad:\n{ex.Message}", (string?)null);
                }
            });
        }

        /// <summary>
        /// Depura los archivos .bak del directorio del período que superen la antigüedad permitida por la regla de negocio.
        /// </summary>
        private int AplicarPoliticaRetencion(string directorioPeriodo, string dbName, string etiquetaPeriodo, int diasRetencion)
        {
            int eliminados = 0;
            try
            {
                DateTime fechaLimite = DateTime.Now.AddDays(-diasRetencion);
                var dirInfo = new DirectoryInfo(directorioPeriodo);
                if (!dirInfo.Exists) return 0;

                var archivosAntiguos = dirInfo.GetFiles($"{dbName}_{etiquetaPeriodo}_*.bak")
                    .Where(f => f.CreationTime < fechaLimite)
                    .ToList();

                foreach (var archivo in archivosAntiguos)
                {
                    try
                    {
                        archivo.Delete();
                        eliminados++;
                    }
                    catch (Exception exDel)
                    {
                        _logger?.LogWarning(exDel, "No se pudo eliminar el backup expirado: {Archivo}", archivo.FullName);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "Error al aplicar política de retención en {Directorio}", directorioPeriodo);
            }
            return eliminados;
        }

        public ConfiguracionBackupPeriodico ObtenerProgramacionPeriodica()
        {
            try
            {
                if (File.Exists(_rutaConfigProgramacion))
                {
                    string json = File.ReadAllText(_rutaConfigProgramacion);
                    var config = JsonSerializer.Deserialize<ConfiguracionBackupPeriodico>(json);
                    if (config != null)
                    {
                        if (string.IsNullOrWhiteSpace(config.RutaDestino))
                        {
                            config.RutaDestino = ObtenerDirectorioBackupsPorDefecto();
                        }
                        return config;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "No se pudo leer la configuración de backups programados.");
            }

            return new ConfiguracionBackupPeriodico
            {
                Habilitado = false,
                Periodo = PeriodoBackup.Diario,
                HoraEjecucion = "23:00",
                RutaDestino = ObtenerDirectorioBackupsPorDefecto()
            };
        }

        public async Task<(bool Exito, string Mensaje)> GuardarProgramacionPeriodicaAsync(ConfiguracionBackupPeriodico config)
        {
            ValidarPermisoAdministrador();

            return await Task.Run(() =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(config.RutaDestino))
                    {
                        config.RutaDestino = ObtenerDirectorioBackupsPorDefecto();
                    }

                    if (config.Habilitado)
                    {
                        config.ProximaEjecucion = CalcularProximaEjecucion(config.Periodo, config.HoraEjecucion, config.UltimaEjecucion);
                    }
                    else
                    {
                        config.ProximaEjecucion = null;
                    }

                    var opcionesJson = new JsonSerializerOptions { WriteIndented = true };
                    string json = JsonSerializer.Serialize(config, opcionesJson);
                    File.WriteAllText(_rutaConfigProgramacion, json);

                    // Intentar sincronizar con el Programador de Tareas de Windows (schtasks) como respaldo del SO
                    RegistrarTareaEnWindowsTaskScheduler(config);

                    if (!config.Habilitado)
                    {
                        _logger?.LogInformation("Programación automática de backups desactivada por el usuario.");
                        return (true, "La programación automática de copias de seguridad ha sido desactivada.");
                    }

                    string nombrePeriodo = config.Periodo switch
                    {
                        PeriodoBackup.Diario => "POR DÍA (Diario - Retención 7 días)",
                        PeriodoBackup.Semanal => "POR SEMANA (Semanal - Retención 4 semanas)",
                        PeriodoBackup.Mensual => "POR MES (Mensual - Retención 12 meses)",
                        PeriodoBackup.Anual => "POR AÑO (Anual - Retención 5 años)",
                        _ => config.Periodo.ToString()
                    };

                    _logger?.LogInformation(
                        "Programación de backup guardada: Período={Periodo}, Hora={Hora}, Próxima={Proxima}",
                        config.Periodo, config.HoraEjecucion, config.ProximaEjecucion);

                    return (true,
                        $"¡Programación automática configurada con éxito!\n\n" +
                        $"• Frecuencia: {nombrePeriodo}\n" +
                        $"• Hora programada: {config.HoraEjecucion} hs\n" +
                        $"• Próxima ejecución: {config.ProximaEjecucion:dd/MM/yyyy HH:mm}\n" +
                        $"• Destino: {Path.Combine(config.RutaDestino, config.Periodo.ToString())}");
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "Error al guardar la programación periódica de backups.");
                    return (false, $"No se pudo guardar la programación:\n{ex.Message}");
                }
            });
        }

        private void ActualizarRegistroUltimaEjecucion(PeriodoBackup periodoEjecutado)
        {
            try
            {
                if (!File.Exists(_rutaConfigProgramacion)) return;
                var config = ObtenerProgramacionPeriodica();
                if (config.Periodo == periodoEjecutado)
                {
                    config.UltimaEjecucion = DateTime.Now;
                    if (config.Habilitado)
                    {
                        config.ProximaEjecucion = CalcularProximaEjecucion(config.Periodo, config.HoraEjecucion, config.UltimaEjecucion);
                    }
                    string json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(_rutaConfigProgramacion, json);
                }
            }
            catch
            {
                // Ignorar errores secundarios de actualización de estado
            }
        }

        public async Task VerificarYEjecutarBackupPendienteAsync()
        {
            try
            {
                var config = ObtenerProgramacionPeriodica();
                if (!config.Habilitado || config.ProximaEjecucion == null) return;

                if (DateTime.Now >= config.ProximaEjecucion.Value)
                {
                    _logger?.LogInformation("Ejecutando backup periódico programado pendiente [{Periodo}]...", config.Periodo);
                    await EjecutarBackupInternoAsync(config.RutaDestino, config.Periodo);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "Error al verificar o ejecutar backup programado pendiente.");
            }
        }

        private void RegistrarTareaEnWindowsTaskScheduler(ConfiguracionBackupPeriodico config)
        {
            try
            {
                const string taskName = "StockOS_Backup_Automatico";

                if (!config.Habilitado)
                {
                    EjecutarComandoSilencioso("schtasks", $"/Delete /TN \"{taskName}\" /F");
                    return;
                }

                string frecuenciaSchTasks = config.Periodo switch
                {
                    PeriodoBackup.Diario => "/SC DAILY",
                    PeriodoBackup.Semanal => "/SC WEEKLY",
                    PeriodoBackup.Mensual => "/SC MONTHLY",
                    PeriodoBackup.Anual => "/SC MONTHLY /MO 12",
                    _ => "/SC DAILY"
                };

                int diasRetencion = ObtenerDiasRetencionPorPeriodo(config.Periodo);
                string dirPeriodo = Path.Combine(config.RutaDestino, config.Periodo.ToString());

                // Buscar script tanto en carpeta de despliegue como en árbol de desarrollo
                string scriptPath = Path.Combine(AppContext.BaseDirectory, "DatabaseScripts", "Backup_Programado_StockOS.ps1");
                if (!File.Exists(scriptPath))
                {
                    scriptPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\..\DatabaseScripts\Backup_Programado_StockOS.ps1"));
                }

                if (File.Exists(scriptPath))
                {
                    string trCommand = $"powershell.exe -NoProfile -ExecutionPolicy Bypass -File \\\"{scriptPath}\\\" -DirectorioBackups \\\"{dirPeriodo}\\\" -Periodo \\\"{config.Periodo}\\\" -DiasRetencion {diasRetencion}";
                    string args = $"/Create /TN \"{taskName}\" {frecuenciaSchTasks} /ST {config.HoraEjecucion} /TR \"{trCommand}\" /F";
                    EjecutarComandoSilencioso("schtasks", args);
                }
            }
            catch
            {
                // Si el usuario de Windows no tiene permisos elevados para schtasks, el planificador interno de StockOS sigue operando
            }
        }

        private static void EjecutarComandoSilencioso(string archivo, string argumentos)
        {
            using var proceso = new Process();
            proceso.StartInfo = new ProcessStartInfo
            {
                FileName = archivo,
                Arguments = argumentos,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            proceso.Start();
            proceso.WaitForExit(3000);
        }

        public async Task<(bool Exito, string Mensaje)> RestaurarBackupAsync(string rutaArchivoBak)
        {
            ValidarPermisoAdministrador();

            return await Task.Run(async () =>
            {
                if (string.IsNullOrWhiteSpace(rutaArchivoBak) || !File.Exists(rutaArchivoBak))
                {
                    return (false, "El archivo de copia de seguridad seleccionado no existe o la ruta es inválida.");
                }

                string archivoParaRestaurar = rutaArchivoBak;
                bool archivoCopiadoAStaging = false;

                try
                {
                    var builder = new SqlConnectionStringBuilder(_connectionString);
                    string dbName = string.IsNullOrWhiteSpace(builder.InitialCatalog) ? "StockOS" : builder.InitialCatalog;
                    string defaultDir = ObtenerDirectorioBackupsPorDefecto();

                    if (!rutaArchivoBak.StartsWith(defaultDir, StringComparison.OrdinalIgnoreCase))
                    {
                        string stagingBak = Path.Combine(defaultDir, $"RestoreStaging_{DateTime.Now:yyyyMMdd_HHmmss}.bak");
                        File.Copy(rutaArchivoBak, stagingBak, overwrite: true);
                        archivoParaRestaurar = stagingBak;
                        archivoCopiadoAStaging = true;
                    }

                    _logger?.LogWarning("Iniciando restauración de la base de datos '{Db}' desde '{Ruta}'", dbName, archivoParaRestaurar);

                    var masterBuilder = new SqlConnectionStringBuilder(_connectionString)
                    {
                        InitialCatalog = "master"
                    };

                    SqlConnection.ClearAllPools();

                    string sqlRestore = $@"
                        ALTER DATABASE [{dbName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                        RESTORE DATABASE [{dbName}] FROM DISK = @RutaArchivo WITH REPLACE;
                        ALTER DATABASE [{dbName}] SET MULTI_USER;";

                    using (var conexion = new SqlConnection(masterBuilder.ConnectionString))
                    {
                        await conexion.OpenAsync();
                        using var comando = new SqlCommand(sqlRestore, conexion);
                        comando.CommandTimeout = 600;
                        comando.Parameters.AddWithValue("@RutaArchivo", archivoParaRestaurar);
                        await comando.ExecuteNonQueryAsync();
                    }

                    _logger?.LogInformation("Base de datos '{Db}' restaurada exitosamente.", dbName);
                    return (true, "¡Base de datos restaurada con éxito!\nSe han restablecido todas las tablas y datos a la versión del backup.");
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "Error al restaurar la base de datos.");

                    try
                    {
                        var builder = new SqlConnectionStringBuilder(_connectionString) { InitialCatalog = "master" };
                        string dbName = string.IsNullOrWhiteSpace(builder.InitialCatalog) ? "StockOS" : builder.InitialCatalog;
                        using var conexionFallback = new SqlConnection(builder.ConnectionString);
                        await conexionFallback.OpenAsync();
                        using var cmdFallback = new SqlCommand($"ALTER DATABASE [{dbName}] SET MULTI_USER;", conexionFallback);
                        await cmdFallback.ExecuteNonQueryAsync();
                    }
                    catch
                    {
                        // Fallback silencioso
                    }

                    return (false, $"Error crítico durante la restauración de la base de datos:\n{ex.Message}");
                }
                finally
                {
                    if (archivoCopiadoAStaging && File.Exists(archivoParaRestaurar))
                    {
                        try { File.Delete(archivoParaRestaurar); } catch { /* ignore */ }
                    }
                }
            });
        }
    }
}
