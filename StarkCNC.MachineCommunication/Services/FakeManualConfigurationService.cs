using StarkCNC.Core.Models;
using StarkCNC.Core.Services;
using System.Diagnostics;

namespace StarkCNC.MachineCommunication.Services;

public class FakeManualConfigurationService : IManualConfigurationService
{
    private readonly IStatusService _statusService;

    public bool Connected => GetRandomBool();

    public FakeManualConfigurationService(IStatusService statusService)
    {
        _statusService = statusService;
    }

    public async Task ConnectAsync()
    {
        await Task.Delay(1000);
        _statusService.CurrentStatus = new Status("Подключение успешно");
    }

    public Task<T?> ReadAsync<T>(string from)
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

    public async Task WriteAsync<T>(T value, string to)
    {
        Debug.WriteLine($"Запрос {to} со значением {value} принят");
        await Task.Delay(100);
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

    private static float GetRandomFloat() {
        var random = new Random();
        var v = random.NextDouble();
        return Convert.ToSingle(v * 100);
    }
}