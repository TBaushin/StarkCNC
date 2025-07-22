using Gcode.Utils;
using Gcode.Utils.Entity;
using StarkCNC.Core.Models;
using System.IO;
using System.Text;

namespace StarkCNC.Core.Services
{
    public class GCodeService : IGCodeService
    {
        public async Task SaveAsync(string path, ICollection<BendingData> data)
        {
            var gcodes = new List<GcodeCommandFrame>();
            foreach (var item in data)
            {
                gcodes.Add(new GcodeCommandFrame()
                { 
                    Y = item.StraightLength, 
                    C = item.BendingAngle, 
                    R = item.BendingRadius, 
                    B = item.RotationAngle
                });
            }

            var gcodeSB = new StringBuilder();
            gcodes.ForEach(gcode =>
            {
                gcodeSB.AppendLine(gcode.ToString());
            });

            if (!File.Exists(path))
            {
                var fs = File.Create(path);
                fs.Close();
            }

            await using StreamWriter writer = new StreamWriter(path);
            await writer.WriteAsync(gcodeSB.ToString());
            writer.Close();
        }

        public async Task<ICollection<BendingData>> ReadAsync(string path)
        {
            var data = new List<BendingData>();
            using StreamReader reader = new StreamReader(path);
            string content = await reader.ReadToEndAsync();

            List<string> rawContent = content.Split([Environment.NewLine], StringSplitOptions.None).ToList();
            rawContent.ForEach(c =>
            {
                if (!string.IsNullOrEmpty(c))
                {
                    var gcodeConverted = c.ToGcodeCommandFrame();

                    data.Add(new BendingData()
                    {
                        StraightLength = GetValueOrDefault(gcodeConverted, g => g.Y),
                        BendingAngle = GetValueOrDefault(gcodeConverted, g => g.C),
                        BendingRadius = GetValueOrDefault(gcodeConverted, g => g.R),
                        RotationAngle = GetValueOrDefault(gcodeConverted, g => g.B)
                    });
                }
            });

            reader.Close();
            return data;
        }

        private static double GetValueOrDefault(GcodeCommandFrame frame, Func<GcodeCommandFrame, double?> selector)
        {
            if (frame is null)
                return 0;

            var value = selector(frame);
            if (value is null)
                return 0;

            return (double)value;
        }
    }
}
