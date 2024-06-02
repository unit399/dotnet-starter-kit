using MediatR;

namespace ROC.WebApi.Core.Tenant.Handlers.DisableTenant;

public record DisableTenantCommand(string TenantId) : IRequest<DisableTenantResponse>;