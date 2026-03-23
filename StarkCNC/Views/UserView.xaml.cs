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

        InitializeComponent();

        ViewModel.PropertyChanged += ViewModel_PropertyChanged;

        if (UserListView.Items.Count > 0)
            UserListView.SelectedItem = UserListView.Items[0];
    }

    private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ViewModel.SelectedUser))
        {
            var pbs = VisualFinder.FindVisualChildren<PasswordBox>(MainGrid);
            foreach (var item in pbs)
            {
                if (item is null)
                    continue;

                item.Clear();
            }
        }

        if (e.PropertyName == nameof(ViewModel.RightBlockShowingStatus))
        {
            var pbs = VisualFinder.FindVisualChildren<PasswordBox>(MainGrid);
            foreach (var item in pbs)
            {
                if (item is null)
                    continue;

                item.Clear();
            }
        }
    }

    private void PasswordBox_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
    {
        var pb = sender as PasswordBox;
        if (pb is null)
            return;

        ViewModel.Password = pb.Password;
    }

    private void NewPasswordPB_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
    {
        var pb = sender as PasswordBox;
        if (pb is null)
            return;

        ViewModel.NewPassword = pb.Password;
    }
}