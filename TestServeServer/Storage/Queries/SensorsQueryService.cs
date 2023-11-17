using TestIServe.Contracts.WeatherSensorEmulatorService;

namespace TestIServe.Server.Storage.Queries
{
    /// <inheritdoc />
    public class SensorsQueryService : ISensorsQueryService
    {
        private readonly IStorageContext _storageContext;

        public SensorsQueryService( IStorageContext storageContext)
        {
            _storageContext = storageContext;
        }

        /// <inheritdoc />
        public float? GetAverageTemperature(string sensorId, DateTimeOffset fromTime, int countMinutes)
        {
            var sensorDtos = GetSensorDataByRange(sensorId, fromTime, countMinutes);

            if (sensorDtos == null)
            {
                return null;
            }

            var res = sensorDtos.Average(x => x.TemperatureAverage);

            return res;
        }

        /// <inheritdoc />
        public float? GetAverageHumidifier(string sensorId, DateTimeOffset fromTime, int countMinutes)
        {
            var sensorDtos = GetSensorDataByRange(sensorId, fromTime, countMinutes);

            if (sensorDtos == null)
            {
                return null;
            }

            var res = sensorDtos.Average(x => x.HumidifierAverage);

            return res;
        }

        /// <inheritdoc />
        public float? GetMinCarbonDioxideContent(string sensorId, DateTimeOffset fromTime, int countMinutes)
        {
            var sensorDtos = GetSensorDataByRange(sensorId, fromTime, countMinutes);

            if (sensorDtos == null)
            {
                return null;
            }

            var res = sensorDtos.Min(x => x.CarbonDioxideContentMin);

            return res;
        }

        /// <inheritdoc />
        public float? GetMaxCarbonDioxideContent(string sensorId, DateTimeOffset fromTime, int countMinutes)
        {
            var sensorDtos = GetSensorDataByRange(sensorId, fromTime, countMinutes);

            if (sensorDtos == null)
            {
                return null;
            }

            var res = sensorDtos.Max(x => x.CarbonDioxideContentMax);

            return res;
        }

        public IEnumerable<AggregatedSensorData>? GetSensorDataByRange(string numSensor, DateTimeOffset fromTime, int countMinutes)
        {
            var dateTimeInTicksFrom = fromTime.UtcDateTime.ToFileTimeUtc();
            var dateTimeInTicksTo = fromTime.UtcDateTime.ToFileTimeUtc() + countMinutes * TimeSpan.TicksPerMinute;
            var sensorsInRange = _storageContext.SensorsData[numSensor]
                .Where(x => x.Key >= dateTimeInTicksFrom
                            && x.Key <= dateTimeInTicksTo)
                .Select(x => x.Value);

            var sensorDatabyRange = sensorsInRange.ToList();
            if (!sensorDatabyRange.Any())
            {
                return null;
            }

            return sensorDatabyRange;

        }

        /// <inheritdoc />
        public AggregatedSensorData? GetSensorDataByTimeSlice(string sensorId, long timeSlice)
        {
            _storageContext.SensorsData[sensorId].TryGetValue(timeSlice, out var lastDataSensor);

            return lastDataSensor;
        }

        /// <inheritdoc />
        public IEnumerable<AggregatedSensorDataSlice?> GetAllAggregatedSensorData()
        {
            foreach (var sensorData in _storageContext.SensorsData)
            {
                foreach (var data in sensorData.Value)
                {
                    yield return  new AggregatedSensorDataSlice()
                    {
                        AggreagateSensorData = data.Value, 
                        Id = sensorData.Key, 
                        SliceTime = data.Key
                    };
                }
            }
        }

    }
}
