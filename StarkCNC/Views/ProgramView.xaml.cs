using StarkCNC.ViewModels;
using System.Windows.Controls;

namespace StarkCNC.Views
{
    /// <summary>
    /// Interaction logic for ProgramView.xaml
    /// </summary>
    public partial class ProgramView : Page
    {
        ProgramViewModel ViewModel;

        public ProgramView(ProgramViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;
            InitializeComponent();

            BendingView.Children.Add(ViewModel.GetModels());
        }
    }
}
