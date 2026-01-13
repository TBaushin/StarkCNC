using StarkCNC.Helpers;
using StarkCNC.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace StarkCNC;

/// <summary>
/// Interaction logic for NumberInputBlockWindow.xaml
/// </summary>
public partial class NumberInputBlockWindow : Window
{
    NumberInputViewModel ViewModel;

    public NumberInputBlockWindow(NumberInputViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = ViewModel;

        InitializeComponent();

        Topmost = true;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button button)
            return;

        var content = button.Content;
        if (content is null)
            return;

        var number = content.ToString() ?? string.Empty;
        ViewModel.AddNumber(number);
    }

    private void EnterButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void OutputTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
        e.Handled = !OnlyNumberEnterHelper.IsTextAllowed(e.Text);
    }

    private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        switch ((Decimal)e.Key)
        {
            case (Decimal)Key.NumPad1 or (Decimal)Key.D1:
                OneButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                break;
            case (Decimal)Key.NumPad2 or (Decimal)Key.D2:
                TwoButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                break;
            case (Decimal)Key.NumPad3 or (Decimal)Key.D3:
                ThreeButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                break;
            case (Decimal)Key.NumPad4 or (Decimal)Key.D4:
                FourButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                break;
            case (Decimal)Key.NumPad5 or (Decimal)Key.D5:
                FiveButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                break;
            case (Decimal)Key.NumPad6 or (Decimal)Key.D6:
                SixButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                break;
            case (Decimal)Key.NumPad7 or (Decimal)Key.D7:
                SevenButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                break;
            case (Decimal)Key.NumPad8 or (Decimal)Key.D8:
                EightButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                break;
            case (Decimal)Key.NumPad9 or (Decimal)Key.D9:
                NineButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                break;
            case (Decimal)Key.NumPad0 or (Decimal)Key.D0:
                ZeroButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                break;
            case (Decimal)Key.OemComma or (Decimal)Key.OemPeriod or (Decimal)88:
                PointButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                PointButton.Command?.Execute(null);
                break;
            case (Decimal)Key.Enter:
                EnterButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                break;
            case (Decimal)Key.Back:
                ClearLastButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                ClearLastButton.Command?.Execute(null);
                break;
            case (Decimal)Key.Escape:
                ViewModel.CancelExitCommand?.Execute(this);
                break;
        }
    }
}