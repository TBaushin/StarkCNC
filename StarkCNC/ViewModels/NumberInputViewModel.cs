using CommunityToolkit.Mvvm.Input;

namespace StarkCNC.ViewModels
{
    public partial class NumberInputViewModel
    {
        private bool _needAddPoint;
        private string _outputValue = string.Empty;

        public double ResultValue { get; set; } = 0;

        public string OutputValue
        {
            get => _outputValue;
            set
            {
                _outputValue = value;
                TryConvertToDouble();
            }
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

        public void AddNumber(string number)
        {
            var success = Int32.TryParse(number, out var result);
            if (!success)
                return;

            OutputValue += result;
        }

        private void TryConvertToDouble()
        {
            Double.TryParse(OutputValue, out var result);
            ResultValue = result;
        }
    }
}
