using System.ComponentModel.DataAnnotations;

namespace TestIServe.Server.Configurations
{
    public class SensorDataAggregationServiceOptions
    {
        [Range(1, uint.MaxValue, ErrorMessage = "Value for {0} must be more or equal {1}\"")]
        public uint CountMinutesToAggregate { get; set; } = 1;
    }
}
