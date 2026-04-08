using CommunityToolkit.Mvvm.ComponentModel;
using StarkCNC.Core.Models;
using StarkCNC.Core.Services;
using StarkCNC.Utilities;
using StarkCNC.Windows;
using System.Collections.ObjectModel;
using System.Windows;

namespace StarkCNC.ViewModels;

public partial class AuthorizationWindowViewModel : ViewModelBase
{
    private IUserService _userService;
    private User? _authorizedUser;

    [ObservableProperty]
    private bool _canCreateOrDeleteUsers;

    public ObservableCollection<User> Users { get; } = new ObservableCollection<User>();

    public bool AtStart { get; }

    private AuthorizationWindowViewModel(IUserService userService, User? authorizedUser, bool canCreateOrDeleteUsers, IEnumerable<User> users, bool atStart = false)
    {
        _userService = userService;
        _authorizedUser = authorizedUser;
        CanCreateOrDeleteUsers = canCreateOrDeleteUsers;
        AtStart = atStart;
        foreach (var item in users)
            Users.Add(item);
    }

    public static async Task<AuthorizationWindowViewModel> InitializeAsync(IUserService userService, bool atStart = false)
    {
        if (userService is null)
            throw new ArgumentNullException(nameof(userService));

        var authorizedUser = userService.CurrentUser;
        bool canCreateOrDeleteUsers = await CanManageUsers(userService, authorizedUser).ConfigureAwait(false);
        var users = await userService.GetAllUsersAsync().ConfigureAwait(false);
        return new AuthorizationWindowViewModel(userService, authorizedUser, canCreateOrDeleteUsers, users, atStart);
    }

    private static async Task<bool> CanManageUsers(IUserService userService, User? authorizedUser)
    {
        if (authorizedUser is null)
            return false;

        var role = await userService.GetUserRole(authorizedUser).ConfigureAwait(true);
        if (role is null)
            return false;

        if (RolePermissions.IdentityRoleToRoles(role.Name) == Roles.Operator)
            return false;

        return true;
    }

    public async Task<string> Authorization(User? selectedUser, string password, bool saveSession)
    {
        if (selectedUser is null)
            return "Укажите пользователя";

        string validPassword = string.Empty;
        bool canHasDefaultPassword = await CanHasDefaultPassword(selectedUser).ConfigureAwait(true);
        if (string.IsNullOrEmpty(password) && !canHasDefaultPassword)
            return "Укажите пароль";
        else if (string.IsNullOrEmpty(password) && canHasDefaultPassword)
            validPassword = ControllerRequestStrings.EMPTY_PASSWORD;
        else
            validPassword = password;

        var success = await _userService.Login(selectedUser, validPassword, saveSession).ConfigureAwait(true);
        if (!success)
            return "Не верный пароль";

        return string.Empty;
    }

    public async Task RegisterNewUser()
    {
        var viewModel = await RegistrationWindowViewModel.InitializeAsync(_userService).ConfigureAwait(true);
        var registerWindow = new RegistrationWindow(viewModel);
        registerWindow.ShowDialog();

        Users.Clear();

        foreach (var user in await _userService.GetAllUsersAsync().ConfigureAwait(true))
            Users.Add(user);
    }

    public async Task<string> DeleteSelectedUser(User? selectedUser)
    {
        if (_authorizedUser is null)
            return "Вы не имеете права удалять пользователей";

        if (selectedUser is null)
            return "Не выбран пользователь для удаления";

        var sureDelete = MessageBox.Show("Вы уверены, что хотите удалить пользователя?", "Удаление пользователя", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (sureDelete == MessageBoxResult.Yes)
        {
            try
            {
                await _userService.RemoveUser(selectedUser).ConfigureAwait(true);
            }
            catch (InvalidOperationException ex)
            {
                return ex.Message;
            }
        }

        Users.Clear();

        foreach (var user in await _userService.GetAllUsersAsync().ConfigureAwait(true))
            Users.Add(user);

        return string.Empty;
    }

    private async Task<bool> CanHasDefaultPassword(User user)
    {
        var userRole = await _userService.GetUserRole(user).ConfigureAwait(true);
        if (userRole is null)
            return false;

        if (RolePermissions.IdentityRoleToRoles(userRole.Name) == Roles.Operator)
            return true;

        return false;
    }
}
