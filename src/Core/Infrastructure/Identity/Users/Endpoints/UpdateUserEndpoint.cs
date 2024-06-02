﻿using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using ROC.Core.Infrastructure.Auth.Policy;
using ROC.WebApi.Core.Exceptions;
using ROC.WebApi.Core.Identity.Handlers.UpdateUser;
using ROC.WebApi.Core.Identity.Users.Interfaces;

namespace ROC.Core.Infrastructure.Identity.Users.Endpoints;

public static class UpdateUserEndpoint
{
    internal static RouteHandlerBuilder MapUpdateUserEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/profile",
                (UpdateUserCommand request, ISender mediator, ClaimsPrincipal user, IUserService service) =>
                {
                    if (user.GetUserId() is not { } userId || string.IsNullOrEmpty(userId))
                        throw new UnauthorizedException();
                    return service.UpdateAsync(request, userId);
                })
            .WithName(nameof(UpdateUserEndpoint))
            .WithSummary("update user profile")
            .RequirePermission("Permissions.Users.Update")
            .WithDescription("update user profile");
    }
}