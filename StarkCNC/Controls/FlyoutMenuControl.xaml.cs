using StarkCNC.Models;
using System.Windows;
using System.Windows.Controls;

namespace StarkCNC.Controls;

/// <summary>
/// Interaction logic for FlyoutMenuControl.xaml
/// </summary>
public partial class FlyoutMenuControl : UserControl
{
    private ViewData? _selectedItem;

    public static readonly DependencyProperty PagesProperty = DependencyProperty.Register(
        nameof(Pages), 
        typeof(IEnumerable<ViewData>), 
        typeof(FlyoutMenuControl)
    );

    public static readonly DependencyProperty MenuIsOpenProperty = DependencyProperty
        .Register(nameof(MenuIsOpen), typeof(bool), typeof(FlyoutMenuControl));

    public IEnumerable<ViewData> Pages 
    { 
        get => (IEnumerable<ViewData>)GetValue(PagesProperty); 
        set => SetValue(PagesProperty, value); 
    }

    public bool MenuIsOpen
    {
        get => (bool)GetValue(MenuIsOpenProperty);
        set
        {
            SetValue(MenuIsOpenProperty, value);
        }
    }

    public event RoutedPropertyChangedEventHandler<object>? SelectedItemChanged;

    public FlyoutMenuControl()
    {
        InitializeComponent();
    }

    public void UpdateSelected(ViewData? viewData)
    {
        if (viewData is null)
        {
            foreach (var item in PagesTreeView.Items)
            {
                TreeViewItem? tvItem = PagesTreeView.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;

                if (tvItem is null)
                    continue;

                tvItem.IsSelected = false;
            }

            foreach (var item in PagesTreeView.Items)
            {
                ListViewItem? lvItem = PagesListView.ItemContainerGenerator.ContainerFromItem(item) as ListViewItem;

                if (lvItem is null)
                    continue;

                lvItem.IsSelected = false;
            }

            _selectedItem = null;
        }
        else
        {
            _selectedItem = viewData;
        }

        SetSelectedForPage(_selectedItem);
    }

    private void SetSelectedForPage(ViewData? data)
    {
        if (data is null)
            return;
        
        var tvItemFromPage = PagesTreeView.ItemContainerGenerator.ContainerFromItem(data) as TreeViewItem;
        var lvItemFromPage = PagesListView.ItemContainerGenerator.ContainerFromItem(data) as ListViewItem;

        if (tvItemFromPage is not null)
            tvItemFromPage.IsSelected = true;


        if (lvItemFromPage is not null)
            lvItemFromPage.IsSelected = true;
    }

    private void PageList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.AddedItems.Count < 1)
            return;

        var pageList = sender as ListView;
        if (pageList is null)
            return;

        var navItem = pageList.SelectedItem as ViewData;
        if (navItem is null)
            return;

        _selectedItem = navItem;
        
        if (!MenuIsOpen)
            _selectedItem.NavigationCommand.Execute(null);
    }

    private void PageList_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
        if (e.NewValue is bool selected && selected != true)
            return;

        var pageList = sender as TreeView;
        if (pageList is null)
            return;

        var navItem = pageList.SelectedItem as ViewData;
        if (navItem is null)
            return;

        _selectedItem = navItem;

        if (MenuIsOpen)
            _selectedItem.NavigationCommand.Execute(null);
    }

    private void MenuButton_Click(object sender, RoutedEventArgs e)
    {
        if (MenuIsOpen)
            MenuIsOpen = false;
        else
            MenuIsOpen = true;

        UpdateSelected(null);
    }
}