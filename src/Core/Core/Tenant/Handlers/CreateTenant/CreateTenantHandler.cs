using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ROC.WebApi.Core.Tenant.Interfaces;

namespace ROC.WebApi.Core.Tenant.Handlers.CreateTenant;

public sealed class CreateTenantHandler(ITenantService service)
    : IRequestHandler<CreateTenantCommand, CreateTenantResponse>
{
    public async Task<CreateTenantResponse> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
    {
        var tenantId = await service.CreateAsync(request, cancellationToken);
        return new CreateTenantResponse(tenantId);
    }
}