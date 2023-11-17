using System.Collections.Concurrent;
using TestIServe.Contracts.WeatherSensorEmulatorService;

namespace TestIServe.Server.Storage
{
    /// <inheritdoc />
    public class StorageContext : IStorageContext
    {
        /// <inheritdoc />
        public Dictionary<string, ConcurrentDictionary<long, AggregatedSensorData>> SensorsData { get; set; } = new();
    }
}
