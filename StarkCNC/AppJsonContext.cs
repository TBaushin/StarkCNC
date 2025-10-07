using Microsoft.Extensions.Configuration;
using StarkCNC.Core;
using StarkCNC.Core.Models;
using System.IO;
using System.Text.Json;

namespace StarkCNC;

public class AppJsonContext : JsonContext
{
    public JsonSet<AdjustmentParameters> Adjustments { get; set; }

    public AppJsonContext(IConfiguration configuration) : base(configuration)
    {
    }

    public override Task SaveChangesAsync()
    {
        var toSave = GetToSaveElements();

        foreach (var item in toSave)
        {
            if (!Directory.Exists(_savePath + "\\" + item.Key))
                Directory.CreateDirectory(_savePath + "\\" + item.Key);
            var currentSavePath = _savePath + "\\" + item.Value;

            var set = item.Value as JsonSet<object>;
            
            if (set is null)
                continue;

            set.ForEach(async e =>
            {
                var entityEntryValue = e.GetType().GetProperty("Value");
                if (entityEntryValue is not null)
                {
                    var value = entityEntryValue.GetValue(e) as AdjustmentParameters;
                    if (value is not null)
                        await SaveAdjustmentParametersByName(value, currentSavePath).ConfigureAwait(false);
                    else
                        await SaveObject(value, currentSavePath + "\\" + _defaultFileName).ConfigureAwait(false);
                }
            });
        }

        UpdateStatesAfterSave();

        return Task.CompletedTask;
    }

    private static async Task SaveAdjustmentParametersByName(AdjustmentParameters adjustment, string savePath)
    {
        var json = JsonSerializer.Serialize(adjustment, new JsonSerializerOptions { WriteIndented = true });

        savePath += "\\" + adjustment.Name + ".json";

        await File.WriteAllTextAsync(savePath, json).ConfigureAwait(false);
    }

    private static async Task SaveObject(object? obj, string savePath)
    {
        var json = JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true });

        await File.WriteAllTextAsync(savePath, json).ConfigureAwait(false);
    }
}
