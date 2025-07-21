using StarkCNC.ViewModels;
using System.Windows.Controls;

namespace StarkCNC.Views
{
    /// <summary>
    /// Interaction logic for VisualizationView.xaml
    /// </summary>
    public partial class VisualizationView : Page
    {
        private readonly VisualizationViewModel ViewModel;

        public VisualizationView(VisualizationViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();

            VisualizationPage.Content = ViewModel.GetVisualizationControllerView();
        }
    }
}
