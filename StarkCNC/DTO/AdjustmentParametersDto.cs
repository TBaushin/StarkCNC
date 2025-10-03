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
    private BendDto? _bend;

    [ObservableProperty]
    private BendRollerDto? _bendRoller;

    [ObservableProperty]
    private ClampDto? _clamp;

    [ObservableProperty]
    private ClampRollerDto? _clampRoller;

    [ObservableProperty]
    private ConsoleDto? _console;

    [ObservableProperty]
    private DornDto? _dorn;

    [ObservableProperty]
    private LiftDto? _lift;

    [ObservableProperty]
    private PressDto? _press;

    [ObservableProperty]
    private RotationDto? _rotation;

    [ObservableProperty]
    private SqueezeDto? _squeeze;

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
            Bend = (BendDto?)Bend?.Clone(),
            BendRoller = (BendRollerDto?)BendRoller?.Clone(),
            Clamp = (ClampDto?)Clamp?.Clone(),
            ClampRoller = (ClampRollerDto?)ClampRoller?.Clone(),
            Console = (ConsoleDto?)Console?.Clone(),
            Dorn = (DornDto?)Dorn?.Clone(),
            Lift = (LiftDto?)Lift?.Clone(),
            Press = (PressDto?)Press?.Clone(),
            Rotation = (RotationDto?)Rotation?.Clone(),
            Squeeze = (SqueezeDto?)Squeeze?.Clone(),
            Supply = (SupplyDto?)Supply?.Clone(),
        };

    public AdjustmentParameters? Parse(Guid? id)
    {
        if (id is null)
            id = Guid.NewGuid();

        try
        {
            return new AdjustmentParameters(
                id.Value,
                DtoParser.RequireNotNull(Name, nameof(Name)),
                PipeDiameter,
                Radius,
                DtoParser.RequireNotNull(Type, nameof(Type)),
                InstalledLevel,
                ForwardDangerZoneCoordinate,
                DistanceFromCenter,
                DtoParser.RequireNotNull(Bend?.Parse(), nameof(Bend)),
                DtoParser.RequireNotNull(BendRoller?.Parse(), nameof(BendRoller)),
                DtoParser.RequireNotNull(Clamp?.Parse(), nameof(Clamp)),
                DtoParser.RequireNotNull(ClampRoller?.Parse(), nameof(ClampRoller)),
                DtoParser.RequireNotNull(Console?.Parse(), nameof(Console)),
                DtoParser.RequireNotNull(Dorn?.Parse(), nameof(Dorn)),
                DtoParser.RequireNotNull(Lift?.Parse(), nameof(Lift)),
                DtoParser.RequireNotNull(Press?.Parse(), nameof(Press)),
                DtoParser.RequireNotNull(Rotation?.Parse(), nameof(Rotation)),
                DtoParser.RequireNotNull(Squeeze?.Parse(), nameof(Squeeze)),
                DtoParser.RequireNotNull(Supply?.Parse(), nameof(Supply)));
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
        var type = adjustmentSection.GetSection("Default").Get<bool>() == true ? AdjustmentType.Rolling : AdjustmentType.Winding;
        var typeRequestString = adjustmentSection.GetSection("RequestString").Get<string>() ?? string.Empty;

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
