using System.Diagnostics;
using System.Windows.Threading;

namespace StarkCNC.Exceptions;

public class GlobalExceptionHandler
{
    private static GlobalExceptionHandler? _instance;
    
    public Dispatcher CurrentDispatcher { get; private set; }

    private GlobalExceptionHandler()
    {
        CurrentDispatcher = Dispatcher.CurrentDispatcher;
    }

    private static GlobalExceptionHandler GetInstance()
    {
        if (_instance is null)
            _instance = new GlobalExceptionHandler();
        return _instance;
    }

    public static void StartHandling()
    {
        var instance = GetInstance();
        instance.CurrentDispatcher.UnhandledException += Dispatcher_UnhandledException;
        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
    }

    public static void StopHandling()
    {
        var instance = GetInstance();
        instance.CurrentDispatcher.UnhandledException -= Dispatcher_UnhandledException;
        AppDomain.CurrentDomain.UnhandledException -= CurrentDomain_UnhandledException;
    }

    private static void Dispatcher_UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        e.Handled = true;
    }

    private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        var exception = e.ExceptionObject as Exception;
        Debug.WriteLine($"Exception Happend, {exception?.Message}");
    }
}
