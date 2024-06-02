
Navigate to `./api/server` and run the following EF CLI commands.

```bash
dotnet ef migrations add "Add Identity Schema" --project .././migrations/postgresql/ --context IdentityDbContext -o Identity
dotnet ef migrations add "Add Tenant Schema" --project .././migrations/postgresql/ --context TenantDbContext -o Tenant
dotnet ef migrations add "Add Todo Schema" --project .././migrations/postgresql/ --context TodoDbContext -o Todo
dotnet ef migrations add "Add Catalog Schema" --project .././migrations/postgresql/ --context CatalogDbContext -o Catalog
```

```bash
dotnet ef database update --context IdentityDbContext
dotnet ef database update --context TenantDbContext 
dotnet ef database update --context TodoDbContext   
dotnet ef database update --context CatalogDbContext

```
