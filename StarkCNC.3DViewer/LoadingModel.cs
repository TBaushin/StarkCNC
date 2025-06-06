using StarkCNC._3DViewer.Services;

namespace StarkCNC._3DViewer
{
    internal class LoadingModel
    {
        public required string Path { get; set; }

        public ModelType Type { get; set; }
    }
}
