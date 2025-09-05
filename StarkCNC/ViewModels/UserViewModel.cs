using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarkCNC.Models;
using System.Collections.ObjectModel;

namespace StarkCNC.ViewModels;

public partial class UserViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<User> _users = new ObservableCollection<User>();

    [ObservableProperty]
    private User? _selectedUser;

    [ObservableProperty]
    private User? _editableUser;

    [ObservableProperty]
    private bool _isEditing;

    [ObservableProperty]
    private bool _isReadOnly = true;

    [ObservableProperty]
    private bool _isSaved;

    [ObservableProperty]
    private string _deletedName = string.Empty;

    partial void OnSelectedUserChanged(User? oldValue, User? newValue)
    {
        if (SelectedUser is not null && SelectedUser != EditableUser)
        {
            EditableUser = new User(SelectedUser);
            IsReadOnly = true;
            IsEditing = false;
        }
    }

    [RelayCommand]
    private void AddUser()
    {
        Users.Add(new User(Localization.Language.NewUser, Array.Empty<byte>()));
        SelectedUser = Users.Last();

        IsReadOnly = false;
        IsEditing = true;
    }

    private CancellationTokenSource cancelTokenSource = new();
    private Task displayMessageTask = Task.CompletedTask;
    partial void OnDeletedNameChanged(string? oldValue, string newValue)
    {
        if (string.IsNullOrEmpty(newValue))
        {
            return;
        }

        cancelTokenSource = new();
        displayMessageTask = Task.Delay(2000, cancelTokenSource.Token).ContinueWith(_ =>
        {
            DeletedName = string.Empty;
        }, TaskScheduler.FromCurrentSynchronizationContext());
    }

    [RelayCommand]
    private async Task RemoveUser(User selectedUser)
    {
        cancelTokenSource.Cancel();
        await displayMessageTask;

        DeletedName = selectedUser.Name;

        SelectedUser = null;

        Users.Remove(selectedUser);
        IsReadOnly = true;
        IsEditing = false;
    }

    [RelayCommand]
    private void EditUserStart()
    {
        if (SelectedUser != null)
        {
            EditableUser = new User(SelectedUser);
            IsReadOnly = false;
            IsEditing = true;
        }
    }

    [RelayCommand]
    private void EditUserCommit()
    {
        if (EditableUser != null && SelectedUser != null)
        {
            int index = Users.IndexOf(SelectedUser);
            Users.RemoveAt(index);
            Users.Insert(index, EditableUser);
            SelectedUser = Users[index];
            IsReadOnly = true;
            IsEditing = false;
            IsSaved = true;

            Task.Delay(2000).ContinueWith(_ => IsSaved = false, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }

    [RelayCommand]
    private void EditUserCancel()
    {
        EditableUser = null;
        EditableUser = new User(SelectedUser);
        IsReadOnly = true;
        IsEditing = false;
    }
}
