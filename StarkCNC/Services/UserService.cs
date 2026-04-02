using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using StarkCNC.Core.Models;
using StarkCNC.Core.Services;
using StarkCNC.Database;
using StarkCNC.Utilities;

namespace StarkCNC.Services;

public class UserService : IUserService
{
    private IServiceProvider _serviceProvider;
    private const string _registryKey = "Software\\StarkCNC";

    public User CurrentUser { get; private set; }

    public IdentityRole? CurrentUserRole { get; private set; }

    private Roles _currentUserRole => RolePermissions.IdentityRoleToRoles(CurrentUserRole?.Name);

    public UserService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        OpenSession().GetAwaiter().GetResult();
    }

    public async Task<User?> Register(string username, string password, string? role = null)
    {
        if (!Authorized())
            throw new InvalidOperationException("Вы не вошли в аккаунт и не можете создавать пользователей");

        if (_currentUserRole == Roles.Operator)
            throw new InvalidOperationException("У вас нет доступа к созданию новых пользователей");

        var userManager = _serviceProvider.GetRequiredService<UserManager<User>>();
        var user = new User() { UserName = username };
        var result = await userManager.CreateAsync(user, password).ConfigureAwait(false);
        if (!result.Succeeded)
            return null;


        var resUser = await FindUserByName(username).ConfigureAwait(false);
        if (resUser is null)
            return null;

        if (string.IsNullOrEmpty(role))
            await userManager.AddToRoleAsync(resUser, "Оператор").ConfigureAwait(false);
        else if(!string.IsNullOrEmpty(role) && CanUserUpdate(resUser, RolePermissions.IdentityRoleToRoles(role)))
            await userManager.AddToRoleAsync(resUser, role).ConfigureAwait(false);

        return resUser;
    }

    public async Task<bool> Login(string username, string password, bool saveSession = false)
    {
        var user = await FindUserByName(username).ConfigureAwait(false);
        if (user is null)
            return false;

        return await Login(user, password, saveSession).ConfigureAwait(false);
    }

    public async Task<bool> Login(User user, string password, bool saveSession = false)
    {
        var userManager = _serviceProvider.GetRequiredService<UserManager<User>>();
        bool isPasswordValid = await userManager.CheckPasswordAsync(user, password).ConfigureAwait(false);
        if (!isPasswordValid)
            return false;

        CurrentUser = user;
        CurrentUserRole = await GetUserRole(user).ConfigureAwait(false);

        if (saveSession)
            await SaveSession(user, password).ConfigureAwait(false);
        else
            ClearSession();

        return true;
    }

    public async Task<bool> CheckPasswordWhenChange(string username, string currentPassword)
    {
        var user = await FindUserByName(username).ConfigureAwait(false);
        if (user is null)
            return false;

        return await CheckPasswordWhenChange(user, currentPassword).ConfigureAwait(false);
    }

    public async Task<bool> CheckPasswordWhenChange(User user, string currentPassword)
    {
        var userManager = _serviceProvider.GetRequiredService<UserManager<User>>();
        return await userManager.CheckPasswordAsync(user, currentPassword).ConfigureAwait(false);
    }

    public async Task AddRole(string role)
    {
        var identityRole = new IdentityRole(role);

        await AddRole(identityRole).ConfigureAwait(false);
    }

    public async Task AddRole(IdentityRole role)
    {
        var roleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        await roleManager.CreateAsync(role).ConfigureAwait(false);
    }

    public async Task RemoveUser(string username)
    {
        var user = await FindUserByName(username).ConfigureAwait(false);
        if (user is null)
            return;

        await RemoveUser(user).ConfigureAwait(false);
    }

    public async Task RemoveUser(User user)
    {
        if (user is null)
            throw new ArgumentNullException(nameof(user));

        if (!Authorized())
            throw new InvalidOperationException("Вы не вошли в аккаунт и не можете удалять пользователей");

        var userRole = await GetUserRole(user).ConfigureAwait(false);
        if (!CanUserUpdate(user, RolePermissions.IdentityRoleToRoles(userRole?.Name)))
            throw new InvalidOperationException("Ваш уровень доступа не позволяет вам удалять пользователей данного уровня доступа");

        var userManager = _serviceProvider.GetRequiredService<UserManager<User>>();

        await userManager.DeleteAsync(user).ConfigureAwait(false);

        if (user.Id == CurrentUser.Id)
            CurrentUser = null;
    }

    public async Task RemoveRole(string role)
    {
        var roleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var identityRole = await roleManager.FindByNameAsync(role).ConfigureAwait(false);
        if (identityRole is null)
            return;

        await RemoveRole(identityRole).ConfigureAwait(false);
    }

    public async Task RemoveRole(IdentityRole role)
    {
        var roleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        await roleManager.DeleteAsync(role).ConfigureAwait(false);
    }

    public async Task ChangePassword(string username, string currentPassword, string newPassword)
    {
        var user = await FindUserByName(username).ConfigureAwait(false);
        if (user is null)
            return;

        await ChangePassword(user, currentPassword, newPassword).ConfigureAwait(false);
    }

    public async Task ChangePassword(User user, string currentPassword, string newPassword)
    {
        var userManager = _serviceProvider.GetRequiredService<UserManager<User>>();

        await userManager.ChangePasswordAsync(user, currentPassword, newPassword).ConfigureAwait(false);
    }

    public async Task UpdateUser(string currentUsername, string? newUsername = null, IReadOnlyCollection<byte>? image = null)
    {
        if (!Authorized())
            throw new InvalidOperationException("Вы не вошли в аккаунт и не можете удалять пользователей");

        var user = await FindUserByName(currentUsername).ConfigureAwait(false);
        if (user is null)
            return;

        var userRole = await GetUserRole(user).ConfigureAwait(false);
        if (!CanUserUpdate(user, RolePermissions.IdentityRoleToRoles(userRole?.Name)))
            throw new InvalidOperationException("Ваш уровень доступа не позволяет редактировать пользователей этого уровня доступа");

        if (newUsername is null && image is null)
            return;

        if (!string.IsNullOrEmpty(newUsername))
            user.UserName = newUsername;

        if (image is not null)
            user.Image = image;

        await UpdateUser(user).ConfigureAwait(false);
    }

    public async Task UpdateUser(User user)
    {
        if (user is null)
            throw new ArgumentNullException(nameof(user));

        if (!Authorized())
            throw new InvalidOperationException("Вы не вошли в аккаунт и не можете удалять пользователей");

        var userRole = await GetUserRole(user).ConfigureAwait(false);
        if (!CanUserUpdate(user, RolePermissions.IdentityRoleToRoles(userRole?.Name)))
            throw new InvalidOperationException("Ваш уровень доступа не позволяет редактировать пользователей этого уровня доступа");

        var userManager = _serviceProvider.GetRequiredService<UserManager<User>>();

        await userManager.UpdateAsync(user).ConfigureAwait(false);
    }

    public async Task UpdateRole(IdentityRole role)
    {
        var roleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        await roleManager.UpdateAsync(role).ConfigureAwait(false);
    }

    public async Task<User?> FindUserByName(string name)
    {
        var userManager = _serviceProvider.GetRequiredService<UserManager<User>>();

        return await userManager.FindByNameAsync(name).ConfigureAwait(false);
    }

    public async Task<IdentityRole?> GetUserRole(string username)
    {
        var user = await FindUserByName(username).ConfigureAwait(false);
        if (user is null)
            return null;

        return await GetUserRole(user).ConfigureAwait(false);
    }

    public async Task<IdentityRole?> GetUserRole(User user)
    {
        var context = _serviceProvider.GetRequiredService<AppDbContext>();

        return await context.UserRoles
            .Where(role => role.UserId == user.Id) // Получем список ролей (IQueriable<IdentityUserRole<string>>), к которым принадлежит пользователь
            .Join(context.Roles, userRole => userRole.RoleId, role => role.Id, (userRole, role) => role) // Дальше выполняем JOIN между полученным ранее список и таблицей Roles, где Id совпадают - это и есть роли (IdentityRole) пользователя
            .FirstOrDefaultAsync()
            .ConfigureAwait(false);
    }

    public async Task SetUserRole(string username, string roleName)
    {
        var roleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        var user = await FindUserByName(username).ConfigureAwait(false);
        var role = await roleManager.FindByNameAsync(roleName).ConfigureAwait(false);
        if (user is null)
            return;
        if (role is null)
            return;

        await SetUserRole(user, role).ConfigureAwait(false);
    }

    public async Task SetUserRole(User user, IdentityRole role)
    {
        var userManager = _serviceProvider.GetRequiredService<UserManager<User>>();

        var currentUserRole = await GetUserRole(user).ConfigureAwait(false);
        if (currentUserRole is not null)
            await userManager.RemoveFromRoleAsync(user, currentUserRole.Name).ConfigureAwait(false);

        await userManager.AddToRoleAsync(user, role.Name).ConfigureAwait(false);
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        var userManager = _serviceProvider.GetRequiredService<UserManager<User>>();

        return await userManager.Users.ToListAsync().ConfigureAwait(false);
    }

    public async Task<IEnumerable<IdentityRole>> GetAllAvailableRoles()
    {
        var roles = new List<IdentityRole>();
        if (!Authorized())
            return roles;

        RolePermissions.CanUpdate.TryGetValue(_currentUserRole, out var canChange);
        if (canChange is null || canChange.Length == 0)
            return roles;

        var roleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        foreach (var item in canChange)
        {
            var role = await roleManager
                .FindByNameAsync(RolePermissions.RolesToIndetityRole(item))
                .ConfigureAwait(false);
            if (role is null)
                continue;

            roles.Add(role);
        }

        return roles;
    }

    private bool Authorized() =>
        CurrentUser is not null;

    private bool CanUserUpdate(User itemToUpdate, Roles roleItemToUpdate = Roles.Operator) =>
        Authorized() &&
        (CurrentUser.UserName == itemToUpdate.UserName ||
        (CurrentUser.UserName != itemToUpdate.UserName && RolePermissions.RoleHasModifyPermission(_currentUserRole, roleItemToUpdate)));

    //{
    //    if (!Authorized())
    //        return false;

    //    if (CurrentUser.UserName == itemToUpdate.UserName)
    //        return true;

    //    return RolePermissions.RoleHasModifyPermission(_currentUserRole, roleItemToUpdate);
    //}

    private static async Task SaveSession(User user, string password)
    {
        var userData = new UserData() { User = user, Password = password };
        var result = await Cryptography.EncryptAsync(userData).ConfigureAwait(false);

        var registry = Registry.CurrentUser.OpenSubKey(_registryKey, true);
        if (registry is null)
            registry = Registry.CurrentUser.CreateSubKey(_registryKey);

        registry.SetValue("Session", result);
    }

    private static void ClearSession()
    {
        var registry = Registry.CurrentUser.OpenSubKey(_registryKey, true);
        if (registry is null)
            return;

        registry.SetValue("Session", string.Empty);
    }

    private async Task OpenSession()
    {
        var registry = Registry.CurrentUser.OpenSubKey(_registryKey, false);
        if (registry is null)
            return;

        var session = registry.GetValue("Session") as string;
        if (string.IsNullOrEmpty(session))
            return;

        var userData = await Cryptography.DecryptAsync<UserData>(session).ConfigureAwait(false);
        if (userData is null)
            return;

        await Login(userData.User.UserName, userData.Password, true).ConfigureAwait(false);
    }

    private class UserData
    {
        public User User { get; set; }
        public string Password { get; set; }
    }
}
