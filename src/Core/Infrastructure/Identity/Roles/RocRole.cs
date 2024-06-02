using Microsoft.AspNetCore.Identity;

namespace ROC.Core.Infrastructure.Identity.Roles;

public class RocRole : IdentityRole
{
    public RocRole(string name, string? description = null)
        : base(name)
    {
        Description = description;
        NormalizedName = name.ToUpperInvariant();
    }

    public string? Description { get; set; }
}