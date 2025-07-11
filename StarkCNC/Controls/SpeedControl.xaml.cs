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
        public static readonly DependencyProperty SpeedProperty = DependencyProperty.Register("Speed", typeof(double), typeof(SpeedControl), new PropertyMetadata());

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

        public SpeedControl()
        {
            InitializeComponent();
        }
    }
}
