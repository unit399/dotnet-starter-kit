using Finbuckle.MultiTenant.Abstractions;

namespace ROC.Core.Infrastructure.Tenant.Abstractions;

public interface IRocTenantInfo : ITenantInfo
{
    string? ConnectionString { get; set; }
}