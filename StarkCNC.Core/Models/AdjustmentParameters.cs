using Microsoft.Extensions.Configuration;
using StarkCNC.Core.Models.Adjustment;

namespace StarkCNC.Core.Models;

public class AdjustmentParameters : ICloneable
{
    private readonly IConfiguration _configuration;
    private string _adjustmentTypeRequestString = string.Empty;
    private string _pipeDiameterRequestString = string.Empty;
    private string _forwardDangerZoneCoordinateRequestString = string.Empty;
    private string _distanceFromCenterRequestString = string.Empty;

    public string Name { get; set; } = string.Empty;

    public double PipeDiameter { get; set; }

    public double Radius { get; set; }

    public AdjustmentType Type { get; set; }

    public int InstalledLevel { get; set; }

    public double ForwardDangerZoneCoordinate { get; set; }

    public double DistanceFromCenter { get; set; }

    public Bend Bend { get; set; }

    public BendRoller BendRoller { get; set; }
    
    public Clamp Clamp { get; set; }

    public ClampRoller ClampRoller { get; set; }

    public StarkCNC.Core.Models.Adjustment.Console Console { get; set; }

    public Dorn Dorn { get; set; }

    public Lift Lift { get; set; }

    public Press Press { get; set; }

    public Rotation Rotation { get; set; }

    public Squeeze Squeeze { get; set; }

    public Supply Supply { get; set; }

    public AdjustmentParameters(IConfiguration configuration, string name)
    {
        _configuration = configuration;
        Name = name;

        var adjustmentSection = configuration.GetSection("Adjustment");

        var adjustmentTypeSection = _configuration.GetSection("AdjustmentType");
        Type = adjustmentSection.GetSection("Default").Get<bool>() == true ? AdjustmentType.Rolling : AdjustmentType.Winding;
        _adjustmentTypeRequestString = adjustmentSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var pipeDiameterSection = _configuration.GetSection("PipeDiameter");
        PipeDiameter = pipeDiameterSection.GetSection("Default").Get<double>();
        _pipeDiameterRequestString = pipeDiameterSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var forwardDangerZoneSection = _configuration.GetSection("ForwardDangerZone");
        ForwardDangerZoneCoordinate = forwardDangerZoneSection.GetSection("Default").Get<double>();
        _forwardDangerZoneCoordinateRequestString = forwardDangerZoneSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var distanceFromCenterSection = _configuration.GetSection("DistanceFromCenter");
        DistanceFromCenter = distanceFromCenterSection.GetSection("Default").Get<double>();
        _distanceFromCenterRequestString = distanceFromCenterSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        Bend = Bend.ReadConfiguration(adjustmentSection);
        BendRoller = BendRoller.ReadConfiguration(adjustmentSection);
        Clamp = Clamp.ReadConfiguration(adjustmentSection);
        ClampRoller = ClampRoller.ReadConfiguration(adjustmentSection);
        Console = StarkCNC.Core.Models.Adjustment.Console.ReadConfiguration(adjustmentSection);
        Dorn = Dorn.ReadConfiguration(adjustmentSection);
        Lift = Lift.ReadConfiguration(adjustmentSection);
        Press = Press.ReadConfiguration(adjustmentSection);
        Rotation = Rotation.ReadConfiguration(adjustmentSection);
        Squeeze = Squeeze.ReadConfiguration(adjustmentSection);
        Supply = Supply.ReadConfiguration(adjustmentSection);
    }

    public override bool Equals(object? obj)
    {
        if (obj is not AdjustmentParameters other)
            return false;

        return
            other.Name == Name &&
            other.PipeDiameter == PipeDiameter &&
            other.Radius == Radius &&
            other.Type == Type &&
            other.InstalledLevel == InstalledLevel &&
            other.ForwardDangerZoneCoordinate == ForwardDangerZoneCoordinate &&
            other.DistanceFromCenter == DistanceFromCenter &&
            other.Bend.Equals(Bend) &&
            other.BendRoller.Equals(BendRoller) &&
            other.Clamp.Equals(Clamp) &&
            other.ClampRoller.Equals(ClampRoller) &&
            other.Console.Equals(Console) &&
            other.Dorn.Equals(Dorn) &&
            other.Lift.Equals(Lift) &&
            other.Press.Equals(Press) &&
            other.Rotation.Equals(Rotation) &&
            other.Squeeze.Equals(Squeeze) &&
            other.Supply.Equals(Supply);
    }

    public override int GetHashCode() =>
        HashCode.Combine(
            Name,
            PipeDiameter,
            Type,
            InstalledLevel,
            ForwardDangerZoneCoordinate,
            DistanceFromCenter);
    
    public object Clone() =>
        new AdjustmentParameters(_configuration, Name) {
            Type = Type,
            PipeDiameter = PipeDiameter,
            Radius = Radius,
            InstalledLevel = InstalledLevel,
            ForwardDangerZoneCoordinate = ForwardDangerZoneCoordinate,
            DistanceFromCenter = DistanceFromCenter,
            Bend = (Bend)Bend.Clone(),
            BendRoller = (BendRoller)BendRoller.Clone(),
            Clamp = (Clamp)Clamp.Clone(),
            ClampRoller = (ClampRoller)ClampRoller.Clone(),
            Console = (StarkCNC.Core.Models.Adjustment.Console)Console.Clone(),
            Dorn = (Dorn)Dorn.Clone(),
            Lift = (Lift)Lift.Clone(),
            Press = (Press)Press.Clone(),
            Rotation = (Rotation)Rotation.Clone(),
            Squeeze = (Squeeze)Squeeze.Clone(),
            Supply = (Supply)Supply.Clone(),
        };
}
