using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace StarkCNC.Controls;

/// <summary>
/// Выполните шаги 1a или 1b, а затем 2, чтобы использовать этот пользовательский элемент управления в файле XAML.
///
/// Шаг 1a. Использование пользовательского элемента управления в файле XAML, существующем в текущем проекте.
/// Добавьте атрибут XmlNamespace в корневой элемент файла разметки, где он 
/// будет использоваться:
///
///     xmlns:MyNamespace="clr-namespace:StarkCNC.Controls"
///
///
/// Шаг 1б. Использование пользовательского элемента управления в файле XAML, существующем в другом проекте.
/// Добавьте атрибут XmlNamespace в корневой элемент файла разметки, где он 
/// будет использоваться:
///
///     xmlns:MyNamespace="clr-namespace:StarkCNC.Controls;assembly=StarkCNC.Controls"
///
/// Потребуется также добавить ссылку из проекта, в котором находится файл XAML,
/// на данный проект и пересобрать во избежание ошибок компиляции:
///
///     Щелкните правой кнопкой мыши нужный проект в обозревателе решений и выберите
///     "Добавить ссылку"->"Проекты"->[Поиск и выбор проекта]
///
///
/// Шаг 2)
/// Теперь можно использовать элемент управления в файле XAML.
///
///     <MyNamespace:StatusBar/>
///
/// </summary>
public class StatusBar : Control
{
    public static readonly DependencyProperty ErrorsCountProperty = DependencyProperty
        .Register(nameof(ErrorsCount), typeof(int), typeof(StatusBar), new PropertyMetadata());
    public static readonly DependencyProperty StatusProperty = DependencyProperty
        .Register(nameof(Status), typeof(string), typeof(StatusBar), new PropertyMetadata());
    public static readonly DependencyProperty ShowHistoryProperty = DependencyProperty
        .Register(nameof(ShowHistory), typeof(bool), typeof(StatusBar), new PropertyMetadata());

    static StatusBar()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(StatusBar), new FrameworkPropertyMetadata(typeof(StatusBar)));
    }

    public int ErrorsCount
    {
        get => (int)GetValue(ErrorsCountProperty);
        set => SetValue(ErrorsCountProperty, value);
    }

    public string Status
    {
        get => (string)GetValue(StatusProperty);
        set => SetValue(StatusProperty, value);
    }

    public bool ShowHistory
    {
        get => (bool)GetValue(ShowHistoryProperty);
        set => SetValue(ShowHistoryProperty, value);
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        var showHistory = GetTemplateChild("HistoryButton") as Button;
        if (showHistory is not null)
            showHistory.Click += (sender, args) => ShowHistory = !ShowHistory;

        var currentTime = GetTemplateChild("CurrentTimeLabel") as Label;
        if (currentTime is not null)
        {
            DispatcherTimer timer = new DispatcherTimer(
                TimeSpan.FromSeconds(1),
                DispatcherPriority.Background,
                (_, _) => currentTime.Content = DateTime.Now.ToString("HH:mm", CultureInfo.InvariantCulture),
                Dispatcher);
            timer.Start();
        }
    }
}
