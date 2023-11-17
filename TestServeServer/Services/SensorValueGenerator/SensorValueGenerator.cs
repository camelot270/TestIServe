namespace TestIServe.Server.Services.SensorValueGenerator
{
    /// <inheritdoc />
    public class SensorValueGenerator : ISensorValueGenerator
    {
        readonly Random _random = new Random();
        
        /// <inheritdoc />
        public float GetGeneratedTemperature(double min = 8, double max = 23, double spreadKoef = 5)
        {
            var hour = DateTime.UtcNow.Hour;

            var hourKoef = (24 - hour) % 12 / 12.0;

            var offset = min;

            var valueRange = max - min;

            return (float)(valueRange * hourKoef + offset + _random.NextSingle() * spreadKoef);
        }

        /// <inheritdoc />
        public float GetGeneratedHumidifier(double min = 30, double max = 60, double spreadKoef = 10)
        {
            var minute = DateTime.UtcNow.Minute;

            var minuteKoef = (60 - minute) % 30 / 30.0;

            var offset = min;

            var valueRange = max - min;

            return (float)(valueRange * minuteKoef + offset + _random.NextSingle() * spreadKoef);
        }

        /// <inheritdoc />
        public float GetGeneratedCarbonDioxideContentOnIndoor(double min = 600, double max = 1200, double spreadKoef = 50)
        {
            var minute = DateTime.UtcNow.Minute;

            var minuteKoef = (60 - minute) % 30 / 30.0;

            var offset = min;

            var valueRange = max - min;

            return (float)(valueRange * minuteKoef + offset + _random.NextSingle() * spreadKoef);
        }

        /// <inheritdoc />
        public float GetGeneratedCarbonDioxideContentOnStreet(double min = 400, double max = 500, double spreadKoef = 20)
        {
            var minute = DateTime.UtcNow.Minute;

            var minuteKoef = (60 - minute) % 30 / 30.0;

            var offset = min;

            var valueRange = max - min;

            return (float)(valueRange * minuteKoef + offset + _random.NextSingle() * 3);
        }
    }
}
