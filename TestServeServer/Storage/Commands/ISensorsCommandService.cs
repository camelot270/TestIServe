using TestIServe.Contracts.WeatherSensorEmulatorService;

namespace TestIServe.Server.Storage.Commands
{
    /// <summary>
    /// Сервис для сохранения агрегированных данных 
    /// </summary>
    public interface ISensorsCommandService
    {
        /// <summary>
        /// Сохранить агрегированные данные
        /// </summary>
        /// <param name="sensorId"> уникальный идентификатор датчика</param>
        /// <param name="timeSlice"> время среза данных датчиков</param>
        /// <param name="aggregatedSensorData"> агрегированные данные</param>
        public void SetAggregatedSensorData(string sensorId, long timeSlice, AggregatedSensorData aggregatedSensorData);

    }
}
