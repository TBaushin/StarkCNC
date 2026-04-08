using StarkCNC.Core.Models;
using StarkCNC.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace StarkCNC.Windows;

/// <summary>
/// Interaction logic for AuthorizationWindow.xaml
/// </summary>
public partial class AuthorizationWindow : Window
{
    private AuthorizationWindowViewModel ViewModel;
    private bool isAuthorized = false;

    public AuthorizationWindow(AuthorizationWindowViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = ViewModel;

        InitializeComponent();
    }

    private void ShowError(string error)
    {
        ErrorsForm.Visibility = Visibility.Visible;
        ErrorsText.Content = error;
    }

    private void HideErrors()
    {
        ErrorsForm.Visibility = Visibility.Collapsed;
        ErrorsText.Content = string.Empty;
    }

    private async void RegisterUserButton_Click(object sender, RoutedEventArgs e)
    {
        Hide();
        HideErrors();

        await ViewModel.RegisterNewUser().ConfigureAwait(true);

        Show();
    }

    private async void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        User? selectedUser = Login.SelectedItem as User;

        var error = await ViewModel.DeleteSelectedUser(selectedUser).ConfigureAwait(true);
        if (!string.IsNullOrEmpty(error))
            ShowError(error);
        else
            HideErrors();
    }

    private async void AuthButton_Click(object sender, RoutedEventArgs e)
    {
        User? selectedUser = Login.SelectedItem as User;

        bool saveSession = false;
        if (RememberAuth.IsChecked is bool remember)
            saveSession = remember;

        var error = await ViewModel.Authorization(selectedUser, Password.Password, saveSession).ConfigureAwait(true);
        if (string.IsNullOrEmpty(error))
        {
            isAuthorized = true;
            Close();
        }

        ShowError(error);
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
            AuthButton_Click(new object(), new RoutedEventArgs());
    }

    private void Window_Closing(object sender,  System.ComponentModel.CancelEventArgs e)
    {
        if (!ViewModel.AtStart)
        {
            e.Cancel = false;
            return;
        }
            
        if (isAuthorized)
        {
            e.Cancel = false;
            return;
        }

        var result = MessageBox.Show("Закрыть программу?", "Закрыть программу?", MessageBoxButton.OKCancel, MessageBoxImage.Question);
        if (result == MessageBoxResult.OK)
            e.Cancel = false;
        else
            e.Cancel = true;
    }
}
