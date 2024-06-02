using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ROC.Core.Infrastructure.Persistence.Interceptors;
using ROC.WebApi.Core.Persistence;
using Serilog;

namespace ROC.Core.Infrastructure.Persistence;

public static class Extensions
{
    private static readonly ILogger _logger = Log.ForContext(typeof(Extensions));

    internal static DbContextOptionsBuilder ConfigureDatabase(this DbContextOptionsBuilder builder, string dbProvider,
        string connectionString)
    {
        return dbProvider.ToUpperInvariant() switch
        {
            DbProviders.PostgreSQL => builder.UseNpgsql(connectionString, e =>
                e.MigrationsAssembly("ROC.WebApi.Migrations.PostgreSQL")),
            _ => throw new InvalidOperationException($"DB Provider {dbProvider} is not supported.")
        };
    }

    public static WebApplicationBuilder ConfigureDatabase(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.AddOptions<DatabaseOptions>()
            .BindConfiguration(nameof(DatabaseOptions))
            .ValidateDataAnnotations()
            .PostConfigure(config =>
            {
                _logger.Information("current db provider: {DatabaseProvider}", config.Provider);
            });
        builder.Services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        return builder;
    }

    public static IServiceCollection BindDbContext<TContext>(this IServiceCollection services)
        where TContext : DbContext
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddDbContext<TContext>((sp, options) =>
        {
            var dbConfig = sp.GetRequiredService<IOptions<DatabaseOptions>>().Value;
            options.ConfigureDatabase(dbConfig.Provider, dbConfig.ConnectionString);
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
        });
        return services;
    }
}