
using TestIServe.Contracts.WeatherSensorEmulatorService;

namespace TestIServe.Server.Storage.Queries;

/// <summary>
/// Сервис для получения сохраненных агрегированных данных 
/// </summary>
public interface ISensorsQueryService
{
    /// <summary>
    /// Получить среднюю температуру за промежуток времени
    /// </summary>
    /// <param name="sensorId"> уникальный идентификатор датчика </param>
    /// <param name="fromTime"> время с какого момента считывать</param>
    /// <param name="countMinutes"> количество минут для чтения</param>
    /// <returns> средняя температуру за промежуток времени</returns>
    public float? GetAverageTemperature(string sensorId, DateTimeOffset fromTime, int countMinutes);

    /// <summary>
    /// Получить среднюю влажность за промежуток времени
    /// </summary>
    /// <param name="sensorId"> уникальный идентификатор датчика </param>
    /// <param name="fromTime"> время с какого момента считывать</param>
    /// <param name="countMinutes"> количество минут для чтения</param>
    /// <returns>средняя влажность за промежуток времени</returns>
    public float? GetAverageHumidifier(string sensorId, DateTimeOffset fromTime, int countMinutes);

    /// <summary>
    /// Получить минимальный уровень CO2 за промежуток времени
    /// </summary>
    /// <param name="sensorId"> уникальный идентификатор датчика </param>
    /// <param name="fromTime"> время с какого момента считывать</param>
    /// <param name="countMinutes"> количество минут для чтения</param>
    /// <returns>минимальный уровень CO2 за промежуток времени</returns>
    public float? GetMinCarbonDioxideContent(string sensorId, DateTimeOffset fromTime, int countMinutes);

    /// <summary>
    /// Получить максимальный уровень CO2 за промежуток времени
    /// </summary>
    /// <param name="sensorId"> уникальный идентификатор датчика </param>
    /// <param name="fromTime"> время с какого момента считывать</param>
    /// <param name="countMinutes"> количество минут для чтения</param>
    /// <returns>максимальный уровень CO2 за промежуток времени</returns>
    public float? GetMaxCarbonDioxideContent(string sensorId, DateTimeOffset fromTime, int countMinutes);

    /// <summary>
    /// Получить агрегированные данные по срезу времени
    /// </summary>
    /// <param name="sensorId"></param>
    /// <param name="timeSlice"></param>
    /// <returns></returns>
    public AggregatedSensorData? GetSensorDataByTimeSlice(string sensorId, long timeSlice);

    /// <summary>
    /// Получить все агрегированный данные
    /// </summary>
    /// <returns> все агрегированный данные </returns>
    public IEnumerable<AggregatedSensorDataSlice?> GetAllAggregatedSensorData();

    public IEnumerable<AggregatedSensorData>? GetSensorDataByRange(string numSensor, DateTimeOffset fromTime, int countMinutes);
}