namespace TestIServe.Server.Services
{
    /// <summary>
    /// Интефейс генерации событий датчиков
    /// </summary>
    public interface ISensorDataGenerationService : IDisposable
    {
        /// <summary>
        /// Запустить процесс генерации событий датчиков
        /// </summary>
        void RunProcessGenerateData();
    }
}
