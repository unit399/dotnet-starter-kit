using MediatR;

namespace ROC.WebApi.Core.Tenant.Handlers.ActivateTenant;

public record ActivateTenantCommand(string TenantId) : IRequest<ActivateTenantResponse>;