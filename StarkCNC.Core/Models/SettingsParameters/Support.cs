namespace StarkCNC.Core.Models.SettingsParameters;

public class Support : ICloneable
{
    public Guid Id { get; set; }

    public double FrontLiftBan { get; set; }

    public double MiddleLiftBan { get; set; }

    public double BackLiftBan { get; set; }

    public Support(
        double frontLiftBan,
        double middleLiftBan,
        double backLiftBan)
    {
        FrontLiftBan = frontLiftBan;
        MiddleLiftBan = middleLiftBan;
        BackLiftBan = backLiftBan;
    }

    public object Clone() => MemberwiseClone();

    public override bool Equals(object? obj)
    {
        if (obj is not Support other)
            return false;

        return
            Id == other.Id &&
            FrontLiftBan == other.FrontLiftBan &&
            MiddleLiftBan == other.MiddleLiftBan &&
            BackLiftBan == other.BackLiftBan;
    }

    public override int GetHashCode() =>
        HashCode.Combine(
            Id,
            FrontLiftBan,
            MiddleLiftBan,
            BackLiftBan);
}