using StarkCNC.ViewModels;
using System.Windows;
using System.Windows.Shell;

namespace StarkCNC
{
    /// <summary>
    /// Interaction logic for AdjustmentSettingsWindow.xaml
    /// </summary>
    public partial class AdjustmentSettingsWindow : Window
    {
        private AdjustmentViewModel ViewModel;

        public AdjustmentSettingsWindow(AdjustmentViewModel viewModel, string Title = "Добавление новой оснастки")
        {
            ViewModel = viewModel;
            DataContext = viewModel;

            InitializeComponent();

            WindowChrome.SetWindowChrome(this,
                new WindowChrome
                {
                    CaptionHeight = 50,
                    CornerRadius = new CornerRadius(12),
                    GlassFrameThickness = new Thickness(-1),
                    ResizeBorderThickness = ResizeMode == ResizeMode.NoResize ? default : new Thickness(4),
                    UseAeroCaptionButtons = true,
                    NonClientFrameEdges = NonClientFrameEdges.Right | NonClientFrameEdges.Bottom | NonClientFrameEdges.Left
                }
            );

            Topmost = true;

            TitleTB.Text = Title;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
