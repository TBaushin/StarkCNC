using CommunityToolkit.Mvvm.Input;
using NSubstitute;
using StarkCNC.Controls;

namespace StarkCNC.Tests.Controls;

public class CommandControlTest
{
    [Fact]
    public async Task CommandExecuted()
    {
        // Arrange
        var command = Substitute.For<IRelayCommand>();
        command.CanExecute(Arg.Any<object>()).Returns(true);

        // Act
        await ICommandControl.ExecuteCommand(command);

        // Assert
        command.Received(1).Execute(null);
    }

    [Fact]
    public async Task CommandExecutedAsync()
    {
        // Arrange
        var command = Substitute.For<IAsyncRelayCommand>();
        command.CanExecute(Arg.Any<object>()).Returns(true);
        command.ExecuteAsync(Arg.Any<object>()).Returns(Task.CompletedTask);

        // Act
        await ICommandControl.ExecuteCommand(command, null);

        // Assert
        await command.Received(1).ExecuteAsync(null);
        command.DidNotReceive().Execute(null);
    }
}
