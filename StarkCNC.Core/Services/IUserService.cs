using StarkCNC.Core.Models;

namespace StarkCNC.Core.Services;

public interface IUserService
{
    User? CurrentUser { get; set; }

    Task<User?> AddElementAsync(User user);

    Task RemoveElementAsync(Guid id);

    Task RemoveElementAsync(User user);

    Task UpdateElementAsync(User user);

    Task<IEnumerable<User>> FindByNameAsync(string name);

    Task<User?> FindByIdAsync(Guid id);

    Task<IEnumerable<User>> GetAllAsync();
}
