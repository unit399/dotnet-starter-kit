using FluentValidation;

namespace ROC.WebApi.Core.Tenant.Handlers.ActivateTenant;

public sealed class ActivateTenantValidator : AbstractValidator<ActivateTenantCommand>
{
    public ActivateTenantValidator()
    {
        RuleFor(t => t.TenantId)
            .NotEmpty();
    }
}