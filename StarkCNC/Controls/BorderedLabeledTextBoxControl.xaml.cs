using StarkCNC.Helpers;
using StarkCNC.ViewModels;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace StarkCNC.Controls
{
    /// <summary>
    /// Interaction logic for BorderedLabeledTextBoxControl.xaml
    /// </summary>
    public partial class BorderedLabeledTextBoxControl : UserControl
    {
        public static readonly DependencyProperty LabelTextProperty = DependencyProperty.Register(nameof(LabelText), typeof(object), typeof(BorderedLabeledTextBoxControl), new PropertyMetadata());
        public static readonly DependencyProperty TextBoxTextProperty = DependencyProperty.Register(nameof(TextBoxText), typeof(string), typeof(BorderedLabeledTextBoxControl), new PropertyMetadata());
        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(nameof(IsReadOnly), typeof(bool), typeof(BorderedLabeledTextBoxControl), new PropertyMetadata());
        public static readonly DependencyProperty NeedCallNumberInputProperty = DependencyProperty.Register(nameof(NeedCallNumberInput), typeof(bool), typeof(BorderedLabeledTextBoxControl), new PropertyMetadata());
        public static readonly DependencyProperty IsNumericOnlyProperty = DependencyProperty.Register(nameof(IsNumericOnly), typeof(bool), typeof(BorderedLabeledTextBoxControl), new PropertyMetadata(false));

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

        public bool IsNumericOnly
        {
            get => (bool)GetValue(IsNumericOnlyProperty);
            set => SetValue(IsNumericOnlyProperty, value);
        }

        public BorderedLabeledTextBoxControl()
        {
            InitializeComponent();
        }

        private void InputTextBox_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (NeedCallNumberInput)
            {
                var value = NumberInputViewModel.ShowDialog();
                TextBoxText = value.ToString(CultureInfo.CurrentCulture);
            }
        }

        private void InputTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (IsNumericOnly)
                e.Handled = !OnlyNumberEnterHelper.IsTextAllowed(e.Text);
        }
    }
}
