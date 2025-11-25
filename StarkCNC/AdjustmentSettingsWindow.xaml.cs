using StarkCNC.Core.Models;
using StarkCNC.DTO;
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
    private AdjustmentParametersVisibleDto? _oldParameters;

    public AdjustmentParametersVisibleDto? Result { get; set; }

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
                return (string)_converter.Convert(Result.AdjustmentType, typeof(AdjustmentType), new { }, CultureInfo.CurrentCulture);

            if (_oldParameters is not null)
                return (string)_converter.Convert(_oldParameters.AdjustmentType, typeof(AdjustmentType), new { }, CultureInfo.CurrentCulture);

            return string.Empty;
        }
        set
        {
            if (Result is not null)
                Result.AdjustmentType = (AdjustmentType)_converter.ConvertBack(value, typeof(AdjustmentType), new { }, CultureInfo.CurrentCulture);
        }
    }

    public AdjustmentSettingsWindow(AdjustmentParametersVisibleDto dto, string Title = "Добавление новой оснастки")
    {
        _oldParameters = dto;
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
        if (_oldParameters is not null)
            Result = _oldParameters;
        else
            Result = null;

        Close();
    }
}