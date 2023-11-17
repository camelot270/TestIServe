namespace TestIServe.Client.Services.WeatherSensorEmulatorClientService
{
    /// <inheritdoc />
    internal class WeatherSensorEmulatorClientServiceFactory : IWeatherSensorEmulatorClientServiceFactory
    {
        /// <inheritdoc />
        public IWeatherSensorEmulatorClientService Create(string clientId)
        {
            return new WeatherSensorEmulatorClientService(clientId);
        }
    }
}
