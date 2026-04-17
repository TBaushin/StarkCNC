using CsvHelper;
using StarkCNC.Core.Models;
using System.Globalization;
using System.IO;

namespace StarkCNC.Core.Services;

public interface ICSVService
{
    public static async Task Export(string filepath, IEnumerable<BendingData> data)
    {
        using var writer = new StreamWriter(filepath);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        var csvData = new List<BendingDataCSV>();
        foreach (var item in data)
        {
            csvData.Add(BendingDataCSV.FromBendingData(item));
        }
        await csv.WriteRecordsAsync(csvData).ConfigureAwait(false);
    }

    public static IEnumerable<BendingData> Import(string filepath)
    {
        if (!IsCSV(filepath))
            throw new FileFormatException($"Файл {new FileInfo(filepath).Name} не в формате csv или его не существует");

        using var reader = new StreamReader(filepath);
        using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
        var bdCSV = csvReader.GetRecords<BendingDataCSV>();
        var data = new List<BendingData>();
        foreach (var item in bdCSV)
        {
            data.Add(item.ToBendingData());
        }

        return data;
    }

    private static bool IsCSV(string filepath)
    {
        var file = new FileInfo(filepath);
        if (file.Exists && file.Extension.ToLower() == ".csv")
            return true;
        return false;
    }
}

internal record struct BendingDataCSV(float type, float podacha, float povorot, float gib, float speed, float koef, float vibeg, float radius_Gibki, int m, float dlinaTrubi, float ust_Trubi, float param3, float param4, float param5, float param6, float param7, float param8, float param9, float param10, float param11, float param12, float param13, float param14, float param15, float param16, float param17, float param18, float param19, float param20, float param21, float param22, float param23, float param24, float param25, float param26, float param27, float param28, float param29, float param30, float param31, float param32, float param33, float param34, float param35, float param36, float param37, float param38, float param39, float param40, float param41, float param42, float param43, float param44, float param45, float param46, float param47, float param48, float param49, float param50)
{
    public BendingData ToBendingData() =>
        new BendingData
        {
            PipeLength = dlinaTrubi,
            YSetup = ust_Trubi,
            Supply = podacha,
            SupplySpeed = speed,
            Offset = povorot,
            OffsetSpeed = koef,
            OffsetCoefficient = vibeg,
            BendingAngle = gib,
            BendingAngleSpeed = 0,
            BendingAngleCoefficient = type,
            BendingRadius = radius_Gibki,
            BendingRadiusMode = "",
            RotationAngle = 0,
            RotationSpeed = 0
        };

    public static BendingDataCSV FromBendingData(BendingData data) =>
        new BendingDataCSV(0, data.Supply, data.Offset, data.BendingAngle, data.SupplySpeed, data.OffsetSpeed, data.OffsetCoefficient, data.BendingRadius, 0, data.PipeLength, data.YSetup, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
}
