using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestIServe.Client
{
    /// <summary>
    /// Интерфейс фабрики для создания клиента погодных датчиков
    /// </summary>
    public interface IWeatherSensorClientFactory
    {
        /// <summary>
        /// Создание клиента погодных датчиков
        /// </summary>
        /// <param name="serverAddress"> Адрес сервера</param>
        /// <returns></returns>
        WeatherSensorClient Create(string serverAddress);
    }
}
