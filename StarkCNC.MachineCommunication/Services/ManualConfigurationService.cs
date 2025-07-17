using Microsoft.Extensions.Configuration;
using OpcUaHelper;

namespace StarkCNC.MachineCommunication.Services
{
    public class ManualConfigurationService : IManualConfigurationService
    {
        private readonly string _server;
        private readonly string _requestString;
        private readonly OpcUaClient _client;
        public bool Connected => _client.Connected;

        private bool CanConnect => !string.IsNullOrEmpty(_server) && !string.IsNullOrEmpty(_requestString);

        public ManualConfigurationService(IConfiguration configuration,string configurationString, string ipAddress)
        {
            var section = configuration.GetSection("MachineController");

            _requestString = section.GetSection("RequestString").Get<string>() ?? string.Empty;
            _server = section.GetSection("Server").Get<string>() ?? string.Empty;

            _client = new OpcUaClient();
        }

        public async Task ConnectAsync()
        {
            if (CanConnect)
                await _client.ConnectServer(_server);
        }

        public async Task WriteAsync<T>(T value, string to)
        {
            if (!Connected)
                await ConnectAsync();
            await _client.WriteNodeAsync<T>(_requestString + to, value);
        }

        public async Task<T> ReadAsync<T>(string from)
        {
            if (!Connected)
                await ConnectAsync();
            return await _client.ReadNodeAsync<T>(from);
        }
    }
}
