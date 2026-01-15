using StarkCNC.Core.Models;
using StarkCNC.Helpers;
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

    public AdjustmentSettingsWindow(AdjustmentParameters? adjustment, string Title = "Добавление новой оснастки")
    {
        _oldParameters = adjustment;
        DataContext = this;

        InitializeComponent();

        if (adjustment is null)
            Result = new AdjustmentParameters()
            {
                Name = string.Empty,
                Type = AdjustmentType.Winding,
                PipeDiameter = 0,
                Radius = 0
            };
        else
            Result = (AdjustmentParameters)adjustment.Clone();

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