using StarkCNC.Models;
using System.Windows;
using System.Windows.Controls;

namespace StarkCNC.Controls
{
    /// <summary>
    /// Interaction logic for FlyoutMenuControl.xaml
    /// </summary>
    public partial class FlyoutMenuControl : UserControl
    {
        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(
            nameof(Items), 
            typeof(IEnumerable<ViewData>), 
            typeof(FlyoutMenuControl)
        );

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
            nameof(SelectedItem),
            typeof(object),
            typeof(FlyoutMenuControl)
        );

        public IEnumerable<ViewData> Items 
        { 
            get => (IEnumerable<ViewData>)GetValue(ItemsProperty); 
            set => SetValue(ItemsProperty, value); 
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

        public FlyoutMenuControl()
        {
            InitializeComponent();

            PageList.SelectedItemChanged += PageList_SelectedItemChanged;
        }

        public object GetContainerFromItem(object item)
        {
            return PageList.ItemContainerGenerator.ContainerFromItem(item);
        }

        private void PageList_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SelectedItem = e.NewValue;
        }
    }
}
