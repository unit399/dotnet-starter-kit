using MediatR;

namespace ROC.WebApi.Core.Tenant.Handlers.CreateTenant;

public sealed record CreateTenantCommand(
    string Id,
    string Name,
    string? ConnectionString,
    string AdminEmail,
    string? Issuer) : IRequest<CreateTenantResponse>;