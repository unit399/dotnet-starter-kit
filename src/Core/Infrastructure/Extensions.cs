using System;
using Asp.Versioning.Conventions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ROC.Core.Infrastructure.Auth;
using ROC.Core.Infrastructure.Auth.Jwt;
using ROC.Core.Infrastructure.Behaviours;
using ROC.Core.Infrastructure.Cache;
using ROC.Core.Infrastructure.Exceptions;
using ROC.Core.Infrastructure.Identity;
using ROC.Core.Infrastructure.Jobs;
using ROC.Core.Infrastructure.Logging.Serilog;
using ROC.Core.Infrastructure.Mail;
using ROC.Core.Infrastructure.OpenApi;
using ROC.Core.Infrastructure.Persistence;
using ROC.Core.Infrastructure.Tenant;
using ROC.Core.Infrastructure.Tenant.Endpoints;
using ROC.WebApi.Core;

namespace ROC.Core.Infrastructure;

public static class Extensions
{
    public static WebApplicationBuilder RegisterRocFramework(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.ConfigureSerilog();
        builder.ConfigureDatabase();
        builder.Services.ConfigureMultitenancy();
        builder.Services.ConfigureIdentity();
        builder.Services.ConfigureJwtAuth();
        builder.Services.ConfigureOpenApi();
        builder.Services.ConfigureJobs(builder.Configuration);
        builder.Services.ConfigureMailing();
        builder.Services.ConfigureCaching(builder.Configuration);
        builder.Services.AddExceptionHandler<RocExceptionHandler>();
        builder.Services.AddProblemDetails();

        //define module assemblies
        var assemblies = new[]
        {
            typeof(RocCoreAssemblyInfo).Assembly
        };

        //register validators
        builder.Services.AddValidatorsFromAssemblies(assemblies);

        //register mediatr
        builder.Services.AddMediatR(cfg =>
        {
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            cfg.RegisterServicesFromAssemblies(assemblies);
        });

        return builder;
    }

    public static WebApplication UseRocFramework(this WebApplication app)
    {
        app.UseHttpsRedirection();
        app.UseMultitenancy();
        app.UseExceptionHandler();
        app.UseOpenApi();
        app.UseJobDashboard(app.Configuration);
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapTenantEndpoints();
        app.MapIdentityEndpoints();

        //current user middleware
        app.UseMiddleware<CurrentUserMiddleware>();

        //register api versions
        var versions = app.NewApiVersionSet()
            .HasApiVersion(1)
            .HasApiVersion(2)
            .ReportApiVersions()
            .Build();

        //map versioned endpoint
        app.MapGroup("api/v{version:apiVersion}").WithApiVersionSet(versions);

        return app;
    }
}