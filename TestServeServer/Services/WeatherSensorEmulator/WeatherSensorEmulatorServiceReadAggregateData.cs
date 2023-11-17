using Grpc.Core;
using TestIServe.Contracts.WeatherSensorEmulatorService;

namespace TestIServe.Server.Services.WeatherSensorEmulatorService
{
    public partial class WeatherSensorEmulatorService : WeatherSensorEmulator.WeatherSensorEmulatorBase
    {
        public override Task<ResponseParam> GetAverageTemperature(RequestAggreagationData request, ServerCallContext context)
        {
            bool isHaveValue = false;
            var resValue = _sensorsQueryService.GetAverageTemperature(request.Id, DateTimeOffset.FromFileTime(request.FromTime), request.CountMinutes);

            if (resValue != null)
            {
                isHaveValue = true;
            }

            return
                Task.FromResult(new ResponseParam
                {
                    IsHaveValue = isHaveValue,
                    Value = resValue ?? 0
                });
        }

        public override Task<ResponseParam> GetAverageHumidifier(RequestAggreagationData request, ServerCallContext context)
        {
            bool isHaveValue = false;
            var resValue = _sensorsQueryService.GetAverageHumidifier(request.Id, DateTimeOffset.FromFileTime(request.FromTime), request.CountMinutes);

            if (resValue != null)
            {
                isHaveValue = true;
            }

            return
                Task.FromResult(new ResponseParam
                {
                    IsHaveValue = isHaveValue,
                    Value = resValue ?? 0
                });
        }

        public override Task<ResponseParam> GetMinCarbonDioxideContent(RequestAggreagationData request, ServerCallContext context)
        {
            bool isHaveValue = false;
            var resValue = _sensorsQueryService.GetMinCarbonDioxideContent(request.Id, DateTimeOffset.FromFileTime(request.FromTime), request.CountMinutes);

            if (resValue != null)
            {
                isHaveValue = true;
            }


            return
                Task.FromResult(new ResponseParam
                {
                    IsHaveValue = isHaveValue,
                    Value = resValue ?? 0
                });
        }

        public override Task<ResponseParam> GetMaxCarbonDioxideContent(RequestAggreagationData request, ServerCallContext context)
        {
            bool isHaveValue = false;
            var resValue = _sensorsQueryService.GetMaxCarbonDioxideContent(request.Id, DateTimeOffset.FromFileTime(request.FromTime), request.CountMinutes);

            if (resValue != null)
            {
                isHaveValue = true;
            }

            return
                Task.FromResult(new ResponseParam
                {
                    IsHaveValue = isHaveValue,
                    Value = resValue ?? 0
                });
        }

        public override Task<AggregatedSensorData?> GetAggregatedData(RequestAggreagationData request, ServerCallContext context)
        {
            var collectionData = _sensorsQueryService.GetSensorDataByRange(request.Id, DateTimeOffset.FromFileTime(request.FromTime), request.CountMinutes);
            var data = collectionData?.Aggregate((currentValueInTime, lastDataSensor) => new AggregatedSensorData()
            {
                TemperatureAverage = (currentValueInTime.TemperatureAverage + lastDataSensor.TemperatureAverage) / 2,
                HumidifierAverage = (currentValueInTime.HumidifierAverage + lastDataSensor.HumidifierAverage) / 2,
                CarbonDioxideContentMin = Math.Min(currentValueInTime.CarbonDioxideContentMin,
                    lastDataSensor.CarbonDioxideContentMin),
                CarbonDioxideContentMax = Math.Max(currentValueInTime.CarbonDioxideContentMax,
                    lastDataSensor.CarbonDioxideContentMax)
            });

            return Task.FromResult(data);
        }
    }
}
