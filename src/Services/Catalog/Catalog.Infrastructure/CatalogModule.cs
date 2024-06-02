using Carter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using ROC.Core.Infrastructure.Persistence;
using ROC.WebApi.Catalog.Domain;
using ROC.WebApi.Catalog.Infrastructure.Endpoints.v1;
using ROC.WebApi.Catalog.Infrastructure.Persistence;
using ROC.WebApi.Core.Persistence;

namespace ROC.WebApi.Catalog.Infrastructure;

public static class CatalogModule
{
    public static WebApplicationBuilder RegisterCatalogServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.BindDbContext<CatalogDbContext>();
        builder.Services.AddScoped<IDbInitializer, CatalogDbInitializer>();
        builder.Services.AddKeyedScoped<IRepository<Product>, CatalogRepository<Product>>("catalog:products");
        builder.Services.AddKeyedScoped<IReadRepository<Product>, CatalogRepository<Product>>("catalog:products");
        return builder;
    }

    public static WebApplication UseCatalogModule(this WebApplication app)
    {
        return app;
    }

    public class Endpoints : CarterModule
    {
        public Endpoints() : base("catalog")
        {
        }

        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            var productGroup = app.MapGroup("products").WithTags("products");
            productGroup.MapProductCreationEndpoint();
        }
    }
}