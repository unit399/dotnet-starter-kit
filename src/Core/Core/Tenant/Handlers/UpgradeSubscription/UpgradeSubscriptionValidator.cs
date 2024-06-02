using System;
using FluentValidation;

namespace ROC.WebApi.Core.Tenant.Handlers.UpgradeSubscription;

public class UpgradeSubscriptionValidator : AbstractValidator<UpgradeSubscriptionCommand>
{
    public UpgradeSubscriptionValidator()
    {
        RuleFor(t => t.Tenant).NotEmpty();
        RuleFor(t => t.ExtendedExpiryDate).GreaterThan(DateTime.UtcNow);
    }
}