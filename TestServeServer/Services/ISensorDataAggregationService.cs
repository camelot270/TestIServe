namespace TestIServe.Server.Services;

/// <summary>
/// Интефейс сервиса агрегации
/// </summary>
public interface ISensorDataAggregationService : IDisposable
{
    /// <summary>
    /// Запустить процесс агрегации
    /// </summary>
    void RunAggregateProcess();
}