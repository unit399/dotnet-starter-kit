using System.Security.Claims;

namespace ROC.WebApi.Core.Identity.Users.Interfaces;

public interface ICurrentUserInitializer
{
    void SetCurrentUser(ClaimsPrincipal user);

    void SetCurrentUserId(string userId);
}