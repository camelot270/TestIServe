using System.Diagnostics;
using Grpc.Core;
using Grpc.Net.Client;
using TestIServe.Contracts.WeatherSensorEmulatorService;
using ThreadState = System.Threading.ThreadState;

namespace TestIServe.Client.Services.WeatherSensorEmulatorClientService
{
    public delegate void ResponseSensorsDataDelegate(Dictionary<string, SensorDto> sensor);

    /// <inheritdoc />
    internal class WeatherSensorEmulatorClientService : IWeatherSensorEmulatorClientService
    {
        private readonly string _clientId;
        private CancellationTokenSource _cancellationToken = new();
        private GrpcChannel? _channel;
        private AsyncDuplexStreamingCall<RequestSubscribeUnsubscribe, ResponseSensorsData>? _duplex = null;
        private Thread _streamReadThread;

        private ResponseSensorsDataDelegate _onReadAction = (dtos => Debug.WriteLine(dtos));
        private WeatherSensorEmulator.WeatherSensorEmulatorClient clientService;

        private readonly SortedSet<string>? _listConnectionSensors = new();

        private bool _isStartStreaming = false;
        private bool _isConnected = false;

        public bool IsStartStreaming => _isStartStreaming;
        public bool IsConectded => _isConnected;

        public WeatherSensorEmulatorClientService(string clientId)
        {
            _clientId = clientId;
        }

        /// <inheritdoc />
        public void Connect(GrpcChannel channel)
        {
            _channel = channel;

            clientService = new WeatherSensorEmulator.WeatherSensorEmulatorClient(channel);
            _isConnected = true;
        }

        /// <inheritdoc />
        public void ReConnect(GrpcChannel channel)
        {
            if (IsConectded == false)
            {
                _cancellationToken = new();
                Connect(channel);

                StartWeatherSensorStreamEvents(_onReadAction);

                if (_listConnectionSensors != null && _listConnectionSensors.Any() && _isStartStreaming == false)
                {
                    _ = SendCommandToWeatherSensorStreamAsync(TypeOperation.Subscribe, _listConnectionSensors);
                }
            }
        }

        /// <inheritdoc />
        public void TryDisconnect()
        {
            _isStartStreaming = false;
            _isConnected = false;

            _cancellationToken.Cancel();
            
            _streamReadThread?.ExecutionContext?.Dispose();
            if (_streamReadThread?.ThreadState != ThreadState.Stopped)
            {
                _duplex?.RequestStream?.CompleteAsync();
            }

            _duplex?.Dispose();
            _duplex = null;
        }

        /// <inheritdoc />
        public void StartWeatherSensorStreamEvents(ResponseSensorsDataDelegate onReadAction)
        {
            _onReadAction = onReadAction;

            if (IsConectded && _channel != null)
            {
                _duplex = clientService.StreamCommand();
                _streamReadThread = new Thread(() =>
                    StartReadStreamAsync(_channel, _cancellationToken.Token, onReadAction: onReadAction));
                _streamReadThread.Start();

                _isStartStreaming = true;
            }
        }

        /// <inheritdoc />
        public async Task SendCommandToWeatherSensorStreamAsync(TypeOperation typeOperation, SortedSet<string>? sensorIdList)
        {
            if (sensorIdList == null || !sensorIdList.Any())
            {
                return;
            }

            foreach (var sensorId in sensorIdList)
            {
                if (typeOperation == TypeOperation.Subscribe)
                {
                    _listConnectionSensors?.Add(sensorId);
                }
                else if (typeOperation == TypeOperation.Unsubscribe)
                {
                    _listConnectionSensors?.Remove(sensorId);
                }
            }

            if (_duplex != null)
            {
                await _duplex.RequestStream.WriteAsync(new RequestSubscribeUnsubscribe
                {
                    ClientId = _clientId,
                    Operation = typeOperation,
                    SensorsId = { sensorIdList }
                });
            }
        }

        public async void StartReadStreamAsync(GrpcChannel channel, CancellationToken cancellationToken,
            ResponseSensorsDataDelegate onReadAction)
        {
            try
            {
                while (_duplex != null
                       && await _duplex.ResponseStream.MoveNext(cancellationToken)
                       && !_cancellationToken.IsCancellationRequested)
                {
                    var response = _duplex.ResponseStream.Current;
                    Dictionary<string, SensorDto> sensorsDictionary = new();
                    foreach (var sensor in response.SensorsDto)
                    {
                        sensorsDictionary.Add(sensor.SensorId, sensor);
                    }
                    onReadAction.Invoke(sensorsDictionary);
                }
            }
            catch
            {
                // ignored
            }
        }

        public void Dispose()
        {
            TryDisconnect();
        }

        /// <inheritdoc />
        public async IAsyncEnumerable<AggregatedSensorDataSlice> GetAllData()
        {
            var streamingCall = clientService
                .GetAllData(new() { ClientId = _clientId });

            var slicesEnumerator =
                streamingCall.ResponseStream.ReadAllAsync();

            await foreach (var slice in slicesEnumerator)
            {
                yield return slice;
            }
        }


        /// <inheritdoc />
        public AggregatedSensorData? GetAggregatedData(string sensorId,
            DateTimeOffset fromTime, int countMinutes)
        {
            if (_channel != null && _channel.State != ConnectivityState.Ready
               )
                return null;

            var data = clientService?
                .GetAggregatedData(new() { Id = sensorId, FromTime = fromTime.ToFileTime(),CountMinutes = countMinutes });
            return data;
        }

        /// <inheritdoc />
        public float? GetAverageTemperature(string sensorId,
            DateTimeOffset fromTime, int countMinutes)
        {
            if (_channel != null && _channel.State != ConnectivityState.Ready)
                return null;

            var data = clientService?
                .GetAverageTemperature(new() { Id = sensorId, FromTime = fromTime.ToFileTime(), CountMinutes = countMinutes });
            return data?.Value;
        }

        /// <inheritdoc />
        public float? GetAverageHumidifier(string sensorId,
            DateTimeOffset fromTime, int countMinutes)
        {
            if (_channel != null && _channel.State != ConnectivityState.Ready)
                return null;

            var data = clientService?
                .GetAverageHumidifier(new() { Id = sensorId, FromTime = fromTime.ToFileTime(), CountMinutes = countMinutes });
            return data?.Value;
        }

        /// <inheritdoc />
        public float? GetMinCarbonDioxideContent(string sensorId,
            DateTimeOffset fromTime, int countMinutes)
        {
            if (_channel != null && _channel.State != ConnectivityState.Ready)
                return null;

            var data = clientService?
                .GetMinCarbonDioxideContent(new() { Id = sensorId, FromTime = fromTime.ToFileTime(), CountMinutes = countMinutes });
            return data?.Value;
        }

        /// <inheritdoc />
        public float? GetMaxCarbonDioxideContent(string sensorId,
            DateTimeOffset fromTime, int countMinutes)
        {
            if (_channel != null && _channel.State != ConnectivityState.Ready)
                return null;

            var data = clientService?
                .GetMaxCarbonDioxideContent(new() { Id = sensorId, FromTime = fromTime.ToFileTime(), CountMinutes = countMinutes });
            return data?.Value;
        }


    }
}
