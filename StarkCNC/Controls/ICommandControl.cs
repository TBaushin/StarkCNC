using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace StarkCNC.Controls;

public interface ICommandControl
{
    public static async Task ExecuteCommand(
        ICommand command,
        object? parameter = null)
    {
        if (command is null)
            return;

        if (command is IAsyncRelayCommand asyncCommand && command.CanExecute(parameter))
            await asyncCommand.ExecuteAsync(parameter).ConfigureAwait(true);
        else if (command is RelayCommand relayCommand && relayCommand.CanExecute(parameter))
            command.Execute(parameter);
        else if (command.CanExecute(parameter))
            command.Execute(parameter);
    }
}
