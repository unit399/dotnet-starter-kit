using System;
using MediatR;

namespace ROC.WebApi.Core.Tenant.Handlers.UpgradeSubscription;

public class UpgradeSubscriptionCommand : IRequest<UpgradeSubscriptionResponse>
{
    public string Tenant { get; set; } = default!;
    public DateTime ExtendedExpiryDate { get; set; }
}