using StarkCNC.Core.Models;
using StarkCNC.Services;

namespace StarkCNC.Tests.Services;

public class StatusServiceTest
{
    [Fact]
    public void ChangeCurrentStatusWillNotify()
    {
        // Arrange
        var statusService = new StatusService();
        bool changed = false;

        // Act
        statusService.CurrentStatuses.CollectionChanged += (sender, args) =>
        {
            changed = true;
        };
        statusService.AddStatus(new Status("TestMessage"));

        // Assert
        Assert.True(changed);
    }

    [Fact]
    public void ChangeCurrentStatusWillAddIntoHistory()
    {
        // Arrange
        var statusService = new StatusService();
        var beforeHistoryCountElements = statusService.History.Count;
        var status = new Status("TestMessage");

        // Act
        statusService.AddStatus(status);
        statusService.RemoveStatus(status);
        var currentHistoryCountElements = statusService.History.Count;

        // Assert
        Assert.NotEqual(beforeHistoryCountElements, currentHistoryCountElements);
    }

    [Fact]
    public void ChangeCurrentStatusWillHistoryChanged()
    {
        // Arrange
        var statusService = new StatusService();
        bool changed = false;
        var status = new Status("TestMessage");

        // Act
        statusService.History.CollectionChanged += (sender, args) =>
        {
            changed = true;
        };
        statusService.AddStatus(status);
        statusService.RemoveStatus(status);

        // Assert
        Assert.True(changed);
        
    }

    [Fact]
    public void AfterChangeCurrentStatusHistoryWillHasNewElement()
    {
        // Arrange
        var statusService = new StatusService();
        var expected = new Status("TestMessage");

        // Act
        statusService.AddStatus(expected);
        statusService.RemoveStatus(expected);
        var result = statusService.History;

        // Assert
        Assert.NotNull(result);
        Assert.Contains(expected, result);
    }
}
