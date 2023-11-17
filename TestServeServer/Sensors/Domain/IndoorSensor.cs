using TestIServe.Contracts.WeatherSensorEmulatorService;

namespace TestIServe.Server.Sensors.Domain
{
    public class IndoorSensor : ISensor
    {
        private float _temperature;
        private float _humidifier;
        private float _carbonDioxideContent;
        private string _guid;

        public event Action<SensorDto>? SensorDataChanged;

        public IndoorSensor(string guid)
        {
            _guid = guid;
        }

        public string GetGuid()
        {
            return _guid;
        }

        public float GetTemperature()
        {
            return _temperature;
        }

        public float GetHumidifier()
        {
            return _humidifier;
        }

        public float GetCarbonDioxideContent()
        {
            return _carbonDioxideContent;
        }

        public void SetSensorData(float temperature, float humidifier, float carbonDioxideContent)
        {
            _temperature = temperature;
            _humidifier = humidifier;
            _carbonDioxideContent = carbonDioxideContent;

            SensorDataChanged?.Invoke(
                new SensorDto()
                {
                    CarbonDioxideContent = carbonDioxideContent,
                    Humidifier = humidifier,
                    SensorId = _guid,
                    Temperature = temperature,
                    Time = DateTime.UtcNow.ToFileTimeUtc()
                });
        }
    }
}
