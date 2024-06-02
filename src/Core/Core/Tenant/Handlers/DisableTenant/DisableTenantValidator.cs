using FluentValidation;

namespace ROC.WebApi.Core.Tenant.Handlers.DisableTenant;

public sealed class DisableTenantValidator : AbstractValidator<DisableTenantCommand>
{
    public DisableTenantValidator()
    {
        RuleFor(t => t.TenantId)
            .NotEmpty();
    }
}