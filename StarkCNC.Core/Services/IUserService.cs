using Microsoft.AspNetCore.Identity;

namespace StarkCNC.Core.Services;

public interface IUserService
{
    IdentityUser? CurrentUser { get; set; }

    Task<IdentityUser?> AddElementAsync(IdentityUser user);

    Task RemoveElementAsync(Guid id);

    Task RemoveElementAsync(IdentityUser user);

    Task UpdateElementAsync(IdentityUser user);

    Task<IdentityUser?> FindByNameAsync(string name);

    Task<IdentityUser?> FindByIdAsync(Guid id);

    Task<IEnumerable<IdentityUser>> GetAllAsync();
}
