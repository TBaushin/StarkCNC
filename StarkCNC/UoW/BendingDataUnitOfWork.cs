using StarkCNC.Core.Models;
using StarkCNC.Core.Services;
using StarkCNC.Core.UoW;

namespace StarkCNC.UoW;

public class BendingDataUnitOfWork : IBendingDataUnitOfWork
{
    private IGCodeService _gCodeService;

    public ICollection<BendingData> BendingDatas { get; } = new List<BendingData>();

    public double PipeLength { get; set; }

    public double EstimatedRemainingLength { get; set; }

    public BendingDataUnitOfWork(IGCodeService gCodeService)
    {
        _gCodeService = gCodeService;
    }

    public double CalculateEstimatedRemainingLength()
    {
        double result = PipeLength;
        foreach (var data in BendingDatas)
        {
            result -= data.Supply + (2 * Math.PI * data.BendingRadius / 360 * data.BendingAngle);
        }

        EstimatedRemainingLength = result;
        return EstimatedRemainingLength;
    }

    public double CalculatePipeLength()
    {
        double result = EstimatedRemainingLength;
        foreach (var data in BendingDatas)
        {
            result += data.Supply - (2 * Math.PI * data.BendingRadius / 360 * data.BendingAngle);
        }

        PipeLength = result;
        return PipeLength;
    }

    public async Task ReadFileAsync(string filePath)
    {
        BendingDatas.Clear();
        
        var data = await _gCodeService.ReadAsync(filePath).ConfigureAwait(false);
        if (data is null)
            return;

        foreach(var d in data)
        {
            BendingDatas.Add(d);
        }
    }

    public async Task WriteFileAsync(string filePath)
    {
        await _gCodeService.SaveAsync(filePath, BendingDatas).ConfigureAwait(false);
    }
}
