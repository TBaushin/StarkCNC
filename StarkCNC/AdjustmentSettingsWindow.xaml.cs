using StarkCNC.Core.Models;
using StarkCNC.ViewModels;
using System.Windows;
using System.Windows.Shell;

namespace StarkCNC;

/// <summary>
/// Interaction logic for AdjustmentSettingsWindow.xaml
/// </summary>
public partial class AdjustmentSettingsWindow : Window
{
    private AdjustmentViewModel ViewModel;
    private AdjustmentParameters? _oldParamaters;

    public AdjustmentParameters? Result { get; set; }
    public IEnumerable<AdjustmentType>? Types { get; }

    public AdjustmentSettingsWindow(AdjustmentViewModel viewModel, string Title = "Добавление новой оснастки")
    {
        ViewModel = viewModel;

        DataContext = this;

        Types = ViewModel.Types;

        _oldParamaters = ViewModel.SelectedAdjustment?.Cast().Copy();

        if (_oldParamaters is not null)
            Result = new AdjustmentParameters(_oldParamaters.Name, _oldParamaters.Type)
            {
                PipeDiameter = _oldParamaters.PipeDiameter,
                Radius = _oldParamaters.Radius
            };
        else
            Result = new AdjustmentParameters("", Types.First());

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
        if (_oldParamaters is not null)
            Result = _oldParamaters;
        else
            Result = null;

        Close();
    }
}
