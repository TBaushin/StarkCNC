using StarkCNC.Core.Services;
using StarkCNC.MachineCommunication.Services;
using StarkCNC.Utilities;

namespace StarkCNC.Services;

public class ErrorsService : IErrorsService, IDisposable
{
    private IManualConfigurationService _manualService;
    private IStatusService _statusService;
    private bool _disposed;

    public ErrorsService(IManualConfigurationService manualService, IStatusService statusService)
    {
        _manualService = manualService;
        _statusService = statusService;
    }

    public void Subscribe()
    {
        _manualService.Subscribe<bool>(ControllerRequestStrings.ERRORS_EMERGENCY_STOP, value => SetStatusIfFalse(value, "Аварийный стоп"));
        _manualService.Subscribe<bool>(ControllerRequestStrings.ERRORS_CYCLE_START, value => SetStatusIfTrue(value, "Нажмите кнопку ПУСК для запуска цикла"));
        _manualService.Subscribe<bool>(ControllerRequestStrings.ERRORS_POSSIBLE_COLLISION, value => SetStatusIfTrue(value, "Возможно столкновение"));
        _manualService.Subscribe<bool>(ControllerRequestStrings.ERRORS_BEND_TO_ZERO, value => SetStatusIfTrue(value, "Верните гиб в нулевое положение"));
        _manualService.Subscribe<bool>(ControllerRequestStrings.ERRORS_SUPPORT_SENSOR_ERROR, value => SetStatusIfTrue(value, "Ошибка датчика поддержки"));
        _manualService.Subscribe<bool>(ControllerRequestStrings.ERRORS_PRESS_SENSOR_ERROR, value => SetStatusIfTrue(value, "Ошибка датчика прижима"));
        _manualService.Subscribe<bool>(ControllerRequestStrings.ERRORS_CLAMP_SENSOR_ERROR, value => SetStatusIfTrue(value, "Ошибка датчика зажима"));
        _manualService.Subscribe<bool>(ControllerRequestStrings.ERRORS_COLLET_SENSOR_ERROR, value => SetStatusIfTrue(value, "Ошибка датчиков цанги"));
        _manualService.Subscribe<bool>(ControllerRequestStrings.ERRORS_DORN_SENSOR_ERROR, value => SetStatusIfTrue(value, "Ошибка датчиков дорна"));
        _manualService.Subscribe<bool>(ControllerRequestStrings.ERRORS_PENULTIMATE_BEND_NOT_POSIBLE, value => SetStatusIfTrue(value, "Предпоследний гиб невозможен из-за возможного врезания в прижим"));
        _manualService.Subscribe<bool>(ControllerRequestStrings.ERRORS_MISSING_ROLLING_ADJUSTMENT, value => SetStatusIfTrue(value, "Отсутствует прокатная оснастка"));
        _manualService.Subscribe<bool>(ControllerRequestStrings.ERRORS_NO_PIPE_IN_COLLET, value => SetStatusIfTrue(value, "Установите трубу в цанговый зажим"));
        _manualService.Subscribe<bool>(ControllerRequestStrings.ERRORS_UNITS_NOT_IN_INITIALS_POSITIONS, value => SetStatusIfTrue(value, "Узлы не в исходном Нажмите кнопку СТОП для возврата"));
        _manualService.Subscribe<bool>(ControllerRequestStrings.ERRORS_PIPE_OFFSET, value => SetStatusIfTrue(value, "Нажмите кнопку СТОП для разжима цанги и съёма трубы"));
        _manualService.Subscribe<bool>(ControllerRequestStrings.ERRORS_DRAWING_NOT_POSSIBLE, value => SetStatusIfTrue(value, "Протяжка невозможна из-за возможного врезания в прижим"));
        _manualService.Subscribe<bool>(ControllerRequestStrings.ERRORS_SUPPLY_IN_FORWARD_DANGER_ZONE, value => SetStatusIfTrue(value, "Подача в передней опасной зоне"));
        _manualService.Subscribe<bool>(ControllerRequestStrings.ERRORS_HYDRAULIC_SAFETY_OFF, value => SetStatusIfFalse(value, "Автомат защиты гидравлики выключен"));
        _manualService.Subscribe<bool>(ControllerRequestStrings.ERRORS_BEND_RETURN, value => SetStatusIfTrue(value, "Нажмите кнопку СТОП для возврата гиба"));
        _manualService.Subscribe<bool>(ControllerRequestStrings.ERRORS_SUPPLY_DRIVE_ERROR, value => SetStatusIfTrue(value, "Ошибка привода подачи"));
        _manualService.Subscribe<bool>(ControllerRequestStrings.ERRORS_CONSOLE_DRIVE_ERROR, value => SetStatusIfTrue(value, "Ошибка привода консоли"));
        _manualService.Subscribe<bool>(ControllerRequestStrings.ERRORS_ROTATION_DRIVE_ERROR, value => SetStatusIfTrue(value, "Ошибка привода поворота"));
        _manualService.Subscribe<bool>(ControllerRequestStrings.ERRORS_CONFIRMATION_TUBE_INSTALLED, value => SetStatusIfTrue(value, "Подтвердите что труба установлена во вкладыш перед началом зажима"));
    }

