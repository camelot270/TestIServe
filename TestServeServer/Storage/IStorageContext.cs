using System.Collections.Concurrent;
using TestIServe.Contracts.WeatherSensorEmulatorService;

namespace TestIServe.Server.Storage
{
    /// <summary>
    /// Хранилище агрегированных данных
    /// </summary>
    public interface IStorageContext
    {
        /// <summary>
        /// Агрегированные данные
        /// </summary>
        Dictionary<string, ConcurrentDictionary<long, AggregatedSensorData>> SensorsData { get;}
    }
}
