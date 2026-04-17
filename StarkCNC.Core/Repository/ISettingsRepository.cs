using StarkCNC.Core.Models;

namespace StarkCNC.Core.Repository;

public interface ISettingsRepository
{
    Task<Settings?> AddElementAsync(Settings settings);

    Task UpdateElementAsync(Settings settings);

    Task<Settings?> FindByIdAsync(Guid id);

    Task<Settings?> GetAsync();

    Settings? Get();

    int Count();
}