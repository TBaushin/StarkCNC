using StarkCNC.Core.Models;

namespace StarkCNC.Utilities;

public static class RolePermissions
{
    public static readonly Dictionary<Roles, Roles[]> CanUpdate = new Dictionary<Roles, Roles[]>() // TODO: Можно сделать как поле Role что-то типа IEnumerable<Role> CanUpdate { get; set; }
    {
        { Roles.Service, new[] { Roles.Administrator, Roles.Operator } },
        { Roles.Administrator, new[] { Roles.Operator } },
        { Roles.Operator, Array.Empty<Roles>() }
    };

    public static bool RoleHasModifyPermission(Roles creatorRole, Roles roleToCreate) =>
        CanUpdate.TryGetValue(creatorRole, out var allowed) && allowed.Contains(roleToCreate);

    public static Roles IdentityRoleToRoles(string? roleName) =>
        roleName?.ToUpperInvariant() switch
        {
            "ОПЕРАТОР" => Roles.Operator,
            "АДМИНИСТРАТОР" => Roles.Administrator,
            "СЕРВИС" => Roles.Service,
            _ => Roles.Operator
        };

    public static string RolesToIndetityRole(Roles role) =>
        role switch
        {
            Roles.Operator => "Оператор",
            Roles.Administrator => "Администратор",
            Roles.Service => "Сервис",
            _ => "Оператор"
        };
}
