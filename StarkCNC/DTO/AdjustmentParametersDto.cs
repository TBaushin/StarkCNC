using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Configuration;
using StarkCNC.Core.Models;
using StarkCNC.DTO.Adjustment;
using System.ComponentModel;

namespace StarkCNC.DTO;

public partial class AdjustmentParametersDto : ObservableObject, ICloneable
{
    private string _typeRequestString = string.Empty;
    private string _pipeDiameterRequestString = string.Empty;
    private string _forwardDangerZoneCoordinateRequestString = string.Empty;
    private string _distanceFromCenterRequestString = string.Empty;

    private Dictionary<string, PropertyChangedEventHandler> _childHandlers = new Dictionary<string, PropertyChangedEventHandler>();

    [ObservableProperty]
    private Guid? _id;

    [ObservableProperty]
    private string? _name;

    [ObservableProperty]
    private float _pipeDiameter;

    [ObservableProperty]
    private float _radius;

    [ObservableProperty]
    private AdjustmentType? _type;

    [ObservableProperty]
    private int _installedLevel;

    [ObservableProperty]
    private bool _isEnabled;

    [ObservableProperty]
    private float _forwardDangerZoneCoordinate;

    [ObservableProperty]
    private float _distanceFromCenter;

    [ObservableProperty]
    private Guid _bendId;
    [ObservableProperty]
    private BendDto? _bend;

    [ObservableProperty]
    private Guid _bendRollerId;
    [ObservableProperty]
    private BendRollerDto? _bendRoller;

    [ObservableProperty]
    private Guid _clampId;
    [ObservableProperty]
    private ClampDto? _clamp;

    [ObservableProperty]
    private Guid _clampRollerId;
    [ObservableProperty]
    private ClampRollerDto? _clampRoller;

    [ObservableProperty]
    private Guid _consoleId;
    [ObservableProperty]
    private ConsoleDto? _console;

    [ObservableProperty]
    private Guid _dornId;
    [ObservableProperty]
    private DornDto? _dorn;

    [ObservableProperty]
    private Guid _liftId;
    [ObservableProperty]
    private LiftDto? _lift;

    [ObservableProperty]
    private Guid _pressId;
    [ObservableProperty]
    private PressDto? _press;

    [ObservableProperty]
    private Guid _rotationId;
    [ObservableProperty]
    private RotationDto? _rotation;

    [ObservableProperty]
    private Guid _squeezeId;
    [ObservableProperty]
    private SqueezeDto? _squeeze;

    [ObservableProperty]
    private Guid _supplyId;
    [ObservableProperty]
    private SupplyDto? _supply;

    private AdjustmentParametersDto(
        string typeRequestString,
        string pipeDiameterRequestString,
        string forwardDangerZoneCoordinateRequestString,
        string distanceFromCenterRequestString)
    {
        _typeRequestString = typeRequestString;
        _pipeDiameterRequestString = pipeDiameterRequestString;
        _forwardDangerZoneCoordinateRequestString = forwardDangerZoneCoordinateRequestString;
        _distanceFromCenterRequestString = distanceFromCenterRequestString;
    }

    public object Clone() =>
        new AdjustmentParametersDto(
            _typeRequestString,
            _pipeDiameterRequestString,
            _forwardDangerZoneCoordinateRequestString,
            _distanceFromCenterRequestString)
        {
            Id = Id,
            Name = (string?)Name?.Clone(),
            PipeDiameter = PipeDiameter,
            Radius = Radius,
            Type = Type,
            InstalledLevel = InstalledLevel,
            ForwardDangerZoneCoordinate = ForwardDangerZoneCoordinate,
            DistanceFromCenter = DistanceFromCenter,
            Bend = Bend?.Clone() as BendDto,
            BendRoller = BendRoller?.Clone() as BendRollerDto,
            Clamp = Clamp?.Clone() as ClampDto,
            ClampRoller = ClampRoller?.Clone() as ClampRollerDto,
            Console = Console?.Clone() as ConsoleDto,
            Dorn = Dorn?.Clone() as DornDto,
            Lift = Lift?.Clone() as LiftDto,
            Press = Press?.Clone() as PressDto,
            Rotation = Rotation?.Clone() as RotationDto,
            Squeeze = Squeeze?.Clone() as SqueezeDto,
            Supply = Supply?.Clone() as SupplyDto,
        };

    public AdjustmentParameters? Parse(Guid? id)
    {
        try
        {
            return new AdjustmentParameters(
                DtoParseHelper.GetId(Id, id),
                DtoParser.RequireNotNull(Name, nameof(Name)),
                PipeDiameter,
                Radius,
                DtoParser.RequireNotNull(Type, nameof(Type)),
                InstalledLevel,
                IsEnabled,
                ForwardDangerZoneCoordinate,
                DistanceFromCenter,
                DtoParser.RequireNotNull(Bend?.Parse(BendId), nameof(Bend)),
                DtoParser.RequireNotNull(BendRoller?.Parse(BendRollerId), nameof(BendRoller)),
                DtoParser.RequireNotNull(Clamp?.Parse(ClampId), nameof(Clamp)),
                DtoParser.RequireNotNull(ClampRoller?.Parse(ClampRollerId), nameof(ClampRoller)),
                DtoParser.RequireNotNull(Console?.Parse(ConsoleId), nameof(Console)),
                DtoParser.RequireNotNull(Dorn?.Parse(DornId), nameof(Dorn)),
                DtoParser.RequireNotNull(Lift?.Parse(LiftId), nameof(Lift)),
                DtoParser.RequireNotNull(Press?.Parse(PressId), nameof(Press)),
                DtoParser.RequireNotNull(Rotation?.Parse(RotationId), nameof(Rotation)),
                DtoParser.RequireNotNull(Squeeze?.Parse(SqueezeId), nameof(Squeeze)),
                DtoParser.RequireNotNull(Supply?.Parse(SupplyId), nameof(Supply)));
        }
        catch (ArgumentNullException)
        {
            return null;
        }
    }

