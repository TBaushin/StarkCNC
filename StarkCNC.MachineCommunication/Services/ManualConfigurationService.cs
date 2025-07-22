using Microsoft.Extensions.Configuration;
using OpcUaHelper;
using StarkCNC.Core.Services;
using System.Runtime.Intrinsics.X86;

namespace StarkCNC.MachineCommunication.Services
{
    public class ManualConfigurationService : IManualConfigurationService
    {
        private IStatusService _statusService;

        private readonly string _server;
        private readonly string _requestString;
        private readonly OpcUaClient _client;

        private Task _connectStatusTask;

        public bool Connected => _client.Connected;

        private bool CanConnect => !string.IsNullOrEmpty(_server) && !string.IsNullOrEmpty(_requestString);

        public ManualConfigurationService(IConfiguration configuration, IStatusService statusService)
        {
            _statusService = statusService;
            var section = configuration.GetSection("MachineController");

            _requestString = section.GetSection("RequestString").Get<string>() ?? string.Empty;
            _server = section.GetSection("Server").Get<string>() ?? string.Empty;

            _client = new OpcUaClient();
        }

        public async Task ConnectAsync()
        {
            if (CanConnect)
                try
                {
                    await _client.ConnectServer(_server);
                }
                catch (Opc.Ua.ServiceResultException ex)
                {
                    _statusService.Status = Localization.Language.ConnectionErrorMessage + $" ({ex.Message})";
                }

            RunUpdateTask();
        }

        public async Task WriteAsync<T>(T value, string to)
        {
            if (!Connected)
                await ConnectAsync();

            try
            {
                await _client.WriteNodeAsync<T>(_requestString + to, value);
            }
            catch
            {
                _statusService.Status = Localization.Language.SendRequestErrorMessage;
            }
        }

        public async Task<T> ReadAsync<T>(string from)
        {
            if (!Connected)
                await ConnectAsync();

            try
            {
                return await _client.ReadNodeAsync<T>(from);
            }
            catch
            {
                _statusService.Status = Localization.Language.GetDataRequestErrorMessage;
            }

            throw new InvalidOperationException();
        }

        private void RunUpdateTask()
        {
            if (_connectStatusTask is not null)
                return;

            _connectStatusTask = new Task(async () =>
            {
                while (true)
                {
                    if (!_client.Connected)
                        await ConnectAsync();

                    Thread.Sleep(5000);
                }
            });

            _connectStatusTask.Start();
        }
    }
}
