using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using ROC.WebApi.Core.Identity.Handlers.LoginUser;
using ROC.WebApi.Core.Identity.Handlers.RegisterUser;
using ROC.WebApi.Core.Identity.Handlers.ToggleUserStatus;
using ROC.WebApi.Core.Identity.Handlers.UpdateUser;
using ROC.WebApi.Core.Identity.Users.Entities;

namespace ROC.WebApi.Core.Identity.Users.Interfaces;

public interface IUserService
{
    Task<bool> ExistsWithNameAsync(string name);
    Task<bool> ExistsWithEmailAsync(string email, string? exceptId = null);
    Task<bool> ExistsWithPhoneNumberAsync(string phoneNumber, string? exceptId = null);
    Task<List<UserDetail>> GetListAsync(CancellationToken cancellationToken);
    Task<int> GetCountAsync(CancellationToken cancellationToken);
    Task<UserDetail> GetAsync(string userId, CancellationToken cancellationToken);
    Task ToggleStatusAsync(ToggleUserStatusCommand request, CancellationToken cancellationToken);
    Task<string> GetOrCreateFromPrincipalAsync(ClaimsPrincipal principal);

    Task<RegisterUserResponse> RegisterAsync(RegisterUserCommand request, string origin,
        CancellationToken cancellationToken);

    Task<LoginUserResponse> LoginAsync(LoginUserCommand request, string origin, CancellationToken cancellationToken);

    Task UpdateAsync(UpdateUserCommand request, string userId);
    Task<string> ConfirmEmailAsync(string userId, string code, string tenant, CancellationToken cancellationToken);
    Task<string> ConfirmPhoneNumberAsync(string userId, string code);

    // permisions
    Task<bool> HasPermissionAsync(string userId, string permission, CancellationToken cancellationToken = default);
}