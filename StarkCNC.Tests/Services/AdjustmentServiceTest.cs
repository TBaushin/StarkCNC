using NSubstitute;
using StarkCNC.Core.Models;
using StarkCNC.Core.Repository;
using StarkCNC.MachineCommunication.Services;
using StarkCNC.Services;
using StarkCNC.Tests.Fakes.Repositories;

namespace StarkCNC.Tests.Services;

public class AdjustmentServiceTest
{
    [Fact]
    public async Task OnlyOneElementHasCertainLevel()
    {
        // Arrange
        var manualConfigServer = Substitute.For<IManualConfigurationService>();
        var service = new AdjustmentService(new FakeAdjustmentRepository(), manualConfigServer);
        var firstAdjustment = new AdjustmentParameters() { Id = Guid.NewGuid(), Name = "First" };
        var secondAdjustment = new AdjustmentParameters() { Id = Guid.NewGuid(), Name = "Second" };

        // Act
        await service.AddElementAsync(firstAdjustment);
        await service.AddElementAsync(secondAdjustment);
        await service.SetLevelAsync(firstAdjustment.Id, 1);
        await service.SetLevelAsync(secondAdjustment.Id, 1);

        // Assert
        var result = await service.GetAdjustmentsWithLevelAsync();
        Assert.True(result.Count() == 1);
    }

    [Fact]
    public void AfterInitHasFirstLevel()
    {
        // Arrange
        var repository = Substitute.For<IAdjustmentRepository>();
        var manualConfigService = Substitute.For<IManualConfigurationService>();
        var adjustmentService = new AdjustmentService(repository, manualConfigService);

        // Act
        var currentLevel = adjustmentService.CurrentLevel;

        // Assert
        Assert.Equal(1, currentLevel);
    }

    [Fact]
    public async Task CanUpCurrentLevel()
    {
        // Arrange
        var repository = Substitute.For<IAdjustmentRepository>();
        var manualConfigService = Substitute.For<IManualConfigurationService>();
        var adjustmentService = new AdjustmentService(repository, manualConfigService);

        // Act
        await adjustmentService.AdjustmentStartUp().ConfigureAwait(true);
        await adjustmentService.AdjustmentStartUp().ConfigureAwait(true);
        var currentLevel = adjustmentService.CurrentLevel;

        // Assert
        Assert.Equal(3, currentLevel);
    }

    [Fact]
    public async Task CantUpMoreThanThirdLevel()
    {
        // Arrange
        var repository = Substitute.For<IAdjustmentRepository>();
        var manualConfigService = Substitute.For<IManualConfigurationService>();
        var adjustmentService = new AdjustmentService(repository, manualConfigService);

        // Act
        await adjustmentService.AdjustmentStartUp().ConfigureAwait(true);
        await adjustmentService.AdjustmentStartUp().ConfigureAwait(true);
        await adjustmentService.AdjustmentStartUp().ConfigureAwait(true);
        await adjustmentService.AdjustmentStartUp().ConfigureAwait(true);
        var currentLevel = adjustmentService.CurrentLevel;

        // Assert
        Assert.Equal(3, currentLevel);
    }

    [Fact]
    public async Task CantDownUnderFirstLevel()
    {
        // Arrange
        var repository = Substitute.For<IAdjustmentRepository>();
        var manualConfigService = Substitute.For<IManualConfigurationService>();
        var adjustmentService = new AdjustmentService(repository, manualConfigService);

        // Act
        await adjustmentService.AdjustmentStartDown().ConfigureAwait(true);
        await adjustmentService.AdjustmentStartDown().ConfigureAwait(true);
        await adjustmentService.AdjustmentStartDown().ConfigureAwait(true);
        var currentLevel = adjustmentService.CurrentLevel;

        // Assert
        Assert.Equal(1, currentLevel);
    }

    [Fact]
    public async Task CanUpUnderThirdLevelAndDownAtSecondLevel()
    {
        // Arrange
        var repository = Substitute.For<IAdjustmentRepository>();
        var manualConfigService = Substitute.For<IManualConfigurationService>();
        var adjustmentService = new AdjustmentService(repository, manualConfigService);

        // Act
        await adjustmentService.AdjustmentStartUp().ConfigureAwait(true);
        await adjustmentService.AdjustmentStartUp().ConfigureAwait(true);
        var thirdLevelAchived = adjustmentService.CurrentLevel == 3;
        await adjustmentService.AdjustmentStartDown().ConfigureAwait(true);
        var currentLevel = adjustmentService.CurrentLevel;

        // Assert
        Assert.True(thirdLevelAchived);
        Assert.Equal(2, currentLevel);
    }
}
