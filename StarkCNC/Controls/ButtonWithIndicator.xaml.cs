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
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(ButtonWithIndicator), new PropertyMetadata());
        public static readonly DependencyProperty CommandCancelProperty = DependencyProperty.Register("CommandCancel", typeof(ICommand), typeof(ButtonWithIndicator), new PropertyMetadata());

        public object? ButtonContent
        {
            get => (object)GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public ICommand CommandCancel
        {
            get => (ICommand)GetValue(CommandCancelProperty);
            set => SetValue(CommandCancelProperty, value);
        }

        public event RoutedEventHandler Click;

        public new event MouseButtonEventHandler MouseUp;

        public new event MouseButtonEventHandler MouseDown;

        public ButtonWithIndicator()
        {
            InitializeComponent();
        }

        private void Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Indicator.Color = Colors.Green;
            Command.Execute(this);
        }

        private void Button_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Indicator.Color = Colors.DarkRed;
            CommandCancel.Execute(this);
        }
    }
}
