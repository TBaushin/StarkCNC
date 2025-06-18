using Gcode.Utils;
using Gcode.Utils.Entity;
using StarkCNC.Core.Models;
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
                var gcodeConverted = c.ToGcodeCommandFrame();
                data.Add(new BendingData()
                {
                    StraightLength = (double)gcodeConverted.Y,
                    BendingAngle = (double)gcodeConverted.C,
                    BendingRadius = (double)gcodeConverted.R,
                    RotationAngle = (double)gcodeConverted.B
                });
            });

            reader.Close();
            return data;
        }
    }
}
