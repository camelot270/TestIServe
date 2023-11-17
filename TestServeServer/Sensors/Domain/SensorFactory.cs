namespace TestIServe.Server.Sensors.Domain
{
    public class SensorFactory : ISensorFactory
    {
        public ISensor CreateStreetSensor(string guid)
        {
            return new StreetSensor(guid);
        }

        public ISensor CreateIndoorSensor(string guid)
        {
            return new IndoorSensor(guid);
        }
    }
}
