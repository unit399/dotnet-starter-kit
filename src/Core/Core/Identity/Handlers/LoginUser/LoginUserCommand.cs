using MediatR;

namespace ROC.WebApi.Core.Identity.Handlers.LoginUser;

public class LoginUserCommand : IRequest<LoginUserResponse>
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
}