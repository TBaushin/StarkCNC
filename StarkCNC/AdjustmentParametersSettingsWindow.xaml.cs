using StarkCNC.Models;
using System.Windows;
using System.Windows.Shell;

namespace StarkCNC;

/// <summary>
/// Interaction logic for AdjustmentParametersSettingsWindow.xaml
/// </summary>
public partial class AdjustmentParametersSettingsWindow : Window
{
    public IEnumerable<TitleValue> TitleValues { get; set; }

    public IEnumerable<TitleValue>? Result { get; set; }

    public AdjustmentParametersSettingsWindow(string title, IEnumerable<TitleValue> titleValues)
    {
        TitleValues = titleValues;
        DataContext = this;

        InitializeComponent();
        TitleTextBlock.Text = Title;

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
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        if (TitleValues is not null)
            Result = TitleValues;
        else
            Result = null;

        Close();
    }
}
