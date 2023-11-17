namespace TestIServe.Server.Sensors.Domain
{
    /// <summary>
    /// Интерфейс репозитория датчиков
    /// </summary>
    public interface ISensorRepository : IDisposable
    {
        /// <summary>
        /// Списк подключеных датчиков
        /// </summary>
        Dictionary<string, ISensor> Sensors { get; }

        /// <summary>
        /// Добавить датчик в репозиторий
        /// </summary>
        /// <param name="idSensor"> уникальный идентификаотр датчика </param>
        /// <param name="sensor"> датчик </param>
        /// <returns></returns>
        ISensor CreateSensor(string idSensor, ISensor sensor);

        /// <summary>
        /// Удалить датчик из репозитория
        /// </summary>
        /// <param name="idSensor"> уникальный идентификаотр датчика </param>
        void RemoveSensor(string idSensor);
    }
}
