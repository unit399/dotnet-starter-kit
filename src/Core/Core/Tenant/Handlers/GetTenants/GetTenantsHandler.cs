using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ROC.WebApi.Core.Tenant.Entities;
using ROC.WebApi.Core.Tenant.Interfaces;

namespace ROC.WebApi.Core.Tenant.Handlers.GetTenants;

public sealed class GetTenantsHandler(ITenantService service) : IRequestHandler<GetTenantsQuery, List<TenantDetail>>
{
    public Task<List<TenantDetail>> Handle(GetTenantsQuery request, CancellationToken cancellationToken)
    {
        return service.GetAllAsync();
    }
}