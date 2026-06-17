using NSubstitute;
using StarkCNC.Core.Repository;
using StarkCNC.Core.Services;
using StarkCNC.MachineCommunication.Services;
using StarkCNC.Utilities;
using StarkCNC.ViewModels;

namespace StarkCNC.Tests.ViewModels;

public class SettingsViewModelTest
{
    [Fact]
    public async Task SendsDataWhenPropertiesChanges()
    {
        // Arrange
        var settingsRepository = Substitute.For<ISettingsRepository>();
        var machineConfiguration = Substitute.For<IManualConfigurationService>();
        var userService = Substitute.For<IUserService>();
        var viewModel = new SettingsViewModel(settingsRepository, machineConfiguration, userService);

        // Act
        viewModel.BendAcceleration = 50;
        viewModel.BendAccelerationSendCommand.Execute(null);

        // Assert
        await machineConfiguration
            .Received(1)
            .WriteAsync(viewModel.BendAcceleration, ControllerRequestStrings.BEND_ACCELERATION);
    }
}
