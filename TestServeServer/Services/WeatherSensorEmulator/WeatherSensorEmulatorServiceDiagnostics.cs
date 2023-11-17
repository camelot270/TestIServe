using Grpc.Core;
using TestIServe.Contracts.WeatherSensorEmulatorService;

namespace TestIServe.Server.Services.WeatherSensorEmulatorService
{
    public partial class WeatherSensorEmulatorService : WeatherSensorEmulator.WeatherSensorEmulatorBase
    {
        public override async Task GetAllData(RequestReadAllAggregatedData request, IServerStreamWriter<AggregatedSensorDataSlice> responseStream, ServerCallContext context)
        {
            foreach (var aggregatedSensorData in _sensorsQueryService.GetAllAggregatedSensorData())
            {
                if (aggregatedSensorData != null)
                {
                    await responseStream.WriteAsync(aggregatedSensorData);
                }
            }
        }
    }
}
