using StarkCNC.Helpers;
using StarkCNC.ViewModels;
using System.Windows.Controls;

namespace StarkCNC.Views
{
    /// <summary>
    /// Interaction logic for AdjustmentView.xaml
    /// </summary>
    public partial class AdjustmentView : Page
    {
        public AdjustmentView(AdjustmentViewModel viewModel)
        {
            DataContext = viewModel;

            InitializeComponent();
        }

        private void PipeDiameterInput_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !OnlyNumberEnterHelper.IsTextAllowed(e.Text);
        }
    }
}
