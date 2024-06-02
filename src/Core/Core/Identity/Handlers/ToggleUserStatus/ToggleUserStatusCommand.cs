namespace ROC.WebApi.Core.Identity.Handlers.ToggleUserStatus;

public class ToggleUserStatusCommand
{
    public bool ActivateUser { get; set; }
    public string? UserId { get; set; }
}