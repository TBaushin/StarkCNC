using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Globalization;
using System.Windows;

namespace StarkCNC.ViewModels;

public partial class NumberInputViewModel : ViewModelBase
{
    private string? _originalValue;

    [ObservableProperty]
    private string _outputValue = string.Empty;

    NumberInputViewModel(string? originalValue = null)
    {
        _originalValue = originalValue;
    }

    public void AddNumber(string number)
    {
        var success = Int32.TryParse(number, out var result);
        if (!success)
            return;

        OutputValue += result;
    }

    [RelayCommand]
    private void Clear()
    {
        OutputValue = string.Empty;
    }

    [RelayCommand]
    private void ClearLastCharacter()
    {
        if (OutputValue.Length == 0)
            return;

        OutputValue = OutputValue.Substring(0, OutputValue.Length - 1);
    }

    [RelayCommand]
    private void AddPoint()
    {
        if (OutputValue.Contains('.', StringComparison.CurrentCultureIgnoreCase))
            return;

        if (OutputValue.Length == 0)
            OutputValue = "0";
        OutputValue += ".";
    }

    [RelayCommand]
    private void CancelExit(Window window)
    {
        OutputValue = _originalValue ?? string.Empty;
        window.Close();
    }

    private static double TryConvertToDouble(string outputValue, string? originalValue = null)
    {
        if (string.IsNullOrEmpty(outputValue) && originalValue is not null)
            outputValue = originalValue;

        if (Double.TryParse(outputValue, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
            return result;
        return 0;
    }

    public static double ShowDialog(string? originalValue = null)
    {
        var viewModel = new NumberInputViewModel(originalValue);
        var window = new NumberInputBlockWindow(viewModel);
        window.ShowDialog();
        return TryConvertToDouble(viewModel.OutputValue, originalValue);
    }
}