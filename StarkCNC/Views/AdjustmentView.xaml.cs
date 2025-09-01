using StarkCNC.Helpers;
using StarkCNC.ViewModels;
using System.Windows.Controls;
using System.Windows.Input;

namespace StarkCNC.Views
{
    /// <summary>
    /// Interaction logic for AdjustmentView.xaml
    /// </summary>
    public partial class AdjustmentView : Page
    {
        private AdjustmentViewModel ViewModel;

        public AdjustmentView(AdjustmentViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = viewModel;

            InitializeComponent();
        }

        private void PipeDiameterInput_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !OnlyNumberEnterHelper.IsTextAllowed(e.Text);
        }

        private void AdjustmentManagement_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void AdjustmentManagement_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void AdjustmentManagement_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ViewModel.GoToAdjustmentListCommand.Execute(null);
        }
    }
}
