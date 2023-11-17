using System.Collections.Concurrent;
using TestIServe.Contracts.WeatherSensorEmulatorService;
using TestIServe.Server.Sensors.Domain;
using TestIServe.Server.Storage;

namespace TestIServe.Server.Sensors
{
    /// <inheritdoc />
    public class SensorsSetup: ISensorsSetup
    {
        private readonly ISensorRepository _sensorRepository;
        private readonly ISensorFactory _sensorFactory;
        private readonly IStorageContext _storageContext;

        public SensorsSetup(ISensorRepository sensorRepository, ISensorFactory sensorFactory, IStorageContext storageContext)
        {
            _sensorRepository = sensorRepository;
            _sensorFactory = sensorFactory;
            _storageContext = storageContext;
        }

        /// <inheritdoc />
        public void LoadSensors()
        {
            List<ISensor> sensors = new List<ISensor>();

            // Предполагается загрузка с конфигурации подключенных устройств
            string guid = "1";//Guid.NewGuid().ToString();
            sensors.Add(
                _sensorRepository.CreateSensor(
                    guid, 
                    _sensorFactory.CreateStreetSensor(guid)
                    )
                );

            guid = "2";//Guid.NewGuid().ToString();
            sensors.Add(
                _sensorRepository.CreateSensor(
                    guid, 
                    _sensorFactory.CreateIndoorSensor(guid)
                    )
                );

            foreach (var sensor in sensors)
            {
                _storageContext.SensorsData.Add(sensor.GetGuid(), new ConcurrentDictionary<long, AggregatedSensorData>());
            }
        }
    }
}
