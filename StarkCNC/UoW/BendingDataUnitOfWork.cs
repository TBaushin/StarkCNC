using StarkCNC.Core.Models;
using StarkCNC.Core.Services;
using StarkCNC.Core.UoW;

namespace StarkCNC.UoW;

public class BendingDataUnitOfWork : IBendingDataUnitOfWork
{
    private IGCodeService _gCodeService;

    public ICollection<BendingData> BendingDatas { get; } = new List<BendingData>();

    public BendingDataUnitOfWork(IGCodeService gCodeService)
    {
        _gCodeService = gCodeService;
    }

    public double CalculateEstimatedRemainingLength(double pipeLength)
    {
        double result = pipeLength;
        foreach (var data in BendingDatas)
        {
            result -= data.Supply + (2 * Math.PI * data.BendingRadius / 360 * data.BendingAngle);
        }

        return result;
    }

    public double CalculatePipeLength(double estimatedRemainingLength)
    {
        double result = estimatedRemainingLength;
        foreach (var data in BendingDatas)
        {
            result += data.Supply - (2 * Math.PI * data.BendingRadius / 360 * data.BendingAngle);
        }

        return result;
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
