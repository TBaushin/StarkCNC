using Microsoft.Extensions.Configuration;
using OpcUaHelper;
using StarkCNC.Core.Models;
using StarkCNC.Core.Services;
using System.Diagnostics;

namespace StarkCNC.MachineCommunication.Services;

public class ManualConfigurationService : IManualConfigurationService
{
    private readonly IStatusService _statusService;

    private readonly string _server;
    private readonly string _requestString;
    private readonly OpcUaClient _client;

    private Task? _connectStatusTask;

    public bool Connected => _client.Connected;

    private bool CanConnect =>
        !string.IsNullOrEmpty(_server) && !string.IsNullOrEmpty(_requestString);

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
            try
            {
                await _client.ConnectServer(_server).ConfigureAwait(false);

                _statusService.CurrentStatus = new Status("Подключение успешно", StatusType.Success);
            }
            catch (Opc.Ua.ServiceResultException ex)
            {
#if DEBUG
                Debug.WriteLine(Localization.Language.ConnectionErrorMessage + $" ({ex.Message})");
#endif
                _statusService.CurrentStatus = new Status(Localization.Language.ConnectionErrorMessage + $" ({ex.Message})", StatusType.Error);
            }
        }

        RunUpdateTask();
    }

    public async Task WriteAsync<T>(T value, string to, StatusPage fromPage = StatusPage.Unknown)
    {
        if (!Connected)
            return;

        try
        {
            await _client.WriteNodeAsync<T>(_requestString + to, value).ConfigureAwait(false);
        }
        catch (Exception)
        {
#if DEBUG
            Debug.WriteLine(Localization.Language.SendRequestErrorMessage);
#endif
            _statusService.CurrentStatus = new Status(Localization.Language.SendRequestErrorMessage, StatusType.Error, fromPage);
        }
    }

    public async Task<T?> ReadAsync<T>(string from, StatusPage fromPage = StatusPage.Unknown)
    {
        if (!Connected)
            return default;

        try
        {
            return await _client.ReadNodeAsync<T>(_requestString + from)
                .ConfigureAwait(false);
        }
        catch (Exception)
        {
#if DEBUG
            Debug.WriteLine(Localization.Language.GetDataRequestErrorMessage + $" {from}");
#endif
            _statusService.CurrentStatus = new Status(Localization.Language.GetDataRequestErrorMessage + $" {from}", StatusType.Error, fromPage);
        }

        return default;
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
                    if (_statusService.CurrentStatus == null || _statusService.CurrentStatus.Text == Localization.Language.ConnectionErrorMessage)
                        _statusService.CurrentStatus = null;
                }
                else
                {
                    await ConnectAsync().ConfigureAwait(false);
                }

                await Task.Delay(5000).ConfigureAwait(false);
            }
        });
    }
}