using Microsoft.Extensions.Configuration;
using StarkCNC.Database.Helpers;

namespace StarkCNC.Database;

public class JsonWorker
{
    private static readonly IConfiguration _configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.v2.json", optional: false, reloadOnChange: true)
        .Build();

    private readonly string _savePath;

    private readonly List<IDbHelper> _helpers;

    public JsonWorker()
    {
        _savePath = GetSavePath();
    }

    public async Task SaveAsync()
    {

    }

    public void Save()
    {

    }

    private static string GetSavePath()
    {
        var savePath = _configuration.GetValue<string>("Settings:SaveParameters:Path") ?? string.Empty;
        if (string.IsNullOrEmpty(savePath))
            return AppDomain.CurrentDomain.BaseDirectory;
        return savePath;
    }
}
