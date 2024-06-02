using System;
using Microsoft.AspNetCore.Identity;

namespace ROC.Core.Infrastructure.Identity.RoleClaims;

public class RocRoleClaim : IdentityRoleClaim<string>
{
    public string? CreatedBy { get; init; }
    public DateTimeOffset CreatedOn { get; init; }
}