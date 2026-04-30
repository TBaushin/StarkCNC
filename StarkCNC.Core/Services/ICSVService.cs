using CsvHelper;
using CsvHelper.Configuration;
using StarkCNC.Core.Models;
using System.Globalization;
using System.IO;

namespace StarkCNC.Core.Services;

public interface ICSVService
{
    private static CsvConfiguration configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
    {
        ShouldQuote = (args) => args.Field.StartsWith("Param") || new List<string>()
        {
            "Type", "Podacha", "Povorot", "Gib", "Speed", "Koef", "Vibeg", "Radius_Gibki", "M", "DlinaTrubi", "Ust_Trubi"
        }.Contains(args.Field)
    };

    public static async Task Export(string filepath, IEnumerable<BendingData> data)
    {
        using var writer = new StreamWriter(filepath);
        using var csv = new CsvWriter(writer, configuration);
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
        using var csvReader = new CsvReader(reader, configuration);
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

internal record struct BendingDataCSV(float Type, float Podacha, float Povorot, float Gib, float Speed, float Koef, float Vibeg, float Radius_Gibki, int M, float DlinaTrubi, float Ust_Trubi, float Param3, float Param4, float Param5, float Param6, float Param7, float Param8, float Param9, float Param10, float Param11, float Param12, float Param13, float Param14, float Param15, float Param16, float Param17, float Param18, float Param19, float Param20, float Param21, float Param22, float Param23, float Param24, float Param25, float Param26, float Param27, float Param28, float Param29, float Param30, float Param31, float Param32, float Param33, float Param34, float Param35, float Param36, float Param37, float Param38, float Param39, float Param40, float Param41, float Param42, float Param43, float Param44, float Param45, float Param46, float Param47, float Param48, float Param49, float Param50)
{
    public BendingData ToBendingData() =>
        new BendingData
        {
            PipeLength = DlinaTrubi,
            YSetup = Ust_Trubi,
            Supply = Podacha,
            SupplySpeed = Speed,
            Offset = Povorot,
            OffsetSpeed = Koef,
            OffsetCoefficient = Vibeg,
            BendingAngle = Gib,
            BendingAngleSpeed = 0,
            BendingAngleCoefficient = Type,
            BendingRadius = Radius_Gibki,
            BendingRadiusMode = "",
            RotationAngle = 0,
            RotationSpeed = 0
        };

    public static BendingDataCSV FromBendingData(BendingData data) =>
        new BendingDataCSV(0, data.Supply, data.Offset, data.BendingAngle, data.SupplySpeed, data.OffsetSpeed, data.OffsetCoefficient, data.BendingRadius, 0, data.PipeLength, data.YSetup, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
}