    public void Unsubscribe()
    {
        _manualService.Unsubscribe(ControllerRequestStrings.ERRORS_EMERGENCY_STOP);
        _manualService.Unsubscribe(ControllerRequestStrings.ERRORS_CYCLE_START);
        _manualService.Unsubscribe(ControllerRequestStrings.ERRORS_POSSIBLE_COLLISION);
        _manualService.Unsubscribe(ControllerRequestStrings.ERRORS_BEND_TO_ZERO);
        _manualService.Unsubscribe(ControllerRequestStrings.ERRORS_SUPPORT_SENSOR_ERROR);
        _manualService.Unsubscribe(ControllerRequestStrings.ERRORS_PRESS_SENSOR_ERROR);
        _manualService.Unsubscribe(ControllerRequestStrings.ERRORS_CLAMP_SENSOR_ERROR);
        _manualService.Unsubscribe(ControllerRequestStrings.ERRORS_COLLET_SENSOR_ERROR);
        _manualService.Unsubscribe(ControllerRequestStrings.ERRORS_DORN_SENSOR_ERROR);
        _manualService.Unsubscribe(ControllerRequestStrings.ERRORS_PENULTIMATE_BEND_NOT_POSIBLE);
        _manualService.Unsubscribe(ControllerRequestStrings.ERRORS_MISSING_ROLLING_ADJUSTMENT);
        _manualService.Unsubscribe(ControllerRequestStrings.ERRORS_NO_PIPE_IN_COLLET);
        _manualService.Unsubscribe(ControllerRequestStrings.ERRORS_UNITS_NOT_IN_INITIALS_POSITIONS);
        _manualService.Unsubscribe(ControllerRequestStrings.ERRORS_PIPE_OFFSET);
        _manualService.Unsubscribe(ControllerRequestStrings.ERRORS_DRAWING_NOT_POSSIBLE);
        _manualService.Unsubscribe(ControllerRequestStrings.ERRORS_SUPPLY_IN_FORWARD_DANGER_ZONE);
        _manualService.Unsubscribe(ControllerRequestStrings.ERRORS_HYDRAULIC_SAFETY_OFF);
        _manualService.Unsubscribe(ControllerRequestStrings.ERRORS_BEND_RETURN);
        _manualService.Unsubscribe(ControllerRequestStrings.ERRORS_SUPPLY_DRIVE_ERROR);
        _manualService.Unsubscribe(ControllerRequestStrings.ERRORS_CONSOLE_DRIVE_ERROR);
        _manualService.Unsubscribe(ControllerRequestStrings.ERRORS_ROTATION_DRIVE_ERROR);
        _manualService.Unsubscribe(ControllerRequestStrings.ERRORS_CONFIRMATION_TUBE_INSTALLED);
    }

    private void SetStatusIfTrue(bool sensor, string status)
    {
        if (sensor)
            _statusService.CurrentStatus = new Core.Models.Status(status, Core.Models.StatusType.Error);
    }

    private void SetStatusIfFalse(bool sensor, string status)
    {
        if (!sensor)
            _statusService.CurrentStatus = new Core.Models.Status(status, Core.Models.StatusType.Error);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
        {
            Unsubscribe();
        }

        _disposed = true;
    }
}
