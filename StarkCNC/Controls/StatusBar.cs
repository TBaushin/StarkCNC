using StarkCNC.Core.Models;
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
    public static readonly DependencyProperty StatusesCountProperty = DependencyProperty
        .Register(nameof(StatusesCount), typeof(int), typeof(StatusBar), new PropertyMetadata());
    public static readonly DependencyProperty CurrentStatusIndexProperty = DependencyProperty
        .Register(nameof(CurrentStatusIndex), typeof(int), typeof(StatusBar), new PropertyMetadata());
    public static readonly DependencyProperty StatusesProperty = DependencyProperty
        .Register(nameof(Statuses), typeof(IEnumerable<Status>), typeof(StatusBar), new PropertyMetadata());
    public static readonly DependencyProperty CurrentStatusProperty = DependencyProperty
        .Register(nameof(CurrentStatus), typeof(Status), typeof(StatusBar), new PropertyMetadata());
    public static readonly DependencyProperty CurrentTimeProperty = DependencyProperty
        .Register(nameof(CurrentTime), typeof(string), typeof(StatusBar), new PropertyMetadata());
    public static readonly DependencyProperty ShowHistoryProperty = DependencyProperty
        .Register(nameof(ShowHistory), typeof(bool), typeof(StatusBar), new PropertyMetadata());

    static StatusBar()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(StatusBar), new FrameworkPropertyMetadata(typeof(StatusBar)));
    }

    private DispatcherTimer? _statusesTimer;
    private DispatcherTimer? _timeTimer;

    public int StatusesCount
    {
        get => (int)GetValue(StatusesCountProperty);
        set => SetValue(StatusesCountProperty, value);
    }

    public int CurrentStatusIndex
    {
        get => (int)GetValue(CurrentStatusIndexProperty);
        set => SetValue(CurrentStatusIndexProperty, value);
    }

    public IEnumerable<Status> Statuses
    {
        get => (IEnumerable<Status>)GetValue(StatusesProperty);
        set
        {
            SetValue(StatusesProperty, value);

            if (_statusesTimer is not null)
            {
                _statusesTimer.Stop();
                _statusesTimer.Interval = GetTimeForShowStatus();
                _statusesTimer.Start();
            }
        }
    }

    public Status CurrentStatus
    {
        get => (Status)GetValue(CurrentStatusProperty);
        set => SetValue(CurrentStatusProperty, value);
    }

    public string CurrentTime
    {
        get => (string)GetValue(CurrentTimeProperty);
        set => SetValue(CurrentTimeProperty, value);
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

        _statusesTimer = GetCurrentStatusUpdater();
        _statusesTimer.Start();

        _timeTimer = GetTimeUpdater();
        _timeTimer.Start();
    }

    private DispatcherTimer GetCurrentStatusUpdater()
    {
        return new DispatcherTimer(
            GetTimeForShowStatus(),
            DispatcherPriority.Background,
            (_, _) =>
            {
                var statusesList = Statuses.ToList();
                StatusesCount = statusesList.Count;
                if (!statusesList.Any())
                    return;

                if (CurrentStatusIndex >= statusesList.Count)
                    CurrentStatusIndex = 0;

                CurrentStatus = statusesList[CurrentStatusIndex];
                StatusesCount = statusesList.Count;
                CurrentStatusIndex++;
            },
            Dispatcher);
    }

    private TimeSpan GetTimeForShowStatus()
    {
        if (Statuses.Any())
        {
            return TimeSpan.FromSeconds(Statuses.Count() * 1.5);
        }
        else
        {
            return TimeSpan.FromSeconds(0.5);
        }
    }

    private DispatcherTimer GetTimeUpdater()
    {
        return new DispatcherTimer(
            TimeSpan.FromSeconds(1),
            DispatcherPriority.Background,
            (_, _) => CurrentTime = DateTime.Now.ToString("HH:mm", CultureInfo.InvariantCulture),
            Dispatcher);
    }
}
