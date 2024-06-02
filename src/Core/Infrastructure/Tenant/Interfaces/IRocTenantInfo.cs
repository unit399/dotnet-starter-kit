using Finbuckle.MultiTenant.Abstractions;

namespace ROC.Core.Infrastructure.Tenant.Interfaces;

public interface IRocTenantInfo : ITenantInfo
{
    string? ConnectionString { get; set; }
}