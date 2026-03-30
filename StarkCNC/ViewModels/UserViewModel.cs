using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.Identity;
using StarkCNC.Core.Models;
using StarkCNC.Core.Services;
using StarkCNC.Utilities;
using System.Collections.ObjectModel;

namespace StarkCNC.ViewModels;

public partial class UserViewModel : ViewModelBase
{
    private readonly IUserService _userService;

    [ObservableProperty]
    private ObservableCollection<User> _users = new ObservableCollection<User>();

    [ObservableProperty]
    private ObservableCollection<IdentityRole> _roles = new ObservableCollection<IdentityRole>();

    [ObservableProperty]
    private User? _selectedUser;

    [ObservableProperty]
    private string _selectedUserRoles;

    [ObservableProperty]
    private bool _currentUserIsAdmin;

    [ObservableProperty]
    private string _userName;

    [ObservableProperty]
    private string _password;

    [ObservableProperty]
    private string _newPassword;

    [ObservableProperty]
    private IdentityRole? _newRole;

    [ObservableProperty]
    private string? _formsErrors;

    [ObservableProperty]
    private RightBlockStatus _rightBlockShowingStatus;

    [ObservableProperty]
    private bool _isEditing;

    [ObservableProperty]
    private bool _isReadOnly = true;

    [ObservableProperty]
    private bool _isThisUserAuthorized;

    public UserViewModel(IUserService userService)
    {
        _userService = userService;

        LoadUsersAsync();
    }

    private async void LoadUsersAsync()
    {
        var users = await _userService.GetAllUsersAsync().ConfigureAwait(true);
        Users.Clear();

        foreach (var user in users)
        {
            Users.Add(user);
        }
    }

    partial void OnSelectedUserChanged(User? value)
    {
        UserName = string.Empty;

        if (SelectedUser is not null)
        {
            UserName = SelectedUser.UserName ?? string.Empty;
            SelectedUserRoles = _userService.GetUserRole(UserName).Result?.Name ?? "Оператор";

            var currentUser = _userService.CurrentUser;
            if (currentUser is not null && SelectedUser.Equals(currentUser))
                IsThisUserAuthorized = true;
            else
                IsThisUserAuthorized = false;
        }

        Password = string.Empty;

        ClearErrors();
    }

    [RelayCommand]
    private void AddUser()
    {
        ClearErrors();

        if (_userService.CurrentUser is null)
        {
            FormsErrors = "Вы не авторизованы и не можете создавать пользователей";
            return;
        }

        RightBlockShowingStatus = RightBlockStatus.Creation;

        UserName = string.Empty;
        Password = string.Empty;
    }

    [RelayCommand]
    private async Task RemoveUser()
    {
        ClearErrors();

        if (SelectedUser is null || string.IsNullOrEmpty(SelectedUser.UserName))
            return;

        var user = await _userService.FindUserByName(SelectedUser.UserName).ConfigureAwait(true);
        if (user is not null)
            try
            {
                await _userService.RemoveUser(user).ConfigureAwait(true);
            }
            catch (InvalidOperationException ex)
            {
                FormsErrors = ex.Message;
                return;
            }

        LoadUsersAsync();
    }

    [RelayCommand]
    private void EditUser()
    {
        ClearErrors();

        if (_userService.CurrentUser is null)
        {
            FormsErrors = "Вы не вошли в аккаунт и не можете изменять пользователей";
            return;
        }

        EnableEditing(true);
    }

    [RelayCommand]
    private async Task UserChangesCommit()
    {
        if (SelectedUser is null)
            return;

        bool canHasEmptyPassword = await CanHasDefaultPassword().ConfigureAwait(true);
        string password = "";
        if (string.IsNullOrEmpty(Password) && canHasEmptyPassword)
            password = ControllerRequestStrings.EMPTY_PASSWORD;
        else
            password = Password;

        if (!CheckUserDataIsValidAndShowErrors(UserName, password))
            return;

        var oldUserName = SelectedUser.UserName;
        SelectedUser.UserName = UserName;

        try
        {
            if (!await _userService.CheckPasswordWhenChange(SelectedUser, password).ConfigureAwait(true))
            {
                FormsErrors = "Неверный пароль пользователя";
                return;
            }

            if (!string.IsNullOrEmpty(NewPassword))
            {
                if (!CheckUserDataIsValidAndShowErrors(UserName, password, NewPassword))
                    return;

                await _userService.ChangePassword(SelectedUser, password, NewPassword).ConfigureAwait(true);
            }

            if (NewRole is not null)
            {
                var role = RolePermissions.IdentityRoleToRoles(NewRole.Name);
                if (password == ControllerRequestStrings.EMPTY_PASSWORD && string.IsNullOrEmpty(NewPassword) && role != Core.Models.Roles.Operator)
                {
                    FormsErrors = "Вы не можете установить новую роль для этого пользователя, т.к. у него не установлен пароль";
                    return;
                }
                else
                    await _userService.SetUserRole(SelectedUser, NewRole).ConfigureAwait(false);
            }

            await _userService.UpdateUser(SelectedUser).ConfigureAwait(true);
        }
        catch (InvalidOperationException ex)
        {
            FormsErrors = ex.Message;
            SelectedUser.UserName = oldUserName;
            UserName = oldUserName ?? string.Empty;
            EnableEditing(false);
            return;
        }

        EnableEditing(false);

        LoadUsersAsync();
    }

