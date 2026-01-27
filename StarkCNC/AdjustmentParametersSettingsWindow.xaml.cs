using StarkCNC.Core.Models;
using StarkCNC.Core.Models.Adjustment;
using System.Windows;
using System.Windows.Shell;

namespace StarkCNC;

/// <summary>
/// Interaction logic for AdjustmentParametersSettingsWindow.xaml
/// </summary>
public partial class AdjustmentParametersSettingsWindow : Window
{
    public AdjustmentParameters Adjustment { get; set; }

    public double CurrentPositionCoordinate { get; set; }

    public AdjustmentParametersSettingsWindow(AdjustmentParameters adjustment, string parameter)
    {
        Adjustment = adjustment;
        DataContext = this;

        InitializeComponent();

        ShowParamatersEdits(parameter);

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

    private void ShowParamatersEdits(string parameter)
    {
        switch (parameter)
        {
            case nameof(Adjustment.Supply):
                SupplyStackPanel.Visibility = Visibility.Visible;
                SpeedCoefficient.DataContext = Adjustment.Supply;
                TitleTextBlock.Text = "Подача";
                break;
            case nameof(Adjustment.Console):
                ConsoleStackPanel.Visibility = Visibility.Visible;
                SpeedCoefficient.DataContext = Adjustment.Console;
                TitleTextBlock.Text = "Консоль";
                break;
            case nameof(Adjustment.Rotation):
                RotationStackPanel.Visibility = Visibility.Visible;
                SpeedCoefficient.DataContext = Adjustment.Rotation;
                TitleTextBlock.Text = "Поворот";
                break;
            case nameof(Adjustment.Bend):
                BendStackPanel.Visibility = Visibility.Visible;
                SpeedCoefficient.Visibility = Visibility.Collapsed;
                TitleTextBlock.Text = "Гиб";
                break;
            case nameof(Adjustment.Squeeze):
                SqueezeStackPanel.Visibility = Visibility.Visible;
                SpeedCoefficient.DataContext = Adjustment.Squeeze;
                TitleTextBlock.Text = "Дожим";
                break;
            case nameof(Adjustment.Clamp):
                ClampDornPressStackPanel.Visibility = Visibility.Visible;
                ClampDornPressStackPanel.DataContext = Adjustment.Clamp;
                SpeedCoefficient.DataContext = Adjustment.Clamp;
                TitleTextBlock.Text = "Зажим";
                break;
            case nameof(Adjustment.Dorn):
                ClampDornPressStackPanel.Visibility = Visibility.Visible;
                ClampDornPressStackPanel.DataContext = Adjustment.Dorn;
                SpeedCoefficient.DataContext = Adjustment.Dorn;
                TitleTextBlock.Text = "Дорн";
                break;
            case nameof(Adjustment.Press):
                ClampDornPressStackPanel.Visibility = Visibility.Visible;
                ClampDornPressStackPanel.DataContext = Adjustment.Press;
                SpeedCoefficient.DataContext = Adjustment.Press;
                TitleTextBlock.Text = "Прижим";
                break;
            case nameof(Adjustment.Lift):
                LiftStackPanel.Visibility = Visibility.Visible;
                SpeedCoefficient.DataContext = Adjustment.Lift;
                TitleTextBlock.Text = "Подъём";
                break;
        }
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void SupplyPressZoneSetCurrentPositionButton_Click(object sender, RoutedEventArgs e)
    {
        Adjustment.Supply.PressZonePosition = CurrentPositionCoordinate;
    }

    private void SupplyForwardDangerZoneSetCurrentPositionButton_Click(object sender, RoutedEventArgs e)
    {
        Adjustment.Supply.ForwardDangerZonePosition = CurrentPositionCoordinate;
    }

    private void SupplyColletJawsDepthSetCurrentPosition_Click(object sender, RoutedEventArgs e)
    {
        Adjustment.Supply.ColletJawsDepth = CurrentPositionCoordinate;
    }

    private void ConsoleBendSetCurrentPosition_Click(object sender, RoutedEventArgs e)
    {
        Adjustment.Console.BendPosition = CurrentPositionCoordinate;
    }

    private void ConsoleSecondFloorSetCurrentPosition_Click(object sender, RoutedEventArgs e)
    {
        Adjustment.Console.SecondFloorPosition = CurrentPositionCoordinate;
    }

    private void ConsoleThirdFloorSetCurrentPosition_Click(object sender, RoutedEventArgs e)
    {
        Adjustment.Console.ThirdFloorPosition = CurrentPositionCoordinate;
    }

    private void ClampDornPressForwardSetCurrentPosition_Click(object sender, RoutedEventArgs e)
    {
        var context = ClampDornPressStackPanel.DataContext;
        if (context is null)
            return;

        var type = context.GetType();
        if (type == typeof(Clamp))
        {
            Adjustment.Clamp.ForwardPosition = CurrentPositionCoordinate;
            return;
        }

        if (type == typeof(Dorn))
        {
            Adjustment.Dorn.ForwardPosition = CurrentPositionCoordinate;
            return;
        }

        if (type == typeof(Press))
        {
            Adjustment.Press.ForwardPosition = CurrentPositionCoordinate;
            return;
        }
    }

    private void ClampDornPressMiddleSetCurrentPosition_Click(object sender, RoutedEventArgs e)
    {
        var context = ClampDornPressStackPanel.DataContext;
        if (context is null)
            return;

        var type = context.GetType();
        if (type == typeof(Clamp))
        {
            Adjustment.Clamp.MiddlePosition = CurrentPositionCoordinate;
            return;
        }

        if (type == typeof(Dorn))
        {
            Adjustment.Dorn.MiddlePosition = CurrentPositionCoordinate;
            return;
        }

        if (type == typeof(Press))
        {
            Adjustment.Press.MiddlePosition = CurrentPositionCoordinate;
            return;
        }
    }

    private void ClampDornPressBackwardSetCurrentPosition_Click(object sender, RoutedEventArgs e)
    {
        var context = ClampDornPressStackPanel.DataContext;
        if (context is null)
            return;

        var type = context.GetType();
        if (type == typeof(Clamp))
        {
            Adjustment.Clamp.BackwardPosition = CurrentPositionCoordinate;
            return;
        }

        if (type == typeof(Dorn))
        {
            Adjustment.Dorn.BackwardPosition = CurrentPositionCoordinate;
            return;
        }

        if (type == typeof(Press))
        {
            Adjustment.Press.BackwardPosition = CurrentPositionCoordinate;
            return;
        }
    }

    private void LiftUpperSetCurrentPosition_Click(object sender, RoutedEventArgs e)
    {
        Adjustment.Lift.UpperPosition = CurrentPositionCoordinate;
    }

    private void LiftMiddleSetCurrentPosition_Click(object sender, RoutedEventArgs e)
    {
        Adjustment.Lift.MiddlePosition = CurrentPositionCoordinate;
    }

    private void LiftLowerSetCurrentPosition_Click(object sender, RoutedEventArgs e)
    {
        Adjustment.Lift.LowerPosition = CurrentPositionCoordinate;
    }
}