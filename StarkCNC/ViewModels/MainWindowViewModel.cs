using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarkCNC.ViewModels
{
    internal class MainWindowViewModel : IPage
    {
        public ObservableCollection<IPage> Pages { get; set; }

        MainWindowViewModel() => Pages = [];
    }
}
