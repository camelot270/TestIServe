using System;

namespace TestIServe.Server.Services.SensorValueGenerator
{
    /// <summary>
    /// Интерфейс генерации температуры
    /// </summary>
    public interface ISensorValueGenerator
    {
        /// <summary>
        ///  Получить сгенерированную температуру
        /// </summary>
        /// <param name="min"> минимаельное значение </param>
        /// <param name="max"> максимальное значение</param>
        /// <param name="spreadKoef">Коэффициент разброса сгенерируемого значения</param>
        /// <returns>сгенерированная температура</returns>
        public float GetGeneratedTemperature(double min = 8, double max = 23, double spreadKoef = 5);

        /// <summary>
        ///  Получить сгенерированную влажность
        /// </summary>
        /// <param name="min"> минимаельное значение </param>
        /// <param name="max"> максимальное значение</param>
        /// <param name="spreadKoef">Коэффициент разброса сгенерируемого значения</param>
        /// <returns>сгенерированная влажность</returns>
        public float GetGeneratedHumidifier(double min = 30, double max = 60, double spreadKoef = 10);

        /// <summary>
        ///  Получить сгенерированный уровень CO2 в помещении
        /// </summary>
        /// <param name="min"> минимаельное значение </param>
        /// <param name="max"> максимальное значение</param>
        /// <param name="spreadKoef">Коэффициент разброса сгенерируемого значения</param>
        /// <returns>сгенерированный уровень CO2 в помещении</returns>
        public float GetGeneratedCarbonDioxideContentOnIndoor(double min = 600, double max = 1200, double spreadKoef = 50);

        /// <summary>
        ///  Получить сгенерированный уровень CO2 на улице
        /// </summary>
        /// <param name="min"> минимаельное значение </param>
        /// <param name="max"> максимальное значение</param>
        /// <param name="spreadKoef">Коэффициент разброса сгенерируемого значения</param>
        /// <returns>сгенерированный уровень CO2 на улице</returns>
        public float GetGeneratedCarbonDioxideContentOnStreet(double min = 400, double max = 500,
            double spreadKoef = 20);
    }
}
