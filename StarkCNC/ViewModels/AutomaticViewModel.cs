using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarkCNC.Core.Models;
using StarkCNC.Core.Repository;
using StarkCNC.Core.Services;
using StarkCNC.Core.UoW;
using StarkCNC.MachineCommunication.Services;
using StarkCNC.Utilities;
using System.Collections.ObjectModel;
using System.Windows;

namespace StarkCNC.ViewModels;

public partial class AutomaticViewModel : ViewModelBase, IDisposable
{
    private IManualConfigurationService _configurationService;
    private IBendingDataUnitOfWork _unitOfWork;
    private IUserService _userService;
    private ISettingsRepository _settingsRepository;

    private bool _disposed;

    [ObservableProperty]
    private Settings _settings;

    [ObservableProperty]
    private InputOutputTableViewModel _tableViewModel;

    [ObservableProperty]
    private string _programName = string.Empty;

    [ObservableProperty]
    private string _operator = string.Empty;

    [ObservableProperty]
    private float _speed;

    [ObservableProperty]
    private float _cycleTime;

    [ObservableProperty]
    private float _pipeLength;

    [ObservableProperty]
    private float _setUpPoint;

    [ObservableProperty]
    private int _countCompletedDetails;

    [ObservableProperty]
    private int _taskDetails;

    [ObservableProperty]
    private bool _canChangeCountDetails = true;

    [ObservableProperty]
    private bool _isFullAtomatic;

    [ObservableProperty]
    private float _pipeInstallationDelay;

    [ObservableProperty]
    private float _currentTaskSupply;

    [ObservableProperty]
    private float _currentTaskRotationAngle;

    [ObservableProperty]
    private float _currentTaskBendingAngle;

    [ObservableProperty]
    private float _facticalSupply;

    [ObservableProperty]
    private float _facticalRotationAngle;

    [ObservableProperty]
    private float _facticalBendingAngle;

    [ObservableProperty]
    private float _facticalConsole;

    [ObservableProperty]
    private bool _sendData;

    [ObservableProperty]
    private bool _hasErrors;

    [ObservableProperty]
    private bool _showInputOutputTable;

    public ObservableCollection<BendingDataViewModel> BendingDatas { get; } = new ObservableCollection<BendingDataViewModel>();

    public AutomaticViewModel(
        IManualConfigurationService configurationService,
        IBendingDataUnitOfWork unitOfWork,
        IUserService userService,
        ISettingsRepository settingsRepository)
    {
        _configurationService = configurationService;
        _unitOfWork = unitOfWork;
        _userService = userService;
        _settingsRepository = settingsRepository;

        BendingDatas.CollectionChanged += BendingDatas_CollectionChanged;
    }

    public async Task InitializeAsync()
    {
        ProgramName = _unitOfWork.ProgramName;
        PipeLength = _unitOfWork.PipeLength;
        SetUpPoint = _unitOfWork.SetUpPoint;

        await LoadBendingData().ConfigureAwait(true);

        Operator = _userService.CurrentUser?.UserName ?? string.Empty;

        Settings = await _settingsRepository.GetAsync().ConfigureAwait(true) ?? new Settings();

        Subscribe();

        TableViewModel = new InputOutputTableViewModel(_configurationService, _settingsRepository);
        TableViewModel.ShowOrHideInputOutputTableCommand = ShowOrHideInputOutputTableCommand;
    }

    private async Task LoadBendingData()
    {
        var list = _unitOfWork.BendingDatas
            .Select((item, index) => new BendingDataViewModel(item) { Id = index + 1 })
            .ToList();

        await Application.Current.Dispatcher.InvokeAsync(() =>
        {
            BendingDatas.Clear();
            list.ForEach(item => BendingDatas.Add(item));
        });
    }

    [RelayCommand(AllowConcurrentExecutions = true)]
    private async Task SetAutomaticMode()
    {
        await _configurationService
            .WriteAsync(true, ControllerRequestStrings.AUTOMATIC_TAGS_TURN_ON)
            .ConfigureAwait(true);
    }

    [RelayCommand(AllowConcurrentExecutions = true)]
    private async Task ClearActuatorErrors()
    {
        await _configurationService
            .WriteAsync(true, ControllerRequestStrings.ERRORS_CLEAR_ACTUATOR_ERRORS)
            .ConfigureAwait(true);

        HasErrors = false;
    }

    [RelayCommand]
    private void ShowOrHideInputOutputTable()
    {
        if (ShowInputOutputTable)
        {
            ShowInputOutputTable = false;
            TableViewModel.Unsubscribe();
        }
        else
        {
            ShowInputOutputTable = true;
            TableViewModel.Subscribe();
        }
    }

    private void Subscribe()
    {
        _configurationService.Subscribe<bool>(ControllerRequestStrings.ERRORS_HAS_ERRORS, value => HasErrors = value);
        _configurationService.Subscribe<float>(ControllerRequestStrings.AUTOMATIC_TAGS_CYCLE_TIME, value => CycleTime = value);
        _configurationService.Subscribe<bool>(ControllerRequestStrings.AUTOMATIC_TAGS_SEND_DATA, value => SetSendData(value));
        _configurationService.Subscribe<int>(ControllerRequestStrings.AUTOMATIC_TAGS_COUNT_COMPLETED_DETAILS, value => CountCompletedDetails = value);
        _configurationService.Subscribe<float>(ControllerRequestStrings.SUPPLY_FACTICAL_POSITION, value => FacticalSupply = value);
        _configurationService.Subscribe<float>(ControllerRequestStrings.ROTATION_FACTICAL_POSITION, value => FacticalRotationAngle = value);
        _configurationService.Subscribe<float>(ControllerRequestStrings.BEND_FACTICAL_POSITION, value => FacticalBendingAngle =  value);
        _configurationService.Subscribe<float>(ControllerRequestStrings.CONSOLE_FACTICAL_POSITION, value => FacticalConsole = value);
        _configurationService.Subscribe<float>(ControllerRequestStrings.SUPPLY_VALUE, value => CurrentTaskSupply = value);
        _configurationService.Subscribe<float>(ControllerRequestStrings.ROTATION_VALUE, value => CurrentTaskRotationAngle = value);
        _configurationService.Subscribe<float>(ControllerRequestStrings.BEND_VALUE, value => CurrentTaskBendingAngle = value);
    }

