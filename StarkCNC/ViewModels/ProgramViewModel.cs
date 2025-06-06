using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using StarkCNC._3DViewer.Views;
using StarkCNC.Models;
using System.Collections.ObjectModel;

namespace StarkCNC.ViewModels
{
    public class ProgramViewModel : ObservableObject
    {
        private IServiceProvider _serviceProvider;

        public ObservableCollection<BendingData> bendingDatas { get; set; } = new ObservableCollection<BendingData>();

        public ProgramViewModel(IServiceProvider serviceProvider) 
        {
            _serviceProvider = serviceProvider;
        }

        public ProgramControllerView GetProgramControllerView()
        {
            return _serviceProvider.GetRequiredService<ProgramControllerView>();
        }
    }
}
