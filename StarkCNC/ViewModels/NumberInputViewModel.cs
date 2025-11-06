using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Globalization;

namespace StarkCNC.ViewModels;

public partial class NumberInputViewModel : ObservableObject
{
    private bool _needAddPoint;

    public double ResultValue { get; set; }

    [ObservableProperty]
    private string _outputValue = string.Empty;

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
        ResultValue = 0;
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
        if (_needAddPoint)
            return;

        if (OutputValue.Length == 0)
            OutputValue = "0";
        OutputValue += ".";
        _needAddPoint = true;
    }

    private void TryConvertToDouble()
    {
        Double.TryParse(OutputValue, NumberStyles.Any, CultureInfo.InvariantCulture, out var result);
        ResultValue = result;
    }

    public static double ShowDialog()
    {
        var viewModel = new NumberInputViewModel();
        var window = new NumberInputBlockWindow(viewModel);
        window.ShowDialog();
        return viewModel.ResultValue;
    }
}