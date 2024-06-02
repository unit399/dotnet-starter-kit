using Finbuckle.MultiTenant.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ROC.Core.Infrastructure.Persistence;
using ROC.Core.Infrastructure.Tenant;
using ROC.WebApi.Catalog.Domain;
using ROC.WebApi.Core.Persistence;

namespace ROC.WebApi.Catalog.Infrastructure.Persistence;

public sealed class CatalogDbContext : RocDbContext
{
    public CatalogDbContext(IMultiTenantContextAccessor<RocTenantInfo> multiTenantContextAccessor,
        DbContextOptions<CatalogDbContext> options, IPublisher publisher, IOptions<DatabaseOptions> settings)
        : base(multiTenantContextAccessor, options, publisher, settings)
    {
    }

    public DbSet<Product> Products { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogDbContext).Assembly);
        modelBuilder.HasDefaultSchema(SchemaNames.Catalog);
    }
}