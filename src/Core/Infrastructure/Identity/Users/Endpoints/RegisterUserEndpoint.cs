using System.Threading;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using ROC.Core.Infrastructure.Auth.Policy;
using ROC.WebApi.Core.Identity.Handlers.RegisterUser;
using ROC.WebApi.Core.Identity.Users.Interfaces;

namespace ROC.Core.Infrastructure.Identity.Users.Endpoints;

public static class RegisterUserEndpoint
{
    internal static RouteHandlerBuilder MapRegisterUserEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/register", (RegisterUserCommand request,
                IUserService service,
                HttpContext context,
                CancellationToken cancellationToken) =>
            {
                var origin = $"{context.Request.Scheme}://{context.Request.Host.Value}{context.Request.PathBase.Value}";
                return service.RegisterAsync(request, origin, cancellationToken);
            })
            .WithName(nameof(RegisterUserEndpoint))
            .WithSummary("register user")
            .RequirePermission("Permissions.Users.Create")
            .WithDescription("register user");
    }
}