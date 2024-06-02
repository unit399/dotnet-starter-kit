using System;
using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ROC.Core.Infrastructure.Auth;
using ROC.Core.Infrastructure.Auth.Jwt;
using ROC.Core.Infrastructure.Identity;
using ROC.Core.Infrastructure.Jobs;
using ROC.Core.Infrastructure.Mail;
using ROC.Core.Infrastructure.Persistence;
using ROC.Core.Infrastructure.Tenant.Endpoints;
using ROC.WebApi.Core;

namespace ROC.Core.Infrastructure;

public static class Extensions
{
    public static WebApplicationBuilder RegisterFshFramework(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.ConfigureDatabase();
        builder.Services.ConfigureIdentity();
        builder.Services.ConfigureJwtAuth();
        builder.Services.ConfigureJobs(builder.Configuration);
        builder.Services.ConfigureMailing();
        builder.Services.AddProblemDetails();
        
        //define module assemblies
        var assemblies = new Assembly[]
        {
            typeof(RocCoreAssemblyInfo).Assembly
        };

        //register validators
        builder.Services.AddValidatorsFromAssemblies(assemblies);

        //register mediatr
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(assemblies);
        });
        
        return builder;
    }
    
    public static WebApplication UseFshFramework(this WebApplication app)
    {
        app.UseHttpsRedirection();
        app.UseExceptionHandler();
        app.UseJobDashboard(app.Configuration);
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapTenantEndpoints();
        app.MapIdentityEndpoints();

        //current user middleware
        app.UseMiddleware<CurrentUserMiddleware>();

        return app;
    }
}