using StarkCNC.Services;

namespace StarkCNC.Models;

internal class LoadingModel
{
    public required string Path { get; set; }

    public ModelType Type { get; set; }
}
