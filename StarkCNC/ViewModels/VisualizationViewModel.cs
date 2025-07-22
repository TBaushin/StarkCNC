using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using StarkCNC._3DViewer.Views;

namespace StarkCNC.ViewModels
{
    public class VisualizationViewModel : ObservableObject
    {
        private readonly IServiceProvider _serviceProvider;

        public VisualizationViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public VisualizationControllerView GetVisualizationControllerView()
        {
            return _serviceProvider.GetRequiredService<VisualizationControllerView>();
        }
    }
}
