using System.ComponentModel.DataAnnotations;

namespace TestIServe.Server.Configurations
{
    public class GenerationEventsServiceOptions
    {
        [Range(100, 2000,
            ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int SensorEventIntervalInMs { get; set; }

    }
}
