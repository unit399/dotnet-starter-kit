using System.Collections.ObjectModel;

namespace ROC.WebApi.Shared.Authorization;

public static class RocRoles
{
    public const string Admin = nameof(Admin);
    public const string Basic = nameof(Basic);

    public static IReadOnlyList<string> DefaultRoles { get; } = new ReadOnlyCollection<string>(new[]
    {
        Admin,
        Basic
    });

    public static bool IsDefault(string roleName)
    {
        return DefaultRoles.Any(r => r == roleName);
    }
}