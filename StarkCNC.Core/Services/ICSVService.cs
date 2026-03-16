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

internal record struct BendingDataCSV(double type, double podacha, double povorot, double gib, double speed, double koef, double vibeg, double radius_Gibki, int m, double dlinaTrubi, double ust_Trubi, double param3, double param4, double param5, double param6, double param7, double param8, double param9, double param10, double param11, double param12, double param13, double param14, double param15, double param16, double param17, double param18, double param19, double param20, double param21, double param22, double param23, double param24, double param25, double param26, double param27, double param28, double param29, double param30, double param31, double param32, double param33, double param34, double param35, double param36, double param37, double param38, double param39, double param40, double param41, double param42, double param43, double param44, double param45, double param46, double param47, double param48, double param49, double param50)
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
