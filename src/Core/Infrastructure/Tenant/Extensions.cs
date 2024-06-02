using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using Finbuckle.MultiTenant.Stores.DistributedCacheStore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ROC.Core.Infrastructure.Persistence;
using ROC.Core.Infrastructure.Persistence.Services;
using ROC.Core.Infrastructure.Tenant.Persistence;
using ROC.Core.Infrastructure.Tenant.Services;
using ROC.WebApi.Core.Persistence;
using ROC.WebApi.Core.Tenant;
using ROC.WebApi.Core.Tenant.Interfaces;
using ROC.WebApi.Shared.Authorization;
using Serilog;

namespace ROC.Core.Infrastructure.Tenant;

internal static class Extensions
{
    public static IServiceCollection ConfigureMultitenancy(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);
        services.AddTransient<IConnectionStringValidator, ConnectionStringValidator>();
        services.BindDbContext<TenantDbContext>();
        services
            .AddMultiTenant<RocTenantInfo>(config =>
            {
                // to save database calls to resolve tenant
                // this was happening for every request earlier, leading to ineffeciency
                config.Events.OnTenantResolved = async context =>
                {
                    if (context.StoreType != typeof(DistributedCacheStore<RocTenantInfo>))
                    {
                        var sp = ((HttpContext)context.Context!).RequestServices;
                        var distributedCacheStore = sp
                            .GetService<IEnumerable<IMultiTenantStore<RocTenantInfo>>>()!
                            .FirstOrDefault(s => s.GetType() == typeof(DistributedCacheStore<RocTenantInfo>));

                        await distributedCacheStore!.TryAddAsync((RocTenantInfo)context.TenantInfo!);
                    }

                    await Task.FromResult(0);
                };
            })
            .WithClaimStrategy(RocClaims.Tenant)
            .WithHeaderStrategy(TenantConstants.Identifier)
            .WithDistributedCacheStore(TimeSpan.FromMinutes(60))
            .WithEFCoreStore<TenantDbContext, RocTenantInfo>();
        services.AddScoped<ITenantService, TenantService>();
        return services;
    }

    public static WebApplication UseMultitenancy(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);
        app.UseMultiTenant();

        // set up tenant store
        var tenants = TenantStoreSetup(app);

        // set up tenant databases
        app.SetupTenantDatabases(tenants);

        return app;
    }

    private static IApplicationBuilder SetupTenantDatabases(this IApplicationBuilder app,
        IEnumerable<RocTenantInfo> tenants)
    {
        foreach (var tenant in tenants)
        {
            // create a scope for tenant
            using var tenantScope = app.ApplicationServices.CreateScope();

            //set current tenant so that the right connection string is used
            tenantScope.ServiceProvider.GetRequiredService<IMultiTenantContextSetter>()
                .MultiTenantContext = new MultiTenantContext<RocTenantInfo>
            {
                TenantInfo = tenant
            };

            // using the scope, perform migrations / seeding
            var initializers = tenantScope.ServiceProvider.GetServices<IDbInitializer>();
            foreach (var initializer in initializers)
            {
                initializer.MigrateAsync(CancellationToken.None).Wait();
                initializer.SeedAsync(CancellationToken.None).Wait();
            }
        }

        return app;
    }

    private static IEnumerable<RocTenantInfo> TenantStoreSetup(IApplicationBuilder app)
    {
        var scope = app.ApplicationServices.CreateScope();

        // tenant master schema migration
        var tenantDbContext = scope.ServiceProvider.GetRequiredService<TenantDbContext>();
        if (tenantDbContext.Database.GetPendingMigrations().Any())
        {
            tenantDbContext.Database.Migrate();
            Log.Information("applied database migrations for tenant module");
        }

        // default tenant seeding
        if (tenantDbContext.TenantInfo.Find(TenantConstants.Root.Id) is null)
        {
            var rootTenant = new RocTenantInfo(
                TenantConstants.Root.Id,
                TenantConstants.Root.Name,
                string.Empty,
                TenantConstants.Root.EmailAddress);

            rootTenant.SetValidity(DateTime.UtcNow.AddYears(1));
            tenantDbContext.TenantInfo.Add(rootTenant);
            tenantDbContext.SaveChanges();
            Log.Information("configured default tenant data");
        }

        // get all tenants from store
        var tenantStore = scope.ServiceProvider.GetRequiredService<IMultiTenantStore<RocTenantInfo>>();
        var tenants = tenantStore.GetAllAsync().Result;

        //dispose scope
        scope.Dispose();

        return tenants;
    }
}