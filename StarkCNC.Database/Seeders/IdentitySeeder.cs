using Microsoft.AspNetCore.Identity;
using StarkCNC.Core.Models;
using StarkCNC.Utilities;

namespace StarkCNC.Database.Seeders;

public static class IdentitySeeder
{
    public static async Task SeedAdminAsync(UserManager<User> userManager)
    {
        var user = new User() { UserName = "Сервис" };
        if (await userManager.FindByNameAsync(user.UserName).ConfigureAwait(false) is not null)
            return;
        
        var result = await userManager.CreateAsync(user, ControllerRequestStrings.DEFAULT_USER_PASSWORD).ConfigureAwait(false);
        if (!result.Succeeded)
            return;

        var resUser = await userManager.FindByNameAsync(user.UserName);
        if (resUser is null)
            return;

        await userManager.AddToRoleAsync(resUser, "Сервис").ConfigureAwait(false);
    }

    public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        string[] roles = { "Оператор", "Администратор", "Сервис" };

        foreach (var role in roles)
        {
            var roleExists = await roleManager.RoleExistsAsync(role).ConfigureAwait(false);
            if (!roleExists)
                await roleManager.CreateAsync(new IdentityRole(role)).ConfigureAwait(false);
        }
    }
}
