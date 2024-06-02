namespace ROC.WebApi.Core.Tenant;

public static class TenantConstants
{
    public const string DefaultPassword = "123Pa$$word!";

    public const string Identifier = "tenant";

    public static class Root
    {
        public const string Id = "root";
        public const string Name = "Root";
        public const string EmailAddress = "admin@root.com";
    }
}