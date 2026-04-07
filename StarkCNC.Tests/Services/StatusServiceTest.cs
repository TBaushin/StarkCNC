using StarkCNC.Core.Models;
using StarkCNC.Core.Services;
using System.Threading.Channels;

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
        statusService.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(statusService.CurrentStatus))
                changed = true;
        };
        statusService.CurrentStatus = new Status("TestMessage");

        // Assert
        Assert.True(changed);
    }

    [Fact]
    public void ChangeCurrentStatusWillAddIntoHistory()
    {
        // Arrange
        var statusService = new StatusService();
        var beforeHistoryCountElements = statusService.History.Count;

        // Act
        statusService.CurrentStatus = new Status("TestMessage");
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

        // Act
        statusService.NotifyCollectionChanged += (sender, args) =>
        {
            if (sender is List<Status> history)
                changed = true;
        };
        statusService.CurrentStatus = new Status("TestMessage");

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
        statusService.CurrentStatus = expected;
        var result = statusService.History;

        // Assert
        Assert.NotNull(result);
        Assert.Contains(expected, result);
    }
}
