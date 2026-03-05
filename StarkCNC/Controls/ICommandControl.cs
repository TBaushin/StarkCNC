using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace StarkCNC.Controls;

public interface ICommandControl
{
    public static async Task ExecuteCommand(
        ICommand command,
        object? parameter = null)
    {
        if (command is null || !command.CanExecute(parameter))
            return;

        if (command is IAsyncRelayCommand asyncCommand)
            await asyncCommand.ExecuteAsync(parameter).ConfigureAwait(true);
        else
            command.Execute(parameter);
    }
}
