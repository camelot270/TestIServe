using TestIServe.Contracts.WeatherSensorEmulatorService;

namespace TestIServe.Server.Sensors.Domain
{
    /// <summary>
    /// Интерфейс датчика
    /// </summary>
    public interface ISensor
    {
        /// <summary>
        /// Получить уникальный идентификатор
        /// </summary>
        /// <returns> уникальный идентификатор </returns>
        string GetGuid();

        /// <summary>
        /// Получить значение температуры датчика
        /// </summary>
        /// <returns>Значение температуры датчика</returns>
        float GetTemperature();

        /// <summary>
        /// Получить значение влажности датчика
        /// </summary>
        /// <returns>Значение влажности датчика</returns>
        float GetHumidifier();

        /// <summary>
        /// Получить значение CO2 датчика
        /// </summary>
        /// <returns>Значение CO2 датчика</returns>
        float GetCarbonDioxideContent();

        /// <summary>
        /// Сохранить новые значения датчика
        /// </summary>
        /// <param name="temperature"> температура </param>
        /// <param name="humidifier"> влажность </param>
        /// <param name="carbonDioxideContent"> CO2 </param>
        void SetSensorData(float temperature, float humidifier, float carbonDioxideContent);

        /// <summary>
        /// Событие обновления значений датчиков
        /// </summary>
        public event Action<SensorDto> SensorDataChanged;

    }
}
