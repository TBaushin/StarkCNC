using Microsoft.Extensions.Configuration;
using StarkCNC.Core.Models;
using StarkCNC.Helpers;
using StarkCNC.ViewModels;
using System.Globalization;
using System.Windows;
using System.Windows.Shell;

namespace StarkCNC;

/// <summary>
/// Interaction logic for AdjustmentSettingsWindow.xaml
/// </summary>
public partial class AdjustmentSettingsWindow : Window
{
    private static readonly AdjustmentTypeToStringConverter _converter = new AdjustmentTypeToStringConverter();
    private AdjustmentViewModel ViewModel;
    private AdjustmentParameters? _oldParameters;

    public AdjustmentParameters? Result { get; set; }

    public IEnumerable<string> Types { get; set; } = new List<string>()
    {
        (string)_converter.Convert(AdjustmentType.Winding, typeof(AdjustmentType), new { }, CultureInfo.CurrentCulture),
        (string)_converter.Convert(AdjustmentType.Rolling, typeof(AdjustmentType), new { }, CultureInfo.CurrentCulture)
    };

    public string SelectedType
    {
        get
        {
            if (Result is not null)
                return (string)_converter.Convert(Result.Type, typeof(AdjustmentType), new { }, CultureInfo.CurrentCulture);

            if (_oldParameters is not null)
                return (string)_converter.Convert(_oldParameters.Type, typeof(AdjustmentType), new { }, CultureInfo.CurrentCulture);

            return string.Empty;
        }
        set
        {
            if (Result is not null)
                Result.Type = (AdjustmentType)_converter.ConvertBack(value, typeof(AdjustmentType), new { }, CultureInfo.CurrentCulture);
        }
    }

    public AdjustmentSettingsWindow(IConfiguration configuration, AdjustmentViewModel viewModel, string Title = "Добавление новой оснастки")
    {
        ViewModel = viewModel;

        DataContext = this;

        _oldParameters = ViewModel.SelectedAdjustment?.Cast().Copy();

        if (_oldParameters is not null)
            Result = new AdjustmentParameters(configuration, _oldParameters.Name)
            {
                Type = _oldParameters.Type,
                PipeDiameter = _oldParameters.PipeDiameter,
                Radius = _oldParameters.Radius,
                ForwardDangerZoneCoordinate = _oldParameters.ForwardDangerZoneCoordinate,
                DistanceFromCenter = _oldParameters.DistanceFromCenter,
                BendRoller = _oldParameters.BendRoller,
                Clamp = _oldParameters.Clamp,
                ClampRoller = _oldParameters.ClampRoller,
                Console = _oldParameters.Console,
                Press = _oldParameters.Press,
                Squeeze = _oldParameters.Squeeze,
            };
        else
            Result = new AdjustmentParameters(configuration, "");

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
        if (_oldParameters is not null)
            Result = _oldParameters;
        else
            Result = null;

        Close();
    }
}
