using OpcUaHelper;

namespace StarkCNC.MachineCommunication.Services
{
    public class ManualConfigurationService : IManualConfigurationService
    {
        private readonly string _ipAddress;
        private readonly string _configurationString;
        private readonly OpcUaClient _client;

        public ManualConfigurationService(string configurationString, string ipAddress)
        {
            _configurationString = configurationString;
            _ipAddress = ipAddress;

            _client = new OpcUaClient();
        }

        public async Task ConnectAsync()
        {
            await _client.ConnectServer(_ipAddress);
        }
    }
}
