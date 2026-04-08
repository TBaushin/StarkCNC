using Microsoft.AspNetCore.Identity;
using StarkCNC.Core.Models;
using StarkCNC.Core.Services;
using StarkCNC.Utilities;
using System.Collections.ObjectModel;

namespace StarkCNC.ViewModels;

public partial class RegistrationWindowViewModel : ViewModelBase
{
    private IUserService _userService;

    public ObservableCollection<IdentityRole> AvailableRoles { get; } = new ObservableCollection<IdentityRole>();

    private RegistrationWindowViewModel(IUserService userService, IEnumerable<IdentityRole> availableRoles)
    {
        _userService = userService;

        foreach (var item in availableRoles)
            AvailableRoles.Add(item);
    }

    public static async Task<RegistrationWindowViewModel> InitializeAsync(IUserService userService)
    {
        if (userService is null)
            throw new ArgumentNullException(nameof(userService));

        if (userService.CurrentUser is null)
            throw new InvalidOperationException("Нельзя создавать пользователей не авторизовавшись");

        var availableRoles = await userService.GetAllAvailableRoles().ConfigureAwait(false);

        var viewModel = new RegistrationWindowViewModel(userService, availableRoles);

        return viewModel;
    }

    public async Task<string> RegisterNewUser(string username, string password, IdentityRole? role)
    {
        string validPassword = string.Empty;
        bool canHasDefaultPassword = CanHasDefaultPassword(role);
        if (string.IsNullOrEmpty(password) && !canHasDefaultPassword)
            return "Укажите пароль";
        else if (string.IsNullOrEmpty(password) && canHasDefaultPassword)
            validPassword = ControllerRequestStrings.EMPTY_PASSWORD;
        else
            validPassword = password;

        try
        {
            await _userService.Register(username, validPassword, role?.Name).ConfigureAwait(true);
        }
        catch (InvalidOperationException ex)
        {
            return ex.Message;
        }

        return string.Empty;
    }

    private bool CanHasDefaultPassword(IdentityRole? role)
    {
        if (role is null)
            return true;

        if (RolePermissions.IdentityRoleToRoles(role.Name) == Roles.Operator)
            return true;

        return false;
    }
}
