using StarkCNC.Core.Models;
using StarkCNC.Helpers;
using StarkCNC.ViewModels;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StarkCNC.Views;

/// <summary>
/// Interaction logic for AdjustmentView.xaml
/// </summary>
public partial class AdjustmentView : Page
{
    private AdjustmentTypeToStringConverter _converter = new AdjustmentTypeToStringConverter();
    private AdjustmentViewModel ViewModel;
    private int _levelMustBeSetted;

    public AdjustmentView(AdjustmentViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = ViewModel;

        InitializeComponent();

        InitializeLevels();
    }

    private void InitializeLevels()
    {
        var thirdLevelAdjustment = ViewModel.SetUpAdjustments
            .FirstOrDefault(e => e.InstalledLevel == 3);
        ThirdLevel.AdjustmentName = GenerateAdjustmentName(thirdLevelAdjustment);
        ThirdLevel.Navigate = ViewModel.GoToEditSettingsCommand;

        var secondLevelAdjustment = ViewModel.SetUpAdjustments
            .FirstOrDefault(e => e.InstalledLevel == 2);
        SecondLevel.AdjustmentName = GenerateAdjustmentName(secondLevelAdjustment);
        SecondLevel.Navigate = ViewModel.GoToEditSettingsCommand;

        var firstLevelAdjustment = ViewModel.SetUpAdjustments
            .FirstOrDefault(e => e.InstalledLevel == 1);
        FirstLevel.AdjustmentName = GenerateAdjustmentName(firstLevelAdjustment);
        FirstLevel.Navigate = ViewModel.GoToEditSettingsCommand;
    }

    private string GenerateAdjustmentName(AdjustmentParameters? adjustment) =>
        adjustment is null
        ? string.Empty
        : $"{adjustment.Name} {_converter.Convert(adjustment.Type, typeof(AdjustmentType), new { }, CultureInfo.CurrentCulture)} R{adjustment.Radius}";

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

        //ClearFloorConrolsSelectedAdjustment(adjustment);
        //switch (level)
        //{
        //    case 1:
        //        FirstLevel.SelectedAdjustment = adjustment;
        //        FirstLevel.Navigate = ViewModel.GoToEditSettingsCommand;
        //        break;
        //    case 2:
        //        SecondLevel.SelectedAdjustment = adjustment;
        //        SecondLevel.Navigate = ViewModel.GoToEditSettingsCommand;
        //        break;
        //    case 3:
        //        ThirdLevel.SelectedAdjustment = adjustment;
        //        ThirdLevel.Navigate = ViewModel.GoToEditSettingsCommand;
        //        break;
        //}
    }

    private void ClearFloorConrolsSelectedAdjustment(AdjustmentParameters adjustment)
    {
        var adjustmentName = GenerateAdjustmentName(adjustment);
        if (FirstLevel.AdjustmentName is not null && FirstLevel.AdjustmentName == adjustmentName)
            FirstLevel.AdjustmentName = string.Empty;
        if (SecondLevel.AdjustmentName is not null && SecondLevel.AdjustmentName == adjustmentName)
            SecondLevel.AdjustmentName = string.Empty;
        if (ThirdLevel.AdjustmentName is not null && ThirdLevel.AdjustmentName == adjustmentName)
            ThirdLevel.AdjustmentName = string.Empty;
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