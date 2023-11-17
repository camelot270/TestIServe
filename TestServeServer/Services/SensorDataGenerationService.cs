using Google.Protobuf.Reflection;
using Microsoft.Extensions.Options;
using TestIServe.Server.Configurations;
using TestIServe.Server.Sensors.Domain;
using TestIServe.Server.Services.SensorValueGenerator;
using TestIServe.Server.Storage;

namespace TestIServe.Server.Services
{
    /// <inheritdoc />
    public class SensorDataGenerationService : ISensorDataGenerationService
    {
        private readonly ILogger<SensorDataGenerationService> _logger;
        private readonly ISensorRepository _sensorRepository;
        private readonly ISensorValueGenerator _sensorValueGenerator;

        private readonly int _intervalGenerationEvents = 2000;

        private Thread? _processGenerateDataThread;
        private readonly CancellationTokenSource _cancellationTokenSource = new();

        public SensorDataGenerationService(
            ILogger<SensorDataGenerationService> logger,
            ISensorRepository sensorRepository, 
            IOptions<GenerationEventsServiceOptions> serviceOptions,
            ISensorValueGenerator sensorValueGenerator)
        {
            _logger = logger;
            _sensorRepository = sensorRepository;
            _sensorValueGenerator = sensorValueGenerator;
            try
            {
                _intervalGenerationEvents = serviceOptions.Value.SensorEventIntervalInMs;

            }
            catch (OptionsValidationException optValEx)
            {
                _logger.LogError(optValEx, "Ошибка валидации параметра");
            }
        }

        private void GenerateData()
        {
            while (_cancellationTokenSource.IsCancellationRequested == false)
            {
                foreach (var sensor in _sensorRepository.Sensors)
                {
                    sensor.Value.SetSensorData(_sensorValueGenerator.GetGeneratedTemperature(), 
                        _sensorValueGenerator.GetGeneratedHumidifier(),
                        sensor.Value is StreetSensor ?
                            _sensorValueGenerator.GetGeneratedCarbonDioxideContentOnStreet() :
                            _sensorValueGenerator.GetGeneratedCarbonDioxideContentOnIndoor());
                }

                Thread.Sleep(_intervalGenerationEvents);
            }
        }

        /// <inheritdoc />
        public void RunProcessGenerateData()
        {
            _processGenerateDataThread = new Thread(GenerateData);
            _processGenerateDataThread.Start();
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            if (_processGenerateDataThread is not null)
            {
                if ((_processGenerateDataThread.ThreadState & ThreadState.Stopped) != 0)
                {
                    _processGenerateDataThread?.ExecutionContext?.Dispose();
                }
            }
        }
    }
}
