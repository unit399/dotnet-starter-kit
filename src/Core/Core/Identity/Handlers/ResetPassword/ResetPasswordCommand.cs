namespace ROC.WebApi.Core.Identity.Handlers.ResetPassword;

public class ResetPasswordCommand
{
    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? Token { get; set; }
}