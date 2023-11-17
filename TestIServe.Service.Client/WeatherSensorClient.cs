using Grpc.Core;
using Grpc.Net.Client;
using TestIServe.Client.Services.WeatherSensorEmulatorClientService;
using TestIServe.Contracts.WeatherSensorEmulatorService;

namespace TestIServe.Client
{
    public class WeatherSensorClient : IDisposable
    {
        private readonly string _serverAddress;
        public IWeatherSensorEmulatorClientService? WeatherSensorService = null;
        private GrpcChannel? _channel;
        private CancellationTokenSource _cancellationToken = new CancellationTokenSource();
        private Thread? _checkAndTryConnectThread = null;
        private readonly string _clientGuid;

        private bool _isConnected;

        public bool IsConnected => _isConnected;

        public WeatherSensorClient( IWeatherSensorEmulatorClientServiceFactory weatherSensorEmulatorClientServiceFactory,  string serverAddress)
        {
            _clientGuid = Guid.NewGuid().ToString();
            _serverAddress = serverAddress;
            WeatherSensorService = weatherSensorEmulatorClientServiceFactory.Create(_clientGuid);
        }

        public void Connect(CancellationTokenSource? token = null)
        {
            _cancellationToken = token ?? new CancellationTokenSource();
            _channel = GrpcChannel.ForAddress(
                _serverAddress,
                new GrpcChannelOptions()
                {
                    MaxReconnectBackoff = TimeSpan.FromTicks(TimeSpan.TicksPerMinute),
                    ThrowOperationCanceledOnCancellation = false
                });

            WeatherSensorService?.Connect(_channel);

            _checkAndTryConnectThread = new Thread(() => RunCheckTryConnectionToServer(_channel));
            _checkAndTryConnectThread.Start();
        }


        private async void RunCheckTryConnectionToServer(GrpcChannel channel)
        {
            _channel = channel;
            while (_cancellationToken is { IsCancellationRequested: false })
            {
                await _channel.WaitForStateChangedAsync(ConnectivityState.Ready, _cancellationToken.Token);

                if (_isConnected)
                {
                    WeatherSensorService?.TryDisconnect();
                }

                _isConnected = false;

                if (await TryConnectChannelAsync() == false)
                {
                    continue;
                }

                WeatherSensorService?.ReConnect(_channel);

                _isConnected = true;
            }
        }

        private async Task<bool> TryConnectChannelAsync()
        {
            if (_channel is null)
            {
                return false;
            }

            await _channel.ConnectAsync(_cancellationToken.Token);

            await _channel.WaitForStateChangedAsync(ConnectivityState.Connecting, _cancellationToken.Token);

            return _channel.State == ConnectivityState.Ready;
        }

        public async Task WriteToStreamAsync(TypeOperation typeOperation,
            SortedSet<string>? sensorIdList)
        {
            WeatherSensorService?.SendCommandToWeatherSensorStreamAsync(typeOperation, sensorIdList);
        }


        public void Dispose()
        {
            WeatherSensorService?.Dispose();
            _channel?.Dispose();
            _cancellationToken.Dispose();
        }
    }
}