    private void Unsubscribe()
    {
        _configurationService.Unsubscribe(ControllerRequestStrings.ERRORS_HAS_ERRORS);
        _configurationService.Unsubscribe(ControllerRequestStrings.AUTOMATIC_TAGS_CYCLE_TIME);
        _configurationService.Unsubscribe(ControllerRequestStrings.AUTOMATIC_TAGS_SEND_DATA);
        _configurationService.Unsubscribe(ControllerRequestStrings.AUTOMATIC_TAGS_COUNT_COMPLETED_DETAILS);
        _configurationService.Unsubscribe(ControllerRequestStrings.SUPPLY_FACTICAL_POSITION);
        _configurationService.Unsubscribe(ControllerRequestStrings.ROTATION_FACTICAL_POSITION);
        _configurationService.Unsubscribe(ControllerRequestStrings.BEND_FACTICAL_POSITION);
        _configurationService.Unsubscribe(ControllerRequestStrings.CONSOLE_FACTICAL_POSITION);
        _configurationService.Unsubscribe(ControllerRequestStrings.SUPPLY_VALUE);
        _configurationService.Unsubscribe(ControllerRequestStrings.ROTATION_VALUE);
        _configurationService.Unsubscribe(ControllerRequestStrings.BEND_VALUE);
    }

    private void SetSendData(bool value)
    {
        SendData = value;

        if (SendData == true)
        {
            CanChangeCountDetails = false;
            RunProgram();
        }
    }

    private async void RunProgram()
    {
        // Send data
        await _configurationService
            .WriteAsync<int>(BendingDatas.Count, ControllerRequestStrings.AUTOMATIC_TAGS_ALL_BEND)
            .ConfigureAwait(true);

        _configurationService.Subscribe<int>(ControllerRequestStrings.AUTOMATIC_TAGS_STEP_NUMBER, async (value) =>
        {
            if (value >= 0 && value <= BendingDatas.Count - 1)
            {
                var current = BendingDatas[value];
                await _configurationService
                    .WriteAsync<float>(current.Supply, ControllerRequestStrings.SUPPLY_VALUE)
                    .ConfigureAwait(true);
                await _configurationService
                    .WriteAsync<float>(current.RotationAngle, ControllerRequestStrings.ROTATION_VALUE)
                    .ConfigureAwait(true);
                await _configurationService
                    .WriteAsync<float>(current.BendingAngle, ControllerRequestStrings.BEND_VALUE)
                    .ConfigureAwait(true);

                await _configurationService
                    .WriteAsync<bool>(false, ControllerRequestStrings.AUTOMATIC_TAGS_END_PROGRAM)
                    .ConfigureAwait(true);
            }

            if (value == BendingDatas.Count)
            {
                await _configurationService
                    .WriteAsync<bool>(true, ControllerRequestStrings.AUTOMATIC_TAGS_END_PROGRAM)
                    .ConfigureAwait(true);

                await _configurationService
                    .WriteAsync<bool>(false, ControllerRequestStrings.AUTOMATIC_TAGS_SEND_DATA)
                    .ConfigureAwait(true);
            }
        });
        SetSendData(false);
    }

    private void BendingDatas_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        int i = 1;
        foreach (var item in BendingDatas)
        {
            item.Id = i;
            i++;
        }
    }

    async partial void OnIsFullAtomaticChanged(bool oldValue, bool newValue)
    {
        await _configurationService.WriteAsync<bool>(newValue, ControllerRequestStrings.AUTOMATIC_TAGS_FULL_AUTOMATIC)
            .ConfigureAwait(true);
    }

    async partial void OnPipeInstallationDelayChanged(float oldValue, float newValue)
    {
        if (IsFullAtomatic)
        {
            await _configurationService.WriteAsync<float>(newValue, ControllerRequestStrings.AUTOMATIC_TAGS_DELAY)
                .ConfigureAwait(true);
        }
    }

    partial void OnPipeLengthChanged(float oldValue, float newValue)
    {
        _unitOfWork.PipeLength = newValue;
        _unitOfWork.HasUnsavedData = true;
        _unitOfWork.SaveFile();
    }

    partial void OnSetUpPointChanged(float oldValue, float newValue)
    {
        _unitOfWork.SetUpPoint = newValue;
        _unitOfWork.HasUnsavedData = false;
        _unitOfWork.SaveFile();
    }

    partial void OnCountCompletedDetailsChanged(int oldValue, int newValue)
    {
        if (newValue >= TaskDetails)
        {
#if !DEBUG
            var doneMessageBox = MessageBox.Show("Задание выполнено", "Задание выполнено", MessageBoxButton.OK, MessageBoxImage.Information);
#endif
            CanChangeCountDetails = true;
        }
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
            TableViewModel.Dispose();
            Unsubscribe();
        }

        _disposed = true;
    }
}
