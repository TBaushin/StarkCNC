using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Configuration;
using StarkCNC.Core.Models;
using StarkCNC.DTO.SettingsParameters;

namespace StarkCNC.DTO;

public partial class SettingsDto : ObservableObject, ICloneable
{
    private string _speedRequestString = string.Empty;
    private string _synchronizationCoefficientRequestString = string.Empty;
    private string _interceptionModeRequestString = string.Empty;

    [ObservableProperty]
    private Guid? _id;

    [ObservableProperty]
    private FloorType _floorType;

    [ObservableProperty]
    private bool _isElectricBendingDrive;

    [ObservableProperty]
    private bool _isPunchingCylinder;

    [ObservableProperty]
    private bool _isElectricMachine;

    [ObservableProperty]
    private double _speed;

    [ObservableProperty]
    private double _synchronizationCoefficient;

    [ObservableProperty]
    private bool _interceptionMode;

    [ObservableProperty]
    private Guid _bendId;
    [ObservableProperty]
    private BendDto? _bend;

    [ObservableProperty]
    private Guid _dornId;
    [ObservableProperty]
    private DornDto? _dorn;

    [ObservableProperty]
    private Guid _rotationId;
    [ObservableProperty]
    private RotationDto? _rotation;

    [ObservableProperty]
    private Guid _supportId;
    [ObservableProperty]
    private SupportDto? _support;

    [ObservableProperty]
    private Guid _supplyId;
    [ObservableProperty]
    private SupplyDto? _supply;

    [ObservableProperty]
    private Guid _consoleId;
    [ObservableProperty]
    private ConsoleDto? _console;

    [ObservableProperty]
    private Guid _pipeId;
    [ObservableProperty]
    private PipeDto? _pipe;

    public SettingsDto(
        string speedRequestString,
        string synchronizationCoefficientRequestString,
        string interceptionModeRequestString)
    {
        _speedRequestString = speedRequestString;
        _synchronizationCoefficientRequestString = synchronizationCoefficientRequestString;
        _interceptionModeRequestString = interceptionModeRequestString;
    }

    public object Clone() =>
        new SettingsDto(
            _speedRequestString,
            _synchronizationCoefficientRequestString,
            _interceptionModeRequestString)
            {
                FloorType = FloorType,
                IsElectricBendingDrive = IsElectricBendingDrive,
                IsPunchingCylinder = IsPunchingCylinder,
                IsElectricMachine = IsElectricMachine,
                Speed = Speed,
                SynchronizationCoefficient = SynchronizationCoefficient,
                InterceptionMode = InterceptionMode,
                Bend = Bend?.Clone() as BendDto,
                Dorn = Dorn?.Clone() as DornDto,
                Rotation = Rotation?.Clone() as RotationDto,
                Support = Support?.Clone() as SupportDto,
                Supply = Supply?.Clone() as SupplyDto,
                Console = Console?.Clone() as ConsoleDto,
                Pipe = Pipe?.Clone() as PipeDto
            };

    public Settings? Parse(Guid? id)
    {
        try
        {
            return new Settings(
                id ?? Guid.NewGuid(),
                FloorType,
                IsElectricBendingDrive,
                IsPunchingCylinder,
                IsElectricMachine,
                Speed,
                SynchronizationCoefficient,
                InterceptionMode,
                DtoParser.RequireNotNull(Bend?.Parse(BendId), nameof(Bend)),
                DtoParser.RequireNotNull(Dorn?.Parse(DornId), nameof(Dorn)),
                DtoParser.RequireNotNull(Rotation?.Parse(RotationId), nameof(Rotation)),
                DtoParser.RequireNotNull(Support?.Parse(SupportId), nameof(Support)),
                DtoParser.RequireNotNull(Supply?.Parse(SupplyId), nameof(Supply)),
                DtoParser.RequireNotNull(Console?.Parse(ConsoleId), nameof(Console)),
                DtoParser.RequireNotNull(Pipe?.Parse(PipeId), nameof(Pipe)));
        }
        catch (ArgumentNullException)
        {
            return null;
        }
    }

    public static SettingsDto? CreateFromConfiguration(IConfiguration configuration)
    {
        if (configuration is null)
            throw new ArgumentNullException(nameof(configuration));

        var settingsSection = configuration.GetSection("Settings");
        var floorType = FloorType.SingleLevel;

        var speedSection = settingsSection.GetSection("Speed");
        var speedRequestString = speedSection.GetSection("RequestString").Get<string>() ?? string.Empty;
        var speed = speedSection.GetSection("Default").Get<double>();

        var synchronizationCoefficientSection = settingsSection.GetSection("SynchronizationCoefficient");
        var synchronizationCoefficientRequestString = synchronizationCoefficientSection.GetSection("RequestString").Get<string>() ?? string.Empty;
        var synchronizationCoefficient = synchronizationCoefficientSection.GetSection("Default").Get<double>();

        var interceptionModeSection = settingsSection.GetSection("InterceptionMode");
        var interceptionModeRequestString = interceptionModeSection.GetSection("RequestString").Get<string>() ?? string.Empty;
        var interceptionMode = interceptionModeSection.GetSection("Default").Get<bool>();

        var bend = BendDto.CreateFromConfiguration(settingsSection);
        var dorn = DornDto.CreateFromConfiguration(settingsSection);
        var rotation = RotationDto.CreateFromConfiguration(settingsSection);
        var support = SupportDto.CreateFromConfiguration(settingsSection);
        var supply = SupplyDto.CreateFromConfiguration(settingsSection);
        var console = ConsoleDto.CreateFromConfiguration(settingsSection);
        var pipe = PipeDto.CreateFromConfiguration(settingsSection);

        return new SettingsDto(
            speedRequestString,
            synchronizationCoefficientRequestString,
            interceptionModeRequestString)
            {
                FloorType = floorType,
                Speed = speed,
                SynchronizationCoefficient = synchronizationCoefficient,
                InterceptionMode = interceptionMode,
                Bend = bend,
                Dorn = dorn,
                Rotation = rotation,
                Support = support,
                Supply = supply,
                Console = console,
                Pipe = pipe
            };
    }
}