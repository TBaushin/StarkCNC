namespace StarkCNC.MachineCommunication.Services;

public interface IManualConfigurationService
{
    public bool Connected { get; }

    public Task ConnectAsync();

    public Task WriteAsync<T>(T value, string to);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="from"></param>
    /// <returns></returns>
    public Task<T?> ReadAsync<T>(string from);
}
