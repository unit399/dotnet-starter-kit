using System.Collections.ObjectModel;
using ROC.WebApi.Core.Identity.Users.Entities;

namespace ROC.WebApi.Core.Identity.Handlers.AssignUserRole;

public class AssignUserRoleCommand
{
    public Collection<UserRoleDetail> UserRoles { get; } = new();
}