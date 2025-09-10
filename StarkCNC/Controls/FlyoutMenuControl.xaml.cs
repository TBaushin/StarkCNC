using StarkCNC.Models;
using StarkCNC.Services;
using System.Windows;
using System.Windows.Controls;

namespace StarkCNC.Controls;

/// <summary>
/// Interaction logic for FlyoutMenuControl.xaml
/// </summary>
public partial class FlyoutMenuControl : UserControl
{
    private INavigationService _navigationService;

    private bool _menuOpen = true;
    private ViewData? _selectedItem;

    public static readonly DependencyProperty PagesProperty = DependencyProperty.Register(
        nameof(Pages), 
        typeof(IEnumerable<ViewData>), 
        typeof(FlyoutMenuControl)
    );

    public IEnumerable<ViewData> Pages 
    { 
        get => (IEnumerable<ViewData>)GetValue(PagesProperty); 
        set => SetValue(PagesProperty, value); 
    }

    public event RoutedPropertyChangedEventHandler<object> SelectedItemChanged;

    public FlyoutMenuControl(INavigationService navigationService)
    {
        _navigationService = navigationService;

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

    /*private StackPanel GenerateOpenHeader()
    {
        var sp = new StackPanel();
        MenuButton = new Button() { Width = 253, Margin = new Thickness(18) };

        var buttonContent = new StackPanel() { Orientation = Orientation.Horizontal, Width = 230 };
        buttonContent.Children.Add(new TextBlock() 
            { 
                Margin = new Thickness(0, 4, 0, 0), 
                FontFamily = Application.Current.Resources["SymbolThemeFontFamily"] as FontFamily,
                FontSize = 15,
                Text = "\uE700"
            }
        );
        buttonContent.Children.Add(new TextBlock()
            {
                Margin = new Thickness(8, 0, 0, 0),
                Text = Localization.Language.MenuButton
            }
        );
        MenuButton.Content = buttonContent;

        sp.Children.Add(MenuButton);

        SearchBox = new TextBox() { Width = 250, Margin = new Thickness(10) };
        sp.Children.Add(SearchBox);

        Grid.SetRow(sp, 0);
        return sp;
    }*/

    /*private StackPanel GenerateClosedHeader()
    {
        var sp = new StackPanel();
        MenuButton = new Button()
        {
            Margin = new Thickness(18, 5, 18, 5),
            HorizontalAlignment = HorizontalAlignment.Left
        };

        var buttonContent = new StackPanel() { Orientation = Orientation.Horizontal };
        buttonContent.Children.Add(new TextBlock()
            {
                Margin = new Thickness(0, 4, 0, 0),
                FontFamily = Application.Current.Resources["SymbolThemeFontFamily"] as FontFamily,
                FontSize = 15,
                Text = "\uE700"
            }
        );
        MenuButton.Content = buttonContent;

        sp.Children.Add(MenuButton);

        SearchButton = new Button()
        {
            Margin = new Thickness(18, 5, 18, 5),
            HorizontalAlignment = HorizontalAlignment.Left
        };
        var searchButtonContent = new StackPanel() { Orientation = Orientation.Horizontal };
        searchButtonContent.Children.Add(
            new TextBlock()
            {
                Margin = new Thickness(0, 4, 0, 0),
                FontFamily = Application.Current.Resources["SymbolThemeFontFamily"] as FontFamily,
                FontSize = 15,
                Text = "\uE721"
            }
        );
        SearchButton.Content = searchButtonContent;
        sp.Children.Add(SearchButton);

        sp.Children.Add(new Separator());

        Grid.SetRow(sp, 0);
        return sp;
    }*/

    /*private ListView GenerateOpenBody()
    {
        PageList = new ListView() { Margin = new Thickness(8, 8, 0, 0) };
        PageList.SetBinding(ListView.ItemsSourceProperty, new Binding("Pages"));

        var itemsGridFactory = new FrameworkElementFactory(typeof(Grid));
        itemsGridFactory.SetValue(Grid.MinHeightProperty, 30.0);
        var col1 = new FrameworkElementFactory(typeof(ColumnDefinition));
        col1.SetValue(ColumnDefinition.WidthProperty, GridLength.Auto);
        var col2 = new FrameworkElementFactory(typeof(ColumnDefinition));
        col2.SetValue(ColumnDefinition.WidthProperty, new GridLength(16));
        var col3 = new FrameworkElementFactory(typeof(ColumnDefinition));
        col3.SetValue(ColumnDefinition.WidthProperty, new GridLength(1, GridUnitType.Star));
        itemsGridFactory.AppendChild(col1);
        itemsGridFactory.AppendChild(col2);
        itemsGridFactory.AppendChild(col3);

        var iconTb = new FrameworkElementFactory(typeof(TextBlock));
        iconTb.SetValue(TextBlock.MaxWidthProperty, 16.0);
        iconTb.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
        iconTb.SetValue(TextBlock.FocusableProperty, false);
        iconTb.SetBinding(TextBlock.FontFamilyProperty,
            new Binding() { Source = Application.Current.Resources["SymbolThemeFontFamily"] });
        iconTb.SetValue(TextBlock.FontSizeProperty, 16.0);
        iconTb.SetBinding(TextBlock.TextProperty, new Binding("IconGlyph"));
        iconTb.SetBinding(TextBlock.VisibilityProperty,
            new Binding("IconGlyph") { Converter = (IValueConverter)Resources["EmptyToVisibilityConverter"] });
        itemsGridFactory.AppendChild(iconTb);

        var titleTb = new FrameworkElementFactory(typeof(TextBlock));
        titleTb.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
        titleTb.SetBinding(TextBlock.TextProperty, new Binding("Title"));
        titleTb.SetValue(Grid.ColumnProperty, 2);
        itemsGridFactory.AppendChild(titleTb);

        var hdt = new HierarchicalDataTemplate();
        hdt.ItemsSource = new Binding("Items");
        hdt.VisualTree = itemsGridFactory;
        PageList.ItemTemplate = hdt;

        Grid.SetRow(PageList, 1);
        return PageList;
    }*/

    /*private ListView GenerateClosedBody()
    {
        PageList = new ListView() { Margin = new Thickness(8), HorizontalContentAlignment = HorizontalAlignment.Left };
        PageList.SetBinding(ListView.ItemsSourceProperty, new Binding("Pages"));

        var iconTb = new FrameworkElementFactory(typeof(TextBlock));
        iconTb.SetValue(TextBlock.MarginProperty, new Thickness(0, 4, 0, 0));
        iconTb.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
        iconTb.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Left);
        iconTb.SetBinding(TextBlock.FontFamilyProperty,
            new Binding() { Source = Application.Current.Resources["SymbolThemeFontFamily"] });
        iconTb.SetValue(TextBlock.FontSizeProperty, 15.0);
        iconTb.SetBinding(TextBlock.TextProperty, new Binding("IconGlyph"));
        iconTb.SetBinding(TextBlock.VisibilityProperty,
            new Binding("IconGlyph") { Converter = (IValueConverter)Resources["EmptyToVisibilityConverter"] });

        var hdt = new HierarchicalDataTemplate();
        hdt.ItemsSource = new Binding("Items");
        hdt.VisualTree = iconTb;
        PageList.ItemTemplate = hdt;

        Grid.SetRow(PageList, 1);
        return PageList;
    }*/

    /*private StackPanel GenerateOpenFooter()
    {
        SettingsButton = new Button()
        {
            Width = 40,
            Height = 40,
            Margin = new Thickness(10),
            HorizontalContentAlignment = HorizontalAlignment.Left
        };
        DockPanel.SetDock(SettingsButton, Dock.Bottom);
        SettingsButton.SetBinding(Button.CommandProperty, new Binding("GoSettingsCommand"));
        var spSettings = new StackPanel() { Orientation = Orientation.Horizontal };
        spSettings.Children.Add(
            new TextBlock()
            {
                Margin = new Thickness(0, 4, 0, 0),
                FontFamily = Application.Current.Resources["SymbolThemeFontFamily"] as FontFamily,
                FontSize = 15,
                Text = "\uE713"
            }
        );
        SettingsButton.Content = spSettings;

        UserButton = new Button()
        {
            Width = 210,
            Height = 40,
            Margin = new Thickness(10),
            HorizontalContentAlignment = HorizontalAlignment.Left
        };
        DockPanel.SetDock(UserButton, Dock.Bottom);
        UserButton.SetBinding(Button.CommandProperty, new Binding("GoUsersCommand"));
        var spUser = new StackPanel() { Orientation = Orientation.Horizontal };
        spUser.Children.Add(
            new TextBlock()
            {
                Margin = new Thickness(0, 4, 0, 0),
                FontFamily = Application.Current.Resources["SymbolThemeFontFamily"] as FontFamily,
                FontSize = 15,
                Text = "\uE77B"
            }
        );
        spUser.Children.Add(new TextBlock() { Margin = new Thickness(8, 0, 0, 0) });
        UserButton.Content = spUser;
        var resultSp = new StackPanel() { Orientation = Orientation.Horizontal};
        resultSp.Children.Add(SettingsButton);
        resultSp.Children.Add(UserButton);

        Grid.SetRow(resultSp, 2);
        return resultSp;
    }*/

    /*private StackPanel GenerateClosedFooter()
    {
        SettingsButton = new Button()
        {
            Margin = new Thickness(18, 5, 18, 5),
            HorizontalContentAlignment = HorizontalAlignment.Left
        };
        DockPanel.SetDock(SettingsButton, Dock.Bottom);
        SettingsButton.SetBinding(Button.CommandProperty, new Binding("GoSettingsCommand"));
        var spSettings = new StackPanel() { Orientation = Orientation.Horizontal };
        spSettings.Children.Add(
            new TextBlock()
            {
                Margin = new Thickness(0, 4, 0, 0),
                FontFamily = Application.Current.Resources["SymbolThemeFontFamily"] as FontFamily,
                FontSize = 15,
                Text = "\uE713"
            }
        );
        SettingsButton.Content = spSettings;

        UserButton = new Button()
        {
            Margin = new Thickness(18, 5, 18, 5),
            HorizontalAlignment = HorizontalAlignment.Left
        };
        DockPanel.SetDock(UserButton, Dock.Bottom);
        UserButton.SetBinding(Button.CommandProperty, new Binding("GoUsersCommand"));
        var spUser = new StackPanel() { Orientation = Orientation.Horizontal };
        spUser.Children.Add(
            new TextBlock()
            {
                Margin = new Thickness(0, 4, 0, 0),
                FontFamily = Application.Current.Resources["SymbolThemeFontFamily"] as FontFamily,
                FontSize = 15,
                Text = "\uE77B"
            }
        );
        UserButton.Content = spUser;

        var resultSp = new StackPanel() { Orientation = Orientation.Vertical };
        resultSp.Children.Add(new Separator());
        resultSp.Children.Add(UserButton);
        resultSp.Children.Add(SettingsButton);

        Grid.SetRow(resultSp, 2);
        return resultSp;
    }*/

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
        _navigationService.Navigate(navItem.Page);
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
        _navigationService.Navigate(navItem.Page);
    }

    private void MenuButton_Click(object sender, RoutedEventArgs e)
    {
        if (_menuOpen)
        {
            _menuOpen = false;
            FlyoutMenuOpened.Visibility = Visibility.Collapsed;
            FlyoutMenuClosed.Visibility = Visibility.Visible;
        }
        else
        {
            _menuOpen = true;
            FlyoutMenuOpened.Visibility = Visibility.Visible;
            FlyoutMenuClosed.Visibility = Visibility.Collapsed;
        }

        UpdateSelected(null);
    }
}
