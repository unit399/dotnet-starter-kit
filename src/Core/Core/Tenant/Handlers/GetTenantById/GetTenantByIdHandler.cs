using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ROC.WebApi.Core.Tenant.Entities;
using ROC.WebApi.Core.Tenant.Interfaces;

namespace ROC.WebApi.Core.Tenant.Handlers.GetTenantById;

public sealed class GetTenantByIdHandler(ITenantService service) : IRequestHandler<GetTenantByIdQuery, TenantDetail>
{
    public async Task<TenantDetail> Handle(GetTenantByIdQuery request, CancellationToken cancellationToken)
    {
        return await service.GetByIdAsync(request.TenantId);
    }
}