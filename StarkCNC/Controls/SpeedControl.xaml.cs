using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Extensions;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using SkiaSharp;
using StarkCNC.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StarkCNC.Controls
{
    /// <summary>
    /// Interaction logic for SpeedControl.xaml
    /// </summary>
    public partial class SpeedControl : UserControl
    {
        public static readonly DependencyProperty ControlNameProperty = DependencyProperty.Register("ControlName", typeof(string), typeof(SpeedControl), new PropertyMetadata());
        public static readonly DependencyProperty SpeedProperty = DependencyProperty.Register("Speed", typeof(double), typeof(SpeedControl), new PropertyMetadata(0.0, OnSpeedChanged));
        public static readonly DependencyProperty ResetCommandProperty = DependencyProperty.Register("ResetCommand", typeof(ICommand), typeof(SpeedControl), new PropertyMetadata());
        public static readonly DependencyProperty ForwardCommandProperty = DependencyProperty.Register("ForwardCommand", typeof(ICommand), typeof (SpeedControl), new PropertyMetadata());
        public static readonly DependencyProperty ForwardCancelCommandProperty = DependencyProperty.Register("ForwardCancelCommand", typeof(ICommand), typeof(SpeedControl), new PropertyMetadata());
        public static readonly DependencyProperty BackwardCommandProperty = DependencyProperty.Register("BackwardCommand", typeof(ICommand), typeof(SpeedControl), new PropertyMetadata());
        public static readonly DependencyProperty BackwardCancelCommandProperty = DependencyProperty.Register("BackwardCancelCommand", typeof(ICommand), typeof(SpeedControl), new PropertyMetadata());
        public static readonly DependencyProperty RelativeDispositionCommandProperty = DependencyProperty.Register("RelativeDispositionCommand", typeof(ICommand), typeof(SpeedControl), new PropertyMetadata());

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

        private void BackButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            BackwardCommand.Execute(this);
        }

        private void BackButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            BackwardCancelCommand.Execute(this);
        }

        private void ForwardButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ForwardCommand.Execute(this);
        }
        private void ForwardButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ForwardCancelCommand.Execute(this);
        }

        private void RelativeDisplacementButton_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void RelativeDisplacementButton_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void SpeedTb_Click(object sender, RoutedEventArgs e)
        {
            var value = NumberInputViewModel.ShowDialog();
            SpeedTb.TextBoxText = value.ToString();
            Speed = value;
        }
    }
}
