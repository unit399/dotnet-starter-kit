namespace ROC.WebApi.Core.Identity.Handlers.ChangePassword;

public class ChangePasswordCommand
{
    public string Password { get; set; } = default!;
    public string NewPassword { get; set; } = default!;
    public string ConfirmNewPassword { get; set; } = default!;
}