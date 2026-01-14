using StarkCNC.Core.Models;

namespace StarkCNC.Core.UoW;

public interface IBendingDataUnitOfWork
{
    public string CurrentFilePath { get; }

    public string ProgramName { get; }

    public ICollection<BendingData> BendingDatas { get; }

    public double PipeLength { get; set; }

    public double EstimatedRemainingLength { get; set; }

    public Task ReadFileAsync(string filePath);

    public Task WriteFileAsync(string filePath);

    public double CalculateEstimatedRemainingLength();

    public double CalculatePipeLength();
}