    [RelayCommand]
    private void CancelUserChanges()
    {
        ClearErrors();

        UserName = SelectedUser?.UserName ?? string.Empty;

        EnableEditing(false);
    }

    [RelayCommand]
    private async Task CreateUserSave()
    {
        ClearErrors();

        bool canHasEmptyPassword = await CanHasDefaultPassword(NewRole).ConfigureAwait(true);
        string password = "";
        if (string.IsNullOrEmpty(Password) && canHasEmptyPassword)
            password = ControllerRequestStrings.EMPTY_PASSWORD;
        else
            password = Password;

        if (!CheckUserDataIsValidAndShowErrors(UserName, password))
            return;

        ClearErrors();

        try
        {
            var user = await _userService.Register(UserName, password).ConfigureAwait(true);
            if (user is not null)
                await _userService.SetUserRole(user, NewRole).ConfigureAwait(true);
        }
        catch (InvalidOperationException ex)
        {
            FormsErrors = ex.Message;
            return;
        }

        UserName = string.Empty;
        Password = string.Empty;

        RightBlockShowingStatus = RightBlockStatus.Details;

        LoadUsersAsync();
    }

    [RelayCommand]
    private void CreateUserCancel()
    {
        ClearErrors();

        UserName = SelectedUser?.UserName ?? string.Empty;
        Password = string.Empty;

        ClearErrors();

        RightBlockShowingStatus = RightBlockStatus.Details;
    }

    [RelayCommand]
    private void SetSelectedUserAsCurrentOperator()
    {
        ClearErrors();

        RightBlockShowingStatus = RightBlockStatus.Authorization;
    }

    [RelayCommand]
    private async Task Authorization()
    {
        ClearErrors();

        if (SelectedUser is null)
            return;

        bool canHasDefaultPassword = await CanHasDefaultPassword().ConfigureAwait(true);
        string password;
        if (string.IsNullOrEmpty(Password))
        {
            if (!canHasDefaultPassword)
            {
                FormsErrors = "Введите пароль";
                return;
            }
            else
            {
                password = ControllerRequestStrings.EMPTY_PASSWORD;
            }
        }
        else
        {
            password = Password;
        }


        var result = await _userService.Login(UserName, password).ConfigureAwait(true);
        if (!result)
        {
            FormsErrors = "Неверный пароль";
            return;
        }

        LoadRolesAsync();
        ClearErrors();
        IsThisUserAuthorized = true;
        RightBlockShowingStatus = RightBlockStatus.Details;
    }

    [RelayCommand]
    private void AuthorizationCancel()
    {
        ClearErrors();

        RightBlockShowingStatus = RightBlockStatus.Details;
    }

    private void EnableEditing(bool value)
    {
        if (value)
        {
            IsEditing = true;
            IsReadOnly = false;
        }
        else
        {
            IsEditing = false;
            IsReadOnly = true;
        }
    }

    private async Task<bool> CanHasDefaultPassword(IdentityRole? newRole = null)
    {
        Roles role = Core.Models.Roles.Service;

        if (newRole is not null)
        {
            role = RolePermissions.IdentityRoleToRoles(newRole.Name);
            if (role == Core.Models.Roles.Operator)
                return true;
            else
                return false;
        }

        if (SelectedUser is not null)
        {
            var roleIdentity = await _userService.GetUserRole(SelectedUser).ConfigureAwait(true);
            if (roleIdentity is not null)
                role = RolePermissions.IdentityRoleToRoles(roleIdentity.Name);

            if (role == Core.Models.Roles.Operator)
                return true;

            return false;
        }

        if (UserName is not null)
        {
            var user = await _userService.FindUserByName(UserName).ConfigureAwait(true);
            if (user is not null)
            {
                var roleIdentity = await _userService.GetUserRole(user).ConfigureAwait(true);
                if (roleIdentity is not null)
                    role = RolePermissions.IdentityRoleToRoles(roleIdentity.Name);

                if (role == Core.Models.Roles.Operator)
                    return true;
            }
        }

        return false;
    }

    private bool CheckUserDataIsValidAndShowErrors(string userName, string currentPassword, string? newPassword = null) // TODO: Guard Clauses
    {
        var userNameEmpty = string.IsNullOrEmpty(userName);
        string password;
        bool passwordEmpty;
        if (newPassword is null)
        {
            password = currentPassword;
            passwordEmpty = string.IsNullOrEmpty(currentPassword);
        }
        else
        {
            password = newPassword;
            passwordEmpty = string.IsNullOrEmpty(newPassword);
        }

        if (userNameEmpty && passwordEmpty)
        {
            FormsErrors = "Заполните имя пользователя и пароль";
            return false;
        }
        if (userNameEmpty)
        {
            FormsErrors = "Заполните имя пользователя";
            return false;
        }
        if (passwordEmpty && newPassword is null)
        {
            FormsErrors = "Заполните пароль";
            return false;
        }
        if (password.Length <= 6)
        {
            FormsErrors = "Пароль должен содержать больше 6 символов";
            return false;
        }

        return true;
    }

    private void ClearErrors() =>
        FormsErrors = null;

    private async void LoadRolesAsync()
    {
        Roles.Clear();

        var roles = await _userService.GetAllAvailableRoles().ConfigureAwait(true);
        foreach (var item in roles)
        {
            Roles.Add(item);
        }
    }
}

public enum RightBlockStatus
{
    Details,
    Creation,
    Authorization
}