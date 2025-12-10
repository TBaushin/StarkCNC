using Microsoft.EntityFrameworkCore;
using StarkCNC.Core.Models;
using StarkCNC.Core.Repository;
using StarkCNC.Database;

namespace StarkCNC.Repository;

public class UsersRepository : IUsersRepository
{
    private readonly AppJsonContext _context;

    public UsersRepository(AppJsonContext context)
    {
        _context = context;
    }

    public async Task<User?> AddElementAsync(User user)
    {
        await _context.AddAsync(user).ConfigureAwait(false);
        await _context.SaveChangesAsync().ConfigureAwait(false);

        return user;
    }

    public async Task<User?> FindByIdAsync(Guid id) =>
        await _context.Users.FirstOrDefaultAsync(u => u.Id == id).ConfigureAwait(false);

    public async Task<IEnumerable<User>> FindByNameAsync(string name) =>
        await _context.Users.Where(u => u.Name.Contains(name, StringComparison.CurrentCulture)).ToListAsync().ConfigureAwait(false);

    public async Task<IEnumerable<User>> GetAllAsync() =>
        await _context.Users.ToListAsync().ConfigureAwait(false);

    public async Task RemoveElementAsync(Guid id)
    {
        var item = _context.Users.FirstOrDefault(u => u.Id == id);
        if (item is not null)
        {
            _context.Remove(item);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
    }

    public async Task RemoveElementAsync(User user)
    {
        _context.Remove(user);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task UpdateElementAsync(User user)
    {
        if (user is null)
            throw new ArgumentNullException(nameof(user));

        _context.Entry(user).State = EntityState.Modified;

        await _context.SaveChangesAsync().ConfigureAwait(false);
    }
}
