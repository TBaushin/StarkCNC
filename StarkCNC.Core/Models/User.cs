using Microsoft.AspNetCore.Identity;

namespace StarkCNC.Core.Models;

public class User : IdentityUser
{
    public IReadOnlyCollection<byte>? Image { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is not User user)
            return false;
        return this.UserName == user.UserName && this.Id == user.Id;
    }

    public override int GetHashCode() =>
        base.GetHashCode();
}
