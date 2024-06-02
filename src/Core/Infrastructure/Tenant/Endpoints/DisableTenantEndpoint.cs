using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using ROC.Core.Infrastructure.Auth.Policy;
using ROC.WebApi.Core.Tenant.Handlers.DisableTenant;

namespace ROC.Core.Infrastructure.Tenant.Endpoints;

public static class DisableTenantEndpoint
{
    internal static RouteHandlerBuilder MapDisableTenantEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id}/deactivate",
                (ISender mediator, string id) => mediator.Send(new DisableTenantCommand(id)))
            .WithName(nameof(DisableTenantEndpoint))
            .WithSummary("activate tenant")
            .RequirePermission("Permissions.Tenants.Update")
            .WithDescription("activate tenant");
    }
}