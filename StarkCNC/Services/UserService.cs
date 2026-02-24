using Microsoft.AspNetCore.Identity;
using StarkCNC.Core.Repository;
using StarkCNC.Core.Services;

namespace StarkCNC.Services;

public class UserService : IUserService
{
    private readonly IUsersRepository _repository;

    public IdentityUser? CurrentUser { get; set; }

    public UserService(IUsersRepository usersRepository)
    {
        _repository = usersRepository;
    }

    public async Task<IdentityUser?> AddElementAsync(IdentityUser user) =>
        await _repository.AddElementAsync(user).ConfigureAwait(false);

    public async Task<IdentityUser?> FindByIdAsync(Guid id) =>
        await _repository.FindByIdAsync(id).ConfigureAwait(false);

    public async Task<IdentityUser?> FindByNameAsync(string name) =>
        await _repository.FindByNameAsync(name).ConfigureAwait(false);

    public async Task<IEnumerable<IdentityUser>> GetAllAsync() =>
        await _repository.GetAllAsync().ConfigureAwait(false);

    public async Task RemoveElementAsync(Guid id) =>
        await _repository.RemoveElementAsync(id).ConfigureAwait(false);

    public async Task RemoveElementAsync(IdentityUser user) =>
        await _repository.RemoveElementAsync(user).ConfigureAwait(false);

    public async Task UpdateElementAsync(IdentityUser user) =>
        await _repository.UpdateElementAsync(user).ConfigureAwait(false);
}
