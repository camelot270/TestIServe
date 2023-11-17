namespace TestIServe.Server.Sensors.Domain
{
    public class SensorRepository : ISensorRepository
    {
        private readonly ILogger _logger;

        public SensorRepository(ILogger<SensorRepository> logger)
        {
            _logger = logger;
        }

        private readonly Dictionary<string, ISensor> _sensors = new();

        public Dictionary<string, ISensor> Sensors => _sensors;

        public ISensor CreateSensor(string idSensor, ISensor sensor)
        {
            Sensors.Add(idSensor, sensor);
            return sensor;
        }

        public void RemoveSensor(string idSensor)
        {
            Sensors.Remove(idSensor);
        }

        public void Dispose()
        {
            _sensors.Clear();
        }

    }
}
