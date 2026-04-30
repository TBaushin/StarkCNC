using StarkCNC.Models;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace StarkCNC.Controls;

/// <summary>
/// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
///
/// Step 1a) Using this custom control in a XAML file that exists in the current project.
/// Add this XmlNamespace attribute to the root element of the markup file where it is 
/// to be used:
///
///     xmlns:MyNamespace="clr-namespace:StarkCNC.Controls"
///
///
/// Step 1b) Using this custom control in a XAML file that exists in a different project.
/// Add this XmlNamespace attribute to the root element of the markup file where it is 
/// to be used:
///
///     xmlns:MyNamespace="clr-namespace:StarkCNC.Controls;assembly=StarkCNC.Controls"
///
/// You will also need to add a project reference from the project where the XAML file lives
/// to this project and Rebuild to avoid compilation errors:
///
///     Right click on the target project in the Solution Explorer and
///     "Add Reference"->"Projects"->[Browse to and select this project]
///
///
/// Step 2)
/// Go ahead and use your control in the XAML file.
///
///     <MyNamespace:FlyoutMenu/>
///
/// </summary>
public class FlyoutMenu : Control
{
    public static readonly DependencyProperty MenuIsOpenProperty = DependencyProperty
        .Register(nameof(MenuIsOpen), typeof(bool), typeof(FlyoutMenu), new PropertyMetadata(true));
    public static readonly DependencyProperty SelectedItemProperty = DependencyProperty
        .Register(nameof(SelectedItem), typeof(ViewData), typeof(FlyoutMenu), new PropertyMetadata(null, SelectedItemChanged));
    public static readonly DependencyProperty PagesProperty = DependencyProperty
        .Register(nameof(Pages), typeof(IEnumerable<ViewData>), typeof(FlyoutMenu), new PropertyMetadata());
    public static readonly DependencyProperty CurrentUsernameProperty = DependencyProperty
        .Register(nameof(CurrentUsername), typeof(string), typeof(FlyoutMenu), new PropertyMetadata());
    public static readonly DependencyProperty NavigateToSettingsPageProprty = DependencyProperty
        .Register(nameof(NavigateToSettingsPage), typeof(ICommand), typeof(FlyoutMenu), new PropertyMetadata());
    public static readonly DependencyProperty CallUserWindowProperty = DependencyProperty
        .Register(nameof(CallUserWindow), typeof(ICommand), typeof(FlyoutMenu), new PropertyMetadata());

