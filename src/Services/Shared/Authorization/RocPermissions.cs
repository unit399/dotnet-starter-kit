using System.Collections.ObjectModel;

namespace ROC.WebApi.Shared.Authorization;

public static class RocAction
{
    public const string View = nameof(View);
    public const string Search = nameof(Search);
    public const string Create = nameof(Create);
    public const string Update = nameof(Update);
    public const string Delete = nameof(Delete);
    public const string Export = nameof(Export);
    public const string Generate = nameof(Generate);
    public const string Clean = nameof(Clean);
    public const string UpgradeSubscription = nameof(UpgradeSubscription);
}

public static class RocResource
{
    public const string Tenants = nameof(Tenants);
    public const string Dashboard = nameof(Dashboard);
    public const string Hangfire = nameof(Hangfire);
    public const string Users = nameof(Users);
    public const string UserRoles = nameof(UserRoles);
    public const string Roles = nameof(Roles);
    public const string RoleClaims = nameof(RoleClaims);
    public const string Products = nameof(Products);
    public const string Todos = nameof(Todos);
}

public static class RocPermissions
{
    private static readonly RocPermission[] allPermissions =
    {
        //tenants
        new("View Tenants", RocAction.View, RocResource.Tenants, IsRoot: true),
        new("Create Tenants", RocAction.Create, RocResource.Tenants, IsRoot: true),
        new("Update Tenants", RocAction.Update, RocResource.Tenants, IsRoot: true),
        new("Upgrade Tenant Subscription", RocAction.UpgradeSubscription, RocResource.Tenants, IsRoot: true),

        //identity
        new("View Users", RocAction.View, RocResource.Users),
        new("Search Users", RocAction.Search, RocResource.Users),
        new("Create Users", RocAction.Create, RocResource.Users),
        new("Update Users", RocAction.Update, RocResource.Users),
        new("Delete Users", RocAction.Delete, RocResource.Users),
        new("Export Users", RocAction.Export, RocResource.Users),
        new("View UserRoles", RocAction.View, RocResource.UserRoles),
        new("Update UserRoles", RocAction.Update, RocResource.UserRoles),
        new("View Roles", RocAction.View, RocResource.Roles),
        new("Create Roles", RocAction.Create, RocResource.Roles),
        new("Update Roles", RocAction.Update, RocResource.Roles),
        new("Delete Roles", RocAction.Delete, RocResource.Roles),
        new("View RoleClaims", RocAction.View, RocResource.RoleClaims),
        new("Update RoleClaims", RocAction.Update, RocResource.RoleClaims),

        //products
        new("View Products", RocAction.View, RocResource.Products, true),
        new("Search Products", RocAction.Search, RocResource.Products, true),
        new("Create Products", RocAction.Create, RocResource.Products),
        new("Update Products", RocAction.Update, RocResource.Products),
        new("Delete Products", RocAction.Delete, RocResource.Products),
        new("Export Products", RocAction.Export, RocResource.Products),

        //todos
        new("View Todos", RocAction.View, RocResource.Todos, true),
        new("Search Todos", RocAction.Search, RocResource.Todos, true),
        new("Create Todos", RocAction.Create, RocResource.Todos),
        new("Update Todos", RocAction.Update, RocResource.Todos),
        new("Delete Todos", RocAction.Delete, RocResource.Todos)
    };

    public static IReadOnlyList<RocPermission> All { get; } = new ReadOnlyCollection<RocPermission>(allPermissions);

    public static IReadOnlyList<RocPermission> Root { get; } =
        new ReadOnlyCollection<RocPermission>(allPermissions.Where(p => p.IsRoot).ToArray());

    public static IReadOnlyList<RocPermission> Admin { get; } =
        new ReadOnlyCollection<RocPermission>(allPermissions.Where(p => !p.IsRoot).ToArray());

    public static IReadOnlyList<RocPermission> Basic { get; } =
        new ReadOnlyCollection<RocPermission>(allPermissions.Where(p => p.IsBasic).ToArray());
}

public record RocPermission(
    string Description,
    string Action,
    string Resource,
    bool IsBasic = false,
    bool IsRoot = false)
{
    public string Name => NameFor(Action, Resource);

    public static string NameFor(string action, string resource)
    {
        return $"Permissions.{resource}.{action}";
    }
}