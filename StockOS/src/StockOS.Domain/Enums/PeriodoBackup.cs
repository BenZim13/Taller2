namespace StockOS.Domain.Enums
{
    /// <summary>
    /// Define los períodos disponibles para la generación y programación
    /// de copias de seguridad bajo el esquema de rotación GFS (Grandfather-Father-Son).
    /// </summary>
    public enum PeriodoBackup
    {
        Diario = 1,
        Semanal = 2,
        Mensual = 3,
        Anual = 4
    }
}

