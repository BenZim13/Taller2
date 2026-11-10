<#
.SYNOPSIS
    Script automatizado de respaldo para la base de datos StockOS (SQL Server).
.DESCRIPTION
    Genera una copia de seguridad comprimida (.bak) con timestamp y elimina
    automáticamente los respaldos que superen los días de retención especificados.
#>

param(
    [string]$Servidor = "localhost",
    [string]$BaseDatos = "StockOS",
    [string]$DirectorioBackups = "C:\StockOS\Backups",
    [string]$Periodo = "Diario",
    [int]$DiasRetencion = 30
)

$ErrorActionPreference = "Stop"

try {
    # 1. Asegurar existencia del directorio de destino
    if (-not (Test-Path -Path $DirectorioBackups)) {
        New-Item -ItemType Directory -Path $DirectorioBackups -Force | Out-Null
    }

    $FechaHora = Get-Date -Format "yyyyMMdd_HHmmss"
    $NombreArchivo = "${BaseDatos}_${Periodo}_${FechaHora}.bak"
    $RutaCompleta = Join-Path $DirectorioBackups $NombreArchivo

    Write-Host "[$(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')] Iniciando backup [$Periodo] de la base de datos '$BaseDatos' en '$RutaCompleta'..." -ForegroundColor Cyan

    # 2. Comando T-SQL con compresión y formato
    $Query = @"
BACKUP DATABASE [$BaseDatos] 
TO DISK = N'$RutaCompleta' 
WITH FORMAT, INIT, COMPRESSION, STATS = 10;
"@

    # 3. Ejecutar mediante sqlcmd o dbatools
    $SqlCmdResult = sqlcmd -S $Servidor -E -Q $Query
    Write-Host $SqlCmdResult

    if (Test-Path -Path $RutaCompleta) {
        $TamanioMB = [Math]::Round(((Get-Item $RutaCompleta).Length / 1MB), 2)
        Write-Host "[$(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')] ¡Backup completado con éxito! Tamaño: $TamanioMB MB" -ForegroundColor Green
    } else {
        throw "El archivo de respaldo no fue generado en la ruta esperada."
    }

    # 4. Limpieza de respaldos antiguos (Política de retención)
    Write-Host "[$(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')] Limpiando backups de más de $DiasRetencion días..." -ForegroundColor Yellow
    $FechaLimite = (Get-Date).AddDays(-$DiasRetencion)
    Get-ChildItem -Path $DirectorioBackups -Filter "${BaseDatos}_${Periodo}_*.bak" | Where-Object { $_.CreationTime -lt $FechaLimite } | ForEach-Object {
        Write-Host "Eliminando backup expirado: $($_.FullName)" -ForegroundColor DarkGray
        Remove-Item $_.FullName -Force
    }

    Write-Host "[$(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')] Proceso de respaldo finalizado correctamente." -ForegroundColor Green
}
catch {
    Write-Error "[$(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')] ERROR al realizar el backup: $_"
    exit 1
}

