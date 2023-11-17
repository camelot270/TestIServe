using TestIServe.Contracts.WeatherSensorEmulatorService;

namespace TestIServe.Server.Storage.Commands
{
    /// <inheritdoc />
    public class SensorsCommandService : ISensorsCommandService
    {
        private readonly IStorageContext _storageContext;

        public SensorsCommandService(IStorageContext storageContext)
        {
            _storageContext = storageContext;
        }

        /// <inheritdoc />
        public void SetAggregatedSensorData(string sensorId, long timeSlice, AggregatedSensorData aggregatedSensorData)
        {
            var res = _storageContext.SensorsData[sensorId].AddOrUpdate(
                timeSlice, 
                aggregatedSensorData, 
                (l, dto) => aggregatedSensorData);

        }

    }
}
