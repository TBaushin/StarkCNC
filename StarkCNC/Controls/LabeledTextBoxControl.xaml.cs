using StarkCNC.Utilities;
using StarkCNC.ViewModels;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace StarkCNC.Controls;

/// <summary>
/// Interaction logic for LabeledTextBoxControl.xaml
/// </summary>
public partial class LabeledTextBoxControl : UserControl
{
    public static readonly DependencyProperty LabelTextProperty = DependencyProperty
        .Register(nameof(LabelText), typeof(object), typeof(LabeledTextBoxControl), new PropertyMetadata());
    public static readonly DependencyProperty TextProperty = DependencyProperty
        .Register(nameof(Text), typeof(string), typeof(LabeledTextBoxControl), new PropertyMetadata());
    public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty
        .Register(nameof(IsReadOnly), typeof(bool), typeof(LabeledTextBoxControl), new PropertyMetadata());
    public static readonly DependencyProperty NeedCallNumberInputProperty = DependencyProperty
        .Register(nameof(NeedCallNumberInput), typeof(bool), typeof(LabeledTextBoxControl), new PropertyMetadata());
    public static readonly DependencyProperty IsNumericOnlyProperty = DependencyProperty
        .Register(nameof(IsNumericOnly), typeof(bool), typeof(LabeledTextBoxControl), new PropertyMetadata(false));
    public static readonly DependencyProperty DirectionProperty = DependencyProperty
        .Register(nameof(Direction), typeof(DirectionEnum), typeof(LabeledTextBoxControl), new PropertyMetadata(DirectionEnum.TopToBottom));
    public static readonly RoutedEvent TextChangedEvent = EventManager.
        RegisterRoutedEvent(nameof(TextChanged), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(LabeledTextBoxControl));

    public object? LabelText
    {
        get => (object?)GetValue(LabelTextProperty);
        set => SetValue(LabelTextProperty, value);
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
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

    public DirectionEnum Direction
    {
        get => (DirectionEnum)GetValue(DirectionProperty);
        set => SetValue(DirectionProperty, value);
    }

    public bool LeftToRightVisible => Direction == DirectionEnum.LeftToRigth;
    public bool TopToBottomVisible => Direction == DirectionEnum.TopToBottom;

    public event RoutedEventHandler TextChanged
    {
        add => AddHandler(TextChangedEvent, value);
        remove => RemoveHandler(TextChangedEvent, value);
    }

    public LabeledTextBoxControl()
    {
        InitializeComponent();
    }

    private void InputTextBox_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        var binding = BindingOperations.GetBindingExpression(this, LabeledTextBoxControl.TextProperty);

        if (NeedCallNumberInput)
        {
            var value = NumberInputViewModel.ShowDialog(Text);
            Text = value.ToString(CultureInfo.InvariantCulture);

            binding.UpdateSource();
            binding.UpdateTarget();

            e.Handled = true; // ???
        }
    }

    private void InputTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
        if (IsNumericOnly)
            e.Handled = !OnlyNumberEnterHelper.IsTextAllowed(e.Text);
    }

    private void InputTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        var tb = (TextBox)sender;
        SetCurrentValue(TextProperty, tb.Text);

        var args = new RoutedEventArgs(TextChangedEvent, this);
        RaiseEvent(args);
    }

    private void InputTextBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            InputTextBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Down));
        }
    }
}
