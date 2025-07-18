using StarkCNC.Helpers;
using System.Windows.Controls;
using System.Windows.Media;

namespace StarkCNC.Views
{
    /// <summary>
    /// Interaction logic for ManualView.xaml
    /// </summary>
    public partial class ManualView : Page
    {
        public ManualView()
        {
            InitializeComponent();
        }

        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !OnlyNumberEnterHelper.IsTextAllowed(e.Text);

            base.OnPreviewTextInput(e);
        }

        private void PressureBackButton_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            PressureBackIndicatorFirst.Color = Colors.Green;
            PressureBackIndicatorSecond.Color = Colors.Green;
        }

        private void PressureBackButton_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            PressureBackIndicatorFirst.Color = Colors.DarkRed;
            PressureBackIndicatorSecond.Color = Colors.DarkRed;
        }

        private void PressureForwardButton_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            PressureForwardIndicatorFirst.Color = Colors.Green;
            PressureForwardIndicatorSecond.Color = Colors.Green;
        }

        private void PressureForwardButton_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            PressureForwardIndicatorFirst.Color = Colors.DarkRed;
            PressureForwardIndicatorSecond.Color = Colors.DarkRed;
        }
    }
}
