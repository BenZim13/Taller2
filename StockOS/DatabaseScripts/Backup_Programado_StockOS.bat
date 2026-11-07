@echo off
setlocal
echo ========================================================
echo         StockOS - Copia de Seguridad Automatizada
echo ========================================================

powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0Backup_Programado_StockOS.ps1"

if %ERRORLEVEL% EQU 0 (
    echo.
    echo Respaldo finalizado correctamente.
) else (
    echo.
    echo [ERROR] Ocurrio un problema al ejecutar el respaldo.
)
timeout /t 5

