using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ROC.WebApi.Core.Tenant.Interfaces;

namespace ROC.WebApi.Core.Tenant.Handlers.ActivateTenant;

public sealed class ActivateTenantHandler(ITenantService service)
    : IRequestHandler<ActivateTenantCommand, ActivateTenantResponse>
{
    public async Task<ActivateTenantResponse> Handle(ActivateTenantCommand request, CancellationToken cancellationToken)
    {
        var status = await service.ActivateAsync(request.TenantId, cancellationToken);
        return new ActivateTenantResponse(status);
    }
}