using StarkCNC.ViewModels;
using System.Windows;
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
    }

    private void EditButton_Click(object sender, RoutedEventArgs e)
    {
        ExecuteButtonCommand(sender);

        SaveButton.Focus();
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        ExecuteButtonCommand(sender);

        EditButton.Focus();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        ExecuteButtonCommand(sender);

        EditButton.Focus();
    }

    private static void ExecuteButtonCommand(object sender)
    {
        var button = sender as Button;
        if (button is null)
            return;

        var command = button.Command;
        var commandParameter = button.CommandParameter;

        if (command is not null && command.CanExecute(commandParameter))
            command.Execute(commandParameter);
    }
}
