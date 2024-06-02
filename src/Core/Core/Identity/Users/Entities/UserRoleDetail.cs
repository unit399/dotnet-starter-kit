namespace ROC.WebApi.Core.Identity.Users.Entities;

public class UserRoleDetail
{
    public string? RoleId { get; set; }
    public string? RoleName { get; set; }
    public string? Description { get; set; }
    public bool Enabled { get; set; }
}