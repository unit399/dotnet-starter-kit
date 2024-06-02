﻿using Asp.Versioning.Conventions;
using Carter;
using FluentValidation;
using ROC.WebApi.Todo;

namespace Server;

public static class Installer
{
    public static WebApplicationBuilder RegisterModules(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        //define module assemblies
        var assemblies = new[]
        {
            typeof(TodoModule).Assembly
        };

        //register validators
        builder.Services.AddValidatorsFromAssemblies(assemblies);

        //register mediatr
        builder.Services.AddMediatR(cfg => { cfg.RegisterServicesFromAssemblies(assemblies); });

        //register module services
        builder.RegisterTodoServices();

        //add carter endpoint modules
        builder.Services.AddCarter(configurator: config => { config.WithModule<TodoModule.Endpoints>(); });

        return builder;
    }

    public static WebApplication UseModules(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        //register modules
        app.UseTodoModule();

        //register api versions
        var versions = app.NewApiVersionSet()
            .HasApiVersion(1)
            .HasApiVersion(2)
            .ReportApiVersions()
            .Build();

        //map versioned endpoint
        var endpoints = app.MapGroup("api/v{version:apiVersion}").WithApiVersionSet(versions);

        //use carter
        endpoints.MapCarter();

        return app;
    }
}