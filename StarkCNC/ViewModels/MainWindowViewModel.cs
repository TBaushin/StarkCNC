using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace StarkCNC.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _title = "StarkCNC";

        [ObservableProperty]
        private Page? _selectedPage;

        public ObservableCollection<Page> Pages { get; set; }

        public MainWindowViewModel() => Pages = [];
    }
}
