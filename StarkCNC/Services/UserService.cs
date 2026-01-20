using StarkCNC.Core.Models;
using StarkCNC.Core.Repository;
using StarkCNC.Core.Services;

namespace StarkCNC.Services;

public class UserService : IUserService
{
    private readonly IUsersRepository _repository;

    public User? CurrentUser { get; set; }

    public UserService(IUsersRepository usersRepository)
    {
        _repository = usersRepository;
    }

    public async Task<User?> AddElementAsync(User user) =>
        await _repository.AddElementAsync(user).ConfigureAwait(false);

    public async Task<User?> FindByIdAsync(Guid id) =>
        await _repository.FindByIdAsync(id).ConfigureAwait(false);

    public async Task<IEnumerable<User>> FindByNameAsync(string name) =>
        await _repository.FindByNameAsync(name).ConfigureAwait(false);

    public async Task<IEnumerable<User>> GetAllAsync() =>
        await _repository.GetAllAsync().ConfigureAwait(false);

    public async Task RemoveElementAsync(Guid id) =>
        await _repository.RemoveElementAsync(id).ConfigureAwait(false);

    public async Task RemoveElementAsync(User user) =>
        await _repository.RemoveElementAsync(user).ConfigureAwait(false);

    public async Task UpdateElementAsync(User user) =>
        await _repository.UpdateElementAsync(user).ConfigureAwait(false);
}
