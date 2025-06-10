using StarkCNC.Models;
using StarkCNC.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace StarkCNC.Controls
{
    /// <summary>
    /// Interaction logic for FlyoutMenuControl.xaml
    /// </summary>
    public partial class FlyoutMenuControl : UserControl
    {
        private INavigationService _navigationService;

        private bool _menuOpen = true;

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

        public Button MenuButton { get; set; }

        public Button SettingsButton { get; set; }

        public Button UserButton { get; set; }

        public TextBox SearchBox { get; set; }

        public Button SearchButton { get; set; }

        public TreeView PageList { get; set; }

        public FlyoutMenuControl(INavigationService navigationService)
        {
            _navigationService = navigationService;

            InitializeComponent();

            GenerateOpenPage();

            PageList.SelectedItemChanged += PageList_SelectedItemChanged;
            MenuButton.Click += MenuButton_Click;
        }

        public void UpdateSelected(ViewData? viewData)
        {
            var page = viewData;
            if (page is null)
            {
                foreach (var item in PageList.Items)
                {
                    TreeViewItem treeViewItem = PageList.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;
                    if (treeViewItem is null)
                        continue;

                    treeViewItem.IsSelected = false;
                }

                return;
            }

            var treeViewItemFromPage = PageList.ItemContainerGenerator.ContainerFromItem(page) as TreeViewItem;
            if (treeViewItemFromPage is null)
                return;

            treeViewItemFromPage.IsSelected = true;
        }

        private void GenerateOpenPage()
        {
            FlyoutMenu.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            FlyoutMenu.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            FlyoutMenu.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            FlyoutMenu.Children.Add(GenerateOpenHeader());
            FlyoutMenu.Children.Add(GenerateOpenBody());
            FlyoutMenu.Children.Add(GenerateOpenFooter());
        }

        private void GenerateClosedPage()
        {
            FlyoutMenu.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            FlyoutMenu.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            FlyoutMenu.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            FlyoutMenu.Children.Add(GenerateClosedHeader());
            FlyoutMenu.Children.Add(GenerateClosedBody());
            FlyoutMenu.Children.Add(GenerateClosedFooter());
        }

        private StackPanel GenerateOpenHeader()
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
        }

        private StackPanel GenerateClosedHeader()
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
        }

        private TreeView GenerateOpenBody()
        {
            PageList = new TreeView() { Margin = new Thickness(8, 8, 0, 0) };
            PageList.SetBinding(TreeView.ItemsSourceProperty, new Binding("Pages"));

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
            iconTb.SetBinding(TextBlock.FontFamilyProperty, new Binding() { Source = Application.Current.Resources["SymbolThemeFontFamily"] });
            iconTb.SetValue(TextBlock.FontSizeProperty, 16.0);
            iconTb.SetBinding(TextBlock.TextProperty, new Binding("IconGlyph"));
            iconTb.SetBinding(TextBlock.VisibilityProperty, new Binding("IconGlyph") { Converter = (IValueConverter)Resources["EmptyToVisibilityConverter"] });
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
        }

        private TreeView GenerateClosedBody()
        {
            PageList = new TreeView() { Margin = new Thickness(8), HorizontalContentAlignment = HorizontalAlignment.Left };
            PageList.SetBinding(TreeView.ItemsSourceProperty, new Binding("Pages"));

            var iconTb = new FrameworkElementFactory(typeof(TextBlock));
            iconTb.SetValue(TextBlock.MarginProperty, new Thickness(0, 4, 0, 0));
            iconTb.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
            iconTb.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Left);
            iconTb.SetBinding(TextBlock.FontFamilyProperty, new Binding() { Source = Application.Current.Resources["SymbolThemeFontFamily"] });
            iconTb.SetValue(TextBlock.FontSizeProperty, 15.0);
            iconTb.SetBinding(TextBlock.TextProperty, new Binding("IconGlyph"));
            iconTb.SetBinding(TextBlock.VisibilityProperty, new Binding("IconGlyph") { Converter = (IValueConverter)Resources["EmptyToVisibilityConverter"] });

            var hdt = new HierarchicalDataTemplate();
            hdt.ItemsSource = new Binding("Items");
            hdt.VisualTree = iconTb;
            PageList.ItemTemplate = hdt;

            Grid.SetRow(PageList, 1);
            return PageList;
        }

        private StackPanel GenerateOpenFooter()
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
        }

        private StackPanel GenerateClosedFooter()
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
        }

        private void PageList_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var navItem = PageList.SelectedItem as ViewData;
            if (navItem is null)
                return;

            _navigationService.Navigate(navItem.Page);
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            if (_menuOpen)
            {
                _menuOpen = false;
                FlyoutMenu.Children.Clear();
                FlyoutMenu.RowDefinitions.Clear();
                GenerateClosedPage();

                MenuButton.Click += MenuButton_Click;
                PageList.SelectedItemChanged += PageList_SelectedItemChanged;
            }
            else
            {
                _menuOpen = true;
                FlyoutMenu.Children.Clear();
                FlyoutMenu.RowDefinitions.Clear();
                GenerateOpenPage();

                MenuButton.Click += MenuButton_Click;
                PageList.SelectedItemChanged += PageList_SelectedItemChanged;
            }
        }
    }
}
