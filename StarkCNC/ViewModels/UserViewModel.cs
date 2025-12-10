using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarkCNC.Core.Repository;
using StarkCNC.Models;
using System.Collections.ObjectModel;

namespace StarkCNC.ViewModels;

public partial class UserViewModel : ObservableObject
{
    private readonly IUsersRepository _repository;

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

    public UserViewModel(IUsersRepository repository)
    {
        _repository = repository;

        LoadUsersAsync();
    }

    private async void LoadUsersAsync()
    {
        var users = await _repository.GetAllAsync().ConfigureAwait(false);
        Users.Clear();

        foreach(var user in users)
        {
            Users.Add(new User(user.Id, user.Name, user.Image?.ToArray() ?? Array.Empty<byte>()));
        }
    }

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
    private async Task AddUser()
    {
        var u = new User(Guid.NewGuid(), Localization.Language.NewUser, Array.Empty<byte>());
        Users.Add(u);
        SelectedUser = Users.Last();
        await _repository.AddElementAsync(new Core.Models.User(u.Id, u.Name, u.Image)).ConfigureAwait(false);

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
        await displayMessageTask.ConfigureAwait(false);

        DeletedName = selectedUser.Name;

        SelectedUser = null;

        Users.Remove(selectedUser);
        await _repository.RemoveElementAsync(selectedUser.Id).ConfigureAwait(false);
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
    private async Task EditUserCommit()
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

            var u = await _repository.FindByIdAsync(SelectedUser.Id).ConfigureAwait(false);
            if (u is not null)
            {
                u.Name = SelectedUser.Name;
                u.SetImage(SelectedUser.Image);
                await _repository.UpdateElementAsync(u).ConfigureAwait(false);
            }
            await Task.Delay(2000).ContinueWith(_ => IsSaved = false, TaskScheduler.FromCurrentSynchronizationContext()).ConfigureAwait(false);
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