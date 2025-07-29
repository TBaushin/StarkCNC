using StarkCNC.ViewModels;
using System.Windows.Controls;

namespace StarkCNC.Views
{
    /// <summary>
    /// Interaction logic for AdjustmentListView.xaml
    /// </summary>
    public partial class AdjustmentListView : Page
    {
        AdjustmentViewModel ViewModel;

        public AdjustmentListView(AdjustmentViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;

            InitializeComponent();
        }
    }
}
