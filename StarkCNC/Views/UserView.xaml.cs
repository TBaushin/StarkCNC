using StarkCNC.Controls;
using StarkCNC.Utilities;
using StarkCNC.ViewModels;
using System.Windows.Controls;

namespace StarkCNC.Views;

/// <summary>
/// Interaction logic for UserView.xaml
/// </summary>
public partial class UserView : Page
{
    private readonly UserViewModel ViewModel;

    public UserView(UserViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = ViewModel;

        Loaded += async (_, _) => await ViewModel.InitializeAsync().ConfigureAwait(true);

        InitializeComponent();

        ViewModel.PropertyChanged += ViewModel_PropertyChanged;

        if (UserListView.Items.Count > 0)
            UserListView.SelectedItem = UserListView.Items[0];
    }

    private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ViewModel.SelectedUser) || e.PropertyName == nameof(ViewModel.RightBlockShowingStatus))
            ClearPasswordBox();
    }

    private async void EditUserGrid_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (e.Key != System.Windows.Input.Key.Enter)
            return;

        await ViewModel.UserChangesCommitCommand.ExecuteAsync(null).ConfigureAwait(true);
    }

    private async void CreateUserGrid_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (e.Key != System.Windows.Input.Key.Enter)
            return;

        await ViewModel.CreateUserSaveCommand.ExecuteAsync(null).ConfigureAwait(true);
    }

    private async void AuthUserGrid_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        //if (e.Key != System.Windows.Input.Key.Enter)
        //    return;

        //await ViewModel.AuthorizationCommand.ExecuteAsync(null).ConfigureAwait(true);
    }

    private void ClearPasswordBox()
    {
        var pbs = VisualFinder.FindVisualChildren<PasswordTextBox>(MainGrid);
        foreach (var item in pbs)
        {
            if (item is null)
                continue;

            item.Clear();
        }
    }
}