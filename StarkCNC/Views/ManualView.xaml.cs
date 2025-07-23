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

            ViewModel.FirstSqueeze.PropertyChanged += FirstSqueeze_PropertyChanged;
        }

        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !OnlyNumberEnterHelper.IsTextAllowed(e.Text);

            base.OnPreviewTextInput(e);
        }

        private async void SqueezeBackButton_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            await ViewModel.FirstSqueeze.BackwardStartCommand.ExecuteAsync(null);
        }

        private async void SqueezeBackButton_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            await ViewModel.FirstSqueeze.BackwardCancelCommand.ExecuteAsync(null);
        }

        private async void SqueezeForwardButton_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            await ViewModel.FirstSqueeze.ForwardStartCommand.ExecuteAsync(null);
        }

        private async void SqueezeForwardButton_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            await ViewModel.FirstSqueeze.ForwardCancelCommand.ExecuteAsync(null);
        }

        private async void SupportButton_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            await ViewModel.Support.RunCommand.ExecuteAsync(null);
        }

        private async void SupportButton_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            await ViewModel.Support.CancelCommand.ExecuteAsync(null);
        }

        private async void DornLubricantButton_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            await ViewModel.DornLubricant.RunCommand.ExecuteAsync(null);
        }

        private async void DornLubricantButton_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            await ViewModel.DornLubricant.CancelCommand.ExecuteAsync(null);
        }

        private async void BendAndSqueezeButton_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            await ViewModel.BendAndSqueeze.RunCommand.ExecuteAsync(null);
        }

        private async void BendAndSqueezeButton_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            await ViewModel.BendAndSqueeze.CancelCommand.ExecuteAsync(null);
        }

        private void FirstSqueeze_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            SetSqueezeBackIndicatorFirst();

            SetSqueezeFrontIndicatorFirst();
        }

        private void SetSqueezeBackIndicatorFirst()
        {
            if (ViewModel.FirstSqueeze.RearPosition)
                SqueezeBackIndicatorFirst.Color = Colors.Green;
            else
                SqueezeBackIndicatorFirst.Color = Colors.DarkRed;
        }

        private void SetSqueezeFrontIndicatorFirst()
        {
            if (ViewModel.FirstSqueeze.FrontPosition)
                SqueezeForwardIndicatorFirst.Color = Colors.Green;
            else
                SqueezeForwardIndicatorFirst.Color = Colors.DarkRed;
        }
    }
}
