using System;

namespace ROC.WebApi.Core.Tenant.Handlers.UpgradeSubscription;

public record UpgradeSubscriptionResponse(DateTime NewValidity, string Tenant);