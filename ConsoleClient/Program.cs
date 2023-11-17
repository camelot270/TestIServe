using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TestIServe.Client;
using TestIServe.Contracts.WeatherSensorEmulatorService;

namespace TestIServe.Console
{
    class Program
    {
        public static string HelperCommands =
            "\nСписок команд:\n" +
            "add - Добавить датчики 1, 2\n" +
            "remove - Удалить датчики 1, 2\n" +
            "get_last_10_minute - получить агрегированные данные датчика с id = 1 за последние 10 минут\n" +
            "diagnostics - выводит все агрегированные данные\n" +
            "get_temperature_last_10_minute - получить среднюю температуру датчика с id = 1 за последние 10 минут\n" +
            "get_humidifier_last_10_minute - получить среднюю влажность датчика с id = 1 за последние 10 минут\n" +
            "get_min_co2_last_10_minute - получить минимальный Co2 датчика с id = 1 за последние 10 минут\n" +
            "get_max_co2_last_10_minute - получить максимальный Co2 датчика с id = 1 за последние 10 минут\n" +
            "exit - выход\n";

        static async Task Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<ILoggerFactory, LoggerFactory>()
                .AddSingleton(typeof(ILogger<>), typeof(Logger<>))
                .AddWeatherSensorClientImplementations()
                .BuildServiceProvider();

            var log = serviceProvider.GetService<ILogger<Program>>();

            var sensorClientFactory = serviceProvider.GetService<IWeatherSensorClientFactory>();

            using (WeatherSensorClient client = sensorClientFactory.Create("https://localhost:7013"))
            {
                client.Connect();
                client.WeatherSensorService?.StartWeatherSensorStreamEvents(
                    (Dictionary<string, SensorDto> sensors) =>
                    {
                        StringBuilder stingBuilder = new StringBuilder();
                        foreach (var sensor in sensors)
                        {
                            stingBuilder
                                .Append("SensorId:")
                                .Append(sensor.Value.SensorId)
                                .Append(" Time:")
                                .Append(DateTimeOffset.FromFileTime(sensor.Value.Time))
                                .Append(" {")
                                .Append(" Temperature:")
                                .Append(sensor.Value.Temperature)
                                .Append(" Humidifier:")
                                .Append(sensor.Value.Humidifier)
                                .Append(" Co2:")
                                .Append(sensor.Value.CarbonDioxideContent)
                                .Append(" }")
                                .AppendLine();

                        }
                        System.Console.WriteLine(stingBuilder);

                    });

                bool isExit = false;
                while (isExit == false)
                {
                    try
                    {
                        System.Console.WriteLine(HelperCommands);
                        switch (System.Console.ReadLine())
                        {
                            case "add":
                                client?.WeatherSensorService?.SendCommandToWeatherSensorStreamAsync(TypeOperation.Subscribe,
                                    new SortedSet<string>() { "1", "2" });
                                break;

                            case "remove":
                                client?.WeatherSensorService?.SendCommandToWeatherSensorStreamAsync(TypeOperation.Unsubscribe,
                                    new SortedSet<string>() { "1", "2" });
                                break;

                            case "get_last_10_minute":
                                var res = client?.WeatherSensorService?.GetAggregatedData("1",
                                    DateTimeOffset.Now.Subtract(TimeSpan.FromMinutes(10)), 10);
                                System.Console.WriteLine("SensorId = 1: " + res);
                                break;

                            case "get_temperature_last_10_minute":
                                var res1 = client?.WeatherSensorService?.GetAverageTemperature("1",
                                    DateTimeOffset.Now.Subtract(TimeSpan.FromMinutes(10)), 10);
                                System.Console.WriteLine("SensorId = 1: " + res1);
                                break;

                            case "get_humidifier_last_10_minute":
                                var res2 = client?.WeatherSensorService?.GetAverageHumidifier("1",
                                    DateTimeOffset.Now.Subtract(TimeSpan.FromMinutes(10)), 10);
                                System.Console.WriteLine("SensorId = 1: " + res2);
                                break;

                            case "get_min_co2_last_10_minute":
                                var res3 = client?.WeatherSensorService?.GetMinCarbonDioxideContent("1",
                                    DateTimeOffset.Now.Subtract(TimeSpan.FromMinutes(10)), 10);
                                System.Console.WriteLine("SensorId = 1: " + res3);
                                break;

                            case "get_max_co2_last_10_minute":
                                var res4 = client?.WeatherSensorService?.GetMaxCarbonDioxideContent("1",
                                    DateTimeOffset.Now.Subtract(TimeSpan.FromMinutes(10)), 10);
                                System.Console.WriteLine("SensorId = 1: " + res4);
                                break;


                            case "diagnostics":
                                var data = client?.WeatherSensorService?.GetAllData();
                                if (data != null)
                                {
                                    StringBuilder stingBuilder = new StringBuilder();

                                    await foreach (var slice in data)
                                    {
                                        stingBuilder.Clear();

                                        stingBuilder
                                            .Append("SensorId:")
                                            .Append(slice.Id)
                                            .Append(" Time:")
                                            .Append(DateTimeOffset.FromFileTime(slice.SliceTime))
                                            .Append(slice.AggreagateSensorData);

                                        System.Console.WriteLine(stingBuilder.ToString());
                                    }
                                }

                                break;

                            case "exit":
                                isExit = true;
                                break;
                            default:
                                break;
                        }
                    }
                    catch(Exception ex)
                    {
                        log.LogError(ex.Message);
                    }

                    if (isExit)
                        break;
                }
            }
        }

       
    }
}