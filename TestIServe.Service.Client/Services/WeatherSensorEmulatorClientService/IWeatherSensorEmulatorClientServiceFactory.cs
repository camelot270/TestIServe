namespace TestIServe.Client.Services.WeatherSensorEmulatorClientService
{
    /// <summary>
    /// Интерфейс фабрики для создания сервиса погодных датчиков
    /// </summary>
    public interface IWeatherSensorEmulatorClientServiceFactory
    {
        /// <summary>
        /// Создание сервиса погодных датчиков
        /// </summary>
        /// <param name="clientId"> Идентификатор клиента </param>
        /// <returns></returns>
        IWeatherSensorEmulatorClientService Create(string clientId);
    }
}
