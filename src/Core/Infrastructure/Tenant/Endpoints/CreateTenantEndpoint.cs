using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using ROC.Core.Infrastructure.Auth.Policy;
using ROC.WebApi.Core.Tenant.Handlers.CreateTenant;

namespace ROC.Core.Infrastructure.Tenant.Endpoints;

public static class CreateTenantEndpoint
{
    internal static RouteHandlerBuilder MapRegisterTenantEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", (CreateTenantCommand request, ISender mediator) => mediator.Send(request))
            .WithName(nameof(CreateTenantEndpoint))
            .WithSummary("creates a tenant")
            .RequirePermission("Permissions.Tenants.Create")
            .WithDescription("creates a tenant");
    }
}