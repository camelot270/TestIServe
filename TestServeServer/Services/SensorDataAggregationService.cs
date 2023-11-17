using Google.Protobuf.Reflection;
using Microsoft.Extensions.Options;
using TestIServe.Contracts.WeatherSensorEmulatorService;
using TestIServe.Server.Configurations;
using TestIServe.Server.Sensors.Domain;
using TestIServe.Server.Storage.Commands;
using TestIServe.Server.Storage.Queries;

namespace TestIServe.Server.Services
{
    /// <inheritdoc />
    public class SensorDataAggregationService : ISensorDataAggregationService
    {
        private readonly ILogger<SensorDataAggregationService> _logger;
        private readonly ISensorRepository _sensorRepository;
        private readonly ISensorsCommandService _sensorsCommandService;
        private readonly ISensorsQueryService _sensorsQueryService;

        private readonly uint _countMinutesToAggregate = 1;

        public SensorDataAggregationService(
            ILogger<SensorDataAggregationService> logger,
            ISensorRepository sensorRepository, 
            ISensorsCommandService sensorsCommandService,
            ISensorsQueryService sensorsQueryService,
            IOptions<SensorDataAggregationServiceOptions> serviceOptions
            )
        {
            _logger = logger;
            _sensorRepository = sensorRepository;
            _sensorsCommandService = sensorsCommandService;
            _sensorsQueryService = sensorsQueryService;

            try
            {
                _countMinutesToAggregate = serviceOptions.Value.CountMinutesToAggregate;
            }
            catch (OptionsValidationException optValEx)
            {
                _logger.LogError(optValEx, "Ошибка валидации параметра");
            }
        }

        /// <inheritdoc />
        public void RunAggregateProcess()
        {
            foreach (var sensor in _sensorRepository.Sensors)
            {
                sensor.Value.SensorDataChanged += ValueOnSensorDataChanged;
            }
        }

        private void ValueOnSensorDataChanged(SensorDto obj)
        {
            if (String.IsNullOrEmpty(obj.SensorId))
            {
                return;
            }

            long timeSlice = obj.Time - obj.Time % (TimeSpan.TicksPerMinute * _countMinutesToAggregate);

            var currentValueInTime = new AggregatedSensorData()
            {
                TemperatureAverage = obj.Temperature,
                HumidifierAverage = obj.Humidifier,
                CarbonDioxideContentMin = obj.CarbonDioxideContent,
                CarbonDioxideContentMax = obj.CarbonDioxideContent,
            };


            var lastDataSensor = _sensorsQueryService.GetSensorDataByTimeSlice(obj.SensorId, timeSlice);
            if (lastDataSensor != null)
            {
                currentValueInTime = new AggregatedSensorData()
                {
                    TemperatureAverage = (currentValueInTime.TemperatureAverage + lastDataSensor.TemperatureAverage) / 2,
                    HumidifierAverage = (currentValueInTime.HumidifierAverage + lastDataSensor.HumidifierAverage) / 2,
                    CarbonDioxideContentMin = Math.Min(currentValueInTime.CarbonDioxideContentMin, lastDataSensor.CarbonDioxideContentMin),
                    CarbonDioxideContentMax = Math.Max(currentValueInTime.CarbonDioxideContentMax, lastDataSensor.CarbonDioxideContentMax)
                };
            }

            _sensorsCommandService.SetAggregatedSensorData(obj.SensorId, timeSlice, currentValueInTime);
        }

        public void Dispose()
        {
            foreach (var sensor in _sensorRepository.Sensors)
            {
                sensor.Value.SensorDataChanged -= ValueOnSensorDataChanged;
            }
        }
    }
}
