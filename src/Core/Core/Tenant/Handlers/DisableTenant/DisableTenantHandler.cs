using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ROC.WebApi.Core.Tenant.Interfaces;

namespace ROC.WebApi.Core.Tenant.Handlers.DisableTenant;

public sealed class DisableTenantHandler(ITenantService service)
    : IRequestHandler<DisableTenantCommand, DisableTenantResponse>
{
    public async Task<DisableTenantResponse> Handle(DisableTenantCommand request, CancellationToken cancellationToken)
    {
        var status = await service.DeactivateAsync(request.TenantId);
        return new DisableTenantResponse(status);
    }
}