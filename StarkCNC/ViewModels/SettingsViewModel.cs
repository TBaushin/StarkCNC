using CommunityToolkit.Mvvm.ComponentModel;
using StarkCNC.Core.Models;
using StarkCNC.Core.Repository;

namespace StarkCNC.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    private ISettingsRepository _settingsRepository;
    private Settings _settings;

    [ObservableProperty]
    private bool _dornAutomatic;

    [ObservableProperty]
    private double _dornLeadWithdrawalBeforeBend;

    [ObservableProperty]
    private bool _dornLubricantTurnOn;

    [ObservableProperty]
    private bool _bendSynchronization;

    [ObservableProperty]
    private bool _synchronizationCoefficient;

    [ObservableProperty]
    private bool _bendAndSupplySynchronization;

    [ObservableProperty]
    private bool _interceptionMode;

    [ObservableProperty]
    private bool _consoleOutletForPipeInstalling;

    [ObservableProperty]
    private double _pipeOutletCoordinate;

    [ObservableProperty]
    private bool _singleLeveled;

    [ObservableProperty]
    private bool _withPunchingCylinder;

    [ObservableProperty]
    private double _distanceFromBendingToPunching;

    [ObservableProperty]
    private double _isElectricBendingDrive;

    [ObservableProperty]
    private bool _absoluteUnitCoordinate;

    [ObservableProperty]
    private double _supportFirstLiftBan;

    [ObservableProperty]
    private double _supportSecondLiftBan;

    [ObservableProperty]
    private double _supportThirdLiftBanRear;

    [ObservableProperty]
    private double _supportThirdLiftBanFront;

    [ObservableProperty]
    private double _supportFourthLiftBan;

    [ObservableProperty]
    private bool _banPressWhenSupportIsLifted;

    [ObservableProperty]
    private double _squeezeWorkTime;

    [ObservableProperty]
    private double _supplyStartRollingSpeed;

    [ObservableProperty]
    private bool _incompleteClampMovement;

    [ObservableProperty]
    private bool _hydraulicMovementWithoutSensors;

    [ObservableProperty]
    private bool _showButtonFullAutomatic;

    [ObservableProperty]
    private bool _invertClampSensors;

    [ObservableProperty]
    private double _supplyCoefficient;

    [ObservableProperty]
    private double _rotationCoefficient;

    [ObservableProperty]
    private double _consoleCoefficient;

    [ObservableProperty]
    private double _bendCoefficient;

    [ObservableProperty]
    private double _supplyAcceleration;

    [ObservableProperty]
    private double _rotationAcceleration;

    [ObservableProperty]
    private double _consoleAcceleration;

    [ObservableProperty]
    private double _bendAcceleration;

    [ObservableProperty]
    private double _supplyBraking;

    [ObservableProperty]
    private double _rotationBraking;

    [ObservableProperty]
    private double _consoleBraking;

    [ObservableProperty]
    private double _bendBraking;

    [ObservableProperty]
    private double _supplyJerk;

    [ObservableProperty]
    private double _rotationJerk;

    [ObservableProperty]
    private double _consoleJerk;

    [ObservableProperty]
    private double _bendJerk;

    public SettingsViewModel(ISettingsRepository settingsRepository)
    {
        _settingsRepository = settingsRepository;

        var settings = _settingsRepository.GetAsync().Result;
        if (settings is null)
            _settings = new Settings();
        else
            _settings = settings;
    }
}