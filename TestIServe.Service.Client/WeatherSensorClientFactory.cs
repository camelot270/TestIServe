using TestIServe.Client.Services.WeatherSensorEmulatorClientService;

namespace TestIServe.Client
{
    /// <inheritdoc />
    public class WeatherSensorClientFactory : IWeatherSensorClientFactory
    {
        private readonly IWeatherSensorEmulatorClientServiceFactory _weatherSensorEmulatorClientServiceFactory;

        public WeatherSensorClientFactory(IWeatherSensorEmulatorClientServiceFactory weatherSensorEmulatorClientServiceFactory)
        {
            _weatherSensorEmulatorClientServiceFactory = weatherSensorEmulatorClientServiceFactory;
        }

        /// <inheritdoc />
        public WeatherSensorClient Create(string serverAddress)
        {
            return new WeatherSensorClient(_weatherSensorEmulatorClientServiceFactory, serverAddress);
        }

    }
}
