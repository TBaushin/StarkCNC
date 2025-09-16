using System.Windows;
using System.Windows.Controls;

namespace StarkCNC.Controls
{
    /// <summary>
    /// Interaction logic for PrefixedTextBoxControl.xaml
    /// </summary>
    public partial class PrefixedTextBoxControl : UserControl
    {
        public static readonly DependencyProperty PrefixProperty = DependencyProperty
            .Register(nameof(Prefix), typeof(string), typeof(PrefixedTextBoxControl), new PropertyMetadata(""));
        public static readonly DependencyProperty TextProperty = DependencyProperty
            .Register(nameof(Text), typeof(string), typeof(PrefixedTextBoxControl), new PropertyMetadata(""));

        public string Prefix
        {
            get => (string)GetValue(PrefixProperty);
            set => SetValue(PrefixProperty, value);
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public PrefixedTextBoxControl()
        {
            InitializeComponent();
        }
    }
}
