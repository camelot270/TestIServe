using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestIServe.Client.Services.WeatherSensorEmulatorClientService;

namespace TestIServe.Client
{
    /// <summary>
    /// Extension for '<see cref="Microsoft.Extensions.DependencyInjection.IServiceCollection"/>' (Microsoft.Extensions.DependencyInjection)
    /// </summary>
    public static class Module
    {
        public static IServiceCollection AddWeatherSensorClientImplementations(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IWeatherSensorClientFactory, WeatherSensorClientFactory>();
            serviceCollection.AddSingleton<IWeatherSensorEmulatorClientServiceFactory, WeatherSensorEmulatorClientServiceFactory>();
            serviceCollection.AddSingleton<IWeatherSensorEmulatorClientService, WeatherSensorEmulatorClientService>();
            return serviceCollection;
        }
    }
}
