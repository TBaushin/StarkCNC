using StarkCNC.Core.Models;
using StarkCNC.Core.Services;
using System.IO;
using System.Text;

namespace StarkCNC.Services;

public class GCodeService : IGCodeService
{
    public async Task SaveAsync(string path, ICollection<BendingData> data)
    {
        if (data is null)
            return;

        var builder = new StringBuilder();
        foreach (var item in data)
        {
            var str = ConvertToSaveString(item);
            builder.AppendLine(str);
        }

        if (!File.Exists(path))
        {
            var fs = File.Create(path);
            fs.Close();
        }

        using StreamWriter writer = new StreamWriter(path);
        await writer.WriteAsync(builder.ToString()).ConfigureAwait(false);
        writer.Close();
    }

    public async Task<ICollection<BendingData>> ReadAsync(string path)
    {
        var data = new List<BendingData>();
        using StreamReader reader = new StreamReader(path);
        string content = await reader.ReadToEndAsync().ConfigureAwait(false);

        List<string> rawContent = content
            .Split([Environment.NewLine], StringSplitOptions.RemoveEmptyEntries)
            .ToList();
        rawContent.ForEach(c =>
        {
            data.Add(ConvertToBendingData(c));
        });

        reader.Close();
        return data;
    }

    private static IEnumerable<KeyValuePair<string, object>> ConvertToSave(BendingData data) =>
        new Dictionary<string, object>
        {
            { "L", data.PipeLength },
            { "Y0", data.YSetup },
            { "Y", data.Supply },
            { "Ys", data.SupplySpeed },
            { "Y1", data.Offset },
            { "Y1b", data.OffsetSpeed },
            { "Y2", data.OffsetCoefficient },
            { "C", data.BendingAngle },
            { "Cs", data.BendingAngleSpeed },
            { "Ck", data.BendingAngleCoefficient },
            { "R", data.BendingRadius },
            { "M", data.BendingRadiusMode },
            { "B", data.RotationAngle },
            { "Bs", data.RotationSpeed }
        };

    private static string ConvertToSaveString(BendingData data)
    {
        var dataDict = ConvertToSave(data);

        var builder = new StringBuilder();
        foreach (var item in dataDict)
        {
            if (item.Key.Length > 1 && item.Key != "M")
                builder = builder.Append($"{item.Key}={item.Value} ");
            else if (item.Key == "M")
                builder = builder.Append($"{item.Key}='{item.Value}' ");
            else
                builder = builder.Append($"{item.Key}{item.Value} ");
        }

        return builder.ToString().TrimEnd();
    }

    private static IEnumerable<KeyValuePair<string, object>> ReadString(string data)
    {
        var result = new List<KeyValuePair<string, object>>();
        var parts = data.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        foreach (var item in parts)
        {
            if (item.Contains('=', StringComparison.CurrentCulture))
            {
                var keyValue = item.Split('=', StringSplitOptions.RemoveEmptyEntries);
                if (keyValue.Length == 2 && keyValue[0] != "M")
                {
                    var key = keyValue[0];
                    var valueString = keyValue[1];
                    if (double.TryParse(valueString, out double doubleValue))
                        result.Add(new KeyValuePair<string, object>(key, doubleValue));
                    else
                        result.Add(new KeyValuePair<string, object>(key, valueString));
                }
                else
                {
                    var key = keyValue[0];
                    var valueString = keyValue[1];
                    valueString = valueString.Replace("'", "", StringComparison.CurrentCulture);
                    result.Add(new KeyValuePair<string, object>(key, valueString));
                }
            }
            else
            {
                var key = item.Substring(0, 1);
                var valueString = item.Substring(1);
                if (double.TryParse(valueString, out double doubleValue))
                    result.Add(new KeyValuePair<string, object>(key, doubleValue));
            }
        }

        return result;
    }

    private static BendingData ConvertToBendingData(string data)
    {
        var keyValue = ReadString(data);

        var bendingData = new BendingData();

        foreach (var item in keyValue)
        {
            switch (item.Key)
            {
                case "L":
                    bendingData.PipeLength = (double)item.Value;
                    break;
                case "Y0":
                    bendingData.YSetup = (double)item.Value;
                    break;
                case "Y":
                    bendingData.Supply = (double)item.Value;
                    break;
                case "Ys":
                    bendingData.SupplySpeed = (double)item.Value;
                    break;
                case "Y1":
                    bendingData.Offset = (double)item.Value;
                    break;
                case "Y1b":
                    bendingData.OffsetSpeed = (double)item.Value;
                    break;
                case "Y2":
                    bendingData.OffsetCoefficient = (double)item.Value;
                    break;
                case "C":
                    bendingData.BendingAngle = (double)item.Value;
                    break;
                case "Cs":
                    bendingData.BendingAngleSpeed = (double)item.Value;
                    break;
                case "Ck":
                    bendingData.BendingAngleCoefficient = (double)item.Value;
                    break;
                case "R":
                    bendingData.BendingRadius = (double)item.Value;
                    break;
                case "M":
                    bendingData.BendingRadiusMode = (string)item.Value;
                    break;
                case "B":
                    bendingData.RotationAngle = (double)item.Value;
                    break;
                case "Bs":
                    bendingData.RotationSpeed = (double)item.Value;
                    break;
            }
        }

        return bendingData;
    }
}
