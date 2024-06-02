using Microsoft.AspNetCore.Routing;

namespace ROC.Core.Infrastructure.Identity.Tokens.Endpoints;

internal static class Extensions
{
    public static IEndpointRouteBuilder MapTokenEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapTokenGenerationEndpoint();
        return app;
    }
}