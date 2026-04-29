using StarkCNC.Core.Models;

namespace StarkCNC.MachineCommunication.Services;

public interface IManualConfigurationService
{
    public bool Connected { get; }

    public Task ConnectAsync();

    public Task<bool> TryConnectAsync();

    public Task UpdateConnection(string server);

    public Task WriteAsync<T>(T value, string to, StatusPage fromPage = StatusPage.Unknown);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="from"></param>
    /// <returns></returns>
    public Task<T?> ReadAsync<T>(string from, StatusPage fromPage = StatusPage.Unknown);

    public bool Subscribe<T>(string to, Action<T> setValue);

    public void Unsubscribe(string from);
}