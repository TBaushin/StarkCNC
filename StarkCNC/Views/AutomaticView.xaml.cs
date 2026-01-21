using StarkCNC.ViewModels;
using System.Windows.Controls;
using System.Windows.Threading;

namespace StarkCNC.Views;

/// <summary>
/// Interaction logic for AutomaticView.xaml
/// </summary>
public partial class AutomaticView : Page
{
    private AutomaticViewModel ViewModel;

    private DispatcherTimer _countCompletedDetailsHoldTimer;

    public AutomaticView(AutomaticViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = ViewModel;

        InitializeComponent();

        ViewModel.SetAutomaticModeCommand.Execute(null);

        _countCompletedDetailsHoldTimer = new DispatcherTimer();
        _countCompletedDetailsHoldTimer.Interval = TimeSpan.FromSeconds(0.5);
        _countCompletedDetailsHoldTimer.Tick += (sender, args) => { ViewModel.CountCompletedDetails = 0; };
    }

    private void CountCompletedDetailsLabeledTextBox_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (ViewModel.CanChangeCountDetails)
            _countCompletedDetailsHoldTimer.Start();
    }

    private void CountCompletedDetailsLabeledTextBox_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (ViewModel.CanChangeCountDetails)
            _countCompletedDetailsHoldTimer.Stop();
    }
}
