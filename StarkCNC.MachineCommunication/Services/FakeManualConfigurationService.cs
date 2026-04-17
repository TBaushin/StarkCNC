using StarkCNC.Core.Models;
using StarkCNC.Core.Services;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Threading;

namespace StarkCNC.MachineCommunication.Services;

[SuppressMessage("Usage", "CA5394", Justification = "Не нужна безопасность")]
public class FakeManualConfigurationService : IManualConfigurationService
{
    private readonly IStatusService _statusService;

    private readonly Dictionary<string, DispatcherTimer> _subscribtions = new Dictionary<string, DispatcherTimer>();

    public bool Connected => GetRandomBool();

    public FakeManualConfigurationService(IStatusService statusService)
    {
        _statusService = statusService;
    }

    public async Task ConnectAsync()
    {
        await Task.Delay(1000).ConfigureAwait(true);
        _statusService.CurrentStatus = new Status("Подключение успешно");
    }

    public Task<T?> ReadAsync<T>(string from, StatusPage fromPage = StatusPage.Unknown)
    {
        object? result = typeof(T) switch
        {
            var t when t == typeof(bool) => GetRandomBool(),
            var t when t == typeof(int) => GetRandomInt(),
            var t when t == typeof(double) => GetRandomDouble(),
            var t when t == typeof(float) => GetRandomFloat(),
            _ => default(T)
        };

        return Task.FromResult((T?)result);
    }

    public async Task WriteAsync<T>(T value, string to, StatusPage fromPage = StatusPage.Unknown)
    {
        Debug.WriteLine($"Запрос {to} со значением {value} принят");
        await Task.Delay(100).ConfigureAwait(true);
    }

    public void Subscribe<T>(string to, Action<T> setValue)
    {
        var sub = new DispatcherTimer(
            TimeSpan.FromSeconds(1),
            DispatcherPriority.Normal, async (_, _) =>
            {
                var result = await ReadAsync<T>(to).ConfigureAwait(true);
                if (result is not null)
                    setValue(result);
            },
            Application.Current.Dispatcher);
        _subscribtions.TryAdd(to, sub);
    }

    public void Unsubscribe(string from)
    {
        _subscribtions.TryGetValue(from, out var sub);
        if (sub is not null)
        {
            sub.Stop();
        }
        _subscribtions.Remove(from);
    }

    private static bool GetRandomBool()
    {
        var random = new Random();
        var v = random.Next(2);
        return Convert.ToBoolean(v);
    }

    private static int GetRandomInt()
    {
        var random = new Random();
        var v = random.Next(100);
        return v;
    }

    private static double GetRandomDouble()
    {
        var random = new Random();
        var v = random.NextDouble();
        return v * 100;
    }

    private static float GetRandomFloat()
    {
        var random = new Random();
        var v = random.NextDouble();
        return Convert.ToSingle(v * 100);
    }
}