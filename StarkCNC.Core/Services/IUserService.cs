using Microsoft.AspNetCore.Identity;
using StarkCNC.Core.Models;

namespace StarkCNC.Core.Services;

public interface IUserService
{
    public User? CurrentUser { get; }

    public IdentityRole? CurrentUserRole { get; }

    public Task<User?> Register(string username, string password, string? role = null);

    public Task<bool> Login(string username, string password);

    public Task<bool> Login(User user, string password);

    public Task AddRole(string role);

    public Task AddRole(IdentityRole role);

    public Task RemoveUser(string username);

    public Task RemoveUser(User user);

    public Task RemoveRole(string role);

    public Task RemoveRole(IdentityRole role);

    public Task ChangePassword(string username, string currentPassword, string newPassword);

    public Task ChangePassword(User user, string currentPassword, string newPassword);

    public Task UpdateUser(string currentUsername, string? newUsername = null, IReadOnlyCollection<byte>? image = null);

    public Task UpdateUser(User user);

    public Task UpdateRole(IdentityRole role);

    public Task<User?> FindUserByName(string name);

    public Task<IdentityRole?> GetUserRole(string username);

    public Task<IdentityRole?> GetUserRole(User user);

    public Task<IEnumerable<User>> GetAllUsersAsync();

    public Task<IEnumerable<IdentityRole>> GetAllAvailableRoles();
}
