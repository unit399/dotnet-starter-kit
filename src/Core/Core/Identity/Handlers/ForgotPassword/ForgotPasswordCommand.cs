namespace ROC.WebApi.Core.Identity.Handlers.ForgotPassword;

public class ForgotPasswordCommand
{
    public string Email { get; set; } = default!;
}