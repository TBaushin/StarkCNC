using StarkCNC.DTO;
using StarkCNC.Helpers;
using StarkCNC.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StarkCNC.Views;

/// <summary>
/// Interaction logic for AdjustmentView.xaml
/// </summary>
public partial class AdjustmentView : Page
{
    private AdjustmentViewModel ViewModel;
    private int _levelMustBeSetted;

    public AdjustmentView(AdjustmentViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = viewModel;

        InitializeComponent();
        InitializeLevels();
    }

    private void InitializeLevels()
    {
        ThirdLevel.SelectedAdjustment = ViewModel.SetUpAdjustments
            .FirstOrDefault(e => e.InstalledLevel == 3);
        ThirdLevel.Navigate = ViewModel.GoToEditSettingsCommand;

        SecondLevel.SelectedAdjustment = ViewModel.SetUpAdjustments
            .FirstOrDefault(e => e.InstalledLevel == 2);
        SecondLevel.Navigate = ViewModel.GoToEditSettingsCommand;

        FirstLevel.SelectedAdjustment = ViewModel.SetUpAdjustments
            .FirstOrDefault(e => e.InstalledLevel == 1);
        FirstLevel.Navigate = ViewModel.GoToEditSettingsCommand;
    }

    private void PipeDiameterInput_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
        e.Handled = !OnlyNumberEnterHelper.IsTextAllowed(e.Text);
    }

    private void AdjustmentManagement_MouseDown(object sender, MouseButtonEventArgs e)
    {
        ViewModel.SelectedAdjustment = null;
        ViewModel.GoToAdjustmentListCommand.Execute(null);
    }

    private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var listView = sender as ListView;
        if (listView is null)
            return;

        foreach (var item in e.RemovedItems)
        {
            var listViewItem = listView.ItemContainerGenerator.ContainerFromItem(item) as ListViewItem;
            if (listViewItem is null)
                continue;

            var panels = VisualFinder.FindVisualChildren<StackPanel>(listViewItem).ToList();
            panels.ForEach(p => p.Visibility = Visibility.Collapsed);
        }

        foreach (var item in listView.SelectedItems)
        {
            var listViewItem = listView.ItemContainerGenerator.ContainerFromItem(item) as ListViewItem;
            if (listViewItem is null)
                continue;

            var panels = VisualFinder.FindVisualChildren<StackPanel>(listViewItem).ToList();
            panels
                .Where(p => p.Name == "InstallSelection").ToList()
                .ForEach(p => p.Visibility = Visibility.Visible);
        }
    }

    private void SetUpButton_Click(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        if (button is null)
            return;

        var parent = VisualFinder.FindVisualParent<Grid>(button);
        if (parent is null)
            return;

        foreach(var child in parent.Children)
        {
            if (child is not StackPanel panel)
                continue;

            if (panel.Name.Contains("LevelSelection", StringComparison.OrdinalIgnoreCase))
                panel.Visibility = Visibility.Visible;
            else
                panel.Visibility = Visibility.Collapsed;
        }
    }

    private void ThirdLevelButton_Click(object sender, RoutedEventArgs e)
    {
        _levelMustBeSetted = 3;
    }

    private void SecondLevelButton_Click(object sender, RoutedEventArgs e)
    {
        _levelMustBeSetted = 2;
    }

    private void FirstLevelButton_Click(object sender, RoutedEventArgs e)
    {
        _levelMustBeSetted = 1;
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        CollapseButtonsAndClearSelectedItem(sender);
        _levelMustBeSetted = default;
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        SetLevelToAdjustment(_levelMustBeSetted);
        CollapseButtonsAndClearSelectedItem(sender);
        _levelMustBeSetted = default;
    }

    private void SetLevelToAdjustment(int level)
    {
        var adjustment = ViewModel.SelectedAdjustment;
        if (adjustment is null)
            return;

        ViewModel.SetLevelToAdjustmentCommand.Execute(level);

        ClearFloorConrolsSelectedAdjustment(adjustment);
        switch (level)
        {
            case 1:
                FirstLevel.SelectedAdjustment = adjustment;
                FirstLevel.Navigate = ViewModel.GoToEditSettingsCommand;
                break;
            case 2:
                SecondLevel.SelectedAdjustment = adjustment;
                SecondLevel.Navigate = ViewModel.GoToEditSettingsCommand;
                break;
            case 3:
                ThirdLevel.SelectedAdjustment = adjustment;
                ThirdLevel.Navigate = ViewModel.GoToEditSettingsCommand;
                break;
        }
    }

    private void ClearFloorConrolsSelectedAdjustment(AdjustmentParametersDto adjustment)
    {
        if (FirstLevel.SelectedAdjustment is not null && FirstLevel.SelectedAdjustment == adjustment)
            FirstLevel.SelectedAdjustment = null;
        if (SecondLevel.SelectedAdjustment is not null && SecondLevel.SelectedAdjustment == adjustment)
            SecondLevel.SelectedAdjustment = null;
        if (ThirdLevel.SelectedAdjustment is not null && ThirdLevel.SelectedAdjustment == adjustment)
            ThirdLevel.SelectedAdjustment = null;
    }

    private void CollapseButtonsAndClearSelectedItem(object sender)
    {
        var button = sender as Button;
        if (button is null)
            return;

        var parent = VisualFinder.FindVisualParent<Grid>(button);
        if (parent is null)
            return;

        foreach (var child in parent.Children)
        {
            if (child is not StackPanel panel)
                continue;

            panel.Visibility = Visibility.Collapsed;
        }

        AdjustmentsList.SelectedItem = null;
    }
}