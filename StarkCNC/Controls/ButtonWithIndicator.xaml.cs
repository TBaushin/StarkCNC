using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace StarkCNC.Controls
{
    /// <summary>
    /// Interaction logic for ButtonWithIndicator.xaml
    /// </summary>
    public partial class ButtonWithIndicator : UserControl
    {
        public static readonly DependencyProperty ButtonContentProperty = DependencyProperty.Register("ButtonContent", typeof(object), typeof(ButtonWithIndicator), new PropertyMetadata());
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(IAsyncRelayCommand), typeof(ButtonWithIndicator), new PropertyMetadata());
        public static readonly DependencyProperty CommandCancelProperty = DependencyProperty.Register("CommandCancel", typeof(IAsyncRelayCommand), typeof(ButtonWithIndicator), new PropertyMetadata());
        public static readonly DependencyProperty IndicatorActivatedProperty = DependencyProperty.Register("IndicatorActivated", typeof(bool), typeof(ButtonWithIndicator), new PropertyMetadata());

        public object? ButtonContent
        {
            get => (object)GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        public IAsyncRelayCommand Command
        {
            get => (IAsyncRelayCommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public IAsyncRelayCommand CommandCancel
        {
            get => (IAsyncRelayCommand)GetValue(CommandCancelProperty);
            set => SetValue(CommandCancelProperty, value);
        }

        public bool IndicatorActivated
        {
            get => (bool)GetValue(IndicatorActivatedProperty);
            set
            {
                SetValue(IndicatorActivatedProperty, value);
                SetColor(value);
            }
        }

        public event RoutedEventHandler Click;

        public new event MouseButtonEventHandler MouseUp;

        public new event MouseButtonEventHandler MouseDown;

        public ButtonWithIndicator()
        {
            InitializeComponent();
        }

        private async void Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            await Command.ExecuteAsync(null);
        }

        private async void Button_MouseUp(object sender, MouseButtonEventArgs e)
        {
            await CommandCancel.ExecuteAsync(null);
        }

        private void SetColor(object? value)
        {
            if (value is null || value is not bool)
                return;

            var booleanValue = (bool)value;
            if (booleanValue)
                Indicator.Color = Colors.Green;
            else
                Indicator.Color = Colors.DarkRed;
        }
    }
}
