using System.Threading;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using ROC.Core.Infrastructure.Auth.Policy;
using ROC.WebApi.Core.Identity.Handlers.LoginUser;
using ROC.WebApi.Core.Identity.Users.Interfaces;

namespace ROC.Core.Infrastructure.Identity.Users.Endpoints;

public static class LoginUserEndpoint
{
    internal static RouteHandlerBuilder MapLoginUserEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/login", (LoginUserCommand request,
                IUserService service,
                HttpContext context,
                CancellationToken cancellationToken) =>
            {
                var origin = $"{context.Request.Scheme}://{context.Request.Host.Value}{context.Request.PathBase.Value}";
                return service.LoginAsync(request, origin, cancellationToken);
            })
            .WithName(nameof(LoginUserEndpoint))
            .WithSummary("login user")
            .RequirePermission("Permissions.Users.Login")
            .WithDescription("login user");
    }
}