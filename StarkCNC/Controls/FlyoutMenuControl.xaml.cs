using StarkCNC.Models;
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
        public static readonly DependencyProperty PagesProperty = DependencyProperty.Register(
            nameof(Pages), 
            typeof(IEnumerable<ViewData>), 
            typeof(FlyoutMenuControl)
        );

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
            nameof(SelectedItem),
            typeof(object),
            typeof(FlyoutMenuControl)
        );

        public IEnumerable<ViewData> Pages 
        { 
            get => (IEnumerable<ViewData>)GetValue(PagesProperty); 
            set => SetValue(PagesProperty, value); 
        }

        public object SelectedItem
        {
            get
            {
                return (object)GetValue(SelectedItemProperty);
            }
            set
            {
                SelectedItemChanged.Invoke(this, new RoutedPropertyChangedEventArgs<object>(SelectedItem, value));
                SetValue(SelectedItemProperty, value);
            }
        }

        public event RoutedPropertyChangedEventHandler<object> SelectedItemChanged;

        public Button MenuButton { get; set; }

        public Button SettingsButton { get; set; }

        public Button UserButton { get; set; }

        public TextBox SearchBox { get; set; }

        public TreeView PageList { get; set; }

        public FlyoutMenuControl()
        {
            InitializeComponent();

            GenerateOpenPage();

            PageList.SelectedItemChanged += PageList_SelectedItemChanged;
        }

        public object GetContainerFromItem(object item)
        {
            return PageList.ItemContainerGenerator.ContainerFromItem(item);
        }

        private void GenerateOpenPage()
        {
            var gridMenu = FlyoutMenu as Grid;
            if (gridMenu is null)
                return;
            gridMenu.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            gridMenu.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            gridMenu.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            gridMenu.Children.Add(GenerateOpenHeader());
            gridMenu.Children.Add(GenerateOpenBody());
            gridMenu.Children.Add(GenerateOpenFooter());
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
            });
            buttonContent.Children.Add(new TextBlock()
            {
                Margin = new Thickness(8, 0, 0, 0),
                Text = Localization.Language.MenuButton
            });
            MenuButton.Content = buttonContent;

            sp.Children.Add(MenuButton);

            SearchBox = new TextBox() { Width = 250, Margin = new Thickness(10) };
            sp.Children.Add(SearchBox);

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

        private StackPanel GenerateOpenFooter()
        {
            SettingsButton = new Button()
            {
                Width = 40,
                Height = 40,
                Margin = new Thickness(10),
                HorizontalContentAlignment = HorizontalAlignment.Left,
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
                HorizontalContentAlignment = HorizontalAlignment.Left,
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

        private void PageList_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SelectedItem = e.NewValue;
        }
    }
}
