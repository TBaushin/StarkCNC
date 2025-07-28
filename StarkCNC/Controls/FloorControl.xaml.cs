using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace StarkCNC.Controls
{
    /// <summary>
    /// Interaction logic for FloorControl.xaml
    /// </summary>
    public partial class FloorControl : UserControl
    {
        public static DependencyProperty FloorTextProperty = DependencyProperty.Register(nameof(FloorText), typeof(string), typeof(FloorControl), new PropertyMetadata());
        public static DependencyProperty SearchItemPathProperty = DependencyProperty.Register(nameof(SearchItemPath), typeof(string), typeof(FloorControl), new PropertyMetadata());
        public static DependencyProperty SearchSourceProperty = DependencyProperty.Register(nameof(SearchSource), typeof(IEnumerable), typeof(FloorControl), new PropertyMetadata());
        public static DependencyProperty PipeDiameterProperty = DependencyProperty.Register(nameof(PipeDiameter), typeof(double), typeof(FloorControl), new PropertyMetadata());

        public static ImageSource TurnOnImage;
        public static ImageSource TurnOffImage;

        public string FloorText
        {
            get => (string)GetValue(FloorTextProperty);
            set => SetValue(FloorTextProperty, value);
        }

        public string SearchItemPath
        {
            get => (string)GetValue(SearchItemPathProperty);
            set => SetValue(SearchItemPathProperty, value);
        }

        public IEnumerable SearchSource
        {
            get => (IEnumerable)GetValue(SearchSourceProperty);
            set => SetValue(SearchSourceProperty, value);
        }

        public double PipeDiameter
        {
            get => (double)GetValue(PipeDiameterProperty);
            set => SetValue(PipeDiameterProperty, value);
        }

        public FloorControl()
        {
            InitializeComponent();

            RollerImage.Source = TurnOffImage;
        }

        private void FloorCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not CheckBox cb)
                return;

            var check = cb.IsChecked;
            if (check is null)
                return;

            if ((bool)check)
                RollerImage.Source = TurnOnImage;
            else
                RollerImage.Source = TurnOffImage;

        }
    }
}
