using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Configuration;
using StarkCNC.Core.Models;
using StarkCNC.DTO.Adjustment;

namespace StarkCNC.DTO;

public partial class AdjustmentParametersDto : ObservableObject, ICloneable
{
    private string _typeRequestString = string.Empty;
    private string _pipeDiameterRequestString = string.Empty;
    private string _forwardDangerZoneCoordinateRequestString = string.Empty;
    private string _distanceFromCenterRequestString = string.Empty;

    [ObservableProperty]
    private Guid? _id;

    [ObservableProperty]
    private string? _name;

    [ObservableProperty]
    private double _pipeDiameter;

    [ObservableProperty]
    private double _radius;

    [ObservableProperty]
    private AdjustmentType? _type;

    [ObservableProperty]
    private int _installedLevel;

    [ObservableProperty]
    private double _forwardDangerZoneCoordinate;

    [ObservableProperty]
    private double _distanceFromCenter;

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

    public static AdjustmentParametersDto? CreateFromConfiguration(IConfiguration configuration)
    {
        if (configuration is null)
            throw new ArgumentNullException(nameof(configuration));

        var adjustmentSection = configuration.GetSection("Adjustment");

        var adjustmentTypeSection = configuration.GetSection("AdjustmentType");
        var type = adjustmentTypeSection.GetSection("Default").Get<bool>() == true ? AdjustmentType.Rolling : AdjustmentType.Winding;
        var typeRequestString = adjustmentTypeSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var pipeDiameterSection = configuration.GetSection("PipeDiameter");
        var pipeDiameter = pipeDiameterSection.GetSection("Default").Get<double>();
        var pipeDiameterRequestString = pipeDiameterSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var forwardDangerZoneSection = configuration.GetSection("ForwardDangerZone");
        var forwardDangerZoneCoordinate = forwardDangerZoneSection.GetSection("Default").Get<double>();
        var forwardDangerZoneCoordinateRequestString = forwardDangerZoneSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var distanceFromCenterSection = configuration.GetSection("DistanceFromCenter");
        var distanceFromCenter = distanceFromCenterSection.GetSection("Default").Get<double>();
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
}