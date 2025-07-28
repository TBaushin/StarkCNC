using StarkCNC.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace StarkCNC.Controls
{
    /// <summary>
    /// Interaction logic for LabeledTextBoxControl.xaml
    /// </summary>
    public partial class LabeledTextBoxControl : UserControl
    {
        public static readonly DependencyProperty LabelTextProperty = DependencyProperty.Register("LabelText", typeof(object), typeof(LabeledTextBoxControl), new PropertyMetadata());
        public static readonly DependencyProperty TextBoxTextProperty = DependencyProperty.Register("TextBoxText", typeof(string), typeof(LabeledTextBoxControl), new PropertyMetadata());
        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(LabeledTextBoxControl), new PropertyMetadata());
        public static readonly DependencyProperty NeedCallNumberInputProperty = DependencyProperty.Register("NeedCallNumberInput", typeof(bool), typeof(LabeledTextBoxControl), new PropertyMetadata());

        public object? LabelText
        {
            get => (object?)GetValue(LabelTextProperty);
            set => SetValue(LabelTextProperty, value);
        }

        public string TextBoxText
        {
            get => (string)GetValue(TextBoxTextProperty);
            set => SetValue(TextBoxTextProperty, value);
        }

        public bool IsReadOnly
        {
            get => (bool)GetValue(IsReadOnlyProperty);
            set => SetValue(IsReadOnlyProperty, value);
        }

        public bool NeedCallNumberInput
        {
            get => (bool)GetValue(NeedCallNumberInputProperty);
            set => SetValue(NeedCallNumberInputProperty, value);
        }

        public LabeledTextBoxControl()
        {
            InitializeComponent();
        }

        private void InputTextBox_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (NeedCallNumberInput)
            {
                var value = NumberInputViewModel.ShowDialog();
                InputTextBox.Text = value.ToString();
            }
        }
    }
}
