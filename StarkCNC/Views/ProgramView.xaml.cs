using Microsoft.Extensions.DependencyInjection;
using StarkCNC._3DViewer.Views;
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

        public ProgramView(ProgramViewModel viewModel, IServiceProvider serviceProvider)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;
            InitializeComponent();

            ProgramControllerBorder.Child = serviceProvider.GetRequiredService<ProgramControllerView>();
        }
    }
}