    private static void SelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not FlyoutMenu menu)
            return;

        menu.FlyoutMenu_SelectedItemChanged(e.NewValue, new PropertyChangedEventArgs(nameof(menu.SelectedItem)));
    }

    static FlyoutMenu()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(FlyoutMenu), new FrameworkPropertyMetadata(typeof(FlyoutMenu)));
    }

    public bool MenuIsOpen
    {
        get => (bool)GetValue(MenuIsOpenProperty);
        set => SetValue(MenuIsOpenProperty, value);
    }

    public ViewData? SelectedItem
    {
        get => (ViewData?)GetValue(SelectedItemProperty);
        set
        {
            SetValue(SelectedItemProperty, value);
            SelectedItemChangedHandler?.Invoke(value, new PropertyChangedEventArgs(nameof(SelectedItem)));
        }
    }

    public IEnumerable<ViewData> Pages
    {
        get => (IEnumerable<ViewData>)GetValue(PagesProperty);
        set => SetValue(PagesProperty, value);
    }

    public string CurrentUsername
    {
        get => (string)GetValue(CurrentUsernameProperty);
        set => SetValue(CurrentUsernameProperty, value);
    }

    public ICommand NavigateToSettingsPage
    {
        get => (ICommand)GetValue(NavigateToSettingsPageProprty);
        set => SetValue(NavigateToSettingsPageProprty, value);
    }

    public ICommand CallUserWindow
    {
        get => (ICommand)(GetValue(CallUserWindowProperty));
        set => SetValue(CallUserWindowProperty, value);
    }

    private event PropertyChangedEventHandler? SelectedItemChangedHandler;

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        SelectedItemChangedHandler += FlyoutMenu_SelectedItemChanged;

        if (GetTemplateChild("MenuButton") is Button btn)
            btn.Click += Btn_Click;

        if (GetTemplateChild("PagesList") is ListView lv)
            lv.SelectionChanged += Lv_SelectionChanged;

        if (GetTemplateChild("PagesTree") is TreeView tv)
        {
            tv.SelectedItemChanged += Tv_SelectedItemChanged;
            HookTreeView(tv);
        }
    }

    private async void FlyoutMenu_SelectedItemChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is ViewData vd)
            await ICommandControl.ExecuteCommand(vd.NavigationCommand).ConfigureAwait(true);
        else
            ClearSelection();
    }

    private void Btn_Click(object sender, RoutedEventArgs e)
    {
        MenuIsOpen = !MenuIsOpen;
    }

    public async void UpdateSelected(ViewData? viewData)
    {
        var tv = GetTemplateChild("PagesTree") as TreeView;
        var lv = GetTemplateChild("PagesList") as ListView;
        if (tv is null || lv is null)
            return;

        if (viewData is null)
        {
            foreach (var item in tv.Items)
            {
                TreeViewItem? tvItem = tv.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;

                if (tvItem is null)
                    continue;

                tvItem.IsSelected = false;
            }

            foreach (var item in tv.Items)
            {
                ListViewItem? lvItem = lv.ItemContainerGenerator.ContainerFromItem(item) as ListViewItem;

                if (lvItem is null)
                    continue;

                lvItem.IsSelected = false;
            }

            SelectedItem = null;
        }
        else
        {
            SelectedItem = viewData;
        }

        SetSelectedForPage(SelectedItem);
    }

    private void SetSelectedForPage(ViewData? data)
    {
        if (data is null)
            return;

        var tv = GetTemplateChild("PagesTree") as TreeView;
        var lv = GetTemplateChild("PagesList") as ListView;
        if (tv is null || lv is null)
            return;

        var tvItemFromPage = tv.ItemContainerGenerator.ContainerFromItem(data) as TreeViewItem;
        var lvItemFromPage = lv.ItemContainerGenerator.ContainerFromItem(data) as ListViewItem;

        if (tvItemFromPage is not null)
            tvItemFromPage.IsSelected = true;


        if (lvItemFromPage is not null)
            lvItemFromPage.IsSelected = true;
    }

    private void HookTreeView(TreeView tv)
    {
        tv.ItemContainerGenerator.StatusChanged += (s, e) =>
        {
            if (tv.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
                ApplySelection(tv);
        };
    }

    private void ApplySelection(TreeView tv)
    {
        if (SelectedItem is null)
            return;

        var tvItemFromPage = tv.ItemContainerGenerator.ContainerFromItem(SelectedItem) as TreeViewItem;

        if (tvItemFromPage is not null)
            tvItemFromPage.IsSelected = true;
    }

    private void ClearSelection()
    {
        var tv = GetTemplateChild("PagesTree") as TreeView;
        var lv = GetTemplateChild("PagesList") as ListView;
        if (tv is null || lv is null)
            return;

        if (SelectedItem is null)
        {
            foreach (var item in tv.Items)
            {
                TreeViewItem? tvItem = tv.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;

                if (tvItem is null)
                    continue;

                tvItem.IsSelected = false;
            }

            foreach (var item in tv.Items)
            {
                ListViewItem? lvItem = lv.ItemContainerGenerator.ContainerFromItem(item) as ListViewItem;

                if (lvItem is null)
                    continue;

                lvItem.IsSelected = false;
            }
        }
    }

    private async void Tv_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
        if (e.NewValue is bool selected && selected != true)
            return;

        var pageList = sender as TreeView;
        if (pageList is null)
            return;

        var navItem = pageList.SelectedItem as ViewData;
        if (navItem is null)
            return;

        SelectedItem = navItem;
        SetSelectedForPage(SelectedItem);
    }

    private async void Lv_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.AddedItems.Count < 1)
            return;

        var pageList = sender as ListView;
        if (pageList is null)
            return;

        var navItem = pageList.SelectedItem as ViewData;
        if (navItem is null)
            return;

        SelectedItem = navItem;
        SetSelectedForPage(SelectedItem);
    }
}
