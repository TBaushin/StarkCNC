using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Extensions;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using SkiaSharp;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StarkCNC.Controls;

/// <summary>
/// Interaction logic for SpeedControl.xaml
/// </summary>
public partial class SpeedControl : UserControl
{
    public static readonly DependencyProperty ControlNameProperty = DependencyProperty
        .Register(nameof(ControlName), typeof(string), typeof(SpeedControl), new PropertyMetadata());
    public static readonly DependencyProperty SpeedProperty = DependencyProperty
        .Register(nameof(Speed), typeof(double), typeof(SpeedControl), new PropertyMetadata(0.0, OnSpeedChanged));
    public static readonly DependencyProperty CoordinateProperty = DependencyProperty
        .Register(nameof(Coordinate), typeof(double), typeof(SpeedControl), new PropertyMetadata(0.0));
    public static readonly DependencyProperty RelativeDisplacementProperty = DependencyProperty
        .Register(nameof(RelativeDisplacement), typeof(double), typeof(SpeedControl), new PropertyMetadata(0.0));
    public static readonly DependencyProperty ResetCommandProperty = DependencyProperty
        .Register(nameof(ResetCommand), typeof(ICommand), typeof(SpeedControl), new PropertyMetadata());
    public static readonly DependencyProperty ForwardCommandProperty = DependencyProperty
        .Register(nameof(ForwardCommand), typeof(ICommand), typeof (SpeedControl), new PropertyMetadata());
    public static readonly DependencyProperty ForwardCancelCommandProperty = DependencyProperty
        .Register(nameof(ForwardCancelCommand), typeof(ICommand), typeof(SpeedControl), new PropertyMetadata());
    public static readonly DependencyProperty BackwardCommandProperty = DependencyProperty
        .Register(nameof(BackwardCommand), typeof(ICommand), typeof(SpeedControl), new PropertyMetadata());
    public static readonly DependencyProperty BackwardCancelCommandProperty = DependencyProperty
        .Register(nameof(BackwardCancelCommand), typeof(ICommand), typeof(SpeedControl), new PropertyMetadata());
    public static readonly DependencyProperty RelativeDispositionCommandProperty = DependencyProperty
        .Register(nameof(RelativeDispositionCommand), typeof(ICommand), typeof(SpeedControl), new PropertyMetadata());
    public static readonly DependencyProperty RelativeDispositionCancelCommandProperty = DependencyProperty
        .Register(nameof(RelativeDispositionCancelCommand), typeof(ICommand), typeof(SpeedControl), new PropertyMetadata());

    public string? ControlName
    {
        get => (string)GetValue(ControlNameProperty);
        set => SetValue(ControlNameProperty, value);
    }

    public double? Speed
    {
        get => (double)GetValue(SpeedProperty);
        set => SetValue(SpeedProperty, value);
    }

    public double? Coordinate
    {
        get => (double)GetValue(CoordinateProperty);
        set => SetValue(CoordinateProperty, value);
    }

    public double? RelativeDisplacement
    {
        get => (double)GetValue(RelativeDisplacementProperty);
        set => SetValue(RelativeDisplacementProperty, value);
    }

    public ICommand ResetCommand
    {
        get => (ICommand)GetValue(ResetCommandProperty);
        set => SetValue(ResetCommandProperty, value);
    }

    public ICommand ForwardCommand
    {
        get => (ICommand)GetValue(ForwardCommandProperty);
        set => SetValue(ForwardCommandProperty, value);
    }

    public ICommand ForwardCancelCommand
    {
        get => (ICommand)GetValue(ForwardCancelCommandProperty);
        set => SetValue(ForwardCancelCommandProperty, value);
    }

    public ICommand BackwardCommand
    {
        get => (ICommand)GetValue(BackwardCommandProperty);
        set => SetValue(BackwardCommandProperty, value);
    }

    public ICommand BackwardCancelCommand
    {
        get => (ICommand)(GetValue(BackwardCancelCommandProperty));
        set => SetValue(BackwardCancelCommandProperty, value);
    }

    public ICommand RelativeDispositionCommand
    {
        get => (ICommand)GetValue(RelativeDispositionCommandProperty);
        set => SetValue(RelativeDispositionCommandProperty, value);
    }

    public ICommand RelativeDispositionCancelCommand
    {
        get => (ICommand)GetValue(RelativeDispositionCancelCommandProperty);
        set => SetValue(RelativeDispositionCancelCommandProperty, value);
    }

    private NeedleVisual _needle;

    public SpeedControl()
    {
        InitializeComponent();

        Gauge.Series = GaugeGenerator.BuildAngularGaugeSections(
            new GaugeItem(100, s => SetStyle(14, 12, new SolidColorPaint(SKColors.DeepSkyBlue), s))
        );

        _needle = new NeedleVisual
        {
            Value = Speed ?? 0,
            Fill = new SolidColorPaint(SKColors.White)
        };

        Gauge.VisualElements = [new AngularTicksVisual {
            Labeler = value => value.ToString("N1", CultureInfo.InvariantCulture),
            LabelsSize = 12,
            LabelsOuterOffset = -15,
            OuterOffset = 15,
            TicksLength = 13,
        }, _needle];
    }

    private static void OnSpeedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is SpeedControl control)
        {
            double newSpeed = (double)e.NewValue;
            control._needle.Value = newSpeed;
        }
    }

    private static void SetStyle(double sectionsOuter, double sectionWidth, SolidColorPaint color, PieSeries<ObservableValue> series)
    {
        series.OuterRadiusOffset = sectionsOuter;
        series.MaxRadialColumnWidth = sectionWidth;
        series.CornerRadius = 0;
        series.Fill = color;
    }

    private void BackButton_MouseDown(object sender, MouseButtonEventArgs e)
    {
        BackwardCommand.Execute(null);
    }

    private void BackButton_MouseUp(object sender, MouseButtonEventArgs e)
    {
        BackwardCancelCommand.Execute(null);
    }

    private void ForwardButton_MouseDown(object sender, MouseButtonEventArgs e)
    {
        ForwardCommand.Execute(null);
    }
    private void ForwardButton_MouseUp(object sender, MouseButtonEventArgs e)
    {
        ForwardCancelCommand.Execute(null);
    }

    private void RelativeDisplacementButton_MouseDown(object sender, MouseButtonEventArgs e)
    {
        RelativeDispositionCommand.Execute(null);
    }

    private void RelativeDisplacementButton_MouseUp(object sender, MouseButtonEventArgs e)
    {
        RelativeDispositionCancelCommand.Execute(null);
    }
}