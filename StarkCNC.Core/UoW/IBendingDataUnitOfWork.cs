using StarkCNC.Core.Models;

namespace StarkCNC.Core.UoW;

public interface IBendingDataUnitOfWork
{
    public ICollection<BendingData> BendingDatas { get; }

    public Task ReadFileAsync(string filePath);

    public Task WriteFileAsync(string filePath);

    public double CalculateEstimatedRemainingLength(double pipeLength);

    public double CalculatePipeLength(double estimatedRemainingLength);
}
