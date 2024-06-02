using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ROC.WebApi.Core.Tenant.Entities;
using ROC.WebApi.Core.Tenant.Handlers.CreateTenant;

namespace ROC.WebApi.Core.Tenant.Interfaces;

public interface ITenantService
{
    Task<List<TenantDetail>> GetAllAsync();
    Task<bool> ExistsWithIdAsync(string id);
    Task<bool> ExistsWithNameAsync(string name);
    Task<TenantDetail> GetByIdAsync(string id);
    Task<string> CreateAsync(CreateTenantCommand request, CancellationToken cancellationToken);
    Task<string> ActivateAsync(string id, CancellationToken cancellationToken);
    Task<string> DeactivateAsync(string id);
    Task<DateTime> UpradeSubscription(string id, DateTime extendedExpiryDate);
}