using Microsoft.AspNetCore.Identity;

namespace StarkCNC.Core.Models;

public class User : IdentityUser
{
    public IReadOnlyCollection<byte>? Image { get; set; }
}
