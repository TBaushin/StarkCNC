using StarkCNC.Core.Models;

namespace StarkCNC.Core.UoW;

public interface IBendingDataUnitOfWork
{
    public string CurrentFilePath { get; }

    public string ProgramName { get; }

    public ICollection<BendingData> BendingDatas { get; }

    public double PipeLength { get; set; }

    public double SetUpPoint { get; set; }

    public double EstimatedRemainingLength { get; set; }

    public bool HasUnsavedData { get; set; }

    public Task CreateNewFile();

    public Task OpenFile();

    public Task<bool> SaveFile();

    public Task ReadFileAsync(string filePath);

    public Task WriteFileAsync(string filePath);

    public double CalculateEstimatedRemainingLength();

    public double CalculatePipeLength();
}
