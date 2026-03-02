using Microsoft.AspNetCore.Identity;

namespace StarkCNC.Core.Repository;

public interface IUsersRepository
{
    Task<IdentityUser?> AddElementAsync(IdentityUser user);

    Task RemoveElementAsync(Guid id);

    Task RemoveElementAsync(IdentityUser user);

    Task UpdateElementAsync(IdentityUser user);

    Task<IdentityUser?> FindByNameAsync(string name);

    Task<IdentityUser?> FindByIdAsync(Guid id);

    Task<IEnumerable<IdentityUser>> GetAllAsync();
}
