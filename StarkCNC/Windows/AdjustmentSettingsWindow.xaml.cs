using StarkCNC.Core.Models;
using StarkCNC.Services;
using StarkCNC.Utilities;
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

    public AdjustmentSettingsWindow(AdjustmentParameters? adjustment, AdjustmentParametersConstructor constructor, string Title = "Добавление новой оснастки")
    {
        _oldParameters = adjustment;
        DataContext = this;

        if (adjustment is null)
            Result = constructor.Build(string.Empty, AdjustmentType.Winding, 0, 0).Result;
        else
            Result = (AdjustmentParameters)adjustment.Clone();

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
        if (string.IsNullOrEmpty(NameTextBox.Text) || NameTextBox.Text.Length == 0)
        {
            NotFilledWarningTextBlock.Visibility = Visibility.Visible;
            return;
        }

        Result?.Name = NameTextBox.Text;
        Result?.PipeDiameter = Convert.ToDouble(PipeDiameterTextBox.Text, CultureInfo.InvariantCulture);
        Result?.Radius = Convert.ToDouble(RadiusTextBox.Text, CultureInfo.InvariantCulture);
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

    private void NameTextBox_TextChanged(object sender, RoutedEventArgs e)
    {
        if (NameTextBox.Text.Length > 0)
            NotFilledWarningTextBlock.Visibility = Visibility.Collapsed;
    }
}