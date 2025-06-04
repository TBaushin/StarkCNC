using StarkCNC.Services;
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

        IBendingModelsLoadingService _bendingModelsLoadingService;

        public ProgramView(ProgramViewModel viewModel, IBendingModelsLoadingService bendingModelsLoadingService)
        {
            _bendingModelsLoadingService = bendingModelsLoadingService;
            ViewModel = viewModel;
            DataContext = ViewModel;
            InitializeComponent();

            _bendingModelsLoadingService.Load();
            BendingView.Children.Add(_bendingModelsLoadingService.GetModelVisual3D());
        }
    }
}
