using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ROC.WebApi.Core.Tenant.Interfaces;

namespace ROC.WebApi.Core.Tenant.Handlers.UpgradeSubscription;

public class UpgradeSubscriptionHandler : IRequestHandler<UpgradeSubscriptionCommand, UpgradeSubscriptionResponse>
{
    private readonly ITenantService _tenantService;

    public UpgradeSubscriptionHandler(ITenantService tenantService)
    {
        _tenantService = tenantService;
    }

    public async Task<UpgradeSubscriptionResponse> Handle(UpgradeSubscriptionCommand request,
        CancellationToken cancellationToken)
    {
        var validUpto = await _tenantService.UpradeSubscription(request.Tenant, request.ExtendedExpiryDate);
        return new UpgradeSubscriptionResponse(validUpto, request.Tenant);
    }
}