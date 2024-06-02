using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ROC.Core.Infrastructure.Identity.RoleClaims;
using ROC.Core.Infrastructure.Identity.Roles;
using ROC.Core.Infrastructure.Identity.Users;

namespace ROC.Core.Infrastructure.Identity.Persistence;

public class ApplicationUserConfig : IEntityTypeConfiguration<RocUser>
{
    public void Configure(EntityTypeBuilder<RocUser> builder)
    {
        builder
            .ToTable("Users", IdentityConstants.SchemaName)
            .IsMultiTenant();

        builder
            .Property(u => u.ObjectId)
            .HasMaxLength(256);
    }
}

public class ApplicationRoleConfig : IEntityTypeConfiguration<RocRole>
{
    public void Configure(EntityTypeBuilder<RocRole> builder)
    {
        builder
            .ToTable("Roles", IdentityConstants.SchemaName)
            .IsMultiTenant()
            .AdjustUniqueIndexes();
    }
}

public class ApplicationRoleClaimConfig : IEntityTypeConfiguration<RocRoleClaim>
{
    public void Configure(EntityTypeBuilder<RocRoleClaim> builder)
    {
        builder
            .ToTable("RoleClaims", IdentityConstants.SchemaName)
            .IsMultiTenant();
    }
}

public class IdentityUserRoleConfig : IEntityTypeConfiguration<IdentityUserRole<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
    {
        builder
            .ToTable("UserRoles", IdentityConstants.SchemaName)
            .IsMultiTenant();
    }
}

public class IdentityUserClaimConfig : IEntityTypeConfiguration<IdentityUserClaim<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserClaim<string>> builder)
    {
        builder
            .ToTable("UserClaims", IdentityConstants.SchemaName)
            .IsMultiTenant();
    }
}

public class IdentityUserLoginConfig : IEntityTypeConfiguration<IdentityUserLogin<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserLogin<string>> builder)
    {
        builder
            .ToTable("UserLogins", IdentityConstants.SchemaName)
            .IsMultiTenant();
    }
}

public class IdentityUserTokenConfig : IEntityTypeConfiguration<IdentityUserToken<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserToken<string>> builder)
    {
        builder
            .ToTable("UserTokens", IdentityConstants.SchemaName)
            .IsMultiTenant();
    }
}