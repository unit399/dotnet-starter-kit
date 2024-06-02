using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using ROC.Core.Infrastructure.Auth;
using ROC.Core.Infrastructure.Identity.Persistence;
using ROC.Core.Infrastructure.Identity.Roles;
using ROC.Core.Infrastructure.Identity.Tokens;
using ROC.Core.Infrastructure.Identity.Tokens.Endpoints;
using ROC.Core.Infrastructure.Identity.Users;
using ROC.Core.Infrastructure.Identity.Users.Endpoints;
using ROC.Core.Infrastructure.Identity.Users.Services;
using ROC.Core.Infrastructure.Persistence;
using ROC.WebApi.Core.Identity.Tokens;
using ROC.WebApi.Core.Identity.Users.Interfaces;
using ROC.WebApi.Core.Persistence;

namespace ROC.Core.Infrastructure.Identity;

internal static class Extensions
{
    internal static IServiceCollection ConfigureIdentity(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);
        services.AddScoped<CurrentUserMiddleware>();
        services.AddScoped<ICurrentUser, CurrentUser>();
        services.AddScoped(sp => (ICurrentUserInitializer)sp.GetRequiredService<ICurrentUser>());
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<ITokenService, TokenService>();
        services.BindDbContext<IdentityDbContext>();
        services.AddScoped<IDbInitializer, IdentityDbInitializer>();
        return services
            .AddIdentity<RocUser, RocRole>(options =>
            {
                options.Password.RequiredLength = IdentityConstants.PasswordLength;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<IdentityDbContext>()
            .AddDefaultTokenProviders()
            .Services;
    }

    public static IEndpointRouteBuilder MapIdentityEndpoints(this IEndpointRouteBuilder app)
    {
        var users = app.MapGroup("api/users").WithTags("users");
        users.MapUserEndpoints();

        var tokens = app.MapGroup("api/token").WithTags("token");
        tokens.MapTokenEndpoints();
        return app;
    }
}