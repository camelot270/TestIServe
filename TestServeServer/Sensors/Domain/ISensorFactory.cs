namespace TestIServe.Server.Sensors.Domain
{
    /// <summary>
    /// Интерфейс фабрики создания датчиков
    /// </summary>
    public interface ISensorFactory
    {
        /// <summary>
        /// Создать уличный датчик
        /// </summary>
        /// <param name="guid"> уникальны идентификатор датчика </param>
        /// <returns>уличный датчик</returns>
        public ISensor CreateStreetSensor(string guid);

        /// <summary>
        /// Создать датчик внутри помещения
        /// </summary>
        /// <param name="guid"> уникальны идентификатор датчика </param>
        /// <returns>датчик внутри помещения</returns>
        public ISensor CreateIndoorSensor(string guid);
    }
}
