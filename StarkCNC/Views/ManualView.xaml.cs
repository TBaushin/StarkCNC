using StarkCNC.Helpers;
using StarkCNC.ViewModels;
using System.Windows.Controls;
using System.Windows.Media;

namespace StarkCNC.Views
{
    /// <summary>
    /// Interaction logic for ManualView.xaml
    /// </summary>
    public partial class ManualView : Page
    {
        private readonly ManualViewModel ViewModel;

        public ManualView(ManualViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;

            InitializeComponent();

            IsVisibleChanged += ManualView_IsVisibleChanged;
        }

        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !OnlyNumberEnterHelper.IsTextAllowed(e.Text);

            base.OnPreviewTextInput(e);
        }

        private void SqueezeBackButton_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ViewModel.FirstSqueeze.BackwardStartCommand.Execute(null);
        }

        private void SqueezeBackButton_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ViewModel.FirstSqueeze.BackwardCancelCommand.Execute(null);
        }

        private void SqueezeForwardButton_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ViewModel.FirstSqueeze.ForwardStartCommand.Execute(null);
        }

        private void SqueezeForwardButton_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ViewModel.FirstSqueeze.ForwardCancelCommand.Execute(null);
        }

        private void SupportButton_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ViewModel.Support.RunCommand.Execute(null);
        }

        private void SupportButton_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ViewModel.Support.CancelCommand.Execute(null);
        }

        private void DornLubricantButton_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ViewModel.DornLubricant.RunCommand.Execute(null);
        }

        private void DornLubricantButton_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ViewModel.DornLubricant.CancelCommand.Execute(null);
        }

        private void BendAndSqueezeButton_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ViewModel.BendAndSqueeze.RunCommand.Execute(null);
        }

        private void BendAndSqueezeButton_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ViewModel.BendAndSqueeze.CancelCommand.Execute(null);
        }

        private void ManualView_IsVisibleChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is not bool value)
                return;

            if (value)
                ViewModel.ManualModeTurnOnCommand.Execute(null);
            else
                ViewModel.ManualModeTurnOffCommand.Execute(null);
        }
    }
}
