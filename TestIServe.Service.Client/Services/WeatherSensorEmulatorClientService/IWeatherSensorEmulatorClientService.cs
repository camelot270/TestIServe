using Grpc.Core;
using Grpc.Net.Client;
using System.Threading.Channels;
using TestIServe.Contracts.WeatherSensorEmulatorService;

namespace TestIServe.Client.Services.WeatherSensorEmulatorClientService;

/// <summary>
/// Интерфейс фабрики для создания клиента погодных датчиков
/// </summary>
public interface IWeatherSensorEmulatorClientService : IDisposable
{
    /// <summary>
    /// Подключения сервиса
    /// </summary>
    /// <param name="channel"> канал подключения </param>
    void Connect(GrpcChannel channel);

    /// <summary>
    /// Переподключение сервиса
    /// </summary>
    /// <param name="channel"> канал подключения </param>
    void ReConnect(GrpcChannel channel);
    
    /// <summary>
    /// Попытатка отключения сервиса
    /// </summary>
    void TryDisconnect();

    /// <summary>
    /// Отправить комаду серверу по каналу
    /// </summary>
    /// <param name="typeOperation"> тип команды</param>
    /// <param name="sensorIdList"> список датчиков для отписки/подписки чтения датчиков</param>
    /// <returns></returns>
    Task SendCommandToWeatherSensorStreamAsync(TypeOperation typeOperation, SortedSet<string>? sensorIdList);

    /// <summary>
    /// Запустить поток чтения событий датчиков
    /// </summary>
    /// <param name="onReadAction"> Функция обработки события</param>
    void StartWeatherSensorStreamEvents(ResponseSensorsDataDelegate onReadAction);

    /// <summary>
    /// Получить все агрегированные данные с сервера
    /// </summary>
    /// <returns>список считанных датчиков</returns>
    IAsyncEnumerable<AggregatedSensorDataSlice> GetAllData();

    /// <summary>
    /// Получить агрегированные данные по диапазону
    /// </summary>
    /// <param name="sensorId"> Id датчика</param>
    /// <param name="fromTime"> начало времени</param>
    /// <param name="countMinutes"> количество минут </param>
    /// <returns>агрегированные данные за промежуток</returns>
    public AggregatedSensorData? GetAggregatedData(string sensorId,
        DateTimeOffset fromTime, int countMinutes);

    /// <summary>
    /// Получить среднюю температуру по диапазону
    /// </summary>
    /// <param name="sensorId"> Id датчика</param>
    /// <param name="fromTime"> начало времени</param>
    /// <param name="countMinutes"> количество минут </param>
    /// <returns>среднюю температуру за промежуток</returns>
    public float? GetAverageTemperature(string sensorId,
        DateTimeOffset fromTime, int countMinutes);

    /// <summary>
    /// Получить среднюю влажность по диапазону
    /// </summary>
    /// <param name="sensorId"> Id датчика</param>
    /// <param name="fromTime"> начало времени</param>
    /// <param name="countMinutes"> количество минут </param>
    /// <returns>среднюю влажность за промежуток</returns>
    public float? GetAverageHumidifier(string sensorId,
        DateTimeOffset fromTime, int countMinutes);

    /// <summary>
    /// Получить минимальный Co2 по диапазону
    /// </summary>
    /// <param name="sensorId"> Id датчика</param>
    /// <param name="fromTime"> начало времени</param>
    /// <param name="countMinutes"> количество минут </param>
    /// <returns>минимальный Co2  за промежуток</returns>
    public float? GetMinCarbonDioxideContent(string sensorId,
        DateTimeOffset fromTime, int countMinutes);

    /// <summary>
    /// Получить максимальный Co2 по диапазону
    /// </summary>
    /// <param name="sensorId"> Id датчика</param>
    /// <param name="fromTime"> начало времени</param>
    /// <param name="countMinutes"> количество минут </param>
    /// <returns>максимальный Co2 за промежуток</returns>
    public float? GetMaxCarbonDioxideContent(string sensorId,
        DateTimeOffset fromTime, int countMinutes);
}