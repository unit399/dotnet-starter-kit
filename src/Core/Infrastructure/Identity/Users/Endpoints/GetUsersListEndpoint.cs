﻿using System.Threading;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using ROC.Core.Infrastructure.Auth.Policy;
using ROC.WebApi.Core.Identity.Users.Interfaces;

namespace ROC.Core.Infrastructure.Identity.Users.Endpoints;

public static class GetUsersListEndpoint
{
    internal static RouteHandlerBuilder MapGetUsersListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/",
                (CancellationToken cancellationToken, IUserService service) =>
                {
                    return service.GetListAsync(cancellationToken);
                })
            .WithName(nameof(GetUsersListEndpoint))
            .WithSummary("get users list")
            .RequirePermission("Permissions.Users.View")
            .WithDescription("get users list");
    }
}