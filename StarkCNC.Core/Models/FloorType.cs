namespace StarkCNC.Core.Models;

public class FloorType
{
    public string Name { get; set; } = string.Empty;

    public int FloorCount { get; set; } = 1;

    public FloorType(string name, int floorCount)
    {
        Name = name;
        FloorCount = floorCount;
    }
}
