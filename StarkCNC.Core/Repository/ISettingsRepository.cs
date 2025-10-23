using StarkCNC.Core.Models;

namespace StarkCNC.Core.Repository;

public interface ISettingsRepository
{
    Task AddElementAsync(Settings settings);

    Task UpdateElementAsync(Settings settings);

    Task<Settings?> FindByIdAsync(Guid id);

    Task<Settings?> GetAsync();

    int Count();

    static bool CanBeAdded(IEnumerable<Settings> settingsCollection) =>
        !settingsCollection.Any();
}