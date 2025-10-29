using System.ComponentModel.DataAnnotations;

namespace StarkCNC.Core.Models.SettingsParameters;

public class Dorn : ICloneable
{
    [Key]
    public Guid Id { get; set; }
    public bool Automatic { get; set; }
    public bool LubricantTurnOn { get; set; }
    public double LeadWithdrawalBeforeBend { get; set; }

    public Dorn(
        bool automatic,
        bool lubricantTurnOn,
        double leadWithdrawalBeforeBend)
    {
        Automatic = automatic;
        LubricantTurnOn = lubricantTurnOn;
        LeadWithdrawalBeforeBend = leadWithdrawalBeforeBend;
    }

    public object Clone() => MemberwiseClone();

    public override bool Equals(object? obj)
    {
        if (obj is not Dorn other)
            return false;

        return
            Id == other.Id &&
            Automatic == other.Automatic &&
            LubricantTurnOn == other.LubricantTurnOn &&
            LeadWithdrawalBeforeBend == other.LeadWithdrawalBeforeBend;
    }

    public override int GetHashCode() =>
        HashCode.Combine(
            Id,
            Automatic,
            LubricantTurnOn,
            LeadWithdrawalBeforeBend);
}
