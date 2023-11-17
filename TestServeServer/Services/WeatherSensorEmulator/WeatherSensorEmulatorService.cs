using Grpc.Core;
using TestIServe.Contracts.WeatherSensorEmulatorService;
using TestIServe.Server.Sensors.Domain;
using TestIServe.Server.Storage.Queries;

namespace TestIServe.Server.Services.WeatherSensorEmulatorService
{
    public partial class WeatherSensorEmulatorService : WeatherSensorEmulator.WeatherSensorEmulatorBase, IDisposable
    {
        private readonly ILogger<WeatherSensorEmulatorService> _logger;
        private readonly ISensorRepository _sensorRepository;
        private readonly ISensorsQueryService _sensorsQueryService;

        private IAsyncStreamReader<RequestSubscribeUnsubscribe>? _requestStream = null;
        private IServerStreamWriter<ResponseSensorsData>? _responseStream = null;

        HashSet<string> _sensorSubscribes = new();

        public WeatherSensorEmulatorService(ILogger<WeatherSensorEmulatorService> logger
            , ISensorRepository sensorRepository
            , ISensorsQueryService sensorsQueryService
            )
        {
            _logger = logger;
            _sensorRepository = sensorRepository;
            _sensorsQueryService = sensorsQueryService;
        }

        ~WeatherSensorEmulatorService()
        {
            Dispose();
        }

        public override async Task StreamCommand(IAsyncStreamReader<RequestSubscribeUnsubscribe> requestStream, IServerStreamWriter<ResponseSensorsData> responseStream, ServerCallContext context)
        {
            _requestStream = requestStream;
            _responseStream = responseStream;

            try
            {
                while (await requestStream.MoveNext()
                       && !context.CancellationToken.IsCancellationRequested)
                {
                    var current = requestStream.Current;
                    Console.WriteLine($"Message from Client: {current.ClientId}");

                    switch (current.Operation)
                    {
                        case TypeOperation.None:
                            break;
                        case TypeOperation.Subscribe:
                            foreach (var sensorId in current.SensorsId)
                            {
                                SubscribeToSensor(sensorId);
                            }
                            break;
                        case TypeOperation.Unsubscribe:
                            foreach (var sensorId in current.SensorsId)
                            {
                                UnSubscribeToSensor(sensorId);
                            }
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    //_= SendResponseMessage(current, responseStream);
                }
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
            {
                Console.WriteLine("Operation Cancelled");
            }

            Console.WriteLine("Operation Complete.");

            ReleaseUnmanagedResources();
        }

        void SubscribeToSensor(string sensorId)
        {
            if (_sensorSubscribes.Contains(sensorId))
            {
                // Данный датчик уже подписан
                return;
            }

            if (!_sensorRepository.Sensors.ContainsKey(sensorId))
            {
                // Данный датчик не подключен
                return;
            }

            _sensorSubscribes.Add(sensorId);
            _sensorRepository.Sensors[sensorId].SensorDataChanged += GenerateSensorData;

        }

        void UnSubscribeToSensor(string sensorId)
        {
            if (!_sensorSubscribes.Contains(sensorId))
            {
                // Нет подписки на данный датчик
                return;
            }

            if (!_sensorRepository.Sensors.ContainsKey(sensorId))
            {
                // Данный датчик не подключен
                return;
            }

            _sensorRepository.Sensors[sensorId].SensorDataChanged -= GenerateSensorData;

            _sensorSubscribes.Remove(sensorId);
        }

        private void GenerateSensorData(SensorDto obj)
        {
            if (_responseStream == null || _requestStream?.Current == null)
            {
                return;
            }

            List<SensorDto> responseList = new(_sensorSubscribes.Count);
            
            responseList.Add(obj);

            _responseStream.WriteAsync(new ResponseSensorsData
            {
                ClientId = _requestStream.Current.ClientId,
                SensorsDto = { responseList }
            });
        }

        private void ReleaseUnmanagedResources()
        {
            foreach (var sensorId in _sensorSubscribes)
            {
                UnSubscribeToSensor(sensorId);
            }

            _sensorSubscribes.Clear();
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }
    }
}