using CommunityToolkit.Mvvm.ComponentModel;
using StarkCNC.Models;
using StarkCNC.Services;
using System.Collections.ObjectModel;
using System.Windows.Media.Media3D;

namespace StarkCNC.ViewModels
{
    public class ProgramViewModel : ObservableObject
    {
        public ObservableCollection<BendingData> bendingDatas { get; set; } = new ObservableCollection<BendingData>();

        public ProgramViewModel() { }
    }
}
