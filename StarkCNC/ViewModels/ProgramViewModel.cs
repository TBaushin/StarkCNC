using CommunityToolkit.Mvvm.ComponentModel;
using StarkCNC.Models;
using System.Collections.ObjectModel;

namespace StarkCNC.ViewModels
{
    public class ProgramViewModel : ObservableObject
    {
        public ObservableCollection<BendingData> bendingDatas { get; set; } = new ObservableCollection<BendingData>();
    }
}
