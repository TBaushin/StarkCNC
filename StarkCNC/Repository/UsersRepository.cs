using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StarkCNC.Core.Repository;
using System.Diagnostics;

namespace StarkCNC.Repository;

public class UsersRepository : IUsersRepository
{
    private UserManager<IdentityUser> _userManager;
    private RoleManager<IdentityRole> _roleManager;

    public UsersRepository(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<IdentityUser?> AddElementAsync(IdentityUser user)
    {
        var result = await _userManager.CreateAsync(user, "").ConfigureAwait(false);
        if (result.Errors.Any())
        {
            Debug.WriteLine(result.Errors.ToList().Select(e => e.Code + " " + e.Description));
            return null;
        }

        return user;
    }

    public async Task<IdentityUser?> FindByIdAsync(Guid id) =>
        await _userManager.FindByIdAsync(id.ToString()).ConfigureAwait(false);

    public async Task<IdentityUser?> FindByNameAsync(string name) =>
        await _userManager.FindByNameAsync(name).ConfigureAwait(false);

    public async Task<IEnumerable<IdentityUser>> GetAllAsync() =>
        await _userManager.Users.ToListAsync().ConfigureAwait(false);

    public async Task RemoveElementAsync(Guid id)
    {
        var user = await FindByIdAsync(id).ConfigureAwait(false);
        if (user is not null)
            await _userManager.DeleteAsync(user).ConfigureAwait(false);
    }

    public async Task RemoveElementAsync(IdentityUser user) =>
        await _userManager.DeleteAsync(user).ConfigureAwait(false);

    public async Task UpdateElementAsync(IdentityUser user) =>
        await _userManager.UpdateAsync(user).ConfigureAwait(false);
}
