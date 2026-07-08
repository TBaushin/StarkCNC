using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Configuration;
using Opc.Ua;
using Opc.Ua.Client;
using OpcUaHelper;
using StarkCNC.Core.Models;
using StarkCNC.Core.Repository;
using StarkCNC.MachineCommunication.Services;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Threading;

namespace StarkCNC.Services;

public partial class ManualConfigurationService : ObservableObject, IManualConfigurationService
{
    private readonly IStatusService _statusService;

    private readonly SemaphoreSlim _connectGate = new SemaphoreSlim(1, 1);

    private string _server;
    private string _requestString;
    private readonly OpcUaClient _client;

    private DispatcherTimer? _timer;

    [ObservableProperty]
    private bool _connected;

    public ManualConfigurationService(
        IConfiguration configuration,
        ISettingsRepository settingsRepository,
        IStatusService statusService)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(settingsRepository);

        _statusService = statusService;

        _client = new OpcUaClient();

        var settings = settingsRepository.Get();
        if (settings is null || string.IsNullOrEmpty(settings.Server))
        {
            var section = configuration.GetSection("MachineController");

            _requestString = section.GetSection("RequestString").Get<string>() ?? string.Empty;
            _server = section.GetSection("Server").Get<string>() ?? string.Empty;
        }
        else
        {
            _server = settings.Server;
            _requestString = string.Empty;
        }
    }

    public async Task ConnectAsync()
    {
        if (!await _connectGate.WaitAsync(0).ConfigureAwait(false))
            return;

        try
        {
            string server = string.Empty;
            if (!_server.StartsWith("opc.tcp://", StringComparison.InvariantCulture))
                server = "opc.tcp://";
            server += _server;
            if (!_server.EndsWith(":4840", StringComparison.InvariantCulture))
                server += ":4840";

            await _client.ConnectServer(server).ConfigureAwait(false);

            _requestString = $"ns=4;s=|var|{FindControllerName(_client.Session, ObjectIds.ObjectsFolder)}.Application.";

            Connected = true;

        }
        catch (Opc.Ua.ServiceResultException ex)
        {
#if DEBUG
            Debug.WriteLine(MachineCommunication.Localization.Language.ConnectionErrorMessage + $" ({ex.Message})");
#endif
            Connected = false;
            Application.Current.Dispatcher.Invoke(() =>
            {
                _statusService.AddStatus(
                    new Status(
                        MachineCommunication.Localization.Language.ConnectionErrorMessage + $" ({ex.Message})",
                        StatusType.Error));
                });
        }
        finally
        {
            _connectGate.Release();
        }

        RunUpdateTask();
    }

    public async Task<bool> TryConnectAsync(CancellationToken token)
    {
        if (_connectGate.CurrentCount == 0)
            return _client.Connected;

        bool serverIsRunning = false;

        while (!token.IsCancellationRequested)
        {
            try
            {
                string server = _server;
                if (server.Contains("localhost", StringComparison.InvariantCultureIgnoreCase))
                    server = "127.0.0.1";

                if (server.StartsWith("opc.tcp://", StringComparison.InvariantCultureIgnoreCase))
                    server = server.Replace("opc.tcp://", "", StringComparison.InvariantCultureIgnoreCase);
                if (server.EndsWith(":4840", StringComparison.InvariantCultureIgnoreCase))
                    server = server.Replace(":4840", "", StringComparison.InvariantCultureIgnoreCase);

                using var pinger = new Ping();
                var reply = await pinger.SendPingAsync(server).ConfigureAwait(false);
                serverIsRunning = reply.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                // Ignore
            }

            if (serverIsRunning)
                await ConnectAsync().ConfigureAwait(false);

            return _client.Connected;
        }
        return _client.Connected;
    }

    public async Task UpdateConnection(string server)
    {
        if (_client.Connected)
        {
            _client.Disconnect();
            Connected = false;
        }

        _server = server;

        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        Connected = await TryConnectAsync(cts.Token).ConfigureAwait(false);
    }

    public async Task WriteAsync<T>(T value, string to, StatusPage fromPage = StatusPage.Unknown)
    {
        if (!_client.Connected)
            return;

        if (string.IsNullOrEmpty(to))
            return;

        try
        {
            await _client.WriteNodeAsync<T>(_requestString + to, value).ConfigureAwait(false);
        }
        catch (Opc.Ua.ServiceResultException)
        {
#if DEBUG
            Debug.WriteLine(MachineCommunication.Localization.Language.SendRequestErrorMessage);
#endif
            Application.Current.Dispatcher.Invoke(() =>
                _statusService.AddStatus(
                    new Status(
                        MachineCommunication.Localization.Language.SendRequestErrorMessage,
                        StatusType.Error,
                        fromPage)));
        }
    }

    public async Task<T?> ReadAsync<T>(string from, StatusPage fromPage = StatusPage.Unknown)
    {
        if (!_client.Connected)
            return default;

        if (string.IsNullOrEmpty(from))
            return default;

        try
        {
            return await _client.ReadNodeAsync<T>(_requestString + from)
                .ConfigureAwait(false);
        }
        catch (Opc.Ua.ServiceResultException)
        {
#if DEBUG
            Debug.WriteLine(MachineCommunication.Localization.Language.GetDataRequestErrorMessage + $" {from}");
#endif

            Application.Current.Dispatcher.Invoke(() =>
                _statusService.AddStatus(
                    new Status(
                        MachineCommunication.Localization.Language.GetDataRequestErrorMessage + $" {from}",
                        StatusType.Error,
                        fromPage)));
        }

        return default;
    }

    public bool Subscribe<T>(string to, Action<T> setValue)
    {
        if (_client is null || !_client.Connected || string.IsNullOrEmpty(_requestString))
            return false;

        if (string.IsNullOrEmpty(to))
            return false;

        Debug.WriteLine($"Current subscribtion count: {_client.Session.SubscriptionCount}");
        try
        {
            _client.AddSubscription(to, _requestString + to, (_, _, args) =>
            {
                var notification = args.NotificationValue as MonitoredItemNotification;
                if (notification is null)
                    return;

                var raw = notification.Value.WrappedValue.Value;
                if (raw is T result)
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        setValue(result);
                    });
            });
            return true;
        }
        catch (Opc.Ua.ServiceResultException ex)
        {
            Debug.WriteLine($"Error: {ex.GetType()} {ex.Message}");
        }

        return false;
    }

    public void Unsubscribe(string from)
    {
        if (_client is null || !_client.Connected)
            return;

        try
        {
            _client.RemoveSubscription(from);
            Debug.WriteLine($"Unsubscribed {from} with current subscribtion count {_client.Session.SubscriptionCount}");
        }
        catch (Opc.Ua.ServiceResultException ex)
        {
            Debug.WriteLine($"Error: {ex.GetType()} {ex.Message}");
        }
    }

    private void RunUpdateTask()
    {
        if (_timer is not null)
            return;

        _timer = new DispatcherTimer(
            TimeSpan.FromSeconds(5),
            DispatcherPriority.Background,
            async (_, _) =>
            {
                Debug.WriteLine("manual timer works!");

                if (_client.Connected)
                {
                    _statusService.RemoveStatus(new Status(MachineCommunication.Localization.Language.ConnectionErrorMessage));
                }
                else
                {
                    Connected = false;
                    using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                    Connected = await TryConnectAsync(cts.Token).ConfigureAwait(false);
                }
            },
            Application.Current.Dispatcher);
        _timer.Start();
    }

    private static string? FindControllerName(ISession session, NodeId nodeId)
    {
        string controllerName = string.Empty;
        session.Browse(
            null,
            null,
            nodeId,
            0,
            BrowseDirection.Forward,
            ReferenceTypeIds.HierarchicalReferences,
            true,
            (uint)(NodeClass.Object | NodeClass.Variable),
            out var cp,
            out var refs);

        foreach (var r in refs)
        {
            if (!r.BrowseName.Name.Contains("DeviceSet", StringComparison.InvariantCultureIgnoreCase))
                continue;

            var childId = ExpandedNodeId.ToNodeId(r.NodeId, session.NamespaceUris);
            if (childId is not null)
            {
                session.Browse(
                    null,
                    null,
                    childId,
                    0,
                    BrowseDirection.Forward,
                    ReferenceTypeIds.HierarchicalReferences,
                    true,
                    (uint)(NodeClass.Object | NodeClass.Variable),
                    out var cpChild,
                    out var refsChild);
                if (refsChild.Count > 0)
                    return refsChild[0].BrowseName.Name;
            }
        }

        return controllerName;
    }

    partial void OnConnectedChanged(bool value)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            if (value)
            {
                _statusService.RemoveStatusThatsContains(new Status(MachineCommunication.Localization.Language.ConnectionErrorMessage, StatusType.Error));
                _statusService.AddStatus(new Status("Подключение успешно", StatusType.Success));
            }
            else
            {
                _statusService.RemoveStatus(new Status("Подключение успешно", StatusType.Success));
            }
        });
    }
}