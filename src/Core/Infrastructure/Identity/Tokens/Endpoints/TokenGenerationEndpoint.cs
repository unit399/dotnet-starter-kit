﻿using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ROC.WebApi.Core.Identity.Tokens;
using ROC.WebApi.Core.Identity.Tokens.Features.Generate;
using ROC.WebApi.Core.Tenant;

namespace ROC.Core.Infrastructure.Identity.Tokens.Endpoints;

public static class TokenGenerationEndpoint
{
    internal static RouteHandlerBuilder MapTokenGenerationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", (TokenGenerationCommand request,
                [FromHeader(Name = TenantConstants.Identifier)]
                string tenant,
                ITokenService service,
                HttpContext context,
                CancellationToken cancellationToken) =>
            {
                var ip = "N/A";
                if (context.Request.Headers.TryGetValue("X-Forwarded-For", out var ipList))
                    ip = ipList.FirstOrDefault() ?? "N/A";
                else if (context.Connection.RemoteIpAddress != null)
                    ip = context.Connection.RemoteIpAddress.MapToIPv4().ToString();
                return service.GenerateTokenAsync(request, ip!, cancellationToken);
            })
            .WithName(nameof(TokenGenerationEndpoint))
            .WithSummary("generate JWTs")
            .WithDescription("generate JWTs")
            .AllowAnonymous();
    }
}