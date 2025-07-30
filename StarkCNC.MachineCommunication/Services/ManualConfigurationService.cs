using Microsoft.Extensions.Configuration;
using OpcUaHelper;
using StarkCNC.Core.Services;

namespace StarkCNC.MachineCommunication.Services
{
    public class ManualConfigurationService : IManualConfigurationService
    {
        private readonly IStatusService _statusService;

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
            {
                //try
                {
                    await Task.Run(async () => await _client.ConnectServer(_server)).ConfigureAwait(false);
                }
                //catch (Opc.Ua.ServiceResultException ex)
                {
                    //_statusService.Status = Localization.Language.ConnectionErrorMessage + $" ({ex.Message})";
                }
            }

            RunUpdateTask();
        }

        public async Task WriteAsync<T>(T value, string to)
        {
            if (!Connected)
                await ConnectAsync();

            if (!Connected)
                return;

            try
            {
                await Task.Run(async () => await _client.WriteNodeAsync<T>(_requestString + to, value)).ConfigureAwait(false);
            }
            catch (Exception)
            {
                _statusService.Status = Localization.Language.SendRequestErrorMessage;
            }
        }

        public async Task<T> ReadAsync<T>(string from)
        {
            //if (!Connected)
              //  await ConnectAsync();

            if (Connected)
             {
                 //try
                 {
                    
                    var a = await  _client.ReadNodeAsync<T>(_requestString + from);
                    return a;
                }
                 //catch (Exception)
                 {
                     _statusService.Status = Localization.Language.GetDataRequestErrorMessage;
                 }
             }

             return default(T);
        }
      
        private void RunUpdateTask()
        {
            if (_connectStatusTask is not null)
                return;

            _connectStatusTask = Task.Run(async () =>
            {
                while (true)
                {
                    if (_client.Connected)
                    {
                        if (_statusService.Status == Localization.Language.ConnectionErrorMessage)
                            _statusService.Status = "";
                    }
                    else
                    {
                        await ConnectAsync();
                    }

                    await Task.Delay(5000);
                }
            });
        }
    }
}
