using StarkCNC.Core.Models;
using System.Globalization;
using System.IO;
using System.Text;

namespace StarkCNC.Core.Services;

public interface ICSVService
{
    public static async Task Export(string filepath, List<BendingData> data)
    {
        var csvBuilder = new StringBuilder();

        string header = "type,podacha,povorot,gib,speed,koef,vibeg,radius_Gibki,m,dlinaTrubi,ust_Trubi," +
                        string.Join(",", Enumerable.Range(3, 48).Select(n => $"param{n}"));

        csvBuilder.Append(header).Append("\n\r");

        foreach (var item in data)
        {
            var line = string.Format(CultureInfo.InvariantCulture,
                "{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}," +
                "{11},{12},{13},{14},{15},{16},{17},{18},{19},{20}," +
                "{21},{22},{23},{24},{25},{26},{27},{28},{29},{30}," +
                "{31},{32},{33},{34},{35},{36},{37},{38},{39},{40}," +
                "{41},{42},{43},{44},{45},{46},{47},{48},{49},{50}," +
                "{51},{52},{53},{54},{55},{56},{57},{58}",

                item.BendingAngleCoefficient,
                item.Supply,
                item.Offset,
                item.BendingAngle,
                item.SupplySpeed,
                item.OffsetSpeed,
                item.OffsetCoefficient,
                item.BendingAngleSpeed,
                0,
                item.PipeLength,
                item.YSetup,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
            );
            csvBuilder.Append(line).Append("\n\r");
        }

        await File.WriteAllTextAsync(filepath, csvBuilder.ToString(), Encoding.UTF8);
    }

    public static async Task<List<BendingData>> Import(string filepath)
    {
        if (!IsCSV(filepath))
            throw new FileFormatException($"Файл {new FileInfo(filepath).Name} не в формате csv или его не существует");

        var fileText = await File.ReadAllTextAsync(filepath).ConfigureAwait(false);
        if (string.IsNullOrEmpty(fileText))
            throw new FileLoadException($"Не удалось прочесть файл {new FileInfo(filepath).Name}");

        var data = new List<BendingData>();
        var lines = fileText.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 1; i < lines.Length; i++)
        {
            var values = lines[i].Split(',');
            if (values.Length > 59)
                throw new FileFormatException($"Файл {new FileInfo(filepath).Name} имеет неверный формат. Столбцов должно быть не больше 59, а найдено {values.Length}");


            var bdCSV = new BendingDataCSV(
                Convert.ToDouble(values[0]),Convert.ToDouble(values[1]), Convert.ToDouble(values[2]), Convert.ToDouble(values[3]),
                Convert.ToDouble(values[4]), Convert.ToDouble(values[5]), Convert.ToDouble(values[6]), Convert.ToDouble(values[7]),
                Convert.ToInt32(values[8]), Convert.ToDouble(values[9]), Convert.ToDouble(values[10]), Convert.ToDouble(values[11]),
                Convert.ToDouble(values[12]), Convert.ToDouble(values[13]), Convert.ToDouble(values[14]), Convert.ToDouble(values[15]),
                Convert.ToDouble(values[16]), Convert.ToDouble(values[17]), Convert.ToDouble(values[18]), Convert.ToDouble(values[19]),
                Convert.ToDouble(values[20]), Convert.ToDouble(values[21]), Convert.ToDouble(values[22]), Convert.ToDouble(values[23]),
                Convert.ToDouble(values[24]), Convert.ToDouble(values[25]), Convert.ToDouble(values[26]), Convert.ToDouble(values[27]),
                Convert.ToDouble(values[28]), Convert.ToDouble(values[29]), Convert.ToDouble(values[30]), Convert.ToDouble(values[31]),
                Convert.ToDouble(values[32]), Convert.ToDouble(values[33]), Convert.ToDouble(values[34]), Convert.ToDouble(values[35]),
                Convert.ToDouble(values[36]), Convert.ToDouble(values[37]), Convert.ToDouble(values[38]), Convert.ToDouble(values[39]),
                Convert.ToDouble(values[40]), Convert.ToDouble(values[41]), Convert.ToDouble(values[42]), Convert.ToDouble(values[43]),
                Convert.ToDouble(values[44]), Convert.ToDouble(values[45]), Convert.ToDouble(values[46]), Convert.ToDouble(values[47]),
                Convert.ToDouble(values[48]), Convert.ToDouble(values[49]), Convert.ToDouble(values[50]), Convert.ToDouble(values[51]),
                Convert.ToDouble(values[52]), Convert.ToDouble(values[53]), Convert.ToDouble(values[54]), Convert.ToDouble(values[55]),
                Convert.ToDouble(values[56]), Convert.ToDouble(values[57]), Convert.ToDouble(values[58]));

            data.Add(bdCSV.ToBendingData());
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
}
