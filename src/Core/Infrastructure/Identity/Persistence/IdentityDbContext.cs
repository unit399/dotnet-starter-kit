using Finbuckle.MultiTenant.Abstractions;
using Finbuckle.MultiTenant.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ROC.Core.Infrastructure.Identity.RoleClaims;
using ROC.Core.Infrastructure.Identity.Roles;
using ROC.Core.Infrastructure.Identity.Users;
using ROC.Core.Infrastructure.Persistence;
using ROC.Core.Infrastructure.Tenant;
using ROC.WebApi.Core.Persistence;

namespace ROC.Core.Infrastructure.Identity.Persistence;

public class IdentityDbContext : MultiTenantIdentityDbContext<RocUser,
    RocRole,
    string,
    IdentityUserClaim<string>,
    IdentityUserRole<string>,
    IdentityUserLogin<string>,
    RocRoleClaim,
    IdentityUserToken<string>>
{
    private readonly DatabaseOptions _settings;

    public IdentityDbContext(IMultiTenantContextAccessor<RocTenantInfo> multiTenantContextAccessor,
        DbContextOptions<IdentityDbContext> options, IOptions<DatabaseOptions> settings) : base(
        multiTenantContextAccessor, options)
    {
        _settings = settings.Value;
        TenantInfo = multiTenantContextAccessor.MultiTenantContext.TenantInfo!;
    }

    private new RocTenantInfo TenantInfo { get; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(IdentityDbContext).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!string.IsNullOrWhiteSpace(TenantInfo?.ConnectionString))
            optionsBuilder.ConfigureDatabase(_settings.Provider, TenantInfo.ConnectionString);
    }
}