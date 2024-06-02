using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ROC.WebApi.Core.Identity.Users.Interfaces;

namespace ROC.Core.Infrastructure.Auth;

public class CurrentUserMiddleware(ICurrentUserInitializer currentUserInitializer) : IMiddleware
{
    private readonly ICurrentUserInitializer _currentUserInitializer = currentUserInitializer;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        _currentUserInitializer.SetCurrentUser(context.User);
        await next(context);
    }
}