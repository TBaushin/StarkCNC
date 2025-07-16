using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Extensions;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using SkiaSharp;
using System.Windows;
using System.Windows.Controls;

namespace StarkCNC.Controls
{
    /// <summary>
    /// Interaction logic for SpeedControl.xaml
    /// </summary>
    public partial class SpeedControl : UserControl
    {
        public static readonly DependencyProperty ControlNameProperty = DependencyProperty.Register("ControlName", typeof(string), typeof(SpeedControl), new PropertyMetadata());
        public static readonly DependencyProperty SpeedProperty = DependencyProperty.Register("Speed", typeof(double), typeof(SpeedControl), new PropertyMetadata(0.0, OnSpeedChanged));

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

        private NeedleVisual _needle;

        public SpeedControl()
        {
            InitializeComponent();

            Gauge.Series = GaugeGenerator.BuildAngularGaugeSections(
                new GaugeItem(50, s => SetStyle(14, 12, new SolidColorPaint(SKColors.DeepSkyBlue), s)),
                new GaugeItem(25, s => SetStyle(14, 12, new SolidColorPaint(SKColors.Orange), s)),
                new GaugeItem(25, s => SetStyle(14, 12, new SolidColorPaint(SKColors.Red), s))
            );

            _needle = new NeedleVisual
            {
                Value = Speed ?? 0,
                Fill = new SolidColorPaint(SKColors.White)
            };

            Gauge.VisualElements = [new AngularTicksVisual {
                Labeler = value => value.ToString("N1"),
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

        private void SetStyle(double sectionsOuter, double sectionWidth, SolidColorPaint color, PieSeries<ObservableValue> series)
        {
            series.OuterRadiusOffset = sectionsOuter;
            series.MaxRadialColumnWidth = sectionWidth;
            series.CornerRadius = 0;
            series.Fill = color;
        }
    }
}
