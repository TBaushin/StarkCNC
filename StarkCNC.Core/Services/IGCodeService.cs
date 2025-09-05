using StarkCNC.Core.Models;

namespace StarkCNC.Core.Services;

public interface IGCodeService
{
    public Task SaveAsync(string path, ICollection<BendingData> data);

    public Task<ICollection<BendingData>> ReadAsync(string path);
}
