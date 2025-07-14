using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace StarkCNC.Controls
{
    /// <summary>
    /// Interaction logic for RepeatButtonWithIndicator.xaml
    /// </summary>
    public partial class RepeatButtonWithIndicator : UserControl
    {
        public static readonly DependencyProperty ButtonContentProperty = DependencyProperty.Register("ButtonContent", typeof(object), typeof(RepeatButtonWithIndicator), new PropertyMetadata());
        public static readonly DependencyProperty IntervalProperty = DependencyProperty.Register("Interval", typeof(int), typeof(RepeatButtonWithIndicator), new PropertyMetadata(0));
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(RepeatButtonWithIndicator), new PropertyMetadata());

        public object? ButtonContent
        {
            get => (object)GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        public int Interval
        {
            get => (int)GetValue(IntervalProperty);
            set => SetValue(IntervalProperty, value);
        }

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public event RoutedEventHandler Click;

        public new event MouseButtonEventHandler MouseUp;

        public new event MouseButtonEventHandler MouseDown;

        public RepeatButtonWithIndicator()
        {
            InitializeComponent();
        }

        private void RepeatButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Indicator.Color = Colors.Green;
        }

        private void RepeatButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Indicator.Color = Colors.DarkRed;
        }
    }
}