    public static AdjustmentParametersDto? CreateFromConfiguration()
    {
        var adjustmentSection = App.Configuration.GetSection("Adjustment");

        var adjustmentTypeSection = adjustmentSection.GetSection("AdjustmentType");
        var type = adjustmentTypeSection.GetSection("Default").Get<bool>() == true ? AdjustmentType.Rolling : AdjustmentType.Winding;
        var typeRequestString = adjustmentTypeSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var pipeDiameterSection = adjustmentSection.GetSection("PipeDiameter");
        var pipeDiameter = pipeDiameterSection.GetSection("Default").Get<float>();
        var pipeDiameterRequestString = pipeDiameterSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var forwardDangerZoneSection = adjustmentSection.GetSection("ForwardDangerZone");
        var forwardDangerZoneCoordinate = forwardDangerZoneSection.GetSection("Default").Get<float>();
        var forwardDangerZoneCoordinateRequestString = forwardDangerZoneSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var distanceFromCenterSection = adjustmentSection.GetSection("DistanceFromCenter");
        var distanceFromCenter = distanceFromCenterSection.GetSection("Default").Get<float>();
        var distanceFromCenterRequestString = distanceFromCenterSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var bend = BendDto.CreateFromConfiguration(adjustmentSection);
        var bendRoller = BendRollerDto.CreateFromConfiguration(adjustmentSection);
        var clamp = ClampDto.CreateFromConfiguration(adjustmentSection);
        var clampRoller = ClampRollerDto.CreateFromConfiguration(adjustmentSection);
        var console = ConsoleDto.CreateFromConfiguration(adjustmentSection);
        var dorn = DornDto.CreateFromConfiguration(adjustmentSection);
        var lift = LiftDto.CreateFromConfiguration(adjustmentSection);
        var press = PressDto.CreateFromConfiguration(adjustmentSection);
        var rotation = RotationDto.CreateFromConfiguration(adjustmentSection);
        var squeeze = SqueezeDto.CreateFromConfiguration(adjustmentSection);
        var supply = SupplyDto.CreateFromConfiguration(adjustmentSection);

        return new AdjustmentParametersDto(
            typeRequestString,
            pipeDiameterRequestString,
            forwardDangerZoneCoordinateRequestString,
            distanceFromCenterRequestString)
        {
            Type = type,
            PipeDiameter = pipeDiameter,
            ForwardDangerZoneCoordinate = forwardDangerZoneCoordinate,
            DistanceFromCenter = distanceFromCenter,
            Bend = bend,
            BendRoller = bendRoller,
            Clamp = clamp,
            ClampRoller = clampRoller,
            Console = console,
            Dorn = dorn,
            Lift = lift,
            Press = press,
            Rotation = rotation,
            Squeeze = squeeze,
            Supply = supply,
        };
    }

    partial void OnBendChanged(BendDto? oldValue, BendDto? newValue) =>
        OnChildChanged(oldValue, newValue, nameof(Bend));

    partial void OnBendRollerChanged(BendRollerDto? oldValue, BendRollerDto? newValue) =>
        OnChildChanged(oldValue, newValue, nameof(BendRoller));

    partial void OnClampChanged(ClampDto? oldValue, ClampDto? newValue) =>
        OnChildChanged(oldValue, newValue, nameof(Clamp));

    partial void OnClampRollerChanged(ClampRollerDto? oldValue, ClampRollerDto? newValue) =>
        OnChildChanged(oldValue, newValue, nameof(ClampRoller));

    partial void OnConsoleChanged(ConsoleDto? oldValue, ConsoleDto? newValue) =>
        OnChildChanged(oldValue, newValue, nameof(Console));

    partial void OnDornChanged(DornDto? oldValue, DornDto? newValue) =>
        OnChildChanged(oldValue, newValue, nameof(Dorn));

    partial void OnLiftChanged(LiftDto? oldValue, LiftDto? newValue) =>
        OnChildChanged(oldValue, newValue, nameof(Lift));

    partial void OnPressChanged(PressDto? oldValue, PressDto? newValue) =>
        OnChildChanged(oldValue, newValue, nameof(Press));

    partial void OnRotationChanged(RotationDto? oldValue, RotationDto? newValue) =>
        OnChildChanged(oldValue, newValue, nameof(Rotation));

    partial void OnSqueezeChanged(SqueezeDto? oldValue, SqueezeDto? newValue) =>
        OnChildChanged(oldValue, newValue, nameof(Squeeze));

    partial void OnSupplyChanged(SupplyDto? oldValue, SupplyDto? newValue) =>
        OnChildChanged(oldValue, newValue, nameof(Supply));

    private void OnChildChanged(ObservableObject? oldValue, ObservableObject? newValue, string childName)
    {
        if (!_childHandlers.TryGetValue(childName, out var handler))
        {
            handler = (_, _) => OnPropertyChanged(childName);
            _childHandlers[childName] = handler;
        }

        if (oldValue is not null)
            oldValue.PropertyChanged -= handler;

        if (newValue is not null)
            newValue.PropertyChanged += handler;
    }
}